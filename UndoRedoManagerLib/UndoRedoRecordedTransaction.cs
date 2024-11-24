namespace UndoRedoManagerLib;

/// <summary>
/// Represents a recorded transaction for undo/redo operations.
/// </summary>
public class UndoRedoRecordedTransaction
{
    private readonly List<IUndoRedoCommand> _commands;
    public object? Tag { get; }

    internal UndoRedoRecordedTransaction(IEnumerable<IUndoRedoCommand> commands, object? tag)
    {
        _commands = new List<IUndoRedoCommand>(commands);
        Tag = tag;
    }

    /// <summary>
    /// Rolls back the transaction.
    /// </summary>
    public void Rollback()
    {
        for (int i = _commands.Count - 1; i >= 0; i--)
        {
            _commands[i].Undo();
        }
    }

    /// <summary>
    /// Rolls forward the transaction.
    /// </summary>
    public void RollForward()
    {
        foreach (var command in _commands)
        {
            command.Execute();
        }
    }
}