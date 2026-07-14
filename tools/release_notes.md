## Custom Science Contracts 0.7.0 — Campaign Integrity

Version 0.7.0 is a compatibility-preserving reliability and progression release for KSP 1.12.x
Science Mode. Existing mission ids, prerequisite chains and completion counts remain valid.

### Highlights

- Mission Control and Active Missions now occupy KSP's stock launcher lane beside stock applications,
  using the same public `AddApplication` route as Kerbalism.
- Save format v2 writes atomically, preserves claim-ready evidence, and keeps a true append-only Program
  Log. Older saves migrate automatically; historical payouts that were never stored are clearly marked.
- Return missions invisibly verify that at least one Kerbal who reached the source comes home. Already
  active pre-0.7 returns retain a compatibility path.
- Station upgrades no longer demand an evacuation. The first 150-day endurance milestone remains;
  later upgrade tiers are less repetitive at 60 days, with 3-day stabilization checks.
- Difficulty is split into Economy, Pacing and Operations while Casual/Normal/Hard remain one-click
  presets. Operational relay checks, startup diagnostics and faster indexed vessel evaluation round out
  the runtime improvements.
- Catalog generators share compatibility policies and are guarded by semantic validators, regression
  tests and public CI. Release ZIPs are deterministic and include SHA-256 checksums.

### Balance target

The default SOL catalog was tested against Community Tech Tree and Probes Before Crew. Its one-pass
non-repeatable rewards cover about half of the nominal full CTT cost, intentionally leaving experiments
important while still providing visible progress to players who complete only part of the campaign.

### Downloads

- `CustomScienceContracts-v0.7.0.zip`: complete mod with the default SOL campaign.
- `CustomScienceContracts-v0.7.0_Stock-Config.zip`: optional Stock Kerbol catalog overlay; install the
  main ZIP first, then overwrite the catalog files with this package.
