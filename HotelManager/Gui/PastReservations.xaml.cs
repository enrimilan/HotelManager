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
    /// Interaction logic for PastReservations.xaml
    /// </summary>
    public partial class PastReservations : UserControl
    {
        private List<Reservation> items = new List<Reservation>();
        private ReservationService reservationService = ServiceFactory.GetReservationService();
        private AbortableBackgroundWorker worker = new AbortableBackgroundWorker();

        public PastReservations()
        {
            InitializeComponent();
            searchBox.AddHandler(TextBox.TextChangedEvent, new TextChangedEventHandler(OnTextChanged));
            Search("");
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
            reservationList.Visibility = Visibility.Hidden;
            noReservationsMessage.Visibility = Visibility.Hidden;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // simulate processing
            Thread.Sleep(1000);

            string query = (string)e.Argument;
            items = reservationService.FindPastReservation(query);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            circualProgessBar.Visibility = Visibility.Hidden;
            reservationList.ItemsSource = items;
            if (items.Count == 0)
            {
                noReservationsMessage.Visibility = Visibility.Visible;
            }
            else
            {
                reservationList.Visibility = Visibility.Visible;
            }

            searchBox.Visibility = Visibility.Visible;
        }
    }
}
