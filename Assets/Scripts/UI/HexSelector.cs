using DS7.Data;
using DS7.Grid;
using DS7.Units;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DS7.UI
{
    /// <summary>
    /// Handles hex selection via mouse click / raycast.
    /// Highlights movement range and attack targets when a unit is selected.
    /// </summary>
    public class HexSelector : MonoBehaviour
    {
        [Header("Layers")]
        public LayerMask hexLayer;
        public LayerMask unitLayer;

        [Header("References")]
        public UnitInfoPanel unitInfoPanel;
        public CommandMenu   commandMenu;

        // ── State ─────────────────────────────────────────────────────────────
        private Unit     _selectedUnit;
        private HexCell  _selectedCell;
        private bool     _awaitingMoveTarget;

        private HexGrid _grid;

        private void Start() => _grid = HexGrid.Instance;

        // ── Update Loop ───────────────────────────────────────────────────────
        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (Input.GetMouseButtonDown(0))
                HandleLeftClick();

            if (Input.GetMouseButtonDown(1))
                Deselect();
        }

        // ── Click Handling ────────────────────────────────────────────────────
        private void HandleLeftClick()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // First check if we clicked a unit
            if (Physics.Raycast(ray, out var unitHit, 500f, unitLayer))
            {
                var unit = unitHit.collider.GetComponentInParent<Unit>();
                if (unit != null)
                {
                    SelectUnit(unit);
                    return;
                }
            }

            // Otherwise check hex
            if (Physics.Raycast(ray, out var hexHit, 500f, hexLayer))
            {
                var cell = hexHit.collider.GetComponentInParent<HexCell>();
                if (cell == null) return;

                if (_awaitingMoveTarget && _selectedUnit != null)
                {
                    ExecuteMove(cell);
                }
                else
                {
                    SelectCell(cell);
                }
            }
        }

        // ── Select Unit ───────────────────────────────────────────────────────
        private void SelectUnit(Unit unit)
        {
            _selectedUnit = unit;
            _selectedCell = _grid?.GetCell(unit.CurrentCoords);

            _grid?.ClearAllHighlights();
            _selectedCell?.SetHighlight(HexCell.HighlightMode.Selected);

            // Show movement range
            if (!unit.HasMoved)
            {
                var range = _grid?.GetMovementRange(unit);
                if (range != null)
                    _grid.HighlightMovementRange(range);
            }

            // Show attack targets
            if (!unit.HasActed)
            {
                var targets = _grid?.GetAttackTargets(unit, unit.HasMoved);
                if (targets is { Count: > 0 })
                    _grid.HighlightAttackTargets(targets);
            }

            unitInfoPanel?.Show(unit);
            commandMenu?.Show(unit);
        }

        // ── Select Cell ───────────────────────────────────────────────────────
        private void SelectCell(HexCell cell)
        {
            _grid?.ClearAllHighlights();
            cell.SetHighlight(HexCell.HighlightMode.Selected);
            _selectedCell = cell;
            _selectedUnit = null;

            unitInfoPanel?.Hide();
            commandMenu?.Hide();
        }

        // ── Execute Move ──────────────────────────────────────────────────────
        private void ExecuteMove(HexCell target)
        {
            if (_selectedUnit == null) return;

            bool highSpeed = IsHighSpeedHex(target);
            _selectedUnit.GetComponent<UnitController>()?.TryMove(
                target, _selectedUnit.CurrentAltitude, highSpeed);

            _awaitingMoveTarget = false;
            _grid?.ClearAllHighlights();

            // Re-highlight after move
            SelectUnit(_selectedUnit);
        }

        private bool IsHighSpeedHex(HexCell cell)
        {
            if (_selectedUnit == null) return false;
            var range = _grid?.GetMovementRange(_selectedUnit);
            return range != null && range.HighSpeedRange.Contains(cell);
        }

        // ── Deselect ──────────────────────────────────────────────────────────
        public void Deselect()
        {
            _selectedUnit       = null;
            _selectedCell       = null;
            _awaitingMoveTarget = false;

            _grid?.ClearAllHighlights();
            unitInfoPanel?.Hide();
            commandMenu?.Hide();
        }

        // ── Called by CommandMenu ─────────────────────────────────────────────
        public void BeginMoveCommand()  => _awaitingMoveTarget = true;
    }
}
