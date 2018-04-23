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
    public class SwitchButton : ToggleButton
    {

        static SwitchButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchButton), new FrameworkPropertyMetadata(typeof(SwitchButton)));
        }


        public String Icon
        {
            get { return (String)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(String), typeof(SwitchButton), new PropertyMetadata(String.Empty));



        public String CheckedIcon
        {
            get { return (String)GetValue(CehckedIconProperty); }
            set { SetValue(CehckedIconProperty, value); }
        }

        public static readonly DependencyProperty CehckedIconProperty =
            DependencyProperty.Register("CheckedIcon", typeof(String), typeof(SwitchButton), new PropertyMetadata(String.Empty));



        public Double IconSize
        {
            get { return (Double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(Double), typeof(SwitchButton), new FrameworkPropertyMetadata(14D));



        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }
            set { SetValue(IconMarginProperty, value); }
        }

        public static readonly DependencyProperty IconMarginProperty =
            DependencyProperty.Register("IconMargin", typeof(Thickness), typeof(SwitchButton), new PropertyMetadata(new Thickness(1)));



        public Brush CheckedForeground
        {
            get { return (Brush)GetValue(CheckedForegroundProperty); }
            set { SetValue(CheckedForegroundProperty, value); }
        }

        public static readonly DependencyProperty CheckedForegroundProperty =
            DependencyProperty.Register("CheckedForeground", typeof(Brush), typeof(SwitchButton), new PropertyMetadata(Brushes.RoyalBlue));



        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverForegroundProperty =
            DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(SwitchButton), new PropertyMetadata(Brushes.LightBlue));

    }
}
