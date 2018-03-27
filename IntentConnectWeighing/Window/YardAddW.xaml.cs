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
namespace IntentConnectWeighing
{
    /// <summary>
    /// CameraAddW.xaml 的交互逻辑
    ///  CameraAddW.xaml's interactive logical 
    /// </summary>
    public partial class YardAddW : Window
    {
        private List<Company> mCompanys= new List<Company>() { };
        private Yard mYard;
        private Company mCompany;
        public Action<Yard> ParentRefreshData { get; set; }
        private bool isAdd = true;
        public YardAddW(Company company = null, Yard yard = null)
        {
            InitializeComponent();
            if (yard != null)
            {
                mYard = yard;
                isAdd = false;
            }
            else {
                mYard = new Yard() { id=Guid.NewGuid().ToString()};
            }   
            mCompany = company;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (mYard != null && mYard.id != null)
            {
                BindingCurrrData();
            }
        }

        private void BindingCurrrData()
        {
            this.nameTb.Text = mYard.name;
        }

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

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            DatabaseOPtionHelper optionHelper = DatabaseOPtionHelper.GetInstance();
            int res = 0;
            if (isAdd ==false)
            {
                //update
                mYard.name = this.nameTb.Text;
                mYard.syncTime = (Int32)DateTimeHelper.GetTimeStamp();
                res = optionHelper.update(mYard);
                if (res > 0)
                {
                    MessageBox.Show("修改成功！");
                    RefreshParentData();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("修改失败！");
                    return;
                }
            }
            else
            {
                //insert
                if (mYard.isDelete == 1)
                {
                    mYard.isDelete = 0;
                    mYard.deleteTime = null;
                    mYard.syncTime = DateTimeHelper.GetTimeStamp();
                    res = optionHelper.update(mYard);
                }
                else
                {
                    if (String.IsNullOrEmpty(mYard.affiliatedCompanyId) || String.IsNullOrEmpty(mYard.affiliatedCompany))
                    {
                        MessageBox.Show("添加货场名称必须先选择货场的所属公司！");
                        return;
                    }
                    mYard.name = this.nameTb.Text.Trim();
                    mYard.addTime = DateTimeHelper.getCurrentDateTime();
                    mYard.addUserId = App.currentUser.id;
                    mYard.addUserName = App.currentUser.name;
                    mYard.isDelete = 0;
                    mYard.deleteTime = null;
                    mYard.syncTime = DateTimeHelper.GetTimeStamp();
                    res = optionHelper.insert(mYard);
                }
                if (res > 0)
                {
                    MessageBox.Show("保存成功！");                    
                    RefreshParentData();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("保存失败！");
                    return;
                }
            }
        }
    

        //Refresh Parent Data
        private void RefreshParentData()
        {
            if (this.ParentRefreshData != null)
            {
                ParentRefreshData(mYard);
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CheckedExist(String name)
        {
            string @condition = YardEnum.name.ToString() + "=" + Constract.valueSplit + name + Constract.valueSplit
                + " and "
                + YardEnum.affiliated_company_id.ToString() + "+" + Constract.valueSplit + mYard.affiliatedCompanyId + Constract.valueSplit;
            string sql = DbBaseHelper.getSelectSqlNoSoftDeleteCondition(DataTabeName.yard.ToString(), null, condition, null, null, null, 1, 0);
            DataTable dt = DatabaseOPtionHelper.GetInstance().select(sql);
            List<Yard> list = JsonHelper.DataTableToEntity<Yard>(dt);
            if (list == null || list.Count <= 0)
            {
                this.AlertInfoTb.Text = "该货场名称可以添加！";
                this.AlertInfoTb.Foreground = Brushes.Green;
                return;
            }
            else
            {
                mYard = JsonHelper.DataTableToEntity<Yard>(dt)[0];
            }
            if (mYard != null)
            {
                if (mYard.isDelete != 1)
                {
                    this.AlertInfoTb.Text = "该货场已经存在，不需要再添加！";
                    this.nameTb.Focus();
                }
            }
        }

        private void nameTb_LostFocus(object sender, RoutedEventArgs e)
        {
            String name = this.nameTb.Text.Trim();
            if (String.IsNullOrEmpty(name))
            {
                this.saveBtn.IsEnabled = false;
                this.AlertInfoTb.Foreground = Brushes.Gray;
                this.AlertInfoTb.Text = "货场名称建议采用中文,不可以重复！";
                return;
            }
            if (!RegexHelper.IsChineseCharacter(name))
            {
                this.AlertInfoTb.Foreground = Brushes.Red;
                this.AlertInfoTb.Text = "货场名称建议采用中文";
            }
            mYard.name = name;
            this.saveBtn.IsEnabled = true;
            CheckedExist(name);
        }

        private void CateNameCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Company  company = (Company)this.CompanyNameCb.SelectedItem;
            mYard.affiliatedCompanyId = company.id;
            mYard.affiliatedCompany = company.name;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (mCompany == null)
            {
                InitCompanyData();
            }
            else {
                mCompanys.Clear();
                mCompanys.Add(mCompany);
                this.CompanyNameCb.ItemsSource = mCompanys;
                this.CompanyNameCb.SelectedIndex = 0;
            }
             
        }

        private void InitCompanyData()
        {
            String sql = DbBaseHelper.getSelectSql(DataTabeName.company.ToString());
            DataTable dt = DatabaseOPtionHelper.GetInstance().select(sql);
            mCompanys = JsonHelper.DataTableToEntity<Company>(dt);
            this.CompanyNameCb.ItemsSource = mCompanys;
            if (mYard != null) {
                for (int i = 0; i < this.CompanyNameCb.Items.Count; i++)
                {
                    Company company = (Company)this.CompanyNameCb.Items[i];
                    if (company.id == mYard.affiliatedCompanyId)
                    {
                        this.CompanyNameCb.SelectedIndex = i;
                    }
                }
            }          
        }

        private void AddressTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            mYard.address = this.AddressTb.Text.Trim();
        }
    }
}
