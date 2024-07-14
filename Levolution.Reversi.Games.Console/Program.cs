using Levolution.Reversi.Records;
using System.Text;

namespace Levolution.Reversi.Games.Console;

using Console = System.Console;

class Program
{
    static void Main(string[] _)
    {
        var data = Data.InitialPlaced
            .Place(3, 2, Player.Dark)
            .Place(2, 2, Player.Light)
            .Place(4, 5, Player.Dark)
            .Place(3, 5, Player.Light)
            .Place(2, 5, Player.Dark)
            .Place(3, 6, Player.Light)
            .Place(3, 7, Player.Dark);

        Console.WriteLine(ToString(data));
    }

    private static string ToString(Data data) => ToString(data, new()).ToString();

    private static StringBuilder ToString(in Data data, StringBuilder builder)
    {
        for (var r = 0; r < Data.Rows; r++)
        {
            for (var c = 0; c < Data.Columns; c++)
            {
                var cell = data[r, c];
                builder.Append(CellStateToChar(cell));
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
