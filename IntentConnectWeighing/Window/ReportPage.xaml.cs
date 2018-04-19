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

namespace IntentConnectWeighing
{
    /// <summary>
    /// ReportPage.xaml 的交互逻辑
    /// </summary>
    public partial class ReportPage : Page
    {
        public ReportPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateInChart();
        }

        #region InChart
        private void GenerateInChart() {
            List<WeighingBill> list = WeighingBillModel.GetInFinished();            
            this.InChartDataGrid.ItemsSource = list;
        }

        private void InChartDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;        
        }
     

        private void InChartDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WeighingBill bill = this.InChartDataGrid.SelectedItem as WeighingBill;           
            new WeihgingBillDetailW(bill) { }.ShowDialog();
        }
        #endregion
    }
}
