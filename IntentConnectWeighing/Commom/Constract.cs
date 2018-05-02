using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntentConnectWeighing
{
    class Constract
    {
        public static readonly string defaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public static readonly string DateFormat = "yyyy-MM-dd";

        public static string BasePath = "/IntentConnectWeighing;component/";

        public static User currentUser =null;

        public static string valueSplit = "'";

        public static string templatePath=MyHelper.FileHelper.GetRunTimeRootPath()+"\\template\\";

        public static string tempPath = MyHelper.FileHelper.GetRunTimeRootPath() + "temp";
        public static string tempSupplyFileName = "SupplyCompanys.xml";
        public static string tempCustomerFileName = "CustomerCompanys.xml";
        public static string tempMatreialFileName = "MatreialCompanys.xml";
        public static string tempCarFileName = "CarCompanys.xml";

        public static String CaputureSuffix = ".jpg";
    }
}
