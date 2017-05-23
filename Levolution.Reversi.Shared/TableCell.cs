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
        /// 
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this, _isSelectedPropertyChangedEventArgs);
            }
        }
        private bool _isSelected;
        private PropertyChangedEventArgs _isSelectedPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(IsSelected));

        /// <summary>
        /// 
        /// </summary>
        public bool IsPlaceable
        {
            get { return _isPlaceable; }
            set
            {
                _isPlaceable = value;
                PropertyChanged?.Invoke(this, _isPlaceablePropertyChangedEventArgs);
            }
        }
        private bool _isPlaceable;
        private PropertyChangedEventArgs _isPlaceablePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(IsPlaceable));

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
