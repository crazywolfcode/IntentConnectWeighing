using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MyHelper;
using System.Data;
namespace IntentConnectWeighing
{
    public class DatabaseOPtionHelper
    {
        private static SqlDao.DbHelper Instance;    
        private static String dbType = ConfigurationHelper.GetConfig(ConfigItemName.dbType.ToString());
        public static string connStrTemplate = " Data Source={0};Version=3;Pooling=False;Max Pool Size=100;";
        public static SqlDao.DbHelper GetInstance()
        {
            if (Instance == null)
            {
                if (dbType == DbType.mysql.ToString()) {                    
                    Instance = new SqlDao.MySqlHelper(ConfigurationHelper.GetConnectionConfig(ConfigItemName.mysqlConn.ToString()));
                } else if(dbType == DbType.sqlite.ToString()) {
                    Instance = new SqlDao.SQLiteHelper(ConfigurationHelper.GetConnectionConfig(ConfigItemName.sqliteConn.ToString()));
                }               
            }
            return Instance;
        }
    }
}
