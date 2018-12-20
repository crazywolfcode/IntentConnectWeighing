using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ScaleDataInterpreter
{

    /// <summary>
    /// 磅称数据解释器
    /// </summary>
    public interface IScaleDataInterpreter
    {
        ScaleDataResult ReadValue();
    }

    public class ScaleDataResult
    {
        public ScaleDataResult(int code, string msg, Double value)
        {
            this.ErrCode = code;
            this.Msg = msg;
            this.Value = value;
        }

        public ScaleDataResult(string msg, Double value)
        {
            this.Msg = msg;
            this.Value = value;
        }
        public int ErrCode { get; set; }

        public string Msg { get; set; }

        public double Value { get; set; }
    }
}