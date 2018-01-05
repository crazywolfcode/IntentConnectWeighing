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
using System.Windows.Shapes;

namespace IntentConnectWeighing
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : BaseWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            this.CommandBindings.Add(ShowSettingWindowsCommand.commandBinding);
        }
        private void BaseWindow_Activated(object sender, EventArgs e)
        {
            if (this.IsLoaded && this.WindowState != WindowState.Minimized) {
                //refresh all data TODO
            }
        }

        // 窗口加载
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            base.myInitializeStyle(this, MyWindowsStyle.main);
            centerNavMenu();      
            HomePage index = new HomePage();
            index.ParentWindow = this;
            this.MainFrame.Navigate(index);            
        }

        private void BaseWindow_ContentRendered(object sender, EventArgs e)
        {
            App.setCurrentWindow(this);

            //create sync_table info 
            // wait 1 minutes start Synchronization sync_up、 sync_down、up_bill
        }
        /// <summary>
        /// 将导航菜单居中
        /// </summary>
        private void centerNavMenu()
        {
            double windowWidth = this.Body.ActualWidth;
            double navMenuWidth = this.MenuStackPanel.ActualWidth;
            double LogoStackPanelWidth = this.LogoStackPanel.ActualWidth;
            this.MenuStackPanel.Margin = new Thickness((this.Body.ActualWidth - this.MenuStackPanel.ActualWidth) / 2 - LogoStackPanelWidth, 0, 0, 0);
        }


        private void Body_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            centerNavMenu();
        }

        //导航
        private void NavGroupRadioButton_Click(object sender, RoutedEventArgs e)
        {
            string pageAddress = string.Empty;
            RadioButton button = sender as RadioButton;
            if (button.IsChecked == true)
            {
                pageAddress = "/IntentConnectWeighing;component/Window/pageAddress.xaml".Replace("pageAddress", button.Name + "Page");
                this.MainFrame.Source = new Uri(pageAddress, UriKind.RelativeOrAbsolute);
                App.Current.Resources[ResourceName.MenuIndexNumber.ToString()] = 12;
                int a = ResourceHelper.getIntFromDictionaryResource(ResourceName.MenuIndexNumber);
                switch (button.Name)
                {
                    case "NavIndex":

                        break;
                    case "NavWeighing":

                        break;
                    case "NavReport":

                        break;
                    case "NavBaseData":

                        break;
                    default:

                        break;
                }
            }
        }

        private void userHeaderImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.userMenuPopup.IsOpen == false)
            {
                this.userMenuPopup.IsOpen = true;
            }
            else { this.userMenuPopup.IsOpen = false; }
        }

        private void quitBtn_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void BaseWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
            else
            {
                this.ShowInTaskbar = true;
            }
        }
   
        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = WindowState.Minimized;
            App.notifyIcon.BalloonTipTitle = "minimiaced ";
            App.notifyIcon.BalloonTipText = "minimiaced  in there";
        }
       
    }
}
