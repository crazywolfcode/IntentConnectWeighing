using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper
{
    public class ListHepler
    {
        public static List<T> Except<T>(List<T> sourceList, List<T> exceptList) {         
         
            for (int i = 0; i < exceptList.Count; i++)
            {
                if (sourceList.Contains(exceptList[i])) {
                    sourceList.Remove(exceptList[i]);
                }             
            }            
            return sourceList;
        }

    }
}
