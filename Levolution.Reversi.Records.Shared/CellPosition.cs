using System;
using System.Collections.Generic;

namespace Levolution.Reversi.Records
{
    /// <summary>
    /// Position on reversi table.
    /// </summary>
    public struct CellPosition
    {
        /// <summary>
        /// Row number.
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// Column number.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Create new <see cref="CellPosition"/> instance.
        /// </summary>
        /// <param name="row">Row position number.</param>
        /// <param name="column">Column position number.</param>
        public CellPosition(int row, int column) : this()
        {
            Row = row;
            Column = column;
        }

        /// <summary>
        /// <see cref="CellPosition"/> to string that is formated at "a1" - "h8".
        /// </summary>
        /// <returns>Formatted position string.</returns>
        public override string ToString() => $"{(char)(Column + 'a')}{Row + 1}";

        /// <summary>
        /// Parse from string that is formated at "a1" - "h8".
        /// </summary>
        /// <param name="s">position string.</param>
        /// <returns>parsed <see cref="CellPosition"/>.</returns>
        public static CellPosition Parse(string s)
        {
            if (s == null) { throw new ArgumentNullException(nameof(s)); }
            if (s.Length != 2) { throw new ArgumentException($"\"{s}\" is not position string.", nameof(s)); }

            var ary = s.ToCharArray();
            var cc = ary[0];
            var rc = ary[1];
            return ToCellPosition(cc, rc);
        }

        /// <summary>
        /// Parse cell position characters.
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="rc"></param>
        /// <returns></returns>
        private static CellPosition ToCellPosition(char cc, char rc)
        {
            if (!(cc >= 'a' && cc <= 'h')) { throw new ArgumentException($"\"{cc}\" is invalid value.", nameof(cc)); }
            if (!(rc >= '1' && rc <= '8')) { throw new ArgumentException($"\"{rc}\" is invalid value.", nameof(rc)); }
            return new CellPosition(rc - '1', cc - 'a');
        }

        /// <summary>
        /// Postion string to <see cref="CellPosition"/>s.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static IEnumerable<CellPosition> ParseList(string s)
        {
            if (s == null) { throw new ArgumentNullException(nameof(s)); }
            if ((s.Length % 2) != 0) { throw new ArgumentException($"\"{s}\" is not position string.", nameof(s)); }

            var ary = s.ToCharArray();
            for (var i = 0; i < ary.Length; i += 2)
            {
                yield return ToCellPosition(ary[i], ary[i + 1]);
            }
        }
    }
}