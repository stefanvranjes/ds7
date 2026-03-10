using UnityEngine;

namespace DS7.Data
{
    /// <summary>
    /// Terrain type definition: movement costs, defense bonus, detection modifier,
    /// and facility properties.
    /// </summary>
    [CreateAssetMenu(menuName = "DS7/Terrain Data", fileName = "NewTerrain")]
    public class TerrainData : ScriptableObject
    {
        [Header("Identity")]
        public string terrainName;
        public TerrainType terrainType;
        public Sprite tileSprite;
        [ColorUsage(false)] public Color mapColor  = Color.green;
        [Tooltip("Tile tint colour shown in the Map Editor and on the grid.")]
        [ColorUsage(false)] public Color editorTint = Color.white;
        [Tooltip("The vertical height at which highlights should appear on this terrain.")]
        public float visualHeight = 0.5f;

        [Header("Facility")]
        public bool isFacility;
        [Tooltip("Income generated per turn when owned.")]
        public int incomePerTurn;
        [Tooltip("Can produce units here?")]
        public bool canProduce;
        [Tooltip("Can resupply / repair units here?")]
        public bool canResupply;
        [Tooltip("Can equip weapon packs here?")]
        public bool canEquip;

        [Header("Movement Costs")]
        [Tooltip("Cost for each MovementCategory to enter this hex. -1 = impassable.")]
        public MovementCostEntry[] movementCosts;

        [Header("Combat Modifiers")]
        [Tooltip("Defense bonus applies to unit occupying this hex (%).")]
        [Range(0, 100)] public int defenseBonus;
        [Tooltip("Modifier to detection range of units on this hex. Negative = reduced detection.")]
        public int detectionModifier;

        // ── Helpers ──────────────────────────────────────────────────────────

        public int GetMovementCost(MovementCategory category)
        {
            if (movementCosts == null) return -1;
            foreach (var entry in movementCosts)
                if (entry.category == category)
                    return entry.cost;
            return -1; // impassable
        }
    }

    [System.Serializable]
    public class MovementCostEntry
    {
        public MovementCategory category;
        [Tooltip("-1 = impassable")]
        public int cost;
    }
}
