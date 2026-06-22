## Custom Science Contracts 0.4.1

**Custom Science Contracts** gives KSP Science Mode a structured mission campaign:
robotic scouting, crewed milestones, stations, bases, supply routes, relays and long-duration
operations all feed into one progression.

> Alpha release: expect rough edges. Bug reports are welcome on GitHub; KSP version, active config,
> mission id/name and `KSP.log` are the most useful details.

### Important requirement

This release assumes a probe-first progression. Use a tech-tree or progression mod that unlocks
probes before crewed command pods/crew access; otherwise the campaign balance and early mission
flow will not line up correctly.

### Main download

**`CustomScienceContracts-v0.4.1.zip`** — the complete mod with the default Sol campaign.

1. Download and unzip.
2. Copy the `GameData` folder into your KSP install root.
3. Start a Science Mode save and open Mission Control from the toolbar.

### Optional config swaps

Install the main download first, then unzip one of these over it and overwrite the four
`GameData/CustomScienceContracts/Contracts/*.cfg` files. Use only one config swap at a time.

- **Stock Kerbol system** — **`CustomScienceContracts-v0.4.1_Stock-Config.zip`**
- **German Sol** — **`CustomScienceContracts-v0.4.1_Sol-German-Config.zip`**

### Highlights in 0.4.1

- Reworked Mission Control into a fullscreen-style, resizable mission atlas with epoch pages.
- Added rounded mission dependency arrows, status colors, body rows and unlock previews.
- Added stricter robotic-first progression, including assigned docking targets and improved
  return, rover and relay checks.
- Regenerated the Sol and German Sol catalogs with the updated Moon, Mars, Phobos and asteroid flow.
- Reworked the Moon base gate so a base follows sustained lunar station operations, with two
  optional early base-site survey landings as side missions.
- Split early asteroid belt scouting into the Red Horizon chapter so Beltworks focuses on deeper
  belt operations.
- Improved icon loading so the bundled `icon_...` mission icons win over cached/stock names.
