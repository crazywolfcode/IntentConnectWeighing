using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MyHelper
{
    /// <summary>
    /// 加密和解密码类
    /// </summary>
    class EncryptHelper
    {
        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="text">明文</param>
        /// <returns>32位 密文</returns>
        public static string MD5Encrypt(string text, bool isUpper = true)
        {
            byte[] bytes = Encoding.Default.GetBytes(text.Trim());
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] b = md5.ComputeHash(bytes);
            string pwd = string.Empty;
            pwd = BitConverter.ToString(b);
            if (isUpper == true)
            {
                return pwd.Replace("-", "");
            }
            return pwd.Replace("-", "").ToLower();
        }

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="text">明文</param>
        /// <returns> 16 位 密文</returns>
        public static string MD5Encrypt16(string ConvertString, bool isUpper = true)
        {          
            string t2 = BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            t2 = t2.Replace("-", "");
            if (isUpper == true) {
                return t2;
            }
            else
            {
                return t2.ToLower();
            }                       
        }

    }
}
