$ErrorActionPreference = "Stop"
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
python (Join-Path $root "tools/release.py") @args
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
