using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
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
    /// SettingW.xaml 的交互逻辑
    /// </summary>
    public partial class SettingW : Window
    {
        private List<CameraInfo> mCameraInfos;
        private List<Scale> mScales;
        public SettingW()
        {
            InitializeComponent();
        }

        public void InitializingEvent()
        {

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (this.IsLoaded)
            {
                refreshData();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            new WindowBehavior(this).RepairWindowDefaultBehavior();
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            refreshData();
        }
        private void WindowTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {      
            this.Close();
        }

        private void ConfigItemPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        #region 刷新数据

        private void refreshData()
        {
            if (!this.IsLoaded)
            {
                return;
            }
            if (this.BaseSetting.IsChecked == true)
            {
                refreshBaseSettingData();
                return;
            }
            if (this.ScaleSetting.IsChecked == true)
            {
                refreshScaleSettingData();
                return;
            }
            if (this.CameraSetting.IsChecked == true)
            {
                refreshCameraSettingData();
                return;
            }
            if (this.HighSetting.IsChecked == true)
            {
                refreshHighSettingData();
                return;
            }
            if (this.OtherSetting.IsChecked == true)
            {
                refreshOtherSettingData();
                return;
            }
        }

        private void refreshBaseSettingData() { }
        private void refreshScaleSettingData()
        {
            String sql = DbBaseHelper.getSelectSql(DataTabeName.scale.ToString());
            DataTable dt =  DatabaseOPtionHelper.GetInstance().select(sql);
            mScales = JsonHelper.DataTableToEntity<Scale>(dt);
            this.ScaleDataGrid.ItemsSource = mScales;
        }
        private void refreshCameraSettingData()
        {
            String sql = DbBaseHelper.getSelectSql(DataTabeName.camera_info.ToString());
            DataTable dt =  DatabaseOPtionHelper.GetInstance().select(sql);
            mCameraInfos = JsonHelper.DataTableToEntity<CameraInfo>(dt);
            this.CamreaDataGrid.ItemsSource = mCameraInfos;
        }
        private void refreshHighSettingData() { }
        private void refreshOtherSettingData() { }
        #endregion

        #region Camera info 摄像头

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            new CameraAddW(btn.Tag.ToString()).ShowDialog();
        }
        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            MessageBoxResult result = MessageBox.Show("你是否真在的删除该摄像头？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                String id = btn.Tag.ToString();
                if (String.IsNullOrEmpty(id))
                {
                    return;
                }
                String sql = DbBaseHelper.getDeleteSql(DataTabeName.camera_info.ToString(), CameraInfoEnum.id.ToString() + "=" + Constract.valueSplit + id + Constract.valueSplit);
                int res =  DatabaseOPtionHelper.GetInstance().delete(sql);
                if (res > 0)
                {
                    MessageBox.Show("删除成功！");
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }
            }
        }

        private void AddCameraBtn_Click(object sender, RoutedEventArgs e)
        {
            new CameraAddW().ShowDialog();
        }
        private void CamreaDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;    //设置行表头的内容值    
        }
        #endregion


        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string sql = MyHelper.DbBaseHelper.getSelectSql("company", null, null);
            System.Data.DataTable dt = new MyHelper.MySqlHelper().select(sql);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                MyHelper.ConsoleHelper.writeLine("name:" + dt.Columns[i].ColumnName + " type:" + dt.Columns[i].DataType.ToString());
            }
        }



        #region 磅秤 Scale

        private void AddScaleBtn_Click(object sender, RoutedEventArgs e)
        {
            new ScaleAddW().ShowDialog();
        }
        private void ScaleDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
        private void updateScaleBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            new ScaleAddW(btn.Tag.ToString()).ShowDialog();
        }

        private void bundingBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteScaleBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            MessageBoxResult result = MessageBox.Show("你是否真在的删除该磅秤？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                String id = btn.Tag.ToString();
                if (String.IsNullOrEmpty(id))
                {
                    return;
                }
                String sql = DbBaseHelper.getDeleteSql(DataTabeName.scale.ToString(), ScaleEnum.id.ToString() + "=" + Constract.valueSplit + id + Constract.valueSplit);
                int res = DatabaseOPtionHelper.GetInstance().delete(sql);
                if (res > 0)
                {
                    MessageBox.Show("删除成功！");
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }
            }
        }

        #endregion


        #region change the tab of setting item 切换设置选项卡 时刷新数据
        private void BaseSetting_Checked(object sender, RoutedEventArgs e)
        {
            refreshData();
        }

        private void ScaleSetting_Checked(object sender, RoutedEventArgs e)
        {
            refreshData();
        }

        private void CameraSetting_Checked(object sender, RoutedEventArgs e)
        {
            refreshData();
        }

        private void BalanceSetting_Checked(object sender, RoutedEventArgs e)
        {
            refreshData();
        }

        private void HighSetting_Checked(object sender, RoutedEventArgs e)
        {
            refreshData();
        }

        private void OtherSetting_Checked(object sender, RoutedEventArgs e)
        {
            refreshData();
        }
        #endregion
    }
}
