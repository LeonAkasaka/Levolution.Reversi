using Levolution.Reversi.Records;
using System.ComponentModel;

namespace Levolution.Reversi;

/// <summary>
/// The cell of <see cref="Table"/>.
/// </summary>
/// <param name="position">This cell position.</param>
/// <param name="state"></param>
public class TableCell(CellState state = CellState.None) : INotifyPropertyChanged
{
    /// <summary>
    /// Gets and sets the cell position.
    /// </summary>
    //public CellPosition Position { get; } = position;

    /// <summary>
    /// Gets and sets the cell states.
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
    private CellState _state = state;
    private readonly PropertyChangedEventArgs _statePropertyChangedEventArgs = new(nameof(State));

    /// <summary>
    /// Gets and sets if a cell can be selected,
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
    private readonly PropertyChangedEventArgs _isSelectedPropertyChangedEventArgs = new(nameof(IsSelected));

    /// <summary>
    /// Gets and sets if disks can be placed in the cell.
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
    private readonly PropertyChangedEventArgs _isPlaceablePropertyChangedEventArgs = new(nameof(IsPlaceable));

    /// <summary>
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/>.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
}