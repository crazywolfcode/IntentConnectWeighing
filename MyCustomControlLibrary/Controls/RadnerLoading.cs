using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace MyCustomControlLibrary
{
   public class RadnerLoading : UserControl
    {
        static RadnerLoading() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadnerLoading), new FrameworkPropertyMetadata(typeof(RadnerLoading)));
        }
    }
}
