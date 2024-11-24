namespace UndoRedoManagerLib;

/// <summary>
/// Interface for undo/redo commands.
/// </summary>
public interface IUndoRedoCommand
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    void Execute();

    /// <summary>
    /// Undoes the command.
    /// </summary>
    void Undo();
}