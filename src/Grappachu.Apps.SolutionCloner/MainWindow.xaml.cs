﻿using System;
using System.Deployment.Application;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Grappachu.Core.Lang.Extensions;
using Grappachu.Core.Preview.UI;
using Grappachu.SolutionCloner.Engine.Components;
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

                pars.CloneProfile.DeleteFiles = EditorDeleteFiles.Text.Split();
                pars.CloneProfile.ExcludeFolders = EditorExcludeFolders.Text.Split();
                pars.CloneProfile.ReplaceFiles = EditorReplaceFiles.Text.Split();

                TabLog.IsSelected = true;

                new TaskFactory().StartNew(() => builder.Clone(pars));
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
    }
}