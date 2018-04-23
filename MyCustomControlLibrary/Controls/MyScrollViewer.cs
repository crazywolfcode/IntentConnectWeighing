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
    public class MyScrollViewer : ScrollViewer
    {
        static MyScrollViewer() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyScrollViewer), new FrameworkPropertyMetadata(typeof(MyScrollViewer)));
        }
        /// <summary>
        /// the contents of scroll bar floating on it . default true
        /// 滚动条是否浮动的内容上面
        /// </summary>
        public bool IsScrollBarFloatUp
        {
            get { return (bool)GetValue(IsScrollBarFloatUpProperty); }
            set { SetValue(IsScrollBarFloatUpProperty, value); }
        }
        public static readonly DependencyProperty IsScrollBarFloatUpProperty =
            DependencyProperty.Register("IsScrollBarFloatUp", typeof(bool), typeof(MyScrollViewer), new FrameworkPropertyMetadata(true));



        public bool IsShowRepeatbutton
        {
            get { return (bool)GetValue(IsShowRepeatbuttonProperty); }
            set { SetValue(IsShowRepeatbuttonProperty, value); }
        }

        public static readonly DependencyProperty IsShowRepeatbuttonProperty =
            DependencyProperty.Register("IsShowRepeatbutton", typeof(bool), typeof(MyScrollViewer), new FrameworkPropertyMetadata(true));


    }
}
