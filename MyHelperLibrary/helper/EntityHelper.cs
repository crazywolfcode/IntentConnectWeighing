using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper
{
    public class EntityHelper
    {
        public static TResult CopyProperties<TSource, TResult>(TSource source, TResult result){
            if (source != null) {
                PropertyInfo[] sourcePropertyInfos = source.GetType().GetProperties();
                for (int i = 0; i < sourcePropertyInfos.Length; i++)
                {
                    PropertyInfo property = sourcePropertyInfos[i];
                    PropertyInfo targetProperty = result.GetType().GetProperty(property.Name);
                    if (targetProperty != null) {
                        targetProperty.SetValue(result, property.GetValue(source, null));
                    }                    
                }
            }            
            return result;
        }

    }
}
