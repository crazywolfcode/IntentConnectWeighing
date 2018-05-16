using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCustomControlLibrary
{
    public class FiveColumnLoading : System.Windows.Controls.UserControl
    {
        static FiveColumnLoading() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FiveColumnLoading), new System.Windows.FrameworkPropertyMetadata(typeof(FiveColumnLoading)));
        }
    }
}
