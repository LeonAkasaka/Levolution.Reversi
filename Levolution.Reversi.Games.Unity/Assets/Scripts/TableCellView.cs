using Levolution.Reversi.Records;
using UnityEditor;
using UnityEngine;

namespace Levolution.Reversi.Components
{
    /// <summary>
    /// Table cell view component.
    /// </summary>
    public class TableCellView : MonoBehaviour
    {
        #region SerializeField

        [SerializeField]
        private Animator _cellAnimator = null;

        [SerializeField]
        private Animator _diskAnimator = null;

        [SerializeField]
        private CellState _state = CellState.None;

        [SerializeField]
        private bool _isSelected = false;


        [SerializeField]
        private bool _isPlaceable = false;

        #endregion

        public CellState State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnStateChanged();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnSelectedChanged();
            }
        }

        public bool IsPlaceable
        {
            get => _isPlaceable;
            set
            {
                _isPlaceable = value;
                OnPlaceableChanged();
            }
        }

        private void UpdateView()
        {
            OnStateChanged();
            UpdateCell();
        }

        private void OnValidate()
        {
            UpdateView();
        }

        private void Start()
        {
            UpdateView();
        }

        private void OnStateChanged()
        {
            _diskAnimator.Play(State.ToString()); // TODO: to Name hash
#if UNITY_EDITOR
            _diskAnimator.Update(Time.deltaTime);
#endif
        }

        private void OnSelectedChanged()
        {
            UpdateCell();
        }

        private void OnPlaceableChanged()
        {
            UpdateCell();
        }

        private void UpdateCell()
        {
            var name = IsSelected ? "Selected"
                            : IsPlaceable ? "Placeable" : "Unselected";
            _cellAnimator.Play(name);
#if UNITY_EDITOR
            _cellAnimator.Update(Time.deltaTime);
#endif
        }

        /// <summary>
        /// Binding data source.
        /// </summary>
        public TableCell TableCell
        {
            get { return _tableCell; }
            set
            {
                if (_tableCell != null) { _tableCell.PropertyChanged -= OnTableCellPropertyChanged; }

                _tableCell = value;
                UpdateTableCell();

                if (_tableCell != null) { _tableCell.PropertyChanged += OnTableCellPropertyChanged; }

            }
        }
        private TableCell _tableCell;

        private void OnTableCellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateTableCell();
        }

        private void UpdateTableCell()
        {
            State = TableCell?.State ?? CellState.None;
            IsSelected = TableCell?.IsSelected ?? false;
            IsPlaceable = TableCell?.IsPlaceable ?? false;
        }
    }
}
