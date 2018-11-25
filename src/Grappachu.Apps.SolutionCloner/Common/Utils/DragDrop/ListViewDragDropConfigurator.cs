using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Grappachu.SolutionCloner.Common.Utils.DragDrop
{
    internal class ListViewDragDropConfigurator<TData> : IDragDropConfigurator<TData>
    {
        private readonly string _dataFormat;
        private readonly ListView _listView;
        private Point _dragStartPoint;
        private IDropCallback _onDropCallback;

        public ListViewDragDropConfigurator(ListView listView, string dataFormat)
        {
            _listView = listView ?? throw new ArgumentNullException(nameof(listView));
            _dataFormat = dataFormat ?? throw new ArgumentNullException(nameof(dataFormat));
        }


        public void ConfigureDrag()
        {
            _listView.MouseMove += GiveFeedback;
            _listView.PreviewMouseLeftButtonDown += SaveStartPosition;
        }

        public void ConfigureDrop(IDropCallback onDropCallback)
        {
            if (_onDropCallback != null) throw new InvalidOperationException("DragDrop already configured on " + _listView.Name);
            _onDropCallback = onDropCallback;
            _listView.AllowDrop = true;
            _listView.DragEnter += TargetListOnDragEnter;
            _listView.Drop += TargetListOnDrop;
        }
    
        private void TargetListOnDrop(object sender, DragEventArgs e)
        {
            _onDropCallback.DoDrop(sender, e);
        }

        private void TargetListOnDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(_dataFormat) ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }


        private void SaveStartPosition(object sender, MouseButtonEventArgs e)
        {
            // Store the mouse position
            _dragStartPoint = e.GetPosition(null);
        }



        private void GiveFeedback(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = _dragStartPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = (ListView)sender;
                ListViewItem listViewItem = ((DependencyObject)e.OriginalSource).FindAnchestor<ListViewItem>();

                //TODO: controllare
                if (listViewItem == null)
                    return;

                // Find the data behind the ListViewItem
                var contact = (TData)listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

                if (contact == null)
                    return;
                

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject(_dataFormat, contact);

                // TODO: estendere il metodo ConfigureDrag per passare un IDragDropEffectsResolver (opzionale)
                System.Windows.DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }
        }
    }



    public static class WpfDomHelper
    {
        // Helper to search up the VisualTree
        public static T FindAnchestor<T>(this DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T variable)
                {
                    return variable;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
    }
}