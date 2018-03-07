using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    class ProvinceModel
    {
        public static List<Province> GetProvince() {
            List<Province> provinces;
            string sql = MyHelper.DbBaseHelper.getSelectSql(DataTabeName.province.ToString());
            provinces = MyHelper.JsonHelper.DataTableToEntity<Province>(DatabaseOPtionHelper.GetInstance().select(sql));
            return provinces;
        }
    }
}
