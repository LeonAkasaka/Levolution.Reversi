using UnityEngine;

namespace Levolution.Reversi.Components
{
    /// <summary>
    /// Table cell view component.
    /// </summary>
    public class TableCellView : MonoBehaviour
    {
        private const float CellMargin = 1.1F;

        #region SerializeField

        [SerializeField]
        private Animator _animator = null;

        #endregion

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
            var state = TableCell?.State ?? Records.CellState.None;
            _animator.Play(state.ToString()); // TODO: to Name hash

            name = nameof(TableCellView) + $"({TableCell?.Position.ToString()})";

            var x = TableCell?.Position.Column ?? 0;
            var z = TableCell?.Position.Row ?? 0;
            transform.localPosition = new Vector3(x * CellMargin, 0, -(z * CellMargin));
        }
    }

}
