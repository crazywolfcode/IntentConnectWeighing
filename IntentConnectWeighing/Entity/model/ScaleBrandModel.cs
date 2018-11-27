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
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.scale_brand.ToString()) ;
            list = DatabaseOPtionHelper.GetInstance().select<ScaleBrand>(sql);
            return list;
        }
    }
}
