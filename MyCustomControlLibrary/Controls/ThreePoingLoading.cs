using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace MyCustomControlLibrary
{
    public class ThreePoingLoading : UserControl
    {
        static ThreePoingLoading() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThreePoingLoading), new FrameworkPropertyMetadata(typeof(ThreePoingLoading)));
        }
    }
}
