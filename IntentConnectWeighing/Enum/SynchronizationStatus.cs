using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    enum SynchronizationStatus
    {
        AsyncUp,//0需要上传，以本地数据为依据
        Asynced,//1 本地数据和服务器数据一至
        AsuncDown // 2.需要更新数据，以服务器数据为依据
    }
    enum BillUpStatus
    {
        NoUp,//0没有上传过
        UpOne,//1 第一次上伟完成
        UpTwo // 2.第二次上伟完成
    }

    enum BillWeighingStatus {
        FinishOne,//第一次过磅完成
        FinishedTwo,//1 第二次过磅完成
    }
}
