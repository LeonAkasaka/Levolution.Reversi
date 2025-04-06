using System;

namespace Levolution.Reversi.Records;

/// <summary>
/// Provides methods to run a Reversi game with specified player selectors and record the moves.
/// </summary>
public static class GameRunner
{
    /// <summary>
    /// Runs the game with the specified player selectors.
    /// </summary>
    /// <param name="darkSelector">The selector for the dark player.</param>
    /// <param name="lightSelector">The selector for the light player.</param>
    /// <returns>The final game data after all moves have been played.</returns>
    public static Data Run(ISelector darkSelector, ISelector lightSelector)
        => Run(Data.InitialPlaced, Player.Dark, darkSelector, lightSelector);

    /// <summary>
    /// Runs the game with the specified data and player selectors.
    /// </summary>
    /// <param name="data">The initial game data.</param>
    /// <param name="currentPlayer">The player who will make the first move.</param>
    /// <param name="darkSelector">The selector for the dark player.</param>
    /// <param name="lightSelector">The selector for the light player.</param>
    /// <returns>The final game data after all moves have been played.</returns>
    public static Data Run(Data data, Player currentPlayer, ISelector darkSelector, ISelector lightSelector)
    {
        while (true)
        {
            var currentSelector = currentPlayer == Player.Dark ? darkSelector : lightSelector;
            if (!currentSelector.TrySelect(data, currentPlayer, out CellPosition move))
            {
                // No valid moves for the current player, switch to the other player
                currentPlayer = currentPlayer.GetOtherPlayer();
                currentSelector = currentPlayer == Player.Dark ? darkSelector : lightSelector;
                if (!currentSelector.TrySelect(data, currentPlayer, out move))
                {
                    // No valid moves for both players, game over
                    break;
                }
            }

            // Place the piece and update the game data
            data = data.Place(move, currentPlayer);

            // Switch to the other player
            currentPlayer = currentPlayer.GetOtherPlayer();
        }
        return data;
    }

    /// <summary>
    /// Runs the game with the specified player selectors and records the moves.
    /// </summary>
    /// <param name="darkSelector">The selector for the dark player.</param>
    /// <param name="lightSelector">The selector for the light player.</param>
    /// <param name="records">A span to store the positions of the moves made during the game.</param>
    /// <returns>The number of moves made during the game.</returns>
    public static int Run(ISelector darkSelector, ISelector lightSelector, Span<CellPosition> records)
        => Run(Data.InitialPlaced, Player.Dark, darkSelector, lightSelector, records);

    /// <summary>
    /// Runs the game with the specified data, player selectors, and records the moves.
    /// </summary>
    /// <param name="data">The initial game data.</param>
    /// <param name="currentPlayer">The player who will make the first move.</param>
    /// <param name="darkSelector">The selector for the dark player.</param>
    /// <param name="lightSelector">The selector for the light player.</param>
    /// <param name="records">A span to store the positions of the moves made during the game.</param>
    /// <returns>The number of moves made during the game.</returns>
    public static int Run(Data data, Player currentPlayer, ISelector darkSelector, ISelector lightSelector, Span<CellPosition> records)
    {
        int moveCount = 0;

        while (true)
        {
            var currentSelector = currentPlayer == Player.Dark ? darkSelector : lightSelector;
            if (!currentSelector.TrySelect(data, currentPlayer, out CellPosition move))
            {
                // No valid moves for the current player, switch to the other player
                currentPlayer = currentPlayer.GetOtherPlayer();
                currentSelector = currentPlayer == Player.Dark ? darkSelector : lightSelector;
                if (!currentSelector.TrySelect(data, currentPlayer, out move))
                {
                    // No valid moves for both players, game over
                    break;
                }
            }

            // Place the piece and update the game data
            data = data.Place(move, currentPlayer);

            // Save the move to results
            records[moveCount] = move;
            moveCount++;

            // Switch to the other player
            currentPlayer = currentPlayer.GetOtherPlayer();
        }

        return moveCount;
    }
}
