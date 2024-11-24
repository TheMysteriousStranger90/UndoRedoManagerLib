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

First, define commands that represent the changes you want to track. Each command should implement the `IUndoRedoCommand` interface. Example:

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
```



## Future Development

This project is still under development.

## Contributing

Contributions are welcome. Please fork the repository and create a pull request with your changes.

## Author

Bohdan Harabadzhyu

## License

[MIT](https://choosealicense.com/licenses/mit/)