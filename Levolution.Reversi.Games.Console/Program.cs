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
        for (var r = 0; r < Table.Rows; r++)
        {
            for (var c = 0; c < Table.Columns; c++)
            {
                var cell = table.GetCell(r, c);
                builder.Append(CellStateToChar(cell.State));
            }
            builder.AppendLine();
        }
        return builder;
    }

    private static char CellStateToChar(CellState state) => state switch
    {
        CellState.None => '-',
        CellState.Dark => 'D',
        CellState.Light => 'L',
        _ => default,
    };
}
