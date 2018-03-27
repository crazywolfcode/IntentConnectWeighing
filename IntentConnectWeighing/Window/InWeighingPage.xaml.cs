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

namespace IntentConnectWeighing
{
    /// <summary>
    /// InWeighingPage.xaml 的交互逻辑
    /// </summary>
    public partial class InWeighingPage : Page
    {
        #region Variable area

        #endregion
        public InWeighingPage()
        {
            InitializeComponent();
        }



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.GetSoftwareVersion() == SoftwareVersion.netConnection.ToString())
            {
                HideOrShowSendExpandePanel();
                FillCustomerSendData();
            }
            else
            {
                HideOrShowSendExpandePanel(false);
            }
        }

        /// <summary>
        /// hide or show the customer send area
        /// </summary>
        /// <param name="show"></param>
        private void HideOrShowSendExpandePanel(bool show = true)
        {
            if (show == true)
            {
                if (this.LeftBorder.Visibility != Visibility.Visible)
                {
                    this.LeftBorder.Visibility = Visibility.Visible;
                }
            }
            else
            {
                this.LeftBorder.Visibility = Visibility.Collapsed;
            }

        }

        #region  Fill customer send data 

        private void FillCustomerSendData()
        {
            
        }

        #endregion
        /// <summary>
        /// new input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewInPutBtn_Click(object sender, RoutedEventArgs e)
        {
            new InputWindow() { RefershParent = new Action<bool, bool, bool>(this.RefreshData) }.ShowDialog();
        }

        #region Refresh Data
        private void RefreshData(bool l = false, bool c = false, bool r = false)
        {
            if (l == true)
            {
                RefreshLeftData();
            }
            if (c == true)
            {
                RefreshCenterData();
            }
            if (r == true)
            {
                RefreshRightData();
            }
        }
        private void RefreshLeftData() { }
        private void RefreshCenterData() { }
        private void RefreshRightData() { }
        #endregion
    }
}
