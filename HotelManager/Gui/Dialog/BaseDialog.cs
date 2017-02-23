using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotelManager.Gui.Dialog
{
    public class BaseDialog : Window
    {

        public bool Create;
        public string Dialog_Title;

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

        protected void BaseDialog_Loaded(object sender, RoutedEventArgs e)
        {

            BaseDialogControl control = Content as BaseDialogControl;

            Grid Dialog_Grid = control.Template.FindName("Dialog_Grid", control) as Grid;
            Dialog_Grid.MouseDown += Dialog_MouseDown;
            Dialog_Grid.KeyDown += Dialog_KeyDown;

            TextBlock title = control.Template.FindName("Dialog_Title", control) as TextBlock;
            title.Text = Dialog_Title;

            Button closeButton = control.Template.FindName("CloseButton", control) as Button;
            closeButton.Click += CloseButton_Click;

        }

    }
}
