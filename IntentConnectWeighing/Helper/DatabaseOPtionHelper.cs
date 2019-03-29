using System;
using MyHelper;
using SqlDao;
namespace IntentConnectWeighing
{
    public class DatabaseOPtionHelper
    {
        
        private static DbHelper Instance;    
        private static String dbType = ConfigurationHelper.GetConfig(ConfigItemName.dbType.ToString());
        public static string connStrTemplate = " Data Source={0};Version=3;Pooling=False;Max Pool Size=100;";
        public static DbHelper GetInstance()
        {
            if (Instance == null)
            {
                if (dbType == DbType.mysql.ToString()) {                    
                    Instance = new MySqlHelper(ConfigurationHelper.GetConnectionConfig(ConfigItemName.mysqlConn.ToString()));
                } else if(dbType == DbType.sqlite.ToString()) {
                    Instance = new SQLiteHelper(ConfigurationHelper.GetConnectionConfig(ConfigItemName.sqliteConn.ToString()));
                }               
            }
            return Instance;
        }
    }
}
