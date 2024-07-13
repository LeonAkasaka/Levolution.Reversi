using Levolution.Reversi.Records;
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
            var diskState = TableCell?.State ?? Records.CellState.None;
            _diskAnimator.Play(diskState.ToString()); // TODO: to Name hash

            var cellState = TableCell.IsSelected ? "Selected"
                : TableCell.IsPlaceable ? "Placeable" : "Unselected";
            _cellAnimator.Play(cellState);
        }
    }
}
