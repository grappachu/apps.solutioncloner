using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Grappachu.SolutionCloner.Common
{
    public abstract class AbstractCommand : ICommand
    {

        [DebuggerStepThrough]
        public bool CanExecute(object parameters)
        {
            return OnCanExecute();
        }

        protected virtual bool OnCanExecute()
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameters)
        {
            OnExecute(parameters);
        }

        protected abstract void OnExecute(object parameters);
    }
}