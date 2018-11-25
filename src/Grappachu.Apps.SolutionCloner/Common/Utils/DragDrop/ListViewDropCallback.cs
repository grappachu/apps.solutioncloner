using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Grappachu.SolutionCloner.Common.Utils.DragDrop
{
    public class ListViewDropCallback<TData> : IDropCallback
    {
        private readonly Action<TData, int> _callback;
        private readonly string _dataFormat;

        public ListViewDropCallback(Action<TData, int> callback, string dataFormat = null)
        {
            _callback = callback;
            _dataFormat = dataFormat ?? typeof(TData).Name; 
        }

        public void DoDrop(object sender, DragEventArgs e)
        {
            OnDoDrop(sender, e);
        }

        protected virtual void OnDoDrop(object sender, DragEventArgs e)
        {
            int dropIndex = -1; // default position directong to add() call
            var listView = sender as ListView;

            // checking drop destination position
            Point pt = e.GetPosition((UIElement)sender);
            HitTestResult result = VisualTreeHelper.HitTest(listView, pt);
            if (result != null && result.VisualHit != null)
            {
                // checking the object behin the drop position (Item type depend)
                var theOne = result.VisualHit.FindAnchestor<ListViewItem>();

                // identifiing the position according bound view model (context of item)
                if (theOne != null)
                {
                    //identifing the position of drop within the item
                    var itemCenterPosY = theOne.ActualHeight / 2;
                    var dropPosInItemPos = e.GetPosition(theOne);

                    // geting the index
                    var itemIndex = listView.Items.IndexOf(theOne.Content);

                    // decission if insert before or below
                    if (dropPosInItemPos.Y > itemCenterPosY)
                    {  // when drag is gropped in second half the item is inserted bellow
                        itemIndex = itemIndex + 1;
                    }
                    dropIndex = itemIndex;
                }
            }
             

            if (e.Data.GetDataPresent(_dataFormat))
            {
                TData droppedItem = (TData)e.Data.GetData(_dataFormat);
                _callback.Invoke(droppedItem, dropIndex);
            }

            e.Handled = true;
        }
    }
}