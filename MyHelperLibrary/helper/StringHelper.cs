using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.International.Converters.PinYinConverter;

namespace MyHelper
{
   public class StringHelper
    {
        /// <summary>
        /// 得到中文字符串的首字母(不全)
        /// </summary>
        /// <param name="text">中文内容</param>
        /// <param name="isUp">是否为大写，默认True</param>
        /// <returns></returns>
        public static string GetChineseFirstSpell(string text, Boolean isUp = true)
        {
            int len = text.Length;
            string str = "";
            for (int i = 0; i < len; i++)
            {
                str += GetFirstSpell(text.Substring(i, 1));
            }
            if (isUp == false)
            {
                str = str.ToLower();
            }
            return str;
        }

        /// <summary>
        ///  得到单个中文的首字母
        /// </summary>
        /// <param name="cnChar"></param>
        /// <returns></returns>
        private static string GetFirstSpell(string cnChar)
        {
            Byte[] arrCn = Encoding.Default.GetBytes(cnChar);
            if (arrCn.Length > 1)
            {
                int area = (short)arrCn[0];
                int pos = (short)arrCn[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "*";
            }
            else return cnChar;
        }

        /// <summary>   
        /// 汉字转化为拼音  
        /// </summary>   
        /// <param name="str">汉字</param>  
        /// <param name="returnType">返回的类型，1大写 2 小写 3首字符大字</param>   
        ///  <param name="isHasSpace">是否用空格分开，默认True</param>   
        /// <returns>全拼</returns> 
        public static string GetAllSpell(string text, int returnType = 3, Boolean isHasSpace = true)
        {
            string temp = String.Empty;
            foreach (Char obj in text)
            {
                try
                {
                    ChineseChar cc = new ChineseChar(obj);
                    string c = cc.Pinyins[0].ToString();
                    ConsoleHelper.writeLine(c);
                    switch (returnType)
                    {
                        case 1:
                            if (isHasSpace == false)
                            {
                                temp += c.Substring(0, c.Length - 1);
                            }
                            else
                            {
                                temp += c.Substring(0, c.Length - 1) + " ";
                            }
                            break;
                        case 2:
                            if (isHasSpace == false)
                            {
                                temp += c.Substring(0, c.Length - 1).ToLower();
                            }
                            else
                            {
                                temp += c.Substring(0, c.Length - 1).ToLower() + " ";
                            }
                            break;
                        case 3:
                            if (isHasSpace == false)
                            {
                                temp += (c.Substring(0, 1) + c.Substring(1, c.Length - 2).ToLower());
                            }
                            else
                            {
                                temp += (c.Substring(0, 1) + c.Substring(1, c.Length - 2).ToLower()) + " ";
                            }
                            break;

                    }
                }
                catch
                {
                    temp += obj.ToString();
                }
            }
            return temp;
        }

        /// <summary>   
        /// 汉字转化为拼音首字母  
        /// </summary>   
        /// <param name="str">汉字</param>   
        /// <returns>首字母</returns>   
        public static string GetFirstPinyin(string str)
        {
            string r = string.Empty;
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }

        /// <summary>
        /// 数据库命名法转化为驼峰命名法
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string DBNamingToCamelCase(string name)
        {
            if (name == null || name.Length == 0) { return ""; }
            if (name.Contains("_"))
            {
                string[] words = name.Split('_');
                string result = string.Empty;
                for (int i = 0; i < words.Length; i++)
                {
                    if (i == 0)
                    {
                        result = words[i];
                    }
                    else
                    {
                        result += upperCaseFirstLetter(words[i]);
                    }
                }
                return result;
            }
            else
            {
                return name;
            }
        }
        /// <summary>
        /// 驼峰命名法转化为数据库命名法
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string camelCaseToDBnameing(string name)
        {
            if (name != null && name.Length > 0)
            {
                char[] array = name.ToCharArray();
                string result = string.Empty;
                for (int i=0;i<array.Length;i++)
                {
                    if (i == 0)
                    {
                        result +=  array[i].ToString().ToLower();
                    }
                    else {
                        if (isUpper(array[i]))
                        {
                            result += "_" + array[i].ToString().ToLower();
                        }
                        else
                        {
                            result += array[i].ToString();
                        }
                    }                   
                }
                return result;
            }
            return "";
        }

        /// <summary>
        /// 数据库表名转化为类名
        /// </summary>
        /// <param name="dbname"> 数据库表名</param>
        /// <returns>类名</returns>
        public static string dbNameToClassName(string dbname)
        {
            return upperCaseFirstLetter(dbname);
        }


        /// <summary>
        /// 将一个单词的第一个字母变为大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string upperCaseFirstLetter(string str)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        /// <summary>
        /// 判断字符是否为大写字母
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool isUpper(char c)
        {
            if (c > 'A' && c < 'Z')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
