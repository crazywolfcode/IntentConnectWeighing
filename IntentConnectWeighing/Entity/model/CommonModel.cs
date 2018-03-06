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
    }
}
