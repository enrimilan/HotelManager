using HotelManager.Entity;
using HotelManager.Service;
using System.ComponentModel;
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
    public partial class ReservationsForRoom : BaseFrame<Reservation>
    {

        private Room room;
        private ReservationService reservationService = ServiceFactory.GetReservationService();
        private RoomService roomService = ServiceFactory.GetRoomService();

        public ReservationsForRoom(Room room)
        {
            this.room = room;
            InitializeComponent();
            Title.Text = "Reservations for " + room.Number;
            
        }

        protected override void BaseFrame_Loaded(object sender, RoutedEventArgs e)
        {
            base.BaseFrame_Loaded(sender, e);
            emptyListMessage.Text = "No reservations.";
            GridView gridView = list.View as GridView;
            gridView.Columns.Add(CreateColumn("FromDateString", "From"));
            gridView.Columns.Add(CreateColumn("ToDateString", "To"));
            gridView.Columns.Add(CreateColumn("Status", "Status"));
            gridView.Columns.Add(CreateColumn("Person", "Person"));
            gridView.Columns.Add(CreateColumn("Contact", "Contact"));
            gridView.Columns.Add(CreateColumn("CreationDateString", "Created"));
            ReloadData("");
        }

        protected override void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            base.Worker_DoWork(sender, e);
            items = reservationService.FindReservation(room);
        }

        protected override void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            base.Worker_RunWorkerCompleted(sender, e);
            addReservationButton.Visibility = Visibility.Visible;
        }

        protected override ContextMenu BuildMenu(int index)
        {
            Reservation reservation = items[list.SelectedIndex];
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
            if (list.SelectedIndex == -1)
            {
                return;
            }
            Reservation reservation = items[list.SelectedIndex];
            reservation.CheckedIn = false;
            reservation.Past = true;
            reservation.EndDateString = DateTime.Now.ToString(Constants.DateFormat);
            reservation.Status = "Completed";
            reservationService.Edit(reservation);
            room.Status = "Free";
            room.Reservations = room.Reservations - 1;
            roomService.Edit(room);
            ReloadData("");
        }

        private void Checkin_Click(object sender, RoutedEventArgs e)
        {
            if (list.SelectedIndex == -1)
            {
                return;
            }
            Reservation reservation = items[list.SelectedIndex];
            reservation.CheckedIn = true;
            reservation.Status = "Checked in";
            reservationService.Edit(reservation);
            ReloadData("");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (list.SelectedIndex == -1)
            {
                return;
            }

            Reservation reservation = items[list.SelectedIndex];
            reservation.Canceled = true;
            reservation.EndDateString = DateTime.Now.ToString(Constants.DateFormat);
            reservationService.Edit(reservation);
            room.Status = "Free";
            room.Reservations = room.Reservations - 1;
            roomService.Edit(room);
            ReloadData("");
        }

        private void AddReservationButton_Click(object sender, RoutedEventArgs e)
        {   
            AddReservationDialog dialog = new AddReservationDialog(items);
            dialog.Dialog_Title = "Add a new reservation";
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();

            if(dialog.Create && dialog.CheckForErrorsAndProceed(room))
            {
                addReservationButton.Visibility = Visibility.Hidden;
                ReloadData("");
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
