using System.Collections.Generic;
using System.Linq;
using CustomScienceContracts.Model;

namespace CustomScienceContracts.Data
{
    /// <summary>In-memory view of all loaded contracts with lookups by id, branch and subcategory.
    /// Holds MissionContract instances with definition and runtime state.</summary>
    public class ContractCatalog
    {
        private readonly List<MissionContract> _all = new List<MissionContract>();
        private readonly Dictionary<string, MissionContract> _byId = new Dictionary<string, MissionContract>();

        public IReadOnlyList<MissionContract> All => _all;

        public void Set(IEnumerable<MissionContract> contracts)
        {
            _all.Clear();
            _byId.Clear();
            foreach (var c in contracts)
            {
                _all.Add(c);
                _byId[c.Id] = c;
            }
        }

        public MissionContract Get(string id)
        {
            _byId.TryGetValue(id, out var c);
            return c;
        }

        public bool Exists(string id) => _byId.ContainsKey(id);

        public IEnumerable<MissionContract> InSparte(Sparte s) => _all.Where(c => c.HeimatSparte == s);

        public IEnumerable<MissionContract> InSubcategory(Sparte s, string sub) =>
            _all.Where(c => c.HeimatSparte == s && c.Unterkategorie == sub);

        public IEnumerable<string> Subcategories(Sparte s) =>
            _all.Where(c => c.HeimatSparte == s)
                .Select(c => c.Unterkategorie)
                .Distinct();
    }
}
