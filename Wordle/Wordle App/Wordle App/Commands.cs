using System.Windows.Input;
namespace Wordle_App;

public class Commands (Action<object> execute, Func<object, bool>? canExecute = null) : ICommand {
   private readonly Action<object> execute = execute;
   private readonly Func<object, bool>? canExecute = canExecute;

   public bool CanExecute (object? parameter) => canExecute == null || canExecute (parameter!);

   public void Execute (object? parameter) => execute (parameter!);

   public event EventHandler? CanExecuteChanged;
}


