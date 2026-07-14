import sys
import tempfile
import unittest
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT / "tools"))

from catalog_common import long_stay_days, normalize_stock_station_policy, stability_days
from catalog_validation import load_catalog, validate


class CatalogToolTests(unittest.TestCase):
    def test_shared_station_policy(self):
        self.assertEqual(10, stability_days(True))
        self.assertEqual(3, stability_days(False))
        self.assertEqual(150, long_stay_days(True))
        self.assertEqual(60, long_stay_days(False))

    def test_stock_upgrade_keeps_crew(self):
        mission = {"id": "st_kerbin_station_upgrade4", "checks": [
            ("CREW_NONE", [], "empty"),
            ("DURATION", [("days", "10")], "ten days"),
        ]}
        normalize_stock_station_policy(mission)
        self.assertEqual(["DURATION"], [check[0] for check in mission["checks"]])
        self.assertEqual(3, dict(mission["checks"][0][1])["days"])

    def test_validator_detects_unknown_prerequisite(self):
        cfg = """CUSTOM_CONTRACT_CATALOG
{
CONTRACT
{
 id = a
 title = A
 description = A
 sparte = Bemannt
 reward = 1
 epoch = 1
 prerequisite = missing
 CONDITION
 {
  type = COMPOSITE
  CHECK
  {
   kind = CREW_NONE
   label = empty
  }
 }
}
}
"""
        with tempfile.TemporaryDirectory() as directory:
            Path(directory, "a.cfg").write_text(cfg, encoding="utf-8")
            errors = validate(load_catalog(directory))
        self.assertTrue(any("unknown prerequisite" in error for error in errors))


if __name__ == "__main__":
    unittest.main()
