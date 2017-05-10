using Levolution.Reversi.Records;
using System.ComponentModel;

namespace Levolution.Reversi
{
    /// <summary>
    /// Table cell.
    /// </summary>
    public class TableCell : INotifyPropertyChanged
    {
        /// <summary>
        /// Position.
        /// </summary>
        public CellPosition Position { get; }

        /// <summary>
        /// Cell state.
        /// </summary>
        public CellState State
        {
            get { return _state; }
            set
            {
                _state = value;
                PropertyChanged?.Invoke(this, _statePropertyChangedEventArgs);
            }
        }
        private CellState _state;
        private PropertyChangedEventArgs _statePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(State));

        /// <summary>
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/>.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Create new <see cref="TableCell"/> instance.
        /// </summary>
        /// <param name="position">This cell position.</param>
        public TableCell(CellPosition position, CellState state = CellState.None)
        {
            Position = position;
            _state = CellState.None;
        }

    }
}
