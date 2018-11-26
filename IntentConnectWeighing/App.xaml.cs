using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using MyHelper;
using Baidu.Aip;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Threading;
using MyCustomControlLibrary;
namespace IntentConnectWeighing
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application,ScannerGunInterface
    {
        public static User currentUser;
        public static Company currentCompany;
        public static Yard currentYard;
        public static Window currWindow;
        public static Window prevWindow;
        public static String CurrClientId;
        public static string SoftwareVersion;
        public static System.Windows.Forms.NotifyIcon notifyIcon;
 
        #region 本机使用的临时基础数据
        public static Dictionary<String, Company> tempSupplyCompanys = new Dictionary<string, Company>();
        public static Dictionary<String, Company> tempCustomerCompanys = new Dictionary<string, Company>();
        public static Dictionary<String, Material> tempMaterials = new Dictionary<string, Material>();
        public static Dictionary<String, CarInfo> tempCars = new Dictionary<string, CarInfo>();
        public static List<String> decuationDesList = new List<string>() { "扣水", "扣杂物"};
        public static List<String> inputRemarkList = new List<string>() {};
        public static List<String> outputRemarkList = new List<string>() {};
        #endregion
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            CreateNotifyIcon();

            CreateClientId();
            
            //devlepment
            new MainWindow().Show();
           
            // new TestExpender().Show();
            //Navigation();
            currentCompany = new Company()
            {
                id = ConfigurationHelper.GetConfig(ConfigItemName.companyId.ToString()),
                name = ConfigurationHelper.GetConfig(ConfigItemName.companyName.ToString()),
            };
            currentUser = new User()
            {
                id = GetClientId(),
                name = "陈龙飞",
                company = currentCompany.name,
                affiliatedCompanyId = currentCompany.id
            };
            currentYard = YardModel.GetById(MyHelper.ConfigurationHelper.GetConfig(ConfigItemName.yardId.ToString()));
            //Scanner Gun
            StartKeyBoardHook();

            //UI线程未捕获异常处理事件（UI主线程）
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void StartKeyBoardHook() {
            ScannerGunHelper.GetInstance(this).Start();
        }

        private void Navigation()
        {
            string registerStep = "";
            if (string.IsNullOrEmpty(ConfigurationHelper.GetConfig(ConfigItemName.softwareVersion.ToString())))
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


        private void CreateClientId()
        {
            if (string.IsNullOrEmpty(ConfigurationHelper.GetConfig(ConfigItemName.clientId.ToString())))
            {
                CurrClientId = GetClientId();
                ConfigurationHelper.SetConfig(ConfigItemName.clientId.ToString(), CurrClientId);
            }
            else
            {
                CurrClientId = ConfigurationHelper.GetConfig(ConfigItemName.clientId.ToString());
            }
        }

        public string GetClientId()
        {
            return Guid.NewGuid().ToString();
        }

        public bool CheckLogin()
        {
            if (currentUser == null)
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
        private void CreateNotifyIcon()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                BalloonTipTitle = "BalloonTipTitle",
                BalloonTipText = "BalloonTipText intel connectation weighing" + ResourceHelper.getStringFromDictionaryResource(ResourceName.CompanyName),
                //notifyIcon.Icon = new System.Drawing.Icon("../../aislogo.ico");
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath),
                Visible = true,
                BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
            };
           
            notifyIcon.ShowBalloonTip(1000);
            notifyIcon.Text = ResourceHelper.getStringFromDictionaryResource(ResourceName.AppTitle);
            notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
            notifyIcon.Click += NotifyIcon_Click;
            notifyIcon.ContextMenu = GetNotifyMenu();
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
        public System.Windows.Forms.ContextMenu GetNotifyMenu()
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
   
            MMessageBox.Result result = MMessageBox.GetInstance().ShowBox("你确定退出程系吗", "提示",MMessageBox.ButtonType.YesNo,MMessageBox.IconType.Info);
            if (result == MMessageBox.Result.Yes)
            {
                Application.Current.Shutdown();
            }
            else
            {
                currWindow.WindowState = WindowState.Normal;
            }
        }
        #endregion

        /// <summary>
        /// save the config file's config Item to database 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            ScannerGunHelper.Close();
          
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
            InsertOrUpdateAppSettings();
            SaveTempData();
            CommonFunction.UpdateUsedBaseData();
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
        private void InsertOrUpdateConnectionStrings()
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
                        helper = DatabaseOPtionHelper.GetInstance();
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
                                config.updateTime = DateTimeHelper.getCurrentDateTime();
                                if (App.currentUser != null)
                                {
                                    config.updateUserId = config.addUserId;
                                    config.updateUserName = config.addUserName;
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
                        config.updateTime = config.addtime;
                        config.syncTime = DateTimeHelper.ConvertDateTimeToInt(DateTime.Now);
                        if (App.currentUser != null)
                        {
                            config.addUserId = App.currentUser.id;
                            config.addUserName = App.currentUser.name;
                            config.updateUserId = config.addUserId;
                            config.updateUserName = config.addUserName;
                        }
                        helper.insert(config);
                    }
                }
            }
        }
        /// <summary>
        /// insert Or Update App Settings
        /// </summary>
        private void InsertOrUpdateAppSettings()
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
                    helper = DatabaseOPtionHelper.GetInstance();
                }
                sql = DbBaseHelper.getSelectSql("config", null, "client_id ='" + ConfigurationHelper.GetConfig(ConfigItemName.clientId.ToString()) + "' and config_name = '" + key + "'", null, null, null, 1);
                DataTable dt = helper.select(sql);
                if (dt.Rows.Count > 0)
                {
                    List<Config> configs = DbBaseHelper.DataTableToEntitys<Config>(dt);
                    if (configs[0] != null)
                    {
                        config = configs[0];
                        if (config.configValue != collection[key].ToString())
                        {
                            config.configValue = collection[key].ToString();
                            config.syncTime = DateTimeHelper.ConvertDateTimeToInt(DateTime.Now);
                            config.updateTime = DateTimeHelper.getCurrentDateTime();
                            if (App.currentUser != null)
                            {
                                config.updateUserId = config.addUserId;
                                config.updateUserName = config.addUserName;
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
                    config = new Config
                    {
                        id = Guid.NewGuid().ToString(),
                        addtime = DateTimeHelper.getCurrentDateTime(),
                        configName = key,
                        clientId = ConfigurationHelper.GetConfig(ConfigItemName.clientId.ToString()),
                        configValue = collection[key].ToString(),
                        configType = (int)ConfigType.ClientAppConfig
                    };
                    config.updateTime = config.addtime;
                    config.syncTime = DateTimeHelper.ConvertDateTimeToInt(DateTime.Now);
                    if (App.currentUser != null)
                    {
                        config.addUserId = App.currentUser.id;
                        config.addUserName = App.currentUser.name;
                        config.updateUserId = config.addUserId;
                        config.updateUserName = config.addUserName;
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
            if (currWindow == null)
            {
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
        public static void SetCurrentWindow(Window win = null)
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

        public static String GetSoftwareVersion()
        {
            if (String.IsNullOrEmpty(SoftwareVersion))
            {
                SoftwareVersion = ConfigurationHelper.GetConfig(ConfigItemName.softwareVersion.ToString());
            }
            return SoftwareVersion;
        }
        /// <summary>
        /// 保存本机使用过的基础数据
        /// </summary>
        private void SaveTempData() { }

        #region ScannerGunInterface

        public void ScanedFinished(string result)
        {
            MyHelper.ConsoleHelper.writeLine(" result:" + result);
        }
        #endregion


        #region 全局异常处理 TODO

        //UI线程未捕获异常处理事件（UI主线程）
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            string msg = String.Format("{0}\n\n{1}", ex.Message, ex.StackTrace);//异常信息 和 调用堆栈信息
            MessageBox.Show(msg, "UI线程异常");

            e.Handled = true;//表示异常已处理，可以继续运行
        }

        //非UI线程未捕获异常处理事件
        //如果UI线程异常DispatcherUnhandledException未注册，则如果发生了UI线程未处理异常也会触发此异常事件
        //此机制的异常捕获后应用程序会直接终止。没有像DispatcherUnhandledException事件中的Handler=true的处理方式，可以通过比如Dispatcher.Invoke将子线程异常丢在UI主线程异常处理机制中处理
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                string msg = String.Format("{0}\n\n{1}", ex.Message, ex.StackTrace);//异常信息 和 调用堆栈信息
                MessageBox.Show(msg, "非UI线程异常");
            }
        }
        //Task线程内未捕获异常处理事件
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            string msg = String.Format("{0}\n\n{1}", ex.Message, ex.StackTrace);
            MessageBox.Show(msg, "Task异常");
        }

        //异常处理 封装
        private void OnExceptionHandler(Exception ex)
        {
            if (ex != null)
            {
                string errorMsg = "";
                if (ex.InnerException != null)
                {
                    errorMsg += String.Format("【InnerException】{0}\n{1}\n", ex.InnerException.Message, ex.InnerException.StackTrace);
                }
                errorMsg += String.Format("{0}\n{1}", ex.Message, ex.StackTrace);

                ConsoleHelper.writeLine(errorMsg);
            }
        }
        #endregion
    }
}
