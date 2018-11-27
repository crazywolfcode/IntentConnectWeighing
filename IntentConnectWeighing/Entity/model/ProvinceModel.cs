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
            string sql =DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.province.ToString());             
            return DatabaseOPtionHelper.GetInstance().select<Province>(sql);
        }
    }
}
