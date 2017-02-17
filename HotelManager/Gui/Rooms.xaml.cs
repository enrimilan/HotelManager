using HotelManager.Entity;
using HotelManager.Gui.Dialog;
using HotelManager.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelManager.Gui
{
    /// <summary>
    /// Interaction logic for Rooms.xaml
    /// </summary>
    public partial class Rooms : UserControl
    {

        private List<Room> items = new List<Room>();
        private RoomService roomService = ServiceFactory.GetRoomService();
        public Rooms()
        {
            InitializeComponent();

            roomList.ItemsSource = roomService.FindAllRooms();

        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (roomList.SelectedIndex == -1)
            {
                return;
            }

            //TODO
            /*
            items.RemoveAt(lvUsers.SelectedIndex);
            roomList.Items.Refresh();
            MessageDialog messageDialog = new MessageDialog();
            messageDialog.Owner = Application.Current.MainWindow;
            messageDialog.setMessage("Deleted room!");
            messageDialog.ShowDialog();*/
            
        }
    }
   
}
