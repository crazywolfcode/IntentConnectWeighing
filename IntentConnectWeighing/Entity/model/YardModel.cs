using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    class YardModel
    {
        public static Yard GetById(String id) {
            String conditon = YardEnum.id.ToString() + "=" + Constract.valueSplit + id + Constract.valueSplit;
            String sql =DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.yard.ToString(),null,conditon,null,null,null,1);
            List<Yard> list =DatabaseOPtionHelper.GetInstance().select<Yard>(sql);
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            else {
                return list[0];
            }
        }

        public static List<Yard> GetListByCompanyId(String companyId) {
            String condition = YardEnum.affiliated_company_id.ToString() + "=" + Constract.valueSplit + companyId + Constract.valueSplit;
            String sql =DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.yard.ToString(), null, condition);
            List<Yard> list =DatabaseOPtionHelper.GetInstance().select<Yard>(sql);
            return list;
        }
    }
}
