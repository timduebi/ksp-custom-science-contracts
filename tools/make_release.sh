#!/usr/bin/env bash
# Build and package CustomScienceContracts as ONE release with config overlays.
#
#   tools/make_release.sh             # build all assets locally into compiled/release-vX/
#   tools/make_release.sh --publish   # also create/update the single GitHub release
#
# One release, three downloads (named so the main download sorts to the top of the asset list):
#   1. CustomScienceContracts-vX.zip                  full mod, default SOL catalog
#   2. CustomScienceContracts-vX_Sol-German-Config.zip optional: swap SOL catalog to German
#   3. CustomScienceContracts-vX_Stock-Config.zip     optional: swap catalog to stock KSP
# The optional packs only replace GameData/CustomScienceContracts/Contracts/*.cfg; the shared
# engine DLL already supports both the real-solar-system and stock bodies. See DEVELOPMENT.md.
set -euo pipefail

PUBLISH="${1:-}"
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
REPO="timduebi/ksp-custom-science-contracts"
KSP="${KSP:-$HOME/Library/Application Support/Steam/steamapps/common/Kerbal Space Program}"
MANAGED="${KSPManaged:-$KSP/KSP.app/Contents/Resources/Data/Managed}"
# Prefer dotnet/python3 from PATH; DOTNET/PYTHON env vars override.
DOTNET="${DOTNET:-$(command -v dotnet || echo /usr/local/share/dotnet/dotnet)}"
PYTHON="${PYTHON:-python3}"
export DOTNET_CLI_TELEMETRY_OPTOUT=1

# --- Version (single source of truth) -------------------------------------------------
VERSION="$(sed -n 's/.*public const string Version = "\([^"]*\)".*/\1/p' \
  "$ROOT/src/CustomScienceContracts/Core/ModInfo.cs")"
[ -n "$VERSION" ] || { echo "could not read Version from ModInfo.cs"; exit 1; }
OUTDIR="$ROOT/compiled/release-v$VERSION"
mkdir -p "$OUTDIR"
echo "==> CustomScienceContracts v$VERSION (single release + config overlays)"

# --- Validate every catalog that will ship -------------------------------------------
echo "==> Validate"
"$PYTHON" "$ROOT/tools/validate_design.py" >/dev/null
"$PYTHON" "$ROOT/tools/validate_design.py" "$ROOT/custom_science_contracts_stock_missionsdesign.md" >/dev/null
"$PYTHON" "$ROOT/tools/validate_catalog.py" "$ROOT/GameData/CustomScienceContracts/Contracts" sol >/dev/null
"$PYTHON" "$ROOT/tools/validate_catalog.py" "$ROOT/OptionalConfigs/Stock/GameData/CustomScienceContracts/Contracts" stock >/dev/null
"$PYTHON" "$ROOT/tools/validate_catalog.py" "$ROOT/OptionalConfigs/SOL-German/GameData/CustomScienceContracts/Contracts" sol >/dev/null
echo "    validators OK"

# --- Build the shared engine DLL once -------------------------------------------------
echo "==> Build (Release) against: $MANAGED"
"$DOTNET" build -c Release -p:KSPManaged="$MANAGED" "$ROOT/CustomScienceContracts.sln" | tail -3
DLL="$ROOT/GameData/CustomScienceContracts/Plugins/CustomScienceContracts.dll"
[ -f "$DLL" ] || { echo "DLL missing after build"; exit 1; }

# zip if available (macOS/Linux), otherwise Python's zipfile (Windows Git Bash has no zip).
zipdir() {
  if command -v zip >/dev/null 2>&1; then
    ( cd "$1" && rm -f "$2" && zip -rq "$2" . -x '*.DS_Store' )
  else
    rm -f "$2"
    "$PYTHON" - "$1" "$2" <<'PYZIP'
import os, sys, zipfile
src, dst = sys.argv[1], sys.argv[2]
with zipfile.ZipFile(dst, "w", zipfile.ZIP_DEFLATED) as z:
    for root, _, files in os.walk(src):
        for f in files:
            if f == ".DS_Store":
                continue
            p = os.path.join(root, f)
            z.write(p, os.path.relpath(p, src))
PYZIP
  fi
}

# --- 1. Main download: full mod, default SOL catalog ----------------------------------
MAIN="$OUTDIR/main"; rm -rf "$MAIN"; mkdir -p "$MAIN"
cp -R "$ROOT/GameData" "$MAIN/"
find "$MAIN/GameData" -name '*.zip' -delete
# Player download: only the mod plus README and license files. No Logo/CHANGELOG/DEVELOPMENT/DOKUMENTATION (repo/dev docs).
cp "$ROOT/README.md" "$ROOT/LICENSE" "$ROOT/THIRD_PARTY_NOTICES.md" "$MAIN/"
cp -R "$ROOT/LICENSES" "$MAIN/"
MAINZIP="$OUTDIR/CustomScienceContracts-v$VERSION.zip"; zipdir "$MAIN" "$MAINZIP"

# --- 2. Stock config overlay (Contracts cfgs only) ------------------------------------
STK="$OUTDIR/stock-config"; rm -rf "$STK"; mkdir -p "$STK/GameData/CustomScienceContracts/Contracts"
cp "$ROOT/OptionalConfigs/Stock/GameData/CustomScienceContracts/Contracts/"*.cfg "$STK/GameData/CustomScienceContracts/Contracts/"
cp "$ROOT/OptionalConfigs/Stock/README.md" "$STK/"
STOCKZIP="$OUTDIR/CustomScienceContracts-v${VERSION}_Stock-Config.zip"; zipdir "$STK" "$STOCKZIP"

# --- 3. German SOL config overlay (Contracts cfgs only) -------------------------------
GER="$OUTDIR/sol-german"; rm -rf "$GER"; mkdir -p "$GER"
cp -R "$ROOT/OptionalConfigs/SOL-German/GameData" "$GER/"
cp "$ROOT/OptionalConfigs/SOL-German/README.md" "$GER/"
GZIP="$OUTDIR/CustomScienceContracts-v${VERSION}_Sol-German-Config.zip"; zipdir "$GER" "$GZIP"

ASSETS=("$MAINZIP" "$STOCKZIP" "$GZIP")
echo "==> Packaged:"; for a in "${ASSETS[@]}"; do echo "    $(basename "$a") ($(du -h "$a" | cut -f1))"; done

[ "$PUBLISH" = "--publish" ] || { echo "==> Done (local only; pass --publish to release to GitHub)."; exit 0; }

# --- Publish: ONE release (tag vX) with all three assets -------------------------------
TOKEN="$(printf 'protocol=https\nhost=github.com\n\n' | git credential fill 2>/dev/null | sed -n 's/^password=//p')"
[ -n "$TOKEN" ] || { echo "no GitHub token in keychain"; exit 1; }
API="https://api.github.com/repos/$REPO"
TAG="v$VERSION"; RELNAME="KSP Custom Science Contracts v$VERSION"; SHA="$(git rev-parse HEAD)"
BODY="$(cat "$ROOT/tools/release_notes.md" 2>/dev/null || echo "CustomScienceContracts $VERSION")"

echo "==> Publish single release $TAG (stable)"
RID="$(curl -s -H "Authorization: token $TOKEN" "$API/releases/tags/$TAG" \
  | "$PYTHON" -c "import sys,json;d=json.load(sys.stdin);print(d.get('id',''))")"
# Stable release: plain name, not marked as prerelease.
PAYLOAD="$("$PYTHON" -c "import json,sys;print(json.dumps({'tag_name':sys.argv[1],'target_commitish':sys.argv[2],'name':sys.argv[3],'body':sys.stdin.read(),'make_latest':'true','draft':False,'prerelease':False}))" \
  "$TAG" "$SHA" "$RELNAME" <<<"$BODY")"
if [ -n "$RID" ]; then
  curl -s -X PATCH -H "Authorization: token $TOKEN" -H "Content-Type: application/json" --data "$PAYLOAD" "$API/releases/$RID" >/dev/null
else
  RID="$(curl -s -X POST -H "Authorization: token $TOKEN" -H "Content-Type: application/json" --data "$PAYLOAD" "$API/releases" | "$PYTHON" -c "import sys,json;print(json.load(sys.stdin)['id'])")"
fi
for stale in "CustomScienceContracts-v${VERSION}_German-Config.zip"; do
  old="$(curl -s -H "Authorization: token $TOKEN" "$API/releases/$RID/assets" | "$PYTHON" -c "import sys,json;[print(x['id']) for x in json.load(sys.stdin) if x['name']==sys.argv[1]]" "$stale")"
  [ -n "$old" ] && curl -s -X DELETE -H "Authorization: token $TOKEN" "$API/releases/assets/$old" >/dev/null
done
for a in "${ASSETS[@]}"; do
  name="$(basename "$a")"
  old="$(curl -s -H "Authorization: token $TOKEN" "$API/releases/$RID/assets" | "$PYTHON" -c "import sys,json;[print(x['id']) for x in json.load(sys.stdin) if x['name']==sys.argv[1]]" "$name")"
  [ -n "$old" ] && curl -s -X DELETE -H "Authorization: token $TOKEN" "$API/releases/assets/$old" >/dev/null
  curl -s -X POST -H "Authorization: token $TOKEN" -H "Content-Type: application/zip" --data-binary @"$a" \
    "https://uploads.github.com/repos/$REPO/releases/$RID/assets?name=$name" \
    | "$PYTHON" -c "import sys,json;d=json.load(sys.stdin);print('    uploaded',d['name'],d['state'])"
done
echo "==> Published: https://github.com/$REPO/releases/tag/$TAG"
