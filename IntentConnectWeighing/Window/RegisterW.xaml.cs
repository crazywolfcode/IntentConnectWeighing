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
    public partial class RegisterW : Window
    {
        public string companyOrganizationType = string.Empty;
        public bool organizationChanged = false;
        public string currentRegisterStep = null;
        public RegisterW(string registerStep)
        {
            InitializeComponent();
            currentRegisterStep = registerStep;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            // judge windows state and is loaded
            if (this.IsLoaded)
            {
                initializationBtn();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(currentRegisterStep) || currentRegisterStep == CompanyRegisterStep.RegisterOnePage.ToString())
            {
                this.mainFrame.Content = new RegisterOnePage(this);
            }
            else if (currentRegisterStep == CompanyRegisterStep.RegisterPrimaryPage.ToString())
            {
                this.mainFrame.Content = new RegisterPrimaryPage(this);
            }
            else if (currentRegisterStep == CompanyRegisterStep.RegisterSonLicencePage.ToString())
            {
                this.mainFrame.Content = new RegisterSonLicencePage(this);
            }
            else if (currentRegisterStep == CompanyRegisterStep.RegisterPayPage.ToString())
            {
                this.mainFrame.Content = new RegisterPayPage(this);
            }
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            App.SetCurrentWindow(this);
            MessageBox.Show(BaiDuAcesessTokenHelper.getBaiDuAcesessToken());
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
        /// <summary>
        ///save and next 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            string tag = mainFrame.Tag.ToString();
            //save

            //next
            if (organizationChanged)
            {
                if (companyOrganizationType == CompanyOrganizationType.Primary.ToString())
                {
                    if (tag == CompanyRegisterStep.RegisterPrimaryPage.ToString())
                    {
                        this.mainFrame.Content = new RegisterPrimaryLicencePage(this);
                    }
                    else if (tag == CompanyRegisterStep.RegisterPrimaryLicencePage.ToString())
                    {
                        this.mainFrame.Content = new RegisterPayPage(this);
                    }
                    else if (tag == CompanyRegisterStep.RegisterPayPage.ToString())
                    {
                        this.mainFrame.Content = new RegisterFinishedPage(this);
                    }
                    else
                    {
                        this.mainFrame.Content = new RegisterPrimaryPage(this);
                    }
                }
                else
                {
                    if (tag == CompanyRegisterStep.RegisterSonPage.ToString())
                    {
                        this.mainFrame.Content = new RegisterSonLicencePage(this);
                    }
                    else if (tag == CompanyRegisterStep.RegisterSonLicencePage.ToString())
                    {
                        this.mainFrame.Content = new RegisterPayPage(this);
                    }
                    else if (tag == CompanyRegisterStep.RegisterPayPage.ToString())
                    {
                        this.mainFrame.Content = new RegisterFinishedPage(this);
                    }
                    else
                    {
                        this.mainFrame.Content = new RegisterSonPage(this);
                    }                
                }
            }
            else
            {
                if (this.mainFrame.CanGoForward)
                {
                    this.mainFrame.GoForward();
                }
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
        /// finished TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void finishedBtn_Click(object sender, RoutedEventArgs e)
        {
            new ActivationW().Show();
            this.Close();
        }


        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void mainFrame_ContentRendered(object sender, EventArgs e)
        {
            string tag = this.mainFrame.Content.ToString();
            string[] tags = tag.Split('.');
            if (tags.Length > 1)
            {
                mainFrame.Tag = tags[tags.Length - 1];
            }
            else
            {
                mainFrame.Tag = tag;
            }

            initializationBtn();
        }
        
        /// <summary>
        /// show or hiden option button
        /// </summary>
        private void initializationBtn()
        {
            string step = string.Empty;
            step = this.mainFrame.Tag != null ? this.mainFrame.Tag.ToString() : string.Empty;
            nextBtn.Visibility = Visibility.Visible;
            upBtn.Visibility = Visibility.Visible;
            finishedBtn.Visibility = Visibility.Visible;
            if (step == CompanyRegisterStep.RegisterOnePage.ToString())
            {
                upBtn.Visibility = Visibility.Collapsed;
                finishedBtn.Visibility = Visibility.Collapsed;
            }
            else if (step == CompanyRegisterStep.RegisterFinishedPage.ToString())
            {
                nextBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                finishedBtn.Visibility = Visibility.Collapsed;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (App.currWindow == this) {
                App.SetCurrentWindow();
                App.ShowCurrentWindow();
            }            
        }
    }
}
