using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleDataInterpreter
{

    public class DataInterpreter
    {
        public static System.IO.Ports.SerialPort mSerialPort;
        public static IScaleDataInterpreter GetDataInterpreter(Int32 type, System.IO.Ports.SerialPort port)
        {
            mSerialPort = port;
            IScaleDataInterpreter scaleDataInterpreter;
            switch (type)
            {
                case (int)ScaleBrandType.YH:
                    scaleDataInterpreter = new YaoHuanDataInterpreter();
                    break;
                case (int)ScaleBrandType.LBKL:
                    scaleDataInterpreter = new LBKLDataInterpreter();
                    break;
                case (int)ScaleBrandType.TLD:
                    scaleDataInterpreter = new TLDDataInterpreter();
                    break;
                case (int)ScaleBrandType.SDLS:
                    scaleDataInterpreter = new SDLSDataInterpreter();
                    break;
                default:
                    scaleDataInterpreter = new NotSuporInterprete();
                    break;
            }
            return scaleDataInterpreter;
        }
    }
}
