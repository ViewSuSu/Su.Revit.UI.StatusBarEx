using System;
using System.Windows.Input;

namespace Su.Revit.UI.StatusBarEx.Utils
{
    internal class RelayCommand : ICommand
    {
        private Action action;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action?.Invoke();
        }
    }
}
