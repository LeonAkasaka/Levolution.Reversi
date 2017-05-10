﻿using Levolution.Reversi.Records;
using System.Collections.Generic;

namespace Levolution.Reversi
{
    /// <summary>
    /// Reversi table.
    /// </summary>
    public class Table
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
        public IEnumerable<TableCell> Cells => _cells.Values;
        private Dictionary<CellPosition, TableCell> _cells = new Dictionary<CellPosition, TableCell>();

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
                    _cells.Add(pos,  new TableCell(pos));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public TableCell GetCell(CellPosition pos)
        {
            if (_cells.TryGetValue(pos, out TableCell cell))
            {
                return cell;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public TableCell GetCell(int r, int c) => GetCell(new CellPosition(r, c));

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
        public void Reset(IEnumerable<CellPosition> records)
        {
            Reset();
            if (records == null) { return; }

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
            if (r) { Place(pos, player); }
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
            list.AddRange(GetReversibleCellPositionsByDirection(pos, -1, -1, player)); //左上
            list.AddRange(GetReversibleCellPositionsByDirection(pos, -1, 0, player)); //上
            list.AddRange(GetReversibleCellPositionsByDirection(pos, -1, 1, player)); //右上
            list.AddRange(GetReversibleCellPositionsByDirection(pos, 0, -1, player)); //左
            list.AddRange(GetReversibleCellPositionsByDirection(pos, 0, 1, player)); //右
            list.AddRange(GetReversibleCellPositionsByDirection(pos, 1, -1, player)); //左下
            list.AddRange(GetReversibleCellPositionsByDirection(pos, 1, 0, player)); //下
            list.AddRange(GetReversibleCellPositionsByDirection(pos, 1, 1, player)); //右;
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
                IsPlaceableByDirection(pos, -1, -1, player) || //左上
                IsPlaceableByDirection(pos, -1, 0, player) || //上
                IsPlaceableByDirection(pos, -1, 1, player) || //右上
                IsPlaceableByDirection(pos, 0, -1, player) || //左
                IsPlaceableByDirection(pos, 0, 1, player) || //右
                IsPlaceableByDirection(pos, 1, -1, player) || //左下
                IsPlaceableByDirection(pos, 1, 0, player) || //下
                IsPlaceableByDirection(pos, 1, 1, player); //右;
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
    }
}
