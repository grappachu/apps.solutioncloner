using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Grappachu.Apps.SolutionCloner.Engine.Model;
using Grappachu.SolutionCloner.Common;
using Grappachu.SolutionCloner.Common.Dialogs;

namespace Grappachu.SolutionCloner.UI
{
    /// <summary>
    /// Interaction logic for PlaceholderEditor.xaml
    /// </summary>
    public partial class PlaceholderEditor : IOverlayDialogInteractiveControl
    {
        public PlaceholderEditor()
        {
            InitializeComponent();
        }

        public IOverlayDialogPresenter GetDialogPresenter()
        {
            return DataContext as PlaceholderEditorViewModel;
        }
    }

    public class PlaceholderEditorViewModel : DependencyObject, IOverlayDialogPresenter
    {
        private readonly CloneProfile _parsCloneProfile;
        private readonly Action _saveCallback;

        public PlaceholderEditorViewModel(CloneProfile parsCloneProfile, Action saveCallback)
        {
            _parsCloneProfile = parsCloneProfile;
            _saveCallback = saveCallback;
            Items = new ObservableCollection<ItemEditor>();
            SaveAndClose = new RelayCommand(OnSaveCommand);
            InitAsync();
        }


        private void OnSaveCommand(object p)
        {
            var updates = Items.ToArray();
            foreach (var update in updates)
            {
                var entry = _parsCloneProfile.ExtraReplacements.Single(x => x.Placeholder == update.Key);
                entry.Value = update.Val;
            }
            _saveCallback.Invoke();
        }

        private void InitAsync()
        {
            new TaskFactory().StartNew(() =>
            {
                Items = new ObservableCollection<ItemEditor>(
                    _parsCloneProfile.ExtraReplacements.Select(x => new ItemEditor
                    { 
                        Key = x.Placeholder, 
                        Val = x.Value,
                        Description = x.Description,
                        IsMandatory = x.IsMandatory
                    }));
                
            });
        }


        public ObservableCollection<ItemEditor> Items { get; set; }

        public ICommand SaveAndClose { get; }


        public event PropertyChangedEventHandler PropertyChanged;

        public bool RequestClose()
        {
            return true;
        }
    }

    public class ItemEditor
    {
        public string Key { get; set; }
        public string Val { get; set; }
        public string Description { get; set; }
        public bool IsMandatory { get; set; }
    }
}
