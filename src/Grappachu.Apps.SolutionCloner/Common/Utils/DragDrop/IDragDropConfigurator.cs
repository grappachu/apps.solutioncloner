namespace Grappachu.SolutionCloner.Common.Utils.DragDrop
{
    /// <summary>
    ///     Rappresenta un componente per la configurazione delle operazioni di DragDrop sui controlli
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDragDropConfigurator<out T>
    {
        /// <summary>
        ///     Attiva il drag degli elementi di tipo <see cref="T" /> per il componente associato a questo configuratore
        /// </summary>
        void ConfigureDrag();

        /// <summary>
        ///     Attiva il drop degli elementi di tipo <see cref="T" /> sul il componente associato a questo configuratore
        /// </summary>
        /// <param name="onDropAction">Callback richiamata al momento del drop</param>
        void ConfigureDrop(IDropCallback onDropAction);
    }
}