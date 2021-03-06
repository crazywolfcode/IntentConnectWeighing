﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using System.Xml;

namespace MyHelper
{
    /// <summary>
    /// 
    /// </summary>
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
        /// 一个Json串生成具体对象
        /// </summary>
        /// <typeparam name="T">对象的类</typeparam>
        /// <param name="jsonString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(string jsonString)
        {
            return (T)JsonConvert.DeserializeObject(jsonString, typeof(T));            
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
   

    }
}
