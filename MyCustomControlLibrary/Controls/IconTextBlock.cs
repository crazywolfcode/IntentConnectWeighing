using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyCustomControlLibrary
{
    public class IconTextBlock : TextBlock
    {

        static IconTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconTextBlock), new FrameworkPropertyMetadata(typeof(IconTextBlock)));
        }
    }
}
