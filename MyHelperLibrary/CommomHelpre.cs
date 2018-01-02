using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MyHelper
{
 public  class CommomHelpre
    {

        /// <summary>
        /// 获取从start到end 之间的一个随机数
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Int32 getRand(int start, int end)
        {
            return new Random().Next(start, end);
        }
        /// <summary>
        /// 获取一个0.1到1.0 之间的随机数
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Double getRand(double start, double end)
        {
            return new Random().NextDouble();
        }

      /// <summary>
      /// 获取本地IP
      /// </summary>
      /// <returns>IP</returns>       
        public static string getLoclIp()
        {
            string ip = string.Empty;
            foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {              
                if (address.AddressFamily.ToString() == "InterNetwork")
                {
                    ip = address.ToString();
                }
            }
            return ip;
        }
    }
}
