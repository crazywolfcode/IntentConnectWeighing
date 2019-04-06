using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyHelper
{
   public class CookieHelper
    {

        public static readonly Uri uri = new Uri("C:\\");
        public static void SetCookie(Uri uri, String name, String value, DateTime expirationDate)
        {
            String cookie = String.Format("{0}={1}; expires={2}", name, value, GetExpirationDateString(expirationDate));
            Application.SetCookie(uri, cookie);
        }

        public static void SetCookie(String value)
        {
            Application.SetCookie(uri, value);
        }
        public static void SetCookie(Uri uri, String value)
        {
            Application.SetCookie(uri, value);
        }

        public static String GetCookie(Uri uri)
        {
            return Application.GetCookie(uri);
        }

        public static String GetCookie()
        {
            return Application.GetCookie(uri);
        }

        private static String GetExpirationDateString(DateTime expirationDate)
        {
            return expirationDate.ToString("ddd, dd-MMM-yyyy HH:mm:ss") + " GMT";
        }

    }
}
