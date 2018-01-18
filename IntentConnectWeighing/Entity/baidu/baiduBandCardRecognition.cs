using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    class BaiduBandCardRecognition
    {
        public string errorCode { get; set; }      
        public string ErrorMsg { get; set; }
        public string bankCardNumber { get; set; }
        /// <summary>
        ///银行卡类型，0:不能识别; 1: 借记卡; 2: 信用卡
        /// </summary>
        public string bankCardType { get; set; }
        public string bankName { get; set; }     

    }
}
