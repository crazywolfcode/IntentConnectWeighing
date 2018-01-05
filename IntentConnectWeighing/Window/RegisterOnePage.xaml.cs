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
    /// RegisterOnePage.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterOnePage : Page
    {
        public Window parentWindow;
        
        public RegisterOnePage(Window win)
        {
            InitializeComponent();
            parentWindow = win;
        }

        private void sonCompanyRB_Checked(object sender, RoutedEventArgs e)
        {

            if (sonCompanyRB.IsChecked == true) {
                RegisterW w = (RegisterW)parentWindow;
                if (w.companyOrganizationType == CompanyOrganizationType.son.ToString())
                {
                   w.organizationChanged = false;
                }
                else {
                  w.organizationChanged = true;
                }
               w.companyOrganizationType = CompanyOrganizationType.son.ToString();
            }
        }

        private void primaryCompanyRB_Checked(object sender, RoutedEventArgs e)
        {
            if (primaryCompanyRB.IsChecked == true)
            {
                RegisterW w = (RegisterW)parentWindow;
                if (w.companyOrganizationType == CompanyOrganizationType.Primary.ToString())
                {
                 w.organizationChanged = false;
                }
                else
                {
                w.organizationChanged = true;
                }
                w.companyOrganizationType = CompanyOrganizationType.Primary.ToString();
            }
        }
    }
}
