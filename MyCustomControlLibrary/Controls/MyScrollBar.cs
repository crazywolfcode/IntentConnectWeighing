using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
namespace MyCustomControlLibrary
{
  public  class MyScrollBar : ScrollBar
    {

        static MyScrollBar() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyScrollBar), new FrameworkPropertyMetadata(typeof(MyScrollBar)));
        }

        public Visibility LeftRepeatButtonVisibility
        {
            get { return (Visibility)GetValue(LeftRepeatButtonVisibilityProperty); }
            set { SetValue(LeftRepeatButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty LeftRepeatButtonVisibilityProperty =
            DependencyProperty.Register("LeftRepeatButtonVisibility", typeof(Visibility), typeof(MyScrollBar), new FrameworkPropertyMetadata(Visibility.Visible));

        public Visibility RightRepeatButtonVisibility
        {
            get { return (Visibility)GetValue(RightRepeatButtonVisibilityProperty); }
            set { SetValue(RightRepeatButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty RightRepeatButtonVisibilityProperty =
            DependencyProperty.Register("RightRepeatButtonVisibility", typeof(Visibility), typeof(MyScrollBar), new FrameworkPropertyMetadata(Visibility.Visible));

        public Visibility UpRepeatButtonVisibility
        {
            get { return (Visibility)GetValue(UpRepeatButtonVisibilityProperty); }
            set { SetValue(UpRepeatButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty UpRepeatButtonVisibilityProperty =
            DependencyProperty.Register("UpRepeatButtonVisibility", typeof(Visibility), typeof(MyScrollBar), new FrameworkPropertyMetadata(Visibility.Visible));
        
        public Visibility DownRepeatButtonVisibility
        {
            get { return (Visibility)GetValue(DownRepeatButtonVisibilityProperty); }
            set { SetValue(DownRepeatButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty DownRepeatButtonVisibilityProperty =
            DependencyProperty.Register("DownRepeatButtonVisibility", typeof(Visibility), typeof(MyScrollBar), new FrameworkPropertyMetadata(Visibility.Visible));
               
        public String LeftIcon
        {
            get { return (String)GetValue(LeftIconProperty); }
            set { SetValue(LeftIconProperty, value); }
        }
        public static readonly DependencyProperty LeftIconProperty =
            DependencyProperty.Register("LeftIcon", typeof(String), typeof(MyScrollBar), new FrameworkPropertyMetadata(String.Empty));

        public String RightIcon
        {
            get { return (String)GetValue(RightIconProperty); }
            set { SetValue(RightIconProperty, value); }
        }
        public static readonly DependencyProperty RightIconProperty =
            DependencyProperty.Register("RightIcon", typeof(String), typeof(MyScrollBar), new FrameworkPropertyMetadata(String.Empty));

        public String UpIcon
        {
            get { return (String)GetValue(UpIconProperty); }
            set { SetValue(UpIconProperty, value); }
        }
        public static readonly DependencyProperty UpIconProperty =
            DependencyProperty.Register("UpIcon", typeof(String), typeof(MyScrollBar), new FrameworkPropertyMetadata(String.Empty));

        public String BottomIcon
        {
            get { return (String)GetValue(BottomIconProperty); }
            set { SetValue(BottomIconProperty, value); }
        }
        public static readonly DependencyProperty BottomIconProperty =
            DependencyProperty.Register("BottomIcon", typeof(String), typeof(MyScrollBar), new FrameworkPropertyMetadata(String.Empty));

    }
}
