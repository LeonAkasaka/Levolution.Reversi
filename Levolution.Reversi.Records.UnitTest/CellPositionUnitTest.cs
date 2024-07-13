using System;
using Xunit;

namespace Levolution.Reversi.Records.UnitTest;

public class CellPositionUnitTest
{
    [Fact]
    public void ParseTest()
    {
        // invalid data.
        Assert.Throws<ArgumentNullException>(() => CellPosition.Parse(null));
        Assert.Throws<ArgumentException>(() => CellPosition.Parse(""));
        Assert.Throws<ArgumentException>(() => CellPosition.Parse("a"));
        Assert.Throws<ArgumentException>(() => CellPosition.Parse("a1X"));
        Assert.Throws<ArgumentException>(() => CellPosition.Parse("1a"));
        Assert.Throws<ArgumentException>(() => CellPosition.Parse("11"));
        Assert.Throws<ArgumentException>(() => CellPosition.Parse("X1"));
        Assert.Throws<ArgumentException>(() => CellPosition.Parse("XX"));
        Assert.Throws<ArgumentException>(() => CellPosition.Parse("a0"));
        Assert.Throws<ArgumentException>(() => CellPosition.Parse("a9"));
        Assert.Throws<ArgumentException>(() => CellPosition.Parse("i1"));

        var a1 = CellPosition.Parse("a1");
        Assert.Equal(0, a1.Row);
        Assert.Equal(0, a1.Column);

        var a2 = CellPosition.Parse("a2");
        Assert.Equal(1, a2.Row);
        Assert.Equal(0, a2.Column);

        var a3 = CellPosition.Parse("a3");
        Assert.Equal(2, a3.Row);
        Assert.Equal(0, a3.Column);

        var a4 = CellPosition.Parse("a4");
        Assert.Equal(3, a4.Row);
        Assert.Equal(0, a4.Column);

        var a5 = CellPosition.Parse("a5");
        Assert.Equal(4, a5.Row);
        Assert.Equal(0, a5.Column);

        var a6 = CellPosition.Parse("a6");
        Assert.Equal(5, a6.Row);
        Assert.Equal(0, a6.Column);

        var a7 = CellPosition.Parse("a7");
        Assert.Equal(6, a7.Row);
        Assert.Equal(0, a7.Column);

        var a8 = CellPosition.Parse("a8");
        Assert.Equal(7, a8.Row);
        Assert.Equal(0, a8.Column);

        var b1 = CellPosition.Parse("b1");
        Assert.Equal(0, b1.Row);
        Assert.Equal(1, b1.Column);

        var b2 = CellPosition.Parse("b2");
        Assert.Equal(1, b2.Row);
        Assert.Equal(1, b2.Column);

        var b3 = CellPosition.Parse("b3");
        Assert.Equal(2, b3.Row);
        Assert.Equal(1, b3.Column);

        var b4 = CellPosition.Parse("b4");
        Assert.Equal(3, b4.Row);
        Assert.Equal(1, b4.Column);

        var b5 = CellPosition.Parse("b5");
        Assert.Equal(4, b5.Row);
        Assert.Equal(1, b5.Column);

        var b6 = CellPosition.Parse("b6");
        Assert.Equal(5, b6.Row);
        Assert.Equal(1, b6.Column);

        var b7 = CellPosition.Parse("b7");
        Assert.Equal(6, b7.Row);
        Assert.Equal(1, b7.Column);

        var b8 = CellPosition.Parse("b8");
        Assert.Equal(7, b8.Row);
        Assert.Equal(1, b8.Column);

        var c1 = CellPosition.Parse("c1");
        Assert.Equal(0, c1.Row);
        Assert.Equal(2, c1.Column);

        var c2 = CellPosition.Parse("c2");
        Assert.Equal(1, c2.Row);
        Assert.Equal(2, c2.Column);

        var c3 = CellPosition.Parse("c3");
        Assert.Equal(2, c3.Row);
        Assert.Equal(2, c3.Column);

        var c4 = CellPosition.Parse("c4");
        Assert.Equal(3, c4.Row);
        Assert.Equal(2, c4.Column);

        var c5 = CellPosition.Parse("c5");
        Assert.Equal(4, c5.Row);
        Assert.Equal(2, c5.Column);

        var c6 = CellPosition.Parse("c6");
        Assert.Equal(5, c6.Row);
        Assert.Equal(2, c6.Column);

        var c7 = CellPosition.Parse("c7");
        Assert.Equal(6, c7.Row);
        Assert.Equal(2, c7.Column);

        var c8 = CellPosition.Parse("c8");
        Assert.Equal(7, c8.Row);
        Assert.Equal(2, c8.Column);

        var d1 = CellPosition.Parse("d1");
        Assert.Equal(0, d1.Row);
        Assert.Equal(3, d1.Column);

        var d2 = CellPosition.Parse("d2");
        Assert.Equal(1, d2.Row);
        Assert.Equal(3, d2.Column);

        var d3 = CellPosition.Parse("d3");
        Assert.Equal(2, d3.Row);
        Assert.Equal(3, d3.Column);

        var d4 = CellPosition.Parse("d4");
        Assert.Equal(3, d4.Row);
        Assert.Equal(3, d4.Column);

        var d5 = CellPosition.Parse("d5");
        Assert.Equal(4, d5.Row);
        Assert.Equal(3, d5.Column);

        var d6 = CellPosition.Parse("d6");
        Assert.Equal(5, d6.Row);
        Assert.Equal(3, d6.Column);

        var d7 = CellPosition.Parse("d7");
        Assert.Equal(6, d7.Row);
        Assert.Equal(3, d7.Column);

        var d8 = CellPosition.Parse("d8");
        Assert.Equal(7, d8.Row);
        Assert.Equal(3, d8.Column);

        var e1 = CellPosition.Parse("e1");
        Assert.Equal(0, e1.Row);
        Assert.Equal(4, e1.Column);

        var e2 = CellPosition.Parse("e2");
        Assert.Equal(1, e2.Row);
        Assert.Equal(4, e2.Column);

        var e3 = CellPosition.Parse("e3");
        Assert.Equal(2, e3.Row);
        Assert.Equal(4, e3.Column);

        var e4 = CellPosition.Parse("e4");
        Assert.Equal(3, e4.Row);
        Assert.Equal(4, e4.Column);

        var e5 = CellPosition.Parse("e5");
        Assert.Equal(4, e5.Row);
        Assert.Equal(4, e5.Column);

        var e6 = CellPosition.Parse("e6");
        Assert.Equal(5, e6.Row);
        Assert.Equal(4, e6.Column);

        var e7 = CellPosition.Parse("e7");
        Assert.Equal(6, e7.Row);
        Assert.Equal(4, e7.Column);

        var e8 = CellPosition.Parse("e8");
        Assert.Equal(7, e8.Row);
        Assert.Equal(4, e8.Column);

        var f1 = CellPosition.Parse("f1");
        Assert.Equal(0, f1.Row);
        Assert.Equal(5, f1.Column);

        var f2 = CellPosition.Parse("f2");
        Assert.Equal(1, f2.Row);
        Assert.Equal(5, f2.Column);

        var f3 = CellPosition.Parse("f3");
        Assert.Equal(2, f3.Row);
        Assert.Equal(5, f3.Column);

        var f4 = CellPosition.Parse("f4");
        Assert.Equal(3, f4.Row);
        Assert.Equal(5, f4.Column);

        var f5 = CellPosition.Parse("f5");
        Assert.Equal(4, f5.Row);
        Assert.Equal(5, f5.Column);

        var f6 = CellPosition.Parse("f6");
        Assert.Equal(5, f6.Row);
        Assert.Equal(5, f6.Column);

        var f7 = CellPosition.Parse("f7");
        Assert.Equal(6, f7.Row);
        Assert.Equal(5, f7.Column);

        var f8 = CellPosition.Parse("f8");
        Assert.Equal(7, f8.Row);
        Assert.Equal(5, f8.Column);

        var g1 = CellPosition.Parse("g1");
        Assert.Equal(0, g1.Row);
        Assert.Equal(6, g1.Column);

        var g2 = CellPosition.Parse("g2");
        Assert.Equal(1, g2.Row);
        Assert.Equal(6, g2.Column);

        var g3 = CellPosition.Parse("g3");
        Assert.Equal(2, g3.Row);
        Assert.Equal(6, g3.Column);

        var g4 = CellPosition.Parse("g4");
        Assert.Equal(3, g4.Row);
        Assert.Equal(6, g4.Column);

        var g5 = CellPosition.Parse("g5");
        Assert.Equal(4, g5.Row);
        Assert.Equal(6, g5.Column);

        var g6 = CellPosition.Parse("g6");
        Assert.Equal(5, g6.Row);
        Assert.Equal(6, g6.Column);

        var g7 = CellPosition.Parse("g7");
        Assert.Equal(6, g7.Row);
        Assert.Equal(6, g7.Column);

        var g8 = CellPosition.Parse("g8");
        Assert.Equal(7, g8.Row);
        Assert.Equal(6, g8.Column);

        var h1 = CellPosition.Parse("h1");
        Assert.Equal(0, h1.Row);
        Assert.Equal(7, h1.Column);

        var h2 = CellPosition.Parse("h2");
        Assert.Equal(1, h2.Row);
        Assert.Equal(7, h2.Column);

        var h3 = CellPosition.Parse("h3");
        Assert.Equal(2, h3.Row);
        Assert.Equal(7, h3.Column);

        var h4 = CellPosition.Parse("h4");
        Assert.Equal(3, h4.Row);
        Assert.Equal(7, h4.Column);

        var h5 = CellPosition.Parse("h5");
        Assert.Equal(4, h5.Row);
        Assert.Equal(7, h5.Column);

        var h6 = CellPosition.Parse("h6");
        Assert.Equal(5, h6.Row);
        Assert.Equal(7, h6.Column);

        var h7 = CellPosition.Parse("h7");
        Assert.Equal(6, h7.Row);
        Assert.Equal(7, h7.Column);

        var h8 = CellPosition.Parse("h8");
        Assert.Equal(7, h8.Row);
        Assert.Equal(7, h8.Column);
    }
}
