using System;

namespace Levolution.Reversi.Records;

/// <summary>
/// Interface for selecting a cell position on the board.
/// </summary>
public interface ISelector
{
    /// <summary>
    /// Select a cell position on the board.
    /// </summary>
    /// <param name="data">The current game state.</param>
    /// <param name="player">The player whose turn it is to make a selection.</param>
    /// <param name="cellPosition">The cell position to select.</param>
    /// <returns>True if a placeable cell is found; otherwise, false.</returns>
    bool TrySelect(Data data, Player player, out CellPosition cellPosition);
}

/// <summary>
/// Randomly selects a cell position on the board.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RandomSelector"/> class with a specified random instance.
/// </remarks>
/// <param name="random">Random instance for generating random numbers.</param>
public class RandomSelector(Random random) : ISelector
{
    private readonly Random _random = random ?? throw new ArgumentNullException(nameof(random));

    /// <summary>
    /// Initializes a new instance of the <see cref="RandomSelector"/> class with a default random instance.
    /// </summary>
    public RandomSelector() : this(new Random()) { }

    /// <inheritdoc/>
    public bool TrySelect(Data data, Player player, out CellPosition cellPosition)
    {
        Span<CellPosition> placeableCells = stackalloc CellPosition[Data.Rows * Data.Columns];
        int placeableCount = data.GetPlaceableCells(player, placeableCells);

        if (placeableCount == 0)
        {
            cellPosition = default;
            return false;
        }

        cellPosition = placeableCells[_random.Next(placeableCount)];
        return true;
    }
}

/// <summary>  
/// Provides a strategy for selecting the optimal cell position on the Reversi board by looking ahead a specified number of moves.  
/// </summary>  
public class StrategicSelector : ISelector
{
    private const int MaxMoves = Data.Rows * Data.Columns - 4; // Maximum moves in Reversi

    /// <summary>  
    /// Gets or sets the number of moves to look ahead.  
    /// </summary>  
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is less than 1 or greater than the maximum number of moves.</exception>
    public int LookAhead
    {
        get => _lookAhead;
        set
        {
            if (value < 1 || value > MaxMoves)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"LookAhead must be between 1 and {MaxMoves}.");
            }
            _lookAhead = value;
        }
    }
    private int _lookAhead = 1;

    /// <inheritdoc/>
    public bool TrySelect(Data data, Player player, out CellPosition cellPosition)
    {
        Span<CellPosition> placeableCells = stackalloc CellPosition[Data.Rows * Data.Columns];
        var placeableCount = data.GetPlaceableCells(player, placeableCells);

        // If no placeable cells are found, return false
        if (placeableCount == 0)
        {
            cellPosition = default;
            return false;
        }

        var maxDifference = int.MinValue;
        cellPosition = placeableCells[0];

        // Iterate through all placeable cells to find the best move
        foreach (var pos in placeableCells[..placeableCount])
        {
            var nextData = data.Place(pos, player);
            var difference = CountStoneDifference(nextData, player, player, LookAhead);

            // Update the best move if a better one is found
            if (difference > maxDifference)
            {
                maxDifference = difference;
                cellPosition = pos;
            }
        }

        return true;
    }

    /// <summary>  
    /// Recursively counts the difference in the number of stones between the two players after a specified number of moves.  
    /// </summary>  
    /// <param name="data">The current game state.</param>  
    /// <param name="currentPlayer">The player whose turn it is to make a selection.</param>  
    /// <param name="ownPlayer">The player whose stones are being counted.</param>
    /// <param name="n">The number of moves to look ahead.</param>  
    /// <returns>The difference in the number of stones between the two players.</returns>  
    private int CountStoneDifference(Data data, Player currentPlayer, Player ownPlayer, int n)
    {
        if (n == 0)
        {
            return ownPlayer == Player.Dark ? data.DarkCount - data.LightCount : data.LightCount - data.DarkCount;
        }

        Span<CellPosition> placeableCells = stackalloc CellPosition[Data.Rows * Data.Columns];
        var placeableCount = data.GetPlaceableCells(currentPlayer, placeableCells);

        // Iterate through all placeable cells to find the best move
        var maxDifference = int.MinValue;
        foreach (var pos in placeableCells[..placeableCount])
        {
            var nextData = data.Place(pos, currentPlayer);
            var difference = CountStoneDifference(nextData, currentPlayer.GetOtherPlayer(), ownPlayer, n - 1);

            // Update the best move if a better one is found
            if (difference > maxDifference)
            {
                maxDifference = difference;
            }
        }
        return maxDifference;
    }
}
