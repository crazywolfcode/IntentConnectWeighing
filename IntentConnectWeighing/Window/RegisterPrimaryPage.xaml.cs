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
using Baidu.Aip.Ocr;
using Microsoft.Win32;
using MyHelper;
namespace IntentConnectWeighing
{
    /// <summary>
    /// RegisterPrimaryPage.xaml 的交互逻辑
    /// </summary>


    public partial class RegisterPrimaryPage : Page
    {
        private Window parentWindow;
        private Company mCompany = new Company();
        public RegisterPrimaryPage(Window win)
        {
            InitializeComponent();
            this.parentWindow = win;
            this.DataContext = mCompany;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            showPanel();
        }

        private void showPanel()
        {
            if (NetBaseHelper.IsConnectedToInternet() == true)
            {
                this.AutomaticRecognitionRB.IsChecked = true;
            }
            else
            {
                this.InputRB.IsChecked = true;
            }

        }

        private void InputRB_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void AutomaticRecognitionRB_Checked(object sender, RoutedEventArgs e)
        {

        }
        private string imagePath;
        private void choseImgBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                imagePath = dialog.FileName;
                filePathTB.Text = imagePath;
                licenseImage.Source = new BitmapImage(new Uri(imagePath));
                AutomaticRecognition();
            }
        }
        /// <summary>
        /// Automatic Recognition 开始自动识别
        /// </summary>
        private void AutomaticRecognition() {
            string apiKey = ConfigurationHelper.GetConfig(ConfigItemName.baiduApiKey.ToString());
            string apiScret = ConfigurationHelper.GetConfig(ConfigItemName.baiduApiSecretKey.ToString());
            Ocr ocr = new Ocr(apiKey,apiScret);         
            Newtonsoft.Json.Linq.JObject ob = ocr.BusinessLicense(FileHelper.getBytes(imagePath));
            BaiduLicenseRecognition baiduLicense = BaiduAipHelper.getLicenseRecognition(ob);
            bindingControlValue(baiduLicense);
        }

        private void bindingControlValue(BaiduLicenseRecognition recognition)
        {
            mCompany.name = recognition.companyName;
            mCompany.legalPerson = recognition.logalPerson;
            mCompany.liceseEspriseTime = recognition.expriseDate;
            mCompany.creditNumber = recognition.creditNumber;
            mCompany.busineseLincenseNumber = recognition.licenseNumber;
            mCompany.address = recognition.address;
        }

        private void licenseImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.licenseImage.Stretch = Stretch.None;
        }
    }
}
