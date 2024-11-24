namespace UndoRedoManagerLib;

/// <summary>
/// Represents a change in a transaction.
/// </summary>
/// <typeparam name="T">The type of the value being changed.</typeparam>
public class MainTransactionChange<T> : TransactionChange
{
    private readonly Action<T> _applyChange;
    private readonly T _initialValue;
    private readonly T _updatedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainTransactionChange{T}"/> class.
    /// </summary>
    /// <param name="applyChange">The action to apply the change.</param>
    /// <param name="initialValue">The initial value.</param>
    /// <param name="updatedValue">The updated value.</param>
    public MainTransactionChange(Action<T> applyChange, T initialValue, T updatedValue)
    {
        _applyChange = applyChange;
        _initialValue = initialValue;
        _updatedValue = updatedValue;
    }

    /// <summary>
    /// Rolls back the change.
    /// </summary>
    public override void Rollback() => _applyChange(_initialValue);

    /// <summary>
    /// Rolls forward the change.
    /// </summary>
    public override void RollForward() => _applyChange(_updatedValue);
}