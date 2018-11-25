using System.ComponentModel;

namespace Grappachu.SolutionCloner.Common.Dialogs
{
    /// <inheritdoc />
    /// <summary>
    ///     Rappresenta il viewModel della finestra di dialogo <see cref="T:Grappachu.SolutionCloner.Common.Dialogs.OverlayDialog" />
    /// </summary>
    public interface IOverlayDialogPresenter : INotifyPropertyChanged
    {
        /// <summary>
        ///     Richiamato su richiesta di chiusura e restituisce true se il controllo acconsente all'uscita
        /// </summary>
        /// <returns></returns>
        bool RequestClose();
    }
}