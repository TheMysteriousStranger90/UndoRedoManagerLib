using UndoRedoManagerConsoleExample;
using UndoRedoManagerConsoleExample.Models;

namespace UndoRedoManagerLib.Tests;

public class CommandTests
{
    [Fact]
    public void ChangeNameCommand_Execute_ChangesName()
    {
        var person = new Person("John Doe", 30, new Address("123 Main St", "Springfield"));
        var command = new ChangeNameCommand(person, "Jane Doe");

        command.Execute();

        Assert.Equal("Jane Doe", person.Name);
    }

    [Fact]
    public void ChangeNameCommand_Undo_RevertsName()
    {
        var person = new Person("John Doe", 30, new Address("123 Main St", "Springfield"));
        var command = new ChangeNameCommand(person, "Jane Doe");

        command.Execute();
        command.Undo();

        Assert.Equal("John Doe", person.Name);
    }

    [Fact]
    public void ChangeAgeCommand_Execute_ChangesAge()
    {
        var person = new Person("John Doe", 30, new Address("123 Main St", "Springfield"));
        var command = new ChangeAgeCommand(person, 25);

        command.Execute();

        Assert.Equal(25, person.Age);
    }

    [Fact]
    public void ChangeAgeCommand_Undo_RevertsAge()
    {
        var person = new Person("John Doe", 30, new Address("123 Main St", "Springfield"));
        var command = new ChangeAgeCommand(person, 25);

        command.Execute();
        command.Undo();

        Assert.Equal(30, person.Age);
    }

    [Fact]
    public void ChangeAddressCommand_Execute_ChangesAddress()
    {
        var person = new Person("John Doe", 30, new Address("123 Main St", "Springfield"));
        var newAddress = new Address("456 Elm St", "Shelbyville");
        var command = new ChangeAddressCommand(person, newAddress);

        command.Execute();

        Assert.Equal(newAddress, person.Address);
    }

    [Fact]
    public void ChangeAddressCommand_Undo_RevertsAddress()
    {
        var person = new Person("John Doe", 30, new Address("123 Main St", "Springfield"));
        var newAddress = new Address("456 Elm St", "Shelbyville");
        var command = new ChangeAddressCommand(person, newAddress);

        command.Execute();
        command.Undo();

        Assert.Equal("123 Main St", person.Address.Street);
        Assert.Equal("Springfield", person.Address.City);
    }
}