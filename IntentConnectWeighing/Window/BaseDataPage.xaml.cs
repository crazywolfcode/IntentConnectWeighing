using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Data;
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
    /// BaseDataPage.xaml 的交互逻辑
    /// </summary>
    public partial class BaseDataPage : Page
    {
        private ListView currentListView;
        public Window paretntWindow;
        private List<MaterialV> mMaterialVs = null;
        private List<ProvinceV> mProvinceVs = null;
        private Boolean materialNeedRefresh = true;
        private Boolean companyNeedRefresh = true;
        private Boolean carNeedRefresh = true;
        private Material currMaterial;
        private CarInfo currCarInfo;
        private Company currCompany;
        private List<CarHeaderV> mCarHeaderVs;

        public BaseDataPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.scrollViewer.Height = this.scrollViewer.ActualHeight - 30;

            FillData();
        }

        #region 填充数据

        private void TabBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                FillData();
            }
        }
        /// <summary>
        /// 填充数据
        /// </summary>
        private void FillData()
        {
            if (CompanyTabBtn.IsChecked == true)
            {
                FillCompanyData();
                return;
            }
            if (MaterialTabBtn.IsChecked == true)
            {
                FillMaterialData();
                return;
            }
            if (CarTabBtn.IsChecked == true)
            {
                FillCarData();
                return;
            }
        }

        private void FillCompanyData()
        {
            GenerterProvinceVData();
            if (mProvinceVs == null || mProvinceVs.Count <= 0)
            {
                //  fill alert into
                FillCompanyAlretData();
            }
            else
            {
                // fill true data
                if (companyNeedRefresh == true)
                {
                    companyNeedRefresh = false;
                    this.CompanyListContentStackPanel.Children.Clear();
                    for (int i = 0; i < mProvinceVs.Count; i++)
                    {
                        Expander expander = new Expander();
                        expander.Style = FindResource(ResourceName.BaseDataExpenderStyle.ToString()) as Style;
                        expander.Header = mProvinceVs[i].Province;
                        expander.HeaderTemplate = FindResource(ResourceName.CompanyBaseDataTemplate.ToString()) as DataTemplate;

                        ListView listView = new ListView();
                        listView.ItemsSource = mProvinceVs[i].Companys;
                        listView.Style = FindResource(ResourceName.BaseDataListViewStyle.ToString()) as Style;
                        listView.ItemContainerStyle = FindResource(ResourceName.ListViewItemCompanyBaseDataStyle.ToString()) as Style;
                        listView.SelectionChanged += CompanyListView_SelectionChanged; ;
                        expander.Content = listView;
                        this.CompanyListContentStackPanel.Children.Add(expander);
                    }
                }
            }

        }

        private void CompanyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            if (lv.SelectedIndex == -1)
            {
                return;
            }
            if (currentListView != lv)
            {
                if (currentListView != null)
                {
                    currentListView.SelectedIndex = -1;
                }
            }
            currentListView = lv;
            currCompany = lv.SelectedItem as Company;
            this.CompanyDetailGrid.DataContext = currCompany;
        }

        private void GenerterProvinceVData()
        {
            if (mProvinceVs == null)
            {
                mProvinceVs = new List<ProvinceV>();
            }
            if (companyNeedRefresh == false)
            {
                return;
            }
            else
            {
                mProvinceVs.Clear();
            }
            List<Province> provinces = new List<Province>();
            String pSql = DbBaseHelper.getSelectSql(DataTabeName.province.ToString());
            DataTable cateDt = DatabaseOPtionHelper.GetInstance().select(pSql);
            if (cateDt.Rows.Count > 0)
            {
                provinces = JsonHelper.DataTableToEntity<Province>(cateDt);
                if (provinces == null || provinces.Count <= 0)
                {
                    FillCompanyAlretData();
                    return;
                }
                Province p = null;
                String condition = string.Empty;
                String sql = string.Empty;
                DataTable mDt = null;
                for (int i = 0; i < provinces.Count; i++)
                {
                    p = provinces[i];
                    condition = CompanyEnum.affiliated_province_id + "=" + Constract.valueSplit + p.id + Constract.valueSplit;
                    sql = DbBaseHelper.getSelectSql(DataTabeName.company.ToString(), null, condition);
                    mDt = DatabaseOPtionHelper.GetInstance().select(sql);
                    List<Company> mlist = JsonHelper.DataTableToEntity<Company>(mDt);
                    if(mlist!=null && mlist.Count > 0)
                    {
                        ProvinceV v = new ProvinceV()
                        {
                            Province = p,
                            Companys = mlist
                        };
                        mProvinceVs.Add(v);
                    }
                }
            }
        }
        private void FillCompanyAlretData()
        {
            TextBlock tb = new TextBlock();
            tb.Text = "没有任何公司信息 右击此处可以刷新  你也可以点击下面的添加按键进行添加！";
            tb.Margin = new Thickness(10.0, 25.0, 10.0, 0);
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Foreground = Brushes.Gray;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            this.MaterialListContentStackPanel.Children.Clear();
            this.MaterialListContentStackPanel.Children.Add(tb);
        }

        private void FillMaterialData()
        {
            GenerterMaterialVData();
            if (mMaterialVs == null || mMaterialVs.Count <= 0)
            {
                //  fill alert into
                FillMaterialAlretData();
            }
            else
            {
                // fill true data
                if (materialNeedRefresh == true)
                {
                    materialNeedRefresh = false;
                    this.MaterialListContentStackPanel.Children.Clear();
                    for (int i = 0; i < mMaterialVs.Count; i++)
                    {
                        Expander expander = new Expander();
                        expander.Style = FindResource(ResourceName.BaseDataExpenderStyle.ToString()) as Style;
                        expander.Header = mMaterialVs[i].Category;
                        expander.HeaderTemplate = FindResource(ResourceName.MaterialBaseDataTemplate.ToString()) as DataTemplate;

                        ListView listView = new ListView();
                        listView.ItemsSource = mMaterialVs[i].Materials;
                        listView.Style = FindResource(ResourceName.BaseDataListViewStyle.ToString()) as Style;
                        listView.ItemContainerStyle = FindResource(ResourceName.ListViewItemMaterialBaseDataStyle.ToString()) as Style;
                        listView.SelectionChanged += ListView_SelectionChanged;
                        expander.Content = listView;
                        this.MaterialListContentStackPanel.Children.Add(expander);
                    }
                }
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            if (lv.SelectedIndex == -1)
            {
                return;
            }
            if (currentListView != lv)
            {
                if (currentListView != null)
                {
                    currentListView.SelectedIndex = -1;
                }
            }
            currentListView = lv;
            currMaterial = lv.SelectedItem as Material;
            this.MatereialDetailInfoGrid.DataContext = currMaterial;
        }

        private void GenerterMaterialVData()
        {
            if (mMaterialVs == null)
            {
                mMaterialVs = new List<MaterialV>();
            }
            if (materialNeedRefresh == false)
            {
                return;
            }
            else
            {
                mMaterialVs.Clear();
            }
            List<MaterialCategory> cates = new List<MaterialCategory>();
            String CategorySql = DbBaseHelper.getSelectSql(DataTabeName.material_category.ToString());
            DataTable cateDt = DatabaseOPtionHelper.GetInstance().select(CategorySql);
            if (cateDt.Rows.Count > 0)
            {
                cates = JsonHelper.DataTableToEntity<MaterialCategory>(cateDt);
                if (cates == null || cates.Count <= 0)
                {
                    FillMaterialAlretData();
                    return;
                }
                MaterialCategory mc = null;
                String condition = string.Empty;
                String sql = string.Empty;
                DataTable mDt = null;
                for (int i = 0; i < cates.Count; i++)
                {
                    mc = cates[i];
                    condition = MaterialEnum.category_id + "=" + Constract.valueSplit + mc.id + Constract.valueSplit;
                    sql = DbBaseHelper.getSelectSql(DataTabeName.material.ToString(), null, condition);
                    mDt = DatabaseOPtionHelper.GetInstance().select(sql);
                    List<Material> mlist = JsonHelper.DataTableToEntity<Material>(mDt);
                    MaterialV v = new MaterialV()
                    {
                        Category = mc,
                        Materials = mlist
                    };
                    mMaterialVs.Add(v);
                }
            }
        }

        private void FillMaterialAlretData()
        {
            TextBlock tb = new TextBlock();
            tb.Text = "没有任何物资  右击此处可以刷新或者添加物资分类  你也可以点击下面的添加按键进行添加！";
            tb.Margin = new Thickness(10.0, 25.0, 10.0, 0);
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Foreground = Brushes.Gray;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            this.MaterialListContentStackPanel.Children.Clear();
            this.MaterialListContentStackPanel.Children.Add(tb);
        }

        private void FillCarData()
        {
            GenerterCarheaderlVData();
            if (mCarHeaderVs == null || mCarHeaderVs.Count <= 0)
            {
                //  fill alert into
                FillCarAlretData();
            }
            else
            {
                // fill true data
                if (carNeedRefresh == false)
                {
                    return;
                }
                carNeedRefresh = false;
                this.CarListContentStackPanel.Children.Clear();
                for (int i = 0; i < mCarHeaderVs.Count; i++)
                {
                    Expander expander = new Expander();
                    expander.Style = FindResource(ResourceName.BaseDataExpenderStyle.ToString()) as Style;
                    expander.Header = mCarHeaderVs[i].head;
                    expander.HeaderTemplate = FindResource(ResourceName.CarBaseDataTemplate.ToString()) as DataTemplate;

                    ListView listView = new ListView() { Name = "CarlistView" };
                    listView.ItemsSource = mCarHeaderVs[i].carInfos;
                    listView.Style = FindResource(ResourceName.BaseDataListViewStyle.ToString()) as Style;
                    listView.ItemContainerStyle = FindResource(ResourceName.ListViewItemCarBaseDataStyle.ToString()) as Style;
                    listView.SelectionChanged += CarlistView_SelectionChanged;
                    expander.Content = listView;
                    this.CarListContentStackPanel.Children.Add(expander);
                }
            }
        }

        private void CarlistView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            if (lv.SelectedIndex == -1)
            {
                return;
            }
            if (currentListView != lv)
            {
                if (currentListView != null)
                {
                    currentListView.SelectedIndex = -1;
                }
            }
            currentListView = lv;
            currCarInfo = lv.SelectedItem as CarInfo;
            this.CarinfoDetailGrid.DataContext = currCarInfo;           
        }

        private void FillCarAlretData()
        {
            TextBlock tb = new TextBlock();
            tb.Text = "没有任何车辆信息, 右击此处可以刷新, 你也可以点击下面的添加按键进行添加！";
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(10.0, 25.0, 10.0, 0);
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Foreground = Brushes.Gray;
            this.CarListContentStackPanel.Children.Clear();
            this.CarListContentStackPanel.Children.Add(tb);
        }
        private void GenerterCarheaderlVData()
        {
            if (mCarHeaderVs == null)
            {
                mCarHeaderVs = new List<CarHeaderV>();
            }
            if (carNeedRefresh == false)
            {
                return;
            }
            else
            {
                mCarHeaderVs.Clear();
            }
            List<CarHeader> cates = new List<CarHeader>();
            String sql = DbBaseHelper.getSelectSql(DataTabeName.car_header.ToString(), null, null, null, null, CarHeaderEnum.content.ToString() + " asc ");
            DataTable Dt = DatabaseOPtionHelper.GetInstance().select(sql);
            if (Dt.Rows.Count > 0)
            {
                cates = JsonHelper.DataTableToEntity<CarHeader>(Dt);
                if (cates == null || cates.Count <= 0)
                {
                    FillMaterialAlretData();
                    return;
                }
                CarHeader ch = null;
                String condition = string.Empty;
                String carsql = string.Empty;
                DataTable mDt = null;
                for (int i = 0; i < cates.Count; i++)
                {
                    ch = cates[i];
                    condition = CarInfoEnum.car_number + " like " + Constract.valueSplit + ch.content + "%" + Constract.valueSplit;
                    sql = DbBaseHelper.getSelectSql(DataTabeName.car_info.ToString(), null, condition);
                    mDt = DatabaseOPtionHelper.GetInstance().select(sql);
                    List<CarInfo> mlist = JsonHelper.DataTableToEntity<CarInfo>(mDt);
                    if (mlist.Count > 0)
                    {
                        CarHeaderV v = new CarHeaderV()
                        {
                            head = ch,
                            carInfos = mlist
                        };
                        mCarHeaderVs.Add(v);
                    }
                }
            }
        }
        #endregion

        #region add 添加

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CompanyTabBtn.IsChecked == true)
            {
                AddCompany();
                return;
            }
            if (MaterialTabBtn.IsChecked == true)
            {
                AddMaterial();
                return;
            }
            if (CarTabBtn.IsChecked == true)
            {
                AddCar();
                return;
            }
        }

        private void AddCompany()
        {
            if (App.GetSoftwareVersion() == SoftwareVersion.localLan.ToString() || App.GetSoftwareVersion() == SoftwareVersion.localSingle.ToString())
            {
                new CompanyAddW() { ParentRefreshData = new Action<bool, bool, bool>(RefreshData) }.ShowDialog();
            }
            else
            {

            }
        }
        private void AddCar()
        {
            new CarAddW() { ParentRefreshData = new Action<bool, bool, bool>(RefreshData) }.ShowDialog();
        }

        private void AddMaterial()
        {
            new MaterialAddW() { ParentRefreshData = new Action<Boolean, Boolean, Boolean>(RefreshData) }.ShowDialog();
        }
        #endregion

        #region add material category 添加物资分类

        private void AddMaterialCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            new MaterialCategoryAddW() { ParentRefreshData = new Action<Boolean, Boolean, Boolean>(RefreshData) }.ShowDialog();
        }
        #endregion

        //menu
        private void AddMaterialCategory_Click(object sender, RoutedEventArgs e)
        {
            new MaterialCategoryAddW() { ParentRefreshData = new Action<bool, bool, bool>(RefreshData) }.ShowDialog();
        }

        #region Refresh data 

        public void RefreshData(Boolean refreshCompany, Boolean refreshMaterial, Boolean refreshCar)
        {
            this.companyNeedRefresh = refreshCompany;
            this.carNeedRefresh = refreshCar;
            this.materialNeedRefresh = refreshMaterial;
            FillData();
        }
        private void RefreshMaterialCategory_Click(object sender, RoutedEventArgs e)
        {
            RefreshMaterialData();
        }

        private void RefreshCarCategory_Click(object sender, RoutedEventArgs e)
        {
            RefreshCarData();
        }

        private void RefreshCompanyCategory_Click(object sender, RoutedEventArgs e)
        {
            RefreshCompanyData();
        }
        private void RefreshCompanyData()
        {
            companyNeedRefresh = true;
            FillCompanyData();
        }
        private void RefreshMaterialData()
        {
            materialNeedRefresh = true;
            FillMaterialData();
        }
        private void RefreshCarData()
        {
            carNeedRefresh = true;
            FillCarData();
        }

        #endregion

        #region Material Update and delete event

        private void MaterialDetailUpdteBtn_Click(object sender, RoutedEventArgs e)
        {
            new MaterialAddW(currMaterial) { ParentRefreshData = new Action<bool, bool, bool>(RefreshData) }.ShowDialog();
        }

        private void DeleteMaterialBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"你确定要删除 {currMaterial.name} 吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                int res = DatabaseOPtionHelper.GetInstance().delete(currMaterial);
                if (res > 0)
                {
                    String cateid = currMaterial.categoryId;
                    new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                    {
                        String condition = MaterialCategoryEnum.id.ToString() + "=" + Constract.valueSplit + cateid + Constract.valueSplit;
                        CommonModel.FieldTryDesc(DataTabeName.material_category.ToString(), MaterialCategoryEnum.children_count.ToString(), 1, condition);
                    })).Start();

                    MessageBox.Show("删除成功！");
                    currMaterial = null;
                    this.MatereialDetailInfoGrid.DataContext = null;
                    materialNeedRefresh = true;
                    FillMaterialData();
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }
            }
            else
            {
                return;
            }
        }
        #endregion

        #region CarInfo Update and Delete Event
        private void CarinfoDetailUpdteBtn_Click(object sender, RoutedEventArgs e)
        {
            new CarAddW(currCarInfo) { ParentRefreshData = new Action<bool, bool, bool>(RefreshData) }.ShowDialog();
        }

        private void DeleteCarInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"你确定要删除 {currCarInfo.carNumber} 吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                int res = DatabaseOPtionHelper.GetInstance().delete(currCarInfo);
                if (res > 0)
                {
                    MessageBox.Show("删除成功！");
                    currMaterial = null;
                    this.CarinfoDetailGrid.DataContext = null;
                    carNeedRefresh = true;
                    FillCarData();
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }
            }
            else
            {
                return;
            }
        }
        #endregion

        #region Company Info Update and Delete Event
        private void DeleteCompanyBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"你确定要删除 {currCompany.name} 吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                int res = DatabaseOPtionHelper.GetInstance().delete(currCompany);
                if (res > 0)
                {                   
                    MessageBox.Show("删除成功！");
                    //new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                    //    string condition = ProvinceEnum.id.ToString() + "=" + Constract.valueSplit + currCompany.affiliatedProvinceId + Constract.valueSplit;
                    //    CommonModel.FieldTryDesc(DataTabeName.province.ToString(), ProvinceEnum.children_count.ToString(), 1, condition);
                    //})).Start();
                    currMaterial = null;
                    this.CompanyDetailGrid.DataContext = null;
                    companyNeedRefresh = true;
                    FillCompanyData();
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }
            }
            else
            {
                return;
            }
        }

        private void CompanyDetailUpdteBtn_Click(object sender, RoutedEventArgs e)
        {
            new CompanyAddW(currCompany) { ParentRefreshData = new Action<bool, bool, bool>(RefreshData) }.ShowDialog();
        }
        #endregion
    }
}
