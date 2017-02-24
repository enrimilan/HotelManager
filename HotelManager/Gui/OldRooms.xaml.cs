using HotelManager.Entity;
using HotelManager.Service;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HotelManager.Gui
{
    /// <summary>
    /// Interaction logic for OldRooms.xaml
    /// </summary>
    public partial class OldRooms : BaseFrameWithSearch<Room>
    {
        
        private RoomService roomService = ServiceFactory.GetRoomService();

        public OldRooms()
        {
            InitializeComponent();
        }

        protected override void BaseFrame_Loaded(object sender, RoutedEventArgs e)
        {
            base.BaseFrame_Loaded(sender, e);
            emptyListMessage.Text = "No rooms.";
            GridView gridView = list.View as GridView;
            gridView.Columns.Add(CreateColumn("Number", "Number"));
            gridView.Columns.Add(CreateColumn("MovedDateString", "Moved"));
            gridView.Columns.Add(CreateColumn("CreationDateString", "Created"));
            ReloadData("");
        }

        protected override void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            base.Worker_DoWork(sender, e);
            string query = (string)e.Argument;
            items = roomService.FindRoom(query, true);
        }

        protected override ContextMenu BuildMenu(int index)
        {
            ContextMenu menu = new ContextMenu();
            Room room = items[list.SelectedIndex];
            MenuItem MoveBackToActualRooms = new MenuItem();
            MoveBackToActualRooms.Header = "Move back to actual rooms";
            MoveBackToActualRooms.Click += MoveBackToActualRooms_Click;
            menu.Items.Add(MoveBackToActualRooms);
            return menu;
        }

        private void MoveBackToActualRooms_Click(object sender, RoutedEventArgs e)
        {
            if (list.SelectedIndex == -1)
            {
                return;
            }
            Room room = items[list.SelectedIndex];
            room.IsOld = false;
            roomService.Edit(room);
            searchBox.Visibility = Visibility.Hidden;
            ReloadData(searchBox.SearchTextBox.Text);
        }
        
    }
}
