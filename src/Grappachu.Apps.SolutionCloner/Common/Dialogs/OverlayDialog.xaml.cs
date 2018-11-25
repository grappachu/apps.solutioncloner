using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Grappachu.SolutionCloner.Common.Dialogs
{
   /// <summary>
    /// Interaction logic for OverlayDialog.xaml
    /// </summary>
    public partial class OverlayDialog
    {
        public OverlayDialog()
        {
            InitializeComponent();

        }

        private void OverlayDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Add focus between tolerances?
        }


        public static OverlayDialog Activate(string title, UserControl control)
        {
            var dlg = new OverlayDialog { TxtTitle = { Text = title } };
            if (control != null)
            {
                // dlg.Workspace.Children.Clear();
                control.HorizontalAlignment = HorizontalAlignment.Stretch;
                control.VerticalAlignment = VerticalAlignment.Stretch;
                dlg.Workspace.Children.Add(control);
                if (control is IOverlayDialogInteractiveControl interactiveControl)
                {
                    dlg.DataContext = interactiveControl.GetDialogPresenter();
                }
            }
            dlg.ShowDialog();

            return dlg;
        }



        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OverlayDialog_OnClosing(object sender, CancelEventArgs e)
        {
            if (DataContext is IOverlayDialogPresenter control && !control.RequestClose())
            {
                e.Cancel = true;
            }
        }
    }
}
