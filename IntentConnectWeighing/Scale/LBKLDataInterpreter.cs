using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    class LBKLDataInterpreter : ScaleDataInterpreter
    {
        public ScaleDataResult ReadValue(SerialPort port)
        {
            if (port.IsOpen == false)
            {
                try
                {
                    port.Open();
                }
                catch (Exception e)
                {
                    return new ScaleDataResult(-1, "串口打开失败：" + e.Message, 0);
                }
            }
            ScaleDataResult result = new ScaleDataResult(-1, "数据解释失败！", 0);
            try {
                int bytes = port.BytesToRead;
                byte[] buffer = new byte[bytes];
                port.Read(buffer, 0, bytes);
                string str = port.Encoding.GetString(buffer);             
                result.Value = Convert.ToDouble(str);
                result.ErrCode = 0;
                result.Msg = "成功";
            } catch (Exception e){
                result.Msg = result.Msg + ": " + e.Message;
            }            
            return result;
        }
    }
}
