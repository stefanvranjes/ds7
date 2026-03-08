using System.Collections.Generic;
using UnityEngine;

namespace DS7.Data
{
    /// <summary>
    /// Per-nation data: display info, starting funds rate, unit roster.
    /// </summary>
    [CreateAssetMenu(menuName = "DS7/Nation Data", fileName = "NewNation")]
    public class NationData : ScriptableObject
    {
        [Header("Identity")]
        public Nation nation;
        public string displayName;
        public Color flagColor = Color.white;
        public Sprite flagSprite;

        [Header("Economy")]
        [Tooltip("Base funds earned per turn from owned facilities.")]
        public int baseFundRate = 100;

        [Header("Unit Roster")]
        [Tooltip("All units available to this nation for production.")]
        public List<UnitData> unitRoster = new();

        [Header("Campaign Slot")]
        [Tooltip("Maximum saved units carried between Campaign battles.")]
        public int maxSavedUnits = 20;
    }
}
