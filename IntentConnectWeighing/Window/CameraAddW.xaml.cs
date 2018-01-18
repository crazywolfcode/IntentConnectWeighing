using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        private string ip = "180.130.175.145";
        private string port = "8010";
        private string userName = "admin";
        private string pwd = "txmy8888";
        #region Camera   
        public Int32 currCameraId = -1;
        public bool isInitSDK;
        public bool isLogin = false;
        public bool isPreviewSuccess = false;        
        #endregion
        public CameraAddW()
        {
            InitializeComponent();

        }
        public delegate void MyDebugInfo(string str);
        public void DebugInfo(string str)
        {
            ConsoleHelper.writeLine(str);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.IpTB.Text = "180.130.175.145";
            this.portTB.Text = "8010";
            this.userNameTB.Text = "admin";
            this.pwdTB.Text = "txmy8888";
            setPreviewImageHeight();
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            App.setCurrentWindow(this);
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
            if (isInitSDK && isPreviewSuccess) {
                CameraHelper.captureJpeg(Guid.NewGuid().ToString()+".jpg",currCameraId,1);
                CameraHelper.captureBmp(Guid.NewGuid().ToString() + ".bmp",currCameraId);
            }else{
                ConsoleHelper.writeLine("captureJpeg failured");
            }


            if (!checkInput())
            {
                CameraInfo camera = new CameraInfo();
                camera.clientId = ConfigurationHelper.GetConfig(ConfigItemName.clientId.ToString());
                camera.companyId = ConfigurationHelper.GetConfig(ConfigItemName.companyId.ToString());
                camera.status = 1;
                camera.isdelete = 0;
                camera.id = Guid.NewGuid().ToString();
               // string sql = DbBaseHelper.getSelectSqlNoSoftDeleteCondition();
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
                currCameraId = CameraHelper.loginCamera(ip,port,userName,pwd ,ref info);
                if (currCameraId >=0)
                {
                   System.Windows.Forms.PictureBox pb = this.previewFormsHost.Child as System.Windows.Forms.PictureBox;
                    int chanlnum = (int)info.byChanNum;
                    isPreviewSuccess= CameraHelper.preview( ref pb, chanlnum, currCameraId);
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
            if (string.IsNullOrEmpty(port))
            {
                MessageBox.Show("端口不能为空");
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
                MessageBox.Show("摄像头的登vi录密码不能为空");
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
                App.setCurrentWindow();
                App.ShowCurrentWindow();
            }
        }

        private void previewFormsHost_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("System.Windows.Forms.PictureBoxSystem.Windows.Forms.PictureBoxSystem.Windows.Forms.PictureBox");
        }
    }
}
