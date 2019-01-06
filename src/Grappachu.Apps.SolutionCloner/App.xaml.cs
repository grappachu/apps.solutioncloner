using System.Deployment.Application;
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
#if !DEBUG
 if (ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.IsFirstRun) 
#endif
            {
                var templateManager = new TemplateManager();
                templateManager.BuildTemplates();
            }

            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Dialogs.ShowError(e.Exception);
        }
    }
}
