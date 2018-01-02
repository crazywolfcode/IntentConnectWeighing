using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntentConnectWeighing
{
    class Constract
    {
      
        public static string BasePath = "/IntentConnectWeighing;component/";

        public static loginedUser currentUser =null;

        public static string sqliteConnecationString = "Data Source={0};Version=3;";
        public static string mysqlConnecationString = "Data Source={0}\\mysql; Initial Catalog=test;User ID={1};Password={2};";
    }
}
