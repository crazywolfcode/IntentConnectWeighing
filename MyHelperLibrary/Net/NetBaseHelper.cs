using System;
using System.Collections.Generic;
using System.Linq;


namespace MyHelper
{
    /// <summary>
    /// 网络请求助手类的基类
    /// </summary>
    public class NetBaseHelper
    {
        public static readonly string keepAlive = "keep-alive";

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
