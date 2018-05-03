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
using System.Printing;
using System.Drawing;

namespace IntentConnectWeighing
{
    /// <summary>
    /// WeihgingBillDetailW.xaml 的交互逻辑
    ///  WeihgingBillDetailW.xaml's interactive logical 
    /// </summary>
    public partial class WeihgingBillDetailW : Window
    {
        #region Variable              
        private WeightingBillType mType;
        private WeighingBill mWeighingBill;

        public  Action<bool> RefreshParend;
        #endregion
        public WeihgingBillDetailW(WeighingBill bill)
        {
            InitializeComponent();
            mWeighingBill = bill;
            if (mWeighingBill == null)
            {
                this.Close();
            }
            if (mWeighingBill.type == (int)WeightingBillType.CK)
            {
                mType = WeightingBillType.CK;
            }
            else if (mWeighingBill.type == (int)WeightingBillType.RK)
            {
                mType = WeightingBillType.RK;
            }
            else
            {
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.InPanel.Visibility = Visibility.Collapsed;
            this.OutPanel.Visibility = Visibility.Collapsed;
            if (mType == WeightingBillType.RK)
            {
                this.InPanel.Visibility = Visibility.Visible;
                this.InGrid.DataContext = mWeighingBill;
            }
            else
            {
                this.OutPanel.Visibility = Visibility.Visible;
                this.OutGrid.DataContext = mWeighingBill;
            }

        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        #region Window Default Event
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
            if (this.RefreshParend != null)
            {
                this.RefreshParend(true);
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        #endregion

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            int count = System.Convert.ToInt32(MyHelper.ConfigurationHelper.GetConfig(ConfigItemName.defaultPrintTimes.ToString()));
            if ((mWeighingBill.printTimes) < count)
            {
                Print();
            }
            else
            {
                this.PrintBtn.IsEnabled = false;
            }

        }

        private void FinishUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MyHelper.ConfigurationHelper.GetConfig(ConfigItemName.noUpdateFinishedData.ToString()) == "false")
            {
                UpdateBill();
            }
            else
            {
                this.FinishUpdateBtn.IsEnabled = false;
            }
        }

        private void OutFinishUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MyHelper.ConfigurationHelper.GetConfig(ConfigItemName.noUpdateFinishedData.ToString()) == "false")
            {
                UpdateBill();
            }
            else
            {
                this.OutFinishUpdateBtn.IsEnabled = false;
            }
        }

        private void OutPrintBtn_Click(object sender, RoutedEventArgs e)
        {
            int count = System.Convert.ToInt32(MyHelper.ConfigurationHelper.GetConfig(ConfigItemName.defaultPrintTimes.ToString()));
            if ((mWeighingBill.printTimes) < count)
            {
                Print();
            }
            else
            {
                this.PrintBtn.IsEnabled = false;
            }
        }

        private void Print()
        {
            new PrintBillW(mType, mWeighingBill) { }.ShowDialog();
        }

        private void UpdateBill()
        {
            if (mType == WeightingBillType.CK)
            {
                new OutputUpdateW(mWeighingBill) { RefershParent = new Action<WeighingBill>(AfterUpdate) }.ShowDialog();
            }
            else
            {
                new InputUpdateW(mWeighingBill) { RefershParent = new Action<WeighingBill>(AfterUpdate) }.ShowDialog();
            }
        }

        private void AfterUpdate(WeighingBill bill)
        {
            if (bill == null)
            {
                this.Close();
            }
            else
            {
                mWeighingBill = bill;
                if (InPanel.Visibility == Visibility.Visible)
                {
                    this.InGrid.DataContext = null;
                    this.InGrid.DataContext = mWeighingBill;
                }
                else
                {
                    this.OutGrid.DataContext = null;
                    this.OutGrid.DataContext = this.InGrid.DataContext = mWeighingBill;
                }
            }
        }
    }
}
