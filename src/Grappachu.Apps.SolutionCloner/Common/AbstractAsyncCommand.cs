using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Grappachu.SolutionCloner.Common
{
    public abstract class AbstractAsyncCommand : ICommand
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
            ExecuteAsync(parameters);
        }

        private async void ExecuteAsync(object parameters)
        {
            await new TaskFactory().StartNew(() => OnExecute(parameters));
        }

        protected abstract void OnExecute(object parameters);
    }
}