﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyHelper;
using System.Printing;
using System.Threading;

namespace IntentConnectWeighing
{
    /// <summary>
    /// WeihgingBillDetailW.xaml 的交互逻辑
    ///  WeihgingBillDetailW.xaml's interactive logical 
    /// </summary>
    public partial class OutputBuLuW : WeighingWindow
    {

        #region Variable              

        #endregion
        public OutputBuLuW()
        {
            InitializeComponent();
            mWeighingBill = new WeighingBill()
            {
                id = Guid.NewGuid().ToString(),
                type = (int)WeightingBillType.CK,
                sendNumber = CommonFunction.GetWeighingNumber(WeightingBillType.CK, true, "BL"),
                affiliatedCompanyId = App.currentCompany.id,
                sendStatus= 1,
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //将本机使用的基础数据设置默认数据源 下四个方法
            SetSupplyCompanyDefaultSource();
            SetCustomerCompanyDefaultSource();
            SetMaterialDefaultSource();
            SetCarDefaultSource();

            SetRemarkDefaultSource(this.RemarkCbox);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BuildCurrWeighingBill();
            SetWeihinger();
        }

        #region Weihinger
        private void SetWeihinger()
        {
            this.WeihingerTbox.Text = App.currentUser.name;
            mWeighingBill.sendUserName = App.currentUser.name;
            mWeighingBill.sendUserId = App.currentUser.id;
        }
        #endregion
        /// <summary>
        /// 构建当前的磅单
        /// </summary>
        private void BuildCurrWeighingBill()
        {
               
            this.BillNumberTb.Text = mWeighingBill.sendNumber;
        }

        #region Window Default Event
        /// <summary>
        /// window move event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headerBorder_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isOptionSuccess == true)
            {
                if (this.RefershParent != null)
                {
                    RefershParent(true,true,true);
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        #endregion

        #region 将本机使用的基础数据设置默认数据源 最新使用的排在最前面

        private void SetSupplyCompanyDefaultSource()
        {
            if (App.tempSupplyCompanys.Count <= 0)
            {
                String path = System.IO.Path.Combine(Constract.tempPath, Constract.tempSupplyFileName);
                String xml = String.Empty;
                if (FileHelper.Exists(path))
                {
                    xml = FileHelper.Reader(path, Encoding.UTF8);
                    if (String.IsNullOrEmpty(xml))
                    {
                        this.SupplyCb.ItemsSource = null;
                        return;
                    }

                    if (App.tempSupplyCompanys.Count <= 0)
                    {
                        List<Company> list = (List<Company>)XmlHelper.Deserialize(typeof(List<Company>), xml);
                        foreach (Company cp in list)
                        {
                            App.tempSupplyCompanys.Add(cp.id, cp);
                        }
                        App.tempSupplyCompanys = App.tempSupplyCompanys.OrderByDescending(O => O.Value.syncTime).ToDictionary(p => p.Key, O => O.Value);
                    }
                    this.SupplyCb.ItemsSource = App.tempSupplyCompanys.Values.ToList();
                }
                else
                {
                    FileHelper.CreateFile(path);
                }
            }
        }
        private void SetCustomerCompanyDefaultSource()
        {
            if (App.tempCustomerCompanys.Count <= 0)
            {
                String path = System.IO.Path.Combine(Constract.tempPath, Constract.tempCustomerFileName);
                String xml = String.Empty;
                if (FileHelper.Exists(path))
                {
                    xml = FileHelper.Reader(path, Encoding.UTF8);
                    if (String.IsNullOrEmpty(xml))
                    {
                        this.ReceiverCompanyCb.ItemsSource = null;
                        return;
                    }
                    if (App.tempCustomerCompanys.Count <= 0)
                    {
                        List<Company> list = (List<Company>)XmlHelper.Deserialize(typeof(List<Company>), xml);
                        foreach (Company cp in list)
                        {
                            App.tempCustomerCompanys.Add(cp.id, cp);
                        }
                        App.tempCustomerCompanys = App.tempCustomerCompanys.OrderByDescending(O => O.Value.syncTime).ToDictionary(p => p.Key, O => O.Value);
                    }
                    this.ReceiverCompanyCb.ItemsSource = App.tempCustomerCompanys.Values.ToList();
                }
                else
                {
                    FileHelper.CreateFile(path);
                }
            }
        }
        private void SetMaterialDefaultSource()
        {
            if (App.tempMaterials.Count <= 0)
            {
                String path = System.IO.Path.Combine(Constract.tempPath, Constract.tempMatreialFileName);
                String xml = String.Empty;
                if (FileHelper.Exists(path))
                {
                    xml = FileHelper.Reader(path, Encoding.UTF8);
                    if (String.IsNullOrEmpty(xml))
                    {
                        this.MaterialNameCb.ItemsSource = null;
                        return;
                    }

                    if (App.tempMaterials.Count <= 0)
                    {
                        List<Material> list = (List<Material>)XmlHelper.Deserialize(typeof(List<Material>), xml);
                        foreach (Material material in list)
                        {
                            App.tempMaterials.Add(material.id, material);
                        }
                        App.tempMaterials = App.tempMaterials.OrderByDescending(O => O.Value.syncTime).ToDictionary(p => p.Key, O => O.Value);
                    }
                    this.MaterialNameCb.ItemsSource = App.tempMaterials.Values.ToList();
                }
                else
                {
                    FileHelper.CreateFile(path);
                }
            }
        }
        private void SetCarDefaultSource()
        {

            if (App.tempCars.Count <= 0)
            {
                String path = System.IO.Path.Combine(Constract.tempPath, Constract.tempCarFileName);
                String xml = String.Empty;
                if (FileHelper.Exists(path))
                {
                    xml = FileHelper.Reader(path, Encoding.UTF8);
                    if (String.IsNullOrEmpty(xml))
                    {
                        this.CarNumberCb.ItemsSource = null;
                        return;
                    }

                    if (App.tempCars.Count <= 0)
                    {
                        List<CarInfo> list = (List<CarInfo>)XmlHelper.Deserialize(typeof(List<CarInfo>), xml);
                        foreach (CarInfo car in list)
                        {
                            App.tempCars.Add(car.id, car);
                        }
                        App.tempCars = App.tempCars.OrderByDescending(O => O.Value.syncTime).ToDictionary(p => p.Key, O => O.Value);
                    }
                    this.CarNumberCb.ItemsSource = App.tempCars.Values.ToList();
                }
                else
                {
                    FileHelper.CreateFile(path);
                }
            }
        }

        #endregion

        #region Supply 
        private bool isSupplySelected = false;
        private void SupplyCb_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = this.SupplyCb.Text.Trim();
            if (text.Length < 2)
            {
                isSupplySelected = false;
                this.SupplyCb.IsDropDownOpen = false;
            }
            if (isSupplySelected == true)
            {
                return;
            }
            if (text.Length >= 2)
            {
                List<Company> list = CompanyModel.IndistinctSearchByNameOrNameFirstCase(text);
                if (list.Count > 0)
                {
                    isSupplySelected = false;
                }
                this.tempSupplyCompanys.Clear();
                foreach (var item in list)
                {
                    this.tempSupplyCompanys.Add(item.id, item);
                }
                this.SupplyCb.ItemsSource = this.tempSupplyCompanys.Values.ToList();
                if (SupplyCb.ItemsSource != null)
                {
                    this.SupplyCb.IsDropDownOpen = true;
                }
            }
            else
            {
                this.SupplyCb.ItemsSource = App.tempSupplyCompanys.Values.ToList();
                this.SendYardCb.ItemsSource = null;
            }
        }

        private void SupplyCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Company company = null;
            if (SupplyCb.SelectedIndex != -1)
            {
                isSupplySelected = true;
                company = (Company)SupplyCb.SelectedItem;
            }

            if (sendCompany != company && company != null)
            {
                sendCompany = company;
                if (checkSupplyCustomer() == false)
                {
                    MessageBox.Show("发货公司和收货公司不能是同一个！");
                    //this.SupplyCb.Text = null;
                    this.SupplyCb.SelectedIndex = -1;
                    sendCompany = null;
                    return;
                }
                mWeighingBill.sendCompanyId = sendCompany.id;
                mWeighingBill.sendCompanyName = sendCompany.name;
                this.SendYardCb.ItemsSource = null;
                ShowSendYard();
            }
        }

        #endregion

        #region Receiver Company
        private bool isSelectReceiveCompany = false;
        private void ReceiverCompanyCb_TextChanged(object sender, TextChangedEventArgs e)
        {

            String text = this.ReceiverCompanyCb.Text.Trim();
            if (text.Length < 2)
            {
                isSelectReceiveCompany = false;
                this.ReceiverCompanyCb.IsDropDownOpen = false;
            }
            if (isSelectReceiveCompany == true) { return; }
            if (text.Length >= 2)
            {
                List<Company> list = CompanyModel.IndistinctSearchByNameOrNameFirstCase(text);
                if (list.Count > 0)
                {
                    isSelectReceiveCompany = true;
                }
                this.tempCustomerCompanys.Clear();
                foreach (var item in list)
                {
                    this.tempCustomerCompanys.Add(item.id, item);
                }
                this.ReceiverCompanyCb.ItemsSource = this.tempCustomerCompanys.Values.ToList();
                if (this.ReceiverCompanyCb.ItemsSource != null)
                {
                    this.ReceiverCompanyCb.IsDropDownOpen = true;
                }
                else
                {
                    this.ReceiverCompanyCb.IsDropDownOpen = false;
                }
            }
            else
            {
                this.ReceiverCompanyCb.ItemsSource = App.tempCustomerCompanys.Values.ToList();
                this.ReceiverYardCb.ItemsSource = null;
            }
        }

        private void ReceiverCompanyCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Company company = null;
            if (ReceiverCompanyCb.SelectedIndex != -1)
            {
                company = (Company)ReceiverCompanyCb.SelectedItem;
            }

            if (receiverCompany != company && company != null)
            {
                receiverCompany = company;
                if (checkSupplyCustomer() == false)
                {
                    MessageBox.Show("发货公司和收货公司不能是同一个！");
                    //this.ReceiverCompanyCb.Text = null;
                    this.ReceiverCompanyCb.SelectedIndex = -1;
                    receiverCompany = null;
                    return;
                }
                mWeighingBill.receiveCompanyId = receiverCompany.id;
                mWeighingBill.receiveCompanyName = receiverCompany.name;
                company = null;
                this.ReceiverYardCb.ItemsSource = null;
                ShowReceivedYard();
            }
        }
        /// <summary>
        /// 供应商和客户是不能是同一个公司
        /// </summary>
        /// <returns></returns>
        private bool checkSupplyCustomer()
        {
            if (receiverCompany != null && sendCompany != null)
            {
                if (receiverCompany.id == sendCompany.id)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Material 
        private bool isSelectMaterial = false;
        private void MaterialNameCb_TextChanged(object sender, TextChangedEventArgs e)
        {

            String text = this.MaterialNameCb.Text.Trim();
            if (text.Length < 2)
            {
                isSelectMaterial = false;
                this.MaterialNameCb.IsDropDownOpen = false;
            }
            if (isSelectMaterial == true) { return; }
            if (text.Length >= 2)
            {
                List<Material> list = MaterialModel.IndistictSearchByNameORFirstCase(text);
                if (list.Count > 0)
                {
                    isSelectMaterial = true;
                }
                this.tempMaterials.Clear();
                foreach (var item in list)
                {
                    this.tempMaterials.Add(item.id, item);
                }
                this.MaterialNameCb.ItemsSource = this.tempMaterials.Values.ToList();
                if (this.MaterialNameCb.ItemsSource != null)
                {
                    this.MaterialNameCb.IsDropDownOpen = true;
                }
                else
                {
                    this.MaterialNameCb.IsDropDownOpen = false;
                }
            }
            else
            {
                this.MaterialNameCb.ItemsSource = App.tempMaterials.Values.ToList();
            }
        }

        private void MaterialNameCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MaterialNameCb.SelectedIndex != -1)
            {
                material = (Material)MaterialNameCb.SelectedItem;
                mWeighingBill.sendMaterialId = material.id;
                mWeighingBill.sendMaterialName = material.name;
            }
        }
        #endregion

        #region CarInfo Selection
        private bool isCarInfoSelectioned = false;
        private void CarNumberCb_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = this.CarNumberCb.Text.Trim();
            if (text.Length <= 0)
            {
                isCarInfoSelectioned = false;
                car = null;
                this.CarNumberCb.IsDropDownOpen = false;
                SetCarInfo();
                return;
            }
            if (isCarInfoSelectioned == true) { return; }

            if (text.Length >= 2)
            {
                List<CarInfo> list = CarInfoModel.FuzzySearch(text);
                if (list.Count > 0)
                {
                    isCarInfoSelectioned = true;
                }
                this.tempCars.Clear();
                foreach (var item in list)
                {
                    this.tempCars.Add(item.id, item);
                }
                this.CarNumberCb.ItemsSource = this.tempCars.Values.ToList();
                if (CarNumberCb.ItemsSource != null)
                {
                    this.CarNumberCb.IsDropDownOpen = true;
                }
            }
            else
            {
                this.CarNumberCb.ItemsSource = App.tempCars.Values.ToList();
            }
        }

        private void CarNumberCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CarNumberCb.SelectedIndex != -1)
            {
                car = (CarInfo)CarNumberCb.SelectedItem;
                isCarInfoSelectioned = true;
                mWeighingBill.plateNumber = car.carNumber;
                mWeighingBill.carId = car.id;
                mWeighingBill.driver = car.driver;
                mWeighingBill.driverIdNumber = car.driverIdnumber;
                mWeighingBill.driverMobile = car.driverMobile;

                SetCarInfo();
            }
        }

        private void SetCarInfo()
        {
            if (car != null)
            {
                this.DriverTbox.Text = car.driver;
                this.PhoneTbox.Text = car.driverMobile;
            }
            else
            {
                this.DriverTbox.Text = string.Empty;
                this.PhoneTbox.Text = string.Empty;
            }
        }
        #endregion

        #region SendYard

        private bool isShowSendYard = false;
        /// <summary>
        ///  获取和显示发货的货场
        /// </summary>
        private void ShowSendYard()
        {
            List<Yard> list = YardModel.GetListByCompanyId(sendCompany.id);
            if (list.Count > 0)
            {
                this.SendYardCb.ItemsSource = list;
                this.SendYardCb.SelectedIndex = 0;
            }
        }

        private void SendYardCb_DropDownOpened(object sender, EventArgs e)
        {
            isShowSendYard = true;
            if (this.SendYardCb.ItemsSource == null)
            {
                MessageBoxResult result = MessageBox.Show($"您还没有设置货场名称", "提示", MessageBoxButton.YesNo, MessageBoxImage.Hand);
                if (result == MessageBoxResult.Yes)
                {
                    new YardAddW(sendCompany) { ParentRefreshData = new Action<Yard>(AffterAddYard) }.ShowDialog();
                }
            }
        }

        private void SendYardCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SendYardCb.SelectedIndex > -1)
            {
                Yard yard = (Yard)this.SendYardCb.SelectedItem;
                if (yard != null)
                {
                    mWeighingBill.sendYardId = yard.id;
                    mWeighingBill.sendYardName = yard.name;
                }
                else
                {
                    mWeighingBill.sendYardId = String.Empty;
                    mWeighingBill.sendYardName = String.Empty;
                }
            }
            else
            {
                mWeighingBill.sendYardId = String.Empty;
                mWeighingBill.sendYardName = String.Empty;
            }

        }
        private void AffterAddYard(Yard yard)
        {
            if (yard != null)
            {
                if (isShowSendYard == true)
                {
                    this.SendYardCb.ItemsSource = null;
                    this.SendYardCb.ItemsSource = new List<Yard>() { yard };
                    this.SendYardCb.SelectedItem = 0;
                }
                else
                {
                    this.ReceiverYardCb.ItemsSource = null;
                    this.ReceiverYardCb.ItemsSource = new List<Yard>() { yard };
                    this.ReceiverYardCb.SelectedItem = 0;
                }
            }
        }
        #endregion

        #region Received Yard
        private void ShowReceivedYard()
        {
            List<Yard> list = YardModel.GetListByCompanyId(receiverCompany.id);
            if (list.Count > 0)
            {
                this.ReceiverYardCb.ItemsSource = list;
                this.ReceiverYardCb.SelectedIndex = 0;
            }
        }

        private void ReceiverYardCb_DropDownOpened(object sender, EventArgs e)
        {
            isShowSendYard = false;
            if (this.ReceiverYardCb.ItemsSource == null)
            {
                MessageBoxResult result = MessageBox.Show($"您还没有设置收货货场名称", "提示", MessageBoxButton.YesNo, MessageBoxImage.Hand);
                if (result == MessageBoxResult.Yes)
                {
                    new YardAddW(receiverCompany) { ParentRefreshData = new Action<Yard>(AffterAddYard) }.ShowDialog();
                }
            }
        }
        private void ReceiverYardCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ReceiverYardCb.SelectedIndex > -1)
            {
                Yard yard = (Yard)this.ReceiverYardCb.SelectedItem;
                if (yard != null)
                {
                    mWeighingBill.receiveYardName = yard.name;
                    mWeighingBill.receiveYardId = yard.id;
                }
                else
                {
                    mWeighingBill.receiveYardName = String.Empty;
                    mWeighingBill.receiveYardId = String.Empty;
                }
            }
            else
            {
                mWeighingBill.receiveYardName = String.Empty;
                mWeighingBill.receiveYardId = String.Empty;
            }
        }
        #endregion

        #region Send weight value change event
        private void SendTraeWeightTbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcReceiptWeightValue();
        }

        private void SendGrossWeightTbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcReceiptWeightValue();
        }

        /// <summary>
        /// 计算收货吨位  calculating the tonnage of receipt
        /// </summary>
        private void CalcReceiptWeightValue()
        {
            if (this.IsLoaded == false) return;
            double gross = 0;
            double trae = 0;
            double net = 0;
            try { gross = Convert.ToDouble(this.SendGrossWeightTbox.Text); } catch { }
            try { trae = Convert.ToDouble(this.SendTraeWeightTbox.Text); } catch { }
            net = System.Math.Round(gross - trae, 2);
            this.SendNetWeightTbox.Text = net.ToString();

            mWeighingBill.sendGrossWeight = gross;
            mWeighingBill.sendTraeWeight = trae;
            mWeighingBill.sendNetWeight = net;
        }

        #endregion

        private void ReceivedRemardTbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded == false) { return; }
            mWeighingBill.sendRemark = this.RemarkCbox.Text;
        }
        #region save
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            //使用线程更新 备注
            UpdateRemark();
            if (CheckInput() == true)
            {
                Insert();
            }
        }
        //insert
        private void Insert()
        {
            mWeighingBill.syncTime = DateTimeHelper.GetTimeStamp();
            int res = DatabaseOPtionHelper.GetInstance().insert(mWeighingBill);
            if (res > 0)
            {
                isOptionSuccess = true;
                MessageBoxResult result = MessageBox.Show("保存成功", "恭喜");                
                UpdateUsedBaseData();       
                PrintBill(WeightingBillType.CK);
                this.Close();
            }
        }


        private bool CheckInput()
        {
            if (string.IsNullOrEmpty(mWeighingBill.sendCompanyId) || string.IsNullOrEmpty(mWeighingBill.sendCompanyId))
            {
                MessageBox.Show("请选择供应商信息");
                this.SupplyCb.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(mWeighingBill.sendYardId) || string.IsNullOrEmpty(mWeighingBill.sendYardName))
            {
                MessageBox.Show("请选择发货货场");
                this.SendYardCb.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(mWeighingBill.receiveCompanyId) || string.IsNullOrEmpty(mWeighingBill.receiveCompanyName))
            {
                MessageBox.Show("请选择收货公司");
                this.ReceiverCompanyCb.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(mWeighingBill.receiveYardId) || string.IsNullOrEmpty(mWeighingBill.receiveYardName))
            {
                MessageBox.Show("请选择收货场");
                this.ReceiverYardCb.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(mWeighingBill.sendMaterialId) || string.IsNullOrEmpty(mWeighingBill.sendMaterialName))
            {
                MessageBox.Show("请选择物资名称");
                this.MaterialNameCb.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(mWeighingBill.carId) || string.IsNullOrEmpty(mWeighingBill.plateNumber))
            {
                MessageBox.Show("请输入车辆信息");
                this.CarNumberCb.Focus();
                return false;
            }
            return true;
        }

        private void UpdateRemark()
        {
            Thread thread = new Thread(new ParameterizedThreadStart(CommonFunction.UpdateOutputReamak));
            thread.Start(mWeighingBill.sendRemark);
            SetRemarkDefaultSource(this.RemarkCbox,1);
        }
        #endregion



        private void InDTP_ValueChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            if (this.IsLoaded == true)
                mWeighingBill.sendInTime = InDTP.StringValue;
        }

        private void OutDTP_ValueChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            if (this.IsLoaded == true)
                mWeighingBill.sendOutTime = OutDTP.StringValue;
        }
    }
}
