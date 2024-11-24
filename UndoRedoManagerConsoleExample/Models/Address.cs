namespace UndoRedoManagerConsoleExample.Models;

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }

    public Address(string street, string city)
    {
        Street = street;
        City = city;
    }

    public override string ToString()
    {
        return $"{Street}, {City}";
    }
}