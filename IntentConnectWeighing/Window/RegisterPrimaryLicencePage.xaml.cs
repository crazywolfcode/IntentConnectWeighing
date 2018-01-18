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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Baidu.Aip.Ocr;
using MyHelper;
namespace IntentConnectWeighing
{
    /// <summary>
    /// RegisterPrimaryLicencePage.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterPrimaryLicencePage : Page
    {
        private static Ocr mOcr;
        private Window parentWindow;
        private string imagePath;
        public RegisterPrimaryLicencePage(Window win)
        {
            InitializeComponent();
            parentWindow = win;
        }


        private void setImage()
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                BitmapImage image = new BitmapImage(new Uri(imagePath));
                this.LicenseImg.Source = image;
                this.LicenseImg.Stretch = Stretch.UniformToFill;
            }
        }

        private void choseImgBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                imagePath = dialog.FileName;
                filePathTB.Text = imagePath;
                setImage();
                //IDRecognition();
                //bankCardRecobnition();
                //driverLicenseRecognition();
                //vehicleLicenseRecognition();
            }
        }
        /// <summary>
        /// Automatic Recognition 开始自动识别
        /// </summary>
        private void IDRecognition()
        {
            string apiKey = ConfigurationHelper.GetConfig(ConfigItemName.baiduApiKey.ToString());
            string apiScret = ConfigurationHelper.GetConfig(ConfigItemName.baiduApiSecretKey.ToString());
            Ocr ocr = new Ocr(apiKey, apiScret);
            Newtonsoft.Json.Linq.JObject ob = ocr.Idcard(FileHelper.getBytes(imagePath), "front");
            //BaiduLicenseRecognition baiduLicense = BaiduAipHelper.getLicenseRecognition(ob);   
            ConsoleHelper.writeLine(ob.ToString());
            MessageBox.Show(ob.ToString());
        }

        private void bankCardRecobnition()
        {
            Newtonsoft.Json.Linq.JObject ob = getOcr().Bankcard(FileHelper.getBytes(imagePath));
            BaiduBandCardRecognition card = BaiduAipHelper.getBandCardRecognition(ob);

        }

        private void driverLicenseRecognition()
        {
            Newtonsoft.Json.Linq.JObject ob = getOcr().DrivingLicense(FileHelper.getBytes(imagePath));
            BaiduDriverLicenseRecognition driver = BaiduAipHelper.getDriverRecognition(ob);
            MessageBox.Show(driver.name);
        }

        private void vehicleLicenseRecognition()
        {
            Newtonsoft.Json.Linq.JObject ob = getOcr().VehicleLicense(FileHelper.getBytes(imagePath));
            baiduVehicleRecognition vehicle = BaiduAipHelper.getvehicleRecognition(ob);
            MessageBox.Show(vehicle.engineNumber + " :" + vehicle.carNumber);
        }
        private Ocr getOcr()
        {
            if (mOcr == null)
            {
                string apiKey = ConfigurationHelper.GetConfig(ConfigItemName.baiduApiKey.ToString());
                string apiScret = ConfigurationHelper.GetConfig(ConfigItemName.baiduApiSecretKey.ToString());
                mOcr = new Ocr(apiKey, apiScret);
            }
            return mOcr;
        }
    }
}
