using Levolution.Reversi.Records;
using System.Text;

namespace Levolution.Reversi.Games.Console;

using Console = System.Console;

class Program
{
    static void Main(string[] _)
    {
        var table = new Table();
        table.Reset();

        Console.WriteLine(ToString(table));
    }

    private static string ToString(Table table) => ToString(table, new()).ToString();

    private static StringBuilder ToString(Table table, StringBuilder builder)
    {
        for (var c = 0; c < Table.Columns; c++)
        {
            for (var r = 0; r < Table.Rows; r++)
            {
                var cell = table.GetCell(r, c);
                builder.Append(CellStateToChar(cell.State));
            }
            builder.AppendLine();
        }
        return builder;
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
