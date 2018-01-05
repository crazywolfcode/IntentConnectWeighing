using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntentConnectWeighing
{
    class ResourceHelper
    {       
            /// <summary>
            /// get string from dictionary resource
            /// </summary>
            /// <param name="resourceName"></param>
            /// <returns></returns>
            public static string getStringFromDictionaryResource(ResourceName resourceName)
            {
                return App.Current.Resources[resourceName.ToString()] as string;
            }


            public static void setStringToDictionaryResource(ResourceName resourceName, string value)
            {
                App.Current.Resources[resourceName.ToString()] = value;
            }

            public static int getIntFromDictionaryResource(ResourceName resourceName)
            {
                Object obj = App.Current.Resources[resourceName.ToString()];
                if (obj == null)
                    return 0;
                else
                    return (int)obj;
            }

            public static void setIntToDictionaryResource(ResourceName resourceName, int value)
            {
                App.Current.Resources[resourceName.ToString()] = value;
            }
     
    }
}
