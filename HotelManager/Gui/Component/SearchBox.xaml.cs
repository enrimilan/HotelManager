using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotelManager.Gui.Component
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {

        public SearchBox()
        {
            InitializeComponent();
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var t = (TextBox)sender;
            t.SelectAll();
        }

        private void SearchTextBox_GotMouseCapture(object sender, MouseEventArgs e)
        {
            var t = (TextBox)sender;
            t.SelectAll();
        }
    
    }
}
