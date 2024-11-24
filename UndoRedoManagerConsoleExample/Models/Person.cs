namespace UndoRedoManagerConsoleExample.Models;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Address Address { get; set; }

    public Person(string name, int age, Address address)
    {
        Name = name;
        Age = age;
        Address = address;
    }

    public override string ToString()
    {
        return $"{Name}, {Age} years old, lives at {Address}";
    }
}