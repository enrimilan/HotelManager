using HotelManager.Async;
using HotelManager.Entity;
using HotelManager.Service;
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

        private void OnTextChanged(object Sender, TextChangedEventArgs e)
        {
            Search(searchBox.SearchTextBox.Text);
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
            items = roomService.FindRoom(query);
        }

        void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
