using HotelManager.Async;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HotelManager.Gui
{
    public class BaseFrame<T> : UserControl
    {

        protected BaseFrameControl control;
        protected AbortableBackgroundWorker worker = new AbortableBackgroundWorker();
        protected List<T> items = new List<T>();
        protected ListView list;
        protected ContentControl circularProgessBar;
        protected TextBlock emptyListMessage;

        protected virtual void BaseFrame_Loaded(object sender, RoutedEventArgs e)
        {
            control = Content as BaseFrameControl;
            list = control.Template.FindName("list", control) as ListView;
            circularProgessBar = control.Template.FindName("circularProgessBar", control) as ContentControl;
            emptyListMessage = control.Template.FindName("emptyListMessage", control) as TextBlock;
            list.ContextMenuOpening += HandlerForCMO;
        }

        protected virtual ContextMenu BuildMenu(int index)
        {
            return null;
        }
        
        protected virtual void Worker_OnPreExecute()
        {
            circularProgessBar.Visibility = Visibility.Visible;
            list.Visibility = Visibility.Hidden;
            emptyListMessage.Visibility = Visibility.Hidden;
        }

        protected virtual void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // simulate processing
            Thread.Sleep(1000);
        }

        protected virtual void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            circularProgessBar.Visibility = Visibility.Hidden;
            list.ItemsSource = items;
            if (items.Count == 0)
            {
                emptyListMessage.Visibility = Visibility.Visible;
            }
            else
            {
                list.Visibility = Visibility.Visible;
            }
        }

        protected void HandlerForCMO(object sender, ContextMenuEventArgs e)
        {
            if (list.SelectedIndex == -1)
            {
                return;
            }
            FrameworkElement fe = e.Source as FrameworkElement;
            fe.ContextMenu = BuildMenu(list.SelectedIndex);
        }

        public void ReloadData(string query)
        {
            // stop possible current running background worker
            if (worker.IsBusy)
            {
                // don't update ui if aborted since the next worker will do that anyway.
                worker.RunWorkerCompleted -= Worker_RunWorkerCompleted;

                worker.Abort();
                worker.Dispose();
            }

            Worker_OnPreExecute();
            worker = new AbortableBackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync(query);
        }

        protected GridViewColumn CreateColumn(string binding, string header)
        {
            GridViewColumn gvc = new GridViewColumn();
            gvc.DisplayMemberBinding = new Binding(binding);
            gvc.Header = header;
            gvc.Width = 150;
            return gvc;
        }

    }
}
