using System;
using System.Windows.Data;


namespace IntentConnectWeighing
{
    /// <summary>
    /// 性别属性的转换（0：男，1：女）
    /// </summary>
    class SexConverter :IValueConverter
    {
        public static readonly SexConverter sexConverter;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = "";
     
                string sex = (string)value;
            if (sex == "1")
            {
                result = "男";
            }
            else if (sex == "0")
            {
                result = "女";
            }
            else if (sex == "男") {
                result = "1";
            }
            else if(sex == "女")
            {
                result = "0";
            }
               
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        static SexConverter(){
            sexConverter =  new SexConverter();
            }
    }
}
