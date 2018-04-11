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
namespace IntentConnectWeighing
{
    /// <summary>
    /// CameraAddW.xaml 的交互逻辑
    ///  CameraAddW.xaml's interactive logical 
    /// </summary>
    public partial class CompanyAddW : Window
    {
        private Company SourceCompany;//传递过来的Company 用作修改
        private int OldPrpvinceId = -1;
        private Company mCompany;
        private Company mPersonCompany;
        private User PersonBose;
        private Province mProvince;
        private Province mPersonProvince;
        private List<Province> mProvinces;
        public Action<Boolean, Boolean, Boolean> ParentRefreshData { get; set; }
        private bool IsCompnayCanOption = false;
        private bool IsPersonCompnayCanOption = false;
        public CompanyAddW(Company company = null)
        {
            InitializeComponent();
            SourceCompany = company;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (SourceCompany == null)
            {              
                mCompany = new Company
                {
                    id = Guid.NewGuid().ToString(),
                    addtime = DateTimeHelper.getCurrentDateTime(),
                    syncTime = DateTimeHelper.GetTimeStamp()
                };
                mPersonCompany = new Company
                {
                    id = Guid.NewGuid().ToString(),
                    addtime = DateTimeHelper.getCurrentDateTime(),
                    syncTime = DateTimeHelper.GetTimeStamp()
                };
                PersonBose = new User()
                {
                    id = Guid.NewGuid().ToString(),
                    userType = 1,
                    roleLevel = (int)UserRoleLevel.Boss,
                    addtime = DateTimeHelper.getCurrentDateTime(),
                    addUserId = App.currentUser.id,
                    addUserName = App.currentUser.name,
                    syncTime = DateTimeHelper.GetTimeStamp()
                };
            }
            else
            {
                OldPrpvinceId = SourceCompany.affiliatedProvinceId;
                if (SourceCompany.customerType == (int)CompanyCustomerTyle.Company)
                {
                    mCompany = SourceCompany;
                }
                else
                {
                    mPersonCompany = SourceCompany;
                     GetPersonBose();
                }
                HideNoUpdatePenel();
                BindingCurrCompany();
            }
        }
        /// <summary>
        /// 隐藏不需要修改布局
        /// </summary>
        private void HideNoUpdatePenel()
        {
            if (SourceCompany.customerType == (int)CompanyCustomerTyle.Company)
            {
                this.PensonRB.IsChecked = false;
                this.PensonRB.Visibility = Visibility.Collapsed;
                this.CompanyRB.IsChecked = true;
            }
            else
            {
                this.CompanyRB.IsChecked = false;
                this.CompanyRB.Visibility = Visibility.Collapsed;
                this.PensonRB.IsChecked = true;
            }
        }

        private void BindingCurrCompany()
        {
            if (CompanyRB.IsChecked == true)
            {
                this.FullNameTB.Text = mCompany.name;
                this.LicenceNumberTB.Text = mCompany.busineseLincenseNumber;
                this.AbbreviationTB.Text = mCompany.abbreviation;
                this.LegalPersonTB.Text = mCompany.legalPerson;       
                this.AddressTb.Text = mCompany.address;
            }
            else
            {
                this.NameTB.Text = mPersonCompany.name;
                this.IdNumberTB.Text = mPersonCompany.busineseLincenseNumber;           
                this.AddressTb.Text = mPersonCompany.address;
            }
        }

        private void GetPersonBose()
        {
            String Condition = UserEnum.id_number + "=" + Constract.valueSplit + mPersonCompany.busineseLincenseNumber + Constract.valueSplit;
            String Sql = DbBaseHelper.getSelectSql(DataTabeName.user.ToString(), null, Condition);
            List<User> list = JsonHelper.DataTableToEntity<User>(DatabaseOPtionHelper.GetInstance().select(Sql));
            if (list != null && list.Count > 0)
            {
                PersonBose = list[0];
            }
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            InitProvince();
            if (SourceCompany == null)
            {
                this.ProvinceCmb.ItemsSource = mProvinces;
                this.ProvinceCmb.DisplayMemberPath = "provinceName";
                this.ProvinceCmb.SelectedValuePath = "id";
                this.CompanyProvinceCmb.ItemsSource = mProvinces;
                this.CompanyProvinceCmb.DisplayMemberPath = "provinceName";
                this.CompanyProvinceCmb.SelectedValuePath = "id";
            }
            else
            {
                if (SourceCompany.customerType == (int)CompanyCustomerTyle.Company)
                {
                    this.CompanyProvinceCmb.ItemsSource = mProvinces;
                    this.CompanyProvinceCmb.DisplayMemberPath = "provinceName";
                    this.CompanyProvinceCmb.SelectedValuePath = "id";
                    for (int i = 0; i < mProvinces.Count; i++)
                    {
                        if (mProvinces[i].id == SourceCompany.affiliatedProvinceId) {
                            this.CompanyProvinceCmb.SelectedIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    this.ProvinceCmb.ItemsSource = mProvinces;
                    this.ProvinceCmb.DisplayMemberPath = "provinceName";
                    this.ProvinceCmb.SelectedValuePath = "id";
                    for (int i = 0; i < mProvinces.Count; i++)
                    {
                        if (mProvinces[i].id == SourceCompany.affiliatedProvinceId)
                        {
                            this.ProvinceCmb.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void InitProvince()
        {
            mProvinces = ProvinceModel.GetProvince();
        }
        #region Window event
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
        #endregion

        //Refresh Parent Data
        private void RefreshParentData()
        {
            if (this.ParentRefreshData != null)
            {
                ParentRefreshData(false, false, true);
            }
        }

        #region 保存 

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CompanyRB.IsChecked == true)
            {
                if (IsCompnayCanOption == false)
                {
                    return;
                }
            }
            if (PensonRB.IsChecked == true)
            {
                if (IsPersonCompnayCanOption == false)
                {
                    return;
                }
            }

            if (SourceCompany != null)
            {
                Update();
            }
            else
            {
                Insert();
            }
        }

        private void Update()
        {
            if (!ValidateInput())
            {
                return;
            }
            if (CompanyRB.IsChecked == true)
            {
                UpdateCompany();
            }
            else
            {
                UpdatePersonCompany();
            }
        }

        private void Insert()
        {
            if (!ValidateInput())
            {
                return;
            }
            if (CompanyRB.IsChecked == true)
            {
                InsertCompany();
            }
            else
            {
                InsertPersonCompany();
            }
        }

        /// <summary>
        /// 验证输入的信息
        /// </summary>
        private bool ValidateInput()
        {
            if (PensonRB.IsChecked == true)
            {
                if (string.IsNullOrEmpty(mPersonCompany.name))
                {
                    MessageBox.Show("姓名输入不正确！");
                    this.NameTB.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(mPersonCompany.busineseLincenseNumber))
                {
                    MessageBox.Show("证件号输入不正确！");
                    this.IdNumberTB.Focus();
                    return false;
                }
                if (String.IsNullOrEmpty(mPersonCompany.affiliatedProvince) || mPersonCompany.affiliatedProvinceId <= 0)
                {
                    MessageBox.Show("所属省份必须选择！");
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(mCompany.name))
                {
                    MessageBox.Show("公司名称输入不正确！");
                    this.FullNameTB.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(mCompany.busineseLincenseNumber))
                {
                    MessageBox.Show("执照编号输入不正确！");
                    this.LicenceNumberTB.Focus();
                    return false;
                }
                if (String.IsNullOrEmpty(mCompany.affiliatedProvince) || mCompany.affiliatedProvinceId <= 0)
                {
                    MessageBox.Show("所属省份必须选择！");
                    return false;
                }
                if (String.IsNullOrEmpty(mCompany.legalPerson))
                {
                    MessageBox.Show("法人输入不正确！");
                    this.LegalPersonTB.Focus();
                    return false;
                }
                if (!String.IsNullOrEmpty(this.AddressTb.Text))
                {
                    mCompany.address = this.AddressTb.Text.Trim();
                }
            }
            return true;
        }

        private void UpdateCompany()
        {
            int res = 0;
            res = DatabaseOPtionHelper.GetInstance().update(mCompany);
            if (res > 0)
            {
                if (OldPrpvinceId !=(int)this.CompanyProvinceCmb.SelectedValue && (int)this.CompanyProvinceCmb.SelectedValue>0)
                {
                    ChangeChildrenCount(OldPrpvinceId,false);
                    ChangeChildrenCount(mCompany.affiliatedProvinceId);
                }
                MessageBox.Show("修改成功！");
                this.Close();
                this.ParentRefreshData(true, false, false);
            }
            else
            {
                MessageBox.Show("修改失败！");
            }
        }

        private void UpdatePersonCompany()
        {
            BuildBoss();
            int res = 0;
            String[] sqls=null;
            if (PersonBose != null)
            {
                sqls = new string[] {
                     DbBaseHelper.getUpdateSql(mPersonCompany),
                    DbBaseHelper.getUpdateSql(PersonBose)
                };
            }
            else {
                sqls = new string[] {
                     DbBaseHelper.getUpdateSql(mPersonCompany),
                };
            }     
            res = DatabaseOPtionHelper.GetInstance().TransactionExecute(sqls);
            if (res > 0)
            {
                if (OldPrpvinceId != (int)this.ProvinceCmb.SelectedValue && (int)this.ProvinceCmb.SelectedValue>0)
                {
                    ChangeChildrenCount(OldPrpvinceId,false);
                    ChangeChildrenCount(mPersonCompany.affiliatedProvinceId);
                }
                MessageBox.Show("修改成功！");
                this.Close();
                this.ParentRefreshData(true, false, false);
            }
            else
            {
                MessageBox.Show("修改失败！");
            }
        }

        private void InsertCompany()
        {
            int res = 0;
            mCompany.customerType = (int)CompanyCustomerTyle.Company;
            res = DatabaseOPtionHelper.GetInstance().insert(mCompany);
            if (res > 0)
            {
                ChangeChildrenCount();
                MessageBox.Show("增加成功！");
                this.Close();
                this.ParentRefreshData(true, false, false);
            }
            else
            {
                MessageBox.Show("增加失败！");
            }
        }

        private void InsertPersonCompany()
        {
            BuildBoss();
            int res = 0;
            mCompany.customerType = (int)CompanyCustomerTyle.Person;
            String[] sqls = new string[] {
                DbBaseHelper.getInsertSql(mPersonCompany),
                DbBaseHelper.getInsertSql(PersonBose)
            };
            res = DatabaseOPtionHelper.GetInstance().TransactionExecute(sqls);
            if (res > 0)
            {
                ChangeChildrenCount();
                MessageBox.Show("增加成功！");
                this.Close();
                this.ParentRefreshData(true, false, false);
            }
            else
            {
                MessageBox.Show("增加失败！");
            }
        }
        /// <summary>
        /// Change province ChildrenCount
        /// </summary>
        /// <param name="isAdd"></param>
        private void ChangeChildrenCount(int provinceId = 0, bool isAdd = true)
        {
            return;
            String condition = string.Empty;
            int id = provinceId;
            if (provinceId != 0)
            {
                if (CompanyRB.IsChecked == true)
                {
                    id = mCompany.affiliatedProvinceId;
                }
                else
                {
                    id = mPersonCompany.affiliatedProvinceId; ;
                }
            }
            condition = ProvinceEnum.id.ToString() + "=" + Constract.valueSplit + id + Constract.valueSplit;
            if (isAdd == true)
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    CommonModel.FieldTryAsc(DataTabeName.province.ToString(), ProvinceEnum.children_count.ToString(), 1, condition);
                })).Start();
            }
            else
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    CommonModel.FieldTryDesc(DataTabeName.province.ToString(), ProvinceEnum.children_count.ToString(), 1, condition);
                })).Start();
            }

        }

        private void BuildBoss()
        {
            if (SourceCompany == null)
            {
                PersonBose.name = mPersonCompany.name;
                try
                {
                    PersonBose.loginName = StringHelper.GetAllSpell(PersonBose.name).Replace(" ", "").ToLower();
                }
                catch { }
                PersonBose.idNumber = mPersonCompany.busineseLincenseNumber;
                PersonBose.passwprd = EncryptHelper.MD5Encrypt(ConfigurationHelper.GetConfig(ConfigItemName.defaultPwd.ToString()));
                PersonBose.syncTime = DateTimeHelper.GetTimeStamp();
                PersonBose.company = mPersonCompany.name;
                PersonBose.affiliatedCompanyId = mPersonCompany.id;
                PersonBose.isDelete = 0;
                PersonBose.remark = "增加个人类型的公司时自动生成";
            }
            else
            {
                if (PersonBose == null)
                {
                    return;
                }
                PersonBose.name = mPersonCompany.name;
                try
                {
                    PersonBose.loginName = StringHelper.GetAllSpell(PersonBose.name).Replace(" ", "").ToLower();
                }
                catch { }
                PersonBose.company = mPersonCompany.name;
                PersonBose.syncTime = DateTimeHelper.GetTimeStamp();
            }
        }
        #endregion

        #region Company Input Event

        private void CompanyProvinceCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.CompanyProvinceCmb.SelectedIndex > -1)
            {
                mProvince = (Province)this.CompanyProvinceCmb.SelectedItem;
                if (mProvince != null)
                {
                    mCompany.affiliatedProvinceId = mProvince.id;
                    mCompany.affiliatedProvince = mProvince.provinceName;
                }
            }
        }
        private String currLicence = string.Empty;
        private String currCompany = String.Empty;
        private void FullNameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            currCompany = this.FullNameTB.Text.Trim();
            if (currCompany.Length < 4)
            { return; }
            mCompany.name = currCompany;
            CheckCompanyExist();
        }

        private void LicenceNumberTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            currLicence = this.LicenceNumberTB.Text.Trim();
            if (currLicence.Length < 8)
            {
                return;
            }
            mCompany.busineseLincenseNumber = currLicence;
            CheckCompanyExist();
        }
        private void CheckCompanyExist()
        {
            if (string.IsNullOrEmpty(currCompany) || String.IsNullOrEmpty(currLicence))
            {
                return;
            }
            String @condition = CompanyEnum.name.ToString() + "+" + Constract.valueSplit + currCompany + Constract.valueSplit +
                " and " + CompanyEnum.businese_lincense_number.ToString() + " = " + Constract.valueSplit + currLicence + Constract.valueSplit;
            String sql = DbBaseHelper.getSelectSql(DataTabeName.company.ToString(), null, condition);
            List<Company> list = JsonHelper.DataTableToEntity<Company>(DatabaseOPtionHelper.GetInstance().select(sql));
            if (list != null && list.Count > 0)
            {
                if (list[0].isDelete == 1)
                {
                    mCompany = list[0];
                    mCompany.isDelete = 0;
                    mCompany.deleteTime = null;
                    IsCompnayCanOption = true;
                }
                else
                {
                    MessageBox.Show("该公司信息已经存在，不可以再添加！");
                    IsCompnayCanOption = false;
                    return;
                }
            }
            else
            {
                IsCompnayCanOption = true;
            }
        }

        private void AbbreviationTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            mCompany.abbreviation = this.AbbreviationTB.Text;
            if (!String.IsNullOrEmpty(mCompany.abbreviation))
            {
                try
                {
                    mCompany.abbreviationFirstCase = StringHelper.GetFirstPinyin(mCompany.abbreviation);
                }
                catch { }
            }
        }

        private void LegalPersonTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            String legalPerson = this.LegalPersonTB.Text.Trim();
            if (legalPerson.Length >= 2)
            {
                mCompany.legalPerson = legalPerson;
            }
        }
        #endregion

        #region person Company Input Event

        private void ProvinceCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ProvinceCmb.SelectedIndex > -1)
            {
                mPersonProvince = (Province)this.ProvinceCmb.SelectedItem;
                if (mPersonProvince != null)
                {
                    mPersonCompany.affiliatedProvinceId = mPersonProvince.id;
                    mPersonCompany.affiliatedProvince = mPersonProvince.provinceName;
                }
            }
        }
        private string currPersonCompanyName = String.Empty;
        private string currPersonCompanyIdNumber = String.Empty;
        private void NameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            string str = this.NameTB.Text.Trim();
            if (str.Length >= 2)
            {
                currPersonCompanyName = str;
                mPersonCompany.name = currPersonCompanyName;
            }
            CheckPersonCompanyExist();
        }

        private void IdNumberTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            string str = this.IdNumberTB.Text.Trim();
            if (str.Length >= 8)
            {
                currPersonCompanyIdNumber = str;
                mPersonCompany.busineseLincenseNumber = currPersonCompanyIdNumber;
            }
            CheckPersonCompanyExist();
        }

        private void CheckPersonCompanyExist()
        {
            if (string.IsNullOrEmpty(currPersonCompanyName) || String.IsNullOrEmpty(currPersonCompanyIdNumber))
            {
                return;
            }
            String @condition = CompanyEnum.name.ToString() + "+" + Constract.valueSplit + currPersonCompanyName + Constract.valueSplit +
              " and " + CompanyEnum.businese_lincense_number.ToString() + " = " + Constract.valueSplit + currPersonCompanyIdNumber + Constract.valueSplit;
            String sql = DbBaseHelper.getSelectSql(DataTabeName.company.ToString(), null, condition);
            List<Company> list = JsonHelper.DataTableToEntity<Company>(DatabaseOPtionHelper.GetInstance().select(sql));
            if (list != null && list.Count > 0)
            {
                if (list[0].isDelete == 1)
                {
                    mPersonCompany = list[0];
                    mPersonCompany.isDelete = 0;
                    mPersonCompany.deleteTime = null;
                    IsPersonCompnayCanOption = true;
                }
                else
                {
                    MessageBox.Show("信息已经存在，不可以再添加");
                    IsPersonCompnayCanOption = false;
                    return;
                }
            }
            else
            {
                IsPersonCompnayCanOption = true;
            }
        }

        #endregion

        #region 公司类型选择
        private void privateRb_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                mCompany.type = (int)CompanyType.PrivaterEnterprise;
            }
        }

        private void StateOwnedRb_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                mCompany.type = (int)CompanyType.StateOwnedEnterprise;
            }
        }

        private void governmentRB_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                mCompany.type = (int)CompanyType.GovernmentSector;
            }
        }
        #endregion
    }
}
