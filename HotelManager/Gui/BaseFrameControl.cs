using System.Windows;
using System.Windows.Controls;

namespace HotelManager.Gui
{
    public class BaseFrameControl : Control
    {
        static BaseFrameControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseFrameControl), new FrameworkPropertyMetadata(typeof(BaseFrameControl)));
        }

        public object Left
        {
            get { return (object)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }

        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(object), typeof(BaseFrameControl), new UIPropertyMetadata());

        public object Right
        {
            get { return (object)GetValue(RightProperty); }
            set { SetValue(RightProperty, value); }
        }

        public static readonly DependencyProperty RightProperty = DependencyProperty.Register("Right", typeof(object), typeof(BaseFrameControl), new UIPropertyMetadata());

    }
}
