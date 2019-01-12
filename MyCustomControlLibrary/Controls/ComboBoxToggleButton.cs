using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MyCustomControlLibrary
{
   public class ComboBoxToggleButton: System.Windows.Controls.Primitives.ToggleButton
    {

        static ComboBoxToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxToggleButton), new FrameworkPropertyMetadata(typeof(ComboBoxToggleButton)));
        }
        
        public Brush FillColor
        {
            get { return (Brush)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

        public static readonly DependencyProperty FillColorProperty =
            DependencyProperty.Register("FillColor", typeof(Brush), typeof(ComboBoxToggleButton), new PropertyMetadata(new BrushConverter().ConvertFromString("#999")));


    }
}
