using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IntentConnectWeighing
{
     class CompanyTypeConverter : IValueConverter
    {
        //0 未知 1私营 2 国营 3政府
        public Object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if ((int)value == (int)CompanyType.PrivaterEnterprise) { return "私营"; }
                else if ((int)value == (int)CompanyType.StateOwnedEnterprise) { return "国营"; }
                else if ((int)value == (int)CompanyType.GovernmentSector) { return "政府"; }
                else if ((int)value == (int)CompanyType.Other)
                {
                    return "其它";
                }
                return "未知";
            }
            catch
            {
                return "未知";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
