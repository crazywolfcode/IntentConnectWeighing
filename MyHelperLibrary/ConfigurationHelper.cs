using System;
using System.Configuration;
using System.Xml;
namespace MyHelper
{
    /// <summary>
    /// the helper of config file manager
    /// the path of config file must in project root path 
    ///  config file's name must is "App.config"
    /// </summary>
    public class ConfigurationHelper
    {
        public static string ConnectionStringsSectionName = "connectionStrings";
        public static string AppSettingsSectionName = "appSettings";
        /// <summary>
        /// 置配制的值,会有异常，有异常是返回null
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConfig(string configName)
        {
            try
            {
                return ConfigurationManager.AppSettings[configName];
            }
            catch (Exception)
            {
                return null;
            }

        }
        /// <summary>
        /// set the config item's value,if not Exist Auto Create,last save to config file
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="value"></param>
        public static void SetConfig(string configName, string value)
        {
            Configuration cf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            bool exists = false;
            foreach (string key in cf.AppSettings.Settings.AllKeys)
            {
                if (key == configName)
                {
                    exists = true;
                    continue;
                }
            }
            if (exists)
            {
                cf.AppSettings.Settings[configName].Value = value;
            }
            else
            {
                cf.AppSettings.Settings.Add(configName, value);                
            }
            cf.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(AppSettingsSectionName);
            // save config value to file
            SvaeConfigValueToFile(configName, value);
        }
        /// <summary>
        /// 依据连接串名字connectionName返回数据连接字符串 
        /// </summary>
        /// <param name="configName">依据连接串名字</param>
        /// <returns></returns>
        public static string GetConnectionConfig(string configName)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[configName].ConnectionString.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }


        ///<summary>
        ///更新连接字符串 如果没有则自动创建
        ///</summary>
        ///<param name="newName">连接字符串名称</param>
        ///<param name="newConString">连接字符串内容</param>
        public static void SetConnectionConfig(string configName, string value)
        {
            bool isExists = false;
            if (ConfigurationManager.ConnectionStrings[configName] != null)
            {
                isExists = true;
            }
            ConnectionStringSettings css = new ConnectionStringSettings(configName, value);
            Configuration cf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (isExists)
            {
                cf.ConnectionStrings.ConnectionStrings.Remove(configName);
            }
            cf.ConnectionStrings.ConnectionStrings.Add(css);
            cf.Save();
            ConfigurationManager.RefreshSection(ConnectionStringsSectionName);
            SvaeConfigValueToFile(configName, value, "connectionStrings");
        }

        /// <summary>
        /// update config item's value and save it in configuation file
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="value"></param>
        /// <param name="groupName">appSettings or connectionSettings </param>
        public static void SvaeConfigValueToFile(string configName, string value, string groupName = "appSettings")
        {
            string configFileName = "App.config";
            string filePath = FileHelper.GetProjectRootPath() + "/" + configFileName;
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode group = doc.DocumentElement.SelectSingleNode(groupName);
            if (group == null)
            {
                group = doc.CreateNode(XmlNodeType.Element, groupName, null);
                group.AppendChild(createXmlNode(doc, configName, value, groupName));
                doc.AppendChild(group);
            }
            bool isFind = false;
            foreach (XmlNode item in group.ChildNodes)
            {
                if (groupName == "connectionStrings")
                {
                    if (item.Attributes["name"].InnerText == configName)
                    {
                        item.Attributes["connectionString"].InnerText = value;
                        isFind = true;
                        continue;
                    }
                }
                else
                {
                    if (item.Attributes["key"].InnerText == configName)
                    {
                        item.Attributes["value"].InnerText = value;
                        isFind = true;
                        continue;
                    }
                }
            }
            if (isFind == false)
            {
                group.AppendChild(createXmlNode(doc, configName, value, groupName));               
            }
            doc.Save(filePath);
        }

        private static XmlNode createXmlNode(XmlDocument doc, string configName, string value, string groupName)
        {
            string name = "key";
            string valuetag = "value";
            if (groupName == "connectionStrings")
            {
                name = "name";
                valuetag = "connectionString";
            }
            XmlNode node = doc.CreateElement("add") as XmlNode;
            XmlAttribute att = doc.CreateAttribute(name);
            att.InnerText = configName;
            XmlAttribute val = doc.CreateAttribute(valuetag);
            val.InnerText = value;
            node.Attributes.Append(att);
            node.Attributes.Append(val);
            return node;
        }
        
        //---
    }
}
