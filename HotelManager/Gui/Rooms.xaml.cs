using HotelManager.Async;
using HotelManager.Entity;
using HotelManager.Gui.Dialog;
using HotelManager.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
namespace HotelManager.Gui
{
    /// <summary>
    /// Interaction logic for Rooms.xaml
    /// </summary>
    public partial class Rooms : UserControl
    {

        private List<Room> items = new List<Room>();
        private RoomService roomService = ServiceFactory.GetRoomService();
        private AbortableBackgroundWorker worker = new AbortableBackgroundWorker();

        public Rooms()
        {
            InitializeComponent();
            searchBox.AddHandler(TextBox.TextChangedEvent, new TextChangedEventHandler(OnTextChanged));
            Search("");  
        }

        private void HandlerForCMO(object sender, ContextMenuEventArgs e)
        {
            if (roomList.SelectedIndex == -1)
            {
                return;
            }

            FrameworkElement fe = e.Source as FrameworkElement;
            fe.ContextMenu = BuildMenu(roomList.SelectedIndex);
        }

        private ContextMenu BuildMenu(int index)
        {
            ContextMenu menu = new ContextMenu();
            Room room = items[roomList.SelectedIndex];

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
            if (roomList.SelectedIndex == -1)
            {
                return;
            }
            Room room = items[roomList.SelectedIndex];
            room.IsOld = true;
            room.MovedDateString = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
            roomService.Edit(room);
            searchBox.Visibility = Visibility.Hidden;
            Refresh();
        }

        private void EditReservations_Click(object sender, RoutedEventArgs e)
        {
            if (roomList.SelectedIndex == -1)
            {
                return;
            }

            Main main = (Main)Window.GetWindow(this);
            main.container.Dispatcher.Invoke(delegate
            {
                main.container.NavigationService.Navigate(new ReservationsForRoom(items[roomList.SelectedIndex]));
            });

        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            CreateRoomDialog createRoomDialog = new CreateRoomDialog();
            createRoomDialog.Owner = Application.Current.MainWindow;
            createRoomDialog.ShowDialog();

            if (createRoomDialog.Create)
            {
                MessageDialog messageDialog = new MessageDialog();
                messageDialog.Owner = Application.Current.MainWindow;

                if (createRoomDialog.UserInput.Text.Equals(""))
                {
                    messageDialog.setTitle("Error");
                    messageDialog.setMessage("Room number can't be empty!");
                    messageDialog.ShowDialog();
                    return;
                }

                if (roomService.FindRoom(createRoomDialog.UserInput.Text, false).Count > 0 || roomService.FindRoom(createRoomDialog.UserInput.Text, true).Count > 0)
                {
                    messageDialog.setTitle("Error");
                    messageDialog.setMessage(createRoomDialog.UserInput.Text + " already exists!");
                    messageDialog.ShowDialog();
                    return;
                }
                roomService.Create(new Room(createRoomDialog.UserInput.Text));
                searchBox.Visibility = Visibility.Hidden;
                Refresh();
            }
        }

        private void OnTextChanged(object Sender, TextChangedEventArgs e)
        {
            Search(searchBox.SearchTextBox.Text);
        }

        private void Search(string query)
        {
            BeforeSearch();
            
            // stop possible current running background worker
            if (worker.IsBusy)
            {
                // don't update ui if aborted since the next worker will do that anyway.
                worker.RunWorkerCompleted -= Worker_RunWorkerCompleted;

                worker.Abort();
                worker.Dispose();
            }

            worker = new AbortableBackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync(query);
        }

        public void Refresh()
        {
            Search(searchBox.SearchTextBox.Text);
        }

        private void BeforeSearch()
        {
            circualProgessBar.Visibility = Visibility.Visible;
            roomList.Visibility = Visibility.Hidden;
            noRoomsMessage.Visibility = Visibility.Hidden;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // simulate processing
            Thread.Sleep(1000);

            string query = (string) e.Argument;
            items = roomService.FindRoom(query, false);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            circualProgessBar.Visibility = Visibility.Hidden;
            roomList.ItemsSource = items;
            if(items.Count == 0)
            {
                noRoomsMessage.Visibility = Visibility.Visible;
            }
            else
            {
                roomList.Visibility = Visibility.Visible;
            }
            
            createButton.Visibility = Visibility.Visible;
            searchBox.Visibility = Visibility.Visible;
        }

    }
   
}
