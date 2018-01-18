using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using System.Xml;

namespace MyHelper
{
    public class JsonHelper
    {
        /// <summary>
        /// 从一个XMl生成Json
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>Json</returns>
        public static string XmlToJson(string xml)
        {
            XmlDocument ad = new XmlDocument();
            ad.LoadXml(xml);
            return JsonConvert.SerializeXmlNode(ad.DocumentElement);
        }

        /// <summary>
        /// 从一个对象信息生成Json串 包含DataTable
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 从一个Json串生成对象信息
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object JsonToObject(string jsonString, Type type)
        {
            return JsonConvert.DeserializeObject(jsonString, type);
        }
        /// <summary>
        /// 从一个Json串生成DataTable对象信息
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string jsonString)
        {
            return JsonConvert.DeserializeObject<DataTable>(jsonString);
        }
        /// <summary>
        ///  Table To Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> TableToEntity<T>(DataTable dt) where T : class, new()
        {
            Type type = typeof(T);
            List<T> list = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                PropertyInfo[] pArray = type.GetProperties();
                T entity = new T();
                foreach (PropertyInfo p in pArray)
                {
                    if (row[StringHelper.camelCaseToDBnameing(p.Name)] is DBNull)
                    {
                        p.SetValue(entity, null, null);
                        continue;
                    }
                    p.SetValue(entity, row[StringHelper.camelCaseToDBnameing(p.Name)], null);
                }
                list.Add(entity);
            }
            return list;
        }

    }
}
