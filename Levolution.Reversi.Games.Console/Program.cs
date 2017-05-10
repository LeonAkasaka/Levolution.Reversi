namespace Levolution.Reversi.Games.Console
{
    using System;
    using Levolution.Reversi.Records;
    using Console = System.Console;

    class Program
    {
        static void Main(string[] args)
        {
            var table = new Table();
            table.Reset();

            for(var c = 0; c < Table.Columns; c++)
            {
                for(var r = 0; r < Table.Rows; r++)
                {
                    var cell = table.GetCell(r, c);
                    Console.Write(CellStateToChar(cell.State));
                }
                Console.WriteLine();
            }
        }

        private static char CellStateToChar(CellState state)
        {
            switch (state)
            {
                case CellState.None: return '-';
                case CellState.Dark: return 'D';
                case CellState.Light: return 'L';
            }

            return default(char);
        }
    }
}
