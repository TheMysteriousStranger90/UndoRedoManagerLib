namespace UndoRedoManagerLib;

/// <summary>
/// Represents a command that groups multiple undo/redo commands.
/// </summary>
public class TransactionCommand : IUndoRedoCommand
{
    private readonly List<IUndoRedoCommand> _commands;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionCommand"/> class.
    /// </summary>
    /// <param name="commands">The list of commands to group.</param>
    public TransactionCommand(List<IUndoRedoCommand> commands)
    {
        _commands = commands;
    }

    /// <summary>
    /// Executes all commands in the transaction.
    /// </summary>
    public void Execute()
    {
        foreach (var command in _commands)
        {
            command.Execute();
        }
    }

    /// <summary>
    /// Undoes all commands in the transaction.
    /// </summary>
    public void Undo()
    {
        for (int i = _commands.Count - 1; i >= 0; i--)
        {
            _commands[i].Undo();
        }
    }
}