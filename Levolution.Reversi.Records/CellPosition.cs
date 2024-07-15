using System;
using System.Collections.Generic;

namespace Levolution.Reversi.Records;

/// <summary>
/// Represents a position on a board grid using row and column indices.
/// </summary>
/// <param name="row">The row index of the cell position.</param>
/// <param name="column">The column index of the cell position.</param>
public readonly struct CellPosition(int row, int column)
{
    /// <summary>
    /// Gets the row index of the cell position.
    /// </summary>
    public int Row { get; } = row;

    /// <summary>
    /// Gets the column index of the cell position.
    /// </summary>
    public int Column { get; } = column;

    /// <summary>
    /// Converts the current CellPosition value to its string representation.
    /// </summary>
    /// <returns>A string representation of the current CellPosition value, where the column is represented by a letter ('a' to 'h') and the row is represented by a number ('1' to '8').</returns>
    public override string ToString() => $"{(char)(Column + 'a')}{Row + 1}";

    public override bool Equals(object obj)
    {
        if (obj is CellPosition a) { return this == a; }
        return false;
    }

    public override int GetHashCode()
    {
        return Row ^ Column;
    }

    public static bool operator ==(CellPosition a, CellPosition b)
    {
        return a.Row == b.Row && a.Column == b.Column;
    }
    public static bool operator !=(CellPosition a, CellPosition b)
    {
        return !(a == b);
    }

    /// <summary>
    /// Parses a string representation of a cell position into a CellPosition value.
    /// </summary>
    /// <param name="s">The string representation of the cell position to parse. It should be two characters long.</param>
    /// <returns>A CellPosition value corresponding to the specified string representation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input string is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the input string length is not equal to 2, indicating an invalid position string.</exception>
    public static CellPosition Parse(string s)
    {
        if (s == null) { throw new ArgumentNullException(nameof(s)); }
        if (s.Length != 2) { throw new ArgumentException($"\"{s}\" is not position string.", nameof(s)); }

        return ToCellPosition(s[0], s[1]);
    }

    /// <summary>
    /// Converts a pair of characters representing a cell position into a CellPosition value.
    /// </summary>
    /// <param name="cc">The character representing the column ('a' to 'h').</param>
    /// <param name="rc">The character representing the row ('1' to '8').</param>
    /// <returns>A CellPosition value corresponding to the specified column and row characters.</returns>
    /// <exception cref="ArgumentException">Thrown if the column character is not between 'a' and 'h', or if the row character is not between '1' and '8'.</exception>
    private static CellPosition ToCellPosition(char cc, char rc)
    {
        if (!(cc >= 'a' && cc <= 'h')) { throw new ArgumentException($"\"{cc}\" is invalid value.", nameof(cc)); }
        if (!(rc >= '1' && rc <= '8')) { throw new ArgumentException($"\"{rc}\" is invalid value.", nameof(rc)); }
        return new CellPosition(rc - '1', cc - 'a');
    }

    /// <summary>
    /// Parses a string representation of cell positions into an enumerable collection of CellPosition values.
    /// </summary>
    /// <param name="s">The string representation of cell positions to parse.</param>
    /// <returns>An enumerable collection of CellPosition values parsed from the input string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input string is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the input string length is not even, indicating an invalid position string.</exception>
    public static IEnumerable<CellPosition> ParseList(string s)
    {
        if (s == null) { throw new ArgumentNullException(nameof(s)); }
        if ((s.Length % 2) != 0) { throw new ArgumentException($"\"{s}\" is not position string.", nameof(s)); }

        for (var i = 0; i < s.Length; i += 2)
        {
            yield return ToCellPosition(s[i], s[i + 1]);
        }
    }
}