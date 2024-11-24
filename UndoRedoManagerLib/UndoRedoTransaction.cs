namespace UndoRedoManagerLib;

/// <summary>
/// Represents a transaction for undo/redo operations.
/// </summary>
public class UndoRedoTransaction : IDisposable
{
    private readonly UndoRedoContext _context;
    private readonly object? _tag;
    private bool _disposed;
    private readonly List<IUndoRedoCommand> _commands = new();

    internal UndoRedoTransaction(UndoRedoContext context, object? tag)
    {
        _context = context;
        _tag = tag;
    }

    /// <summary>
    /// Adds a command to the transaction.
    /// </summary>
    /// <param name="command">The command to add.</param>
    public void AddCommand(IUndoRedoCommand command)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(UndoRedoTransaction));

        _commands.Add(command);
    }

    /// <summary>
    /// Commits the transaction.
    /// </summary>
    public void Commit()
    {
        if (_disposed)
            return;

        _disposed = true;
        _context.AddTransaction(new UndoRedoRecordedTransaction(_commands, _tag));
    }

    /// <summary>
    /// Rolls back the transaction.
    /// </summary>
    public void Rollback()
    {
        if (_disposed)
            return;

        _disposed = true;
        foreach (var command in _commands)
        {
            command.Undo();
        }
    }

    /// <summary>
    /// Disposes the transaction, rolling it back if not committed.
    /// </summary>
    public void Dispose() => Rollback();
}