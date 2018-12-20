using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleDataInterpreter
{
    public class TLDDataInterpreter : DataInterpreter, IScaleDataInterpreter
    {
        ScaleDataResult IScaleDataInterpreter.ReadValue()
        {
            return new ScaleDataResult(-1, "不支持的显示控制器，请联系系统管理员" , 0);
        }
    }
}
