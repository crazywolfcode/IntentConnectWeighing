using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    class BaiduDriverLicenseRecognition
    {
        public string errorCode { get; set; }
        public string ErrorMsg { get; set; }
        public string number { get; set; }
        public string expriseDate { get; set; }
        public string type { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string address { get; set; }
        public string name { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        public string country { get; set; }
        public string birth { get; set; }
        public string sex { get; set; }
        /// <summary>
        /// 初次领证日期
        /// </summary>
        public string firstDate { get; set; }
    }
}
