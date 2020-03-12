using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper
{
    public static class EntityHelper
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
        /// <summary>
        /// 从源对像中复制相同属性的值，不会覆盖目标对像属性上已经有的值。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns>target</returns>
        public static TResult CopyValueFrom<TResult, TSource>(this TResult target, TSource source) where TResult : class
        {

            if (source != null)
            {
                if (target == null)
                {
                    target = (TResult)Activator.CreateInstance(target.GetType());
                }
                var sourcePropertyInfos = source.GetType().GetProperties().Where(pi => pi.CanRead);
                foreach (var propertyInfo in sourcePropertyInfos)
                {
                    var targetValue = propertyInfo.GetValue(target, null);
                    var defaultValue = propertyInfo.PropertyType.GetDefault();
                    
                    if (targetValue != null && !targetValue.Equals(defaultValue)) continue;

                    var sourceValue = propertyInfo.GetValue(source, null);
                    propertyInfo.SetValue(target, sourceValue, null);
                }
            }
            return target;

        }
        public static object GetDefault(this Type type)
        {
            // If no Type was supplied, if the Type was a reference type, or if the Type was a System.Void, return null
            if (type == null || !type.IsValueType || type == typeof(void))
                return null;

            // If the supplied Type has generic parameters, its default value cannot be determined
            if (type.ContainsGenericParameters)
            {
                throw new ArgumentException(
                    "{" + MethodBase.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                    "> contains generic parameters, so the default value cannot be retrieved");
            }

            // If the Type is a primitive type, or if it is another publicly-visible value type (i.e. struct), return a 
            //  default instance of the value type
            if (type.IsPrimitive || !type.IsNotPublic)
            {
                try
                {
                    return Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    throw new ArgumentException(
                        "{" + MethodBase.GetCurrentMethod() +
                        "} Error:\n\nThe Activator.CreateInstance method could not " +
                        "create a default instance of the supplied value type <" + type +
                        "> (Inner Exception message: \"" + e.Message + "\")", e);
                }
            }

            // Fail with exception
            throw new ArgumentException("{" + MethodBase.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" +
                                        type +
                                        "> is not a publicly-visible type, so the default value cannot be retrieved");
        }
    }
}
