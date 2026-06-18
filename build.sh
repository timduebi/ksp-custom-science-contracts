#!/usr/bin/env bash
# Builds CustomScienceContracts and installs it into the KSP GameData folder.
# Usage: ./build.sh
# Adjust paths here or through environment variables if needed.
set -euo pipefail

KSP="${KSP:-$HOME/Library/Application Support/Steam/steamapps/common/Kerbal Space Program}"
MANAGED="${KSPManaged:-$KSP/KSP.app/Contents/Resources/Data/Managed}"
DOTNET="${DOTNET:-/usr/local/share/dotnet/dotnet}"

PROJ_DIR="$(cd "$(dirname "$0")" && pwd)"
export DOTNET_CLI_TELEMETRY_OPTOUT=1

echo "==> Build (Release) against: $MANAGED"
"$DOTNET" build -c Release -p:KSPManaged="$MANAGED" "$PROJ_DIR/CustomScienceContracts.sln"

echo "==> Installing to: $KSP/GameData/"
rm -rf "$KSP/GameData/CustomScienceContracts"
cp -R "$PROJ_DIR/GameData/CustomScienceContracts" "$KSP/GameData/"

echo "==> Done. DLL:"
ls -la "$KSP/GameData/CustomScienceContracts/Plugins/CustomScienceContracts.dll"
