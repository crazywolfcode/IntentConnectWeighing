using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    public class BaseDataClassV
    {
        public Company send { get; set; }
        public Company receive { get; set; }
        public Material material { get; set; }
        public CarInfo carInfo { get; set; }
    }
}
