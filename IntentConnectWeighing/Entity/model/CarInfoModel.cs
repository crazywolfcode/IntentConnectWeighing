using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    public class CarInfoModel
    {
        public static List<CarInfo> FuzzySearch(String palteNumberPart)
        {
            string condition = CarInfoEnum.car_number.ToString() + " like '%" + palteNumberPart + "%' ";           
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.car_info.ToString(), null, condition);
            List<CarInfo> list = DatabaseOPtionHelper.GetInstance().select<CarInfo>(sql);
            return list;
        }
    }
}
