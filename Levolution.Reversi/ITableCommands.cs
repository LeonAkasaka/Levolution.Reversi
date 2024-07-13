using Levolution.Reversi.Records;

namespace Levolution.Reversi;

public interface ITableCommands
{
    void MoveUp();

    void MoveDown();

    void MoveLeft();

    void MoveRight();

    void Select(CellPosition pt);

    void Place();
}