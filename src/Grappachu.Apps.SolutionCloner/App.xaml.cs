using System.Deployment.Application;
using System.Windows.Forms;
using Grappachu.Core.Preview.UI;
using Application = System.Windows.Application;

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
            if ((ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.IsFirstRun) || Control.ModifierKeys == Keys.Shift)
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
