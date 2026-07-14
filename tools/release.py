#!/usr/bin/env python3
"""Cross-platform, deterministic CSC build/package entry point."""
import argparse
import hashlib
import json
import os
from pathlib import Path
import re
import shutil
import subprocess
import zipfile

ROOT = Path(__file__).resolve().parents[1]


def run(*command):
    print("+", " ".join(map(str, command)))
    subprocess.run([str(item) for item in command], cwd=ROOT, check=True)


def version():
    source = (ROOT / "src/CustomScienceContracts/Core/ModInfo.cs").read_text(encoding="utf-8")
    match = re.search(r'Version\s*=\s*"([^"]+)"', source)
    if not match:
        raise SystemExit("Could not read ModInfo.Version")
    value = match.group(1)
    avc = json.loads((ROOT / "GameData/CustomScienceContracts/CustomScienceContracts.version").read_text(encoding="utf-8"))["VERSION"]
    avc_value = f"{avc['MAJOR']}.{avc['MINOR']}.{avc['PATCH']}"
    project = re.search(r"<Version>([^<]+)</Version>",
                        (ROOT / "src/CustomScienceContracts/CustomScienceContracts.csproj").read_text(encoding="utf-8")).group(1)
    if len({value, avc_value, project}) != 1:
        raise SystemExit(f"Version mismatch: ModInfo={value}, AVC={avc_value}, project={project}")
    return value


def deterministic_zip(source: Path, target: Path, epoch: int):
    target.parent.mkdir(parents=True, exist_ok=True)
    if target.exists():
        target.unlink()
    # ZIP cannot represent dates before 1980 and stores two-second precision.
    import datetime
    date = datetime.datetime.fromtimestamp(max(epoch, 315532800), datetime.timezone.utc)
    stamp = (date.year, date.month, date.day, date.hour, date.minute, date.second // 2 * 2)
    with zipfile.ZipFile(target, "w", zipfile.ZIP_DEFLATED, compresslevel=9) as archive:
        for path in sorted(p for p in source.rglob("*") if p.is_file()):
            relative = path.relative_to(source).as_posix()
            if path.name == ".DS_Store":
                continue
            info = zipfile.ZipInfo(relative, stamp)
            info.compress_type = zipfile.ZIP_DEFLATED
            info.external_attr = 0o100644 << 16
            archive.writestr(info, path.read_bytes(), compress_type=zipfile.ZIP_DEFLATED, compresslevel=9)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--ksp-managed", type=Path, default=os.environ.get("KSPManaged"))
    parser.add_argument("--skip-build", action="store_true")
    parser.add_argument("--skip-tests", action="store_true")
    parser.add_argument("--output", type=Path)
    args = parser.parse_args()
    release_version = version()

    if not args.skip_tests:
        run("python", "tools/validate_design.py")
        run("python", "tools/validate_design.py", "custom_science_contracts_stock_missionsdesign.md")
        run("python", "tools/validate_catalog.py")
        run("python", "-m", "unittest", "discover", "-s", "tests", "-p", "test_*.py")
        run("dotnet", "test", "tests/CoreLogic.Tests/CoreLogic.Tests.csproj", "-c", "Release")
    if not args.skip_build:
        if not args.ksp_managed:
            raise SystemExit("Pass --ksp-managed or set KSPManaged to build the plugin")
        run("dotnet", "build", "CustomScienceContracts.sln", "-c", "Release",
            f"-p:KSPManaged={Path(args.ksp_managed).resolve()}")

    plugin = ROOT / "GameData/CustomScienceContracts/Plugins/CustomScienceContracts.dll"
    if not plugin.is_file():
        raise SystemExit(f"Built plugin missing: {plugin}")
    output = (args.output or ROOT / "compiled" / f"release-v{release_version}").resolve()
    staging = output / "staging"
    if staging.exists():
        shutil.rmtree(staging)
    main_stage = staging / "main"
    stock_stage = staging / "stock"
    shutil.copytree(ROOT / "GameData", main_stage / "GameData", ignore=shutil.ignore_patterns("*.pdb", "*.zip"))
    for name in ("README.md", "LICENSE", "THIRD_PARTY_NOTICES.md"):
        shutil.copy2(ROOT / name, main_stage / name)
    shutil.copytree(ROOT / "LICENSES", main_stage / "LICENSES")
    shutil.copytree(ROOT / "OptionalConfigs/Stock/GameData", stock_stage / "GameData")
    shutil.copy2(ROOT / "OptionalConfigs/Stock/README.md", stock_stage / "README.md")

    try:
        epoch = int(os.environ.get("SOURCE_DATE_EPOCH") or
                    subprocess.check_output(["git", "show", "-s", "--format=%ct", "HEAD"], cwd=ROOT, text=True).strip())
    except Exception:
        epoch = 315532800
    assets = [
        output / f"CustomScienceContracts-v{release_version}.zip",
        output / f"CustomScienceContracts-v{release_version}_Stock-Config.zip",
    ]
    deterministic_zip(main_stage, assets[0], epoch)
    deterministic_zip(stock_stage, assets[1], epoch)
    checksums = []
    for asset in assets:
        digest = hashlib.sha256(asset.read_bytes()).hexdigest()
        checksums.append(f"{digest}  {asset.name}")
        print(f"{asset.name}: {asset.stat().st_size} bytes, sha256 {digest}")
    (output / "SHA256SUMS.txt").write_text("\n".join(checksums) + "\n", encoding="ascii")
    shutil.rmtree(staging)


if __name__ == "__main__":
    main()
