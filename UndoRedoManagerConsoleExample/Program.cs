using UndoRedoManagerConsoleExample;
using UndoRedoManagerConsoleExample.Models;
using UndoRedoManagerLib;

class Program
{
    static void Main(string[] args)
    {
        var context = new UndoRedoContext();
        var manager = new UndoRedoManager(1000, enableGrouping: false); // Stack size and Disable grouping for this example

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

            // Using ChangePropertyCommand to change the Address property of the Person object.
            // 1. Create custom commands like ChangeNameCommand and ChangeAgeCommand for specific properties.
            //    - These commands are tailored to specific use cases, often encapsulating additional logic or validation.
            // 2. Use the generic ChangePropertyCommand when we simply need to change a property value without custom logic.
            //    - This allows us to reuse a single command implementation for any property of any object.
            //    - In this case, ChangePropertyCommand<Address> is used to change the Address property of the Person object.
            //    - The lambda expression `value => person.Address = value` specifies how the property should be updated.
            //    - The old value (person.Address) and new value (new Address("456 Elm St", "Shelbyville")) are provided for undo/redo functionality.
            new ChangePropertyCommand<Address>(
                value => person.Address = value,
                person.Address,
                new Address("456 Elm St", "Shelbyville"))
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