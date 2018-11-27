using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
   public class CompanyModel
    {
        public static List<Company> IndistinctSearchByNameOrNameFirstCase(String queryStr) {
            if (String.IsNullOrEmpty(queryStr))
            {
                return null;
            }
            else {
                string condition = CompanyEnum.name.ToString() + " like '%" + queryStr+ "%' " + " OR " + CompanyEnum.name_first_case.ToString() + " like '%" + queryStr.ToUpper() + "%'";
                String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.company.ToString(), null, condition);
                List<Company> list =DatabaseOPtionHelper.GetInstance().select<Company>(sql);
                return list;
            }
        }
    }
}
