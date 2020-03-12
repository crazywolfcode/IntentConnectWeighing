using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyHelper
{
    public sealed class DpiHelper
    {
        public static Double DpiX
        {
            get
            {
                double dpiX;
                using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
                {
                    dpiX = graphics.DpiX;
                }
                return dpiX ;
            }
        }
        public static Double DpiY
        {
            get
            {
                double dpiY;
                using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
                {
                    dpiY = graphics.DpiY;
                }
                return dpiY;
            }
        }
    }
}
