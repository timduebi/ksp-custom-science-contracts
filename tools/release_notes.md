## Custom Science Contracts 0.5.0

**Custom Science Contracts** gives KSP Science Mode a structured mission campaign:
robotic scouting, crewed milestones, stations, bases, supply routes, relays and
long-duration operations all feed into one progression.

**First stable release.** The alpha label is gone — the mission engine, the atlas
and save handling have been in daily use across the Sol and Stock campaigns.
Bug reports are still welcome on GitHub; KSP version, active config, mission
id/name and `KSP.log` are the most useful details.

### Important requirement

The default Sol campaign assumes a probe-first progression. Use a tech-tree or
progression mod that unlocks probes before crewed command pods/crew access;
otherwise the default campaign balance and early mission flow will not line up
correctly. The optional Stock Kerbol campaign is deliberately crewed-first and
does not require that probe-first setup.

### Main download

**`CustomScienceContracts-v0.5.0.zip`** — the complete mod with the default Sol campaign.

1. Download and unzip.
2. Copy the `GameData` folder into your KSP install root.
3. Start a Science Mode save and open Mission Control from the toolbar.

### Optional config swaps

Install the main download from the same release first, then unzip one of these
over it and overwrite the four `GameData/CustomScienceContracts/Contracts/*.cfg`
files. Use only one config swap at a time.

- **Stock Kerbol system** — **`CustomScienceContracts-v0.5.0_Stock-Config.zip`**
- **German Sol** — **`CustomScienceContracts-v0.5.0_Sol-German-Config.zip`**

### Highlights in 0.5.0

**Campaign Atlas as timeline**
- Repeatable missions now stay permanently visible in the Campaign Atlas after
  their first completion, marked as completed (green) — the atlas is a full
  history of every mission you have finished at least once. Re-accepting them
  happens from the Repeatables view.

**Repeatables view rebuilt**
- No more epoch pages: one view lists every unlocked repeatable, grouped by
  target body in atlas order, so any supply or rotation flight is found in
  seconds.
- Every repeatable card shows its state at all times: "Ready — can be accepted
  again", "Available after 2 more missions (1/2)" with a progress bar, "Active",
  "Ready to claim" or "Waiting for a free mission slot".
- Repeatable missions carry a ↻ badge everywhere.

**Atlas and navigation**
- Epoch tabs now size themselves to the window and wrap into a second row
  instead of running off-screen — every epoch is always reachable.
- Each epoch tab shows the number of currently acceptable missions, a
  completion progress bar and a checkmark once the epoch is done.
- Mission Control opens on the epoch where your campaign currently is, and the
  window can now be dragged by its title bar as well as resized.
- Expanded mission cards show their objectives directly — one click fewer.

**Fixes and cleanup**
- Skipping a mission now advances repeatable cooldowns exactly like claiming it.
- Removed a potential NullReferenceException in the check loop when no vessel
  list is available.
- Campaign counts no longer double-count pool repeatables; they are counted in
  the Repeatables tab.
- Removed dead settings keys (`activeButtonX/Y/Size`, `lockedPreviewTrigger`,
  `markerRadiusKmResupply`) and unused code; reduced per-frame UI work.
