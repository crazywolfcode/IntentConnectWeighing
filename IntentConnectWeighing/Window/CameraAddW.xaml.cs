using System;
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
using IntentConnectWeighing.CameraSdk;
using System.Runtime.InteropServices;
namespace IntentConnectWeighing
{
    /// <summary>
    /// CameraAddW.xaml 的交互逻辑
    ///  CameraAddW.xaml's interactive logical 
    /// </summary>
    public partial class CameraAddW : Window
    {
        private string ip = string.Empty;
        private string port = string.Empty;
        private string userName = string.Empty;
        private string pwd = string.Empty;
        private String mId;
        private CameraInfo mCameraInfo;
        #region Camera   
        public Int32 currCameraId = -1;
        public bool isInitSDK;
        public bool isLogin = false;
        public bool isPreviewSuccess = false;        
        #endregion
        public CameraAddW(String cameraId = null)
        {
            InitializeComponent();
            this.mId = cameraId;
        }
        public delegate void MyDebugInfo(string str);
        public void DebugInfo(string str)
        {
            ConsoleHelper.writeLine(str);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(mId)) {
                bindingCurrrCamera();
            }                        
            setPreviewImageHeight();
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            App.SetCurrentWindow(this);
        }

        private void bindingCurrrCamera() {
            String condition = CameraInfoEnum.id.ToString() + "=" + Constract.valueSplit + mId + Constract.valueSplit;
            String sql = DbBaseHelper.getSelectSql(DataTabeName.camera_info.ToString(), null, condition);

            DataTable dt = DatabaseOPtionHelper.GetInstance().select(sql);
            if (dt.Rows.Count > 0) {
                mCameraInfo =( JsonHelper.DataTableToEntity<CameraInfo>(dt))[0];
            }
            this.IpTB.Text = mCameraInfo.ip;
            this.portTB.Text = mCameraInfo.port;
            this.userNameTB.Text = mCameraInfo.userName;
            this.pwdTB.Text = mCameraInfo.password;
        }
        private void setPreviewImageHeight()
        {
            this.previewFormsHost.Height = this.previewStackPanel.ActualHeight;
            this.previewFormsHost.Width = this.previewStackPanel.ActualWidth;
        }
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
       
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            //抓图
            //if (isInitSDK && isPreviewSuccess) {
            //    CameraHelper.captureJpeg(Guid.NewGuid().ToString()+".jpg",currCameraId,1);
            //    CameraHelper.captureBmp(Guid.NewGuid().ToString() + ".bmp",currCameraId);
            //}else{
            //    ConsoleHelper.writeLine("captureJpeg failured");
            //}

            if (!IsInitialized) {
                MessageBox.Show("摄像头初始化失败！");
                return;
            }

            if (!isPreviewSuccess) {
                MessageBox.Show("保存摄像头之前必须先预览成功！");
                return;
            }

            if (checkInput())
            {
                String condition = String.Empty;
                String sql = string.Empty;
                if (!String.IsNullOrEmpty(mId)) {
                    //update
                    mCameraInfo.syncTime =(int) DateTimeHelper.GetTimeStamp();
                    mCameraInfo.ip = this.IpTB.Text.Trim();
                    mCameraInfo.port = this.portTB.Text.Trim();
                    mCameraInfo.userName = this.userNameTB.Text.Trim();
                    mCameraInfo.password = this.pwdTB.Text.Trim();
                    int res = DatabaseOPtionHelper.GetInstance().update(mCameraInfo);
                    if (res > 0)
                    {
                        MessageBox.Show("修改成功！");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("修改失败！");
                        return;
                    }
                } else {
                    //insert
                    CameraInfo camera = new CameraInfo();
                    string cid = ConfigurationHelper.GetConfig(ConfigItemName.clientId.ToString());                    
                    condition = CameraInfoEnum.client_id.ToString() + "=" + Constract.valueSplit + cid + Constract.valueSplit + " and " +
                        CameraInfoEnum.ip.ToString() + "=" + Constract.valueSplit + IpTB.Text.ToString() + Constract.valueSplit +
                        " and " + CameraInfoEnum.port.ToString() + "=" + Constract.valueSplit + this.portTB.Text.ToString() + Constract.valueSplit;
                    sql = DbBaseHelper.getSelectSqlNoSoftDeleteCondition(DbBaseHelper.getTableName(camera), null, condition, null, null, null, 1, 0);
                    DatabaseOPtionHelper optionHelper =  DatabaseOPtionHelper.GetInstance();
                    DataTable dt = optionHelper.select(sql);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("该摄像头已经存在，不要再添加拉！");
                        return;
                    }
                    else
                    {
                        camera.clientId = cid;
                        camera.companyId = ConfigurationHelper.GetConfig(ConfigItemName.companyId.ToString());
                        camera.status = 0;
                        camera.isDelete = 0;
                        camera.syncTime = 0;
                        camera.ip = this.IpTB.Text.Trim();
                        camera.port = this.portTB.Text.Trim();
                        camera.userName = this.userNameTB.Text.Trim();
                        camera.password = this.pwdTB.Text.Trim();
                        camera.id = Guid.NewGuid().ToString();
                        int res = optionHelper.insert(camera);
                        if (res > 0)
                        {
                            MessageBox.Show("保存成功！");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("保存失败！");
                            return;
                        }
                    }
                }

               
            }           
        }
        /// <summary>
        /// preview camera
        /// 1.inint sdk 
        /// 2.login cameera
        /// 3.preview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookBtn_Click(object sender, RoutedEventArgs e)
        {

            isInitSDK = CameraHelper.InitSDK();
            if (isInitSDK)
            {
                CHCNetSDK.NET_DVR_DEVICEINFO_V30 info = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
                currCameraId = CameraHelper.loginCamera(this.IpTB.Text.Trim(),this.portTB.Text.Trim(),this.userNameTB.Text.Trim(),this.pwdTB.Text.Trim() ,ref info);
                if (currCameraId >=0)
                {
                   System.Windows.Forms.PictureBox pb = this.previewFormsHost.Child as System.Windows.Forms.PictureBox;
                    int chanlnum = (int)info.byChanNum;
                    isPreviewSuccess= CameraHelper.preview( ref pb, chanlnum, currCameraId);
                    if (isPreviewSuccess)
                    {
                        MessageBox.Show("预览成功！");
                    }
                    else {
                        MessageBox.Show("预览失败！");
                    }
                }
            }
            else
            {
                MessageBox.Show("init failure");
            }          
            switchBtnVisbility();
        }

        private void nulookBtn_Click(object sender, RoutedEventArgs e)
        {
           CameraHelper.stopPreview(currCameraId);
            CameraHelper.loginOutCamera(currCameraId);
            System.Windows.Forms.PictureBox pb = this.previewFormsHost.Child as System.Windows.Forms.PictureBox;
            pb.Image = null;
            isPreviewSuccess = false;
            switchBtnVisbility();
        }

        private bool checkInput()
        {
            ip = this.IpTB.Text.Trim();
            port = this.portTB.Text.Trim();
            userName = this.userNameTB.Text.Trim();
            pwd = this.pwdTB.Text.Trim();
            if (string.IsNullOrEmpty(ip))
            {
                MessageBox.Show("摄像头的地址不能为空");
                this.IpTB.Focusable = true;
                return false;
            }
            if (!RegexHelper.IsIPv4(ip)) {
                MessageBox.Show("摄像头的地址不是正确的IPV4地址");
                this.IpTB.Focusable = true;
                return false;
            }
            if (string.IsNullOrEmpty(port))
            {
                MessageBox.Show("端口不能为空");
                this.portTB.Focusable = true;
                return false;
            }
            if (!RegexHelper.IsNumber(port)) {
                MessageBox.Show("端口只是是数字");
                this.portTB.Focusable = true;
                return false;
            }
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("摄像头的登录账号不能为空");
                this.IpTB.Focusable = true;
                return false;
            }
            if (string.IsNullOrEmpty(port))
            {
                MessageBox.Show("摄像头的登录密码不能为空");
                this.portTB.Focusable = true;
                return false;
            }
            return true;
        }

        private void switchBtnVisbility()
        {
            if (isPreviewSuccess)
            {
                lookBtn.Visibility = Visibility.Collapsed;
                this.nulookBtn.Visibility = Visibility.Visible;
            }
            else
            {
                lookBtn.Visibility = Visibility.Visible;
                this.nulookBtn.Visibility = Visibility.Collapsed;
            }
        }

 

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isInitSDK)
            {
                CHCNetSDK.NET_DVR_Cleanup();
            }
            if (App.currWindow == this)
            {
                App.SetCurrentWindow();
                App.ShowCurrentWindow();
            }
        }

        private void previewFormsHost_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("System.Windows.Forms.PictureBoxSystem.Windows.Forms.PictureBoxSystem.Windows.Forms.PictureBox");
        }
    }
}
