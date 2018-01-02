using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyHelper;

namespace IntentConnectWeighing
{
    /// <summary>
    /// InitializationSettingW.xaml 的交互逻辑
    /// </summary>
    public partial class InitializationSettingW : BaseWindow
    {
        private string currentDbType;
        public InitializationSettingW()
        {
            InitializeComponent();
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            base.myInitializeStyle(this, MyWindowsStyle.dialog);
            base.setWindowTitle(this, "初始化基本设置");
            base.setIcon(this, helpers.getImageSourceParth("Themes/Img/Icon/setting-ico.png"));
            getinitializationConfig();
        }

        private void getinitializationConfig()
        {
            currentDbType = ConfigurationHelper.GetConfig(ConfigItemName.dbType.ToString());
            if (currentDbType == DbType.sqlite.ToString())
            {
                SqliteRB.IsChecked = true;
            }
            else
            {
                MysqlRB.IsChecked = true;
            }
        }

        private void SaveMysqlBtn_Click(object sender, RoutedEventArgs e)
        {
            //string connstring = connStr.Text;
            //if (connstring.Length <= 0)
            //{
            //    MessageBox.Show("请填写所的配置项");
            //}
            //if (MySqlHelper.CheckConn(connstring))
            //{
            //    try
            //    {
            //        ConfigurationHelper.SetConnectionConfig(ConfigItemName.mysqlConn.ToString(), connstring);
            //        ConfigurationHelper.SetConfig(ConfigItemName.dbType.ToString(), DbType.mysql.ToString());
            //        AlertInfoTB.Text = "连接数据库成功";
            //        //disable save button
            //        SaveMysqlBtn.IsEnabled = false;
            //    }
            //    catch (Exception)
            //    {
            //        AlertInfoTB.Text = "无法连接数据库";
            //    }
            //}
        }

        private void buildConnString()
        {
            string dbname = DbName.Text.Trim();
            string address = IPAddress.Text.Trim();
            string userid = UserID.Text.Trim();
            string port = Port.Text.Trim();
            string pwd = Pwd.Text.Trim();
            if (string.IsNullOrEmpty(dbname) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(port)) { return; }
            string conn = MyHelper.MySqlHelper.buildConnectionString(address, dbname, userid, pwd, port);
            connStr.Text = conn;
        }

        private void Pwd_TextChanged(object sender, TextChangedEventArgs e)
        {
            buildConnString();

        }

        private void UserID_TextChanged(object sender, TextChangedEventArgs e)
        {
            buildConnString();
        }

        private void Port_TextChanged(object sender, TextChangedEventArgs e)
        {
            buildConnString();
        }

        private void DbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            buildConnString();
        }

        private void IPAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            buildConnString();
        }

        private void Mysql_Checked(object sender, RoutedEventArgs e)
        {
            if (MysqlRB.IsChecked == true)
            {
                string conn = ConfigurationHelper.GetConnectionConfig(ConfigItemName.sqliteConn.ToString());
                if (!string.IsNullOrEmpty(conn))
                {
                    connStr.Text = conn;
                    AlertInfoTB.Text = "数据库已经配置成功，建议不要轻易改变";
                }
            }
        }

        private void SqliteRB_Checked(object sender, RoutedEventArgs e)
        {
            string defaultconn = SQLiteHelper.createConnString(ConfigurationHelper.GetConfig(ConfigItemName.sqliteDbName.ToString()));
            SqliteDBNameTb.Text = ConfigurationHelper.GetConfig(ConfigItemName.sqliteDbName.ToString());
            SqlietConnStrTB.Text = defaultconn;
            if (string.IsNullOrEmpty(currentDbType))
            {
                SqliteAlertTB.Text = "数据库已经配置成功，建议不要轻易改变";
            }
        }

        private void SaveSqliteBtn_Click(object sender, RoutedEventArgs e)
        {
            string defaultconn = SqlietConnStrTB.Text;
            if (currentDbType == DbType.mysql.ToString())
            {
                MessageBoxResult result = MessageBox.Show("已经成功的配置拉Mysql数据库，是否要切换，切换可能导致原来的数据丢失！", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            if (SQLiteHelper.CheckConn(defaultconn))
            {
                currentDbType = DbType.sqlite.ToString();
                ConfigurationHelper.SetConnectionConfig(ConfigItemName.sqliteConn.ToString(), defaultconn);
                ConfigurationHelper.SetConfig(ConfigItemName.sqliteDbPath.ToString(), SQLiteHelper.getDbSavePath());
                ConfigurationHelper.SetConfig(ConfigItemName.dbType.ToString(),DbType.sqlite.ToString());
                SqliteAlertTB.Text = "Sqlite 数据库配置成功";
                //disable save button
                SaveSqliteBtn.IsEnabled = false;
            }
            else
            {
                SqliteAlertTB.Text = "Sqlite 数据库配置失败";
            }
        }
    }
}
