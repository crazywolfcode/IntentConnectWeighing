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
using MyHelper;
namespace IntentConnectWeighing
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page
    {

        public BaseWindow ParentWindow;
        public HomePage()
        {
            InitializeComponent();
            this.CommandBindings.Add(TestCommand.ChangeContentCommandBinding);
            this.CommandBindings.Add(TestCommand.ShowNameCommandBinding);
        }

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ParentWindow != null)
            {
                ParentWindow.ChangeAlertNumber(ResourceName.MenuIndexNumber, 1, 1);
            }

        }


        private void ErrorButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(FileHelper.GetRunTimeRootPath());
            //if (this.ParentWindow != null)
            //{
            //    ParentWindow.changeAlertNumber(ResourceName.MenuIndexNumber, 1, 0);
            //}
        }

        public void ShowCompanyName()
        {
            MessageBox.Show(ResourceHelper.getStringFromDictionaryResource(ResourceName.CompanyName));
        }

        private void getIp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(CommomHelpre.getLoclIp());
        }



    }
}
