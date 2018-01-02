using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyHelper;

namespace IntentConnectWeighing
{
    public class LoginHelper
    {
        public static List<loginedUser> getHostoryUser()
        {
            List<loginedUser> list = (List<loginedUser>)XmlHelper.Deserialize(typeof(List<loginedUser>), FileHelper.Reader(FileHelper.GetRunTimeRootPath() + "/users.xml"));
            if (list == null || list.Count <= 0) {
                return new List<loginedUser>();
            }
            return list;
        }
    }
}
