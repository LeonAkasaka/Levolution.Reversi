using Levolution.Reversi.Records;
using System.Linq;

namespace Levolution.Reversi.Games
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
        public TableCommands(Table table, Player firstPlayer)
        {
            Table = table;
            CurrentPlayer = firstPlayer;
        }

        public void MoveDown()
        {
        }

        public void MoveLeft()
        {
        }

        public void MoveRight()
        {
        }

        public void MoveUp()
        {
        }

        public void Place(CellPosition pt)
        {
            Table.Place(pt, CurrentPlayer);

            var otherPlayer = CurrentPlayer.GetOtherPlayer();
            CurrentPlayer = Table.GetPlaceableCells(otherPlayer).Any() ? otherPlayer : CurrentPlayer;
        }

        public void Select(CellPosition pt)
        {
            Table.SelectedCell = pt;
        }
    }
}
