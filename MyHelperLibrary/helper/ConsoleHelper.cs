using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyHelper
{
    /// <summary>
    /// 调试的日志助手类
    /// </summary>
    public class ConsoleHelper
    {
        private static string Tag = "------------------>";

        public static void writeLine(string msg)
        {
            Console.WriteLine(Tag + msg);
        }
        public static void write(string msg)
        {
            Console.Write(Tag + msg);
        }
    }
}
