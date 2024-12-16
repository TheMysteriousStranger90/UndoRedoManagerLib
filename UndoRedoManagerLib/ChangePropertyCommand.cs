namespace UndoRedoManagerLib;

public class ChangePropertyCommand<T> : IUndoRedoCommand
{
    private readonly Action<T> _setValue;
    private readonly T _oldValue;
    private readonly T _newValue;
    private readonly Func<T, bool>? _validate; // Дополнительная функция для проверки значения.

    public ChangePropertyCommand(Action<T> setValue, T oldValue, T newValue, Func<T, bool>? validate = null)
    {
        _setValue = setValue;
        _oldValue = oldValue;
        _newValue = newValue;
        _validate = validate;
        
        if (_validate != null && !_validate(newValue))
            throw new ArgumentException("The new value does not meet the validation criteria.");
    }

    public void Execute()
    {
        if (_validate == null || _validate(_newValue))
            _setValue(_newValue);
    }

    public void Undo() => _setValue(_oldValue);
}
