using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHelper;
namespace IntentConnectWeighing
{
   public class CommonModel
    {
        public static int FieldTryAsc(string tableName,String fieldName,int number,string condition)
        {
            int res = 0;
            try
            {
                string set = fieldName + "=" + fieldName + " + " + number;
                string sql = DbBaseHelper.getUpdateSql(tableName, set, condition);
                res =  DatabaseOPtionHelper.GetInstance().update(sql);               
                return res;
            }
            catch 
            {
                return res;
            }
        }

        public static int FieldTryDesc(string tableName, String fieldName, int number, string condition)
        {
            int res = 0;
            try
            {
                string set = fieldName + "=" + fieldName + " - " + number;
                string sql = DbBaseHelper.getUpdateSql(tableName, set, condition);
                res = DatabaseOPtionHelper.GetInstance().update(sql);          
                return res;
            }
            catch
            {           
                return res;
            }
        }

        /// <summary>
        /// 获取当天当前的出、入库过磅数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetTodayCount(WeightingBillType type) {
            int count = 0;
            String condition = WeighingBillEnum.type.ToString()+"="+((int)type);
            if (type == WeightingBillType.RK)
            {
                condition +=" and "+ WeighingBillEnum.receive_in_time + " like '%" + DateTimeHelper.getCurrentDateTime(DateTimeHelper.DateFormat) + "%'";
            }
            else {
                condition += " and " + WeighingBillEnum.send_in_time + " like '%" + DateTimeHelper.getCurrentDateTime(DateTimeHelper.DateFormat) + "%'";
            }
            string sql = DbBaseHelper.getSelectSql(DataTabeName.weighing_bill.ToString(),WeighingBillEnum.id.ToString(),condition);
            System.Data.DataTable dt = DatabaseOPtionHelper.GetInstance().select(sql);
           if(dt.Rows.Count > 0)
            {
                count = dt.Rows.Count;
            }
            return count;
        }
    }
}
