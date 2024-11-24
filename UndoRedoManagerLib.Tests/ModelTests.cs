using UndoRedoManagerConsoleExample.Models;

namespace UndoRedoManagerLib.Tests;

public class ModelTests
{
    [Fact]
    public void Person_Properties_AreSetCorrectly()
    {
        var address = new Address("123 Main St", "Springfield");
        var person = new Person("John Doe", 30, address);

        Assert.Equal("John Doe", person.Name);
        Assert.Equal(30, person.Age);
        Assert.Equal(address, person.Address);
    }

    [Fact]
    public void Address_Properties_AreSetCorrectly()
    {
        var address = new Address("123 Main St", "Springfield");

        Assert.Equal("123 Main St", address.Street);
        Assert.Equal("Springfield", address.City);
    }
}