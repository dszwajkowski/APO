using System;
using System.Windows.Input;

namespace ApoUI.ViewModels
{
    public class ExecuteCommand : ICommand
    {
        private Action action;
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public bool CanExecute(object parameter)
        {
            // Always execute
            return true;
        }

        public ExecuteCommand(Action _action)
        {
            action = _action;
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}
