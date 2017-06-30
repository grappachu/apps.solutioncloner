using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Grappachu.SolutionCloner.Properties;
using Grappachu.SolutionCloner.UI.Util.CommonInternal; 

namespace Grappachu.SolutionCloner.UI.PathSelector
{
    /// <summary>
    ///     Interaction logic for PathSelector.xaml
    /// </summary>
    public sealed partial class PathSelector : INotifyPropertyChanged
    {
        private bool _allowCreateFolders;
        private ActivationEvent _browseEvent;
        private string _dialogTitle;
        private bool _directoryMode;
        private string _fileFilter;
        private string _value;
        private string _initialDirectory;

        /// <summary>
        /// 
        /// </summary>
        public PathSelector()
        {
            InitializeComponent();

            BrowseEvent = ActivationEvent.Default;
            DirectoryMode = false;
            SelectedValue = null;
            IsManuallyEditable = true;
            FileFilter = null;
            DialogTitle = null;
            BrowseButtonVisible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        public ActivationEvent BrowseEvent
        {
            get { return _browseEvent; }
            set
            {
                if (value == _browseEvent) return;
                _browseEvent = value;
                OnPropertyChanged(nameof(BrowseEvent));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        public bool DirectoryMode
        {
            get { return _directoryMode; }
            set
            {
                if (value == _directoryMode) return;
                _directoryMode = value;
                OnPropertyChanged(nameof(DirectoryMode));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        public bool AllowCreateFolders
        {
            get { return _allowCreateFolders; }
            set
            {
                if (value == _allowCreateFolders) return;
                _allowCreateFolders = value;
                OnPropertyChanged(nameof(AllowCreateFolders));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        public bool IsManuallyEditable
        {
            get { return !TxtPath.IsReadOnly; }
            set
            {
                TxtPath.IsReadOnly = !value;
                OnPropertyChanged(nameof(IsManuallyEditable));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public ImageSource WatermarkIcon
        {
            get { return ImgIcon.Source; }
            set
            {
                ImgIcon.Source = value;
                OnPropertyChanged(nameof(WatermarkIcon));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public string WatermarkText
        {
            get { return TxtWatermark.Text; }
            set
            {
                TxtWatermark.Text = value;
                OnPropertyChanged(nameof(WatermarkText));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Category("Common")]
        public string SelectedValue
        {
            get { return _value; }
            set
            {
                _value = value;
                TxtPath.Text = _value;
                if (_value != null)
                {
                    _initialDirectory = _value;
                }
                OnPropertyChanged(nameof(SelectedValue));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Category("Common")]
        public string InitialDirectory
        {
            get
            {
                return _initialDirectory;
            }
            set
            {
                _initialDirectory = value; 
                OnPropertyChanged(nameof(InitialDirectory));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        public string FileFilter
        {
            get { return _fileFilter; }
            set
            {
                if (value == _fileFilter) return;
                _fileFilter = value;
                OnPropertyChanged(nameof(FileFilter));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public string DialogTitle
        {
            get { return _dialogTitle; }
            set
            {
                if (value == _dialogTitle) return;
                _dialogTitle = value;
                OnPropertyChanged(nameof(DialogTitle));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public bool BrowseButtonVisible
        {
            get { return BtnBrowse.Visibility == Visibility.Visible; }
            set
            {
                BtnBrowse.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged(nameof(BrowseButtonVisible));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public object BrowseButtonContent
        {
            get { return BtnBrowse.Content; }
            set
            {
                BtnBrowse.Content = value;
                OnPropertyChanged(nameof(BrowseButtonContent));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        private string GetInitialDirectory()
        {
            if (DirectoryMode)
            {
                return Directory.Exists(SelectedValue)
                    ? SelectedValue
                    : InitialDirectory;
            }

            var root = string.IsNullOrEmpty(SelectedValue)
                ? InitialDirectory
                : Path.GetDirectoryName(SelectedValue) ?? string.Empty;
            return Directory.Exists(root)
                ? root
                : Directory.GetCurrentDirectory();
        }

        private void ChoosePath(string startupPath)
        {
            if (DirectoryMode)
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    if (DialogTitle != null)
                    {
                        dialog.Description = DialogTitle;
                    }
                    dialog.SelectedPath = startupPath;
                    dialog.ShowNewFolderButton = AllowCreateFolders;
                    var result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        SelectedValue = dialog.SelectedPath;
                    }
                }
            }
            else
            {
                using (var dialog = new OpenFileDialog())
                {
                    if (DialogTitle != null)
                    {
                        dialog.Title = DialogTitle;
                    }
                    dialog.Multiselect = false;
                    dialog.InitialDirectory = startupPath;
                    if (!string.IsNullOrEmpty(FileFilter))
                    {
                        dialog.Filter = FileFilter;
                    }
                    var result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        SelectedValue = dialog.FileName;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsValid()
        {
            return DirectoryMode ? Directory.Exists(SelectedValue) : File.Exists(SelectedValue);
        }

        private void WatermarkTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FlagsHelper.IsSet(BrowseEvent, ActivationEvent.DoubleClick))
            {
                ChoosePath(GetInitialDirectory());
            }
        }

        private void WatermarkTextBox_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (FlagsHelper.IsSet(BrowseEvent, ActivationEvent.Click))
            {
                ChoosePath(GetInitialDirectory());
            }
        }

        private void TxtPath_OnTouchDown(object sender, TouchEventArgs e)
        {
            if (FlagsHelper.IsSet(BrowseEvent, ActivationEvent.Touch))
            {
                ChoosePath(GetInitialDirectory());
            }
        }

        private void PathSelector_OnLoaded(object sender, RoutedEventArgs e)
        {
            ImgIcon.Height = FontSize;
            ImgIcon.Margin = new Thickness(ImgIcon.Margin.Left, ImgIcon.Margin.Top, FontSize/2, ImgIcon.Margin.Bottom);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            if (BrowseEvent != ActivationEvent.None)
            {
                ChoosePath(GetInitialDirectory());
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum ActivationEvent
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Click = 1,
        /// <summary>
        /// 
        /// </summary>
        DoubleClick = 2,
        /// <summary>
        /// 
        /// </summary>
        Touch = 4,
        /// <summary>
        /// 
        /// </summary>
        Default = DoubleClick | Touch
    }
}