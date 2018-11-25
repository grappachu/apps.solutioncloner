using System.Windows;
using System.Windows.Controls;
using Grappachu.Core.Lang.Extensions;

namespace Grappachu.SolutionCloner.Common.Utils.DragDrop
{

    /// <summary>
    /// Raccoglie un insieme di funzioni per semplificare le operazioni di DragDrop
    /// </summary>

    internal static class DragDropUtils
    {
        /// <summary>
        /// Crea una nuova istanza di <see cref="IDragDropConfigurator{T}" /> per il componente <param name="targetList"></param>
        /// </summary>
        /// <example>
        /// <code>
        ///     private void ListView_OnLoaded(object sender, RoutedEventArgs e)
        ///     {
        ///         if (sender is ListView listView)
        ///         {
        ///             var dragDropConfigurator = DragDropUtils.CreateConfigurator&lt;MyDataModel&gt;(listView);
        ///             dragDropConfigurator.ConfigureDrag();
        ///             dragDropConfigurator.ConfigureDrop(new ListViewDropCallback&lt;MyDataModel&gt;((dropped, idx) =>
        ///             {
        ///                 var viewModel = (RecipesPresenter)DataContext;
        ///                 if (idx &lt; 0)
        ///                     viewModel.Add(dropped);
        ///                 else
        ///                     viewModel.Insert(dropped, idx);
        ///             }));
        ///         }
        ///     }
        /// </code>
        /// </example>
        /// <typeparam name="T">Il tipo di dato che deve essere trasportato (dovrebbe essere il tipo di ItemSource della lista)</typeparam>
        /// <param name="targetList">Il controllo su cui deve essere configurato il DragDrop</param>
        /// <param name="dataFormat">Il formato dei dati da utilizzare per il trasporto. Vedi <see cref="IDragDropConfigurator{T}"/>.
        /// Se non viene specificato nulla sarà usato un formato personalizzato basato sul tipo <see cref="DataFormats"/> </param>
        /// <returns></returns>
        public static IDragDropConfigurator<T> CreateConfigurator<T>(ListView targetList, string dataFormat = null)
        {
            var format = dataFormat.Or(typeof(T).Name);
            var handler = new ListViewDragDropConfigurator<T>(targetList, format);
            return handler;
        }

    }
}