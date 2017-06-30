using System;
using System.Deployment.Application;
using System.IO;
using System.Reflection;
using System.Windows;
using Grappachu.Core.Preview.UI;
using Grappachu.SolutionCloner.Engine.Components;
using Grappachu.SolutionCloner.Engine.Model;
using log4net.Core;

namespace Grappachu.SolutionCloner
{
    /// <summary>
    ///     Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        public string CurrentVersion
        {
            get
            {
                return ApplicationDeployment.IsNetworkDeployed
                    ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                    : Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        private void BtnClone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LogViewer.Clear();
                var builder = new Engine.SolutionCloner(new CloneSettingsValidator(), new UpdaterFactory<IFileUpdater>());
                var pars = new CloneSettings
                {
                    TemplateSource = new DirectoryInfo(TxtSource.SelectedValue),
                    TargetFolder = new DirectoryInfo(TxtTarget.SelectedValue),
                    TemplateKey = TxtOriginalKey.Text,
                    TargetKey = TxtNewKey.Text
                };

                Dispatcher.BeginInvoke(new Action(() => { builder.Clone(pars); }));
            }
            catch (Exception ex)
            {
                Dialogs.ShowError(ex);
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Title += " v." + CurrentVersion;
                LogViewer.DisplayLevel = Level.Debug;
            }
            catch (Exception ex)
            {
                Dialogs.ShowError(ex);
            }
        }
    }
}