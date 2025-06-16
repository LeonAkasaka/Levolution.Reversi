using System;
using System.Linq;
using Xunit;

namespace Levolution.Reversi.Records.UnitTest;

/// <summary>
/// Unit tests for verifying the behavior of the Data class.
/// </summary>
public class DataUnitTest
{
    /// <summary>
    /// Verifies that the Data constructor initializes all cells to None.
    /// </summary>
    [Fact]
    public void CtorTest()
    {
        var data = new Data();
        for (var r = 0; r < Data.Rows; r++)
        {
            for (var c = 0; c < Data.Columns; c++)
            {
                Assert.Equal(CellState.None, data[r, c]);
            }
        }
    }

    /// <summary>
    /// Verifies that the Data.InitialPlaced property creates the correct initial board setup.
    /// </summary>
    [Fact]
    public void InitialPlacedTest()
    {
        var data = Data.InitialPlaced;

        Assert.Equal(CellState.None, data[0, 0]);
        Assert.Equal(CellState.None, data[0, 1]);
        Assert.Equal(CellState.None, data[0, 2]);
        Assert.Equal(CellState.None, data[0, 3]);
        Assert.Equal(CellState.None, data[0, 4]);
        Assert.Equal(CellState.None, data[0, 5]);
        Assert.Equal(CellState.None, data[0, 6]);
        Assert.Equal(CellState.None, data[0, 7]);

        Assert.Equal(CellState.None, data[1, 0]);
        Assert.Equal(CellState.None, data[1, 1]);
        Assert.Equal(CellState.None, data[1, 2]);
        Assert.Equal(CellState.None, data[1, 3]);
        Assert.Equal(CellState.None, data[1, 4]);
        Assert.Equal(CellState.None, data[1, 5]);
        Assert.Equal(CellState.None, data[1, 6]);
        Assert.Equal(CellState.None, data[1, 7]);

        Assert.Equal(CellState.None, data[2, 0]);
        Assert.Equal(CellState.None, data[2, 1]);
        Assert.Equal(CellState.None, data[2, 2]);
        Assert.Equal(CellState.None, data[2, 3]);
        Assert.Equal(CellState.None, data[2, 4]);
        Assert.Equal(CellState.None, data[2, 5]);
        Assert.Equal(CellState.None, data[2, 6]);
        Assert.Equal(CellState.None, data[2, 7]);

        Assert.Equal(CellState.None, data[3, 0]);
        Assert.Equal(CellState.None, data[3, 1]);
        Assert.Equal(CellState.None, data[3, 2]);
        Assert.Equal(CellState.Light, data[3, 3]);
        Assert.Equal(CellState.Dark, data[3, 4]);
        Assert.Equal(CellState.None, data[3, 5]);
        Assert.Equal(CellState.None, data[3, 6]);
        Assert.Equal(CellState.None, data[3, 7]);

        Assert.Equal(CellState.None, data[4, 0]);
        Assert.Equal(CellState.None, data[4, 1]);
        Assert.Equal(CellState.None, data[4, 2]);
        Assert.Equal(CellState.Dark, data[4, 3]);
        Assert.Equal(CellState.Light, data[4, 4]);
        Assert.Equal(CellState.None, data[4, 5]);
        Assert.Equal(CellState.None, data[4, 6]);
        Assert.Equal(CellState.None, data[4, 7]);

        Assert.Equal(CellState.None, data[5, 0]);
        Assert.Equal(CellState.None, data[5, 1]);
        Assert.Equal(CellState.None, data[5, 2]);
        Assert.Equal(CellState.None, data[5, 3]);
        Assert.Equal(CellState.None, data[5, 4]);
        Assert.Equal(CellState.None, data[5, 5]);
        Assert.Equal(CellState.None, data[5, 6]);
        Assert.Equal(CellState.None, data[5, 7]);

        Assert.Equal(CellState.None, data[6, 0]);
        Assert.Equal(CellState.None, data[6, 1]);
        Assert.Equal(CellState.None, data[6, 2]);
        Assert.Equal(CellState.None, data[6, 3]);
        Assert.Equal(CellState.None, data[6, 4]);
        Assert.Equal(CellState.None, data[6, 5]);
        Assert.Equal(CellState.None, data[6, 6]);
        Assert.Equal(CellState.None, data[6, 7]);

        Assert.Equal(CellState.None, data[7, 0]);
        Assert.Equal(CellState.None, data[7, 1]);
        Assert.Equal(CellState.None, data[7, 2]);
        Assert.Equal(CellState.None, data[7, 3]);
        Assert.Equal(CellState.None, data[7, 4]);
        Assert.Equal(CellState.None, data[7, 5]);
        Assert.Equal(CellState.None, data[7, 6]);
        Assert.Equal(CellState.None, data[7, 7]);
    }

    /// <summary>
    /// Verifies that the indexer can set and get cell values.
    /// </summary>
    [Theory]
    [InlineData(0, 0)]
    [InlineData(Data.Rows - 1, Data.Columns - 1)]
    public void Indexer_Test(int row, int column)
    {
        var data = new Data();

        data[row, column] = CellState.Dark;
        Assert.Equal(CellState.Dark, data[row, column]);

        data[row, column] = CellState.Light;
        Assert.Equal(CellState.Light, data[row, column]);

        data[row, column] = CellState.None;
        Assert.Equal(CellState.None, data[row, column]);
    }

    /// <summary>
    /// Verifies that the indexer throws ArgumentOutOfRangeException when accessing out-of-range indices.
    /// </summary>
    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(Data.Rows, 0)]
    [InlineData(0, Data.Columns)]
    public void IndexerExceptionTest(int row, int column)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var data = new Data();
            data[row, column] = CellState.None;
        });
    }

    /// <summary>
    /// Verifies the board state changes when placing a piece using the Place method.
    /// </summary>
    [Fact]
    public void PlaceTest()
    {
        var data = Data.InitialPlaced;
        data = data.Place(3, 2, Player.Dark);
        Assert.Equal(CellState.Dark, data[3, 2]);
        Assert.Equal(CellState.Dark, data[3, 3]);
        Assert.Equal(CellState.Dark, data[3, 4]);
        Assert.Equal(CellState.Dark, data[4, 3]);
        Assert.Equal(CellState.Light, data[4, 4]);

        data = data.Place(2, 2, Player.Light);
        Assert.Equal(CellState.Light, data[2, 2]);
        Assert.Equal(CellState.Dark, data[3, 2]);
        Assert.Equal(CellState.Light, data[3, 3]);
        Assert.Equal(CellState.Dark, data[3, 4]);
        Assert.Equal(CellState.Dark, data[4, 3]);
        Assert.Equal(CellState.Light, data[4, 4]);
    }

    /// <summary>
    /// Verifies the number of pieces after replaying a record using the Play method.
    /// </summary>
    [Theory]
    [InlineData("c4c3f5d6c5c6d3f3d7g5f6e3h4h5h6e6f4g3f2g2g1h1h2h3g4h7e2d2c2f1e7g6g7h8f7g8f8e8d8c8c7c1d1e1b8a8b7a7b1a1b4a4b5b3b2a2a3a5a6b6", 12, 52)]
    [InlineData("c4c5c6e3f4g5f5f6h5e6f3g4d3g6h4g3h3h2h1f2e1f1g1g2d6e2d2b4a4c3b3b5a5d1c1c2b1b2a1a2a3h6h7g7h8g8f8f7d7e7d8b6a6c7e8b7c8b8a8a7", 48, 16)]
    public void PlayTest(string record, int darkCount, int lightCount)
    {
        var records = CellPosition.ParseList(record).ToArray();
        Data.Play(records, out var data);

        Assert.Equal(darkCount, data.DarkCount);
        Assert.Equal(lightCount, data.LightCount);
    }

    /// <summary>
    /// Verifies the equality members and operators of the Data struct.
    /// </summary>
    [Fact]
    public void EqualityTest()
    {
        var data1 = new Data();
        var data2 = new Data();
        var data3 = Data.InitialPlaced;
        var data4 = Data.InitialPlaced;
        
        // Same content: Equals, ==, !=
        Assert.True(data1.Equals(data2));
        Assert.True(data1 == data2);
        Assert.False(data1 != data2);
        Assert.True(data3.Equals(data4));
        Assert.True(data3 == data4);
        Assert.False(data3 != data4);

        // Different content: Equals, ==, !=
        Assert.False(data1.Equals(data3));
        Assert.False(data1 == data3);
        Assert.True(data1 != data3);

        // Equals(object) with different type
        Assert.False(data1.Equals("not a Data"));
    }

    /// <summary>
    /// Verifies the GetHashCode implementation of the Data struct.
    /// </summary>
    [Fact]
    public void GetHashCodeTest()
    {
        var data1 = new Data();
        var data2 = new Data();
        var data3 = Data.InitialPlaced;
        var data4 = Data.InitialPlaced;

        // Same content: hash codes should be equal
        Assert.Equal(data1.GetHashCode(), data2.GetHashCode());
        Assert.Equal(data3.GetHashCode(), data4.GetHashCode());

        // Different content: hash codes should be different (not guaranteed, but likely)
        Assert.NotEqual(data1.GetHashCode(), data3.GetHashCode());
    }

    /// <summary>
    /// Verifies the TryGetCell methods for valid and invalid positions.
    /// </summary>
    [Fact]
    public void TryGetCellTest()
    {
        var data = Data.InitialPlaced;

        // Valid position
        Assert.True(data.TryGetCell(3, 3, out var cell));
        Assert.Equal(CellState.Light, cell);

        // Invalid position (row out of range)
        Assert.False(data.TryGetCell(-1, 0, out _));
        Assert.False(data.TryGetCell(Data.Rows, 0, out _));

        // Invalid position (column out of range)
        Assert.False(data.TryGetCell(0, -1, out _));
        Assert.False(data.TryGetCell(0, Data.Columns, out _));

        // TryGetCell(CellPosition)
        Assert.True(data.TryGetCell(new CellPosition(4, 3), out var cell2));
        Assert.Equal(CellState.Dark, cell2);
    }

    /// <summary>
    /// Verifies the GetPlaceableCells method for both players.
    /// </summary>
    [Fact]
    public void GetPlaceableCellsTest()
    {
        var data = Data.InitialPlaced;

        Span<CellPosition> darkCells = stackalloc CellPosition[Data.Rows * Data.Columns];
        Span<CellPosition> lightCells = stackalloc CellPosition[Data.Rows * Data.Columns];
        int darkCount = data.GetPlaceableCells(Player.Dark, darkCells);
        int lightCount = data.GetPlaceableCells(Player.Light, lightCells);

        // Initial position: both players should have 4 placeable cells
        Assert.Equal(4, darkCount);
        Assert.Equal(4, lightCount);
    }

    /// <summary>
    /// Verifies the IsPlaceable methods for both players and valid/invalid positions.
    /// </summary>
    [Fact]
    public void IsPlaceableTest()
    {
        var data = Data.InitialPlaced;
        
        // Valid placeable position for Dark
        Assert.True(data.IsPlaceable(2, 3, Player.Dark));
        Assert.True(data.IsPlaceable(new CellPosition(2, 3), Player.Dark));
        
        // Not placeable (already occupied)
        Assert.False(data.IsPlaceable(3, 3, Player.Dark));

        // Not placeable (out of range)
        Assert.False(data.IsPlaceable(-1, 0, Player.Dark));
        Assert.False(data.IsPlaceable(0, -1, Player.Dark));
        Assert.False(data.IsPlaceable(Data.Rows, 0, Player.Dark));
        Assert.False(data.IsPlaceable(0, Data.Columns, Player.Dark));
    }

    /// <summary>
    /// Verifies the DarkCount and LightCount properties.
    /// </summary>
    [Fact]
    public void CountPropertiesTest()
    {
        var data = Data.InitialPlaced;
        Assert.Equal(2, data.DarkCount);
        Assert.Equal(2, data.LightCount);

        var data2 = data.Place(2, 3, Player.Dark);
        Assert.Equal(4, data2.DarkCount);
        Assert.Equal(1, data2.LightCount);
    }
}
