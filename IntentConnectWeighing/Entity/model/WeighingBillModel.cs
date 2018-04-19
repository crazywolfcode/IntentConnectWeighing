using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    public class WeighingBillModel
    {
        public static WeighingBill GetById(String id)
        {
            string condition = WeighingBillEnum.id.ToString() + "=" + Constract.valueSplit + id + Constract.valueSplit;
            String sql = MyHelper.DbBaseHelper.getSelectSql(DataTabeName.weighing_bill.ToString(), null, condition, null, null, null, 1);
            List<WeighingBill> list = MyHelper.DbBaseHelper.DataTableToEntity<WeighingBill>(DatabaseOPtionHelper.GetInstance().select(sql));
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// obtian invoice
        /// </summary>
        /// <returns></returns>
        public static List<WeighingBill> GetInvoice()
        {
            List<WeighingBill> list = new List<WeighingBill>();
            String conditon = @WeighingBillEnum.send_status + "=" + 1 + " and " +
              WeighingBillEnum.type.ToString() + "=" + ((int)WeightingBillType.CK) + " and " +
              WeighingBillEnum.relative_bill_id.ToString() + " is null and " +
              WeighingBillEnum.receive_yard_id.ToString() + "=" + Constract.valueSplit + App.currentYard.id + Constract.valueSplit + " and " +
              WeighingBillEnum.receive_company_id.ToString() + "=" + Constract.valueSplit + App.currentCompany.id + Constract.valueSplit;

            String sql = MyHelper.DbBaseHelper.getSelectSql(DataTabeName.weighing_bill.ToString(), null, conditon, null, null, WeighingBillEnum.send_out_time + " desc ", 20);
            list = MyHelper.DbBaseHelper.DataTableToEntity<WeighingBill>(DatabaseOPtionHelper.GetInstance().select(sql));
            return list;
        }
        /// <summary>
        /// 获取未完成入库数据 ，不传开始时间和结束时间，则是获取所有的数据
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public static List<WeighingBill> GetInNoFinished(String startDate = null, String endDate = null)
        {
            String condition = WeighingBillEnum.receive_status.ToString() + "=" + 0 + " and " + WeighingBillEnum.type.ToString() + " = " + ((int)WeightingBillType.RK);
            if (!string.IsNullOrEmpty(startDate))
            {
                condition += " and " + WeighingBillEnum.receive_in_time.ToString() + " >=" + Constract.valueSplit + startDate + Constract.valueSplit;
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                condition += " and " + WeighingBillEnum.receive_in_time.ToString() + " <=" + Constract.valueSplit + endDate + Constract.valueSplit;
            }
            String sql = MyHelper.DbBaseHelper.getSelectSql(DataTabeName.weighing_bill.ToString(), null, condition);
            List<WeighingBill> list = MyHelper.DbBaseHelper.DataTableToEntity<WeighingBill>(DatabaseOPtionHelper.GetInstance().select(sql));
            return list;
        }

        /// <summary>
        /// 获取已经完成入库数据，不传开始时间和结束时间，则是获取当天的数据 
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public static List<WeighingBill> GetInFinished(String startDate = null, String endDate = null)
        {
            String condition = WeighingBillEnum.receive_status.ToString() + "=" + 1 + " and " + WeighingBillEnum.type.ToString() + " = " + ((int)WeightingBillType.RK);
            if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                condition += " and " + WeighingBillEnum.receive_out_time.ToString() + " like '%" + MyHelper.DateTimeHelper.getCurrentDateTime(MyHelper.DateTimeHelper.DateFormat) + "%'";
            }
            else
            {
                if (!string.IsNullOrEmpty(startDate))
                {
                    condition += " and " + WeighingBillEnum.receive_out_time.ToString() + " >=" + Constract.valueSplit + startDate + Constract.valueSplit;
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    condition += " and " + WeighingBillEnum.receive_out_time.ToString() + " <=" + Constract.valueSplit + endDate + Constract.valueSplit;
                }
            }
            String sql = MyHelper.DbBaseHelper.getSelectSql(DataTabeName.weighing_bill.ToString(), null, condition);
            List<WeighingBill> list = MyHelper.DbBaseHelper.DataTableToEntity<WeighingBill>(DatabaseOPtionHelper.GetInstance().select(sql));
            return list;
        }

        /// <summary>
        /// 获取未完成的出库数据，不传开始时间和结束时间，则是获取所有的数据
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public static List<WeighingBill> GetOutNOFinished(String startDate = null, String endDate = null)
        {
            String condition = WeighingBillEnum.send_status.ToString() + "=" + 0 + " and " + WeighingBillEnum.type.ToString() + " = " + ((int)WeightingBillType.CK);
            if (!string.IsNullOrEmpty(startDate))
            {
                condition += " and " + WeighingBillEnum.send_in_time.ToString() + " >=" + Constract.valueSplit + startDate + Constract.valueSplit;
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                condition += " and " + WeighingBillEnum.send_in_time.ToString() + " <=" + Constract.valueSplit + endDate + Constract.valueSplit;
            }
            String sql = MyHelper.DbBaseHelper.getSelectSql(DataTabeName.weighing_bill.ToString(), null, condition);
            List<WeighingBill> list = MyHelper.DbBaseHelper.DataTableToEntity<WeighingBill>(DatabaseOPtionHelper.GetInstance().select(sql));
            return list;
        }
        /// <summary>
        /// 获取已经完成的出库数据，不传开始时间和结束时间，则是获取当天的数据 
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public static List<WeighingBill> GetOutFinished(String startDate = null, String endDate = null)
        {
            String condition = WeighingBillEnum.send_status.ToString() + "=" + 1 + " and " + WeighingBillEnum.type.ToString() + " = " + ((int)WeightingBillType.CK);
            if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                condition += " and " + WeighingBillEnum.send_out_time.ToString() + " like '%" + MyHelper.DateTimeHelper.getCurrentDateTime(MyHelper.DateTimeHelper.DateFormat) + "%'";
            }
            else
            {
                if (!string.IsNullOrEmpty(startDate))
                {
                    condition += " and " + WeighingBillEnum.send_out_time.ToString() + " >=" + Constract.valueSplit + startDate + Constract.valueSplit;
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    condition += " and " + WeighingBillEnum.send_out_time.ToString() + " <=" + Constract.valueSplit + endDate + Constract.valueSplit;
                }
            }
            String sql = MyHelper.DbBaseHelper.getSelectSql(DataTabeName.weighing_bill.ToString(), null, condition);
            List<WeighingBill> list = MyHelper.DbBaseHelper.DataTableToEntity<WeighingBill>(DatabaseOPtionHelper.GetInstance().select(sql));
            return list;
        }

    }
}
