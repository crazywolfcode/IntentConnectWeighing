﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHelper;

namespace IntentConnectWeighing
{
 public   class MaterialModel
    {
        public static List<Material> IndistictSearchByNameORFirstCase(String queryStr) {
            if (String.IsNullOrEmpty(queryStr)) {
                return null;
            }
            string condition = MaterialEnum.name.ToString() + " like '%" + queryStr + "%' " + " OR " + MaterialEnum.name_first_case.ToString() + " like '%" + queryStr.ToUpper() + "%'";
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.material.ToString(), null, condition);
            List<Material> list = DatabaseOPtionHelper.GetInstance().select<Material>(sql);
            return list;
        }
    }
}
