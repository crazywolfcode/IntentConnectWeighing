using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace MyCustomControlLibrary
{
    public class HorizontalPoingLoading : UserControl
    {
        public double EllipseDiameter
        {
            get { return (double)GetValue(EllipseDiameterProperty); }
            set { SetValue(EllipseDiameterProperty, value); }
        }
        public static readonly DependencyProperty EllipseDiameterProperty =
            DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(HorizontalPoingLoading), new PropertyMetadata(5.0, OnEllipseDiameterChanged));

        private static void OnEllipseDiameterChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is HorizontalPoingLoading loading)
            {
                loading.EllipseDiameter = (double)e.NewValue;
            }
        }
        
        public System.Windows.Media.Brush FillBrush
        {
            get { return (System.Windows.Media.Brush)GetValue(FillBrushProperty); }
            set { SetValue(FillBrushProperty, value); }
        }

        public static readonly DependencyProperty FillBrushProperty =
            DependencyProperty.Register("FillBrush", typeof(System.Windows.Media.Brush), typeof(HorizontalPoingLoading), new PropertyMetadata(Brushes.Blue));

        static HorizontalPoingLoading() {

            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalPoingLoading),new FrameworkPropertyMetadata(typeof(HorizontalPoingLoading)));

        }

    }
}
