#!/usr/bin/env python3
"""Reproducible reward/tech-tree compatibility report for SOL + CTT + Probes Before Crew."""
import argparse
from collections import defaultdict
from pathlib import Path
import re
from catalog_validation import blocks, load_catalog, top_values


def tech_nodes(paths):
    nodes = {}
    for path in paths:
        for block in blocks(Path(path).read_text(encoding="utf-8", errors="replace"), "RDNode"):
            values, _ = top_values(block)
            node_id = values.get("id")
            try:
                cost = float(values.get("cost", "0"))
            except ValueError:
                continue
            if node_id:
                nodes[node_id] = cost
    return nodes


def pbc_targets(directory):
    targets = set()
    if not directory:
        return targets
    pattern = re.compile(r"(?im)^\s*[@%]?TechRequired\s*=\s*([A-Za-z][A-Za-z0-9_]*)\s*$")
    for path in Path(directory).rglob("*.cfg"):
        targets.update(pattern.findall(path.read_text(encoding="utf-8", errors="replace")))
    return targets


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--catalog", type=Path, default=Path("GameData/CustomScienceContracts/Contracts"))
    parser.add_argument("--stock-tree", type=Path, required=True)
    parser.add_argument("--ctt-tree", type=Path, required=True)
    parser.add_argument("--pbc", type=Path)
    args = parser.parse_args()

    missions = load_catalog(args.catalog)
    rewards = defaultdict(float)
    repeatable = 0.0
    for mission in missions:
        value = float(mission.values["reward"])
        rewards[int(mission.values["epoch"])] += value
        if mission.values.get("repeatable", "false").lower() == "true":
            repeatable += value

    nodes = tech_nodes([args.stock_tree, args.ctt_tree])
    targets = pbc_targets(args.pbc)
    # PBC deliberately sends hidden/deprecated parts to this sentinel, not a purchasable node.
    sentinels = {"unresearcheable", "unresearchable"}
    unknown = sorted(targets - set(nodes) - sentinels)
    total_rewards = sum(rewards.values())
    nonrepeatable = total_rewards - repeatable
    total_cost = sum(nodes.values())
    print(f"Catalog missions: {len(missions)}")
    print(f"One-pass rewards: {total_rewards:.0f} science ({nonrepeatable:.0f} non-repeatable; {repeatable:.0f} repeatable pool)")
    print(f"Stock + CTT nodes: {len(nodes)}, nominal full-tree cost: {total_cost:.0f} science")
    print(f"Non-repeatable reward coverage: {nonrepeatable / total_cost:.1%}")
    cumulative = 0.0
    for epoch in sorted(rewards):
        cumulative += rewards[epoch]
        print(f"  Epoch {epoch}: +{rewards[epoch]:.0f}, cumulative {cumulative:.0f}")
    if args.pbc:
        print(f"PBC literal TechRequired targets: {len(targets)}; unresolved against Stock+CTT: {len(unknown)}")
        for node_id in unknown:
            print(f"  unresolved: {node_id}")
    # PBC contains ModuleManager variable copies; literal targets must all resolve. CSC is meant to
    # supplement, not replace, experiment science, so a one-pass coverage above 20% is intentional.
    if unknown or nonrepeatable / total_cost < 0.20:
        raise SystemExit(1)


if __name__ == "__main__":
    main()
