## Custom Science Contracts 0.8.0 — Stations That Earn the Name

Custom Science Contracts 0.8.0 turns station certification into a real construction standard and
strengthens the campaign's long-duration infrastructure play. It also completes the progression,
logistics and save-compatibility work prepared since 0.7.0.

This is a stable release for Kerbal Space Program 1.12.x Science Mode. Existing saves are supported.

### Mandatory station engineering

The separate optional certification cards are gone. A station now has to satisfy the engineering
requirements when it is built:

- every ordinary orbital-station core requires minimum mass, docking ports and ElectricCharge capacity;
- the requirements scale with the station's required seat capacity;
- every expansion rechecks mass, ports and power;
- the first expansion adds a mandatory compatible science laboratory, and every later expansion keeps
  that laboratory requirement;
- expansion missions no longer require evacuating the existing crew;
- ordinary orbital stations do not need a fuel stockpile. Fuel remains limited to dedicated depots, fuel
  stations and meaningful surface logistics.

The laboratory check accepts stock- and Kerbalism-style module alternatives:
`ModuleScienceLab`, `ModuleScienceConverter` or `Laboratory`.

Initial construction uses this baseline:

- mass: `max(12, 8 + 2 × seats)` tonnes;
- docking ports: `max(2, ceil(seats / 2))`;
- ElectricCharge: `max(1000, 250 × seats)`;
- uncrewed and stable in the target orbit for 10 days.

Expansions use:

- mass: `max(18, 8 + 3 × seats)` tonnes;
- the same capacity-scaled docking and ElectricCharge formulas;
- at least one compatible laboratory;
- three days of occupied stabilization.

The authored Eve support station and Jool gateway keep their stricter existing port/power thresholds;
0.8 only adds the missing mass baseline.

### Harder endurance milestones

Every infrastructure stage that previously required a 60-day stay now requires 120 days, including the
Titan and generated base stages. The first major endurance milestone in each generated chain remains 150
days. This preserves the campaign's initial long-stay achievement while preventing later, larger stations
from becoming easier than the infrastructure meant to support them.

### Clearer campaign progression

- The recommended path is a complete topological route: it includes every prerequisite needed by its
  milestones and shows only the single mission recommended next.
- Recommended missions never lock side missions or add a second hidden progression system.
- Locked cards show the shortest complete prerequisite sequence needed to reach them.
- The green atlas header message and generated transition/finale banners are absent; mission descriptions
  remain the narrative source.

### Better networks and logistics

- Later relay missions validate a phased operational topology with redundancy instead of accepting only a
  raw satellite count.
- Resource-delivery missions record the destination inventory at acceptance and require newly delivered
  resources. Orbital deliveries survive the vessel merge created by docking.
- Active missions accepted under an older evaluation schema keep their acceptance-time behavior, so an
  update cannot silently move the goalposts in the middle of a flight.

### Save compatibility

0.8.0 restores and regression-tests every historical CSC save layout. KSP's `ConfigNode.Save` writes a
valid bare document without the root-node name; the loader now accepts both that historical form and the
new explicitly wrapped form.

The following state survives the update:

- completed and active mission ids;
- ready-to-claim evidence and duration timers;
- station, vessel and relay-fleet bindings;
- repeatable cooldowns and completion counts;
- science/difficulty/UI settings;
- Program Log entries and imported pre-0.7 history.

The six removed optional certification ids are ignored safely if an old state file contains them. They
were never prerequisites, so no campaign path is broken.

### Catalogs and balance

- Default SOL English: 246 missions, with a 102-step recommended route.
- Optional Stock English: 118 missions, with a 65-step recommended route.
- Maintained SOL German repository catalog: 246 missions, with identical ids and logic to SOL English.

All three catalogs pass the semantic validator. The reward curve is checked against the stock tech tree,
Community Tech Tree and Probes Before Crew targets. CSC remains a progression supplement: experiment
science is still necessary, while players who complete only part of the campaign still feel meaningful
technology progress.

### Interface and launcher behavior

Mission Control and Active Missions register directly in KSP's stock Application Launcher lane through
`AddApplication`. CSC does not bundle or require Toolbar Controller, Toolbar Continued or Contract
Configurator. The Active Missions icon retains its ready-to-claim badge.

### Documentation

The README has been rewritten as a full manual, including:

- exact hard, profile-specific, recommended and optional dependencies;
- clean installation, Stock overlay and upgrade instructions;
- mission counts and campaign-edition differences;
- the station engineering formulas and worked examples;
- relay, delivery, duration, return and precision-landing behavior;
- difficulty, repeatable and save-backup behavior;
- troubleshooting and contributor/release workflow.

### Downloads

- `CustomScienceContracts-v0.8.0.zip` — complete mod, plugin, assets and default SOL English campaign.
- `CustomScienceContracts-v0.8.0_Stock-Config.zip` — optional Stock Kerbol catalog overlay. Install the
  main ZIP first, then overwrite the four contract catalog files with this package.
- `SHA256SUMS.txt` — SHA-256 hashes for both deterministic ZIP assets.

For a clean upgrade from an old manual copy, close KSP, delete only
`GameData/CustomScienceContracts`, then install the main 0.8.0 ZIP. Save progress lives separately in
`saves/<SaveName>/CustomScienceContracts/` and is not removed by that cleanup.
