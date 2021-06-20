using Levolution.Reversi.Records;
using System.Linq;
using Xunit;

namespace Levolution.Reversi.UnitTest
{
    public class TableUnitTest
    {
        [Fact]
        public void CtorTest()
        {
            var table = new Table();
            Assert.Equal(Table.Rows * Table.Columns, table.Cells.Count());
            Assert.Equal(default(CellPosition), table.SelectedCell);

            var allPositions = table.Cells.Select(x => x.Position).ToArray();
            for (var r = 0; r < Table.Rows; r++)
            {
                for (var c = 0; c < Table.Columns; c++)
                {
                    var cp = new CellPosition(r, c);
                    Assert.Contains(cp, table.Cells.Select(x => x.Position));
                }
            }

            Assert.All(table.Cells, x => Assert.Equal(CellState.None, x.State));
        }

        [Fact]
        public void ResetTest()
        {
            var table = new Table();
            table.Reset();

            for (var r = 0; r < Table.Rows; r++)
            {
                for (var c = 0; c < Table.Columns; c++)
                {
                    if (r == 3 && c == 3) { continue; }
                    if (r == 3 && c == 4) { continue; }
                    if (r == 4 && c == 3) { continue; }
                    if (r == 4 && c == 4) { continue; }

                    Assert.Equal(CellState.None, table.GetCell(r, c).State);
                }
            }

            Assert.Equal(CellState.Light, table.GetCell(3, 3).State);
            Assert.Equal(CellState.Light, table.GetCell(4, 4).State);
            Assert.Equal(CellState.Dark, table.GetCell(3, 4).State);
            Assert.Equal(CellState.Dark, table.GetCell(4, 3).State);
        }

        [Theory]
        [InlineData("c4c3f5d6c5c6d3f3d7g5f6e3h4h5h6e6f4g3f2g2g1h1h2h3g4h7e2d2c2f1e7g6g7h8f7g8f8e8d8c8c7c1d1e1b8a8b7a7b1a1b4a4b5b3b2a2a3a5a6b6", 12, 52)]
        [InlineData("c4c5c6e3f4g5f5f6h5e6f3g4d3g6h4g3h3h2h1f2e1f1g1g2d6e2d2b4a4c3b3b5a5d1c1c2b1b2a1a2a3h6h7g7h8g8f8f7d7e7d8b6a6c7e8b7c8b8a8a7", 48, 16)]
        public void ResetRecordsTest(string record, int darkCount, int lightCount)
        {
            var positions = CellPosition.ParseList(record);
            var table = new Table();
            table.Reset(positions);

            Assert.Equal(darkCount, table.Cells.Count(x => x.State == CellState.Dark));
            Assert.Equal(lightCount, table.Cells.Count(x => x.State == CellState.Light));
        }
    }
}
