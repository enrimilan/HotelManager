using HotelManager.Entity;
using HotelManager.Gui.Dialog;
using HotelManager.Service;
using HotelManager.Util;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HotelManager.Gui
{
    /// <summary>
    /// Interaction logic for Rooms.xaml
    /// </summary>
    public partial class Rooms : BaseFrameWithSearch<Room>
    {

        private RoomService roomService = ServiceFactory.GetRoomService();

        public Rooms()
        {
            InitializeComponent();
        }

        protected override void BaseFrame_Loaded(object sender, RoutedEventArgs e)
        {
            base.BaseFrame_Loaded(sender, e);
            emptyListMessage.Text = "No rooms.";
            GridView gridView = list.View as GridView;
            gridView.Columns.Add(CreateColumn("Number", "Number"));
            gridView.Columns.Add(CreateColumn("Status", "Status"));
            gridView.Columns.Add(CreateColumn("Reservations", "Reservations"));
            gridView.Columns.Add(CreateColumn("CreationDateString", "Created"));
            ReloadData("");
        }

        protected override void Worker_OnPreExecute()
        {
            base.Worker_OnPreExecute();
            createButton.Visibility = Visibility.Hidden;
        }

        protected override void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            base.Worker_DoWork(sender, e);
            string query = (string)e.Argument;
            items = roomService.FindRoom(query, false);
        }

        protected override void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            base.Worker_RunWorkerCompleted(sender, e);
            createButton.Visibility = Visibility.Visible;
        }

        protected override ContextMenu BuildMenu(int index)
        {
            ContextMenu menu = new ContextMenu();
            Room room = items[list.SelectedIndex];

            MenuItem editReservations = new MenuItem();
            editReservations.Header = "Edit reservations";
            editReservations.Click += EditReservations_Click;
            menu.Items.Add(editReservations);

            if (room.Reservations == 0 && room.Status.Equals("Free"))
            {
                MenuItem moveToOldRooms = new MenuItem();
                moveToOldRooms.Header = "Move to old rooms";
                moveToOldRooms.Click += MoveToOldRooms_Click;
                menu.Items.Add(moveToOldRooms);
            }

            return menu;
        }

        private void MoveToOldRooms_Click(object sender, RoutedEventArgs e)
        {
            if (list.SelectedIndex == -1)
            {
                return;
            }
            Room room = base.items[list.SelectedIndex];
            room.IsOld = true;
            room.MovedDateString = DateTime.Now.ToString(Constants.DateFormat);
            roomService.Edit(room);
            searchBox.Visibility = Visibility.Hidden;
            ReloadData(searchBox.SearchTextBox.Text);
        }

        private void EditReservations_Click(object sender, RoutedEventArgs e)
        {
            if (list.SelectedIndex == -1)
            {
                return;
            }

            Main main = (Main)Window.GetWindow(this);
            main.container.Dispatcher.Invoke(delegate
            {
                main.container.NavigationService.Navigate(new ReservationsForRoom(items[list.SelectedIndex]));
            });
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            CreateRoomDialog createRoomDialog = new CreateRoomDialog();
            createRoomDialog.Dialog_Title = "Create a new room";
            createRoomDialog.Owner = Application.Current.MainWindow;
            createRoomDialog.ShowDialog();

            if (createRoomDialog.Create && createRoomDialog.CheckForErrorsAndProceed())
            {
                searchBox.Visibility = Visibility.Hidden;
                ReloadData(searchBox.SearchTextBox.Text);
            }
        }

    }
}
