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
        private Faction _owner = Faction.Neutral;
        /// <summary>Which faction currently owns this facility.</summary>
        public Faction Owner
        {
            get => _owner;
            set
            {
                if (_owner != value)
                {
                    _owner = value;
                    RefreshVisuals();
                }
            }
        }

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
        /// <summary>Set of factions exerting ZOC over this cell (updated each turn).</summary>
        public HashSet<Faction> ZocFactions { get; } = new();

        public bool IsInZocOf(Faction faction) => ZocFactions.Contains(faction);

        // ── Fog of War ────────────────────────────────────────────────────────
        /// <summary>Factions that can currently see this hex.</summary>
        public HashSet<Faction> VisibleTo { get; } = new();

        public bool IsVisibleTo(Faction faction) => VisibleTo.Contains(faction);

        // ── Overlays (Roads / Rivers) ────────────────────────────────────────
        [field: SerializeField] public int RoadMask { get; private set; } = -1;
        [field: SerializeField] public int RiverMask { get; private set; } = -1;

        private GameObject _roadOverlay;
        private GameObject _riverOverlay;

        public void SetRoadMask(int mask)
        {
            RoadMask = mask;
            RefreshOverlays();
        }

        public void SetRiverMask(int mask)
        {
            RiverMask = mask;
            RefreshOverlays();
        }

        public void ClearOverlays()
        {
            RoadMask = -1;
            RiverMask = -1;
            RefreshOverlays();
        }

        // ── Initializer ───────────────────────────────────────────────────────
        public void Initialize(HexCoordinates coords, DS7.Data.TerrainData terrain)
        {
            Coordinates = coords;
            Terrain = terrain;
            facilityHealth = (terrain != null && terrain.isFacility) ? 100 : 0;
            // Overlays are persistent unless cleared
            if (RoadMask == 0) RoadMask = -1; // Default to none if uninitialized
            if (RiverMask == 0) RiverMask = -1;
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
            // Managed by pooling
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

        /// <summary>Repaints the tile tint and updates overlays.</summary>
        public void RefreshVisuals()
        {
            if (_meshRenderer == null) _meshRenderer = GetComponent<MeshRenderer>();
            if (_meshRenderer != null)
            {
                Color tint = (Terrain != null) ? Terrain.editorTint : Color.white;
                _meshRenderer.material.color = tint;
            }

            // Faction coloring on the last child (top surface)
            if (transform.childCount > 0)
            {
                Transform lastChild = transform.GetChild(transform.childCount - 1);
                var lastChildRenderer = lastChild.GetComponent<MeshRenderer>();
                if (lastChildRenderer != null)
                {
                    Color factionColor = FactionManager.Instance != null 
                        ? FactionManager.Instance.GetFactionColor(Owner) 
                        : Color.white;
                    lastChildRenderer.material.color = factionColor;
                }
            }

            RefreshOverlays();
        }

        private void RefreshOverlays()
        {
            UpdateOverlay(ref _roadOverlay, "Road", RoadMask);
            UpdateOverlay(ref _riverOverlay, "River", RiverMask);
        }

        private void UpdateOverlay(ref GameObject overlayObj, string folder, int mask)
        {
            if (mask < 0)
            {
                if (overlayObj != null) DestroyImmediate(overlayObj);
                overlayObj = null;
                return;
            }

            // Load variant
            string baseName = folder;
            string prefabName = (mask == 0) ? baseName : $"{baseName} {mask + 1}";
            // Note: In DS7 style, Road 1 might be mask 0, but usually mask 0 is "no road".
            // If mask 0 is "dot", and mask 1 is "E", etc.
            // Following previous logic: mask 0 -> "Road", mask X -> "Road X+1"
            
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{folder}/{prefabName}");
            if (prefab == null) return;

            if (overlayObj != null)
            {
                // Check if it's already the right one (simple check by name)
                if (overlayObj.name == prefabName) return;
                DestroyImmediate(overlayObj);
            }

            overlayObj = Instantiate(prefab, transform.position, Quaternion.identity, transform);
            overlayObj.name = prefabName;
            
            // Adjust height to sit on top of base terrain
            //float heightOffset = (Terrain != null) ? Terrain.visualHeight : 0.1f;
            //overlayObj.transform.position += Vector3.up * (heightOffset + 0.01f);
        }
    }
}
