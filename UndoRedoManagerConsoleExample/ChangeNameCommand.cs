using UndoRedoManagerConsoleExample.Models;
using UndoRedoManagerLib;

namespace UndoRedoManagerConsoleExample;

public class ChangeNameCommand : IUndoRedoCommand
{
    private readonly MainTransactionChange<string> _change;

    public ChangeNameCommand(Person person, string newName)
    {
        _change = new MainTransactionChange<string>(
            value => person.Name = value,
            person.Name,
            newName);
    }

    public void Execute() => _change.RollForward();
    public void Undo() => _change.Rollback();
}