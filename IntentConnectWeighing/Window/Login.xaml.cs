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
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Data;
using MyHelper;

namespace IntentConnectWeighing
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login :Window
    {
        private User currentUser;
        private List<User> HistoryUsers;
        public Login()
        {
            this.Style = (Style)App.Current.Resources[ResourceName.LoginWindowStyle.ToString()];
            InitializeComponent();
            //缩放，最大化 等默认事件的修复
            WindowBehavior.newInstance(this).RepairWindowDefaultBehavior();
        }


        /// <summary>
        /// 窗口加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitTitleDragEvent();
            InitCloseBtn(this);
            this.HistoryUsers = LoginHelper.getHostoryUser();
            this.UserNameCb.ItemsSource = this.HistoryUsers;
            this.UserNameCb.DisplayMemberPath = "Name";
            this.UserNameCb.SelectedValuePath = "Id";

            //new Thread(new ThreadStart(ASynchronization)).Start();
            //initialization base config 初始基本配置
        }


        private void Window_ContentRendered(object sender, EventArgs e)
        {
            App.SetCurrentWindow(this);
        }

        public async void ASynchronization()
        {
            string url = HttpClientHelper.baseAddress + ConfigurationHelper.GetConfig("syncUp");
            DateTime uptime = DateTime.Now;
            ResponseContent response = await HttpClientHelper.GetAsync(url, true);
            //ResponseContent response = HttpWebRequestHelper.Post(url, "table=company");
            DateTime finshed = DateTime.Now;
            if (response == null)
            {
                MessageBox.Show("failure");
                return;
            }
            if (response.StatusCode == 1)
            {
                MessageBox.Show("ok" + response.ErrorMsg + " " + response.Data);
  
                Company coms = JsonHelper.JsonToObject(response.Data.ToString(), typeof(Company)) as Company;
                DatabaseOPtionHelper.GetInstance().insertOrUpdate(coms);
            }
            else
            {
                MessageBox.Show("failure" + response.ErrorMsg);
            }
            double time = DateTimeHelper.DateDifflMilliseconds(uptime, finshed);
            ConsoleHelper.writeLine(time + " 微秒");
            this.ASynchronization();
        }


        #region 窗口事件

        /// <summary>
        /// 绑定窗口的拖动和双击事件
        /// </summary>
        /// <param name="isCanDrag"></param>
        public void InitTitleDragEvent(bool isCanDrag = true)
        {
            if (isCanDrag == true)
            {
                Border windowTitle = (Border)TemplateHelper.GetControlTemplate(ResourceName.LoginWindowControlTemplate).FindName("windowTitle", this);
                windowTitle.MouseMove += WindowTitle_MouseMove;
                windowTitle.MouseDown += WindowTitle_MouseDown;
            }
        }
        /// <summary>
        /// 窗口移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WindowTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        int i = 0;
        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void WindowTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //如果是右键点击，直接返回
            if (e.RightButton == MouseButtonState.Pressed)
            {
                return;
            }

            i += 1;
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer.Tick += (s, e1) => { timer.IsEnabled = false; i = 0; };
            timer.IsEnabled = true;

            //如果不是双击，直接返回
            if (i % 2 != 0)
            {
                return;
            }
            timer.IsEnabled = false;
            i = 0;
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        /// <summary>
        /// 初始化关闭按键
        /// </summary>
        /// <param name="isShow"> 是否显示 </param>
        /// <param name="window">控件</param>
        public void InitCloseBtn(System.Windows.Window window, bool isShow = true)
        {
            WindowButton closeBtn = (WindowButton)TemplateHelper.GetControlTemplate(ResourceName.LoginWindowControlTemplate).FindName("CloseBtn", window);
            if (isShow == true)
            {
                closeBtn.Visibility = Visibility.Visible;
                closeBtn.Click += CloseBtn_Click;
            }
            else
            {
                closeBtn.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 关闭窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); //关闭窗口
        }
        #endregion

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            ////get data from db;
            //string sql = DbBaseHelper.getSelectSql("company",null,null);
            //// MessageBox.Show(sql);
            //SQLiteHelper sqliteHelper = new SQLiteHelper();
            //MySqlHelper helper = new MySqlHelper();
            // DataTable dt = sqliteHelper.ExcuteDataTable(sql, null);

            ////datatabole to list
            // List<Company> companys = DbBaseHelper.DataTableToList<Company>(dt);

            // string updateSql=string.Empty;
            //long ts = DateTimeHelper.GetTimeStamp();
            //foreach (Company cp in companys)
            //{
            //    cp.name = cp.name.Trim();
            //    cp.syncTime = ts;
            //    updateSql = DbBaseHelper.getUpdateSql<Company>(cp);              
            //   sqliteHelper.ExecuteNonQuery(updateSql, null);
            //}

            //ASynchronization();

            //   new Thread(new ThreadStart(updata)).Start();
            //PingReply reply = new Ping().Send("180.130.175.145");
            //ConsoleHelper.writeLine("reply.status:" + reply.Status);          
            //ConsoleHelper.writeLine("reply.Buffer:" + reply.Buffer);
            //ConsoleHelper.writeLine("reply.Address:" + reply.Address);
            //ConsoleHelper.writeLine("reply.RoundtripTime:" + reply.RoundtripTime);
            //ConsoleHelper.writeLine("reply.Options:" + reply.Options);
            //ConsoleHelper.writeLine("reply.Options.Ttl:" + reply.Options.Ttl);
            //MessageBox.Show(NetBaseHelper.getLocalConnectionStatus()+"");
          
            MessageBox.Show(BaiDuAcesessTokenHelper.getBaiDuAcesessToken());
           
        }

        public void Updata()
        {
            string url = HttpClientHelper.baseAddress + "/api/index/syncUp.html";
            //SQLiteHelper helper = new SQLiteHelper();
            //string sql = DbBaseHelper.getSelectSql("company");
            //DataTable dt = helper.ExcuteDataTable(sql, null);
            //string json = JsonHelper.ObjectToJson(dt);
            string postData = "table=company&data=" + "4444444444444444444";

            DateTime uptime = DateTime.Now;
            // ResponseContent response = HttpClientHelper.PostAsync(url, NetBaseHelper.getListKeyValuePAir(postData)).Result;           
            ResponseContent response = HttpWebRequestHelper.Post(url, postData);
            DateTime finshed = DateTime.Now;
            if (response != null)
            {
                MessageBox.Show("statusCode:" + response.StatusCode + " ErrorMsg: " + response.ErrorMsg);
            }
            double time = DateTimeHelper.DateDifflMilliseconds(uptime, finshed);
            ConsoleHelper.writeLine(time + " 微秒");
        }

        public void ASynchronizationTwo()
        {
            string url = HttpClientHelper.baseAddress + "/api/index/webtest.html";
            DateTime uptime = DateTime.Now;
            ResponseContent response = HttpWebRequestHelper.Post(url, "table=company&data=crazywolf000");
            DateTime finshed = DateTime.Now;
            if (response == null)
            {
                MessageBox.Show("failure");
                return;
            }
            if (response.StatusCode == 1)
            {
                MessageBox.Show("ok" + response.ErrorMsg + " " + response.Data);
                DataTable dt = JsonHelper.JsonToDataTable(response.Data.ToString()) as DataTable;
                foreach (DataRow row in dt.Rows)
                {
                    DatabaseOPtionHelper.GetInstance().update(row);
                }
            }
            else
            {
                MessageBox.Show("failure" + response.ErrorMsg);
            }
            double time = DateTimeHelper.DateDifflMilliseconds(uptime, finshed);
            ConsoleHelper.writeLine(time + " 微秒");
            this.ASynchronizationTwo();
        }

        /// <summary>
        /// 登录按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            //loginedUser user;

            //if (this.UserNameCb.SelectedItem != null)
            //{
            //    user = ((loginedUser)this.UserNameCb.SelectedItem);
            //}
            //else
            //{
            //    user = new loginedUser();
            //    user.Id = Guid.NewGuid().ToString();
            //    user.Name = this.UserNameCb.Text;
            //    user.Password = this.PasswordPB.Password;
            //}

            //if (this.RememberPasswordCb.IsChecked == true)
            //{
            //    user.IsRememberPassword = 1;
            //}
            //else
            //{
            //    user.IsRememberPassword = 0;
            //}
            //if (this.AutoLoginCb.IsChecked == true)
            //{
            //    user.IsAutoLogin = 1;
            //}
            //else
            //{
            //    user.IsAutoLogin = 0;
            //}
            //user.Phone = "180 8746 7482";
            //user.LastloginTime = DateTime.Now.ToShortDateString();
            //string userString = XmlHelper.Serialize(typeof(loginedUser), user);
            //if (!this.HistoryUsers.Contains(user))
            //{
            //    HistoryUsers.Add(user);
            //    DispatcherTimer dt = new DispatcherTimer(DispatcherPriority.Normal);
            //    dt.Tag = XmlHelper.Serialize(typeof(List<loginedUser>), HistoryUsers);
            //    dt.Tick += Dt_Tick;
            //    dt.Start();
            //}
            //Constract.currentUser = user;
            //MainWindow window = new MainWindow();
            //this.Close();
            //window.Show();              
            
            //TODO 将当前用户所在的公司 设置到当前的公司 里;

            AnimationHelper.getVibrationAnimation(this, Orientation.Vertical, Handle).Begin();
        }

        public void Handle(object sender, EventArgs e)
        {
            MainWindow window = new MainWindow();
            App.currWindow = window;
            this.Close();
            window.Show();
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            DispatcherTimer dt = (DispatcherTimer)sender;
            FileHelper.Write(FileHelper.GetRunTimeRootPath() + "/users.xml", dt.Tag.ToString());
            dt.Stop();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Name == "AutoLoginCb")
            {
                if (cb.IsChecked == true)
                {
                    if (this.RememberPasswordCb.IsChecked == false)
                    {
                        this.RememberPasswordCb.IsChecked = true;
                    }
                }
            }
            else if (cb.Name == "RememberPasswordCb")
            {
                if (cb.IsChecked == false)
                {
                    this.AutoLoginCb.IsChecked = false;
                }
            }

        }



        private void Window_StateChanged(object sender, EventArgs e)
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

        private void UserNameCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.UserNameCb.SelectedItem != null)
            {
                currentUser = (User)this.UserNameCb.SelectedItem;
            }
            else
            {
                currentUser = null;
            }
            //if (u.IsAutoLogin == 1)
            //{
            //    this.RememberPasswordCb.IsChecked = true;
            //    this.AutoLoginCb.IsChecked = true;
            //}
            //else if(u.IsRememberPassword == 1){
            //    this.RememberPasswordCb.IsChecked = true;
            //}
        }

        private void UserNameCb_TextChanged(object sender, RoutedEventArgs e)
        {
            if (this.UserNameCb.SelectedItem == null)
            {
                //  MessageBox.Show("UserNameCb_TextChanged  " + this.UserNameCb.Text);
            }
        }

        private void InitSettintBtn_Click(object sender, RoutedEventArgs e)
        {
            InitializationSettingW initW = new InitializationSettingW();
            initW.ShowDialog();
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            SelectVersionW w = new SelectVersionW();
            w.Show();           
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(App.currWindow == this) {
            this.WindowState = WindowState.Minimized;
            this.ShowInTaskbar = false;          
            e.Cancel = true;
            }
        }
    }
}
