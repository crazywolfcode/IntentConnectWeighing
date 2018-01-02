using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using MyHelper;

namespace IntentConnectWeighing
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static Window currWindow;
        public static System.Windows.Forms.NotifyIcon notifyIcon;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (CheckLogin() == true)
            {
                MainWindow window = new MainWindow();
                currWindow = window;
                window.Show();
            }
            else {
                Login loginWindow = new Login();
                currWindow = loginWindow;
                loginWindow.Show();
            }
            createNotifyIcon();

            createClientId();
        }

        private void createClientId() {
            if (string.IsNullOrEmpty(ConfigurationHelper.GetConfig(ConfigItemName.clientId.ToString()))) {
                ConfigurationHelper.SetConfig(ConfigItemName.clientId.ToString(), getClientId());
            }
        }

        private string getClientId() {
            return Guid.NewGuid().ToString();
        }

        public bool CheckLogin() {
            if (Constract.currentUser == null) {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region  Notify icon

        /// <summary>
        /// 创建Notification
        /// </summary>
        private void createNotifyIcon()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipTitle = "BalloonTipTitle";
            notifyIcon.BalloonTipText = "BalloonTipText intel connectation weighing" + ResourceHelper.getStringFromDictionaryResource(ResourceName.CompanyName);
            //notifyIcon.Icon = new System.Drawing.Icon("../../aislogo.ico");
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            notifyIcon.Visible = true;
            notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            notifyIcon.ShowBalloonTip(1000);
            notifyIcon.Text = ResourceHelper.getStringFromDictionaryResource(ResourceName.AppTitle);
            notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
            notifyIcon.Click += NotifyIcon_Click;
            notifyIcon.ContextMenu = getNotifyMenu();
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            if (currWindow.WindowState == WindowState.Minimized)
            {
                currWindow.WindowState = WindowState.Normal;
                currWindow.Activate();
            }
            else
            {
                currWindow.WindowState = WindowState.Minimized;
            }
        }

        private void NotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (currWindow.WindowState == WindowState.Minimized)
            {
                currWindow.WindowState = WindowState.Normal;
                currWindow.Activate();
            }
            else
            {
                currWindow.WindowState = WindowState.Minimized;
            }
        }

     

        /// <summary>
        /// 创建Notify icon Menu
        /// </summary>
        /// <returns></returns>
        public System.Windows.Forms.ContextMenu getNotifyMenu()
        {

            System.Windows.Forms.MenuItem[] notifyMenu;
            System.Windows.Forms.MenuItem quitMenuItem = new System.Windows.Forms.MenuItem();
            quitMenuItem.Text = ResourceHelper.getStringFromDictionaryResource(ResourceName.OptionQuit);
            quitMenuItem.DefaultItem = true;
            quitMenuItem.Click += QuitMenuItem_Click;
            notifyMenu = new System.Windows.Forms.MenuItem[] { quitMenuItem };
            return new System.Windows.Forms.ContextMenu(notifyMenu);
        }

        private void QuitMenuItem_Click(object sender, EventArgs e)
        {
            currWindow.Activate();
            MessageBoxResult result;
            result = MessageBox.Show("你确定退出程系吗", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
            else {
                currWindow.WindowState = WindowState.Normal;              
            }
        }
        #endregion
    }
}
