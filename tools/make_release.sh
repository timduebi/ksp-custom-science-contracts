#!/usr/bin/env bash
# Build and package one edition of CustomScienceContracts from this single branch.
#
#   tools/make_release.sh sol            # build the SOL edition zips (English + German config)
#   tools/make_release.sh stock          # build the Stock edition zip (English only)
#   tools/make_release.sh sol   --publish  # also create/update the GitHub release
#   tools/make_release.sh stock --publish
#
# Both editions share ONE engine (one DLL build), so an engine change reaches both editions
# simply by running this script twice. See DEVELOPMENT.md.
set -euo pipefail

EDITION="${1:-}"
PUBLISH="${2:-}"
case "$EDITION" in sol|stock) ;; *) echo "usage: $0 <sol|stock> [--publish]"; exit 2;; esac

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
REPO="timduebi/ksp-custom-science-contracts"
KSP="${KSP:-$HOME/Library/Application Support/Steam/steamapps/common/Kerbal Space Program}"
MANAGED="${KSPManaged:-$KSP/KSP.app/Contents/Resources/Data/Managed}"
DOTNET="${DOTNET:-/usr/local/share/dotnet/dotnet}"
export DOTNET_CLI_TELEMETRY_OPTOUT=1

# --- Version (single source of truth) -------------------------------------------------
VERSION="$(sed -n 's/.*public const string Version = "\([^"]*\)".*/\1/p' \
  "$ROOT/src/CustomScienceContracts/Core/ModInfo.cs")"
[ -n "$VERSION" ] || { echo "could not read Version from ModInfo.cs"; exit 1; }
OUTDIR="$ROOT/compiled/release-v$VERSION"
mkdir -p "$OUTDIR"
echo "==> CustomScienceContracts $EDITION v$VERSION"

# --- Validate the catalog this edition will ship --------------------------------------
echo "==> Validate"
if [ "$EDITION" = "sol" ]; then
  python3 "$ROOT/tools/validate_design.py" >/dev/null
  python3 "$ROOT/tools/validate_catalog.py" "$ROOT/GameData/CustomScienceContracts/Contracts" sol >/dev/null
  STOCK_CONTRACTS=""
else
  python3 "$ROOT/tools/validate_design.py" "$ROOT/custom_science_contracts_stock_missionsdesign.md" >/dev/null
  STOCK_CONTRACTS="$ROOT/OptionalConfigs/Stock/GameData/CustomScienceContracts/Contracts"
  python3 "$ROOT/tools/validate_catalog.py" "$STOCK_CONTRACTS" stock >/dev/null
fi
echo "    validators OK"

# --- Build the shared engine DLL once -------------------------------------------------
echo "==> Build (Release) against: $MANAGED"
"$DOTNET" build -c Release -p:KSPManaged="$MANAGED" "$ROOT/CustomScienceContracts.sln" \
  | tail -3
DLL="$ROOT/GameData/CustomScienceContracts/Plugins/CustomScienceContracts.dll"
[ -f "$DLL" ] || { echo "DLL missing after build"; exit 1; }

# --- Stage the main download ----------------------------------------------------------
stage_main() {
  local stage="$1" readme="$2"
  rm -rf "$stage"; mkdir -p "$stage"
  cp -R "$ROOT/GameData" "$stage/"
  cp "$readme" "$stage/README.md"
  cp "$ROOT/LICENSE" "$ROOT/THIRD_PARTY_NOTICES.md" "$ROOT/CHANGELOG.md" "$stage/"
  cp -R "$ROOT/LICENSES" "$stage/"
}

ASSETS=()
if [ "$EDITION" = "sol" ]; then
  STAGE="$OUTDIR/sol-main"
  stage_main "$STAGE" "$ROOT/README.md"
  cp "$ROOT/DOKUMENTATION.md" "$STAGE/"      # SOL ships the user documentation
  SOLZIP="$OUTDIR/CustomScienceContracts-SOL-v$VERSION.zip"
  ( cd "$STAGE" && rm -f "$SOLZIP" && zip -rq "$SOLZIP" . -x '*.DS_Store' )
  ASSETS+=("$SOLZIP")

  # German optional config: only the Contracts cfgs + a README.
  GSTAGE="$OUTDIR/sol-german"
  rm -rf "$GSTAGE"; mkdir -p "$GSTAGE"
  cp -R "$ROOT/OptionalConfigs/SOL-German/GameData" "$GSTAGE/"
  cp "$ROOT/OptionalConfigs/SOL-German/README.md" "$GSTAGE/"
  GZIP="$OUTDIR/CustomScienceContracts-SOL-GermanConfig-v$VERSION.zip"
  ( cd "$GSTAGE" && rm -f "$GZIP" && zip -rq "$GZIP" . -x '*.DS_Store' )
  ASSETS+=("$GZIP")

  TAG="v$VERSION"; RELNAME="KSP Custom Science Contracts – SOL v$VERSION"; MAKELATEST="true"
else
  STAGE="$OUTDIR/stock-main"
  stage_main "$STAGE" "$ROOT/OptionalConfigs/Stock/README.md"
  # Swap the default (SOL) catalog for the Stock catalog. No SOL-specific DOKUMENTATION.md.
  rm -f "$STAGE/GameData/CustomScienceContracts/Contracts/"*.cfg
  cp "$STOCK_CONTRACTS/"*.cfg "$STAGE/GameData/CustomScienceContracts/Contracts/"
  STOCKZIP="$OUTDIR/CustomScienceContracts-Stock-v$VERSION.zip"
  ( cd "$STAGE" && rm -f "$STOCKZIP" && zip -rq "$STOCKZIP" . -x '*.DS_Store' )
  ASSETS+=("$STOCKZIP")

  TAG="stock-v$VERSION"; RELNAME="KSP Custom Science Contracts – Stock v$VERSION"; MAKELATEST="false"
fi

echo "==> Packaged:"
for a in "${ASSETS[@]}"; do echo "    $(basename "$a") ($(du -h "$a" | cut -f1))"; done

[ "$PUBLISH" = "--publish" ] || { echo "==> Done (local only; pass --publish to release to GitHub)."; exit 0; }

# --- Publish to GitHub (create or update the release, upload assets) -------------------
TOKEN="$(printf 'protocol=https\nhost=github.com\n\n' | git credential fill 2>/dev/null | sed -n 's/^password=//p')"
[ -n "$TOKEN" ] || { echo "no GitHub token in keychain"; exit 1; }
API="https://api.github.com/repos/$REPO"
SHA="$(git rev-parse HEAD)"

echo "==> Publish release $TAG ($RELNAME)"
RID="$(curl -s -H "Authorization: token $TOKEN" "$API/releases/tags/$TAG" \
  | python3 -c "import sys,json;d=json.load(sys.stdin);print(d.get('id',''))")"

PAYLOAD="$(python3 -c "import json,sys; print(json.dumps({'tag_name':sys.argv[1],'target_commitish':sys.argv[2],'name':sys.argv[3],'make_latest':sys.argv[4]}))" \
  "$TAG" "$SHA" "$RELNAME" "$MAKELATEST")"
if [ -n "$RID" ]; then
  curl -s -X PATCH -H "Authorization: token $TOKEN" -H "Content-Type: application/json" \
    --data "$PAYLOAD" "$API/releases/$RID" >/dev/null
else
  RID="$(curl -s -X POST -H "Authorization: token $TOKEN" -H "Content-Type: application/json" \
    --data "$PAYLOAD" "$API/releases" | python3 -c "import sys,json;print(json.load(sys.stdin)['id'])")"
fi

for a in "${ASSETS[@]}"; do
  name="$(basename "$a")"
  # Remove an existing asset of the same name so re-publishing is idempotent.
  old="$(curl -s -H "Authorization: token $TOKEN" "$API/releases/$RID/assets" \
    | python3 -c "import sys,json;[print(x['id']) for x in json.load(sys.stdin) if x['name']==sys.argv[1]]" "$name")"
  [ -n "$old" ] && curl -s -X DELETE -H "Authorization: token $TOKEN" "$API/releases/assets/$old" >/dev/null
  curl -s -X POST -H "Authorization: token $TOKEN" -H "Content-Type: application/zip" \
    --data-binary @"$a" "https://uploads.github.com/repos/$REPO/releases/$RID/assets?name=$name" \
    | python3 -c "import sys,json;d=json.load(sys.stdin);print('    uploaded',d['name'],d['state'])"
done
echo "==> Published: https://github.com/$REPO/releases/tag/$TAG"
