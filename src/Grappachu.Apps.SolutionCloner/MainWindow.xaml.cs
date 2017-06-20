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
    public partial class MainWindow : Window
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
                    TemplateSource = new DirectoryInfo(TxtSource.Text),
                    TargetFolder = new DirectoryInfo(TxtTarget.Text),
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

    //internal class AbsurumDateConverter
    //{
    //    private readonly int _startYear;

    //    public AbsurumDateConverter(int startYear = 1900)
    //    {
    //        _startYear = startYear;
    //    }

    //    public DateTime Parse(decimal date)
    //    {
    //        var y = (int) (date / 1000 + _startYear);
    //        var d = date % 100;


    //        return new DateTime(y, 1, 1).AddDays((double) (d - 1));
    //    }

    //    public decimal UnParse(DateTime date)
    //    {
    //        return (date.Year - _startYear) * 1000 + date.DayOfYear;
    //    }
    //}
}