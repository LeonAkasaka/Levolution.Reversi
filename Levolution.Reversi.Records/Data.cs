using System;

namespace Levolution.Reversi.Records;

/// <summary>
/// 
/// </summary>
public struct Data : IEquatable<Data>
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
    /// Gets the initial placed data.
    /// </summary>
    public static Data InitialPlaced
    {
        get
        {
            var data = new Data
            {
                _dark = 34628173824,
                _light = 68853694464
            };

            //data[3, 4] = CellState.Dark;
            //data[4, 3] = CellState.Dark;
            //data[3, 3] = CellState.Light;
            //data[4, 4] = CellState.Light;

            return data;
        }
    }

    /// <summary>
    /// Resets the game based on the given records.
    /// </summary>
    /// <param name="records">The sequence of cell positions representing the game moves.</param>
    /// <param name="data">The output parameter that will hold the state of the game after processing the records.</param>
    /// <returns>The next player.</returns>
    public static Player Play(ReadOnlySpan<CellPosition> records, out Data data) => Play(InitialPlaced, records, out data);

    /// <summary>
    /// Resets the game based on the given records starting from a specified source state..
    /// </summary>
    /// <param name="source">The initial state of the game from which to start processing the records.</param>
    /// <param name="records">The sequence of cell positions representing the game moves.</param>
    /// <param name="data">The output parameter that will hold the state of the game after processing the records.</param>
    /// <returns>The next player.</returns>
    public static Player Play(in Data source, ReadOnlySpan<CellPosition> records, out Data data)
    {
        data = source;
        var player = Player.Dark;
        foreach (var pos in records)
        {
            if (!data.IsPlaceable(pos, player))
            {
                player = player.GetOtherPlayer();
                if (!data.IsPlaceable(pos, player)) { break; }
            }

            data = data.Place(pos, player);
            player = player.GetOtherPlayer();
        }
        return player;
    }

    private ulong _dark;
    private ulong _light;

    /// <summary>
    /// Gets the nember of dark cells.
    /// </summary>
    public readonly int DarkCount => PopCount(_dark);

    /// <summary>
    /// Gets the nember of light cells.
    /// </summary>
    public readonly int LightCount => PopCount(_light);

    private static int PopCount(ulong x) // todo: System.Numerics.BitOperations
    {
        int count = 0;
        while (x != 0)
        {
            count += (int)(x & 1);
            x >>= 1;
        }
        return count;
    }

    /// <summary>
    /// Gets the cell at the specified position.
    /// </summary>
    /// <param name="cellPosition">The position of the cell.</param>
    /// <returns>The cell at the specified position.</returns>
    public CellState this[CellPosition cellPosition]
    {
        readonly get => this[cellPosition.Row, cellPosition.Column];
        set => this[cellPosition.Row, cellPosition.Column] = value;
    }

    /// <summary>
    /// Gets the cell at the specified row and column.
    /// </summary>
    /// <param name="row">The row of the cell.</param>
    /// <param name="column">The column of the cell.</param>
    /// <returns>The cell at the specified row and column.</returns>
    public CellState this[int row, int column]
    {
        readonly get
        {
            if (row < 0 || row >= Rows) { throw new ArgumentOutOfRangeException(nameof(row)); }
            if (column < 0 || column >= Columns) { throw new ArgumentOutOfRangeException(nameof(column)); }

            var index = column + (row * Columns);
            return (_dark >> index & 1) != 0 ? CellState.Dark
                : (_light >> index & 1) != 0 ? CellState.Light
                : CellState.None;
        }
        set
        {
            if (row < 0 || row >= Rows) { throw new ArgumentOutOfRangeException(nameof(row)); }
            if (column < 0 || column >= Columns) { throw new ArgumentOutOfRangeException(nameof(column)); }

            var index = column + (row * Columns);
            if (value == CellState.Dark)
            {
                _dark |= (1UL << index);
                _light &= ~(1UL << index);
            }
            else if (value == CellState.Light)
            {
                _dark &= ~(1UL << index);
                _light |= (1UL << index);
            }
            else
            {
                _dark &= ~(1UL << index);
                _light &= ~(1UL << index);
            }
        }
    }

    /// <summary>
    /// Tries to get the cell at the specified position.
    /// </summary>
    /// <param name="cellPosition">The position of the cell.</param>
    /// <param name="cellState">The cell at the specified position, if found.</param>
    /// <returns>True if the cell was found; otherwise, false.</returns>
    public readonly bool TryGetCell(CellPosition cellPosition, out CellState cellState) => TryGetCell(cellPosition.Row, cellPosition.Column, out cellState);

    /// <summary>
    /// Tries to get the cell at the specified row and column.
    /// </summary>
    /// <param name="row">The row of the cell.</param>
    /// <param name="column">The column of the cell.</param>
    /// <param name="cellState">The cell at the specified row and column, if found.</param>
    /// <returns>True if the cell was found; otherwise, false.</returns>
    public readonly bool TryGetCell(int row, int column, out CellState cellState)
    {
        if (row < 0 || row >= Rows || column < 0 || column >= Columns)
        {
            cellState = default;
            return false;
        }

        cellState = this[row, column];
        return true;
    }

    /// <summary>
    /// Determines if a given row and column are placeable for a specific player.
    /// </summary>
    /// <param name="row">The row to check.</param>
    /// <param name="column">The column to check.</param>
    /// <param name="player">The player to check for.</param>
    /// <returns>True if the position is placeable; otherwise, false.</returns>
    public readonly bool IsPlaceable(int row, int column, Player player) => IsPlaceable(new(row, column), player);


    /// <summary>
    /// Determines if a given position is placeable for a specific player.
    /// </summary>
    /// <param name="pos">The position to check.</param>
    /// <param name="player">The player to check for.</param>
    /// <returns>True if the position is placeable; otherwise, false.</returns>
    public readonly bool IsPlaceable(CellPosition pos, Player player)
    {
        if (this[pos] != CellState.None) { return false; }

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

    private readonly bool IsPlaceableByDirection(CellPosition pos, int dr, int dc, Player player)
    {
        var other = player.GetOtherPlayer().ToCellState();
        var nPos = new CellPosition(pos.Row + dr, pos.Column + dc);
        if (!TryGetCell(nPos, out var cell) || cell != other) { return false; }

        for (
            var iPos = new CellPosition(nPos.Row + dr, nPos.Column + dc);
            iPos.Row >= 0 && iPos.Column >= 0 && iPos.Row < Rows && iPos.Column < Columns;
            iPos = new CellPosition(iPos.Row + dr, iPos.Column + dc)
        )
        {
            var s = this[iPos];
            if (s != other) { return s != CellState.None; }
        }
        return false;
    }


    /// <summary>
    /// Places a piece for the specified player at the specified row and column.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="column">The column index.</param>
    /// <param name="player">The player making the move.</param>
    /// <returns>An next data of the positions where pieces were placed or flipped.</returns>
    public readonly Data Place(int row, int column, Player player) => Place(new(row, column), player);

    /// <summary>
    /// Places a piece for the specified player at the specified position.
    /// </summary>
    /// <param name="pos">The position to place the piece.</param>
    /// <param name="player">The player making the move.</param>
    /// <returns>An next data of the positions where pieces were placed or flipped.</returns>
    public readonly Data Place(CellPosition pos, Player player)
    {
        var cells = GetReversibleCells(pos, player);
        var index = pos.Column + (pos.Row * Columns);
        cells |= (1UL << index);

        var data = this;
        if (player == Player.Dark)
        {
            data._dark |= cells;
            data._light &= ~cells;
        }
        else
        {
            data._light |= cells;
            data._dark &= ~cells;
        }
        return data;
    }

    private readonly ulong GetReversibleCells(CellPosition pos, Player player)
    {
        var state = this[pos];
        if (state != CellState.None) { return 0; }

        var result = 0UL;
        result |= GetReversibleCellsByDirection(pos, -1, -1, player); // left, top
        result |= GetReversibleCellsByDirection(pos, -1, 0, player); // top
        result |= GetReversibleCellsByDirection(pos, -1, 1, player); // right, top
        result |= GetReversibleCellsByDirection(pos, 0, -1, player); // left
        result |= GetReversibleCellsByDirection(pos, 0, 1, player); // right
        result |= GetReversibleCellsByDirection(pos, 1, -1, player); // left, bottom
        result |= GetReversibleCellsByDirection(pos, 1, 0, player); // bottom
        result |= GetReversibleCellsByDirection(pos, 1, 1, player); // right, bottom
        return result;
    }

    private readonly ulong GetReversibleCellsByDirection(CellPosition pos, int dr, int dc, Player player)
    {
        var own = player.ToCellState();
        var other = player.GetOtherPlayer().ToCellState();

        var ncp = new CellPosition(pos.Row + dr, pos.Column + dc);
        if (!TryGetCell(ncp.Row, ncp.Column, out var nCell) || nCell != other)
        {
            return 0;
        }

        var index = ncp.Column + (ncp.Row * Columns);
        var result = (1UL << index);
        for (var icp = new CellPosition(ncp.Row + dr, ncp.Column + dc);
            icp.Row >= 0 && icp.Column >= 0 &&
            icp.Row < Rows && icp.Column < Columns;
            icp = new CellPosition(icp.Row + dr, icp.Column + dc)
        )
        {
            var cell = this[icp.Row, icp.Column];
            if (cell == other)
            {
                index = icp.Column + (icp.Row * Columns);
                result |= (1UL << index);
            }
            else if (cell == own) { return result; }
            else { break; }
        }
        return 0;
    }

    public readonly override bool Equals(object obj) => obj is Data other && Equals(other);

    public readonly bool Equals(Data other) => _dark == other._dark && _light == other._light;

    public readonly override int GetHashCode() => HashCode.Combine(_dark, _light);

    public static bool operator ==(Data left, Data right) => left.Equals(right);

    public static bool operator !=(Data left, Data right) => !(left == right);
}
