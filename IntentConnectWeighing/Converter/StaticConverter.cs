using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace IntentConnectWeighing
{
    class StaticConverter
    {
        public static BooleanToVisibilityConverter BooleanToVisibilityConverter
        {
            get { return new BooleanToVisibilityConverter(); }
        }

        public static TrueToFalseConverter TrueToFalseConverter
        {
            get { return new TrueToFalseConverter(); }
        }

        public static ThicknessToDoubleConverter ThicknessToDoubleConverter
        {
            get { return new ThicknessToDoubleConverter(); }
        }
        public static BackgroundToForegroundConverter BackgroundToForegroundConverter
        {
            get { return new BackgroundToForegroundConverter(); }
        }
        public static TreeViewMarginConverter TreeViewMarginConverter
        {
            get { return new TreeViewMarginConverter(); }
        }

        public static PercentToAngleConverter PercentToAngleConverter
        {
            get { return new PercentToAngleConverter(); }
        }
    }
}
