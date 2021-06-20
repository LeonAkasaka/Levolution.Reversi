using Levolution.Reversi.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Levolution.Reversi
{
    /// <summary>
    /// Reversi table.
    /// </summary>
    public class Table : INotifyPropertyChanged
    {
        /// <summary>
        /// Rows.
        /// </summary>
        public const int Rows = 8;

        /// <summary>
        /// Columns.
        /// </summary>
        public const int Columns = 8;

        /// <summary>
        /// Cell states.
        /// </summary>
        public IEnumerable<TableCell> Cells
        {
            get
            {
                for (var r = 0; r < Rows; r++)
                {
                    for (var c = 0; c < Columns; c++)
                    {
                        yield return _cells[r, c];
                    }
                }
            }
        }
        private readonly TableCell[,] _cells = new TableCell[Rows, Columns];

        /// <summary>
        /// Gets selected cell.
        /// </summary>
        public CellPosition SelectedCell
        {
            get { return _selectedCell; }
            set
            {
                _selectedCell = value;
                OnSelectedCellChanged();
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, _isSelectedCellPropertyChangedEventArgs);
                }
            }
        }
        private CellPosition _selectedCell;
        private PropertyChangedEventArgs _isSelectedCellPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(SelectedCell));

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        public Table()
        {
            for (var r = 0; r < Rows; r++)
            {
                for (var c = 0; c < Columns; c++)
                {
                    var pos = new CellPosition(r, c);
                    _cells[r, c] = new TableCell(pos);
                }
            }

            SelectedCell = default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellPosition"></param>
        /// <returns></returns>
        public TableCell GetCell(CellPosition cellPosition) => _cells[cellPosition.Row, cellPosition.Column];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public TableCell GetCell(int row, int column) => _cells[row, column];

        /// <summary>
        /// Reset game.
        /// </summary>
        public void Reset()
        {
            Clear();

            GetCell(3, 3).State = CellState.Light;
            GetCell(3, 4).State = CellState.Dark;
            GetCell(4, 3).State = CellState.Dark;
            GetCell(4, 4).State = CellState.Light;
        }

        /// <summary>
        /// Reset by game's record (Algebraic notation).
        /// </summary>
        /// <param name="records">Algebraic notation of reversi.</param>
        /// <returns>next player.</returns>
        public Player Reset(IEnumerable<CellPosition> records)
        {   
            if (records == null) { throw new ArgumentNullException(nameof(records)); }

            Reset();
            if (!records.Any()) { return Player.Dark; }

            var player = Player.Dark;
            foreach(var record in records)
            {
                if (TryPlace(record, player)) { player = player.GetOtherPlayer(); }
                else
                {
                    var other = player.GetOtherPlayer();
                    if (!TryPlace(record, other)) { break; } // Game over.
                }
            }
            return player;
        }

        /// <summary>
        /// All table cells initialize as  <see cref="CellState.None"/>.
        /// </summary>
        public void Clear()
        {
            foreach(var cell in Cells) { cell.State = CellState.None; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool TryPlace(CellPosition pos, Player player)
        {
            var r = IsPlaceable(pos, player);
            if (r)
            {
                foreach(var pt in Place(pos, player))
                {
                    GetCell(pt).State = player.ToCellState();
                }
            }
            return r;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public IEnumerable<CellPosition> Place(CellPosition pos, Player player)
        {
            return PlaceInternal(pos, player).ToArray();
        }

        private IEnumerable<CellPosition> PlaceInternal(CellPosition pos, Player player)
        {
            var cellPositions = GetReversibleCellPositions(pos, player);

            var pc = player.ToCellState();
            GetCell(pos).State = pc;
            yield return pos;
            foreach (var pt in cellPositions)
            {
                GetCell(pt).State = pc;
                yield return pos;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public IEnumerable<CellPosition> Place(int row, int column, Player player) => Place(new CellPosition(row, column), player);

        private IEnumerable<CellPosition> GetReversibleCellPositions(CellPosition pos, Player player)
        {
            var state = GetCell(pos).State;
            if (state != CellState.None) { return new CellPosition[0]; }

            var list = new List<CellPosition>();
            list.AddRange(GetReversibleCellPositionsByDirection(pos, -1, -1, player)); // loft, top
            list.AddRange(GetReversibleCellPositionsByDirection(pos, -1, 0, player)); // top
            list.AddRange(GetReversibleCellPositionsByDirection(pos, -1, 1, player)); // right, top
            list.AddRange(GetReversibleCellPositionsByDirection(pos, 0, -1, player)); // left
            list.AddRange(GetReversibleCellPositionsByDirection(pos, 0, 1, player)); // right
            list.AddRange(GetReversibleCellPositionsByDirection(pos, 1, -1, player)); // left, bottom
            list.AddRange(GetReversibleCellPositionsByDirection(pos, 1, 0, player)); // bottom
            list.AddRange(GetReversibleCellPositionsByDirection(pos, 1, 1, player)); // right
            return list;
        }
        private IEnumerable<CellPosition> GetReversibleCellPositionsByDirection(CellPosition pos, int dr, int dc, Player player)
        {
            var own = player.ToCellState();
            var other = player.GetOtherPlayer().ToCellState();

            var ncp = new CellPosition(pos.Row + dr, pos.Column + dc);
            if ((GetCell(ncp.Row, ncp.Column)?.State ?? CellState.None) != other)
            {
                return new CellPosition[0];
            }

            var list = new List<CellPosition>() { ncp };
            for (var icp = new CellPosition(ncp.Row + dr, ncp.Column + dc);
                icp.Row >= 0 && icp.Column >= 0 &&
                icp.Row < Rows && icp.Column < Columns;
                icp = new CellPosition(icp.Row + dr, icp.Column + dc)
            )
            {
                var cell = GetCell(icp.Row, icp.Column)?.State ?? CellState.None;
                if (cell == other) { list.Add(icp); }
                else if (cell == own) { return list; }
                else { break; }
            }
            return new CellPosition[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public IEnumerable<CellPosition> GetPlaceableCells(Player player)
        {
            for (var r = 0; r < Rows; r++)
            {
                for (var c = 0; c < Columns; c++)
                {
                    if (IsPlaceable(r, c, player)) { yield return new CellPosition(r, c); }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsPlaceable(CellPosition pos, Player player)
        {

            var state = GetCell(pos)?.State ?? CellState.None;
            if (state != CellState.None) { return false; }

            return
                IsPlaceableByDirection(pos, -1, -1, player) || // left, top
                IsPlaceableByDirection(pos, -1, 0, player) || // top
                IsPlaceableByDirection(pos, -1, 1, player) || // right, top
                IsPlaceableByDirection(pos, 0, -1, player) || // left
                IsPlaceableByDirection(pos, 0, 1, player) || // right
                IsPlaceableByDirection(pos, 1, -1, player) || // left, bottom
                IsPlaceableByDirection(pos, 1, 0, player) || // bottom
                IsPlaceableByDirection(pos, 1, 1, player); // right;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsPlaceable(int row, int column, Player player) => IsPlaceable(new CellPosition(row, column), player);

        private bool IsPlaceableByDirection(CellPosition pos, int dr, int dc, Player player)
        {
            var other = player.GetOtherPlayer().ToCellState();

            var nPos = new CellPosition(pos.Row + dr, pos.Column + dc);
            if ((GetCell(nPos)?.State ?? CellState.None) != other) { return false; }

            for (
                var iPos = new CellPosition(nPos.Row + dr, nPos.Column + dc);
                iPos.Row >= 0 && iPos.Column >= 0 && iPos.Row < Rows && iPos.Column < Columns;
                iPos = new CellPosition(iPos.Row + dr, iPos.Column + dc)
            )
            {
                var s = GetCell(iPos)?.State ?? CellState.None;
                if (s != other) { return s != CellState.None; }
            }

            return false;
        }
 
        private void OnSelectedCellChanged()
        {
            var oldSelectedCell = Cells.FirstOrDefault(x => x.IsSelected);
            if (oldSelectedCell != null)
            {
                oldSelectedCell.IsSelected = false;
            }

            var newSelectedCell = Cells.First(x => x.Position == SelectedCell);
            newSelectedCell.IsSelected = true;
        }
    }
}
