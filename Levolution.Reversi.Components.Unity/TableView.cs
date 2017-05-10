using Levolution.Reversi.Records;
using UnityEngine;

namespace Levolution.Reversi.Components
{
    public class TableView : MonoBehaviour
    {
        #region SerializeField

        [SerializeField]
        private TableCellView _cellViewPrefab = null;

        [SerializeField]
        private string _records = null;

        #endregion

        public Table Table { get; private set; }

        void Start()
        {
            var records = CellPosition.ParseList(_records);
            Table = new Table();
            Table.Reset(records);

            foreach (var cell in Table.Cells)
            {
                CreateTableCellView(cell);
            }
        }

        private TableCellView CreateTableCellView(TableCell cell)
        {
            var cellView = Instantiate(_cellViewPrefab, transform, true);
            cellView.TableCell = cell;

            return cellView;
        }
    }

}
