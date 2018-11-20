using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    /// <summary>
    /// CoMmon Function Library
    /// </summary>
    public class CommonFunction
    {
        /// <summary>
        /// Update Used Base Data run in the app exiting
        /// </summary>
        public static void UpdateUsedBaseData()
        {
            if (App.tempSupplyCompanys != null)
            {
                String xml = MyHelper.XmlHelper.Serialize(typeof(List<Company>), App.tempSupplyCompanys.Values.ToList());
                MyHelper.FileHelper.Write(System.IO.Path.Combine(Constract.tempPath, Constract.tempSupplyFileName), xml);
            }
            if (App.tempCustomerCompanys != null)
            {
                String xml = MyHelper.XmlHelper.Serialize(typeof(List<Company>), App.tempCustomerCompanys.Values.ToList());
                MyHelper.FileHelper.Write(System.IO.Path.Combine(Constract.tempPath, Constract.tempCustomerFileName), xml);
            }

            if (App.tempMaterials != null)
            {
                String xml = MyHelper.XmlHelper.Serialize(typeof(List<Material>), App.tempMaterials.Values.ToList());
                MyHelper.FileHelper.Write(System.IO.Path.Combine(Constract.tempPath, Constract.tempMatreialFileName), xml);
            }

            if (App.tempCars != null)
            {
                String xml = MyHelper.XmlHelper.Serialize(typeof(List<CarInfo>), App.tempCars.Values.ToList());
                MyHelper.FileHelper.Write(System.IO.Path.Combine(Constract.tempPath, Constract.tempCarFileName), xml);
            }
        }

        internal static void UpdateDecuationList(object des)
        {
            if (des != null && !String.IsNullOrEmpty(des.ToString()))
            {
                if (!App.decuationDesList.Contains(des.ToString()))
                {
                    App.decuationDesList.Add(des.ToString());
                }
            }
        }

        internal static void UpdateInputReamak(object remark) {
            if (remark != null && !String.IsNullOrEmpty(remark.ToString())) {
                if (!App.inputRemarkList.Contains(remark.ToString()))
                {
                    App.inputRemarkList.Add(remark.ToString());
                }
            }
        }

        internal static void UpdateOutputReamak(object remark)
        {
            if (remark != null && !String.IsNullOrEmpty(remark.ToString()))
            {
                if (!App.outputRemarkList.Contains(remark.ToString()))
                {
                    App.outputRemarkList.Add(remark.ToString());
                }
            }
        }

        /// <summary>
        /// Update Used Base Data run in the save success
        /// </summary>
        public static void TempUpdateUsedBase(object baseData)
        {
            BaseDataClassV baseDataClassV = (BaseDataClassV)baseData;
            Company supply = baseDataClassV.send;
            Company Customer= baseDataClassV.receive;
            Material material = baseDataClassV.material;
            CarInfo carInfo = baseDataClassV.carInfo;
            if (supply != null)
            {
                supply.syncTime = MyHelper.DateTimeHelper.GetTimeStamp();
                if (App.tempSupplyCompanys.ContainsKey(supply.id))
                {
                    App.tempSupplyCompanys.Remove(supply.id);
                }
                App.tempSupplyCompanys.Add(supply.id, supply);
                App.tempSupplyCompanys.OrderBy(O => O.Value.syncTime);
            }
            if (Customer != null)
            {
                Customer.syncTime = MyHelper.DateTimeHelper.GetTimeStamp();
                if (App.tempCustomerCompanys.ContainsKey(Customer.id))
                {
                    App.tempCustomerCompanys.Remove(Customer.id);
                }
                App.tempCustomerCompanys.Add(Customer.id, Customer);
                App.tempCustomerCompanys.OrderBy(O => O.Value.syncTime);
            }
            if (material != null)
            {
                material.syncTime = MyHelper.DateTimeHelper.GetTimeStamp();
                if (App.tempMaterials.ContainsKey(material.id))
                {
                    App.tempMaterials.Remove(material.id);
                }
                App.tempMaterials.Add(material.id, material);
                App.tempMaterials.OrderBy(O => O.Value.syncTime);
            }
            if (carInfo != null)
            {
                carInfo.syncTime = MyHelper.DateTimeHelper.GetTimeStamp();
                if (App.tempCars.ContainsKey(carInfo.id))
                {
                    App.tempCars.Remove(carInfo.id);
                }
                App.tempCars.Add(carInfo.id, carInfo);
                App.tempCars.OrderBy(O => O.Value.syncTime);
            }
        }

        /// <summary>
        /// Get Weighing Number 获取磅单
        /// </summary>
        /// <param name="WeightingBillType"> CK OR RK</param>
        /// <param name="ExtStr"> BL OR WL</param>
        /// <param name="noaml">true 正常过磅 false WeightingBillType + CurrentDateTime</param>
        /// <returns></returns>
        public static String GetWeighingNumber(WeightingBillType type, bool noaml = true,String ExtStr = null)
        {
            if (noaml == false)
            {
                return type.ToString() + MyHelper.DateTimeHelper.getCurrentDateTime(MyHelper.DateTimeHelper.BillNumberDateTimeFormat);
            }
            String header = string.Empty;
            if (App.currentCompany.customerType == (int)CompanyCustomerTyle.Person)
            {
                if (String.IsNullOrEmpty(App.currentCompany.nameFirstCase))
                {
                    header = MyHelper.StringHelper.GetFirstPinyin(App.currentCompany.name);
                }
                else
                {
                    header = App.currentCompany.nameFirstCase;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(App.currentCompany.abbreviationFirstCase))
                {
                    if (!String.IsNullOrEmpty(App.currentCompany.abbreviation))
                    {
                        header = MyHelper.StringHelper.GetFirstPinyin(App.currentCompany.abbreviation);
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(App.currentCompany.nameFirstCase))
                        {
                            header = MyHelper.StringHelper.GetFirstPinyin(App.currentCompany.name);
                        }
                        else
                        {
                            header = App.currentCompany.nameFirstCase;
                        }
                    }

                }
                else
                {
                    header = App.currentCompany.abbreviationFirstCase;
                }
            }
            String dateStr = MyHelper.DateTimeHelper.getCurrentDateTime(MyHelper.DateTimeHelper.BillNumberDateTimeFormat);

            string sort = string.Empty;
            int count = CommonModel.GetTodayCount(type);
            switch (count.ToString().Length)
            {
                case 1:
                    sort = "000" + (count + 1).ToString();
                    break;
                case 2:
                    sort = "00" + (count + 1).ToString();
                    break;
                case 3:
                    sort = "0" + (count + 1).ToString();
                    break;
                default:
                    sort = (count + 1).ToString();
                    break;
            }
            if (String.IsNullOrEmpty(ExtStr))
            {
                return type.ToString().ToUpper() + "-" + header + dateStr + sort;
            }
            else {
                return type.ToString().ToUpper() + "-" + ExtStr.ToUpper() +"-" + header + dateStr + sort;
            }
           
        }
        
        public static void MargeToSend(ref WeighingBill send, WeighingBill receiver)
        {
            send.relativeBillId = receiver.id;
            send.receiveUserId = receiver.sendUserId;
            send.receiveUserName = receiver.sendUserName;
            send.receiveInTime = receiver.sendInTime;
            send.receiveOutTime = receiver.sendOutTime;
            send.receiveMaterialId = receiver.sendMaterialId;
            send.receiveMaterialName = receiver.sendMaterialName;
            send.receiveNumber = receiver.sendNumber;
            send.receiveRemark = receiver.sendRemark;
            send.receiveCompanyId = receiver.sendCompanyId;
            send.receiveCompanyName = receiver.sendCompanyName;
            send.receiveGrossWeight = receiver.sendGrossWeight;
            send.receiveTraeWeight = receiver.sendTraeWeight;
            send.receiveNetWeight = receiver.sendNetWeight;
            send.decuationWeight = receiver.decuationWeight;
            send.decuationDescription = receiver.decuationDescription;
            send.differenceWeight = receiver.differenceWeight;
        }

        public static void MargeToReciver(ref WeighingBill receiver, WeighingBill send)
        {
            receiver.relativeBillId = send.id;
            receiver.sendUserId = send.sendUserId;
            receiver.sendUserName = send.sendUserName;
            receiver.sendInTime = send.sendInTime;
            receiver.sendOutTime = send.sendOutTime;
            receiver.sendMaterialId = send.sendMaterialId;
            receiver.sendMaterialName = send.sendMaterialName;
            receiver.sendNumber = send.sendNumber;
            receiver.sendRemark = send.sendRemark;
            receiver.sendCompanyId = send.sendCompanyId;
            receiver.sendCompanyName = send.sendCompanyName;
            receiver.sendYardId = send.sendYardId;
            receiver.sendYardName = send.sendYardName;
            receiver.receiveCompanyId = send.receiveCompanyId;
            receiver.receiveCompanyName = send.receiveCompanyName;
            receiver.receiveYardId = send.receiveYardId;
            receiver.receiveYardName = send.receiveYardName;
            receiver.sendGrossWeight = send.sendGrossWeight;
            receiver.sendTraeWeight = send.sendTraeWeight;
            receiver.carId = send.carId;
            receiver.sendNetWeight = send.sendNetWeight;
            receiver.plateNumber = send.plateNumber;
            receiver.driver = send.driver;
            receiver.driverMobile = send.driverMobile;
            receiver.driverIdNumber = send.driverIdNumber;
        }

        /// <summary>
        /// 将派车单的信息同步到当前的磅单
        /// </summary>
        /// <param name="send">派车单</param>
        /// <param name="weighingBill">磅单</param>
        public static void SendCardMargeToWeighing(SendCarBill send, ref WeighingBill weighingBill) {
            weighingBill.sendCarBillId = send.id;
            weighingBill.sendCarBillNumber = send.number;
            weighingBill.sendCarUser = send.userName;
            weighingBill.sendCompanyId = send.sendCompanyId;
            weighingBill.sendCompanyName = send.sendCompanyName;
            weighingBill.sendYardId = send.sendYardId;
            weighingBill.sendYardName = send.sendYardName;
            weighingBill.receiveCompanyId = send.receiveCompanyId;
            weighingBill.receiveCompanyName = send.receiveCompanyName;
            weighingBill.receiveYardId = send.receiveYardId;
            weighingBill.receiveYardName = send.receiveYardName;
            weighingBill.sendMaterialId = send.materialId;
            weighingBill.sendMaterialName = send.materialName;
            weighingBill.carId = send.carId;
            weighingBill.plateNumber = send.plateNumber;
            weighingBill.driver = send.driver;
            weighingBill.driverMobile = send.driverMobile;
            weighingBill.receiveRemark = weighingBill.receiveRemark +" "+ send.remark;
        }

        /// <summary>
        /// 将当前的磅单的信息同步到派车单
        /// </summary>
        /// <param name="weighingBill">磅单</param>
        /// <param name="sendCarBill">派车单</param>
        public static void WeighingMargeToSendCarrBill(WeighingBill weighingBill,ref SendCarBill sendCarBill)
        {
            sendCarBill.inFactoryTime = weighingBill.sendInTime;
            sendCarBill.outFactoryTime = weighingBill.sendOutTime;
            sendCarBill.weighingNumber = weighingBill.sendNumber;
            sendCarBill.weighingBillId = weighingBill.id;
            sendCarBill.syncTime = MyHelper.DateTimeHelper.GetTimeStamp();
        }

        /// <summary>
        /// 设置当前显示控制的解释器
        /// </summary>
        public static ScaleDataInterpreter SetInterpreter(int brandType)
        {
            ScaleDataInterpreter formarter;
            switch (brandType)
            {
                case (int)ScaleBrandType.YH:
                    formarter = new YaoHuanDataInterpreter();
                    break;
                case (int)ScaleBrandType.LBKL:
                    formarter = new   LBKLDataInterpreter();
                    break;
                case (int)ScaleBrandType.TLD:
                    formarter = new   TLDDataInterpreter();
                    break;
                case (int)ScaleBrandType.SDLS:
                    formarter = new SDLSDataInterpreter();
                    break;
                default:
                    formarter = new NotSuporInterprete();
                    break;
            }
            return formarter;
        }
    }
}
