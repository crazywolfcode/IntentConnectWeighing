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

        public BaseDataPage()
        {
            InitializeComponent();
        }
        private List<MaterialV> mMaterialVs = null;
        private Boolean materialNeedRefresh = true;
        private Boolean companyNeedRefresh = true;
        private Boolean carNeedRefresh = true;
        private List<CarInfo> mCarInfos = null;
        private Material currMaterial;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.scrollViewer.Height = this.scrollViewer.ActualHeight - 30;

            FillData();
        }
        #region 填充数据

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
            string path = Constract.templatePath + "TestItem.xaml";
            this.CompanyListContentStackPanel.Children.Clear();
            for (int i = 0; i < 10; i++)
            {
                FrameworkElement element = TemplateHelper.GetFrameworkElementFromXaml(path);
                Button addbtn;
                addbtn = element.FindName("AddBtn") as Button;
                addbtn.Tag = "ButtonTag" + i;
                addbtn.Command = AddCommand.ShowSelfTagCommand;
                addbtn.CommandBindings.Add(AddCommand.ShowSelfTagCommandBinding);
                this.CompanyListContentStackPanel.Children.Add(element);
            }
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
            DataTable cateDt = new DatabaseOPtionHelper().select(CategorySql);
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
                    mDt = new DatabaseOPtionHelper().select(sql);
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
            if (mCarInfos == null || mCarInfos.Count <= 0)
            {
                //  fill alert into
                TextBlock tb = new TextBlock();
                tb.Text = "没有任何车辆信息\n 右击此处可以刷新/n 你也可以点击下面的添加按键进行添加！";
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Margin = new Thickness(10.0, 25.0, 10.0, 0);
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.Foreground = Brushes.Gray;
                this.CarListContentStackPanel.Children.Clear();
                this.CarListContentStackPanel.Children.Add(tb);
            }
            else
            {
                // fill true data

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
            MessageBox.Show("公司信息不可这样添加，需要对方公司自己注册。");
        }
        private void AddCar() { }

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

        private void TabBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                FillData();
            }
        }

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
                int res = new DatabaseOPtionHelper().delete(currMaterial);
                if (res > 0)
                {
                    String cateid = currMaterial.categoryId;
                    new System.Threading.Thread(new System.Threading.ThreadStart(() => {
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
    }
}
