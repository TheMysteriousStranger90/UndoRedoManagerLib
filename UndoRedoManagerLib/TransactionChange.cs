namespace UndoRedoManagerLib;

/// <summary>
/// Represents a transaction change.
/// </summary>
public abstract class TransactionChange
{
    /// <summary>
    /// Rolls back the change.
    /// </summary>
    public abstract void Rollback();

    /// <summary>
    /// Rolls forward the change.
    /// </summary>
    public abstract void RollForward();
}