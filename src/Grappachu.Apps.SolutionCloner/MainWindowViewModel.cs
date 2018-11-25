using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Grappachu.Apps.SolutionCloner.Engine.Interfaces;
using Grappachu.Apps.SolutionCloner.Engine.Model.Templates;
using Grappachu.SolutionCloner.Common;

namespace Grappachu.SolutionCloner
{
    internal class MainWindowViewModel : ObservableObject
    {
        private readonly ITemplateEnumerator _templateEnumerator;

        public MainWindowViewModel(ITemplateEnumerator templateEnumerator)
        {
            _templateEnumerator = templateEnumerator;

            InitDataAsync();
        }

        public ObservableCollection<TemplateInfo> AvailableTemplates { get; set; }

        public static readonly DependencyProperty CloneFromTemplateProperty = DependencyProperty.Register("CloneFromTemplate", typeof(bool?), typeof(MainWindowViewModel), new PropertyMetadata(default(bool?)));
        public static readonly DependencyProperty SelectedTemplateProperty = DependencyProperty.Register("SelectedTemplate", typeof(TemplateInfo), typeof(MainWindowViewModel), new PropertyMetadata(default(TemplateInfo)));
      
        public TemplateInfo SelectedTemplate
        {
            get => (TemplateInfo)GetValue(SelectedTemplateProperty);
            set => SetValue(SelectedTemplateProperty, value);
        }

        public bool? CloneFromTemplate
        {
            get => (bool?)GetValue(CloneFromTemplateProperty);
            set => SetValue(CloneFromTemplateProperty, value);
        }


        private void InitDataAsync()
        {
            new TaskFactory().StartNew(() =>
            {
                AvailableTemplates = new ObservableCollection<TemplateInfo>(_templateEnumerator.GetTemplates());
                RaisePropertyChangedEvent(nameof(AvailableTemplates));
            });
        }

       
    }
}