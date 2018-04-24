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
using IntentConnectWeighing.CameraSdk;

namespace IntentConnectWeighing
{
    /// <summary>
    /// OutWeighingPage.xaml 的交互逻辑
    /// </summary>
    public partial class OutWeighingPage : Page
    {
        #region Variable area 
        public bool IsSendCarBill = false;
        public WeighingBill mWeighingBill;
        public SendCarBill mSendCarBilll;
        #endregion

        public OutWeighingPage()
        {
            InitializeComponent();
            this.CommandBindings.Add(SendCarInFactoryCommand.CommandBinding);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.GetSoftwareVersion() == SoftwareVersion.netConnection.ToString() || App.GetSoftwareVersion() == SoftwareVersion.netSingleBusiness.ToString())
            {
                HideOrShowSendExpandePanel(true);
                FillSendCarBillData();
            }
            else
            {
                HideOrShowSendExpandePanel();
            }

            MustNeedSendcar();

            ShowCurrentPanel();
            RefreshData(false, false, true);
            RefreshFinishedData();
        }

        /// <summary>
        /// 出库存过磅是否必须有派车单 是 hide add button
        /// </summary>
        private void MustNeedSendcar()
        {
            if (MyHelper.ConfigurationHelper.GetConfig(ConfigItemName.outWeighingMustHasSendCarbill.ToString()) == "true")
            {
                this.NewOutPutBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.NewOutPutBtn.Visibility = Visibility.Visible;
            }
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
            if (IsSendCarBill == true)
            {
                this.SendBillInFactoryBtn.Visibility = Visibility.Visible;
                this.SendBillGrid.Visibility = Visibility.Visible;
            }
            else if (mWeighingBill == null)
            {
                this.EmptyPanel.Visibility = Visibility.Visible;
            }
            else
            {
                if (mWeighingBill.sendStatus == 1)
                {
                    this.FinishGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    this.NoFinishGrid.Visibility = Visibility.Visible;
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


        #region  Fill customer send data 
        /// <summary>
        /// 获取发货单 obtian the invoice
        /// </summary>
        private void FillSendCarBillData()
        {
            List<SendCarBill> sendList = SendCardBillModel.GetSendCarBill();
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
        private void NewOutPutBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MyHelper.ConfigurationHelper.GetConfig(ConfigItemName.outWeighingMustHasSendCarbill.ToString()) == "true")
            {
                this.NewOutPutBtn.IsEnabled = false;
                return;
            }
            else
            {
                Infactory(null, false);
            }
        }

        public void Infactory(Object bill, bool sendCar = false)
        {
            new OutputWindow(bill, sendCar) { RefershParent = new Action<bool, bool, bool>(RefreshData) }.ShowDialog();
        }

        #region send in factory
        private void SendBillInFactoryBtn_Click(object sender, RoutedEventArgs e)
        {
            Infactory(mSendCarBilll, true);
        }
        #endregion

        #region Out Fatory
        private void OutFactoryBtn_Click(object sender, RoutedEventArgs e)
        {
            Infactory(mWeighingBill, false);
        }
        #endregion

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
            FillSendCarBillData();
        }
        private void SendBollListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IsSendCarBill = true;
            Infactory(mSendCarBilll, true);
        }

        private void SendBollListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SendBillListView.SelectedIndex < 0) { return; }
            IsSendCarBill = true;
            mWeighingBill = null;
            mSendCarBilll = this.SendBillListView.SelectedItem as SendCarBill;
            this.SendBillGrid.DataContext = mSendCarBilll;
            ShowCurrentPanel();
        }
        #endregion

        #region Refresh Center Data
        private void RefreshCenterData()
        {
            RefreshCurrBillData();
            RefreshStaticData();
        }
        private void RefreshCurrBillData()
        {
            if (mSendCarBilll != null)
            {
                this.SendBillInFactoryBtn.Visibility = Visibility.Collapsed;
            }
            else if (mWeighingBill != null)
            {
                mWeighingBill = WeighingBillModel.GetById(mWeighingBill.id);
                this.NoFinishGrid.DataContext = mWeighingBill;
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
            List<WeighingBill> list = WeighingBillModel.GetOutNOFinished();
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
            MessageBox.Show(this.NoFinishListView.Height + " " + this.NoFinishListView.ActualHeight + " " + this.RightMainPanel.Height + " " + this.RightMainPanel.ActualHeight);
            IsSendCarBill = false;
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
            List<WeighingBill> list = WeighingBillModel.GetOutFinished();
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
            IsSendCarBill = false;
            mWeighingBill = this.FinishListView.SelectedItem as WeighingBill;
            ShowCurrentPanel();
            this.FinishGrid.DataContext = mWeighingBill;
        }

        #endregion

        #endregion

        #region Update
        private void NoFinishUpdateBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void FinishUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if ("true" == MyHelper.ConfigurationHelper.GetConfig(ConfigItemName.noUpdateFinishedData.ToString()))
            {
                MessageBox.Show("不可以修改已经完成的数据！");
                return;
            }
        }
        #endregion

        #region Print
        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            new PrintBillW(WeightingBillType.CK, mWeighingBill) { RefreshData = new Action(this.RefreshCurrBillData) }.ShowDialog();
        }
        #endregion

        #region Delete No finished bill
        private void DeleteNofinishedBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TODO");
        }

        #endregion

        #region left Float Button   左边的浮动按钮
        private void SendCarRefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshLeftData();
        }

        private void SendCarListBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Show SendCar List TODO");
        }
        #endregion
        
    }
}
