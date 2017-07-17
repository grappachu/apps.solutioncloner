using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Grappachu.Core.Lang.Extensions;
using Grappachu.Core.Preview.UI;
using Grappachu.SolutionCloner.Engine.Components;
using Grappachu.SolutionCloner.Engine.Components.Scanner;
using Grappachu.SolutionCloner.Engine.Interfaces;
using Grappachu.SolutionCloner.Engine.Model;
using Grappachu.SolutionCloner.Properties;
using log4net.Core;

namespace Grappachu.SolutionCloner
{
    /// <summary>
    ///     Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly SolutionScanner _scanner;

        public MainWindow()
        {
            _scanner = new SolutionScanner();
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


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Title += " v." + CurrentVersion;
                LogViewer.DisplayLevel = Level.Debug;

                var defaultRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                TxtSource.InitialDirectory = Settings.Default.LastSourcePath.Or(defaultRoot);
                TxtTarget.InitialDirectory = Settings.Default.LastTargetPath.Or(defaultRoot);

                var defaultProfile = new CloneProfile();
                EditorDeleteFiles.Text = string.Join(Environment.NewLine, defaultProfile.DeleteFiles);
                EditorExcludeFolders.Text = string.Join(Environment.NewLine, defaultProfile.ExcludeFolders);
                EditorReplaceFiles.Text = string.Join(Environment.NewLine, defaultProfile.ReplaceFiles);
            }
            catch (Exception ex)
            {
                Dialogs.ShowError(ex);
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

                pars.CloneProfile.DeleteFiles = Parse(EditorDeleteFiles.Text);
                pars.CloneProfile.ExcludeFolders = Parse(EditorExcludeFolders.Text);
                pars.CloneProfile.ReplaceFiles = Parse(EditorReplaceFiles.Text);

                TabLog.IsSelected = true;

                SetWaiting(true);
                new TaskFactory()
                    .StartNew(() => builder.Clone(pars))
                    .ContinueWith(t => { Dispatcher.BeginInvoke(new Action(() => { SetWaiting(false); })); });

                // Dispatcher.BeginInvoke(new Action(() => { builder.Clone(pars); }));

                Settings.Default.LastSourcePath = pars.TemplateSource.Parent?.FullName;
                Settings.Default.LastTargetPath = pars.TargetFolder.Parent?.FullName;
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Dialogs.ShowError(ex);
            }
        }

        private void SetWaiting(bool waiting)
        {
            if (waiting)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                BtnClone.IsEnabled = false;
                PrgWait.Visibility = Visibility.Visible;
            }
            else
            {
                PrgWait.Visibility = Visibility.Hidden;
                BtnClone.IsEnabled = true;
                Mouse.OverrideCursor = null;
            }
        }


        private static IList<string> Parse(string text)
        {
            return text.Split().Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }

        private void TxtSource_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TxtSource.SelectedValue))
            {
                OnSuggestReplacements(TxtSource.SelectedValue);
            }
        }


        private void OnSuggestReplacements(string sourcePath)
        {
            if (string.IsNullOrWhiteSpace(TxtOriginalKey.Text))
            {
                var solutionFiles = _scanner.FindAll(sourcePath).ToArray();
                if (solutionFiles.Any())
                {
                    var solution = solutionFiles.First();
                    var proj = solution.GetProjects().FirstOrDefault();
                    if (proj != null)
                    {
                        var ns = proj.GetNamespace();
                        TxtOriginalKey.Text = ns;
                    }
                }
            }
        }
    }
}