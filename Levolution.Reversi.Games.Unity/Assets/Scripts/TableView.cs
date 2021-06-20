using Levolution.Reversi.Records;
using System.Collections.Generic;
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

        /// <summary>
        /// Data source.
        /// </summary>
        public Table Table { get; } = new Table();

        public IEnumerable<TableCellView> CellViews => _cellViews;
        private readonly List<TableCellView> _cellViews = new List<TableCellView>();

        public Player FirstPlayer { get; private set; }

        private void Awake()
        {
            var records = CellPosition.ParseList(_records);
            FirstPlayer = Table.Reset(records);

            foreach (var cell in Table.Cells)
            {
                _cellViews.Add(CreateTableCellView(cell));
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
