using System;
using System.Windows.Input;

namespace AdeDl.App.Commands
{
    public class Command : ICommand
    {
        private readonly Action<object?> _action;

        public Command(Action action) => _action = _ => action.Invoke();

        public Command(Action<object> action) => _action = action;

        public virtual bool CanExecute(object parameter) => true;

        public virtual void Execute(object parameter) => _action.Invoke(parameter);

        public event EventHandler CanExecuteChanged;
    }
}