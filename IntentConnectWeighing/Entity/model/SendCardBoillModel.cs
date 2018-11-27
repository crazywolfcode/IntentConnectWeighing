using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    class SendCardBillModel
    {
        /// <summary>
        /// 获取未过磅的派车单
        /// </summary>
        /// <returns></returns>
        public static List<SendCarBill> GetSendCarBill() {
            List<SendCarBill> list = null;
            string time = MyHelper.DateTimeHelper.getCurrentDateTime();
            String conditon = SendCarBillEnum.weight_status + "=" + 0 + " and " +
              SendCarBillEnum.send_company_id.ToString() + "=" + Constract.valueSplit + App.currentCompany.id + Constract.valueSplit + " and " +
              SendCarBillEnum.send_yard_id.ToString() + "=" + Constract.valueSplit + App.currentYard.id + Constract.valueSplit + " and " +
              SendCarBillEnum.effective_date.ToString() + "<=" + Constract.valueSplit + time + Constract.valueSplit + " and " + SendCarBillEnum.expiry_date.ToString() + ">=" + Constract.valueSplit + time + Constract.valueSplit ;           
            String sql =DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.send_car_bill.ToString(), null, conditon, null, null, SendCarBillEnum.effective_date + " desc ");
            list =DatabaseOPtionHelper.GetInstance().select<SendCarBill>(sql);
            return list;
        }

        public static SendCarBill GetById(string sendCarBillId)
        {
            List<SendCarBill> list = null;
            String conditon = SendCarBillEnum.id.ToString() + "=" + Constract.valueSplit + sendCarBillId + Constract.valueSplit;
            String sql =DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.send_car_bill.ToString(), null, conditon, null, null, null,1);
            list =DatabaseOPtionHelper.GetInstance().select<SendCarBill>(sql);
            if (list == null || list.Count <= 0) {
                return null;
            }
            return list[0];
        }
    }
}
