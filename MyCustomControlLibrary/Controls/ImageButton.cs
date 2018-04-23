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
   public class ImageButton : Button
    {
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }
        public static readonly DependencyProperty MoverBrushProperty = DependencyProperty.Register("MoverBrush", typeof(Brush), typeof(ImageButton), new PropertyMetadata(null));
        public static readonly DependencyProperty EnterBrushProperty = DependencyProperty.Register("EnterBrush", typeof(Brush), typeof(ImageButton), new PropertyMetadata(null));

        public Brush MyMoverBrush
        {
            get
            {
                return base.GetValue(ImageButton.MoverBrushProperty) as Brush;
            }
            set
            {
                base.SetValue(ImageButton.MoverBrushProperty, value);
            }
        }

        public Brush MyEnterBrush
        {
            get
            {
                return base.GetValue(ImageButton.EnterBrushProperty) as Brush;
            }
            set
            {
                base.SetValue(ImageButton.EnterBrushProperty, value);
            }
        }


        public Thickness CornerRadius
        {
            get { return (Thickness)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(Thickness), typeof(ImageButton), new PropertyMetadata(new Thickness(0)));


    }
}
