using System.Windows;
using System.Windows.Controls;

namespace HotelManager.Gui.Dialog
{
    public partial class BaseDialogControl : Control
    {
        static BaseDialogControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseDialogControl), new FrameworkPropertyMetadata(typeof(BaseDialogControl)));
        }

        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object),typeof(BaseDialogControl), new UIPropertyMetadata());

    }
}
