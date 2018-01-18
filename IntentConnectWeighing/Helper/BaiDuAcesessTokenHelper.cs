using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHelper;
namespace IntentConnectWeighing
{
    class BaiDuAcesessTokenHelper
    {
        private static BDAccessToken mToken;
        public static string tokenFilePath =System.IO.Path.Combine(FileHelper.GetRunTimeRootPath() ,"temp");
        public static string tokenFileName = "baiduAcsessToken.xml";
        public static string tokenXmlFilePath = tokenFilePath + "\\" + tokenFileName;
        /// <summary>
        /// get baidu access token 
        /// </summary>
        /// <returns>string or null</returns>
        public static string getBaiDuAcesessToken()
        {
            string accessToken = getFromXml();
            if (!checkExpiresTime())
            {
                accessToken = getFromBaidu();
            }
            return accessToken;
        }

        private static string getFromXml()
        {
            try
            {
                if (MyHelper.FileHelper.FolderExistsCreater(tokenFilePath))
                {
                    if (!FileHelper.Exists(tokenXmlFilePath))
                    {
                        FileHelper.createFile(tokenXmlFilePath);
                        return null;
                    }
                    string tokenstr = FileHelper.Reader(tokenXmlFilePath);
                    if (!string.IsNullOrEmpty(tokenstr))
                    {
                        mToken = XmlHelper.Deserialize(typeof(BDAccessToken), tokenstr) as BDAccessToken;
                        if (mToken != null)
                        {                           
                            return mToken.accessToken;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }


        }

        private static string getFromBaidu()
        {           
            string resultJson = HttpWebRequestHelper.getBaiduAcesessToken(
                            BaiduAipHelper.BaiduTokenUrl,
                             ConfigurationHelper.GetConfig(ConfigItemName.baiduApiKey.ToString()),
                             ConfigurationHelper.GetConfig(ConfigItemName.baiduApiSecretKey.ToString())
                             ).Result;
           string replaceJson= StringHelper.jsonCamelCaseToDBnameing(resultJson);
            mToken =JsonHelper.JsonToObject(replaceJson, typeof(BDAccessToken)) as BDAccessToken;
            mToken.getDate = DateTimeHelper.getCurrentDateTime();
            if (mToken.error != null)
            {
                return null;
            }
            else
            {
                writeToxml();           
                return mToken.accessToken;
            }

        }

        private static bool checkExpiresTime()
        {
            if (mToken == null)
            {
                return false;
            }
            long lasttime = DateTimeHelper.ConvertDateTimeToInt(DateTime.Parse(mToken.getDate));
            long nowTime = DateTimeHelper.ConvertDateTimeToInt(DateTime.Now);
            //the  subtracted 1200 is to make the exprise time less then default 30 days
            if (nowTime - lasttime < Convert.ToInt64(mToken.expiresIn) - 1200)
            {
                return true;
            }
            return false;
        }

        private static void writeToxml()
        {
            if (mToken == null || mToken.error != null)
            {
                return;
            }
            String xml = XmlHelper.Serialize(typeof(BDAccessToken), mToken);
            FileHelper.Write(tokenXmlFilePath, xml);
        }
    }
}
