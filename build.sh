#!/usr/bin/env bash
# Baut CustomScienceContracts und installiert es in den KSP-GameData-Ordner.
# Aufruf:  ./build.sh
# Pfade bei Bedarf anpassen (oder als Umgebungsvariablen setzen).
set -euo pipefail

KSP="${KSP:-$HOME/Library/Application Support/Steam/steamapps/common/Kerbal Space Program}"
MANAGED="${KSPManaged:-$KSP/KSP.app/Contents/Resources/Data/Managed}"
DOTNET="${DOTNET:-/usr/local/share/dotnet/dotnet}"

PROJ_DIR="$(cd "$(dirname "$0")" && pwd)"
export DOTNET_CLI_TELEMETRY_OPTOUT=1

echo "==> Build (Release) gegen: $MANAGED"
"$DOTNET" build -c Release -p:KSPManaged="$MANAGED" "$PROJ_DIR/CustomScienceContracts.sln"

echo "==> Installiere nach: $KSP/GameData/"
rm -rf "$KSP/GameData/CustomScienceContracts"
cp -R "$PROJ_DIR/GameData/CustomScienceContracts" "$KSP/GameData/"

echo "==> Fertig. DLL:"
ls -la "$KSP/GameData/CustomScienceContracts/Plugins/CustomScienceContracts.dll"
