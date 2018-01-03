using Levolution.Reversi.Records;
using System.Linq;
using System;

namespace Levolution.Reversi
{
    public class TableCommands : ITableCommands
    {
        /// <summary>
        /// 
        /// </summary>
        public Table Table { get; }

        /// <summary>
        /// 
        /// </summary>
        public Player CurrentPlayer { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="table"></param>
        /// <param name="firstPlayer"></param>
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
            foreach(var cell in Table.Cells)
            {
                cell.IsPlaceable = false;
            }
        }

        public void MoveDown() => Move(1, 0);

        public void MoveLeft() => Move(0, -1);

        public void MoveRight() => Move(0, 1);

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

        public void Place()
        {
            if (Table.TryPlace(Table.SelectedCell, CurrentPlayer))
            {
                var otherPlayer = CurrentPlayer.GetOtherPlayer();
                CurrentPlayer = Table.GetPlaceableCells(otherPlayer).Any() ? otherPlayer : CurrentPlayer;
                UpdatePlaceableCells(CurrentPlayer);
            }
        }

        public void Select(CellPosition pt)
        {
            Table.SelectedCell = pt;
        }
    }
}
