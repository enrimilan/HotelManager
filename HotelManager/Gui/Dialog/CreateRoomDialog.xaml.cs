using System;
using System.Windows;
using System.Windows.Input;

namespace HotelManager.Gui.Dialog
{
    /// <summary>
    /// Interaction logic for CreateRoomDialog.xaml
    /// </summary>
    public partial class CreateRoomDialog : Window
    {

        public bool Create = false;

        public CreateRoomDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// CloseButton_Clicked
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// CreateButton_Clicked
        /// </summary>
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Create = true;
            this.Close();
        }

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

    }
}
