using System.Windows;
using Grappachu.Core.Preview.UI;

namespace Grappachu.SolutionCloner
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException; 
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Dialogs.ShowError(e.Exception);
        }
    }
}
