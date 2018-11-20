using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace IntentConnectWeighing
{
  public interface ScaleDataInterpreter
    {
       ScaleDataResult ReadValue(SerialPort port);
    }

   public class ScaleDataResult
    {
      public  ScaleDataResult(int code, string msg, Double value) {
            this.ErrCode = code;
            this.Msg = msg;
            this.Value = value;
        }

        public ScaleDataResult( string msg, Double value)
        {
            this.Msg = msg;
            this.Value = value;
        }
        public int ErrCode
        {
            get; set;
        }

        public string Msg { get; set; }

        public double Value { get; set; }
    }
}