using System.Windows;
using System.Windows.Input;

namespace HotelManager.Gui.Dialog
{
    public class BaseDialog : Window
    {

        public bool Create; 

        public BaseDialog()
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ResizeMode = ResizeMode.NoResize;
        }

        protected void Dialog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        protected void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Create = true;
            Close();
        }

        protected void Dialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Create = true;
                Close();
            }
        }

    }
}
