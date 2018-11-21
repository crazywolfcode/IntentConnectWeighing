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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyHelper;

namespace IntentConnectWeighing
{
    /// <summary>
    /// InWeighingPage.xaml 的交互逻辑
    /// </summary>
    public partial class InWeighingPage : Page
    {
        #region Variable area 
        public bool CurrentWeighingBillIsSendBill = false;
        public WeighingBill mWeighingBill;
        #endregion
        public InWeighingPage()
        {
            InitializeComponent();
            this.CommandBindings.Add(InFactoryComand.CommandBinding);
        }



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.GetSoftwareVersion() == SoftwareVersion.netConnection.ToString() || App.GetSoftwareVersion() == SoftwareVersion.netSingleBusiness.ToString())
            {
                HideOrShowSendExpandePanel(true);
                FillCustomerSendData();
            }
            else
            {
                HideOrShowSendExpandePanel();
            }

            ShowCurrentPanel();
            RefreshData(false, false, true);
            RefreshFinishedData();
        }
        /// <summary>
        /// 显示当前数据区
        /// </summary>
        private void ShowCurrentPanel()
        {
            this.EmptyPanel.Visibility = Visibility.Collapsed;
            this.SendBillGrid.Visibility = Visibility.Collapsed;
            this.NoFinishGrid.Visibility = Visibility.Collapsed;
            this.FinishGrid.Visibility = Visibility.Collapsed;
            if (mWeighingBill == null)
            {
                this.EmptyPanel.Visibility = Visibility.Visible;
            }
            else
            {
                if (CurrentWeighingBillIsSendBill == true)
                {
                    this.SendBillInFactoryBtn.Visibility = Visibility.Visible;
                    this.SendBillGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    if (mWeighingBill.receiveStatus == 1)
                    {
                        this.FinishGrid.Visibility = Visibility.Visible;
                        HideOrShowUpdateFinishedBtn();
                    }
                    else
                    {
                        this.OutFactoryBtn.Visibility = Visibility.Visible;
                        this.NoFinishGrid.Visibility = Visibility.Visible;
                        HideOrShowDeleteNofinishBtn();
                    }
                }
            }

        }
        /// <summary>
        /// hide or show the customer send area
        /// </summary>
        /// <param name="show"></param>
        private void HideOrShowSendExpandePanel(bool show = false)
        {
            if (show == true)
            {
                if (this.LeftBorder.Visibility != Visibility.Visible)
                {
                    this.LeftBorder.Visibility = Visibility.Visible;
                }
            }
            else
            {
                this.LeftBorder.Visibility = Visibility.Collapsed;
            }

        }


        private void HideOrShowDeleteNofinishBtn()
        {
            if ("true" == ConfigurationHelper.GetConfig(ConfigItemName.noDeleteInGross.ToString()))
            {
                this.DeleteNofinishedBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.DeleteNofinishedBtn.Visibility = Visibility.Visible;
            }

        }

        private void HideOrShowUpdateFinishedBtn()
        {
            if ("true" == ConfigurationHelper.GetConfig(ConfigItemName.noUpdateFinishedData.ToString()))
            {
                this.FinishUpdateBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.FinishUpdateBtn.Visibility = Visibility.Visible;
            }

        }

        #region  Fill customer send data 
        /// <summary>
        /// 获取发货单 obtian the invoice
        /// </summary>
        private void FillCustomerSendData()
        {
            List<WeighingBill> sendList = WeighingBillModel.GetInvoice();
            if (sendList != null && sendList.Count > 0)
            {
                this.SendBillListView.ItemsSource = sendList;
                this.SendBillCountTb.Text = sendList.Count.ToString();
            }
            else
            {
                this.SendBillListView.ItemsSource = null;
                this.SendBillCountTb.Text = "0";
            }
        }

        #endregion
        /// <summary>
        /// new input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewInPutBtn_Click(object sender, RoutedEventArgs e)
        {
            new InputWindow() { RefershParent = new Action<bool, bool, bool>(this.RefreshData) }.ShowDialog();
        }

        #region Refresh Data
        private void RefreshData(bool l = false, bool c = false, bool r = false)
        {
            if (l == true)
            {
                if (App.GetSoftwareVersion() == SoftwareVersion.netConnection.ToString() || App.GetSoftwareVersion() == SoftwareVersion.netSingleBusiness.ToString())
                {
                    RefreshLeftData();
                }
            }
            if (c == true)
            {
                RefreshCenterData();
            }
            if (r == true)
            {
                RefreshRightData();
            }
        }
        #region Refresh Left Data
        private void RefreshLeftData()
        {
            FillCustomerSendData();
        }
        private void SendBollListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentWeighingBillIsSendBill = true;
            Infactory();
        }

        private void SendBollListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SendBillListView.SelectedIndex < 0) { return; }
            CurrentWeighingBillIsSendBill = true;
            mWeighingBill = null;
            mWeighingBill = this.SendBillListView.SelectedItem as WeighingBill;
            this.SendBillGrid.DataContext = mWeighingBill;
            ShowCurrentPanel();
        }
        #endregion

        #region Refresh Center Data
        private void RefreshCenterData()
        {
            RefreshCurrBillData();
            RefreshStaticData();
        }
        public void RefreshCurrBillData()
        {
            //todo
            if (mWeighingBill == null)
            {
                return;
            }
            string condition = WeighingBillEnum.id.ToString() + "=" + Constract.valueSplit + mWeighingBill.id + Constract.valueSplit;
            String sql = DbBaseHelper.getSelectSql(DataTabeName.weighing_bill.ToString(), null, condition, null, null, null, 1);
            List<WeighingBill> list = DbBaseHelper.DataTableToEntitys<WeighingBill>(DatabaseOPtionHelper.GetInstance().select(sql));
            if (list.Count > 0)
            {
                mWeighingBill = list[0];
                if (this.SendBillGrid.Visibility == Visibility.Visible)
                {
                    this.SendBillGrid.DataContext = mWeighingBill;
                    this.SendBillInFactoryBtn.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.NoFinishGrid.DataContext = mWeighingBill;
                    this.OutFactoryBtn.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void RefreshStaticData() { }
        #endregion

        #region Refresh Right Data

        private void RefreshRightData()
        {
            //no finished
            if (this.NoFinishedTabBtn.IsChecked == true)
            {
                RefreshNoFinishedData();
                return;
            }

            //finished
            if (this.FinishedTabBtn.IsChecked == true)
            {
                RefreshFinishedData();
                return;
            }
        }

        private void NoFinishedTabBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                RefreshRightData();
            }
        }
        //get data
        private void RefreshNoFinishedData()
        {
            List<WeighingBill> list = WeighingBillModel.GetInNoFinished();
            if (list.Count > 0)
            {
                this.NoFinishListView.ItemsSource = list;
                this.NoFinishedNumberTb.Text = list.Count.ToString();
            }
            else
            {
                this.NoFinishedNumberTb.Text = "0";
                this.NoFinishListView.ItemsSource = null;
            }
        }
        private void NoFinishListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NoFinishListView.SelectedIndex < 0)
            {
                return;
            }
            CurrentWeighingBillIsSendBill = false;
            mWeighingBill = this.NoFinishListView.SelectedItem as WeighingBill;
            ShowCurrentPanel();
            this.NoFinishGrid.DataContext = mWeighingBill;
        }
        private void NoFinishListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(mWeighingBill.plateNumber);
        }
        //Finished

        private void FinishedTabBtn_Checked(object sender, RoutedEventArgs e)
        {
            RefreshRightData();
        }
        private void RefreshFinishedData()
        {
            List<WeighingBill> list = WeighingBillModel.GetInFinished();
            if (list.Count > 0)
            {
                this.FinishListView.ItemsSource = list;
                this.FinishedNumberTb.Text = list.Count.ToString();
            }
            else
            {
                this.FinishedNumberTb.Text = "0";
                this.FinishListView.ItemsSource = null;
            }
        }
        private void FinishListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(mWeighingBill.plateNumber);
        }

        private void FinishListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.FinishListView.SelectedIndex < 0)
            {
                return;
            }
            CurrentWeighingBillIsSendBill = false;
            mWeighingBill = this.FinishListView.SelectedItem as WeighingBill;
            ShowCurrentPanel();
            this.FinishGrid.DataContext = mWeighingBill;
        }
        #endregion

        #endregion

        public void Infactory()
        {
            new InputWindow(mWeighingBill, true) { RefershParent = new Action<bool, bool, bool>(RefreshData) }.ShowDialog();
        }
        #region send in factory
        private void SendBillInFactoryBtn_Click(object sender, RoutedEventArgs e)
        {
            Infactory();
        }
        #endregion

        #region Out Fatory
        private void OutFactoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mWeighingBill.receiveStatus == 1)
            {
                MessageBox.Show("该车辆已经出场！");
                return;
            }
            new InputWindow(mWeighingBill, false) { RefershParent = new Action<bool, bool, bool>(RefreshData) }.ShowDialog();
        }
        #endregion

        #region Update
        private void NoFinishUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            new InputUpdateW(mWeighingBill) { RefershParentPage = new Action<bool, bool, bool>(RefreshData) }.ShowDialog();
        }
        private void FinishUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if ("true" == ConfigurationHelper.GetConfig(ConfigItemName.noUpdateFinishedData.ToString()))
            {
                MessageBox.Show("不可以修改已经完成的数据！");
                return;
            }
            new InputUpdateW(mWeighingBill) { RefershParentPage = new Action<bool, bool, bool>(RefreshData) }.ShowDialog();
        }
        #endregion

        #region Print
        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            new PrintBillW(WeightingBillType.RK, mWeighingBill) { RefreshData = new Action(RefreshCurrBillData) }.ShowDialog();
        }
        #endregion

        #region Delete No finished bill
        private void DeleteNofinishedBtn_Click(object sender, RoutedEventArgs e)
        {
            if ("true" == ConfigurationHelper.GetConfig(ConfigItemName.noDeleteInGross.ToString()))
            {
                MessageBox.Show("不可以删除入库的毛重！");
                return;
            }

        }
        #endregion

        private void NoFinishGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            HideOrShowDeleteNofinishBtn();
        }

        private void FinishGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            HideOrShowUpdateFinishedBtn();
        }
        #region left float button 左边的浮动按钮
        private void SendListRefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshLeftData();
        }

        private void SendListBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Show Sended List TODO");
        }
        #endregion


        #region More按钮
        private void MoreMenuBtm_Click(object sender, RoutedEventArgs e)
        {
            if (this.MenuPopup.IsOpen != true)
            {
                this.MenuPopup.IsOpen = true;
            }
            else
            {
                this.MenuPopup.IsOpen = false;
            }
        }
        #endregion
        #region 补录 外来 过磅
        private void BuLuBtn_Click(object sender, RoutedEventArgs e)
        {
            openBuluWindow();
        }

        private void BuluBill_Click(object sender, RoutedEventArgs e)
        {
            openBuluWindow();
        }

        private void openBuluWindow()
        {
            new InputBuluW() { RefershParent = new Action<bool, bool, bool>(RefreshData) }.ShowDialog();
        }
        #endregion


    }
}
