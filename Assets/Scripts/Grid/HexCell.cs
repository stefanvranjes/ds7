using System.Collections.Generic;
using DS7.Data;
using DS7.Units;
using UnityEngine;

namespace DS7.Grid
{
    /// <summary>
    /// A single cell in the hex grid. Tracks terrain, facility state, ownership,
    /// and the units occupying each altitude layer.
    /// </summary>
    public class HexCell : MonoBehaviour
    {
        // ── Identity ──────────────────────────────────────────────────────────
        public HexCoordinates Coordinates { get; private set; }

        // ── Terrain ───────────────────────────────────────────────────────────
        [field: SerializeField] public DS7.Data.TerrainData Terrain { get; private set; }

        public bool IsFacility => Terrain != null && Terrain.isFacility;

        // ── Ownership & Facility State ────────────────────────────────────────
        /// <summary>Which nation currently owns this facility (null = neutral).</summary>
        public Nation Owner { get; set; } = Nation.Neutral;

        /// <summary>Capture progress counter. Ground units add 1/turn while occupying.</summary>
        public int CaptureProgress { get; set; } = 0;

        /// <summary>Facility health 0–100. Reduced by bombing, restored by engineers.</summary>
        [Range(0, 100)] public int facilityHealth = 100;

        // ── Unit Occupancy (one unit per altitude layer) ───────────────────────
        private readonly Dictionary<AltitudeLayer, Unit> _occupants = new();

        public bool IsOccupied(AltitudeLayer altitude) => _occupants.ContainsKey(altitude);

        public Unit GetUnit(AltitudeLayer altitude)
            => _occupants.TryGetValue(altitude, out var u) ? u : null;

        public bool TryPlace(Unit unit, AltitudeLayer altitude)
        {
            if (_occupants.ContainsKey(altitude)) return false;
            _occupants[altitude] = unit;
            return true;
        }

        public void RemoveUnit(AltitudeLayer altitude)
            => _occupants.Remove(altitude);

        public IEnumerable<Unit> AllUnits() => _occupants.Values;

        // ── Zone of Control ───────────────────────────────────────────────────
        /// <summary>Set of nations exerting ZOC over this cell (updated each turn).</summary>
        public HashSet<Nation> ZocNations { get; } = new();

        public bool IsInZocOf(Nation nation) => ZocNations.Contains(nation);

        // ── Fog of War ────────────────────────────────────────────────────────
        /// <summary>Nations that can currently see this hex.</summary>
        public HashSet<Nation> VisibleTo { get; } = new();

        public bool IsVisibleTo(Nation nation) => VisibleTo.Contains(nation);

        // ── Initializer ───────────────────────────────────────────────────────
        public void Initialize(HexCoordinates coords, DS7.Data.TerrainData terrain)
        {
            Coordinates = coords;
            Terrain = terrain;
            facilityHealth = 100;
        }

        // ── Visual Feedback ───────────────────────────────────────────────────
        public enum HighlightMode { None, MoveRange, AttackRange, Selected, Enemy, Brush }

        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void SetHighlight(HighlightMode mode)
        {
            // Highlights are now managed by HexGrid pooling and MapEditor positioning.
            // This method can be used for material color fallbacks if desired,
            // or left empty if the pooling handles everything.
        }

        public void UpdateHighlightPositions()
        {
            // No longer needed here as MapEditor positions pooled objects in world space.
        }

        // ── Map-Editor Helpers ────────────────────────────────────────────────
        /// <summary>Replaces the terrain on this cell at runtime (used by MapEditor).</summary>
        public void SetTerrain(DS7.Data.TerrainData terrain)
        {
            Terrain = terrain;
            facilityHealth = (terrain != null && terrain.isFacility) ? 100 : 0;
            RefreshVisuals();
        }

        /// <summary>Toggles whether a facility on this cell is active.</summary>
        public void SetFacilityActive(bool active)
        {
            if (!IsFacility) return;
            facilityHealth = active ? 100 : 0;
        }

        /// <summary>Repaints the tile tint from the TerrainData colour (or white for null).</summary>
        public void RefreshVisuals()
        {
            if (_meshRenderer == null) _meshRenderer = GetComponent<MeshRenderer>();
            if (_meshRenderer == null) return;

            Color tint = (Terrain != null) ? Terrain.editorTint : Color.white;
            _meshRenderer.material.color = tint;
        }
    }
}
