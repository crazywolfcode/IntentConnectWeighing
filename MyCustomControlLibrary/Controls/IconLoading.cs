using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace MyCustomControlLibrary
{
    public class IconLoading:UserControl
    {
        
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(IconLoading), new PropertyMetadata(""));



        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(IconLoading), new PropertyMetadata(20.0));



        static IconLoading() {     
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconLoading), new FrameworkPropertyMetadata(typeof(IconLoading)));
        }
    }
}
