using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    class ScaleModel
    {
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="scale"></param>
        /// <returns> true 存在 false 不存在</returns>
        public bool isExist(Scale scale) {
            string condition = ScaleEnum.brand + " = '" + scale.brand + "' and " + ScaleEnum.com + " = '" + scale.com + "' and " + ScaleEnum.series + " ='" + scale.series + "'";
            string sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.scale.ToString(),null,condition);
            List<Scale> list= DatabaseOPtionHelper.GetInstance().select<Scale>(sql);
            if (list.Count > 0)
            {
                return true;
            }          
            return false;
        }
    }
}
