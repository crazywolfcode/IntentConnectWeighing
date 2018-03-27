using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IntentConnectWeighing.CameraSdk;

namespace IntentConnectWeighing
{
    /// <summary>
    /// OutWeighingPage.xaml 的交互逻辑
    /// </summary>
    public partial class OutWeighingPage : Page
    {
        public OutWeighingPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            //Camera();
        }

 
        //private void Camera() {
        //    String Ip = "180.130.175.145";
        //    String loginName = "admin";
        //    String Pwd = "txmy8888";
        //    String Port = "8011";
        //    if (CameraHelper.InitSDK()) {
        //        CHCNetSDK.NET_DVR_DEVICEINFO_V30 deviceInfro = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
        //        int res =CameraHelper.loginCamera(Ip,Port,loginName,Pwd,ref deviceInfro);
        //        if (res >= 0)
        //        {
        //            //System.Windows.Forms.PictureBox pb = this.previewFormsHost.Child as System.Windows.Forms.PictureBox;
        //            int chanip = deviceInfro.byChanNum;
        //            bool preview = CameraHelper.preview(ref pb, chanip, res);
        //            if (preview)
        //            {
        //                MessageBox.Show(" preview successed");
        //            }
        //            else
        //            {
        //                MessageBox.Show(" preview failure");
        //            }
        //        }
        //        else {
        //            MessageBox.Show(" login failure");
        //        }
        //    } else {
        //        MessageBox.Show(" CHC SDK init failure");
        //    }
            
        //}

  
    }
}
