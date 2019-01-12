using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ScaleDataInterpreter
{
    public class YaoHuanDataInterpreter : DataInterpreter, IScaleDataInterpreter
    {

        ScaleDataResult IScaleDataInterpreter.ReadValue()
        {
            if (mSerialPort.IsOpen == false)
            {
                try
                {
                    mSerialPort.Open();
                }
                catch (Exception e)
                {
                    return new ScaleDataResult(-1, "串口打开失败：" + e.Message, -1.0);
                }
            }
            Int32 value = -1;
            ScaleDataResult result = new ScaleDataResult(value, "数据解释出错", -1);
            try
            {
                int bytes = mSerialPort.BytesToRead;
                byte[] buffer = new byte[bytes];
                bool isNegative = false;
                mSerialPort.Read(buffer, 0, bytes);
                mSerialPort.Encoding = Encoding.UTF8;
                string readStr = mSerialPort.Encoding.GetString(buffer);                
                string oldStr = String.Empty;                 
                 if(readStr.Length >= 10){
                    string[] weightstrs;
                    if (readStr.Contains("-")) {
                        weightstrs = readStr.Split('-');
                        isNegative = true;
                    }
                    else
                    {
                        isNegative = false;
                        weightstrs = readStr.Split('+');
                    }
                    foreach (string weightstr in weightstrs)
                    {
                        if (weightstr.Length >= 10)
                        {
                            string values = weightstr.Substring(0, 7);
                            if (oldStr != values)
                            {
                                if (isNegative == true) {
                                    values = "-" + values;
                                }
                                oldStr = values;
                                Double dou = Convert.ToDouble(values);
                                Double number = Convert.ToDouble(Math.Round((dou / 10000), 2, MidpointRounding.AwayFromZero));
                                result.Value = number;
                                result.ErrCode = 0;
                                result.Msg = "成功";
                            }
                        }                      
                    }
                }
            }
            catch (Exception e)
            {
                result.Msg = result.Msg + " :" + e.Message;
            }
            return result;
        }
    }
}
