﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using MyHelper;
using Baidu.Aip;

namespace IntentConnectWeighing
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static User currentUser;
        public static Window currWindow;
        public static Window prevWindow;
        public static System.Windows.Forms.NotifyIcon notifyIcon;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            createNotifyIcon();

            createClientId();

            //devlepment
            new SettingW().Show();

            //navigation();
        }

        private void navigation() {
            string registerStep = "";
            if (string.IsNullOrWhiteSpace(ConfigurationHelper.GetConfig(ConfigItemName.softwareVersion.ToString())))
            {
                SelectVersionW selectW = new SelectVersionW();
                registerStep = ConfigurationHelper.GetConfig(ConfigItemName.companyRegisterStep.ToString());
                selectW.Show();
            }
            else if (registerStep != CompanyRegisterStep.RegisterFinishedPage.ToString())
            {
                new RegisterW(registerStep).Show();
            }
            else if (CheckLogin() == true)
            {
                MainWindow win = new MainWindow();
                win.Show();
            }
            else
            {
                Login loginWindow = new Login();
                loginWindow.Show();
            }
        }

        private void createClientId()
        {
            if (string.IsNullOrEmpty(ConfigurationHelper.GetConfig(ConfigItemName.clientId.ToString())))
            {
                ConfigurationHelper.SetConfig(ConfigItemName.clientId.ToString(), getClientId());
            }
        }

        private string getClientId()
        {
            return Guid.NewGuid().ToString();
        }

        public bool CheckLogin()
        {
            if (Constract.currentUser == null)
            {
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
            ShowCurrentWindow();
        }

        private void NotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ShowCurrentWindow();
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
            else
            {
                currWindow.WindowState = WindowState.Normal;
            }
        }
        #endregion

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ConsoleHelper.writeLine("There are unhandled exceptions in the program");
        }
        /// <summary>
        /// save the config file's config Item to database 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //DateTime start = DateTime.Now;
            //    try
            //    {
            //        insertOrUpdateConnectionStrings();
            //}
            //    catch (Exception exception)
            //    {
            //        ConsoleHelper.writeLine("save app ConnectionStrings to dabase error: " + exception.Message);
            //    }
            //try
            //{
            insertOrUpdateAppSettings();
            //}
            //catch (Exception exception)
            //{
            //    ConsoleHelper.writeLine("save AppSettings to dabase error: " + exception.Message);
            //}
            //double time = DateTimeHelper.DateDifflMilliseconds(start, DateTime.Now);

            //ConsoleHelper.writeLine("suer time :" + time + " ms");

        }
        /// <summary>
        /// insert Or Update Connection Strings
        /// </summary>
        private void insertOrUpdateConnectionStrings()
        {
            ConnectionStringSettingsCollection conns = ConfigurationManager.ConnectionStrings;
            DatabaseOPtionHelper helper = null;
            string sql = string.Empty;
            for (int i = 0; i < conns.Count; i++)
            {
                Config config = null;
                if (!conns[i].Name.Contains("Local"))
                {
                    sql = DbBaseHelper.getSelectSql("config", null, "client_id =' " + ConfigurationHelper.GetConfig(ConfigItemName.clientId.ToString()) + "' and config_name = ' " + conns[i].Name + "'", null, null, null, 1);
                    if (helper == null)
                    {
                        helper = new DatabaseOPtionHelper();
                    }
                    DataTable dt = helper.select(sql);
                    if (dt.Rows.Count > 0)
                    {
                        config = JsonHelper.JsonToObject(JsonHelper.ObjectToJson(dt.Rows[0]), typeof(Config)) as Config;
                        if (config != null)
                        {
                            if (config.configValue != conns[i].ConnectionString)
                            {
                                config.configValue = conns[i].ConnectionString;
                                config.syncTime = DateTimeHelper.ConvertDateTimeToInt(DateTime.Now);
                                config.lastUpdateTime = DateTimeHelper.getCurrentDateTime();
                                if (App.currentUser != null)
                                {
                                    config.lastUpdateUserId = config.addUserId;
                                    config.lastUpdateUserName = config.addUserName;
                                }
                            }
                            helper.update(config);
                        }
                        else
                        {
                            //conveter error
                        }
                    }
                    else
                    {
                        config = new Config();
                        config.id = Guid.NewGuid().ToString();
                        config.clientId = ConfigurationHelper.GetConfig(ConfigItemName.clientId.ToString());
                        config.configName = conns[i].Name;
                        config.configValue = conns[i].ConnectionString;
                        config.addtime = DateTimeHelper.getCurrentDateTime();
                        config.configType = (int)ConfigType.ClientAppConfig;
                        config.lastUpdateTime = config.addtime;
                        config.syncTime = DateTimeHelper.ConvertDateTimeToInt(DateTime.Now);
                        if (App.currentUser != null)
                        {
                            config.addUserId = App.currentUser.id;
                            config.addUserName = App.currentUser.name;
                            config.lastUpdateUserId = config.addUserId;
                            config.lastUpdateUserName = config.addUserName;
                        }
                        helper.insert(config);
                    }

                }
            }
        }
        /// <summary>
        /// insert Or Update App Settings
        /// </summary>
        private void insertOrUpdateAppSettings()
        {
            DatabaseOPtionHelper helper = null;
            string sql = string.Empty;
            NameValueCollection collection = ConfigurationManager.AppSettings;
            string[] keys = collection.AllKeys;
            foreach (string key in keys)
            {
                Config config = null;
                if (helper == null)
                {
                    helper = new DatabaseOPtionHelper();
                }
                sql = DbBaseHelper.getSelectSql("config", null, "client_id ='" + ConfigurationHelper.GetConfig(ConfigItemName.clientId.ToString()) + "' and config_name = '" + key + "'", null, null, null, 1);
                DataTable dt = helper.select(sql);
                if (dt.Rows.Count > 0)
                {
                    
                    List<Config> configs = JsonHelper.TableToEntity<Config>(dt);
                    if (configs[0]!= null)
                    {
                        config = configs[0];
                        if (config.configValue != collection[key].ToString())
                        {
                            config.configValue = collection[key].ToString();
                            config.syncTime = DateTimeHelper.ConvertDateTimeToInt(DateTime.Now);
                            config.lastUpdateTime = DateTimeHelper.getCurrentDateTime();
                            if (App.currentUser != null)
                            {
                                config.lastUpdateUserId = config.addUserId;
                                config.lastUpdateUserName = config.addUserName;
                            }
                        }
                        helper.update(config);
                    }
                    else
                    {
                        //conveter error
                    }
                }
                else
                {
                    config = new Config();
                    config.id = Guid.NewGuid().ToString();
                    config.addtime = DateTimeHelper.getCurrentDateTime();
                    config.configName = key;
                    config.clientId = ConfigurationHelper.GetConfig(ConfigItemName.clientId.ToString());
                    config.configValue = collection[key].ToString();
                    config.configType = (int)ConfigType.ClientAppConfig;
                    config.lastUpdateTime = config.addtime;
                    config.syncTime = DateTimeHelper.ConvertDateTimeToInt(DateTime.Now);
                    if (App.currentUser != null)
                    {
                        config.addUserId = App.currentUser.id;
                        config.addUserName = App.currentUser.name;
                        config.lastUpdateUserId = config.addUserId;
                        config.lastUpdateUserName = config.addUserName;
                    }
                    helper.insert(config);
                }

            }
        }
        /// <summary>
        /// Show Current Window
        /// </summary>
        public static void ShowCurrentWindow()
        {
            if (currWindow == null) {
                return;
            }            
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
        /// set the current window
        /// </summary>
        /// <param name="win"></param>
        public static void setCurrentWindow(Window win = null)
        {
            if (win == null)
            {
                currWindow = prevWindow;
            }
            else
            {
                if (currWindow == null)
                { currWindow = prevWindow = win; }
                else
                {
                    prevWindow = currWindow;
                    currWindow = win;
                }
            }
        }

    }
}
