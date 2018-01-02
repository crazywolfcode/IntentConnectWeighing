using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyHelper
{
    public class FileHelper
    {
        /// <summary>
        /// 
        /// 判定相应位置下的文件是否存在
        /// </summary>
        /// <param name="path"> 路径</param>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }
        /// <summary>
        /// 判定相应位置下的文件夹是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FolderExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 判定相应位置下的文件夹是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FolderExistsCreater(string path)
        {
            if (Directory.Exists(path) == true)
            {
                return true;
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                try
                {
                    dir.Create();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("--------->" + e.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        public static void Write(string path, string content)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.BaseStream.Seek(0, SeekOrigin.Begin);
                    writer.Write(content);
                    writer.Flush();
                }
            }
        }
        /// <summary>
        /// 读文件 Encording gb2312
        /// </summary>
        /// <param name="path"> 路径</param>
        /// <returns> 文件内容</returns>
        public static string Reader(string path, Encoding encoding =null)
        {
            if (encoding == null) {
                encoding = Encoding.GetEncoding("gb2312");
            }
            
            if (File.Exists(path) == false)
            {
                return null;
            }
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, encoding))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filename">路径包含文件名</param>
        /// <returns>true or false</returns>
        public static bool createFile(string filename)
        {
            if (File.Exists(filename) == false)
            {
                try
                {
                    File.Create(filename);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return true;
            }            
        }

        /// <summary>
        /// 获取项目的根路径
        /// </summary>
        /// <returns>根路径</returns>
        public static string GetProjectRootPath()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string rootpath = path.Substring(0, path.LastIndexOf("\\"));
            rootpath = rootpath.Substring(0, rootpath.LastIndexOf("\\"));
            rootpath = rootpath.Substring(0, rootpath.LastIndexOf("\\"));
            return rootpath;
        }
        /// <summary>
        /// 获取项目运行时的根路径
        /// </summary>
        /// <returns></returns>
        public static string GetRunTimeRootPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory.ToString();
        }

    }
}
