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

    public class ReportWeightSummaryV
    {

        public Int64 companys { get; set; }
        public Int64 materials { get; set; }
        public Int64 cars { get; set; }
        public Double sendweight { get; set; }
        public Double grossweight { get; set; }
        public Double traeweight { get; set; }
        public Double netweight { get; set; }
        public Double decuationweight { get; set; }
        public Double differenceweight { get; set; }
    }
}
