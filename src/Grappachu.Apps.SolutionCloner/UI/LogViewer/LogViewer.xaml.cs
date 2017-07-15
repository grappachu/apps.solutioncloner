using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Grappachu.SolutionCloner.Properties;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace Grappachu.SolutionCloner.UI.LogViewer
{
    /// <summary>
    ///     Logica di interazione per LogView.xaml
    /// </summary>
    public partial class LogViewer
    {
        private LogViewAppender _appender;
        private Level _displayLevel;
        private int _maxItems;

        /// <summary>
        ///     Istanza un nuovo LogViewer
        /// </summary>
        public LogViewer()
        {
            InitializeComponent();
            DisplayLevel = Level.Info;
            MaxItems = 0;
        }

        public void Clear()
        {
            List.Items.Clear();
        }


        /// <summary>
        ///     Ottiene o imposta un numero massimo di elementi da visualizzare nella lista
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        public int MaxItems
        {
            get { return _maxItems; }
            set
            {
                _maxItems = value;
                if (_appender != null)
                {
                    _appender.MaxItems = value;
                    OnPropertyChanged(nameof(MaxItems));
                }
            }
        }


        /// <summary>
        ///     Ottiene o imposta il livello di log minimo da visualizzare
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        public Level DisplayLevel
        {
            get { return _displayLevel; }
            set
            {
                _displayLevel = value;
                if (_appender != null)
                {
                    _appender.Threshold = value;
                    OnPropertyChanged(nameof(DisplayLevel));
                }
            }
        }

        /// <summary>
        ///     Viene generato quando il valore di una proprietà cambia.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void LogView_OnLoaded(object sender, RoutedEventArgs e)
        {
            AttachLogger();
            SetLevel(_appender.Threshold);
        }

        private void LogView_OnUnloaded(object sender, RoutedEventArgs e)
        {
            DetachLogger();
        }

        private bool _isAppenderAttached;
        private readonly object _isAppenderAttachedLock = new object();

        private void AttachLogger()
        {
            lock (_isAppenderAttachedLock)
            {
                if (!_isAppenderAttached)
                {
                    _appender = new LogViewAppender(List);
                    BasicConfigurator.Configure(_appender);
                    _appender.Threshold = _displayLevel;
                    _appender.MaxItems = MaxItems;
                    _appender.ActivateOptions();
                    _isAppenderAttached = true;
                }
            }
        }

        private void DetachLogger()
        {
            lock (_isAppenderAttachedLock)
            {
                if (!_isAppenderAttached)
                {
                    var root = ((Hierarchy)LogManager.GetRepository()).Root;
                    var attachable = (IAppenderAttachable)root;
                    if (attachable != null)
                    {
                        attachable.RemoveAppender(_appender);
                        _appender.Close();
                    }
                    _isAppenderAttached = false;
                }
            }
        }




        private void SetLevel(Level level)
        {
            _appender.Threshold = level;
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private class LogViewAppender : AppenderSkeleton
        {
            private const string TimestampFormatString = @"HH:mm:ss";
            private readonly ListView _lstLog;

            public LogViewAppender(ListView lstLog)
            {
                _lstLog = lstLog;
                Threshold = Level.Info;
            }

            public int MaxItems { get; set; }

            private bool HasWarnings { get; set; }


            protected override void Append(LoggingEvent loggingEvent)
            {
                if (loggingEvent != null)
                {
                    if (loggingEvent.Level >= Level.Warn)
                    {
                        HasWarnings = true;
                    }

                    if (!_lstLog.Dispatcher.CheckAccess())
                    {
                         _lstLog.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => Append(loggingEvent)));
                    }
                    else
                    {
                        var item = new MyLogItem(loggingEvent);

                        if (IsScrolledToEnd())
                        {
                            _lstLog.Items.Add(item);
                            _lstLog.ScrollIntoView(item);
                        }
                        else
                        {
                            _lstLog.Items.Add(item);
                        }

                        if (MaxItems > 0)
                        {
                            while (_lstLog.Items.Count > MaxItems)
                            {
                                _lstLog.Items.RemoveAt(0);
                            }
                        }
                    }
                }
            }


            private bool IsScrolledToEnd()
            {
                return _lstLog.SelectedItem == null;
            }


            public void ClipLogs()
            {
                var sb = new StringBuilder();
                foreach (MyLogItem logItem in _lstLog.Items)
                {
                    sb.AppendFormat("{0} | {1} {2}" + Environment.NewLine,
                        logItem.TimeStamp.ToString(TimestampFormatString),
                        logItem.Level.ToString().PadRight(6),
                        logItem.RenderedMessage);
                }
                Clipboard.SetText(sb.ToString());
            }
        }

        internal class MyLogItem
        {
            public MyLogItem(LoggingEvent le)
            {
                TimeStamp = le.TimeStamp;
                Level = le.Level;
                RenderedMessage = le.RenderedMessage;
            }

            public DateTime TimeStamp { get; set; }
            public Level Level { get; set; }
            public string RenderedMessage { get; set; }

            public Brush Foreground
            {
                get
                {
                    if (Level >= Level.Error)
                    {
                        return Brushes.Red;
                    }
                    if (Level >= Level.Warn)
                    {
                        return Brushes.DarkOrange;
                    }
                    if (Level >= Level.Info)
                    {
                        return Brushes.Black;
                    }
                    if (Level >= Level.Debug)
                    {
                        return Brushes.Gray;
                    }
                    return Brushes.BlueViolet;
                }
            }
        }
    }
}