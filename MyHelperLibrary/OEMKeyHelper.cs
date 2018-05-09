using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper
{
    public class OEMKeyHelper
    {
        private static bool isShift = false;


        public static string OemKeyToVisualKey(System.Windows.Forms.KeyEventArgs args)
        {
            String res = string.Empty;
            if (args != null)
            {
                if (args.KeyData == System.Windows.Forms.Keys.RShiftKey || args.KeyData == System.Windows.Forms.Keys.LShiftKey)
                {
                    isShift = true;
                    return string.Empty;
                }
                if (args.KeyData >= System.Windows.Forms.Keys.NumPad0 && args.KeyData <= System.Windows.Forms.Keys.NumPad9)
                {
                    res = args.KeyData.ToString().Last().ToString();
                }
                else if ((args.KeyData >= System.Windows.Forms.Keys.D0 && args.KeyData <= System.Windows.Forms.Keys.D9))
                {
                    switch (args.KeyData)
                    {
                        case System.Windows.Forms.Keys.D0:
                            if (isShift == true)
                            {
                                res = ")";
                            }
                            else
                            {
                                res = args.KeyData.ToString().Last().ToString();
                            }
                            break;
                        case System.Windows.Forms.Keys.D1:
                            if (isShift == true) { res = "!"; }
                            else
                            {
                                res = args.KeyData.ToString().Last().ToString();
                            }
                            break;
                        case System.Windows.Forms.Keys.D2:
                            if (isShift == true) { res = "@"; }
                            else
                            {
                                res = args.KeyData.ToString().Last().ToString();
                            }
                            break;
                        case System.Windows.Forms.Keys.D3:
                            if (isShift == true) { res = "#"; }
                            else
                            {

                                res = args.KeyData.ToString().Last().ToString();
                            }
                            break;
                        case System.Windows.Forms.Keys.D4:
                            if (isShift == true) { res = "$"; }
                            else { res = args.KeyData.ToString().Last().ToString(); }
                            break;
                        case System.Windows.Forms.Keys.D5:
                            if (isShift == true) { res = "%"; }
                            else { res = args.KeyData.ToString().Last().ToString(); }
                            break;
                        case System.Windows.Forms.Keys.D6:
                            if (isShift == true) { res = "^"; }
                            else { res = args.KeyData.ToString().Last().ToString(); }
                            break;
                        case System.Windows.Forms.Keys.D7:
                            if (isShift == true) { res = "&"; }
                            else { res = args.KeyData.ToString().Last().ToString(); }
                            break;
                        case System.Windows.Forms.Keys.D8:
                            if (isShift == true) { res = "*"; }
                            else { res = args.KeyData.ToString().Last().ToString(); }
                            break;
                        case System.Windows.Forms.Keys.D9:
                            if (isShift == true) { res = "("; }
                            else { res = args.KeyData.ToString().Last().ToString(); }
                            break;
                    }
                }
                else
                {
                    switch (args.KeyData)
                    {
                        case System.Windows.Forms.Keys.Oem1:
                            if (isShift == true) { res = ":"; }
                            else { res = ";"; }
                            break;
                        case System.Windows.Forms.Keys.Oem2:
                            if (isShift == true) { res = "?"; }
                            else { res = "/"; }
                            break;
                        case System.Windows.Forms.Keys.Oem3:
                            if (isShift == true) { res = "~"; }
                            else { res = "`"; }
                            break;
                        case System.Windows.Forms.Keys.Oem4:
                            if (isShift == true) { res = "{"; }
                            else { res = "["; }
                            break;
                        case System.Windows.Forms.Keys.Oem5:
                            if (isShift == true) { res = "|"; }
                            else { res = "\\"; }
                            break;
                        case System.Windows.Forms.Keys.Oem6:
                            if (isShift == true) { res = "}"; }
                            else { res = "]"; }
                            break;
                        case System.Windows.Forms.Keys.Oem7:
                            if (isShift == true) { res = "\""; }
                            else { res = "'"; }
                            break;
                        case System.Windows.Forms.Keys.OemMinus:
                            if (isShift == true) { res = "_"; }
                            else { res = "-"; }
                            break;
                        case System.Windows.Forms.Keys.Oemplus:
                            if (isShift == true) { res = "+"; }
                            else { res = "="; }
                            break;
                        case System.Windows.Forms.Keys.Oemcomma:
                            if (isShift == true) { res = "<"; }
                            else { res = ","; }
                            break;
                        case System.Windows.Forms.Keys.OemPeriod:
                            if (isShift == true) { res = ">"; }
                            else { res = "."; }
                            break;
                        default:
                            if (isShift == false) { res = args.KeyData.ToString().ToLower(); }
                            else { res = args.KeyData.ToString(); }
                            break;
                    }
                }
            }
            isShift = false;
            MyHelper.ConsoleHelper.writeLine(res);
            return res;
        }
    }
}
