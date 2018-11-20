using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    class ScaleBrandModel
    {
        
        public List<ScaleBrand> getAll() {
            List<ScaleBrand> list = null;
            String sql = MyHelper.DbBaseHelper.getSelectSql(DataTabeName.scale_brand.ToString()) ;
            list = MyHelper.DbBaseHelper.DataTableToEntitys<ScaleBrand>(DatabaseOPtionHelper.GetInstance().select(sql));
            return list;
        }
    }
}
