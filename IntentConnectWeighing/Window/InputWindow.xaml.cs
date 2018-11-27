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
    public partial class InputWindow : WeighingWindow
    {
        private WeighingBill mSendWeighingBill;       
        public InputWindow(WeighingBill bill = null, bool isSend = false)
        {
            InitializeComponent();
            if (isSend == true)
            {
                mSendWeighingBill = bill;
            }
            else
            {
                mWeighingBill = bill;
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
            SetCarDecuationDescriptionDefaultSource(this.DecuationDescriptionCb);
            SetRemarkDefaultSource(this.ReceivedRemardCombox);
            
            if (mSendWeighingBill == null)
            {
                this.SendNetWeightTbox.IsEnabled = true;
            }
                        
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
                currBillNumber = CommonFunction.GetWeighingNumber(WeightingBillType.RK);
                this.BillNumberTb.Text = currBillNumber;

                mWeighingBill = new WeighingBill()
                {
                    id = Guid.NewGuid().ToString(),
                    type = (int)WeightingBillType.RK,
                    receiveUserId = App.currentUser.id,
                    receiveUserName = App.currentUser.name,
                    receiveNumber = currBillNumber,
                    affiliatedCompanyId = App.currentCompany.id,
                    receiveStatus = 0,
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

                this.SendGrossWeightTbox.Text = "0.0";
                this.SendTraeWeightTbox.Text = "0.0";
                this.SendNetWeightTbox.Text = "0.0";

                this.SendNetWeightTbox.IsEnabled = true;
                this.MaterialNameCb.IsEnabled = true;
                this.SupplyCb.IsEnabled = true;
                this.ReceiverCompanyCb.IsEnabled = true;
                this.CarNumberCb.IsEnabled = true;
                this.SendYardCb.IsEnabled = true;
                this.ReceiverYardCb.IsEnabled = true;

                #endregion
                if (mSendWeighingBill != null)
                {
                    //有发货单 最后要将收货信息合并到发货的信息上。
                    CommonFunction.MargeToReciver(ref mWeighingBill, mSendWeighingBill);
                    #region 将发货信息显示到控件上面
                    this.SupplyCb.Text = mWeighingBill.sendCompanyName;
                    this.SendYardCb.Text = mWeighingBill.sendYardName;
                    this.CarNumberCb.Text = mWeighingBill.plateNumber;
                    this.DriverTbox.Text = mWeighingBill.driver;
                    this.PhoneTbox.Text = mWeighingBill.driverMobile;
                    this.ReceiverCompanyCb.Text = mWeighingBill.receiveCompanyName;
                    this.ReceiverYardCb.Text = mWeighingBill.receiveYardName;
                    this.MaterialNameCb.Text = mWeighingBill.sendMaterialName;
                    mWeighingBill.receiveMaterialName = mSendWeighingBill.sendMaterialName;
                    mWeighingBill.receiveMaterialId = mSendWeighingBill.sendMaterialId;
                    this.SendGrossWeightTbox.Text = mWeighingBill.sendGrossWeight.ToString();
                    this.SendTraeWeightTbox.Text = mWeighingBill.sendTraeWeight.ToString();
                    this.SendNetWeightTbox.Text = mWeighingBill.sendNetWeight.ToString();
                    this.SendNetWeightTbox.IsEnabled = false;
                    this.CarNumberCb.IsEnabled = false;
                    this.SupplyCb.IsEnabled = false;
                    this.SendYardCb.IsEnabled = false;
                    #endregion
                    // 货物名称，收货信息 不一样时 
                    if (ConfigurationHelper.GetConfig(ConfigItemName.softwareVersion.ToString()) == SoftwareVersion.netConnection.ToString())
                    {
                        if ("false" == ConfigurationHelper.GetConfig(ConfigItemName.allowDiffrenceMaterialWeighing.ToString()))
                        {
                            isAllowDiffrenceMaterial = false;
                            //mWeighingBill.receiveMaterialId = mSendWeighingBill.sendMaterialId;
                            //mWeighingBill.receiveMaterialName = mSendWeighingBill.sendMaterialName;
                            //this.MaterialNameCb.Text = mSendWeighingBill.sendMaterialName;
                            this.MaterialNameCb.IsEnabled = false;
                        }
                        else
                        {
                            isAllowDiffrenceMaterial = true;
                            this.MaterialNameCb.IsEnabled = true;
                        }
                        if ("false" == MyHelper.ConfigurationHelper.GetConfig(ConfigItemName.allowDiffrenceCompany.ToString()))
                        {
                            isAllowDiffrenceCompany = false;
                            this.ReceiverCompanyCb.IsEnabled = false;
                        }
                        else
                        {
                            isAllowDiffrenceCompany = true;
                            this.ReceiverCompanyCb.IsEnabled = true;
                        }
                        if ("false" == MyHelper.ConfigurationHelper.GetConfig(ConfigItemName.allowDiffrenceReceiveYard.ToString()))
                        {
                            isAllowDiffrenceReceiveYard = false;
                            this.ReceiverYardCb.IsEnabled = false;
                        }
                        else
                        {
                            isAllowDiffrenceReceiveYard = true;
                            this.ReceiverYardCb.IsEnabled = true;
                        }
                    }
                }
            }
            else
            {
                //out factoty
                isOutFactory = true;
                mWeighingBill.receiveStatus = 1;
                #region 设置控件状态
                this.BillNumberTb.Text = mWeighingBill.receiveNumber;
                this.SupplyCb.Text = mWeighingBill.sendCompanyName;
                this.ReceiverCompanyCb.Text = mWeighingBill.receiveCompanyName;
                this.CarNumberCb.Text = mWeighingBill.plateNumber;
                this.DriverTbox.Text = mWeighingBill.driver;
                this.PhoneTbox.Text = mWeighingBill.driverMobile;
                this.SendYardCb.Text = mWeighingBill.sendYardName;
                this.ReceiverYardCb.Text = mWeighingBill.receiveYardName;
                this.MaterialNameCb.Text = mWeighingBill.receiveMaterialName;
                this.ReceivedGrossWeightTbox.Text = mWeighingBill.receiveGrossWeight.ToString();
                this.SendGrossWeightTbox.Text = mWeighingBill.sendGrossWeight.ToString();
                this.SendTraeWeightTbox.Text = mWeighingBill.sendTraeWeight.ToString();
                this.SendNetWeightTbox.Text = mWeighingBill.sendNetWeight.ToString();
                this.ReceivedRemardCombox.Text = mWeighingBill.receiveRemark;
                this.SendNetWeightTbox.IsEnabled = false;
                //互联版本,出场时是否允许修信息
                if ("false" == ConfigurationHelper.GetConfig(ConfigItemName.outFactoryAllowUpdate.ToString()))
                {
                    this.SendNetWeightTbox.IsEnabled = false;
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
            GetScaleInfo();
            SetCurrScaleInfo();
            SetWeihinger();
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
                if (mScales.Count <= 1) {                   
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
        private void setScale() {
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
            if (mSerialPort.IsOpen == false)
            {
                try
                {
                    mSerialPort.Open();
                }
                catch (Exception e)
                {
                    this.ComAlertTb.Visibility = Visibility.Visible;
                    this.ComAlertTb.Content = "串口打开失败：" + e.Message;
                }
            }
            if (mSerialPort.IsOpen == true)
            {
                mTimer = new Timer(SerialPortCallBack, 1, 1000, 1000);
            }
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
        private void Window_Activated(object sender, EventArgs e)
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
            //使用线程更新 备注 和 扣重说明
            UpdateRemark();
            UpdateDecuationList();
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

            //Double d = DateTimeHelper.GetTimeStamp();
            //this.CaptureJpeg();
            //ConsoleHelper.writeLine("Capture Jpeg cpmplated.." + (DateTimeHelper.GetTimeStamp() - d).ToString());

            //this.UICapture();
        }
        /// <summary>
        /// update decuation description and remark list
        /// </summary>
        private void UpdateRemark() {
            Thread thread = new Thread(new ParameterizedThreadStart(CommonFunction.UpdateInputReamak));
            thread.Start(mWeighingBill.receiveRemark);
            SetRemarkDefaultSource(this.ReceivedRemardCombox);
        }

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
                ShowAlert("请选择收货货场");
                this.ReceiverYardCb.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(mWeighingBill.receiveMaterialId) || string.IsNullOrEmpty(mWeighingBill.receiveMaterialName))
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
            mWeighingBill.receiveInTime = DateTimeHelper.getCurrentDateTime();
            mWeighingBill.syncTime = DateTimeHelper.GetTimeStamp();
            int res = DatabaseOPtionHelper.GetInstance().insert(mWeighingBill);
            if (res > 0)
            {
                isOptionSuccess = true;
                //CaptureJpeg
                if (mCameraInfos != null || mCameraInfos.Count > 0)
                {
                    new Thread(new ThreadStart(this.CaptureJpeg)) { IsBackground = true }.Start();
                }
                //Update Send Bill
                new Thread(new ThreadStart(this.UpdateSendBill)).Start();
                // success to do TempUpdateUsedBase
                UpdateUsedBaseData();
                
                if (ShowAlertResult() == MMessageBox.Result.No)
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
            mWeighingBill.receiveOutTime = DateTimeHelper.getCurrentDateTime();
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

                //Update Send Bill
                new Thread(new ThreadStart(this.UpdateSendBill)).Start();
                 // success to do TempUpdateUsedBase
                UpdateUsedBaseData();
                // print 
                PrintBill(WeightingBillType.RK);

                if (ShowAlertResult() == MMessageBox.Result.No)
                {
                    this.Close();
                }
                this.InFactoryRB.IsChecked = true;
            }
        }

     
        private void UpdateDecuationList()
        {
            Thread thread = new Thread(new ParameterizedThreadStart(CommonFunction.UpdateDecuationList));
            thread.Start(mWeighingBill.decuationDescription);
            SetCarDecuationDescriptionDefaultSource(this.DecuationDescriptionCb);
        }
        /// <summary>
        /// 更新发货单
        /// </summary>
        private void UpdateSendBill()
        {
            if (mSendWeighingBill != null)
            {
                try
                {
                    String condition = @WeighingBillEnum.id.ToString() + " = " + Constract.valueSplit + mWeighingBill.relativeBillId + Constract.valueSplit+
                     " and "+ WeighingBillEnum.type.ToString() +"="+ ((int)WeightingBillType.CK);
                    String set = WeighingBillEnum.relative_bill_id.ToString() + "=" + Constract.valueSplit + mWeighingBill.id + Constract.valueSplit;
                    string sql = DatabaseOPtionHelper.GetInstance().getUpdateSql(DataTabeName.weighing_bill.ToString(), set, condition);
                    DatabaseOPtionHelper.GetInstance().update(sql);
                }
                catch (Exception e)
                {
                    ConsoleHelper.writeLine("Update Send Bill failure:" + e.Message);
                }
            }
            else if (mWeighingBill.relativeBillId != null)
            {
                String where = WeighingBillEnum.id.ToString() + "=" + Constract.valueSplit + mWeighingBill.relativeBillId + Constract.valueSplit;
                String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.weighing_bill.ToString(), null, where);
                List<WeighingBill> list = DatabaseOPtionHelper.GetInstance().select<WeighingBill>(sql);
                WeighingBill wb = null;
                if (list.Count > 0) {
                    wb = list[0];
                }
                else {
                    return;
                }              
                CommonFunction.MargeToSend(ref wb, mWeighingBill);
                try
                {
                    DatabaseOPtionHelper.GetInstance().update(wb);
                }
                catch (Exception e)
                {
                    ConsoleHelper.writeLine("UpdateSendBill failure:" + e.Message);
                }
            }
        }
  
        private void UICapture()
        {
            string filePath = ConfigurationHelper.GetConfig(ConfigItemName.cameraCaptureFilePath.ToString());
            if (String.IsNullOrEmpty(filePath))
            {
                filePath = System.IO.Path.Combine(FileHelper.GetRunTimeRootPath(), "capture");
            }
            String fileName = String.Empty;
            for (int i = 0; i < mPictureBoxs.Count; i++)
            {
                if (string.IsNullOrEmpty(currBillNumber))
                {
                    fileName = Guid.NewGuid() + Constract.CaputureSuffix;
                }
                else
                {
                    fileName = currBillNumber + "_" + i + "_" + Constract.CaputureSuffix;
                }
                String fileNamePath = System.IO.Path.Combine(filePath, fileName);
                //  SaveToImage(mPictureBoxs[i],fileNamePath,2048,1536);
            }
        }
        #endregion

        #region Supply 
        private bool isSupplySelected = false;
        private void SupplyCb_TextChanged(object sender, TextChangedEventArgs e)
        {
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
                if (list.Count <= 0)
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
            if (isAllowDiffrenceCompany == false) { return; }
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
            if (isAllowDiffrenceMaterial == false) { return; }
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
                mWeighingBill.receiveMaterialId = material.id;
                mWeighingBill.receiveMaterialName = material.name;
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

        #region Weihinger
        private void SetWeihinger()
        {
            this.WeihingerTbox.Text = App.currentUser.name;
            mWeighingBill.receiveUserName = App.currentUser.name;
            mWeighingBill.receiveUserId = App.currentUser.id;
        }
        #endregion

        #region 磅称的数据
        private void ShowValueTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded == false) return;
            if (this.InFactoryRB.IsChecked == true)
            {
                this.ReceivedGrossWeightTbox.Text = this.ShowValueTb.Text;
                this.ReceivedTraeWeightTbox.Text = "0";
            }
            else
            {
                this.ReceivedTraeWeightTbox.Text = this.ShowValueTb.Text;
            }
        }
        #endregion

        #region if factory or out factory
        private void InFactoryRB_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                mSendWeighingBill = null;
                mWeighingBill = null;
                isInsert = true;
                BuildCurrWeighingBill();
            }
        }

        private void OutFactoryRb_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                isInsert = false;
                //如果点击了车辆出场，就要将传过来的发货单置空
                mSendWeighingBill = null;
                new CarOutFactoryW(WeightingBillType.RK) { SelectAction = new Action<WeighingBill>(this.SelectCarOutFactory) }.ShowDialog();
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

        private void ReceivedRemardTbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded == false) { return; }
            string value = this.ReceivedRemardCombox.Text.Trim();      
            mWeighingBill.receiveRemark = value;
        }
        #endregion

        #region Send weight value change event
        private void SendNetWeightTbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded == false) { return; }
            Double sendnet = 0;
            try { sendnet = Convert.ToDouble(this.SendNetWeightTbox.Text); } catch { }
            mWeighingBill.sendNetWeight = sendnet;
            CalcReceiptWeightValue();
        }
        #endregion

        #region recived weight value change event
        private void ReceivedGrossWeightTbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded == false) { return; }
            CalcReceiptWeightValue();
        }

        private void ReceivedTraeWeightTbox_TextChanged(object sender, TextChangedEventArgs e)
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
            double decuation = 0;
            double net = 0;
            double sendnet = 0.0;
            double diff = 0;
            try { gross = Convert.ToDouble(this.ReceivedGrossWeightTbox.Text); } catch { }
            try { trae = Convert.ToDouble(this.ReceivedTraeWeightTbox.Text); } catch { }
            try { sendnet = Convert.ToDouble(this.SendNetWeightTbox.Text); } catch { }
            try { decuation = Convert.ToDouble(this.DecuationWeightTbox.Text); } catch { }
            net = gross - trae - decuation;
            this.ReceivedNetWeightTbox.Text = net.ToString();
            diff = System.Math.Round(net - sendnet, 2);
            this.differenceWeightTbox.Text = diff.ToString();

            mWeighingBill.decuationWeight = decuation;
            mWeighingBill.receiveGrossWeight = gross;
            mWeighingBill.receiveTraeWeight = trae;
            mWeighingBill.receiveNetWeight = net;
            mWeighingBill.differenceWeight = diff;          
        }

        private void ReceivedNetWeightTbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcReceiptWeightValue();
        }

        private void DecuationWeightTbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RegexHelper.IsNumber(this.DecuationWeightTbox.Text))
            {
                CalcReceiptWeightValue();
            }
            else
            {
                MessageBox.Show("扣重只能输入数字");
                this.DecuationWeightTbox.Text = "0";
            }

        }

        private void DecuationDescriptionTbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded == false) return;
            string value = this.DecuationDescriptionCb.Text;
            mWeighingBill.decuationDescription = this.DecuationDescriptionCb.Text.Trim();
        }

        private void differenceWeightTbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded == false) return;
            mWeighingBill.differenceWeight = Convert.ToDouble(this.differenceWeightTbox.Text);
        }
        #endregion

        //private void SaveToImage(System.Windows.Forms.PictureBox ui, string fileName, int width, int height)
        //{
        //    try
        //    {
        //        System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
        //        RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, 96d, 96d,PixelFormats.Pbgra32);
        //        bmp.Render(ui);
        //        BitmapEncoder encoder = new PngBitmapEncoder();
        //        encoder.Frames.Add(BitmapFrame.Create(bmp));
        //        encoder.Save(fs);
        //        fs.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        #region select send bill radioButton
        private void SendBillRB_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                isInsert = true;
                mWeighingBill = null;
                new SendBillSelectW() { SelectAction = new Action<WeighingBill>(this.SelectSendBillInFactory) }.ShowDialog();
            }
            this.SendBillRB.IsChecked = false;
        }

        private void SelectSendBillInFactory(WeighingBill bill)
        {            
            if (bill == null)
            {
                this.InFactoryRB.IsChecked = true;
            }
            else
            {
                mSendWeighingBill = bill;             
                BuildCurrWeighingBill();
            }
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


    }
}
