using System;
using System.Globalization;
using System.Windows.Data;

namespace IntentConnectWeighing
{
    /// <summary>
    /// 百分比转换为角度值
    /// </summary>
    class PercentToAngleConverter
    {
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    var percent = 0;
        //    string str = String.Empty;
        //    if (value.ToString().Contains("%"))
        //    {
        //        str = value.ToString().Replace("%","");
        //    }
        //    int val ;
        //    Int32.TryParse(str,out val);
        //    percent = val;
        //    if (percent >= 1) return 360.0D;
        //    return percent * 360;
        //}
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var percent = double.Parse(value.ToString());
            if (percent >= 1) return 360.0D;
            return percent * 360;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
