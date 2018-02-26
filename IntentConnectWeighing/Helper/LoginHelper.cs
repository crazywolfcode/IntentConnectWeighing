using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyHelper;

namespace IntentConnectWeighing
{
    public class LoginHelper
    {
        public static List<User> getHostoryUser()
        {
            List<User> list = (List<User>)XmlHelper.Deserialize(typeof(List<User>), FileHelper.Reader(FileHelper.GetRunTimeRootPath() + "/users.xml"));
            if (list == null || list.Count <= 0) {
                return new List<User>();
            }
            return list;
        }
    }
}
