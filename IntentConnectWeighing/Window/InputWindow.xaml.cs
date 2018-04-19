using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyHelper;
using static System.Windows.Forms.Timer;
using System.Threading;
using System.Text.RegularExpressions;
using IntentConnectWeighing.CameraSdk;
namespace IntentConnectWeighing
{
    /// <summary>
    /// InputWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputWindow : Window
    {
        // left center right
        public Action<bool, bool, bool> RefershParent;
        #region 本机使用的临时基础数据
        private Dictionary<String, Company> tempSupplyCompanys = new Dictionary<string, Company>();
        private Dictionary<String, Company> tempCustomerCompanys = new Dictionary<string, Company>();
        private Dictionary<String, Material> tempMaterials = new Dictionary<string, Material>();
        private Dictionary<String, CarInfo> tempCars = new Dictionary<string, CarInfo>();
        #endregion
        #region Variable area
        private bool isConnectionVersion = true; //是否是互联版本
        private bool isAllowDiffrenceMaterial = true; //是否允许货物名称不一致
        private bool isAllowDiffrenceCompany = true; //是否允许收货公司不一致
        private bool isAllowDiffrenceReceiveYard = true; //是否允许收货货场不一致
        private bool isOutFactory = false; //是否出场
        private bool isOptionSuccess = false;//是否过磅成功过
        private List<Scale> mScales;
        private Scale mCurrScale;
        private List<CameraInfo> mCameraInfos;
        private Timer mTimer;
        private SerialPort mSerialPort = null;
        private string oldstring;
        private double CameraPanelWidth;
        private double CameraWidth;
        private double CameraHeight;
        private int CameraCount;
        private int CarmeraMarging = 0;
        private String currBillNumber;//当前的磅单编号
        private Company sendCompany;
        private Company receiverCompany;
        private Material material;
        private CarInfo car;
        private WeighingBill mWeighingBill;
        private WeighingBill mSendWeighingBill;
        private bool isInsert = true;
        #endregion
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
            //将本机使用的基础数据设置默认数据源 下四个方法
            SetSupplyCompanyDefaultSource();
            SetCustomerCompanyDefaultSource();
            SetMaterialDefaultSource();
            SetCarDefaultSource();
            SetCarDecuationDescriptionDefaultSource();
            //
            if (mSendWeighingBill == null)
            {
                this.SendNetWeightTbox.IsEnabled = true;
            }

            //
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
                this.ReceivedRemardTbox.Text = mWeighingBill.receiveRemark;

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
        private void GetScaleInfo()
        {
            String @condition = ScaleEnum.client_id.ToString() + "=" + Constract.valueSplit + App.CurrClientId + Constract.valueSplit + " and " +
                ScaleEnum.company_id.ToString() + "=" + Constract.valueSplit + App.currentCompany.id + Constract.valueSplit;
            String sql = DbBaseHelper.getSelectSql(DataTabeName.scale.ToString(), null, condition, null, null, ScaleEnum.default_type.ToString() + " desc");
            mScales = DbBaseHelper.DataTableToEntity<Scale>(DatabaseOPtionHelper.GetInstance().select(sql));
        }

        private void SetCurrScaleInfo()
        {
            if (mScales != null && mScales.Count > 0)
            {
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
            ReadScaleData();
            ShowScaleData();
            // 切换摄像头
            GetCameraInfo();
            ShowCamera();
        }


        #region  读取磅称数据，并显示
        private void ReadScaleData()
        {
            DisposeSerialPort();
            mSerialPort = new SerialPort
            {
                BaudRate = mCurrScale.baudRate,
                PortName = mCurrScale.com
            };
            mSerialPort.DataBits = mCurrScale.dataByte;
            mSerialPort.StopBits = StopBits.One;
            mSerialPort.Parity = Parity.None;
            try
            {
                mSerialPort.Open();
            }
            catch (Exception e)
            {
                this.ComAlertTb.Visibility = Visibility.Visible;
                this.ComAlertTb.Content = "数据读取出错：" + e.Message;
            }
        }

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
                    this.ComAlertTb.Content = "显示磅称数据出错：" + e.Message;
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
            int value = 123;
            try
            {
                int bytes = mSerialPort.BytesToRead;
                byte[] buffer = new byte[bytes];
                mSerialPort.Read(buffer, 0, bytes);
                string readbuffer = Encoding.ASCII.GetString(buffer);

                if (readbuffer.Length >= 10)
                {
                    string[] weightstrs = readbuffer.Split('+');
                    foreach (string weightstr in weightstrs)
                    {
                        if (weightstr.Length >= 10)
                        {
                            string weightst = weightstr.Substring(0, 6);
                            if (weightst != oldstring)
                            {
                                oldstring = weightst;
                                string temp = Regex.Replace(weightst, "[0]*([1-9][0-9]*)", "$1");
                                value = Convert.ToInt32(temp);
                                Double dou = Convert.ToDouble(value);
                                String number = Convert.ToString(Math.Round((dou / 1000), 2, MidpointRounding.AwayFromZero));
                                this.ComAlertTb.Visibility = Visibility.Collapsed;
                                this.ComAlertTb.Content = "";
                                this.Dispatcher.Invoke(
                                    new Action(delegate
                                    {
                                        this.ShowValueTb.Text = number;
                                    }));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.writeLine("回调显示出错数据：" + value + " msg " + ex.Message);
            }
        }

        private void DisposeSerialPort()
        {
            if (mSerialPort != null)
            {
                if (mSerialPort.IsOpen == true)
                {
                    mSerialPort.Close();
                }
                mSerialPort.Dispose();
            }
        }

        private void DisposeTimer()
        {
            if (mTimer != null)
            {
                mTimer.Dispose();
                mTimer = null;
            }
        }
        #endregion

        #endregion

        #region Camera Info
        private void GetCameraInfo()
        {
            String @condition = CameraInfoEnum.client_id.ToString() + "=" + Constract.valueSplit + App.CurrClientId + Constract.valueSplit + " and " +
               CameraInfoEnum.company_id.ToString() + "=" + Constract.valueSplit + App.currentCompany.id + Constract.valueSplit + " and " +
                CameraInfoEnum.scale_id.ToString() + "=" + Constract.valueSplit + mCurrScale.id + Constract.valueSplit;
            String sql = DbBaseHelper.getSelectSql(DataTabeName.camera_info.ToString(), null, condition, null, null);
            mCameraInfos = DbBaseHelper.DataTableToEntity<CameraInfo>(DatabaseOPtionHelper.GetInstance().select(sql));
        }
        private List<Int32> CameraIds;
        private List<CHCNetSDK.NET_DVR_DEVICEINFO_V30> mDeviceInfors;
        private List<System.Windows.Forms.PictureBox> mPictureBoxs;
        /// <summary>
        /// 显示摄像头
        /// </summary>        
        private void ShowCamera()
        {
            if (mCameraInfos.Count <= 0)
            {
                return;
            }
            CameraPanelWidth = this.CameraBorder.ActualWidth;
            CameraWidth = Math.Floor(CameraPanelWidth / mCameraInfos.Count);
            CameraHeight = this.CameraBorder.ActualHeight - 2;
            this.CameraStackPanel.Children.Clear();
            if (CameraHelper.InitSDK() == false)
            {
                MessageBox.Show("摄像头SDK初始化失败！");
                return;
            }
            if (CameraIds == null)
            {
                CameraIds = new List<int>();
            }
            else
            {
                StorpPreviewCamera();
                LogoutCamera();
                CameraIds.Clear();
            }
            if (mPictureBoxs == null)
            {
                mPictureBoxs = new List<System.Windows.Forms.PictureBox>();
            }
            else
            {
                mPictureBoxs.Clear();
            }
            if (mDeviceInfors == null)
            {
                mDeviceInfors = new List<CHCNetSDK.NET_DVR_DEVICEINFO_V30>();
            }
            else
            {
                mDeviceInfors.Clear();
            }
            for (int i = 0; i < mCameraInfos.Count; i++)
            {
                CameraInfo camera = mCameraInfos[i];
                System.Windows.Forms.Integration.WindowsFormsHost windowsFormsHost = new System.Windows.Forms.Integration.WindowsFormsHost() { Name = $"WFH{i}" };
                System.Windows.Forms.PictureBox pictureBox = new System.Windows.Forms.PictureBox() { Name = $"pictureBox{i}", Width = (int)CameraWidth, Height = (int)CameraHeight };
                mPictureBoxs.Add(pictureBox);
                CHCNetSDK.NET_DVR_DEVICEINFO_V30 deviceinfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
                //login
                int cameraid = CameraHelper.loginCamera(camera.ip, camera.port, camera.userName, camera.password, ref deviceinfo);
                CameraIds.Add(cameraid);
                mDeviceInfors.Add(deviceinfo);
                //preview
                int ChanNum = deviceinfo.byChanNum;
                int streamType = Convert.ToInt32(ConfigurationHelper.GetConfig(ConfigItemName.cameraStramType.ToString()));
                bool res = CameraHelper.Preview(ref pictureBox, ChanNum, cameraid, streamType);
                windowsFormsHost.Child = pictureBox;
                this.CameraStackPanel.Children.Add(windowsFormsHost);
            }
        }

        private void LogoutCamera()
        {
            if (CameraIds != null && CameraIds.Count > 0)
            {
                for (int i = 0; i < CameraIds.Count; i++)
                {
                    try
                    {
                        CameraHelper.LoginOutCamera(CameraIds[i]);
                    }
                    catch
                    {
                        MessageBox.Show("退出摄像头失败！" + i);
                    }
                }
            }
        }

        private void StorpPreviewCamera()
        {
            if (CameraIds != null && CameraIds.Count > 0)
            {
                for (int i = 0; i < CameraIds.Count; i++)
                {
                    try
                    {
                        CameraHelper.stopPreview(CameraIds[i]);
                    }
                    catch
                    {
                        MessageBox.Show("停止预览失败！" + i);
                    }
                }
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


        private void SetCarDecuationDescriptionDefaultSource()
        {
            this.DecuationDescriptionCb.ItemsSource = App.decuationDesList;
        }
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

        private void RefershParentData(bool left = false, bool center = false, bool right = false)
        {
            if (this.RefershParent != null)
            {
                RefershParent(left, center, right);
            }
        }

        #region save
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
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
                MessageBox.Show("请选择收货货场");
                this.ReceiverYardCb.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(mWeighingBill.receiveMaterialId) || string.IsNullOrEmpty(mWeighingBill.receiveMaterialName))
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
                new Thread(new ThreadStart(this.CaptureJpeg)) { IsBackground = true }.Start();
                MessageBoxResult result = MessageBox.Show("保存成功 ! 要继续过磅吗？", "恭喜", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //Update Send Bill
                new Thread(new ThreadStart(this.UpdateSendBill)).Start();
                // success to do TempUpdateUsedBase
                UpdateUsedBaseData();
                UpdateDecuationList();

                if (result == MessageBoxResult.No)
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
                new Thread(new ThreadStart(this.CaptureJpeg)) { IsBackground = true }.Start();

                //Update Send Bill
                new Thread(new ThreadStart(this.UpdateSendBill)).Start();

                MessageBoxResult result = MessageBox.Show("保存成功 ! 要继续过磅吗？", "恭喜", MessageBoxButton.YesNo, MessageBoxImage.Question);
                // success to do TempUpdateUsedBase
                UpdateUsedBaseData();
                UpdateDecuationList();

                PrintBill();

                if (result == MessageBoxResult.No)
                {
                    this.Close();
                }
                this.InFactoryRB.IsChecked = true;
            }
        }

        private void UpdateUsedBaseData()
        {
            // success to do TempUpdateUsedBase
            Thread thread = new Thread(new ParameterizedThreadStart(CommonFunction.TempUpdateUsedBase));
            thread.Start(new BaseDataClassV() { send = sendCompany, receive = receiverCompany, material = material, carInfo = car });
        }
        private void UpdateDecuationList()
        {
            Thread thread = new Thread(new ParameterizedThreadStart(CommonFunction.UpdateDecuationList));
            thread.Start(mWeighingBill.decuationDescription);
        }
        private void UpdateSendBill()
        {
            if (mSendWeighingBill != null)
            {
                try
                {
                    String condition = @WeighingBillEnum.id.ToString() + " = " + Constract.valueSplit + mWeighingBill.relativeBillId + Constract.valueSplit+
                     " and "+ WeighingBillEnum.type.ToString() +"="+ ((int)WeightingBillType.CK);
                    String set = WeighingBillEnum.relative_bill_id.ToString() + "=" + Constract.valueSplit + mWeighingBill.id + Constract.valueSplit;
                    string sql = DbBaseHelper.getUpdateSql(DataTabeName.weighing_bill.ToString(), set, condition);
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
                String sql = DbBaseHelper.getSelectSql(DataTabeName.weighing_bill.ToString(), null, where);
                List<WeighingBill> list = DbBaseHelper.DataTableToEntity<WeighingBill>(DatabaseOPtionHelper.GetInstance().select(sql));
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
        /// <summary>
        /// 打印
        /// </summary>
        private void PrintBill()
        {
            bool auto = false;
            if (MyHelper.ConfigurationHelper.GetConfig(ConfigItemName.autoPrint.ToString()) == "true") {
                auto = true;
            }
            new PrintBillW(WeightingBillType.RK, mWeighingBill, auto) { }.Show();
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
                string condition = CompanyEnum.name.ToString() + " like '%" + text.ToUpper() + "%' " + " OR " + CompanyEnum.name_first_case.ToString() + " like '%" + text + "%'";
                String sql = DbBaseHelper.getSelectSql(DataTabeName.company.ToString(), null, condition);
                List<Company> list = DbBaseHelper.DataTableToEntity<Company>(DatabaseOPtionHelper.GetInstance().select(sql));
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
                string condition = CompanyEnum.name.ToString() + " like '%" + text + "%' " + " OR " + CompanyEnum.name_first_case.ToString() + " like '%" + text + "%'";
                String sql = DbBaseHelper.getSelectSql(DataTabeName.company.ToString(), null, condition);
                List<Company> list = DbBaseHelper.DataTableToEntity<Company>(DatabaseOPtionHelper.GetInstance().select(sql));
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
                string condition = MaterialEnum.name.ToString() + " like '%" + text + "%' " + " OR " + MaterialEnum.name_first_case.ToString() + " like '%" + text + "%'";
                String sql = DbBaseHelper.getSelectSql(DataTabeName.material.ToString(), null, condition);
                List<Material> list = DbBaseHelper.DataTableToEntity<Material>(DatabaseOPtionHelper.GetInstance().select(sql));
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
            mWeighingBill.receiveRemark = this.ReceivedRemardTbox.Text;
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
            mWeighingBill.decuationDescription = this.DecuationDescriptionCb.Text;
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

        /// <summary>
        /// 通道截图
        /// </summary>
        public void CaptureJpeg()
        {
            if (mCameraInfos == null || mCameraInfos.Count <= 0)
            {
                return;
            }
            string filePath = ConfigurationHelper.GetConfig(ConfigItemName.cameraCaptureFilePath.ToString());
            if (String.IsNullOrEmpty(filePath))
            {
                filePath = System.IO.Path.Combine(FileHelper.GetRunTimeRootPath(), "capture");
            }
            String fileName = String.Empty;
            for (int i = 0; i < mCameraInfos.Count; i++)
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
                CameraHelper.CaptureJpeg(fileNamePath, CameraIds[i], mDeviceInfors[i].byChanNum);
            }
        }



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
