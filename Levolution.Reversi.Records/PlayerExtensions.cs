namespace Levolution.Reversi.Records;

/// <summary>
/// Provide extension methods for <see cref="Player"/>.
/// </summary>
public static class PlayerExtensions
{
    /// <summary>
    /// If player is dark returns light.
    /// otherwise dark.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static Player GetOtherPlayer(this Player player) => player == Player.Dark ? Player.Light : Player.Dark;

    /// <summary>
    /// <see cref="Player"/> to <see cref="CellState"/>.
    /// </summary>
    /// <param name="player">Player disc color.</param>
    /// <returns><see cref="CellState"/>./returns>
    public static CellState ToCellState(this Player player) => (CellState)player;
}
