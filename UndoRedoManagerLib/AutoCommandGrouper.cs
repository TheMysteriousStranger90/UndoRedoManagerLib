namespace UndoRedoManagerLib;

/// <summary>
/// Groups related commands into a single transaction based on timing and type.
/// </summary>
public class AutoCommandGrouper
{
    private readonly TimeSpan _groupingWindow;
    private DateTime _lastCommandTime;
    private Type? _lastCommandType;
    private List<IUndoRedoCommand> _currentGroup;

    public AutoCommandGrouper(TimeSpan? groupingWindow = null)
    {
        _groupingWindow = groupingWindow ?? TimeSpan.FromMilliseconds(500);
        _currentGroup = new List<IUndoRedoCommand>();
    }

    /// <summary>
    /// Determines if a command should be grouped with the previous commands.
    /// </summary>
    public bool ShouldGroup(IUndoRedoCommand command)
    {
        var now = DateTime.Now;
        var timeDifference = now - _lastCommandTime;

        bool shouldGroup = _lastCommandType != null &&
                           command.GetType() == _lastCommandType &&
                           timeDifference <= _groupingWindow;

        _lastCommandTime = now;
        _lastCommandType = command.GetType();

        return shouldGroup;
    }

    /// <summary>
    /// Adds a command to the current group.
    /// </summary>
    public void AddToGroup(IUndoRedoCommand command)
    {
        _currentGroup.Add(command);
    }

    /// <summary>
    /// Creates a transaction command from the current group and clears it.
    /// </summary>
    public TransactionCommand? CreateGroupTransaction()
    {
        if (_currentGroup.Count <= 1)
            return null;

        var transaction = new TransactionCommand(new List<IUndoRedoCommand>(_currentGroup));
        _currentGroup.Clear();
        return transaction;
    }

    /// <summary>
    /// Clears the current group.
    /// </summary>
    public void ClearGroup()
    {
        _currentGroup.Clear();
        _lastCommandType = null;
    }
}