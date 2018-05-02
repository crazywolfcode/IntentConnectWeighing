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
using IntentConnectWeighing.CameraSdk;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace IntentConnectWeighing
{
    /// <summary>
    /// CameraAddW.xaml 的交互逻辑
    ///  CameraAddW.xaml's interactive logical 
    /// </summary>
    public partial class SendBillSelectW : Window
    {
        #region Variable   
        public Action<WeighingBill> SelectAction;
        private List<WeighingBill> mWeighingBills = new List<WeighingBill>();
        private WeighingBill mWeighingBill;
        #endregion
        public SendBillSelectW()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetData();
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        private void GetData()
        {
            mWeighingBills = WeighingBillModel.GetInvoice();
            this.SendBillDataGrid.ItemsSource = mWeighingBills;
        }

        #region windows Event
        /// <summary>
        /// window move event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headerBorder_MouseMove(object sender, MouseEventArgs e)
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
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.SelectAction != null)
            {
                this.SelectAction(mWeighingBill);
            }
        }
        #endregion
        private void MainDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        //
        private void MainDataGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Select(sender as DataGrid);
                return;
            }

        }

        private void Select(DataGrid dg)
        {
            mWeighingBill = (WeighingBill)dg.SelectedItem;
            this.Close();
            if (this.SelectAction != null)
            {
                this.SelectAction(mWeighingBill);
            }
        }

        private void OutDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Select(sender as DataGrid);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
