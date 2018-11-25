using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Grappachu.SolutionCloner.Common.Utils
{
    /// <summary>
    ///     https://stackoverflow.com/questions/3813087/listview-scroll-to-last-item-wpf-c-sharp
    /// </summary>
    public static class ListViewExtensions
    {
        public static readonly DependencyProperty AutoScrollToEndProperty =
            DependencyProperty.RegisterAttached("AutoScrollToEnd", typeof(bool), typeof(ListViewExtensions),
                new UIPropertyMetadata(OnAutoScrollToEndChanged));

        private static readonly DependencyProperty AutoScrollToEndHandlerProperty =
            DependencyProperty.RegisterAttached("AutoScrollToEndHandler", typeof(NotifyCollectionChangedEventHandler),
                typeof(ListViewExtensions));

        public static bool GetAutoScrollToEnd(DependencyObject obj)
        {
            return (bool) obj.GetValue(AutoScrollToEndProperty);
        }

        public static void SetAutoScrollToEnd(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollToEndProperty, value);
        }

        private static void OnAutoScrollToEndChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var listView = s as ListView;

            if (listView == null)
                return;

            var source = (INotifyCollectionChanged) listView.Items.SourceCollection;

            if ((bool) e.NewValue)
            {
                void ScrollToEndHandler(object sender, NotifyCollectionChangedEventArgs args)
                {
                    if (listView.Items.Count <= 0) return;
                    listView.Items.MoveCurrentToLast();
                    listView.ScrollIntoView(listView.Items.CurrentItem);
                }

                source.CollectionChanged += ScrollToEndHandler;

                listView.SetValue(AutoScrollToEndHandlerProperty,
                    (NotifyCollectionChangedEventHandler) ScrollToEndHandler);
            }
            else
            {
                var handler = (NotifyCollectionChangedEventHandler) listView.GetValue(AutoScrollToEndHandlerProperty);

                source.CollectionChanged -= handler;
            }
        }
    }
}