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
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), null, condition, null, null, null, 1);
            List<WeighingBill> list = DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
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
              WeighingBillEnum.receive_status.ToString() + " != 1 and " +
              WeighingBillEnum.receive_yard_id.ToString() + "=" + Constract.valueSplit + App.currentYard.id + Constract.valueSplit + " and " +
              WeighingBillEnum.receive_company_id.ToString() + "=" + Constract.valueSplit + App.currentCompany.id + Constract.valueSplit;

            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), null, conditon, null, null, WeighingBillEnum.send_out_time + " desc ", 20);
            list = DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
            return list;
        }
        /// <summary>
        /// 获取未完成入库数据 ，不传开始时间和结束时间，则是获取所有的数据
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="where">其它条件</param>
        /// <returns></returns>
        public static List<WeighingBill> GetInNoFinished(String startDate = null, String endDate = null, string where = null)
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
            if (!string.IsNullOrEmpty(where))
            {
                condition += " and " + where;
            }
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), null, condition);
            List<WeighingBill> list = DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
            return list;
        }

        /// <summary>
        /// 获取已经完成入库数据，不传开始时间和结束时间，则是获取当天的数据 
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="where">其它条件</param>
        /// <returns></returns>
        public static List<WeighingBill> GetInFinished(String startDate = null, String endDate = null, string where = null)
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
            if (!string.IsNullOrEmpty(where))
            {
                condition += " and " + where;
            }
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), null, condition);
            List<WeighingBill> list = DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
            return list;
        }

        /// <summary>
        /// 获取未完成的出库数据，不传开始时间和结束时间，则是获取所有的数据
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="where">其它条件</param>
        /// <returns></returns>
        public static List<WeighingBill> GetOutNOFinished(String startDate = null, String endDate = null, string where = null)
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
            if (!string.IsNullOrEmpty(where))
            {
                condition += " and " + where;
            }
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), null, condition);
            List<WeighingBill> list = DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
            return list;
        }
        /// <summary>
        /// 获取已经完成的出库数据，不传开始时间和结束时间，则是获取当天的数据 
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="where">其它条件</param>
        /// <returns></returns>
        public static List<WeighingBill> GetOutFinished(String startDate = null, String endDate = null, string where = null)
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
            if (!string.IsNullOrEmpty(where))
            {
                condition += " and " + where;
            }
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), null, condition);
            List<WeighingBill> list = DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
            return list;
        }

        /// <summary>
        ///  按煤种分组 获取入库车数。
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="where">其它条件</param>
        /// <returns></returns>
        public static Dictionary<String, Int32> GetInCahartDataGroungByMaterial(String startDate = null, String endDate = null, String where = null)
        {
            String condition = WeighingBillEnum.receive_status.ToString() + "=" + 1 + " and " +
                WeighingBillEnum.type.ToString() + " = " + ((int)WeightingBillType.RK) + " and " +
                WeighingBillEnum.receive_yard_id.ToString() + "=" + Constract.valueSplit + App.currentYard.id + Constract.valueSplit;
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
            if (!string.IsNullOrEmpty(where))
            {
                condition += " and " + where;
            }
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), WeighingBillEnum.receive_material_name.ToString() + ", count(" + WeighingBillEnum.id.ToString() + ") as sync_time", condition, WeighingBillEnum.receive_material_name.ToString());
            Dictionary<String, Int32> list = new Dictionary<String, Int32>() { };
           List<WeighingBill> ll= DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
            for (int i = 0; i < ll.Count; i++)
            {
                list.Add(ll[i].receiveMaterialName, Convert.ToInt32(ll[i].syncTime));
            }
            return list;
        }

        /// <summary>
        /// 按煤种分组 获取入库吨数。
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        ///  <param name="where">其它条件</param>
        /// <returns></returns>
        public static Dictionary<String, Double> GetInCahartTonDataGroungByMaterial(String startDate = null, String endDate = null, String where = null)
        {
            String condition = WeighingBillEnum.receive_status.ToString() + "=" + 1 + " and " +
            WeighingBillEnum.type.ToString() + " = " + ((int)WeightingBillType.RK) + " and " +
            WeighingBillEnum.receive_yard_id.ToString() + "=" + Constract.valueSplit + App.currentYard.id + Constract.valueSplit;
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
            if (!string.IsNullOrEmpty(where))
            {
                condition += " and " + where;
            }
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), WeighingBillEnum.receive_material_name.ToString() + ", sum(" + WeighingBillEnum.receive_net_weight.ToString() + ") as receive_net_weight", condition, WeighingBillEnum.receive_material_name.ToString());
            Dictionary<String, Double> list = new Dictionary<String, Double>() { };
           List<WeighingBill> ll= DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
            for (int i = 0; i < ll.Count; i++)
            {
                list.Add(ll[i].receiveMaterialName, Convert.ToDouble(ll[i].receiveNetWeight));
            }
            return list;
        }
        /// <summary>
        /// 按公司分组 获取入库车数。
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="where">其它条件</param>
        /// <returns></returns>
        public static Dictionary<String, Int32> GetInCahartDataGroungBySendCpmpany(String startDate = null, String endDate = null, String where = null)
        {
            String condition = WeighingBillEnum.receive_status.ToString() + "=" + 1 + " and " +
            WeighingBillEnum.type.ToString() + " = " + ((int)WeightingBillType.RK) + " and " +
            WeighingBillEnum.receive_yard_id.ToString() + "=" + Constract.valueSplit + App.currentYard.id + Constract.valueSplit;
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
            if (!string.IsNullOrEmpty(where))
            {
                condition += " and " + where;
            }
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), WeighingBillEnum.send_company_name.ToString() + ", count(" + WeighingBillEnum.id.ToString() + ")  as sync_time ", condition, WeighingBillEnum.send_company_name.ToString());
            Dictionary<String, Int32> list = new Dictionary<String, Int32>() { };
            List<WeighingBill> ll = DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
            for (int i = 0; i <ll.Count; i++)
            {
                list.Add(ll[i].sendCompanyName, Convert.ToInt32(ll[i].syncTime));
            }
            return list;
        }

        /// <summary>
        /// 按公司分组 获取入库吨数。
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="where">其它条件</param>
        /// <returns></returns>
        public static Dictionary<String, double> GetInCahartTonDataGroungBySendCpmpany(String startDate = null, String endDate = null, String where = null)
        {
            String condition = WeighingBillEnum.receive_status.ToString() + "=" + 1 + " and " +
            WeighingBillEnum.type.ToString() + " = " + ((int)WeightingBillType.RK) + " and " +
            WeighingBillEnum.receive_yard_id.ToString() + "=" + Constract.valueSplit + App.currentYard.id + Constract.valueSplit;
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
            if (!string.IsNullOrEmpty(where))
            {
                condition += " and " + where;
            }
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), WeighingBillEnum.send_company_name.ToString() + ", sum(" + WeighingBillEnum.receive_net_weight.ToString() + ")  as receive_net_weight", condition, WeighingBillEnum.send_company_name.ToString());
            Dictionary<String, double> list = new Dictionary<String, double>() { };
            List<WeighingBill> ll = DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
            for (int i = 0; i < ll.Count; i++)
            {
                list.Add(ll[i].sendCompanyName, Convert.ToDouble(ll[i].receiveNetWeight));
            }
            return list;
        }

        /// <summary>
        ///  按煤种分组 获取出库车数。
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        ///  <param name="where">其它条件</param>
        /// <returns></returns>
        public static Dictionary<String, Int32> GetOutCahartDataGroungByMaterial(String startDate = null, String endDate = null, String where = null)
        {
            String condition = WeighingBillEnum.send_status.ToString() + "=" + 1 + " and " +
                WeighingBillEnum.type.ToString() + " = " + ((int)WeightingBillType.CK) + " and " +
                WeighingBillEnum.send_yard_id.ToString() + "=" + Constract.valueSplit + App.currentYard.id + Constract.valueSplit;
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
            if (!string.IsNullOrEmpty(where))
            {
                condition += " and " + where;
            }
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), WeighingBillEnum.send_material_name.ToString() + ", count(" + WeighingBillEnum.id.ToString() + ") as sync_time", condition, WeighingBillEnum.send_material_name.ToString());
            Dictionary<String, Int32> list = new Dictionary<String, Int32>() { };
             List<WeighingBill> ll = DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
            for (int i = 0; i < ll.Count; i++)
            {
                list.Add(ll[i].sendMaterialName, Convert.ToInt32(ll[i].syncTime));
            }
            return list;
        }

        /// <summary>
        /// 按煤种分组 获取出库吨数。
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        ///  <param name="where">其它条件</param>
        /// <returns></returns>
        public static Dictionary<String, Double> GetOutCahartTonDataGroungByMaterial(String startDate = null, String endDate = null, String where = null)
        {
            String condition = WeighingBillEnum.send_status.ToString() + "=" + 1 + " and " +
            WeighingBillEnum.type.ToString() + " = " + ((int)WeightingBillType.CK) + " and " +
            WeighingBillEnum.send_yard_id.ToString() + "=" + Constract.valueSplit + App.currentYard.id + Constract.valueSplit;
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
            if (!string.IsNullOrEmpty(where))
            {
                condition += " and " + where;
            }
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), WeighingBillEnum.send_material_name.ToString() + ", sum(" + WeighingBillEnum.send_net_weight.ToString() + ") as send_net_weight", condition, WeighingBillEnum.send_material_name.ToString());
            Dictionary<String, Double> list = new Dictionary<String, Double>() { };
             List<WeighingBill> ll = DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
            for (int i = 0; i < ll.Count; i++)
            {
                list.Add(ll[i].sendMaterialName, Convert.ToDouble(ll[i].sendNetWeight));
            }
            return list;
        }
        /// <summary>
        /// 按公司分组 获取出库车数。
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="where">其它条件</param>
        /// <returns></returns>
        public static Dictionary<String, Int32> GetOutCahartDataGroungBySendCpmpany(String startDate = null, String endDate = null, String where = null)
        {
            String condition = WeighingBillEnum.send_status.ToString() + "=" + 1 + " and " +
            WeighingBillEnum.type.ToString() + " = " + ((int)WeightingBillType.CK) + " and " +
            WeighingBillEnum.send_yard_id.ToString() + "=" + Constract.valueSplit + App.currentYard.id + Constract.valueSplit;
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
            if (!string.IsNullOrEmpty(where))
            {
                condition += " and " + where;
            }
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), WeighingBillEnum.receive_company_name.ToString() + ", count(" + WeighingBillEnum.id.ToString() + ")  as sync_time ", condition, WeighingBillEnum.receive_company_name.ToString());
            Dictionary<String, Int32> list = new Dictionary<String, Int32>() { };
             List<WeighingBill> ll = DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
            for (int i = 0; i < ll.Count; i++)
            {
                list.Add(ll[i].receiveCompanyName, Convert.ToInt32(ll[i].syncTime));
            }
            return list;
        }

        /// <summary>
        /// 按公司分组 获取出库车数。
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="where">其它条件</param>
        /// <returns></returns>
        public static Dictionary<String, double> GetOutCahartTonDataGroungBySendCpmpany(String startDate = null, String endDate = null, String where = null)
        {
            String condition = WeighingBillEnum.send_status.ToString() + "=" + 1 + " and " +
            WeighingBillEnum.type.ToString() + " = " + ((int)WeightingBillType.CK) + " and " +
            WeighingBillEnum.send_yard_id.ToString() + "=" + Constract.valueSplit + App.currentYard.id + Constract.valueSplit;
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
            if (!string.IsNullOrEmpty(where))
            {
                condition += " and " + where;
            }
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), WeighingBillEnum.receive_company_name.ToString() + ", sum(" + WeighingBillEnum.send_net_weight.ToString() + ")  as send_net_weight", condition, WeighingBillEnum.receive_company_name.ToString());
            Dictionary<String, double> list = new Dictionary<String, double>() { };
             List<WeighingBill> ll = DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
            for (int i = 0; i < ll.Count; i++)
            {
                list.Add(ll[i].receiveCompanyName, Convert.ToDouble(ll[i].sendNetWeight));
            }
            return list;
        }

        /// <summary>
        /// 获取报表的明细报表的列表统计数据
        /// </summary>
        /// <param name="type">not null </param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="where">其它条件</param>
        /// <returns></returns>
        public static List<ReportWeightListV> GetListStatisticData(WeightingBillType type, String startDate = null, String endDate = null, String where = null)
        {
            String infield = @"send_company_name as company,
                                    send_yard_name as yard, 
                                    receive_material_name as material,
                                    COUNT(DISTINCT(id)) as cars,
                                    SUM(receive_gross_weight) as grossweight,
                                    SUM(receive_trae_weight) as traeweight,
                                    SUM(receive_net_weight) as weight";

            String outfield = @"receive_company_name as company,
                                        receive_yard_name as yard, 
                                        send_material_name as material,
                                        COUNT(DISTINCT(id)) as cars,
                                        SUM(send_gross_weight) as grossweight,
                                        SUM(send_trae_weight) as traeweight,
                                        SUM(send_net_weight) as weight";

            String inGroupBy = @" send_company_name,send_yard_name,receive_material_name";
            String outGroupBy = @" receive_company_name,receive_yard_name,send_material_name";
            List<ReportWeightListV> list = new List<ReportWeightListV>();
            string condition = WeighingBillEnum.type.ToString() + " = " + ((int)type); ;
            if (type == WeightingBillType.RK)
            {
                condition += " and " + WeighingBillEnum.receive_status.ToString() + "= 1 ";
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
            }
            else if (type == WeightingBillType.CK)
            {
                condition += " and " + WeighingBillEnum.send_status.ToString() + "= 1 ";
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
            }
            else
            {
                return null;
            }
            if (!string.IsNullOrEmpty(where))
            {
                condition += " and " + where;
            }
            string sql = string.Empty;
            if (type == WeightingBillType.RK)
            {
                sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), infield, condition, inGroupBy);
            }
            else if (type == WeightingBillType.CK)
            {
                sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), outfield, condition, outGroupBy);
            }
            else
            {
                return null;
            }
            list =DatabaseOPtionHelper.GetInstance().select<ReportWeightListV>(sql);
            return list;
        }
        /// <summary>
        /// 获取报表的明细报表的 汇总 统计数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="where">其它条件</param>
        /// <returns></returns>
        public static ReportWeightSummaryV GetReportWeightSummaryData(WeightingBillType type, String startDate = null, String endDate = null, String where = null)
        {
            ReportWeightSummaryV reportWeightSummaryV = null;
            String infield = @"COUNT(DISTINCT(send_company_id)) as companys,
                                        COUNT(DISTINCT(receive_material_id)) as materials,
                                        COUNT(DISTINCT(id)) as cars,
                                        SUM(send_net_weight) as sendweight,
                                        SUM(receive_gross_weight) as grossweight,
                                        SUM(receive_trae_weight) as traeweight,
                                        SUM(receive_net_weight) as netweight,
                                        SUM(decuation_weight) as decuationweight,
                                        SUM(difference_weight) as differenceweight";

            String outfield = @"COUNT(DISTINCT(receive_company_id)) as companys,
                                        COUNT(DISTINCT(Send_material_id)) as materials,
                                        COUNT(DISTINCT(id)) as cars,
                                        SUM(Send_gross_weight) as grossweight,
                                        SUM(Send_trae_weight) as traeweight,
                                        SUM(Send_net_weight) as netweight";
            string condition = WeighingBillEnum.type.ToString() + " = " + ((int)type);
            if (type == WeightingBillType.RK)
            {
                condition += " and " + WeighingBillEnum.receive_status.ToString() + "= 1 ";
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
            }
            else if (type == WeightingBillType.CK)
            {
                condition += " and " + WeighingBillEnum.send_status.ToString() + "= 1 ";
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
            }
            else
            {
                return null;
            }
            if (!string.IsNullOrEmpty(where))
            {
                condition += " and " + where;
            }
            string sql = string.Empty;
            if (type == WeightingBillType.RK)
            {
                sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), infield, condition);
            }
            else if (type == WeightingBillType.CK)
            {
                sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), outfield, condition);
            }
            else
            {
                return null;
            }
            reportWeightSummaryV = DatabaseOPtionHelper.GetInstance().select<ReportWeightSummaryV>(sql)[0];       
            return reportWeightSummaryV;
        }
        /// <summary>
        /// 删除 
        /// </summary>
        /// <param name="weighing"></param>
        /// <param name="istrue">是否真的删除</param>
        /// <returns></returns>
        public static int Delete(WeighingBill weighing,bool istrue =false) {
            int res = 0;
            if (weighing == null) {
                return 0;
            }
            if (istrue == true)
            {
                res = DatabaseOPtionHelper.GetInstance().delete(weighing, true);
            }
            else {
                weighing.isDelete = 1;
                weighing.deleteTime = MyHelper.DateTimeHelper.getCurrentDateTime();
                res = DatabaseOPtionHelper.GetInstance().update(weighing);
            }            
            return res;
        }

    }
}
