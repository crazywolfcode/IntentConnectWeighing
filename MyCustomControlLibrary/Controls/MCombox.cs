using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyCustomControlLibrary
{
  public  class MCombox: ComboBox
    {
        static MCombox() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MCombox),new FrameworkPropertyMetadata(typeof(MCombox)));
        }


        public Brush SelectBackground
        {
            get { return (Brush)GetValue(SelectBackgroundProperty); }
            set { SetValue(SelectBackgroundProperty, value); }
        }
        
        public static readonly DependencyProperty SelectBackgroundProperty =
            DependencyProperty.Register("SelectBackground", typeof(Brush), typeof(MCombox), new PropertyMetadata(Brushes.LightGray));



        public Brush SelectForegound
        {
            get { return (Brush)GetValue(SelectForegoundProperty); }
            set { SetValue(SelectForegoundProperty, value); }
        }

        public static readonly DependencyProperty SelectForegoundProperty =
            DependencyProperty.Register("SelectForegound", typeof(Brush), typeof(MCombox), new PropertyMetadata(Brushes.Black));


        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(MCombox), new PropertyMetadata(Brushes.LightGray));



        public Brush MouseOverForegound
        {
            get { return (Brush)GetValue(MouseOverForegoundProperty); }
            set { SetValue(MouseOverForegoundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverForegoundProperty =
            DependencyProperty.Register("MouseOverForegound", typeof(Brush), typeof(MCombox), new PropertyMetadata(Brushes.Black));



        public Brush IconFillColor
        {
            get { return (Brush)GetValue(IconFillColorProperty); }
            set { SetValue(IconFillColorProperty, value); }
        }

        public static readonly DependencyProperty IconFillColorProperty =
            DependencyProperty.Register("IconFillColor", typeof(Brush), typeof(MCombox), new PropertyMetadata(new BrushConverter().ConvertFromString("#999")));
        
    }
}
