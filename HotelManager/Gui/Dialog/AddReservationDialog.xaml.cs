using HotelManager.Entity;
using System.Collections.Generic;
using System;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows;
using HotelManager.Util;
using HotelManager.Service;

namespace HotelManager.Gui.Dialog
{
    /// <summary>
    /// Interaction logic for AddReservationDialog.xaml
    /// </summary>
    public partial class AddReservationDialog : BaseDialog
    {

        private List<CalendarDateRange> FromBlackoutDates = new List<CalendarDateRange>();
        private List<CalendarDateRange> ToBlackoutDates = new List<CalendarDateRange>();
        private List<Reservation> reservations = new List<Reservation>();
        private ReservationService reservationService = ServiceFactory.GetReservationService();
        private RoomService roomService = ServiceFactory.GetRoomService();

        public AddReservationDialog(List<Reservation> reservations)
        {
            InitializeComponent();
            this.reservations = reservations;
            if( reservations.Count > 0 && reservations[0].CheckedIn)
            {
                FromDatePicker.DisplayDateStart = reservations[0].To;
            }

            foreach (Reservation reservation in reservations)
            {
                AddToFromBlackoutDates(new CalendarDateRange(reservation.From, reservation.To.AddDays(-1)));
                AddToToBlackoutDates(new CalendarDateRange(reservation.From.AddDays(1), reservation.To));
            }
        }

        private void Reset()
        {
            DateTime selectedFromDate = FromDatePicker.SelectedDate.Value;
            ToDatePicker.BlackoutDates.Clear();
            ToDatePicker.DisplayDateStart = selectedFromDate.AddDays(1);
            ToDatePicker.SelectedDate = null;
            ToDatePicker.DisplayDateEnd = DateTime.MaxValue;

            // get end date
            foreach (Reservation reservation in reservations)
            {
                if(reservation.From.CompareTo(selectedFromDate) > 0)
                {
                    // this reservation is later than the selected date, so stop here, we have the end date.
                    ToDatePicker.DisplayDateEnd = reservation.From;
                    return;
                }
            }
        }

        private void AddToFromBlackoutDates(CalendarDateRange range)
        {
            FromBlackoutDates.Add(range);
            FromDatePicker.BlackoutDates.Add(range);
        }

        private void AddToToBlackoutDates(CalendarDateRange range)
        {
            ToBlackoutDates.Add(range);
            ToDatePicker.BlackoutDates.Add(range);
        }

        private void FromDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ToDatePicker.IsEnabled = true;
            Reset();
        }

        public bool CheckForErrorsAndProceed(Room room)
        {
            MessageDialog messageDialog = new MessageDialog();
            messageDialog.Owner = Application.Current.MainWindow;

            if (FromDatePicker.SelectedDate == null || ToDatePicker.SelectedDate == null)
            {
                messageDialog.Dialog_Title = "Error";
                messageDialog.Message.Text = "Dates need to be selected!";
                messageDialog.ShowDialog();
                return false;
            }
            if (Person.Text.Equals(""))
            {
                messageDialog.Dialog_Title = "Error";
                messageDialog.Message.Text = "Person name can't be empty!";
                messageDialog.ShowDialog();
                return false;
            }
            if (Contact.Text.Equals(""))
            {
                messageDialog.Dialog_Title = "Error";
                messageDialog.Message.Text = "Person's contact can't be empty!";
                messageDialog.ShowDialog();
                return false ;
            }

            Reservation reservation = new Reservation();
            reservation.FromDateString = FromDatePicker.SelectedDate.Value.ToString(Constants.DateFormat);
            reservation.ToDateString = ToDatePicker.SelectedDate.Value.ToString(Constants.DateFormat);
            reservation.CreationDateString = DateTime.Now.ToString(Constants.DateFormat);
            reservation.Person = Person.Text;
            reservation.Contact = Contact.Text;
            reservation.Room = room;
            reservationService.Create(reservation);
            room.Reservations = room.Reservations + 1;
            roomService.Edit(room);

            return true;
        }

    }
}
