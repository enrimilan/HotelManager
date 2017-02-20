using HotelManager.Async;
using HotelManager.Entity;
using HotelManager.Service;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System;

namespace HotelManager.Gui
{
    /// <summary>
    /// Interaction logic for OldRooms.xaml
    /// </summary>
    public partial class OldRooms : UserControl
    {
        private List<Room> items = new List<Room>();
        private RoomService roomService = ServiceFactory.GetRoomService();
        private AbortableBackgroundWorker worker = new AbortableBackgroundWorker();

        public OldRooms()
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
            MenuItem MoveBackToActualRooms = new MenuItem();
            MoveBackToActualRooms.Header = "Move back to actual rooms";
            MoveBackToActualRooms.Click += MoveBackToActualRooms_Click;
            menu.Items.Add(MoveBackToActualRooms);
            return menu;
        }

        private void MoveBackToActualRooms_Click(object sender, RoutedEventArgs e)
        {
            if (roomList.SelectedIndex == -1)
            {
                return;
            }
            Room room = items[roomList.SelectedIndex];
            room.IsOld = false;
            roomService.Edit(room);
            searchBox.Visibility = Visibility.Hidden;
            Refresh();
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

            string query = (string)e.Argument;
            items = roomService.FindRoom(query, true);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            circualProgessBar.Visibility = Visibility.Hidden;
            roomList.ItemsSource = items;
            if (items.Count == 0)
            {
                noRoomsMessage.Visibility = Visibility.Visible;
            }
            else
            {
                roomList.Visibility = Visibility.Visible;
            }

            searchBox.Visibility = Visibility.Visible;
        }
    }
}
