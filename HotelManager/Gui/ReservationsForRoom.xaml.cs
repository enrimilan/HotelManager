using HotelManager.Async;
using HotelManager.Entity;
using HotelManager.Service;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System;
using HotelManager.Gui.Dialog;
using HotelManager.Util;

namespace HotelManager.Gui
{
    /// <summary>
    /// Interaction logic for ReservationsForRoom.xaml
    /// </summary>
    public partial class ReservationsForRoom : UserControl
    {

        private Room room;
        private List<Reservation> items = new List<Reservation>();
        private ReservationService reservationService = ServiceFactory.GetReservationService();
        private RoomService roomService = ServiceFactory.GetRoomService();
        private AbortableBackgroundWorker worker = new AbortableBackgroundWorker();

        public ReservationsForRoom(Room room)
        {
            this.room = room;
            InitializeComponent();
            Title.Text = "Reservations for " + room.Number;
            ReloadReservations();
        }

        private void HandlerForCMO(object sender, ContextMenuEventArgs e)
        {
            if (reservationList.SelectedIndex == -1)
            {
                return;
            }

            FrameworkElement fe = e.Source as FrameworkElement;
            fe.ContextMenu = BuildMenu(reservationList.SelectedIndex);
        }

        private ContextMenu BuildMenu(int index)
        {
            Reservation reservation = items[reservationList.SelectedIndex];
            ContextMenu menu = new ContextMenu();
            if(index == 0 && !reservation.CheckedIn)
            {
                MenuItem checkin = new MenuItem();
                checkin.Header = "Checkin";
                checkin.Click += Checkin_Click;
                menu.Items.Add(checkin);
            }
            else if(index == 0 && reservation.CheckedIn)
            {
                MenuItem checkout = new MenuItem();
                checkout.Header = "Checkout";
                checkout.Click += Checkout_Click;
                menu.Items.Add(checkout);
            }
            if(!reservation.CheckedIn)
            {
                MenuItem cancel = new MenuItem();
                cancel.Header = "Cancel reservation";
                cancel.Click += Cancel_Click;
                menu.Items.Add(cancel);
            }

            return menu;
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            if (reservationList.SelectedIndex == -1)
            {
                return;
            }
            Reservation reservation = items[reservationList.SelectedIndex];
            reservation.CheckedIn = false;
            reservation.Past = true;
            reservation.EndDateString = DateTime.Now.ToString(Constants.DateFormat);
            reservation.Status = "Completed";
            reservationService.Edit(reservation);
            room.Status = "Free";
            room.Reservations = room.Reservations - 1;
            roomService.Edit(room);
            ReloadReservations();
        }

        private void Checkin_Click(object sender, RoutedEventArgs e)
        {
            if (reservationList.SelectedIndex == -1)
            {
                return;
            }
            Reservation reservation = items[reservationList.SelectedIndex];
            reservation.CheckedIn = true;
            reservation.Status = "Checked in";
            reservationService.Edit(reservation);
            ReloadReservations();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (reservationList.SelectedIndex == -1)
            {
                return;
            }

            Reservation reservation = items[reservationList.SelectedIndex];
            reservation.Canceled = true;
            reservation.EndDateString = DateTime.Now.ToString(Constants.DateFormat);
            reservationService.Edit(reservation);
            room.Status = "Free";
            room.Reservations = room.Reservations - 1;
            roomService.Edit(room);
            ReloadReservations();
        }

        private void ReloadReservations()
        {
            circualProgessBar.Visibility = Visibility.Visible;
            reservationList.Visibility = Visibility.Hidden;
            noReservationsMessage.Visibility = Visibility.Hidden;

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
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // simulate processing
            Thread.Sleep(1000);

            items = reservationService.FindReservation(room);
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

            addReservationButton.Visibility = Visibility.Visible;
        }

        private void AddReservationButton_Click(object sender, RoutedEventArgs e)
        {   
            AddReservationDialog dialog = new AddReservationDialog(items);
            dialog.Dialog_Title = "Add a new reservation";
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();

            if(dialog.Create)
            {

                MessageDialog messageDialog = new MessageDialog();
                messageDialog.Owner = Application.Current.MainWindow;

                if (dialog.FromDatePicker.SelectedDate == null || dialog.ToDatePicker.SelectedDate == null)
                {
                    messageDialog.Dialog_Title = "Error";
                    messageDialog.Message.Text = "Dates need to be selected!";
                    messageDialog.ShowDialog();
                    return;
                }
                if (dialog.Person.Text.Equals(""))
                {
                    messageDialog.Dialog_Title = "Error";
                    messageDialog.Message.Text = "Person name can't be empty!";
                    messageDialog.ShowDialog();
                    return;
                }
                if (dialog.Contact.Text.Equals(""))
                {
                    messageDialog.Dialog_Title = "Error";
                    messageDialog.Message.Text = "Person's contact can't be empty!";
                    messageDialog.ShowDialog();
                    return;
                }

                Reservation reservation = new Reservation();
                reservation.Room = roomService.GetRoom(room.Id);
                reservation.FromDateString = dialog.FromDatePicker.SelectedDate.Value.ToString(Constants.DateFormat);
                reservation.ToDateString = dialog.ToDatePicker.SelectedDate.Value.ToString(Constants.DateFormat);
                reservation.CreationDateString = DateTime.Now.ToString(Constants.DateFormat);
                reservation.Person = dialog.Person.Text;
                reservation.Contact = dialog.Contact.Text;
                reservationService.Create(reservation);
                room.Reservations = room.Reservations + 1;
                roomService.Edit(room);
                addReservationButton.Visibility = Visibility.Hidden;
                ReloadReservations();
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Main main = (Main)Window.GetWindow(this);
            main.container.Dispatcher.Invoke(delegate
            {
                main.container.NavigationService.GoBack();
            });
        }
    }
}
