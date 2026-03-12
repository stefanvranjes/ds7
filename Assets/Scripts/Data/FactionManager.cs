using System.Collections.Generic;
using UnityEngine;

namespace DS7.Data
{
    /// <summary>
    /// Central registry for all Factions in the current mission.
    /// Allows for easy lookup of FactionData and UI colors.
    /// </summary>
    [CreateAssetMenu(menuName = "DS7/Faction Manager", fileName = "FactionManager")]
    public class FactionManager : ScriptableObject
    {
        public static FactionManager Instance { get; private set; }

        private void OnEnable()
        {
            if (Instance == null) Instance = this;
        }

        private void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        [Header("Factions")]
        public List<FactionData> factions = new();

        private Dictionary<Faction, FactionData> _lookup;

        /// <summary>
        /// Ensures the lookup dictionary is populated.
        /// </summary>
        public void Initialize()
        {
            _lookup = new Dictionary<Faction, FactionData>();
            foreach (var data in factions)
            {
                if (data == null) continue;
                if (!_lookup.ContainsKey(data.faction))
                {
                    _lookup.Add(data.faction, data);
                }
            }
        }

        public FactionData GetFactionData(Faction faction)
        {
            if (_lookup == null) Initialize();
            return _lookup.TryGetValue(faction, out var data) ? data : null;
        }

        public Color GetFactionColor(Faction faction)
        {
            var data = GetFactionData(faction);
            return data != null ? data.factionColor : Color.gray;
        }

        public string GetFactionName(Faction faction)
        {
            var data = GetFactionData(faction);
            return data != null ? data.factionName : faction.ToString();
        }
        
        public NationData GetNationData(Faction faction)
        {
            var data = GetFactionData(faction);
            return data != null ? data.selectedNation : null;
        }
    }
}
