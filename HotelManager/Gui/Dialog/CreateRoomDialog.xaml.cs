using HotelManager.Entity;
using HotelManager.Service;
using HotelManager.Util;
using System;
using System.Windows;

namespace HotelManager.Gui.Dialog
{
    /// <summary>
    /// Interaction logic for CreateRoomDialog.xaml
    /// </summary>
    public partial class CreateRoomDialog : BaseDialog
    {

        private RoomService roomService = ServiceFactory.GetRoomService();

        public CreateRoomDialog()
        {
            InitializeComponent();
        }

        public bool CheckForErrorsAndProceed()
        {
            MessageDialog messageDialog = new MessageDialog();
            messageDialog.Owner = Application.Current.MainWindow;
            if (UserInput.Text.Equals(""))
            {
                messageDialog.Dialog_Title = "Error";
                messageDialog.Message.Text = "Room number can't be empty!";
                messageDialog.ShowDialog();
                return false;
            }

            if (roomService.FindRoom(UserInput.Text, false).Count > 0 || roomService.FindRoom(UserInput.Text, true).Count > 0)
            {
                messageDialog.Dialog_Title = "Error";
                messageDialog.Message.Text = UserInput.Text + " already exists!";
                messageDialog.ShowDialog();
                return false;
            }
            Room room = new Room(UserInput.Text);
            room.CreationDateString = DateTime.Now.ToString(Constants.DateFormat);
            roomService.Create(room);
            return true;
        }
        
    }
}
