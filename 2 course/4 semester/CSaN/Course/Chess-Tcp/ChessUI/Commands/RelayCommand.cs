using System.Windows.Input;

namespace ChessUI.Commands;

//For MVVM
public class RelayCommand : ICommand
{
    readonly Action<object> _execute;
    readonly Predicate<object>? _canExecute;

    public RelayCommand(Action<object> execute, Predicate<object>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter) 
        => _canExecute == null || _canExecute(parameter);

    public void Execute(object parameter) => _execute(parameter);

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}