using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;
using System.Windows.Controls;

namespace MyCustomControlLibrary
{
    public class MProgressBar : ProgressBar
    {

        public bool IsShowValue
        {
            get { return (bool)GetValue(IsShowValueProperty); }
            set { SetValue(IsShowValueProperty, value);          
            }
        }

        public static readonly DependencyProperty IsShowValueProperty = DependencyProperty.Register("IsShowValue", typeof(bool), typeof(MProgressBar), new PropertyMetadata(true));

        static MProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MProgressBar), new FrameworkPropertyMetadata(typeof(MProgressBar)));
        }
    }
}
