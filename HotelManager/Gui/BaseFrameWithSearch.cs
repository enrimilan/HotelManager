using HotelManager.Gui.Component;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HotelManager.Gui
{
    public class BaseFrameWithSearch<T> : BaseFrame<T>
    {

        protected SearchBox SearchBoxNew;

        protected override void BaseFrame_Loaded(object sender, RoutedEventArgs e)
        {
            base.BaseFrame_Loaded(sender, e);
            SearchBoxNew = FindName("searchBox") as SearchBox;
            SearchBoxNew.AddHandler(TextBox.TextChangedEvent, new TextChangedEventHandler(OnTextChanged));
        }

        protected override void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            base.Worker_RunWorkerCompleted(sender, e);
            SearchBoxNew.Visibility = Visibility.Visible;
        }

        private void OnTextChanged(object Sender, TextChangedEventArgs e)
        {
            ReloadData(SearchBoxNew.SearchTextBox.Text);
        }

    }
}
