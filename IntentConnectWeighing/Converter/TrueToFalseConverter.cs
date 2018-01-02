using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace IntentConnectWeighing
{
 public sealed  class TrueToFalseConverter : IValueConverter
    {
      
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var v = (bool)value;
            return !v;
        }
 
       public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException("没有实现这个方法");
        }
    }
}
