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
namespace IntentConnectWeighing
{
    /// <summary>
    /// RegisterW.xaml 的交互逻辑
    ///  RegisterW.xaml's interactive logical 
    /// </summary>
    public partial class SelectVersionW : Window
    {
        public string companyOrganizationType = string.Empty;
        public SelectVersionW()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            // judge windows state and is loaded
            if (this.IsLoaded)
            {
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
                          
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            App.SetCurrentWindow(this);
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

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ConfigurationHelper.GetConfig(ConfigItemName.softwareVersion.ToString()))) {
                MessageBox.Show("请选择软件的版本！");
                return;
            }
            RegisterW w = new RegisterW(null);
            w.Show();
            this.Close();
        }
        
        private void helperBT_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(" open broswer ");
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void selectVersionRB_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            string versionName = rb.Name.Replace("RB", "");
            ConfigurationHelper.SetConfig(ConfigItemName.softwareVersion.ToString(), versionName);            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.SetCurrentWindow();
            App.ShowCurrentWindow();            
        }
    }
}
