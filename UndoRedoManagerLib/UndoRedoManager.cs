namespace UndoRedoManagerLib;

/// <summary>
/// Manages undo and redo operations.
/// </summary>
public class UndoRedoManager
{
    private readonly Stack<IUndoRedoCommand> _undoStack = new();
    private readonly Stack<IUndoRedoCommand> _redoStack = new();
    private UndoRedoTransaction? _currentTransaction;
    private readonly int _maxStackSize;
    private readonly AutoCommandGrouper _commandGrouper;
    private bool _isGroupingEnabled;

    public UndoRedoManager(int maxStackSize = 100, bool enableGrouping = true)
    {
        _maxStackSize = maxStackSize;
        _isGroupingEnabled = enableGrouping;
        _commandGrouper = new AutoCommandGrouper();
    }

    /// <summary>
    /// Begins a new transaction.
    /// </summary>
    /// <param name="context">The context for the transaction.</param>
    /// <param name="tag">An optional tag for the transaction.</param>
    public void BeginTransaction(UndoRedoContext context, object? tag = null)
    {
        if (_currentTransaction != null)
            throw new InvalidOperationException("Transaction already in progress.");

        _currentTransaction = new UndoRedoTransaction(context, tag);
    }

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    public void CommitTransaction()
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("No active transaction.");

        _currentTransaction.Commit();
        _currentTransaction = null;
    }

    /// <summary>
    /// Executes a command and adds it to the undo stack.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    public void Execute(IUndoRedoCommand command)
    {
        command.Execute();

        if (_currentTransaction != null)
        {
            _currentTransaction.AddCommand(command);
            return;
        }

        if (_isGroupingEnabled && _commandGrouper.ShouldGroup(command))
        {
            _commandGrouper.AddToGroup(command);
            var groupTransaction = _commandGrouper.CreateGroupTransaction();
            
            if (groupTransaction != null)
            {
                AddToUndoStack(groupTransaction);
                _commandGrouper.ClearGroup();
                return;
            }
        }
        else
        {
            _commandGrouper.ClearGroup();
            _commandGrouper.AddToGroup(command);
        }

        AddToUndoStack(command);
    }
    
    private void AddToUndoStack(IUndoRedoCommand command)
    {
        if (_undoStack.Count >= _maxStackSize)
        {
            _undoStack.RemoveAt(0);
        }
        _undoStack.Push(command);
        _redoStack.Clear();
    }

    /// <summary>
    /// Enables or disables automatic command grouping.
    /// </summary>
    public void SetGroupingEnabled(bool enabled)
    {
        _isGroupingEnabled = enabled;
        if (!enabled)
        {
            _commandGrouper.ClearGroup();
        }
    }

    /// <summary>
    /// Undoes the last command.
    /// </summary>
    public void Undo()
    {
        if (_undoStack.Count == 0)
            throw new InvalidOperationException("No commands to undo.");

        var command = _undoStack.Pop();
        command.Undo();
        _redoStack.Push(command);
    }

    /// <summary>
    /// Redoes the last undone command.
    /// </summary>
    public void Redo()
    {
        if (_redoStack.Count == 0)
            throw new InvalidOperationException("No commands to redo.");

        var command = _redoStack.Pop();
        command.Execute();
        _undoStack.Push(command);
    }
}