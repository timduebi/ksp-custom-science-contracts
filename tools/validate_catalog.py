#!/usr/bin/env python3
import argparse
from pathlib import Path
from catalog_validation import load_catalog, validate


def main():
    parser = argparse.ArgumentParser(description="Validate CSC catalog semantics")
    parser.add_argument("directories", nargs="*", type=Path, default=[
        Path("GameData/CustomScienceContracts/Contracts"),
        Path("OptionalConfigs/SOL-German/GameData/CustomScienceContracts/Contracts"),
        Path("OptionalConfigs/Stock/GameData/CustomScienceContracts/Contracts"),
    ])
    args = parser.parse_args()
    failed = False
    for directory in args.directories:
        missions = load_catalog(directory)
        errors = validate(missions)
        print(f"{directory}: {len(missions)} missions, {len(errors)} error(s)")
        for error in errors:
            print("  ERROR:", error)
        failed |= bool(errors)
    raise SystemExit(1 if failed else 0)


if __name__ == "__main__":
    main()
