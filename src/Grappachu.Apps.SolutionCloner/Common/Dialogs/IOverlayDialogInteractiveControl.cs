namespace Grappachu.SolutionCloner.Common.Dialogs
{
    /// <summary>
    /// Rappresenta un componente in grado di interagire con un contenitore <see cref="OverlayDialog"/>
    /// </summary>
    public interface IOverlayDialogInteractiveControl
    {
        IOverlayDialogPresenter GetDialogPresenter();
    }
}