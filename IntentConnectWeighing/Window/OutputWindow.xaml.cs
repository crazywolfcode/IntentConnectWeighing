using IntentConnectWeighing.CameraSdk;
using MyHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using MyCustomControlLibrary;
namespace IntentConnectWeighing
{
    /// <summary>
    /// InputWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OutputWindow : WeighingWindow
    {
      
        #region Variable area       
        private SendCarBill mSendCarBill;
        private bool IsHasSendCarBill = false; 
        #endregion
        public OutputWindow(object bill = null, bool isSend = false)
        {
            InitializeComponent();                      
            if (isSend == true)
            {
                mSendCarBill = (SendCarBill)bill;
                IsHasSendCarBill = true;
            }
            else
            {
                 mWeighingBill = (WeighingBill)bill;
                if (mWeighingBill != null)
                {
                    isInsert = false;
                    isOutFactory = true;
                    this.OutFactoryRb.IsChecked = true;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //将本机使用的基础数据设置默认数据源 下5个方法
            SetSupplyCompanyDefaultSource(this.SupplyCb);
            SetCustomerCompanyDefaultSource(this.ReceiverCompanyCb);
            SetMaterialDefaultSource(this.MaterialNameCb);
            SetCarDefaultSource(this.CarNumberCb);
            SetRemarkDefaultSource(SendRemardCombox, 1);

            BuildCurrWeighingBill();
        }

        /// <summary>
        /// 构建当前的磅单
        /// </summary>
        private void BuildCurrWeighingBill()
        {
            if (mWeighingBill == null)
            {
                // in factoty
                isOutFactory = false;
                currBillNumber = CommonFunction.GetWeighingNumber(WeightingBillType.CK);
                this.BillNumberTb.Text = currBillNumber;

                mWeighingBill = new WeighingBill()
                {
                    id = Guid.NewGuid().ToString(),
                    type = (int)WeightingBillType.CK,
                    sendUserId = App.currentUser.id,
                    sendUserName = App.currentUser.name,
                    sendNumber = currBillNumber,
                    affiliatedCompanyId = App.currentCompany.id,
                    sendStatus = 0,
                };
                #region 恢复控件状态
                this.SupplyCb.Text = string.Empty;
                this.ReceiverCompanyCb.Text = string.Empty;
                this.CarNumberCb.Text = string.Empty;
                this.DriverTbox.Text = string.Empty;
                this.PhoneTbox.Text = string.Empty;
                this.SendYardCb.Text = string.Empty;
                this.ReceiverYardCb.Text = string.Empty;
                this.MaterialNameCb.Text = string.Empty;

                this.SendGrossWeightTbox.Text = "0";
                this.SendTraeWeightTbox.Text = "0";
                this.SendNetWeightTbox.Text = "0";

                this.MaterialNameCb.IsEnabled = true;
                this.SupplyCb.IsEnabled = true;
                this.ReceiverCompanyCb.IsEnabled = true;
                this.CarNumberCb.IsEnabled = true;
                this.SendYardCb.IsEnabled = true;
                this.ReceiverYardCb.IsEnabled = true;

                #endregion
                if (mSendCarBill != null)
                {
                    //有派车单 
                    CommonFunction.SendCardMargeToWeighing(mSendCarBill, ref mWeighingBill);

                    #region 将派车信息显示到控件上面 
                    this.SupplyCb.Text = mWeighingBill.sendCompanyName;
                    this.SendYardCb.Text = mWeighingBill.sendYardName;
                    this.CarNumberCb.Text = mWeighingBill.plateNumber;
                    this.DriverTbox.Text = mWeighingBill.driver;
                    this.PhoneTbox.Text = mWeighingBill.driverMobile;
                    this.ReceiverCompanyCb.Text = mWeighingBill.receiveCompanyName;
                    this.ReceiverYardCb.Text = mWeighingBill.receiveYardName;
                    this.MaterialNameCb.Text = mWeighingBill.sendMaterialName;
                    this.SendRemardCombox.Text = mWeighingBill.receiveRemark;

                    this.MaterialNameCb.IsEnabled = false;
                    this.CarNumberCb.IsEnabled = false;
                    this.SupplyCb.IsEnabled = false;
                    this.SendYardCb.IsEnabled = false;
                    #endregion                   
                }
            }
            else
            {
                //out factoty
                isOutFactory = true;
                mWeighingBill.sendStatus = 1;
                #region 设置控件状态
                this.BillNumberTb.Text = mWeighingBill.sendNumber;
                this.SupplyCb.Text = mWeighingBill.sendCompanyName;
                this.ReceiverCompanyCb.Text = mWeighingBill.receiveCompanyName;
                this.CarNumberCb.Text = mWeighingBill.plateNumber;
                this.DriverTbox.Text = mWeighingBill.driver;
                this.PhoneTbox.Text = mWeighingBill.driverMobile;
                this.SendYardCb.Text = mWeighingBill.sendYardName;
                this.ReceiverYardCb.Text = mWeighingBill.receiveYardName;
                this.MaterialNameCb.Text = mWeighingBill.sendMaterialName;

                this.SendGrossWeightTbox.Text = mWeighingBill.sendGrossWeight.ToString();
                this.SendTraeWeightTbox.Text = mWeighingBill.sendTraeWeight.ToString();
                this.SendNetWeightTbox.Text = mWeighingBill.sendNetWeight.ToString();
                this.SendRemardCombox.Text = mWeighingBill.sendRemark;

                //出场时是否允许修信息
                if ("false" == ConfigurationHelper.GetConfig(ConfigItemName.outFactoryAllowUpdate.ToString()))
                {
                    this.SupplyCb.IsEnabled = false;
                    this.ReceiverCompanyCb.IsEnabled = false;
                    this.CarNumberCb.IsEnabled = false;
                    this.SendYardCb.IsEnabled = false;
                    this.ReceiverYardCb.IsEnabled = false;
                    this.MaterialNameCb.IsEnabled = false;
                }
                #endregion
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            SetWeihinger();
            GetScaleInfo();
            SetCurrScaleInfo();            
        }
        #region scale

        private void SetCurrScaleInfo()
        {
            if (mScales == null || mScales.Count <= 0)
            {
                ComAlertTb.Content = "未设置磅称";
                mCurrScale = null;             
                return;
            }
            if (mScales != null && mScales.Count > 0)
            {
                if (mScales.Count <= 1)
                {
                    mCurrScale = mScales[0];
                    //读取磅称数据，并显示
                    setScale();                   
                    return;
                }
                this.ScalePanel.Visibility = Visibility.Visible;
                this.ScaleListPanel.Children.Clear();
                for (int i = 0; i < mScales.Count; i++)
                {
                    RadioButton rb = new RadioButton()
                    {
                        Height = 25,
                        Style = (Style)FindResource(ResourceName.RadioButtonDefaultStyle.ToString()),
                        Content = mScales[i].name,
                        Tag = i
                    };
                    rb.Checked += Rb_Checked;
                    if (mScales.Count == 1)
                    {
                        rb.IsChecked = true;
                        mCurrScale = mScales[0];
                        rb.Checked -= Rb_Checked;
                    }
                    else if (mScales[i].defaultType == (int)ScaleDefaultType.In)
                    {
                        rb.IsChecked = true;
                        mCurrScale = mScales[i];
                    }
                    this.ScaleListPanel.Children.Add(rb);
                }
            }
        }
        /// <summary>
        ///  scale checked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rb_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            mCurrScale = mScales[Convert.ToInt32(rb.Tag.ToString())];
            //读取磅称数据，并显示
            setScale();
        }

        /// <summary>
        /// 读取磅称数据，并显示,切换摄像头
        /// </summary>
        private void setScale()
        {
            ReadScaleData();
            ShowScaleData();
            // 切换摄像头
            GetCameraInfo();
            ShowCamera(this.CameraBorder,this.CameraStackPanel,this.settingVideoBtn);
        }
        #region  读取磅称数据，并显示
        private void ShowScaleData()
        {
            DisposeTimer();
           mTimer = new Timer(SerialPortCallBack, 1, 1000, 1000);         
        }
        /// <summary>
        /// 回调显示数据
        /// </summary>
        /// <param name="stae"></param>
        private void SerialPortCallBack(Object stae)
        {
            ScaleDataResult result = mScaleDataFormarter.ReadValue(mSerialPort);
            if (result.ErrCode == 0)
            {
                this.ComAlertTb.Visibility = Visibility.Collapsed;
                this.ComAlertTb.Content = "";
                this.Dispatcher.Invoke(
                    new Action(delegate
                    {
                        this.ShowValueTb.Text = result.Value + "";
                    }));
            }
            else
            {
                this.Dispatcher.Invoke(
                new Action(delegate
                {
                    this.ComAlertTb.Visibility = Visibility.Visible;
                    this.ComAlertTb.Content = result.Msg;
                }));
            
            }        
        }

        #endregion

        #endregion
        
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisposeSerialPort();
            DisposeTimer();
            try { CHCNetSDK.NET_DVR_Cleanup(); } catch { }
            if (isOptionSuccess == true)
            {
                RefershParentData(true, true, true);
            }
        }
        #region save 
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            //使用线程更新 备注     
            UpdateRemark();
            if (CheckInput() == true)
            {
                if (isInsert == true)
                {
                    Insert();
                }
                else
                {
                    Update();
                }
            }
        }
       
        /// <summary>
        /// 检查输入和数据
        /// </summary>
        /// <returns></returns>
        private bool CheckInput()
        {
            if (string.IsNullOrEmpty(mWeighingBill.sendCompanyId) || string.IsNullOrEmpty(mWeighingBill.sendCompanyId))
            {                
                ShowAlert("请选择供应商信息");
                this.SupplyCb.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(mWeighingBill.sendYardId) || string.IsNullOrEmpty(mWeighingBill.sendYardName))
            {
                ShowAlert("请选择发货货场");
                this.SendYardCb.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(mWeighingBill.receiveCompanyId) || string.IsNullOrEmpty(mWeighingBill.receiveCompanyName))
            {
                ShowAlert("请选择收货公司");
                this.ReceiverCompanyCb.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(mWeighingBill.receiveYardId) || string.IsNullOrEmpty(mWeighingBill.receiveYardName))
            {
                ShowAlert("请选择收货场");
                this.ReceiverYardCb.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(mWeighingBill.sendMaterialId) || string.IsNullOrEmpty(mWeighingBill.sendMaterialName))
            {
                ShowAlert("请选择物资名称");
                this.MaterialNameCb.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(mWeighingBill.carId) || string.IsNullOrEmpty(mWeighingBill.plateNumber))
            {
                ShowAlert("请输入车辆信息");
                this.CarNumberCb.Focus();
                return false;
            }
            return true;
        }

        //insert
        private void Insert()
        {
            mWeighingBill.sendInTime = DateTimeHelper.getCurrentDateTime();
            mWeighingBill.syncTime = DateTimeHelper.GetTimeStamp();
            int res = DatabaseOPtionHelper.GetInstance().insert(mWeighingBill);
            if (res > 0)
            {
                isOptionSuccess = true;
                //CaptureJpeg
                if(mCameraInfos !=null || mCameraInfos.Count > 0)
                {
                    new Thread(new ThreadStart(this.CaptureJpeg)) { IsBackground = true }.Start();
                }  
                //Update Send Bill
                new Thread(new ThreadStart(this.UpdateSendCarBill)).Start();
             
                // success to do TempUpdateUsedBase
                UpdateUsedBaseData();

                if (ShowAlertResult() == MMessageBox.Reault.No)
                {
                    this.Close();
                }
                this.InFactoryRB.IsChecked = false;
                this.InFactoryRB.IsChecked = true;
            }
        }
        //update
        private void Update()
        {
            mWeighingBill.sendOutTime = DateTimeHelper.getCurrentDateTime();
            mWeighingBill.syncTime = DateTimeHelper.GetTimeStamp();
            int res = DatabaseOPtionHelper.GetInstance().update(mWeighingBill);
            if (res > 0)
            {
                isOptionSuccess = true;
                //CaptureJpeg
                if (CameraIds != null || CameraIds.Count > 0)
                {
                    new Thread(new ThreadStart(this.CaptureJpeg)) { IsBackground = true }.Start();
                }
                // update send car bill 
                new Thread(new ThreadStart(this.UpdateSendCarBill)).Start();
                // success to do TempUpdateUsedBase
                UpdateUsedBaseData();
                //print
                PrintBill(WeightingBillType.CK);

                if (ShowAlertResult() == MMessageBox.Reault.No)
                {
                    this.Close();
                }
                this.InFactoryRB.IsChecked = true;
            }
        }
    
        /// <summary>
        /// 更新派车单
        /// </summary>
        private void UpdateSendCarBill()
        {
            SendCarBill bill=null;
            if (mSendCarBill == null)
            {
                if (mWeighingBill.sendCarBillId != null)
                {
                    bill = SendCardBillModel.GetById(mWeighingBill.sendCarBillId);
                }
            }
            else {
                bill = mSendCarBill;
            }
            if (bill == null)
            {
                return;
            }
            CommonFunction.WeighingMargeToSendCarrBill(mWeighingBill, ref bill);
            if (isInsert)
            {
                bill.weightStatus = (int)SendCarWeighingStatus.In;
            }
            else
            {
                bill.weightStatus = (int)SendCarWeighingStatus.Out;
            }
            DatabaseOPtionHelper.GetInstance().update(bill);
        }
        /// <summary>
        /// 更新备注
        /// </summary>
        private void UpdateRemark()
        {
            Thread thread = new Thread(new ParameterizedThreadStart(CommonFunction.UpdateOutputReamak));
            thread.Start(mWeighingBill.sendRemark);
            SetRemarkDefaultSource(this.SendRemardCombox,1);
        }

   
        #endregion

        #region Supply 
        private bool isSupplySelected = false;
        private void SupplyCb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsHasSendCarBill == true) { return; }
            if (isOutFactory == true)
            {
                return;
            }
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
                    MMessageBox.GetInstance().ShouBox("发货公司和收货公司不能是同一个", "提示", MMessageBox.ButtonType.No, MMessageBox.IconType.error, Orientation.Vertical);
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
            if (isAllowDiffrenceCompany == false) { return; }
            if (IsHasSendCarBill == true) { return; }
            if (isOutFactory == true)
            {
                return;
            }
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
                    MMessageBox.GetInstance().ShouBox("发货公司和收货公司不能是同一个", "提示", MMessageBox.ButtonType.No, MMessageBox.IconType.error, Orientation.Vertical);               
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
            if (isAllowDiffrenceMaterial == false) { return; }
            if (IsHasSendCarBill == true) { return; }
            if (isOutFactory == true)
            {
                return;
            }
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
            if (IsHasSendCarBill == true) { return; }
            if (isOutFactory == true) { return; }
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
                MMessageBox.Reault result = MMessageBox.GetInstance().ShouBox($"您还没有设置货场名称", "提示", MMessageBox.ButtonType.YesNo, MMessageBox.IconType.success, Orientation.Vertical, "是");
                if (result == MMessageBox.Reault.Yes)
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
                MMessageBox.Reault result = MMessageBox.GetInstance().ShouBox($"您还没有设置收货货场名称", "提示", MMessageBox.ButtonType.YesNo, MMessageBox.IconType.success, Orientation.Vertical, "是");

                if (result == MMessageBox.Reault.Yes)
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

        #region Weihinger
        private void SetWeihinger()
        {
            this.WeihingerTbox.Text = App.currentUser.name;
            mWeighingBill.sendUserName = App.currentUser.name;
            mWeighingBill.sendUserId = App.currentUser.id;
        }
        #endregion

        #region 磅称的数据
        private void ShowValueTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded == false) return;
            if (this.InFactoryRB.IsChecked == true)
            {
                this.SendTraeWeightTbox.Text = this.ShowValueTb.Text;
            }
            else
            {
                this.SendGrossWeightTbox.Text = this.ShowValueTb.Text;
            }
        }
        #endregion

        #region if factory or out factory
        private void InFactoryRB_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                mSendCarBill = null;
                mWeighingBill = null;
                isInsert = true;
                IsHasSendCarBill = false;
                isOutFactory = false;
                BuildCurrWeighingBill();
            }
        }

        private void OutFactoryRb_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                isOutFactory = true;
                IsHasSendCarBill = false;
                isInsert = false;
                //如果点击了车辆出场，就要将传过来的派车单置空
                mSendCarBill = null;
                new CarOutFactoryW(WeightingBillType.CK) { SelectAction = new Action<WeighingBill>(this.SelectCarOutFactory) }.ShowDialog();
            }
        }

        private void SelectCarOutFactory(WeighingBill bill)
        {
            if (bill == null)
            {
                this.InFactoryRB.IsChecked = true;
            }
            else
            {
                mWeighingBill = bill;
                BuildCurrWeighingBill();
            }
        }

        private void SendRemardCombox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded == false) { return; }       
            mWeighingBill.sendRemark = this.SendRemardCombox.Text.Trim();
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



        //test
        private void textWeihgtTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.textWeihgtTb.Text.Length <= 0)
            {
                this.textWeihgtTb.Text = "0";
            }
            if (RegexHelper.IsNumber(this.textWeihgtTb.Text))
            {
                this.ShowValueTb.Text = this.textWeihgtTb.Text;
            }
            else
            {
                this.textWeihgtTb.Text = "0";
            }

        }

        #region  Select Send Car bill
        private void SendCarRB_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded) {
                isOutFactory = false;
                IsHasSendCarBill = true;
                isInsert = true;
                mWeighingBill = null;        
                new SendCarBillSelectW() { SelectAction = new Action<SendCarBill>(this.SelectSendCar) }.ShowDialog();
            }
            this.SendCarRB.IsChecked = false;
        }
        private void SelectSendCar(SendCarBill bill)
        {
            if (bill == null)
            {
                this.InFactoryRB.IsChecked = true;
            }
            else
            {
                mSendCarBill = bill;
                BuildCurrWeighingBill();
            }
        }
        #endregion
    }
}
