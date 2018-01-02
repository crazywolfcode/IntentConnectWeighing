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

namespace IntentConnectWeighing
{
    /// <summary>
    /// RegisterW.xaml 的交互逻辑
    ///  RegisterW.xaml's interactive logical 
    /// </summary>
    public partial class RegisterW : Window
    {
        public string companyOrganizationType = string.Empty;
        public RegisterW()
        {
            InitializeComponent();
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new RegisterOnePage(this);
            initializationBtn();
        }
        /// <summary>
        /// show or hiden option button
        /// </summary>
        private void initializationBtn()
        {
            upBtn.Visibility = Visibility.Collapsed;
            saveBtn.Visibility = Visibility.Collapsed;
            activationBtn.Visibility = Visibility.Collapsed;
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
            Page page;
            if (this.mainFrame.CanGoForward)
            {
                this.mainFrame.GoForward();
            }
            else
            {
                if (companyOrganizationType == CompanyOrganizationType.primary.ToString())
                {
                    page = new RegisterPrimaryPage(this);
                }
                else
                {
                    page = new RegisterSonPage(this);
                }
                this.mainFrame.Content = page;
            }
        }

        private void upBtn_Click(object sender, RoutedEventArgs e)
        {
            Page page;
            if (this.mainFrame.CanGoBack)
            {
                this.mainFrame.GoBack();
            }
            else
            {
                page = new RegisterOnePage(this);
                this.mainFrame.Content = page;
            }
        }

        /// <summary>
        /// save TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// activation son company TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void activationBtn_Click(object sender, RoutedEventArgs e)
        {

        }



        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
