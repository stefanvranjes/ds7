using DS7.Data;
using DS7.Units;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DS7.UI
{
    /// <summary>
    /// In-battle command menu displayed when a unit is selected.
    /// Buttons call into UnitController or HexSelector.
    /// </summary>
    public class CommandMenu : MonoBehaviour
    {
        [Header("Buttons")]
        public Button moveButton;
        public Button attackButton;
        public Button resupplyButton;
        public Button captureButton;
        public Button deployButton;
        public Button jamButton;
        public Button equipButton;
        public Button loadButton;
        public Button unloadButton;
        public Button waitButton;

        [Header("Status Label")]
        public TMP_Text activeNationText;

        private Unit          _unit;
        private HexSelector   _selector;

        private void Awake()
        {
            _selector = FindObjectOfType<HexSelector>();
            gameObject.SetActive(false);
            BindButtons();
        }

        // ── Bind ──────────────────────────────────────────────────────────────
        private void BindButtons()
        {
            moveButton?.onClick.AddListener(() => _selector?.BeginMoveCommand());

            attackButton?.onClick.AddListener(() =>
            {
                // Attack is triggered by clicking an enemy cell after selecting it via HexSelector
                // HexSelector handles this automatically once attack targets are highlighted
            });

            resupplyButton?.onClick.AddListener(() =>
            {
                if (_unit == null) return;
                Logistics.SupplySystem.Instance?.Resupply(_unit,
                    GameModes.TurnManager.Instance?.NationFunds);
                Hide();
            });

            captureButton?.onClick.AddListener(() =>
            {
                if (_unit == null) return;
                var cell = Grid.HexGrid.Instance?.GetCell(_unit.CurrentCoords);
                if (cell != null)
                    _unit.GetComponent<UnitController>()?.AttemptCapture(cell);
                Refresh();
            });

            deployButton?.onClick.AddListener(() =>
            {
                _unit?.GetComponent<UnitController>()?.Deploy();
                Refresh();
            });

            jamButton?.onClick.AddListener(() =>
            {
                _unit?.GetComponent<UnitController>()?.ActivateJam();
                Refresh();
            });

            waitButton?.onClick.AddListener(() =>
            {
                _unit?.MarkActed();
                _selector?.Deselect();
            });
        }

        // ── Show / Hide ───────────────────────────────────────────────────────
        public void Show(Unit unit)
        {
            _unit = unit;
            gameObject.SetActive(true);
            Refresh();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _unit = null;
        }

        // ── Refresh Button States ─────────────────────────────────────────────
        private void Refresh()
        {
            if (_unit == null) return;

            bool canAct  = !_unit.HasActed;
            bool canMove = !_unit.HasMoved;

            if (moveButton)     moveButton.interactable     = canMove;
            if (attackButton)   attackButton.interactable   = canAct;
            if (resupplyButton) resupplyButton.interactable = Logistics.SupplySystem.Instance?.CanResupply(_unit) ?? false;
            if (captureButton)  captureButton.interactable  = canAct && _unit.Data.HasAbility(UnitAbility.Capture);
            if (deployButton)   deployButton.interactable   = canAct && !_unit.HasMoved && _unit.Data.HasAbility(UnitAbility.Dply);
            if (jamButton)      jamButton.interactable      = canAct && _unit.Data.HasAbility(UnitAbility.Jam);
            if (equipButton)    equipButton.interactable    = canAct && !canMove == false;
            if (loadButton)     loadButton.interactable     = _unit.Data.HasAbility(UnitAbility.Trans);
            if (unloadButton)   unloadButton.interactable   = _unit.CargoUnits.Count > 0;

            // Update nation label
            if (activeNationText && GameModes.TurnManager.Instance != null)
                activeNationText.text = $"{GameModes.TurnManager.Instance.ActiveNation} — Turn {GameModes.TurnManager.Instance.TurnNumber}";
        }
    }
}
