using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    public class ScannerGunHelper
    {
        private static ScannerGunHelper mHelper;
        private static ScannerGunHook hook = null;
        private static IScannerGunInterface mInterface;
        private static bool isShift = false;

        public static ScannerGunHelper GetInstance(IScannerGunInterface gunInterface)
        {
            mInterface = gunInterface;
            if (mHelper == null)
            {
                hook = new ScannerGunHook();                
                mHelper = new ScannerGunHelper();
            }
            return mHelper;
        }

        public  void Start()
        {
            if (hook != null) {
                hook.KeyDownEvent += Hook_KeyDownEvent;
                hook.Start();
            }          
        }

        private DateTime previewTime;
        private StringBuilder inputKey = new StringBuilder();
        private  void Hook_KeyDownEvent(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            String t = string.Empty;
            DateTime dateTime = DateTime.Now;
            int timediff = (dateTime - previewTime).Milliseconds;
            t = OemKeyToVisualKey(e);
            if (string.IsNullOrEmpty(t))
            {
                return;
            }
            if (inputKey.Length == 0)
            {
                inputKey.Append(t);
            }
            else if (timediff <= 20)
            {
                if (e.KeyValue == (int)System.Windows.Forms.Keys.Return)
                {
                    string res = inputKey.ToString();
                    inputKey.Clear();
                    mInterface.ScanedFinished(res);
                }
                else
                {
                    inputKey.Append(t);
                }
            }
            else
            {
                inputKey.Clear();
            }
            previewTime = dateTime;
        }

        public static void Close()
        {
            if (hook != null)
            {
                hook.Stop();    
            }
            if (mInterface != null)
            {            
                mInterface = null;             
            }
            if (mHelper != null)
            {             
                mHelper = null;
            }
        }

        private  string OemKeyToVisualKey(System.Windows.Forms.KeyEventArgs args)
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
            return res;
        }
    }
}
