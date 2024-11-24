using UndoRedoManagerConsoleExample.Models;
using UndoRedoManagerLib;

namespace UndoRedoManagerConsoleExample;

public class ChangeAgeCommand : IUndoRedoCommand
{
    private readonly MainTransactionChange<int> _change;

    public ChangeAgeCommand(Person person, int newAge)
    {
        _change = new MainTransactionChange<int>(
            value => person.Age = value,
            person.Age,
            newAge);
    }

    public void Execute() => _change.RollForward();
    public void Undo() => _change.Rollback();
}
