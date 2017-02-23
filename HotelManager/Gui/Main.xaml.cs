using HotelManager.Entity;
using HotelManager.Gui.Dialog;
using HotelManager.Service;
using HotelManager.Util;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HotelManager.Gui
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {

        private List<Label> items = new List<Label>();
        private RoomService roomService = ServiceFactory.GetRoomService();

        public Main()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            item1.MouseLeftButtonDown += new MouseButtonEventHandler(item1_MouseLeftButtonDown);
            item2.MouseLeftButtonDown += new MouseButtonEventHandler(item2_MouseLeftButtonDown);
            item3.MouseLeftButtonDown += new MouseButtonEventHandler(item3_MouseLeftButtonDown);
            item4.MouseLeftButtonDown += new MouseButtonEventHandler(item4_MouseLeftButtonDown);
            item5.MouseLeftButtonDown += new MouseButtonEventHandler(item5_MouseLeftButtonDown);

            items.Add(item1);
            items.Add(item2);
            items.Add(item3);
            items.Add(item4);
            items.Add(item5);
            LoadView(item1, "Rooms.xaml");
        }

        private void item1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadView(item1, "Rooms.xaml");
        }

        private void item2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadView(item2, "OldRooms.xaml");
        }

        private void item3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadView(item3, "Reservations.xaml");
        }

        private void item4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadView(item4, "CanceledReservations.xaml");
        }

        private void item5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadView(item5, "PastReservations.xaml");
        }

        private void LoadView(Label item, string fileName)
        {

            // update the labels
            foreach (Label text in items)
            {
                if(text == item)
                {
                    Style style = new Style(typeof(Label));
                    style.Setters.Add(new Setter(Label.ForegroundProperty, Brushes.White));
                    item.Style = style;
                }
                else
                {
                    text.Style = Resources["LeftSideMenuItems"] as Style;
                }

            }

            // load the new view
            container.Dispatcher.Invoke(delegate
            {
                container.Source = new Uri(fileName, UriKind.Relative);
            });
        }

        private void Main_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    Application.Current.MainWindow.DragMove();
                }
        }

        /// <summary>
        /// CloseButton_Clicked
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// MaximizedButton_Clicked
        /// </summary>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }

        /// <summary>
        /// Minimized Button_Clicked
        /// </summary>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Adjusts the WindowSize to correct parameters when Maximize button is clicked
        /// </summary>
        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                MaximizeButton.Content = "1";
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                MaximizeButton.Content = "2";
            }

        }

        private void FileNewRoom_Click(object sender, RoutedEventArgs e)
        {
            CreateRoomDialog createRoomDialog = new CreateRoomDialog();
            createRoomDialog.Dialog_Title = "Create a new room";
            createRoomDialog.Owner = Application.Current.MainWindow;
            createRoomDialog.ShowDialog();

            if(createRoomDialog.Create)
            {
                MessageDialog messageDialog = new MessageDialog();
                messageDialog.Owner = Application.Current.MainWindow;
                
                if (createRoomDialog.UserInput.Text.Equals(""))
                {
                    messageDialog.Dialog_Title = "Error";
                    messageDialog.Message.Text = "Room number can't be empty!";
                    messageDialog.ShowDialog();
                    return;
                }

                if(roomService.FindRoom(createRoomDialog.UserInput.Text, false).Count > 0 || roomService.FindRoom(createRoomDialog.UserInput.Text, true).Count > 0)
                {
                    messageDialog.Dialog_Title = "Error";
                    messageDialog.Message.Text = createRoomDialog.UserInput.Text + " already exists!";
                    messageDialog.ShowDialog();
                    return;
                }
                Room room = new Room(createRoomDialog.UserInput.Text);
                room.CreationDateString = DateTime.Now.ToString(Constants.DateFormat);
                roomService.Create(room);

                UserControl userControl = (UserControl)container.Content;
                if (userControl is Rooms)
                {
                    Rooms rooms = userControl as Rooms;
                    rooms.searchBox.Visibility = Visibility.Hidden;
                    rooms.Refresh();
                }
                
                messageDialog.Message.Text = "Created " + createRoomDialog.UserInput.Text;
                messageDialog.ShowDialog();
            }

        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            // TODO need to close db n stuff here
            Application.Current.Shutdown();
        }

        private void HelpAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog = new MessageDialog();
            messageDialog.Owner = Application.Current.MainWindow;
            messageDialog.Dialog_Title = "About Hotel Manager";
            messageDialog.Message.Text = "Hotel Manager \n   Version 1.0 \n        2017";
            messageDialog.ShowDialog();
        }
    }
}
