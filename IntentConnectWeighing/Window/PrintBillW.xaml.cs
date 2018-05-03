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
    /// CameraAddW.xaml 的交互逻辑
    ///  CameraAddW.xaml's interactive logical 
    /// </summary>
    public partial class PrintBillW : Window
    {
        #region Variable        
        public Action RefreshData;
        private WeightingBillType mType;
        private WeighingBill mWeighingBill;
        private bool isOPtionSuccess = false;
        private bool isAutoPrint = false;
        private System.Threading.Timer mTime;

        #endregion
        public PrintBillW(WeightingBillType type, WeighingBill bill, bool autoPrint = false)
        {
            InitializeComponent();
            mType = type;
            mWeighingBill = bill;
            isAutoPrint = autoPrint;
            if (autoPrint == true)
            {
                this.Visibility = Visibility.Hidden;
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
            generaterQrCode();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (isAutoPrint == true)
            {
                Print();
            }
        }

        private void generaterQrCode()
        {
            if (mType == WeightingBillType.RK)
            {
                var bitmap = MyHelper.QrCode.QrCodeHelper.GenerateQrCode(mWeighingBill.receiveNumber, 100, 100);
                this.INQrCodeImage.Source = BitmapHelper.BitmapToBitmapImage(bitmap);
            }
            else
            {
                var bitmap = MyHelper.QrCode.QrCodeHelper.GenerateQrCode(mWeighingBill.sendNumber, 100, 100);
                this.OutQrCodeImage.Source = BitmapHelper.BitmapToBitmapImage(bitmap);
            }

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
            if (mTime != null)
            {
                mTime.Dispose();
            }
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isOPtionSuccess == true)
            {
                if (this.RefreshData != null)
                {
                    this.RefreshData();
                }
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

        private bool isClickPrint = false;
        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            this.PrintBtn.IsEnabled = false;
            isClickPrint = true;
            Print();
            this.PrintBtn.IsEnabled = true;
        }

        public void Print()
        {
            if (isAutoPrint == false && isClickPrint == false)
            {
                return;
            }
            try
            {
                PrintQueue printQueue = LocalPrintServer.GetDefaultPrintQueue();
                if (printQueue == null || printQueue.IsOffline == true)
                {
                    throw new Exception("打印机不可用！");
                }
                string description = "磅单打印";
                PrintDialog printDialog = new PrintDialog();
                printDialog.CurrentPageEnabled = true;
                printDialog.PageRange = new PageRange(1);

                Panel PrintArea = null;
                if (mType == WeightingBillType.RK)
                {
                    PrintArea = this.InPanel;
                }
                else
                {
                    PrintArea = this.OutPanel;
                }
            
                mTime = new System.Threading.Timer(delegate
                {
                    this.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        printDialog = null;
                        this.Close();
                    }), System.Windows.Threading.DispatcherPriority.Send);
                }, null, 3000, System.Threading.Timeout.Infinite);


                this.Dispatcher.BeginInvoke(new Action(delegate
                {
                    printDialog.PrintVisual(PrintArea, description);
                    mWeighingBill.printTimes = mWeighingBill.printTimes + 1;
                    mWeighingBill.printDateTime = MyHelper.DateTimeHelper.getCurrentDateTime();
                    isOPtionSuccess = true;
                    DatabaseOPtionHelper.GetInstance().update(mWeighingBill);
                }));
            }
            catch (Exception e)
            {
                ConsoleHelper.writeLine($"Pint {mWeighingBill.id} failed:{e.Message}");
                this.Close();
            }

        }
    }
}
