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

        #region window event
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
        #endregion

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
            if (this.OtherSetting.IsChecked == true)
            {
                refreshOtherSettingData();
                return;
            }
            if (this.HighSetting.IsChecked == true)
            {
                refreshHeightSettingData();
                return;
            }
        }

        private void refreshBaseSettingData() { }
        private void refreshScaleSettingData()
        {
            String sql = DbBaseHelper.getSelectSql(DataTabeName.scale.ToString());
            DataTable dt = DatabaseOPtionHelper.GetInstance().select(sql);
            mScales = JsonHelper.DataTableToEntity<Scale>(dt);
            this.ScaleDataGrid.ItemsSource = mScales;
        }
        private void refreshCameraSettingData()
        {
            String sql = DbBaseHelper.getSelectSql(DataTabeName.camera_info.ToString());
            DataTable dt = DatabaseOPtionHelper.GetInstance().select(sql);
            mCameraInfos = JsonHelper.DataTableToEntity<CameraInfo>(dt);
            this.CamreaDataGrid.ItemsSource = mCameraInfos;
        }
       
        private void refreshOtherSettingData() { }
        private void refreshHeightSettingData() {
            //入库过磅毛重不可以删除
            if ("true" == ConfigurationHelper.GetConfig(ConfigItemName.noDeleteInGross.ToString()))
            {
                this.NoDeleteInGrossCB.IsChecked = true;
            }
            else
            {
                this.NoDeleteInGrossCB.IsChecked = false;
            }
            //不可以修改已经完成的数据
            if ("true" == ConfigurationHelper.GetConfig(ConfigItemName.noUpdateFinishedData.ToString()))
            {
                this.NoUpdateFinishedData.IsChecked = true;
            }
            else
            {
                this.NoUpdateFinishedData.IsChecked = false;
            }
            //
            if ("true" == ConfigurationHelper.GetConfig(ConfigItemName.allowDiffrenceMaterialWeighing.ToString()))
            {
                this.StartAautoPtint.IsChecked = true;
            }
            else
            {
                this.StartAautoPtint.IsChecked = false;
            }
            //
            if ("true" == ConfigurationHelper.GetConfig(ConfigItemName.allowDiffrenceCompany.ToString()))
            {
                this.AllowDiffrenceCompanyCB.IsChecked = true;
            }
            else
            {
                this.AllowDiffrenceCompanyCB.IsChecked = false;
            }
            //
            if ("true" == ConfigurationHelper.GetConfig(ConfigItemName.allowDiffrenceReceiveYard.ToString()))
            {
                this.AllowDiffrenceReceiveYardCB.IsChecked = true;
            }
            else
            {
                this.AllowDiffrenceReceiveYardCB.IsChecked = false;
            }
            //
            if ("true" == ConfigurationHelper.GetConfig(ConfigItemName.outFactoryAllowUpdate.ToString()))
            {
                this.OutfactoryAllowUpdateCB.IsChecked = true;
            }
            else
            {
                this.OutfactoryAllowUpdateCB.IsChecked = false;
            }
        }
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

   
        private void OtherSetting_Checked(object sender, RoutedEventArgs e)
        {
            refreshData();
        }
        #endregion

        #region Print Setting 打印
        private void PrintSetting_Checked(object sender, RoutedEventArgs e)
        {
            if ("true" == ConfigurationHelper.GetConfig(ConfigItemName.autoPrint.ToString()))
            {
                this.StartAautoPtint.IsChecked = true;
            }
            else
            {
                this.StartAautoPtint.IsChecked = false;
            }

            this.PrintTimes.Text = ConfigurationHelper.GetConfig(ConfigItemName.defaultPrintTimes.ToString());
        }

        private void StartAautoPtint_Checked(object sender, RoutedEventArgs e)
        {
            ConfigurationHelper.SetConfig(ConfigItemName.autoPrint.ToString(), "true");
        }

        private void StartAautoPtint_Unchecked(object sender, RoutedEventArgs e)
        {
            ConfigurationHelper.SetConfig(ConfigItemName.autoPrint.ToString(), "false");
        }

        private void PrintTimes_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                if (this.PrintTimes.Text.Length > 0 && this.PrintTimes.Text != ConfigurationHelper.GetConfig(ConfigItemName.defaultPrintTimes.ToString()))
                {
                    if (RegexHelper.IsNumber(this.PrintTimes.Text))
                    {
                        int times = Convert.ToInt32(this.PrintTimes.Text);
                        ConfigurationHelper.SetConfig(ConfigItemName.defaultPrintTimes.ToString(), times.ToString());
                    }
                    else
                    {
                        MessageBox.Show("输入的打印次数必须为整数！");
                    }
                }
            }
        }


        #endregion

        #region Height Setting 高级设置

 
        private void HighSetting_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded == false) { return; }
            refreshHeightSettingData();        
        }

        #region 入库过磅毛重不可戏删除
        private void NoDeleteInGrossCB_Checked(object sender, RoutedEventArgs e)
        {
            ConfigurationHelper.SetConfig(ConfigItemName.noDeleteInGross.ToString(), "true");
        }
        private void NoDeleteInGrossCB_Unchecked(object sender, RoutedEventArgs e)
        {
            ConfigurationHelper.SetConfig(ConfigItemName.noDeleteInGross.ToString(), "false");
        }
        #endregion

        #region 不可以修改已经完成的数据
        private void NoUpdateFinishedData_Checked(object sender, RoutedEventArgs e)
        {
            ConfigurationHelper.SetConfig(ConfigItemName.noUpdateFinishedData.ToString(), "true");
        }

        private void NoUpdateFinishedData_Unchecked(object sender, RoutedEventArgs e)
        {
            ConfigurationHelper.SetConfig(ConfigItemName.noUpdateFinishedData.ToString(), "false");
        }
        #endregion

        private void AllowDiffrenceMaterialWeighingCB_Checked(object sender, RoutedEventArgs e)
        {
            ConfigurationHelper.SetConfig(ConfigItemName.allowDiffrenceMaterialWeighing.ToString(), "true");
        }

        private void AllowDiffrenceMaterialWeighingCB_Unchecked(object sender, RoutedEventArgs e)
        {
            ConfigurationHelper.SetConfig(ConfigItemName.allowDiffrenceMaterialWeighing.ToString(), "false");
        }

        private void AllowDiffrenceCompanyCB_Checked(object sender, RoutedEventArgs e)
        {
            ConfigurationHelper.SetConfig(ConfigItemName.allowDiffrenceCompany.ToString(), "true");
        }

        private void AllowDiffrenceCompanyCB_Unchecked(object sender, RoutedEventArgs e)
        {
            ConfigurationHelper.SetConfig(ConfigItemName.allowDiffrenceCompany.ToString(), "false");
        }

        private void AllowDiffrenceReceiveYardCB_Checked(object sender, RoutedEventArgs e)
        {
            ConfigurationHelper.SetConfig(ConfigItemName.allowDiffrenceReceiveYard.ToString(), "true");
        }

        private void AllowDiffrenceReceiveYardCB_Unchecked(object sender, RoutedEventArgs e)
        {
            ConfigurationHelper.SetConfig(ConfigItemName.allowDiffrenceReceiveYard.ToString(), "false");
        }

        private void OutfactoryAllowUpdateCB_Checked(object sender, RoutedEventArgs e)
        {
            ConfigurationHelper.SetConfig(ConfigItemName.outFactoryAllowUpdate.ToString(), "true");
        }

        private void OutfactoryAllowUpdateCB_Unchecked(object sender, RoutedEventArgs e)
        {
            ConfigurationHelper.SetConfig(ConfigItemName.outFactoryAllowUpdate.ToString(), "false");
        }

        #endregion

    
    }
}
