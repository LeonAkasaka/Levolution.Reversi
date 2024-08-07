﻿using Levolution.Reversi.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Levolution.Reversi;

/// <summary>
/// Represents a Reversi game table with an 8x8 grid of cells.
/// </summary>
public class Table : INotifyPropertyChanged
{
    /// <summary>
    /// Number of rows on the table.
    /// </summary>
    public const int Rows = 8;

    /// <summary>
    /// Number of columns on the table.
    /// </summary>
    public const int Columns = 8;

    /// <summary>
    /// Gets or sets the reversi data.
    /// </summary>
    public Data Data
    {
        get
        {
            var data = new Data();
            for (var r = 0; r < Rows; r++)
            {
                for (var c = 0; c < Columns; c++)
                {
                    data[r, c] = _cells[r, c].State;
                }
            }
            return data;
        }
        set
        {
            for (var r = 0; r < Rows; r++)
            {
                for (var c = 0; c < Columns; c++)
                {
                    _cells[r, c].State = value[r, c];
                }
            }
        }
    }

    /// <summary>
    /// Collection of all table cells.
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
    /// Gets or sets the selected cell position.
    /// </summary>
    public CellPosition SelectedCell
    {
        get { return _selectedCell; }
        set
        {
            _selectedCell = value;
            OnSelectedCellChanged(value);
            PropertyChanged?.Invoke(this, _isSelectedCellPropertyChangedEventArgs);
        }
    }
    private CellPosition _selectedCell;
    private readonly PropertyChangedEventArgs _isSelectedCellPropertyChangedEventArgs = new(nameof(SelectedCell));

    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Constructs a new instance of the <see cref="Table"/> class.
    /// </summary>
    public Table()
    {
        for (var r = 0; r < Rows; r++)
        {
            for (var c = 0; c < Columns; c++)
            {
                _cells[r, c] = new TableCell();
            }
        }

        SelectedCell = default;
    }

    /// <summary>
    /// Gets the cell at the specified position.
    /// </summary>
    /// <param name="cellPosition">The position of the cell.</param>
    /// <returns>The cell at the specified position.</returns>
    public TableCell GetCell(CellPosition cellPosition) => _cells[cellPosition.Row, cellPosition.Column];

    /// <summary>
    /// Gets the cell at the specified row and column.
    /// </summary>
    /// <param name="row">The row of the cell.</param>
    /// <param name="column">The column of the cell.</param>
    /// <returns>The cell at the specified row and column.</returns>
    public TableCell GetCell(int row, int column) => _cells[row, column];

    /// <summary>
    /// Tries to get the cell at the specified position.
    /// </summary>
    /// <param name="cellPosition">The position of the cell.</param>
    /// <param name="result">The cell at the specified position, if found.</param>
    /// <returns>True if the cell was found; otherwise, false.</returns>
    public bool TryGetCell(CellPosition cellPosition, out TableCell result) => TryGetCell(cellPosition.Row, cellPosition.Column, out result);

    /// <summary>
    /// Tries to get the cell at the specified row and column.
    /// </summary>
    /// <param name="row">The row of the cell.</param>
    /// <param name="column">The column of the cell.</param>
    /// <param name="result">The cell at the specified row and column, if found.</param>
    /// <returns>True if the cell was found; otherwise, false.</returns>
    public bool TryGetCell(int row, int column, out TableCell result)
    {
        result = default;

        if (row >= 0 && row < Rows && column >= 0 && column < Columns)
        {
            result = GetCell(row, column);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Resets the game to the initial state.
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
    /// Resets the game based on the given records.
    /// </summary>
    /// <param name="records">Algebraic notation of reversi.</param>
    /// <returns>next player.</returns>
    public Player Reset(IEnumerable<CellPosition> records)
    {
        if (records == null) { throw new ArgumentNullException(nameof(records)); }

        Reset();
        if (!records.Any()) { return Player.Dark; }

        var player = Player.Dark;
        foreach (var record in records)
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
    /// Initializes all table cells to <see cref="CellState.None"/>.
    /// </summary>
    public void Clear()
    {
        foreach (var cell in Cells) { cell.State = CellState.None; }
    }

    /// <summary>
    /// Attempts to place a piece for the specified player at the specified position.
    /// </summary>
    /// <param name="pos">The position to place the piece.</param>
    /// <param name="player">The player making the move.</param>
    /// <returns>True if the piece was placed successfully; otherwise, false.</returns>
    public bool TryPlace(CellPosition pos, Player player)
    {
        var r = IsPlaceable(pos, player);
        if (r)
        {
            foreach (var pt in Place(pos, player))
            {
                GetCell(pt).State = player.ToCellState();
            }
        }
        return r;
    }

    /// <summary>
    /// Places a piece for the specified player at the specified position.
    /// </summary>
    /// <param name="pos">The position to place the piece.</param>
    /// <param name="player">The player making the move.</param>
    /// <returns>An enumerable of the positions where pieces were placed or flipped.</returns>
    public IEnumerable<CellPosition> Place(CellPosition pos, Player player)
    {
        return PlaceInternal(pos, player).ToArray();
    }

    /// <summary>
    /// Places a piece for the specified player at the specified row and column.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="column">The column index.</param>
    /// <param name="player">The player making the move.</param>
    /// <returns>An enumerable of the positions where pieces were placed or flipped.</returns>
    public IEnumerable<CellPosition> Place(int row, int column, Player player) => Place(new(row, column), player);

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

    private CellPosition[] GetReversibleCellPositions(CellPosition pos, Player player)
    {
        var state = GetCell(pos).State;
        if (state != CellState.None) { return []; }

        Span<CellPosition> cells = stackalloc CellPosition[Rows * Columns];

        var offset = 0;
        offset = GetReversibleCellPositionsByDirection(pos, -1, -1, player, offset, cells); // left, top
        offset = GetReversibleCellPositionsByDirection(pos, -1, 0, player, offset, cells); // top
        offset = GetReversibleCellPositionsByDirection(pos, -1, 1, player, offset, cells); // right, top
        offset = GetReversibleCellPositionsByDirection(pos, 0, -1, player, offset, cells); // left
        offset = GetReversibleCellPositionsByDirection(pos, 0, 1, player, offset, cells); // right
        offset = GetReversibleCellPositionsByDirection(pos, 1, -1, player, offset, cells); // left, bottom
        offset = GetReversibleCellPositionsByDirection(pos, 1, 0, player, offset, cells); // bottom
        offset = GetReversibleCellPositionsByDirection(pos, 1, 1, player, offset, cells); // right, bottom
        return cells[0..offset].ToArray();
    }

    private int GetReversibleCellPositionsByDirection(CellPosition pos, int dr, int dc, Player player, int offset, Span<CellPosition> result)
    {
        var own = player.ToCellState();
        var other = player.GetOtherPlayer().ToCellState();

        var ncp = new CellPosition(pos.Row + dr, pos.Column + dc);
        if (!TryGetCell(ncp.Row, ncp.Column, out var nCell) || nCell.State != other)
        {
            return offset;
        }

        result[offset] = ncp;
        int count = 1;

        for (var icp = new CellPosition(ncp.Row + dr, ncp.Column + dc);
            icp.Row >= 0 && icp.Column >= 0 &&
            icp.Row < Rows && icp.Column < Columns;
            icp = new CellPosition(icp.Row + dr, icp.Column + dc)
        )
        {
            var cell = GetCell(icp.Row, icp.Column);
            if (cell.State == other) { result[offset + count] = icp; count++; }
            else if (cell.State == own) { return offset + count; }
            else { break; }
        }
        return offset;
    }

    /// <summary>
    /// Get all placeable cells for a given player.
    /// </summary>
    /// <param name="player">The player to check for placeable cells.</param>
    /// <returns>An enumerable of cell positions that are placeable.</returns>
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
    /// Determines if a given position is placeable for a specific player.
    /// </summary>
    /// <param name="pos">The position to check.</param>
    /// <param name="player">The player to check for.</param>
    /// <returns>True if the position is placeable; otherwise, false.</returns>
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
    /// Determines if a given row and column are placeable for a specific player.
    /// </summary>
    /// <param name="row">The row to check.</param>
    /// <param name="column">The column to check.</param>
    /// <param name="player">The player to check for.</param>
    /// <returns>True if the position is placeable; otherwise, false.</returns>
    public bool IsPlaceable(int row, int column, Player player) => IsPlaceable(new(row, column), player);

    private bool IsPlaceableByDirection(CellPosition pos, int dr, int dc, Player player)
    {
        var other = player.GetOtherPlayer().ToCellState();

        var nPos = new CellPosition(pos.Row + dr, pos.Column + dc);
        if (!TryGetCell(nPos, out var cell) || cell.State != other) { return false; }

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

    private void OnSelectedCellChanged(CellPosition cellPosition)
    {
        foreach (var cell in Cells)
        {
            cell.IsSelected = false;
        }

        var newSelectedCell = GetCell(cellPosition);
        newSelectedCell.IsSelected = true;
    }
}
