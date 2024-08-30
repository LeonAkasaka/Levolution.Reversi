using Levolution.Reversi.Records;
using System.Collections.Generic;
using UnityEngine;

namespace Levolution.Reversi.Components
{
    public class TableView : MonoBehaviour
    {
        private const float CellMargin = 1.1F;

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
        private readonly List<TableCellView> _cellViews = new();

        public Player FirstPlayer { get; private set; }

        private void Awake()
        {
            _records = _records.ToLower().Trim();
            var records = CellPosition.ParseList(_records);
            FirstPlayer = Table.Reset(records);

            InitializeTableCells();
        }

        private void InitializeTableCells()
        {
            var index = 0;
            for (var r = 0; r < Table.Rows; r++)
            {
                for (var c = 0; c < Table.Columns; c++)
                {
                    var cell = Table.GetCell(r, c);
                    var cellView = transform.GetChild(index).GetComponent<TableCellView>();
                    cellView.TableCell = cell;
                    _cellViews.Add(cellView);
                    index++;
                }
            }
        }

        private TableCellView CreateTableCellView(CellPosition cellPosition)
        {
            var cellView = Instantiate(_cellViewPrefab, transform, true);

            cellView.name = nameof(TableCellView) + $"({cellPosition})";

            var x = cellPosition.Column;
            var z = cellPosition.Row;
            cellView.transform.localPosition = new Vector3(x * CellMargin, 0, -(z * CellMargin));

            return cellView;
        }
    }
}
