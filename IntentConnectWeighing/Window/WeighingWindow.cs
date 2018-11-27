using IntentConnectWeighing.CameraSdk;
using MyCustomControlLibrary;
using MyHelper;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace IntentConnectWeighing
{
    /// <summary>
    /// 称重的公供代码 
    /// </summary>
    public class WeighingWindow : Window
    {
        // left center right
        public Action<bool, bool, bool> RefershParent;

        #region 本机使用的临时基础数据
        protected Dictionary<String, Company> tempSupplyCompanys = new Dictionary<string, Company>();
        protected Dictionary<String, Company> tempCustomerCompanys = new Dictionary<string, Company>();
        protected Dictionary<String, Material> tempMaterials = new Dictionary<string, Material>();
        protected Dictionary<String, CarInfo> tempCars = new Dictionary<string, CarInfo>();
        #endregion
        #region Variable area
        protected bool isConnectionVersion = true; //是否是互联版本
        protected bool isAllowDiffrenceMaterial = true; //是否允许货物名称不一致
        protected bool isAllowDiffrenceCompany = true; //是否允许收货公司不一致
        protected bool isAllowDiffrenceReceiveYard = true; //是否允许收货货场不一致
        protected bool isOutFactory = false; //是否出场
        protected bool isOptionSuccess = false;//是否过磅成功过
        protected List<Scale> mScales;
        protected Scale mCurrScale;
        protected List<CameraInfo> mCameraInfos;
        protected Timer mTimer;
        protected SerialPort mSerialPort = null;
        protected string oldstring;
        protected double CameraPanelWidth;
        protected double CameraWidth;
        protected double CameraHeight;
        protected int CameraCount;
        protected int CarmeraMarging = 0;
        protected String currBillNumber;//当前的磅单编号
        protected Company sendCompany;
        protected Company receiverCompany;
        protected Material material;
        protected CarInfo car;
        protected WeighingBill mWeighingBill;
        protected ScaleDataInterpreter mScaleDataFormarter;
        protected bool isInsert = true;
        protected List<Int32> CameraIds;
        #endregion

        public void settingVideoBtn_Click(object sender, RoutedEventArgs e)
        {
            new SettingW(SettingSelectType.CameraSetting).ShowDialog();
        }

        #region 将本机使用的基础数据设置默认数据源 最新使用的排在最前面
        protected void SetSupplyCompanyDefaultSource(ComboBox SupplyCb)
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
                        SupplyCb.ItemsSource = null;
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
                    SupplyCb.ItemsSource = App.tempSupplyCompanys.Values.ToList();
                }
                else
                {
                    FileHelper.CreateFile(path);
                }
            }
        }
        protected void SetCustomerCompanyDefaultSource(ComboBox ReceiverCompanyCb)
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
                        ReceiverCompanyCb.ItemsSource = null;
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
                    ReceiverCompanyCb.ItemsSource = App.tempCustomerCompanys.Values.ToList();
                }
                else
                {
                    FileHelper.CreateFile(path);
                }
            }
        }
        protected void SetMaterialDefaultSource(ComboBox MaterialNameCb)
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
                        MaterialNameCb.ItemsSource = null;
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
                    MaterialNameCb.ItemsSource = App.tempMaterials.Values.ToList();
                }
                else
                {
                    FileHelper.CreateFile(path);
                }
            }
        }
        protected void SetCarDefaultSource(ComboBox CarNumberCb)
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
                        CarNumberCb.ItemsSource = null;
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
                    CarNumberCb.ItemsSource = App.tempCars.Values.ToList();
                }
                else
                {
                    FileHelper.CreateFile(path);
                }
            }
        }
        protected void SetCarDecuationDescriptionDefaultSource(ComboBox DecuationDescriptionCb)
        {
            DecuationDescriptionCb.ItemsSource = null;
            DecuationDescriptionCb.ItemsSource = App.decuationDesList;
        }
        protected void SetRemarkDefaultSource(ComboBox RemardCombox, int type = 0)
        {
            RemardCombox.ItemsSource = null;
            if (type == 0)
            {
                RemardCombox.ItemsSource = App.inputRemarkList;
            }
            else
            {
                RemardCombox.ItemsSource = App.outputRemarkList;
            }
        }
        #endregion

        #region  读取磅称数据，并显示
        protected void GetScaleInfo()
        {
            String @condition = ScaleEnum.client_id.ToString() + "=" + Constract.valueSplit + App.CurrClientId + Constract.valueSplit + " and " +
                ScaleEnum.company_id.ToString() + "=" + Constract.valueSplit + App.currentCompany.id + Constract.valueSplit;
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.scale.ToString(), null, condition, null, null, ScaleEnum.default_type.ToString() + " desc");
            mScales =DatabaseOPtionHelper.GetInstance().select<Scale>(sql);
        }

        protected void ReadScaleData()
        {
            DisposeSerialPort();
            mSerialPort = new SerialPort
            {
                BaudRate = mCurrScale.baudRate,
                PortName = mCurrScale.com,
                DataBits = mCurrScale.dataByte,
                StopBits = StopBits.One,
                Parity = Parity.None,
            };
            //设置当前显示控制的解释器。
            mScaleDataFormarter = CommonFunction.SetInterpreter(mCurrScale.brandType);
        }

        protected void DisposeSerialPort()
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

        protected void DisposeTimer()
        {
            if (mTimer != null)
            {
                mTimer.Dispose();
                mTimer = null;
            }
        }
        #endregion

        /// <summary>
        /// 打印
        /// </summary>
        protected void PrintBill(WeightingBillType type)
        {
            bool auto = false;
            if (MyHelper.ConfigurationHelper.GetConfig(ConfigItemName.autoPrint.ToString()) == "true")
            {
                auto = true;
            }
            new PrintBillW(type, mWeighingBill, auto) { }.ShowDialog();
        }

        /// <summary>
        /// 刷新父窗口数据
        /// </summary>
        /// <param name="left"></param>
        /// <param name="center"></param>
        /// <param name="right"></param>
        protected void RefershParentData(bool left = false, bool center = false, bool right = false)
        {
            if (this.RefershParent != null)
            {
                RefershParent(left, center, right);
            }
        }

        #region Camera Info

        protected List<CHCNetSDK.NET_DVR_DEVICEINFO_V30> mDeviceInfors;
        protected List<System.Windows.Forms.PictureBox> mPictureBoxs;

        public void GetCameraInfo()
        {
            String @condition = CameraInfoEnum.client_id.ToString() + "=" + Constract.valueSplit + App.CurrClientId + Constract.valueSplit + " and " +
               CameraInfoEnum.company_id.ToString() + "=" + Constract.valueSplit + App.currentCompany.id + Constract.valueSplit + " and " +
                CameraInfoEnum.scale_id.ToString() + "=" + Constract.valueSplit + mCurrScale.id + Constract.valueSplit;
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.camera_info.ToString(), null, condition, null, null);
            mCameraInfos = DatabaseOPtionHelper.GetInstance().select<CameraInfo>(sql);
        }
        /// <summary>
        /// 显示摄像头
        /// </summary>        
        protected void ShowCamera(Border CameraBorder, StackPanel CameraStackPanel, IconButton settingVideoBtn)
        {
            if (mCameraInfos.Count <= 0)
            {
                settingVideoBtn.Visibility = Visibility.Visible;
                return;
            }
            CameraPanelWidth = CameraBorder.ActualWidth;
            CameraWidth = Math.Floor(CameraPanelWidth / mCameraInfos.Count);
            CameraHeight = CameraBorder.ActualHeight - 2;
            CameraStackPanel.Children.Clear();
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
                try
                {
                    CameraInfo camera = mCameraInfos[i];
                    System.Windows.Forms.Integration.WindowsFormsHost windowsFormsHost = new System.Windows.Forms.Integration.WindowsFormsHost() { Name = $"WFH{i}" };
                    System.Windows.Forms.PictureBox pictureBox = new System.Windows.Forms.PictureBox() { Name = $"pictureBox{i}", Width = (int)CameraWidth, Height = (int)CameraHeight };
                    mPictureBoxs.Add(pictureBox);
                    CHCNetSDK.NET_DVR_DEVICEINFO_V30 deviceinfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
                    //login
                    int cameraid = CameraHelper.loginCamera(camera.ip, camera.port, camera.userName, camera.password, ref deviceinfo);
                    if (cameraid <= -1)
                    {
                        throw new Exception("登陆失败");
                    }
                    CameraIds.Add(cameraid);
                    mDeviceInfors.Add(deviceinfo);
                    //preview
                    int ChanNum = deviceinfo.byChanNum;
                    int streamType = Convert.ToInt32(ConfigurationHelper.GetConfig(ConfigItemName.cameraStramType.ToString()));
                    bool res = CameraHelper.Preview(ref pictureBox, ChanNum, cameraid, streamType);
                    windowsFormsHost.Child = pictureBox;
                    CameraStackPanel.Children.Add(windowsFormsHost);
                }
                catch
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Width = CameraWidth;
                    //textBlock.Height = CameraHeight;
                    textBlock.TextAlignment = TextAlignment.Center;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.VerticalAlignment = VerticalAlignment.Bottom;
                    textBlock.Text = "视频加载失败";
                    textBlock.FontSize = 16;
                    textBlock.Foreground = System.Windows.Media.Brushes.Red;
                    CameraStackPanel.Children.Add(textBlock);
                }
            }
        }

        protected void LogoutCamera()
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

        protected void StorpPreviewCamera()
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

        protected void UpdateUsedBaseData()
        {
            // success to do TempUpdateUsedBase
            Thread thread = new Thread(new ParameterizedThreadStart(CommonFunction.TempUpdateUsedBase));
            thread.Start(new BaseDataClassV() { send = sendCompany, receive = receiverCompany, material = material, carInfo = car });
        }

        /// <summary>
        /// 通道截图
        /// </summary>
        protected void CaptureJpeg()
        {
            string filePath = ConfigurationHelper.GetConfig(ConfigItemName.cameraCaptureFilePath.ToString());
            if (String.IsNullOrEmpty(filePath))
            {
                filePath = System.IO.Path.Combine(FileHelper.GetRunTimeRootPath(), "capture");
            }
            String fileName = String.Empty;
            //根据登陆成功的通过截图
            for (int i = 0; i < CameraIds.Count; i++)
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

        /// <summary>
        /// 显示提示窗口
        /// </summary>
        /// <param name="content">提示内容</param>
        /// <param name="Title">标题</param>
        ///   /// <param name="orientation">方向</param>
        protected void ShowAlert(String content, String Title = "提示", Orientation orientation = Orientation.Horizontal)
        {
            MMessageBox.GetInstance().ShowBox(content, Title, MMessageBox.ButtonType.No, MMessageBox.IconType.error, orientation);
        }

        protected MMessageBox.Result ShowAlertResult()
        {
            return MMessageBox.GetInstance().ShowBox("保存成功 ! 要继续过磅吗？", "恭喜", MMessageBox.ButtonType.YesNo, MMessageBox.IconType.success, Orientation.Vertical, "是");
        }
    }
}
