using System;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace IntentConnectWeighing
{
    class YaoHuanDataInterpreter : ScaleDataInterpreter
    {

        ScaleDataResult ScaleDataInterpreter.ReadValue(SerialPort port)
        {

            if (port.IsOpen == false) {
                try
                {
                    port.Open();
                }
                catch (Exception e)
                {
                    return new ScaleDataResult(-1, "串口打开失败：" + e.Message, 0);
                }
            }         
            Int32 value = -1;
            ScaleDataResult result=   new ScaleDataResult(value, "数据解释出错", 0);
            try
            {
                int bytes = port.BytesToRead;
                byte[] buffer = new byte[bytes];
                port.Read(buffer, 0, bytes);
                string readStr = port.Encoding.GetString(buffer);
                string oldStr = String.Empty;
                if (readStr.Length >= 10)
                {
                    string[] weightstrs = readStr.Split('+');
                    foreach (string weightstr in weightstrs)
                    {
                        if (weightstr.Length >= 10)
                        {
                            string weightst = weightstr.Substring(0, 6);
                            if (oldStr != weightst)
                            {
                                oldStr = weightst;
                                string temp = Regex.Replace(weightst, "[0]*([1-9][0-9]*)", "$1");
                                value = Convert.ToInt32(temp);
                                Double dou = Convert.ToDouble(value);
                                Double number = Convert.ToDouble(Math.Round((dou / 1000), 2, MidpointRounding.AwayFromZero));
                                result.Value = number;
                                result.ErrCode = 0;
                                result.Msg = "成功";
                            }
                        }
                    }
                }
            }
            catch(Exception e) {
                result.Msg = result.Msg + " :" + e.Message;
            }
            return result;
        }
    }
}
