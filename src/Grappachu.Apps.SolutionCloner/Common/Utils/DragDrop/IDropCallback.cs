using System.Windows;

namespace Grappachu.SolutionCloner.Common.Utils.DragDrop
{
    /// <summary>
    /// Rappresenta una generica callback per un'operazione di drop
    /// </summary>
    public interface IDropCallback
    {
        void DoDrop(object sender, DragEventArgs e);
    }
}