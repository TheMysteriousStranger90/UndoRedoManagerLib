using UndoRedoManagerConsoleExample;
using UndoRedoManagerConsoleExample.Models;
using UndoRedoManagerLib;

class Program
{
    static void Main(string[] args)
    {
        var context = new UndoRedoContext();
        var manager = new UndoRedoManager();

        var address = new Address("123 Main St", "Springfield");
        var person = new Person("John Doe", 30, address);

        Console.WriteLine("Initial state:");
        Console.WriteLine(person);

        context.Resume();
        manager.BeginTransaction(context);
        var commands = new List<IUndoRedoCommand>
        {
            new ChangeNameCommand(person, "Jane Doe"),
            new ChangeAgeCommand(person, 25),
            new ChangeAddressCommand(person, new Address("456 Elm St", "Shelbyville"))
        };
        var transactionCommand = new TransactionCommand(commands);
        manager.Execute(transactionCommand);
        manager.CommitTransaction();

        Console.WriteLine("\nAfter changes:");
        Console.WriteLine(person);

        context.Undo();
        Console.WriteLine("\nAfter undo:");
        Console.WriteLine(person);

        context.Redo();
        Console.WriteLine("\nAfter redo:");
        Console.WriteLine(person);
    }
}