namespace UndoRedoManagerLib;

/// <summary>
/// Provides extension methods for the <see cref="Stack{T}"/> class.
/// </summary>
public static class StackExtensions
{
    /// <summary>
    /// Removes the element at the specified index from the stack.
    /// </summary>
    /// <typeparam name="T">The type of elements in the stack.</typeparam>
    /// <param name="stack">The stack from which to remove the element.</param>
    /// <param name="index">The zero-based index of the element to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is out of range.</exception>
    public static void RemoveAt<T>(this Stack<T> stack, int index)
    {
        if (index < 0 || index >= stack.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var tempStack = new Stack<T>();

        // Move elements from the original stack to the temporary stack until the specified index is reached.
        for (int i = 0; i < stack.Count - index - 1; i++)
        {
            tempStack.Push(stack.Pop());
        }

        // Remove the element at the specified index.
        stack.Pop();

        // Move the elements back from the temporary stack to the original stack.
        while (tempStack.Count > 0)
        {
            stack.Push(tempStack.Pop());
        }
    }
}