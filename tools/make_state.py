#!/usr/bin/env python3
"""Erzeugt einen vorprogressten Spielstand (contracts_state.cfg): markiert die Voraussetzungs-
Closure der genannten Meilensteine als CompletedOnce, damit ein Save genau bis dorthin
fortgeschritten startet. Rest folgt den normalen Freischaltregeln."""
import re, os, sys

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CDIR = os.path.join(ROOT, "GameData", "CustomScienceContracts", "Contracts")
FILES = ["A_Pioniere.cfg", "B_Spaeher.cfg", "C_Lebensadern.cfg", "D_Stationen.cfg"]

# Meilensteine: 3 Satelliten im Erdorbit, bemannter Erdorbit, unbemannter Mond-Vorbeiflug.
SEEDS = ["un_earth_science_satellite", "un_earth_satellite_pair", "un_earth_high_satellite",
         "cr_earth_orbit", "un_luna_flyby"]

def blocks(text, name):
    out = []
    for mt in re.finditer(r'(?m)^[ \t]*' + re.escape(name) + r'[ \t]*$', text):
        b = text.find("{", mt.end()); depth, j = 0, b
        while j < len(text):
            if text[j] == "{": depth += 1
            elif text[j] == "}":
                depth -= 1
                if depth == 0: break
            j += 1
        out.append(text[b + 1:j])
    return out

def val(b, k):
    m = re.search(rf"^\s*{k}\s*=\s*(.+?)\s*$", b, re.M); return m.group(1) if m else None
def vals(b, k):
    return re.findall(rf"^\s*{k}\s*=\s*(.+?)\s*$", b, re.M)

prereq = {}
for fn in FILES:
    for c in blocks(open(os.path.join(CDIR, fn), encoding="utf-8").read(), "CONTRACT"):
        cid = val(c, "id")
        if cid: prereq[cid] = vals(c, "prerequisite")

# Closure (Meilensteine + alle Vorfahren)
done, stack = set(), list(SEEDS)
while stack:
    cid = stack.pop()
    if cid in done: continue
    if cid not in prereq:
        print(f"WARN: Seed/Prereq '{cid}' nicht im Katalog"); continue
    done.add(cid)
    stack.extend(prereq[cid])

out = os.path.join(ROOT, "für mich", "contracts_state.cfg") if len(sys.argv) < 2 else sys.argv[1]
os.makedirs(os.path.dirname(out), exist_ok=True)
lines = ["CUSTOM_SCIENCE_CONTRACTS_STATE", "{", "\tversion = 1",
         "\tscienceMultiplier = 1", "\tunlockAll = False"]
for cid in sorted(done):
    lines += ["\tSTATE", "\t{", f"\t\tid = {cid}", "\t\tstatus = CompletedOnce",
              "\t\ttotalCompletions = 1", "\t\tcompletionsSinceLastClaim = 0", "\t}"]
lines.append("}")
open(out, "w", encoding="utf-8").write("\n".join(lines) + "\n")
print(f"Closure: {len(done)} Missionen als erledigt markiert -> {out}")
print("Erledigt:", ", ".join(sorted(done)))
