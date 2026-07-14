import sys
import tempfile
import unittest
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT / "tools"))

from catalog_common import (long_stay_days, normalize_stock_station_policy, parse_check,
                            recommended_route_order, stability_days, upgrade_operational_checks)
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

    def test_station_expansion_scales_ports_and_power(self):
        mission = {"id": "st_kerbin_station_upgrade8", "description": "Expand.", "checks": [
            ("CREW_CAPACITY_MIN", [("min", "8")], "eight seats"),
            ("DURATION", [("days", "10")], "ten days"),
        ]}
        normalize_stock_station_policy(mission)
        checks = {kind: dict(values) for kind, values, _ in mission["checks"]}
        self.assertEqual("4", checks["DOCKING_PORT_COUNT"]["count"])
        self.assertEqual("2000", checks["POWER_CAPACITY_MIN"]["amount"])
        self.assertNotIn("fuel", mission["description"].lower())

    def test_recommended_route_expands_prerequisites_and_orders_them_first(self):
        prerequisites = {"a": [], "b": ["a"], "c": ["b"], "side": []}
        order = recommended_route_order(prerequisites, {"c"}, lambda mission_id: mission_id)
        self.assertEqual({"a": 1, "b": 2, "c": 3}, order)
        self.assertNotIn("side", order)

    def test_relay_upgrade_requires_phasing_and_reserve_without_losing_orbit_fields(self):
        mission = {"checks": [("RELAY_VESSEL_COUNT_INCLINATION",
                               [("body", "Moon"), ("count", "3"),
                                ("inclinationMin", "75"), ("km", "100")], "old")]}
        check = upgrade_operational_checks(mission)["checks"][0]
        values = dict(check[1])
        self.assertEqual("RELAY_NETWORK_TOPOLOGY", check[0])
        self.assertEqual("1", values["redundancy"])
        self.assertEqual("20", values["separationMin"])
        self.assertEqual("150", values["maxGap"])
        self.assertEqual("75", values["inclinationMin"])
        self.assertEqual("100", values["km"])

    def test_only_docked_fuel_snapshot_is_upgraded_to_delivery(self):
        docked = {"checks": [
            ("FUEL_MIN", [("amount", "250")], "old"),
            ("DOCK_STATION", [("stationKey", "target")], "dock"),
        ]}
        ordinary = {"checks": [("FUEL_MIN", [("amount", "250")], "old")]}
        upgraded = upgrade_operational_checks(docked)["checks"][0]
        self.assertEqual("RESOURCE_DELIVERY", upgraded[0])
        self.assertEqual("FUEL_MIN", dict(upgraded[1])["legacyKind"])
        self.assertEqual("FUEL_MIN", upgrade_operational_checks(ordinary)["checks"][0][0])

    def test_module_alternatives_do_not_conflict_with_label_delimiter(self):
        kind, values, label = parse_check(
            "MODULE_COUNT ModuleScienceLab|ModuleScienceConverter|Laboratory 1 | compatible science module")
        self.assertEqual("MODULE_COUNT", kind)
        self.assertEqual("ModuleScienceLab|ModuleScienceConverter|Laboratory", dict(values)["module"])
        self.assertEqual("compatible science module", label)

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
