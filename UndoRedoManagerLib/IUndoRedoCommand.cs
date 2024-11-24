namespace UndoRedoManagerLib;

public interface IUndoRedoCommand
{
    void Execute();
    void Undo();
}