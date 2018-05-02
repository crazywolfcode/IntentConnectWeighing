using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Visifire;
using Visifire.Charts;

namespace IntentConnectWeighing
{
    /// <summary>
    /// ReportPage.xaml 的交互逻辑
    /// </summary>
    public partial class ReportPage : Page
    {
        private Timer RefreshDataTimer;
        private string InDetailTotalStatisticLabelTemplate = "合计: ({0}) 家公司供应({1}) 种物资，共({2})车，原发净重:{3}（吨） 收货毛重:{4}（吨） 收货皮重: {5}（吨）收货净重: {6}（吨）扣吨:{7} (吨) 磅差:{8}(吨)";
        private string OutDetailTotalStatisticLabelTemplate = "合计: 供应 ({0}) 家公司({1}) 种物资，共({2})车，发货皮重: {3}（吨） 发货毛重:{4}（吨） 发货净重: {5}";
        private string ListBoxItemLabelTemplate = " 公司名称：{0}  货场名称：{1}  物资名称：{2}  {3}(车)  毛重：{4}  皮重： {5}  净重：{6} (吨)";
        private string searchCondition = string.Empty;
        public ReportPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.initSearchArea();
            this.RefershData();

            RefreshDataTimer = new Timer(delegate
            {
                this.Dispatcher.BeginInvoke(
                                 new Action(delegate
                                 {
                                     this.TimerRefresh();
                                 }));
            }, null, 60000, 120000);
        }

        private void TimerRefresh()
        {
            initReportHearder();

            if (this.TodayInTabBtn.IsChecked == true)
            {
                RefreshInChart();
                return;
            }
            //if (this.InDetialReportTabBtn.IsChecked == true)
            //{
            //    GetInData();
            //    GetInSummmaryStatisticData();
            //    GetInStatisticData();
            //}
            if (this.TodayOutTabBtn.IsChecked == true)
            {
                RefreshOutChart();
                return;
            }
            //if (this.OutDetialReportTabBtn.IsChecked == true)
            //{
            //    GetOutData();
            //    GetOutSummmaryStatisticData();
            //    GetOutStatisticData();
            //    return;
            //}
            if (this.TodaySummaryTabBtn.IsChecked == true)
            {
                RefreshSummary();
                return;
            }
            return;
        }

        #region 刷新
        private void RefreshContextBtn_Click(object sender, RoutedEventArgs e)
        {
            RefershData();
        }

        private void RefershData()
        {
            initReportHearder();

            if (this.TodayInTabBtn.IsChecked == true)
            {
                RefreshInChart();
            }
            else if (this.InDetialReportTabBtn.IsChecked == true)
            {
                GetInData();
                GetInSummmaryStatisticData();
                GetInStatisticData();
            }
            else if (this.TodayOutTabBtn.IsChecked == true)
            {
                RefreshOutChart();
            }
            else if (this.OutDetialReportTabBtn.IsChecked == true)
            {
                GetOutData();
                GetOutSummmaryStatisticData();
                GetOutStatisticData();
            }
            else if (this.TodaySummaryTabBtn.IsChecked == true)
            {
                RefreshSummary();
            }
            else
            {
                return;
            }

        }
        #endregion

        #region Today In
        private void RefreshInChart()
        {
            if (this.TodayInTabBtn.IsChecked == true)
            {
                GenerateInChart();
            }
        }

        private void GetInData()
        {
            List<WeighingBill> list = WeighingBillModel.GetInFinished(GetStartDate(), GetEndDate(), GetSeachCondition());
            this.InDetailDataGrid.ItemsSource = list;
        }

        private void GetInStatisticData()
        {
            this.InStatisticListBox.Items.Clear();
            List<ReportWeightListV> list = WeighingBillModel.GetListStatisticData(WeightingBillType.RK, GetStartDate(), GetEndDate(), GetSeachCondition());
            foreach (ReportWeightListV v in list)
            {
                this.InStatisticListBox.Items.Add(new TextBlock()
                {
                    Text = String.Format(this.ListBoxItemLabelTemplate, v.company, v.yard, v.material, v.Cars, v.grossweight, v.traeweight, v.weight)
                });
            }
        }

        private void GetInSummmaryStatisticData()
        {
            this.InSummaryTextBlock.Text = null;
            ReportWeightSummaryV v = WeighingBillModel.GetReportWeightSummaryData(WeightingBillType.RK, GetStartDate(), GetEndDate(), GetSeachCondition());
            if (v != null)
            {
                this.InSummaryTextBlock.Text = String.Format(this.InDetailTotalStatisticLabelTemplate, v.companys, v.materials, v.cars.ToString(), v.sendweight, v.grossweight, v.traeweight, v.netweight, v.decuationweight, v.differenceweight);
            }
        }

        private void GenerateInChart()
        {

            this.InMaterialCarChart.Series.Clear();
            this.InMaterialTonChart.Series.Clear();
            this.InCompanyCarChart.Series.Clear();
            this.InCompanyTonChart.Series.Clear();

            Dictionary<String, Int32> materialCars = WeighingBillModel.GetInCahartDataGroungByMaterial(GetStartDate(), GetEndDate(), GetSeachCondition());
            DataSeries materialCarsDataSeries = new DataSeries() { RenderAs = RenderAs.Pie };
            foreach (var dic in materialCars)
            {
                materialCarsDataSeries.DataPoints.Add(new DataPoint() { AxisXLabel = dic.Key, YValue = dic.Value });
            }

            Dictionary<String, Double> materialTons = WeighingBillModel.GetInCahartTonDataGroungByMaterial(GetStartDate(), GetEndDate(), GetSeachCondition());
            DataSeries materialTonsDataSeries = new DataSeries() { RenderAs = RenderAs.Doughnut };
            if (materialTons.Count > 3)
            {
                materialTonsDataSeries.RenderAs = RenderAs.Radar;
            }
            foreach (var dic in materialTons)
            {
                materialTonsDataSeries.DataPoints.Add(new DataPoint() { AxisXLabel = dic.Key, YValue = dic.Value });
            }


            Dictionary<String, Int32> CompanyCars = WeighingBillModel.GetInCahartDataGroungBySendCpmpany(GetStartDate(), GetEndDate(), GetSeachCondition());
            DataSeries companyCarsDataSeries = new DataSeries() { RenderAs = RenderAs.Pie };
            if (CompanyCars.Count > 8)
            {
                companyCarsDataSeries.RenderAs = RenderAs.Doughnut;
            }
            foreach (var dic in CompanyCars)
            {
                companyCarsDataSeries.DataPoints.Add(new DataPoint() { AxisXLabel = dic.Key, YValue = dic.Value });
            }


            Dictionary<String, Double> CompanyTons = WeighingBillModel.GetInCahartTonDataGroungBySendCpmpany(GetStartDate(), GetEndDate(), GetSeachCondition());
            DataSeries companyTonsDataSeries = new DataSeries() { RenderAs = RenderAs.Pie };
            if (CompanyTons.Count > 3)
            {
                companyTonsDataSeries.RenderAs = RenderAs.Radar;
            }
            foreach (var dic in CompanyTons)
            {
                companyTonsDataSeries.DataPoints.Add(new DataPoint() { AxisXLabel = dic.Key, YValue = dic.Value });
            }

            this.InMaterialCarChart.Series.Add(materialCarsDataSeries);
            this.InMaterialTonChart.Series.Add(materialTonsDataSeries);
            this.InCompanyCarChart.Series.Add(companyCarsDataSeries);
            this.InCompanyTonChart.Series.Add(companyTonsDataSeries);
        }

        private void InChartDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void InChartDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WeighingBill bill = this.InDetailDataGrid.SelectedItem as WeighingBill;
            new WeihgingBillDetailW(bill) { }.ShowDialog();
        }

        #endregion


        #region Today Out
        private void RefreshOutChart()
        {
            if (this.TodayOutTabBtn.IsChecked == true)
            {
                GenerateOutChart();
            }
        }
        private void GetOutData()
        {
            List<WeighingBill> list = WeighingBillModel.GetOutFinished(GetStartDate(), GetEndDate(), GetSeachCondition());
            this.OutDetailDataGrid.ItemsSource = list;
        }

        private void GetOutSummmaryStatisticData()
        {
            this.OutSummaryTextBlock.Text = null;
            ReportWeightSummaryV v = WeighingBillModel.GetReportWeightSummaryData(WeightingBillType.CK, GetStartDate(), GetEndDate(), GetSeachCondition());
            if (v != null)
            {
                this.OutSummaryTextBlock.Text = String.Format(this.OutDetailTotalStatisticLabelTemplate, v.companys, v.materials, v.cars, v.traeweight, v.grossweight, v.netweight);
            }
        }

        private void GetOutStatisticData()
        {
            this.OutStatisticListBox.Items.Clear();
            List<ReportWeightListV> list = WeighingBillModel.GetListStatisticData(WeightingBillType.CK, GetStartDate(), GetEndDate(), GetSeachCondition());
            foreach (ReportWeightListV v in list)
            {
                this.OutStatisticListBox.Items.Add(new TextBlock()
                {
                    Text = String.Format(this.ListBoxItemLabelTemplate, v.company, v.yard, v.material, v.Cars.ToString(), v.grossweight, v.traeweight, v.weight.ToString())
                });
            }
        }
        private void OutChartDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void OutChartDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WeighingBill bill = this.OutDetailDataGrid.SelectedItem as WeighingBill;
            new WeihgingBillDetailW(bill) { }.ShowDialog();
        }
        private void GenerateOutChart()
        {

            this.OutMaterialCarChart.Series.Clear();
            this.OutMaterialTonChart.Series.Clear();
            this.OutCompanyCarChart.Series.Clear();
            this.OutCompanyTonChart.Series.Clear();

            Dictionary<String, Int32> materialCars = WeighingBillModel.GetOutCahartDataGroungByMaterial(GetStartDate(), GetEndDate(), GetSeachCondition());
            DataSeries materialCarsDataSeries = new DataSeries() { RenderAs = RenderAs.Pie };
            foreach (var dic in materialCars)
            {
                materialCarsDataSeries.DataPoints.Add(new DataPoint() { AxisXLabel = dic.Key, YValue = dic.Value });
            }

            Dictionary<String, Double> materialTons = WeighingBillModel.GetOutCahartTonDataGroungByMaterial(GetStartDate(), GetEndDate(), GetSeachCondition());
            DataSeries materialTonsDataSeries = new DataSeries() { RenderAs = RenderAs.Doughnut };
            if (materialTons.Count > 3)
            {
                materialTonsDataSeries.RenderAs = RenderAs.Radar;

            }
            foreach (var dic in materialTons)
            {
                materialTonsDataSeries.DataPoints.Add(new DataPoint() { AxisXLabel = dic.Key, YValue = dic.Value });
            }


            Dictionary<String, Int32> CompanyCars = WeighingBillModel.GetOutCahartDataGroungBySendCpmpany(GetStartDate(), GetEndDate(), GetSeachCondition());
            DataSeries companyCarsDataSeries = new DataSeries() { RenderAs = RenderAs.Pie };
            foreach (var dic in CompanyCars)
            {
                companyCarsDataSeries.DataPoints.Add(new DataPoint() { AxisXLabel = dic.Key, YValue = dic.Value });
            }


            Dictionary<String, Double> CompanyTons = WeighingBillModel.GetOutCahartTonDataGroungBySendCpmpany(GetStartDate(), GetEndDate(), GetSeachCondition());
            DataSeries companyTonsDataSeries = new DataSeries() { RenderAs = RenderAs.Pie };
            if (CompanyTons.Count > 3)
            {
                companyTonsDataSeries.RenderAs = RenderAs.Radar;

            }
            foreach (var dic in CompanyTons)
            {
                companyTonsDataSeries.DataPoints.Add(new DataPoint() { AxisXLabel = dic.Key, YValue = dic.Value });
            }

            this.OutMaterialCarChart.Series.Add(materialCarsDataSeries);
            this.OutMaterialTonChart.Series.Add(materialTonsDataSeries);
            this.OutCompanyCarChart.Series.Add(companyCarsDataSeries);
            this.OutCompanyTonChart.Series.Add(companyTonsDataSeries);
        }
        #endregion


        #region Today summary
        private void RefreshSummary()
        {
            if (this.TodaySummaryTabBtn.IsChecked == true)
            {

            }
        }
        #endregion

        #region  detail
        private void RefreshInDetail()
        {
            if (this.InDetialReportTabBtn.IsChecked == true)
            {

            }
        }

        private void RefreshOutDetail()
        {
            if (this.OutDetialReportTabBtn.IsChecked == true)
            {

            }
        }
        #endregion


        #region EXport EXCL，
        private void ExportExcelBtn_Click(object sender, RoutedEventArgs e)
        {
            //if (this.InDetialReportTabBtn.IsChecked == true)
            //{
            //    MyHelper.ExclHelper.Export(this.InDetailDataGrid, MyHelper.DateTimeHelper.getCurrentDateTime(MyHelper.DateTimeHelper.DateFormat) + this.TodayInTabBtn.Content);
            //    return;
            //}

            //if (this.OutDetialReportTabBtn.IsChecked == true)
            //{
            //    MyHelper.ExclHelper.Export(this.OutDetailDataGrid, MyHelper.DateTimeHelper.getCurrentDateTime(MyHelper.DateTimeHelper.DateFormat) + this.TodayInTabBtn.Content);
            //    return;
            //}

            if (this.InDetialReportTabBtn.IsChecked == true)
            {
                MyHelper.ExclHelper.ExclExprotToExcelWitchStatisticInfo(
                                this.InDetailDataGrid,
                                MyHelper.DateTimeHelper.getCurrentDateTime(MyHelper.DateTimeHelper.DateFormat) + this.TodayInTabBtn.Content,
                                mReportHeader.Title,
                                this.TodayInTabBtn.Content.ToString(),
                                this.mReportHeader.YardName + "       " + this.mReportHeader.DateArea + "     导出时间:" + MyHelper.DateTimeHelper.getCurrentDateTime(),
                                this.InSummaryTextBlock.Text,
                                GetListStatisticToListString(this.InStatisticListBox)
                    );
                return;
            }
            if (this.OutDetialReportTabBtn.IsChecked == true)
            {
                MyHelper.ExclHelper.ExclExprotToExcelWitchStatisticInfo(
                        this.OutDetailDataGrid,
                        MyHelper.DateTimeHelper.getCurrentDateTime(MyHelper.DateTimeHelper.DateFormat) + this.TodayOutTabBtn.Content,
                        mReportHeader.Title,
                        this.TodayOutTabBtn.Content.ToString(),
                        this.mReportHeader.YardName + "       " + this.mReportHeader.DateArea + "     导出时间:" + MyHelper.DateTimeHelper.getCurrentDateTime(),
                        this.OutSummaryTextBlock.Text,
                        GetListStatisticToListString(this.OutStatisticListBox)
                   );
                return;
            }
            MessageBox.Show("当前的报表类型支持导出，请选择“入库明细报表” 或 “出库存明细报表”");
        }

        private List<String> GetListStatisticToListString(ListBox listBox)
        {
            if (listBox == null || listBox.Items.Count <= 0)
            {
                return null;
            }
            List<string> list = new List<string>();

            for (int i = 0; i < listBox.Items.Count; i++)
            {
                TextBlock tb = (TextBlock)listBox.Items[i];
                if (tb != null)
                {
                    list.Add( tb.Text);
                }
                else
                {
                    list.Add(" ");
                }
            }
            return list;
        }
        #endregion


        #region Tab Btb Checked

        private bool todayInIsRefreshed = false;
        private void TodayInTabBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                initSearchArea();
                if (todayInIsRefreshed == false)
                {
                    RefershData();
                    this.todayInIsRefreshed = true;
                }
            }
        }
        private bool todayOutIsRefreshed = false;
        private void TodayOutTabBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                initSearchArea();
                if (todayOutIsRefreshed == false)
                {
                    RefershData();
                    this.todayOutIsRefreshed = true;
                }
            }
        }
        private bool todaySummarytIsRefreshed = false;
        private void TodaySummaryTabBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                initSearchArea();
                if (todaySummarytIsRefreshed == false)
                {
                    RefershData();
                    this.todaySummarytIsRefreshed = true;
                }
            }
        }
        private bool InDetialReportIsRefreshed = false;
        private void InDetialReportTabBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                initSearchArea();
                if (InDetialReportIsRefreshed == false)
                {
                    RefershData();
                    this.InDetialReportIsRefreshed = true;
                }
            }

        }
        private bool OutDetialReportIsRefreshed = false;
        private void OutDetialReportTabBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                initSearchArea();
                if (this.OutDetialReportIsRefreshed == false)
                {
                    RefershData();
                    this.OutDetialReportIsRefreshed = true;
                }
            }
        }
        #endregion
        ReportHeaderV mReportHeader;
        #region initialization report header
        private void initReportHearder()
        {
            mReportHeader = new ReportHeaderV();
            mReportHeader.Title = App.currentCompany.name;
            mReportHeader.YardName = App.currentYard.name;
            mReportHeader.PrintTime = MyHelper.DateTimeHelper.getCurrentDateTime();

            if ((this.StratDatePicker.SelectedDate) == null && (this.EndDatePicker.SelectedDate) == null)
            {
                mReportHeader.DateArea = MyHelper.DateTimeHelper.getCurrentDateTime(MyHelper.DateTimeHelper.DateFormat);
            }
            else if ((this.StratDatePicker.SelectedDate) != null && (this.EndDatePicker.SelectedDate) == null)
            {
                mReportHeader.DateArea = ((DateTime)this.StratDatePicker.SelectedDate).Date.ToString().Replace("/", "-") + "  到 今天";
            }
            else if ((this.StratDatePicker.SelectedDate) == null && (this.EndDatePicker.SelectedDate) != null)
            {
                mReportHeader.DateArea = "很久以前 到 " + ((DateTime)this.EndDatePicker.SelectedDate).Date.ToString().Replace("/", "-");
            }
            else
            {
                mReportHeader.DateArea = ((DateTime)this.StratDatePicker.SelectedDate).Date.ToString().Replace("/", "-") + " 到 " + ((DateTime)this.EndDatePicker.SelectedDate).Date.ToString().Replace("/", "-");
            }
            this.DataContext = mReportHeader;
        }
        #endregion

        #region Search area 
        private void initSearchArea()
        {
            this.receiveCompanyCB.IsEnabled = true;
            this.receiveYandCB.IsEnabled = true;
            this.sendCompanyCB.IsEnabled = true;
            this.sendYandCB.IsEnabled = true;
            this.MaterialNameCB.IsEnabled = true;
            this.CarNumberCB.IsEnabled = true;
            this.StratDatePicker.SelectedDate = null;
            this.EndDatePicker.SelectedDate = null;
            this.MaterialNameCB.SelectedIndex = -1;
            this.CarNumberCB.SelectedIndex = -1;
            if (this.TodayInTabBtn.IsChecked == true || this.InDetialReportTabBtn.IsChecked == true)
            {
                this.sendCompanyCB.SelectedIndex = -1;
                this.sendYandCB.SelectedIndex = -1;
                this.receiveCompanyCB.ItemsSource = null;
                this.receiveCompanyCB.IsEnabled = false;
                this.receiveCompanyCB.SelectedIndex = -1;
                this.receiveCompanyCB.ItemsSource = new List<Company>() { App.currentCompany };
                this.receiveCompanyCB.SelectedIndex = 0;
                this.receiveYandCB.IsEnabled = false;
                this.receiveYandCB.SelectedIndex = -1;
                this.receiveYandCB.ItemsSource = null;
                this.receiveYandCB.ItemsSource = new List<Yard>() { App.currentYard };
                this.receiveYandCB.SelectedIndex = 0;
            }
            else if (this.TodayOutTabBtn.IsChecked == true || this.OutDetialReportTabBtn.IsChecked == true)
            {
                this.receiveCompanyCB.SelectedIndex = -1;
                this.receiveYandCB.SelectedIndex = -1;
                this.sendCompanyCB.IsEnabled = false;
                this.sendCompanyCB.SelectedIndex = -1;
                this.sendCompanyCB.ItemsSource = null;
                this.sendCompanyCB.ItemsSource = new List<Company>() { App.currentCompany };
                this.sendCompanyCB.SelectedIndex = 0;
                this.sendYandCB.IsEnabled = false;
                this.sendYandCB.SelectedIndex = -1;
                this.sendYandCB.ItemsSource = null;
                this.sendYandCB.ItemsSource = new List<Yard>() { App.currentYard };
                this.sendYandCB.SelectedIndex = 0;
            }
            else if (this.TodaySummaryTabBtn.IsChecked == true)
            {
                this.sendCompanyCB.SelectedIndex = -1;
                this.sendCompanyCB.IsEnabled = false;
                this.sendYandCB.SelectedIndex = -1;
                this.sendYandCB.IsEnabled = false;
                this.receiveCompanyCB.SelectedIndex = -1;
                this.receiveCompanyCB.IsEnabled = false;
                this.receiveYandCB.SelectedIndex = -1;
                this.receiveYandCB.IsEnabled = false;
                this.MaterialNameCB.ItemsSource = null;
                this.MaterialNameCB.IsEnabled = false;
                this.CarNumberCB.ItemsSource = null;
                this.CarNumberCB.IsEnabled = false;
            }
            else
            {
                return;
            }

        }


        //supply
        private bool isSelectsendcompany = false;
        private void sendCompanyCB_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = sendCompanyCB.Text;
            if (text.Length < 2)
            {
                isSelectsendcompany = false;
                this.sendYandCB.ItemsSource = null;
                return;
            }
            if (isSelectsendcompany == true)
            {
                return;
            }
            if (this.sendCompanyCB.IsEnabled == false)
            {
                return;
            }
            if (text.Length >= 2)
            {
                List<Company> list = CompanyModel.IndistinctSearchByNameOrNameFirstCase(text);
                if (list.Count <= 0)
                {
                    isSelectsendcompany = false;
                }
                else
                {
                    this.sendCompanyCB.ItemsSource = null;
                    this.sendCompanyCB.ItemsSource = list;
                    this.sendCompanyCB.IsDropDownOpen = true;
                }
            }
            else
            {
                isSelectsendcompany = false;
            }
        }

        private void sendCompanyCB_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.sendCompanyCB.SelectedIndex == -1) { return; }
            isSelectsendcompany = true;
            Company cp = (Company)this.sendCompanyCB.SelectedItem;
            if (cp != null)
            {
                ShowSendYard(cp.id);
            }
        }

        // send yard
        /// <summary>
        ///  获取和显示发货的货场
        /// </summary>
        private void ShowSendYard(String companyid)
        {
            List<Yard> list = YardModel.GetListByCompanyId(companyid);
            if (list.Count > 0)
            {
                this.sendYandCB.ItemsSource = list;
                this.sendYandCB.SelectedIndex = 0;
            }
        }
        private void sendYandCB_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        //receive
        private bool isSelectReceiveCompany = false;
        private void receiveCompanyCB_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = receiveCompanyCB.Text;
            if (text.Length <= 0)
            {
                isSelectReceiveCompany = false;
                this.receiveYandCB.ItemsSource = null;
                return;
            }
            if (this.isSelectReceiveCompany == true)
            {
                return;
            }
            if (this.receiveCompanyCB.IsEnabled == false)
            {
                return;
            }

            if (text.Length >= 2)
            {
                List<Company> list = CompanyModel.IndistinctSearchByNameOrNameFirstCase(text);
                if (list.Count <= 0)
                {
                    isSelectReceiveCompany = false;
                }
                else
                {
                    this.receiveCompanyCB.ItemsSource = null;
                    this.receiveCompanyCB.ItemsSource = list;
                    this.receiveCompanyCB.IsDropDownOpen = true;
                }
            }
            else
            {
                isSelectReceiveCompany = false;
            }
        }

        private void receiveCompanyCB_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.receiveCompanyCB.SelectedIndex == -1) { return; }
            isSelectReceiveCompany = true;
            Company cp = (Company)this.receiveCompanyCB.SelectedItem;
            if (cp != null)
            {
                ShowReceiveYard(cp.id);
            }
        }
        // receive yard

        private void ShowReceiveYard(String companyid)
        {
            List<Yard> list = YardModel.GetListByCompanyId(companyid);
            if (list.Count > 0)
            {
                this.receiveYandCB.ItemsSource = list;
                this.receiveYandCB.SelectedIndex = 0;
            }
        }
        private void receiveYandCB_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        //material
        private bool isSelectMaterial = false;
        private void MaterialNameCB_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = MaterialNameCB.Text;
            if (text.Length <= 0)
            {
                isSelectMaterial = false;
                return;
            }
            if (isSelectMaterial == true)
            {
                return;
            }
            if (text.Length > 0)
            {
                List<Material> list = MaterialModel.IndistictSearchByNameORFirstCase(text);
                if (list.Count <= 0)
                {
                    isSelectMaterial = false;
                }
                else
                {
                    this.MaterialNameCB.ItemsSource = null;
                    this.MaterialNameCB.ItemsSource = list;
                    this.MaterialNameCB.IsDropDownOpen = true;
                }
            }
        }

        private void MaterialNameCB_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.MaterialNameCB.SelectedIndex > -1)
            {
                isSelectMaterial = true;
            }
        }
        //car 
        private bool isSelectCar = false;
        private void CarNumberCB_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = CarNumberCB.Text;
            if (text.Length <= 1)
            {
                isSelectCar = false;
                return;
            }
            if (isSelectCar == true)
            {
                return;
            }
            if (text.Length > 0)
            {
                List<CarInfo> list = CarInfoModel.FuzzySearch(text);
                if (list.Count <= 0)
                {
                    isSelectCar = false;
                }
                else
                {
                    this.CarNumberCB.ItemsSource = null;
                    this.CarNumberCB.ItemsSource = list;
                    this.CarNumberCB.IsDropDownOpen = true;
                }
            }
        }

        private void CarNumberCB_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.CarNumberCB.SelectedIndex > -1)
            {
                isSelectCar = true;
            }

        }

        #endregion

        private string GetSeachCondition()
        {
            string condition = string.Empty;
            Company sendC = (Company)this.sendCompanyCB.SelectedItem;
            if (sendC != null)
            {
                if (string.IsNullOrEmpty(condition))
                {
                    condition += WeighingBillEnum.send_company_id.ToString() + " = " + Constract.valueSplit + sendC.id + Constract.valueSplit;
                }
                else
                {
                    condition += " and " + WeighingBillEnum.send_company_id.ToString() + " = " + Constract.valueSplit + sendC.id + Constract.valueSplit;
                }
            }
            Yard sendY = (Yard)this.sendYandCB.SelectedItem;
            if (sendY != null)
            {
                if (String.IsNullOrEmpty(condition))
                {
                    condition += WeighingBillEnum.send_yard_id.ToString() + " = " + Constract.valueSplit + sendY.id + Constract.valueSplit;
                }
                else
                {
                    condition += " and " + WeighingBillEnum.send_yard_id.ToString() + " = " + Constract.valueSplit + sendY.id + Constract.valueSplit;
                }
            }
            Company receiveC = (Company)this.receiveCompanyCB.SelectedItem;
            if (receiveC != null)
            {
                if (string.IsNullOrEmpty(condition))
                {
                    condition += WeighingBillEnum.receive_company_id.ToString() + " = " + Constract.valueSplit + receiveC.id + Constract.valueSplit;
                }
                else
                {
                    condition += " and " + WeighingBillEnum.receive_company_id.ToString() + " = " + Constract.valueSplit + receiveC.id + Constract.valueSplit;
                }
            }
            Yard receiveY = (Yard)this.receiveYandCB.SelectedItem;
            if (receiveY != null)
            {
                if (String.IsNullOrEmpty(condition))
                {
                    condition += WeighingBillEnum.receive_yard_id.ToString() + " = " + Constract.valueSplit + receiveY.id + Constract.valueSplit;
                }
                else
                {
                    condition += " and " + WeighingBillEnum.receive_yard_id.ToString() + " = " + Constract.valueSplit + receiveY.id + Constract.valueSplit;
                }
            }
            Material material = (Material)this.MaterialNameCB.SelectedItem;
            if (material != null)
            {
                if (this.TodayInTabBtn.IsChecked == true || this.InDetialReportTabBtn.IsChecked == true)
                {
                    if (String.IsNullOrEmpty(condition))
                    {
                        condition += WeighingBillEnum.receive_material_id.ToString() + " = " + Constract.valueSplit + material.id + Constract.valueSplit;
                    }
                    else
                    {
                        condition += " and " + WeighingBillEnum.receive_material_id.ToString() + " = " + Constract.valueSplit + material.id + Constract.valueSplit;
                    }
                }
                else if (this.TodayOutTabBtn.IsChecked == true || this.OutDetialReportTabBtn.IsChecked == true)
                {
                    if (String.IsNullOrEmpty(condition))
                    {
                        condition += WeighingBillEnum.send_material_id.ToString() + " = " + Constract.valueSplit + material.id + Constract.valueSplit;
                    }
                    else
                    {
                        condition += " and " + WeighingBillEnum.send_material_id.ToString() + " = " + Constract.valueSplit + material.id + Constract.valueSplit;
                    }
                }
            }
            CarInfo carInfo = (CarInfo)CarNumberCB.SelectedItem;
            if (carInfo != null)
            {
                if (String.IsNullOrEmpty(condition))
                {
                    condition += WeighingBillEnum.plate_number.ToString() + " = " + Constract.valueSplit + carInfo.carNumber + Constract.valueSplit;
                }
                else
                {
                    condition += " and " + WeighingBillEnum.plate_number.ToString() + " = " + Constract.valueSplit + carInfo.carNumber + Constract.valueSplit;
                }
            }
            return condition;
        }
        private string GetStartDate()
        {
            if (StratDatePicker.SelectedDate == null)
            {
                return null;
            }
            DateTime tempdate = (DateTime)StratDatePicker.SelectedDate;
            if (tempdate == null)
            {
                return null;
            }
            string res = tempdate.ToString(Constract.defaultDateTimeFormat);
            return res;
        }

        private string GetEndDate()
        {
            if (EndDatePicker.SelectedDate == null)
            {
                return null;
            }
            DateTime tempdate = (DateTime)EndDatePicker.SelectedDate;
            if (tempdate == null)
            {
                return null;
            }
            string endtime = " 23:59:59";
            string res = tempdate.ToString(Constract.DateFormat) + endtime;
            return res;
        }

        #region Search btn click
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            this.SearchBtn.Cursor = Cursors.Wait;
            this.searchCondition = GetSeachCondition();
            RefershData();
            this.SearchBtn.Cursor = Cursors.Hand;
        }

        #endregion

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.RefreshDataTimer != null)
            {
                this.RefreshDataTimer.Dispose();
            }
        }
        #region Search Tab Button
        private void SearchTabBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.searchAreaPanel.Height == 0)
            {
                SearAreaAnimationToShow();
            }
            else
            {
                SearAreaAnimationToHidden();
            }
        }
        private void SearAreaAnimationToShow()
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 60;
            animation.Duration = TimeSpan.FromSeconds(0.8);
            this.searchAreaPanel.BeginAnimation(HeightProperty, animation);
        }
        private void SearAreaAnimationToHidden()
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 60;
            animation.To = 0;
            animation.Duration = TimeSpan.FromSeconds(0.5);
            this.searchAreaPanel.BeginAnimation(HeightProperty, animation);
        }
        #endregion

    }
}
