# UndoRedoManager
![Image 1](Screenshots/Screen1.png)

UndoRedoManager is a .NET library for managing undo and redo operations. It provides a mechanism to track changes to objects and collections, allowing you to undo and redo those changes as needed. This library is useful for applications that require support for undo/redo functionality.

## Features

- Track changes to objects and collections.
- Support for transactions to group multiple changes.
- Undo and redo operations.
- Notifications for property changes.

## Usage
### Defining Commands

Commands represent the changes you want to track. Each command should implement the IUndoRedoCommand interface. You can either define custom commands or use the built-in ChangePropertyCommand for simpler property changes. Example:

```csharp
using UndoRedoManager;

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
```

### Using ChangePropertyCommand
The ChangePropertyCommand allows you to define property changes dynamically without needing a custom command. This simplifies development while maintaining flexibility. Example:

```
...
            new ChangePropertyCommand<Address>(
                value => person.Address = value,
                person.Address,
                new Address("456 Elm St", "Shelbyville"))
```

Using the UndoRedoManager
Create an instance of UndoRedoManager and use it to execute commands. You can also group multiple commands into a transaction. Example:

```csharp
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
```



## Future Development

This project is still under development.

## Contributing

Contributions are welcome. Please fork the repository and create a pull request with your changes.

## Author

Bohdan Harabadzhyu

## License

[MIT](https://choosealicense.com/licenses/mit/)