using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    /// <summary>
    /// 为入库明细报表 显示列表统计而创建
    /// </summary>

  public  class ReportWeightListV
    {  
        public String company { get; set; }
        public String yard { get; set; }
        public String material { get; set; }
        /// <summary>
        /// 车辆数
        /// </summary>
        public Int64 Cars { get; set; }
        public Double grossweight { get; set; }
        public Double traeweight { get; set; }
        /// <summary>
        /// 总吨数
        /// </summary>
        public Double weight { get; set; }
    }
}
