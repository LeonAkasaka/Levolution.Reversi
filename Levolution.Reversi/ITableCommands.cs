using Levolution.Reversi.Records;

namespace Levolution.Reversi;

/// <summary>
/// Exposes the operations for a <see cref="Table"/>.
/// </summary>
public interface ITableCommands
{
    /// <summary>
    /// Move the selected cell up.
    /// </summary>
    void MoveUp();

    /// <summary>
    /// Move the selected cell down.
    /// </summary>
    void MoveDown();

    /// <summary>
    /// Move the selected cell to the left.
    /// </summary>
    void MoveLeft();

    /// <summary>
    /// Move the selected cell to the right.
    /// </summary>
    void MoveRight();

    /// <summary>
    /// Select the cell at the specified coordinates.
    /// </summary>
    void Select(CellPosition pt);

    /// <summary>
    /// Place a stone on the selected cell.
    /// </summary>
    void Place();
}