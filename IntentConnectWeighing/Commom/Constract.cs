using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntentConnectWeighing
{
    class Constract
    {
      
        public static string BasePath = "/IntentConnectWeighing;component/";

        public static User currentUser =null;

        public static string valueSplit = "'";

        public static string templatePath=MyHelper.FileHelper.GetRunTimeRootPath()+"\\template\\";
    }
}
