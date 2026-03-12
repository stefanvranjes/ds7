using UnityEngine;

namespace DS7.Data
{
    /// <summary>
    /// Links a classic team color (Faction) to a sovereign identity (NationData).
    /// </summary>
    [CreateAssetMenu(menuName = "DS7/Faction Data", fileName = "NewFaction")]
    public class FactionData : ScriptableObject
    {
        public Faction faction;
        public string factionName;
        public Color factionColor = Color.white;
        
        [Tooltip("The nation / unit roster used by this faction in this mission.")]
        public NationData selectedNation;
    }
}
