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
using MyHelper;
namespace IntentConnectWeighing
{
    /// <summary>
    /// RegisterPrimaryPage.xaml 的交互逻辑
    /// </summary>


    public partial class RegisterPrimaryPage : Page
    {
        private Window parentWindow;
        public RegisterPrimaryPage(Window win)
        {
            InitializeComponent();
            this.parentWindow = win;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string licence = string.Empty;
            licence = FileHelper.Reader(FileHelper.GetProjectRootPath() + "/software_using_protocol.txt");
            Run r = this.licenceText.FindName("licenceContent") as Run;
            r.Text = licence;
        }
    }
}
