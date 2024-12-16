namespace UndoRedoManagerLib;

/// <summary>
/// Represents a command to change a property value with undo/redo functionality.
/// </summary>
/// <typeparam name="T">The type of the property value.</typeparam>
public class ChangePropertyCommand<T> : IUndoRedoCommand
{
    private readonly Action<T> _setValue;
    private readonly T _oldValue;
    private readonly T _newValue;
    private readonly Func<T, bool>? _validate;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangePropertyCommand{T}"/> class.
    /// </summary>
    /// <param name="setValue">The action to set the property value.</param>
    /// <param name="oldValue">The old value of the property.</param>
    /// <param name="newValue">The new value of the property.</param>
    /// <param name="validate">An optional validation function for the new value.</param>
    /// <exception cref="ArgumentException">Thrown if the new value does not meet the validation criteria.</exception>
    public ChangePropertyCommand(Action<T> setValue, T oldValue, T newValue, Func<T, bool>? validate = null)
    {
        _setValue = setValue;
        _oldValue = oldValue;
        _newValue = newValue;
        _validate = validate;
        
        if (_validate != null && !_validate(newValue))
            throw new ArgumentException("The new value does not meet the validation criteria.");
    }

    /// <summary>
    /// Executes the command, setting the property to the new value.
    /// </summary>
    public void Execute()
    {
        if (_validate == null || _validate(_newValue))
            _setValue(_newValue);
    }

    /// <summary>
    /// Undoes the command, reverting the property to the old value.
    /// </summary>
    public void Undo() => _setValue(_oldValue);
}