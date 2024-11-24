using UndoRedoManagerConsoleExample.Models;
using UndoRedoManagerLib;

namespace UndoRedoManagerConsoleExample;

public class ChangeAddressCommand : IUndoRedoCommand
{
    private readonly MainTransactionChange<Address> _change;

    public ChangeAddressCommand(Person person, Address newAddress)
    {
        _change = new MainTransactionChange<Address>(
            value => person.Address = value,
            person.Address,
            newAddress);
    }

    public void Execute() => _change.RollForward();
    public void Undo() => _change.Rollback();
}