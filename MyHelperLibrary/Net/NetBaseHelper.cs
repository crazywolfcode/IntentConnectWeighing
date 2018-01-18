using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;


namespace MyHelper
{
    /// <summary>
    /// 网络请求助手类的基类
    /// </summary>
    public class NetBaseHelper
    {
        public static readonly string keepAlive = "keep-alive";
        public static readonly string defaultUrl = "www.baidu.com";
        public static readonly string ContentTypeFormUrlEncoded = "application/x-www-form-urlencoded";
        /// <summary>
        /// get request Parameters
        /// </summary>
        /// <param name="postData">like A=b&C=d</param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> getListKeyValuePAir(string postData)
        {
            List<KeyValuePair<string, string>> parametes = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrEmpty(postData))
            {
                if (postData.Contains("&"))
                {
                    string[] temp = postData.Split('&');
                    foreach (string str in temp)
                    {
                        if (str.Contains('='))
                        {
                            string[] strs = str.Split('=');
                            parametes.Add(new KeyValuePair<string, string>(strs[0], strs[1]));
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            return parametes;
        }
        /// <summary>
        /// check whether the internet is normal
        /// </summary>
        /// <param name="url"> host name or ip address</param>
        /// <returns>true or false</returns>
        public static bool IsConnectedToInternet(string url = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = defaultUrl;
            }
            Ping ping = new Ping();
            PingReply reply = ping.Send(url);
            if(reply.Status == IPStatus.Success)
            {
                return true;
            }
            return false;
        }

        private const int INTERNET_CONNECTION_MODEM = 1;
        private const int INTERNET_CONNECTION_LAN = 2;

        [System.Runtime.InteropServices.DllImport("winInet.dll")]
        private static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved);

        /// <summary>
        /// 获取本地的连接状态
        /// get local internet connection status
        /// </summary>
        /// <returns>0 未连网 1 采用网卡上网 2 采用调制解调器上网</returns>
        public static int getLocalConnectionStatus()
        {
            Int32 dwFlag = new Int32();
            if (!InternetGetConnectedState(ref dwFlag, 0))
            {
                return 0;
            }
            else
            {
                if ((dwFlag & INTERNET_CONNECTION_MODEM) != 0)
                {
                    return 2;
                }
                else if ((dwFlag & INTERNET_CONNECTION_LAN) != 0)
                {
                    return 1;
                }
            }
            return 0;
        }
    }
    
    public class RequestContent
    {
        /// <summary>
        /// 请求地址
        /// </summary>    
        public string Url { get; set; }

        /// <summary>
        /// 传入数据
        /// </summary>
        public string Data { get; set; }
    }

    public class ResponseContent
    {
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }
    }
}
