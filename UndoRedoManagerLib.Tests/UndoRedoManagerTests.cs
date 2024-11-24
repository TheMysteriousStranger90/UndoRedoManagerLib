using UndoRedoManagerConsoleExample;
using UndoRedoManagerConsoleExample.Models;

namespace UndoRedoManagerLib.Tests;

public class UndoRedoManagerTests
{
    [Fact]
    public void Execute_AddsCommandToUndoStack()
    {
        var context = new UndoRedoContext();
        var manager = new UndoRedoManager();
        var person = new Person("John Doe", 30, new Address("123 Main St", "Springfield"));
        var command = new ChangeNameCommand(person, "Jane Doe");

        manager.Execute(command);

        Assert.Equal("Jane Doe", person.Name);
        manager.Undo();
        Assert.Equal("John Doe", person.Name);
    }

    [Fact]
    public void Undo_RevertsLastCommand()
    {
        var context = new UndoRedoContext();
        var manager = new UndoRedoManager();
        var person = new Person("John Doe", 30, new Address("123 Main St", "Springfield"));
        var command = new ChangeNameCommand(person, "Jane Doe");

        manager.Execute(command);
        manager.Undo();

        Assert.Equal("John Doe", person.Name);
    }

    [Fact]
    public void Redo_ReappliesLastUndoneCommand()
    {
        var context = new UndoRedoContext();
        var manager = new UndoRedoManager();
        var person = new Person("John Doe", 30, new Address("123 Main St", "Springfield"));
        var command = new ChangeNameCommand(person, "Jane Doe");

        manager.Execute(command);
        manager.Undo();
        manager.Redo();

        Assert.Equal("Jane Doe", person.Name);
    }

    [Fact]
    public void Transaction_CommitsMultipleCommands()
    {
        var context = new UndoRedoContext();
        var manager = new UndoRedoManager();
        var person = new Person("John Doe", 30, new Address("123 Main St", "Springfield"));

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

        Assert.Equal("Jane Doe", person.Name);
        Assert.Equal(25, person.Age);
        Assert.Equal("456 Elm St", person.Address.Street);
        Assert.Equal("Shelbyville", person.Address.City);

        context.Undo();

        Assert.Equal("John Doe", person.Name);
        Assert.Equal(30, person.Age);
        Assert.Equal("123 Main St", person.Address.Street);
        Assert.Equal("Springfield", person.Address.City);

        context.Redo();

        Assert.Equal("Jane Doe", person.Name);
        Assert.Equal(25, person.Age);
        Assert.Equal("456 Elm St", person.Address.Street);
        Assert.Equal("Shelbyville", person.Address.City);
    }
}