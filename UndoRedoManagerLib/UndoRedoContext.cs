using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UndoRedoManagerLib;

/// <summary>
/// Manages the context for undo/redo operations.
/// </summary>
public class UndoRedoContext : INotifyPropertyChanged
{
    private readonly List<UndoRedoRecordedTransaction> _recordedTransactions = new();
    private int _currentIndex = -1;
    private bool _isSuspended;

    /// <summary>
    /// Gets or sets a value indicating whether the context is suspended.
    /// </summary>
    public bool IsSuspended
    {
        get => _isSuspended;
        private set
        {
            if (_isSuspended != value)
            {
                _isSuspended = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Suspends the context, clearing the transaction history.
    /// </summary>
    public void Suspend()
    {
        IsSuspended = true;
        _currentIndex = -1;
        _recordedTransactions.Clear();
    }

    /// <summary>
    /// Resumes the context.
    /// </summary>
    public void Resume()
    {
        IsSuspended = false;
    }

    /// <summary>
    /// Adds a transaction to the context.
    /// </summary>
    /// <param name="transaction">The transaction to add.</param>
    public void AddTransaction(UndoRedoRecordedTransaction transaction)
    {
        if (IsSuspended)
            return;

        while (_currentIndex < _recordedTransactions.Count - 1)
            _recordedTransactions.RemoveAt(_recordedTransactions.Count - 1);

        _recordedTransactions.Add(transaction);
        _currentIndex = _recordedTransactions.Count - 1;
    }

    /// <summary>
    /// Gets a value indicating whether there are commands to undo.
    /// </summary>
    public bool CanUndo => _currentIndex >= 0;

    /// <summary>
    /// Gets a value indicating whether there are commands to redo.
    /// </summary>
    public bool CanRedo => _currentIndex < _recordedTransactions.Count - 1;

    /// <summary>
    /// Undoes the last transaction.
    /// </summary>
    public void Undo()
    {
        if (!CanUndo)
            throw new InvalidOperationException("No transactions to undo.");

        _recordedTransactions[_currentIndex].Rollback();
        _currentIndex--;
    }

    /// <summary>
    /// Redoes the last undone transaction.
    /// </summary>
    public void Redo()
    {
        if (!CanRedo)
            throw new InvalidOperationException("No transactions to redo.");

        _currentIndex++;
        _recordedTransactions[_currentIndex].RollForward();
    }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}