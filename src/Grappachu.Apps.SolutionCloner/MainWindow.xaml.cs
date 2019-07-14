using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Grappachu.Apps.SolutionCloner.Engine.Components;
using Grappachu.Apps.SolutionCloner.Engine.Components.Scanner;
using Grappachu.Apps.SolutionCloner.Engine.Components.Templates;
using Grappachu.Apps.SolutionCloner.Engine.Interfaces;
using Grappachu.Apps.SolutionCloner.Engine.Model;
using Grappachu.Apps.SolutionCloner.Engine.Model.Templates;
using Grappachu.Core.Collections;
using Grappachu.Core.Lang.Extensions;
using Grappachu.Core.Preview.UI;
using Grappachu.SolutionCloner.Common.Dialogs;
using Grappachu.SolutionCloner.Common.Utils.DragDrop;
using Grappachu.SolutionCloner.Properties;
using Grappachu.SolutionCloner.UI;
using log4net.Core;

namespace Grappachu.SolutionCloner
{
    /// <summary>
    ///     Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly SolutionScanner _scanner;
        private readonly ITemplateEnumerator _templateEnumerator;
        public MainWindow()
        {
            _scanner = new SolutionScanner();
            _templateEnumerator = new TemplateEnumerator(ReadConfig.TemplateFolder);
            InitializeComponent();
        }

        private static string CurrentVersion => ApplicationDeployment.IsNetworkDeployed
            ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
            : Assembly.GetExecutingAssembly().GetName().Version.ToString();


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

                DataContext = new MainWindowViewModel(_templateEnumerator);

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
                var builder = new Apps.SolutionCloner.Engine.SolutionCloner(new FileManager(), new CloneSettingsValidator(), new UpdaterFactory<IFileUpdater>());
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

                if (VieModel.CloneFromTemplate == true)
                {
                    TemplateInfo template = VieModel.SelectedTemplate;

                    var placeholders = GetPlaceholders(template);
                    pars.CloneProfile.ExtraReplacements.AddRange(placeholders);

                    var control = new PlaceholderEditor();
                    control.DataContext = new PlaceholderEditorViewModel(pars.CloneProfile, () =>
                    {
                        var parent = control.FindAnchestor<Window>();
                        parent.DialogResult = true;
                        parent.Close();
                    });
                    var dlg = OverlayDialog.Activate("Configure parameters", control);
                    if (dlg.DialogResult != true)
                    {
                        return;
                    }

                    foreach (var itemReference in template.SolutionItems)
                    {
                        var cr = new CloneReplacement
                        {
                            Placeholder = itemReference.Value,
                            Value = Guid.NewGuid().ToString(),
                            IgnoreCase = true
                        };
                        pars.CloneProfile.ExtraReplacements.Add(cr);
                    }
                }


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

        private static IEnumerable<CloneReplacement> GetPlaceholders(TemplateInfo template)
        {
            var list = new List<CloneReplacement>();
            foreach (var placeholder in template.Placeholders)
            {
                string value = string.Empty;
                switch (placeholder.Key.ToLower())
                {
                    case "§{author}":
                        value = Environment.UserName;
                        break;
                    case "§{today}":
                        value = DateTime.Today.ToString("yyyy-MM-dd");
                        break;
                    case "§{year}":
                        value = DateTime.Today.ToString("yyyy");
                        break;
                }

                var cr = new CloneReplacement
                {
                    Placeholder = placeholder.Key,
                    Value = value,
                    IgnoreCase = false,
                    Description = placeholder.Description,
                    IsMandatory = placeholder.IsMandatory
                };
                list.Add(cr);
            }

            return list;
        }


        private MainWindowViewModel VieModel => DataContext as MainWindowViewModel;

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

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                var template = viewModel.SelectedTemplate;
                if (template != null)
                {
                    TxtNewKey.Text = template.Namespace;
                    TxtSource.SelectedValue = template.GetCloneableRoot();
                }
            }
        }
    }
}