
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace IntentConnectWeighing
{

	/// <summary>
    /// 
    ///</summary>

    public  class weighingBill
    {
        public String id { get; set; }

        public Int64 type { get; set; }

        public String relativeBillId { get; set; }

        public String sendNumber { get; set; }

        public String sendCompanyId { get; set; }

        public String sendCompanyName { get; set; }

        public String sendInTime { get; set; }

        public String sendOutTime { get; set; }

        public String sendUserId { get; set; }

        public String sendUserName { get; set; }

        public Double sendGrossWeight { get; set; }

        public Double sendTraeWeight { get; set; }

        public Double sendNetWeight { get; set; }

        public String sendMaterialId { get; set; }

        public String sendMaterialName { get; set; }

        public String sendRemark { get; set; }

        public String receiveNumber { get; set; }

        public String receiveCompanyId { get; set; }

        public String receiveCompanyName { get; set; }

        public String receiveInTime { get; set; }

        public String receiveOutTime { get; set; }

        public String receiveUserId { get; set; }

        public String receiveUserName { get; set; }

        public Double receiveGrossWeight { get; set; }

        public String receiveTraeWeight { get; set; }

        public String receiveMaterialId { get; set; }

        public String receiveMaterialName { get; set; }

        public String receiveRemark { get; set; }

        public String receiveNetWeight { get; set; }

        public String differenceWeight { get; set; }

        public String deducationWeiht { get; set; }

        public String deducationDescription { get; set; }

        public String carId { get; set; }

        public String plateNumber { get; set; }

        public String driver { get; set; }

        public String driverMobile { get; set; }

        public String driverIdNumber { get; set; }

        public String prINTERGERDateTime { get; set; }

        public Int64 prINTERGERTimes { get; set; }

        public Int64 upStatus { get; set; }

        public Int64 syncTime { get; set; }

        public String affiliatedCompanyId { get; set; }

        public Int64 isDelete { get; set; }

        public String deleteTime { get; set; }

    }
}
