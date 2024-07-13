using Levolution.Reversi.Records;
using System.Linq;

namespace Levolution.Reversi;

/// <summary>
/// Implements a <see cref="ITableCommands"/>.
/// </summary>
public class TableCommands : ITableCommands
{
    /// <summary>
    /// The table to be operation.
    /// </summary>
    public Table Table { get; }

    /// <summary>
    /// The player who is currently operating the table.
    /// </summary>
    public Player CurrentPlayer { get; private set; }

    /// <summary>
    /// Constructs a new instance of the <paramref name="TableCommands"/>.
    /// </summary>
    /// <param name="table">The table to be operation.</param>
    /// <param name="firstPlayer">First player. Default is <see cref="Player.Dark"/>.</param>
    public TableCommands(Table table, Player firstPlayer = Player.Dark)
    {
        Table = table;
        CurrentPlayer = firstPlayer;
        UpdatePlaceableCells(firstPlayer);
    }

    private void UpdatePlaceableCells(Player player)
    {
        ResetPlaceableCells();
        foreach (var pt in Table.GetPlaceableCells(player))
        {
            Table.GetCell(pt).IsPlaceable = true;
        }
    }

    private void ResetPlaceableCells()
    {
        foreach (var cell in Table.Cells)
        {
            cell.IsPlaceable = false;
        }
    }

    /// <summary>
    /// <inheritdoc cref="ITableCommands.MoveDown"/>
    /// </summary>
    public void MoveDown() => Move(1, 0);

    /// <summary>
    /// <inheritdoc cref="ITableCommands.MoveLeft"/>
    /// </summary>
    public void MoveLeft() => Move(0, -1);

    /// <summary>
    /// <inheritdoc cref="ITableCommands.MoveRight"/>
    /// </summary>
    public void MoveRight() => Move(0, 1);

    /// <summary>
    /// <inheritdoc cref="ITableCommands.MoveUp"/>
    /// </summary>
    public void MoveUp() => Move(-1, 0);

    private void Move(int row, int column)
    {
        var oldPt = Table.SelectedCell;
        var newPt = new CellPosition(oldPt.Row + row, oldPt.Column + column);
        if (newPt.Row >= 0 && newPt.Row < Table.Rows && newPt.Column >= 0 && newPt.Column < Table.Columns)
        {
            Table.SelectedCell = newPt;
        }
    }

    /// <summary>
    /// <inheritdoc cref="ITableCommands.Select"/>
    /// </summary>
    public void Select(CellPosition pt)
    {
        Table.SelectedCell = pt;
    }

    /// <summary>
    /// <inheritdoc cref="ITableCommands.Place"/>
    /// </summary>
    public void Place()
    {
        if (Table.TryPlace(Table.SelectedCell, CurrentPlayer))
        {
            var otherPlayer = CurrentPlayer.GetOtherPlayer();
            CurrentPlayer = Table.GetPlaceableCells(otherPlayer).Any() ? otherPlayer : CurrentPlayer;
            UpdatePlaceableCells(CurrentPlayer);
        }
    }
}
