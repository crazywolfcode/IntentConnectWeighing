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
    public partial class CarAddW : Window
    {
        private CarInfo mCarInfo;
        private VchicleLicense mVchicleLicense;
        private DrivingLicense mDrivingLicense;
        private User mDriver;
        private bool isUpdate = false;
        public Action<Boolean, Boolean, Boolean> ParentRefreshData { get; set; }
        public CarAddW(CarInfo carInfo = null)
        {
            InitializeComponent();
            mCarInfo = carInfo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (mCarInfo == null)
            {
                mCarInfo = new CarInfo() { id = Guid.NewGuid().ToString() };
                mVchicleLicense = new VchicleLicense() { id = Guid.NewGuid().ToString() };
                mDrivingLicense = new DrivingLicense() { id = Guid.NewGuid().ToString() };
                mDriver = new User() { id = Guid.NewGuid().ToString() };
            }
            else
            {
                isUpdate = true;
                getVchileLicense();
                getDriver();
                getDrivingLicense();
                bindingVchileLicenseinfo();
                bindingDrivingLicenseinfo();
            }
        }

        private void getVchileLicense()
        {
            String conditon = VchicleLicenseEnum.id.ToString() + "=" + Constract.valueSplit + mCarInfo.vehicleId + Constract.valueSplit;
            String sql = DbBaseHelper.getSelectSql(DataTabeName.vchicle_license.ToString(), null, conditon);
            DataTable dataTable = DatabaseOPtionHelper.GetInstance().select(sql);
            List<VchicleLicense> list = JsonHelper.DataTableToEntity<VchicleLicense>(dataTable);
            if (list != null && list.Count > 0)
            {
                mVchicleLicense = list[0];
            }
            else
            {
                MessageBox.Show("获取行驶证信息出错！" + sql);
            }
        }
        private void getDrivingLicense()
        {
            String conditon = DrivingLicenseEnum.affiliated_user_id.ToString() + "=" + Constract.valueSplit + mDriver.id + Constract.valueSplit;
            String sql = DbBaseHelper.getSelectSql(DataTabeName.driving_license.ToString(), null, conditon);
            DataTable dataTable = DatabaseOPtionHelper.GetInstance().select(sql);
            List<DrivingLicense> list = JsonHelper.DataTableToEntity<DrivingLicense>(dataTable);
            if (list != null && list.Count > 0)
            {
                mDrivingLicense = list[0];
            }
            else
            {
                MessageBox.Show("获取驾驶证信息出错！" + sql);
            }
        }
        private void getDriver()
        {
            String conditon = UserEnum.id_number.ToString() + "=" + Constract.valueSplit + mCarInfo.driverIdnumber + Constract.valueSplit;
            String sql = DbBaseHelper.getSelectSql(DataTabeName.user.ToString(), null, conditon);
            DataTable dataTable = DatabaseOPtionHelper.GetInstance().select(sql);
            List<User> list = JsonHelper.DataTableToEntity<User>(dataTable);
            if (list != null && list.Count > 0)
            {
                mDriver = list[0];
            }
            else
            {
                MessageBox.Show("获取驾驶员信息出错！" + sql);
            }
        }

        private void bindingVchileLicenseinfo()
        {
            if (mVchicleLicense != null && mVchicleLicense.id != null)
            {
                this.CarNumberTb.Text = mVchicleLicense.carNumber;
                this.CarTypeTb.Text = mVchicleLicense.carType;
                this.OwnerTb.Text = mVchicleLicense.owner;
                this.AddressTb.Text = mVchicleLicense.address;
                this.userTypeTb.Text = mVchicleLicense.useCharacter;
                this.BrandTypeTb.Text = mVchicleLicense.brand;
                this.VinTb.Text = mVchicleLicense.vin;
                this.EngineNumberTb.Text = mVchicleLicense.engineNumber;
                this.RegisterDateTb.Text = mVchicleLicense.registerDate;
                this.IssueDateTb.Text = mVchicleLicense.issueDate;
            }
        }
        private void bindingDrivingLicenseinfo()
        {
            if (mDrivingLicense != null && mDrivingLicense.id != null)
            {
                this.NameTb.Text = mDrivingLicense.name;
                this.NumberTb.Text = mDrivingLicense.number;
                ((this.SexCob.Items[mDrivingLicense.sex]) as ComboBoxItem).IsSelected = true;
                this.NationTb.Text = mDrivingLicense.nationality;
                this.DrivingAddressTb.Text = mDrivingLicense.address;
                this.BrithdayTb.Text = mDrivingLicense.birthday;
                this.FirstTimeTb.Text = mDrivingLicense.firstTime;
                this.DriverClassTb.Text = mDrivingLicense.driverClass;
                this.EffictYearTb.Text = mDrivingLicense.effictYear.ToString();
                this.DrivingStartTimeTb.Text = mDrivingLicense.startTime;
                this.DrivingEndTimeTb.Text = mDrivingLicense.endTime;

                this.DrivingPhoneTb.Text = mCarInfo.driverMobile;
            }
        }



        private void Window_ContentRendered(object sender, EventArgs e)
        {

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
            if (isUpdate == true)
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
            if (ValidateInput() == false)
            {
                return;
            }
            BuildUserInfo();
            BuilderCarInfo();
            DatabaseOPtionHelper helper = DatabaseOPtionHelper.GetInstance();
            try
            {
                int res = 0;
                String[] sqls = new string[] {
                    DbBaseHelper.getUpdateSql(mDriver),
                    DbBaseHelper.getUpdateSql(mCarInfo),
                    DbBaseHelper.getUpdateSql(mVchicleLicense),
                    DbBaseHelper.getUpdateSql(mDrivingLicense)
                };
                res = helper.TransactionExecute(sqls);
                if (res > 0)
                {
                    MessageBox.Show("修改成功！");
                    AddCarHeader();
                    this.Close();
                    this.ParentRefreshData(false, false, true);
                }
                else
                {
                    MessageBox.Show("修改出错!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("修改出错：" + ex.Message);
                return;
            }

        }

        private void Insert()
        {
            if (ValidateInput() == false)
            {
                return;
            }
            BuildUserInfo();
            BuilderCarInfo();
            mDrivingLicense.affiliatedUserId = mDriver.id;
            mVchicleLicense.affiliatedCarId = mCarInfo.id;
            DatabaseOPtionHelper helper = DatabaseOPtionHelper.GetInstance();
            try
            {
                int res = 0;
                String[] sqls = new string[] {
                    DbBaseHelper.getInsertSql(mDriver),
                    DbBaseHelper.getInsertSql(mCarInfo),
                    DbBaseHelper.getInsertSql(mVchicleLicense),
                    DbBaseHelper.getInsertSql(mDrivingLicense)
                };
                res = helper.TransactionExecute(sqls);
                if (res > 0)
                {
                    MessageBox.Show("保存成功！");
                    AddCarHeader();
                    this.Close();
                    this.ParentRefreshData(false, false, true);
                }
                else
                {
                    MessageBox.Show("保存失败!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存出错：" + ex.Message);
                return;
            }
        }

        private void BuildUserInfo()
        {
            mDriver.name = mDrivingLicense.name;
            mDriver.loginName = (StringHelper.GetAllSpell(mDriver.name).Replace(" ", "")).ToLower();
            mDriver.passwprd = EncryptHelper.MD5Encrypt( ConfigurationHelper.GetConfig(ConfigItemName.defaultPwd.ToString()));
            mDriver.addUserId = App.currentUser.id;
            mDriver.addUserName = App.currentUser.name;
            mDriver.birthDate = mDrivingLicense.birthday;
            mDriver.mobilephone = mCarInfo.driverMobile;
            mDriver.sex = mDrivingLicense.sex;
            mDriver.status = 0;
            mDriver.syncTime = DateTimeHelper.GetTimeStamp();
            mDriver.roleLevel = 0;
            mDriver.userType = (int)UserType.Driver;
            mDriver.address = mDrivingLicense.address;
            mDriver.idNumber = mDrivingLicense.number;
            mDriver.remark = "添加车辆信息时系统自动生成的";
        }
        private void BuilderCarInfo()
        {
            mCarInfo.carNumber = mVchicleLicense.carNumber;
            mCarInfo.driver = mDrivingLicense.name;
            mCarInfo.driverIdnumber = mDrivingLicense.number;
            mCarInfo.ownerId = null;
            mCarInfo.ownerName = mVchicleLicense.owner;
            mCarInfo.syncTime = DateTimeHelper.GetTimeStamp();
            mCarInfo.vehicleId = mVchicleLicense.id;
            mCarInfo.status = 0;
            mCarInfo.addUserId = App.currentUser.id;
            mCarInfo.addUserName = App.currentUser.name;
            mCarInfo.remark = "添加车辆信息时系统自动生成的";
        }

        private void AddCarHeader()
        {
            String header = mCarInfo.carNumber.Substring(0, 2);
            String condition = CarHeaderEnum.content.ToString() + "=" + Constract.valueSplit + header + Constract.valueSplit;
            String sql = DbBaseHelper.getSelectSqlNoSoftDeleteCondition(DataTabeName.car_header.ToString(), CarHeaderEnum.content.ToString(), condition);
            DataTable dataTable = DatabaseOPtionHelper.GetInstance().select(sql);
            if (dataTable.Rows.Count <= 0)
            {
                CarHeader carHeader = new CarHeader()
                {
                    id = Guid.NewGuid().ToString(),
                    content = header,
                    syncTime = DateTimeHelper.GetTimeStamp()
                };
                DatabaseOPtionHelper.GetInstance().insert(carHeader);
            }
        }


        /// <summary>
        /// 验证输入的信息
        /// </summary>
        private bool ValidateInput()
        {
            //vchicle
            if (String.IsNullOrEmpty(mVchicleLicense.carNumber))
            {
                MessageBox.Show("车牌号为空或者输入的车牌号无效！");
                vchicleTabBtn.IsChecked = true;
                this.CarNumberTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mVchicleLicense.carType))
            {
                MessageBox.Show("车辆类型必须和行驶证上的一至！");
                vchicleTabBtn.IsChecked = true;
                this.CarTypeTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mVchicleLicense.owner))
            {
                MessageBox.Show("车辆所有人必须和行驶证上的一至！");
                vchicleTabBtn.IsChecked = true;
                this.OwnerTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mVchicleLicense.address))
            {
                MessageBox.Show("住址必须和行驶证上的一至！");
                vchicleTabBtn.IsChecked = true;
                this.AddressTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mVchicleLicense.useCharacter))
            {
                MessageBox.Show("使用性质必须和行驶证上的一至！");
                vchicleTabBtn.IsChecked = true;
                this.userTypeTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mVchicleLicense.brand))
            {
                MessageBox.Show("品牌型号必须和行驶证上的一至！");
                vchicleTabBtn.IsChecked = true;
                this.BrandTypeTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mVchicleLicense.vin))
            {
                MessageBox.Show("识别代码必须和行驶证上的一至！");
                vchicleTabBtn.IsChecked = true;
                this.VinTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mVchicleLicense.engineNumber))
            {
                MessageBox.Show("发动机号必须和行驶证上的一至！");
                vchicleTabBtn.IsChecked = true;
                this.EngineNumberTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mVchicleLicense.registerDate))
            {
                MessageBox.Show("注册日期必须和行驶证上的一至！");
                vchicleTabBtn.IsChecked = true;
                this.RegisterDateTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mVchicleLicense.issueDate))
            {
                MessageBox.Show("发证日期必须和行驶证上的一至！");
                vchicleTabBtn.IsChecked = true;
                this.IssueDateTb.Focus();
                return false;
            }
            // driving 
            if (String.IsNullOrEmpty(mDrivingLicense.name))
            {
                MessageBox.Show("姓名必须和行驶证上的一至！");
                DirvingTabBtn.IsChecked = true;
                this.NameTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mDrivingLicense.number))
            {
                MessageBox.Show("证件号必须和行驶证上的一至！");
                DirvingTabBtn.IsChecked = true;
                this.NumberTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mDrivingLicense.nationality))
            {
                MessageBox.Show("国籍必须和行驶证上的一至！");
                DirvingTabBtn.IsChecked = true;
                this.NationTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mDrivingLicense.address))
            {
                MessageBox.Show("住址必须和行驶证上的一至！");
                DirvingTabBtn.IsChecked = true;
                this.DrivingAddressTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mDrivingLicense.birthday))
            {
                MessageBox.Show("出生日期必须和行驶证上的一至！");
                DirvingTabBtn.IsChecked = true;
                this.BrithdayTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mDrivingLicense.firstTime))
            {
                MessageBox.Show("初始领证日期必须和行驶证上的一至！");
                DirvingTabBtn.IsChecked = true;
                this.FirstTimeTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mDrivingLicense.driverClass))
            {
                MessageBox.Show("准驾车型必须和行驶证上的一至！");
                DirvingTabBtn.IsChecked = true;
                this.DriverClassTb.Focus();
                return false;
            }
            if (mDrivingLicense.effictYear <= 0)
            {
                MessageBox.Show("有效期限必须和行驶证上的一至！");
                DirvingTabBtn.IsChecked = true;
                this.EffictYearTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mDrivingLicense.startTime))
            {
                MessageBox.Show("起始日期必须和行驶证上的一至！");
                DirvingTabBtn.IsChecked = true;
                this.DrivingStartTimeTb.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(mDrivingLicense.endTime))
            {
                MessageBox.Show("失效日期必须和行驶证上的一至！");
                DirvingTabBtn.IsChecked = true;
                this.DrivingEndTimeTb.Focus();
                return false;
            }
            //car infor
            if (String.IsNullOrEmpty(mCarInfo.driverMobile))
            {
                MessageBox.Show("手机号码必须正确输入！");
                DirvingTabBtn.IsChecked = true;
                this.DrivingPhoneTb.Focus();
                return false;
            }
            // user
            return true;
        }
        #endregion

        #region Vchicle  Text Changed
        private void CarNumberTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string carnumber = this.CarNumberTb.Text.Trim();
            if (!String.IsNullOrEmpty(carnumber))
            {
                if (RegexHelper.IsVehicleNumber(carnumber))
                {
                    mVchicleLicense.carNumber = carnumber;
                }
            }
        }

        private void CarTypeTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string cartype = this.CarTypeTb.Text.Trim();
            if (!String.IsNullOrEmpty(cartype))
            {
                mVchicleLicense.carType = cartype;
            }
        }

        private void OwnerTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string owner = this.OwnerTb.Text.Trim();
            if (!String.IsNullOrEmpty(owner))
            {
                mVchicleLicense.owner = owner;
            }
        }

        private void AddressTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string address = this.AddressTb.Text.Trim();
            if (!String.IsNullOrEmpty(address))
            {
                mVchicleLicense.address = address;
            }
        }

        private void userTypeTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ut = this.userTypeTb.Text.Trim();
            if (!String.IsNullOrEmpty(ut))
            {
                mVchicleLicense.useCharacter = ut;
            }
        }

        private void BrandTypeTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string bt = this.BrandTypeTb.Text.Trim();
            if (!String.IsNullOrEmpty(bt))
            {
                mVchicleLicense.brand = bt;
            }
        }

        private void VinTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string vin = this.VinTb.Text.Trim();
            if (!String.IsNullOrEmpty(vin))
            {
                mVchicleLicense.vin = vin;
            }
        }

        private void EngineNumberTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string en = this.EngineNumberTb.Text.Trim();
            if (!String.IsNullOrEmpty(en))
            {
                mVchicleLicense.engineNumber = en;
            }
        }

        private void RegisterDateTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string rd = this.RegisterDateTb.Text.Trim();
            if (!String.IsNullOrEmpty(rd))
            {
                mVchicleLicense.registerDate = rd;
            }
        }

        private void IssueDateTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string idt = this.IssueDateTb.Text.Trim();
            if (!String.IsNullOrEmpty(idt))
            {
                mVchicleLicense.issueDate = idt;
            }
        }
        #endregion

        #region driving license text changed
        private void NameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string name = this.NameTb.Text.Trim();
            if (!String.IsNullOrEmpty(name))
            {
                mDrivingLicense.name = name;
            }
        }

        private void NumberTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string number = this.NumberTb.Text.Trim();
            if (!String.IsNullOrEmpty(number))
            {
                mDrivingLicense.number = number;
            }
        }

        private void SexCob_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                mDrivingLicense.sex = this.SexCob.SelectedIndex;
            }
        }

        private void NationTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string nation = this.NationTb.Text.Trim();
            if (!String.IsNullOrEmpty(nation))
            {
                mDrivingLicense.nationality = nation;
            }
        }

        private void DrivintAddressTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string da = this.DrivingAddressTb.Text.Trim();
            if (!String.IsNullOrEmpty(da))
            {
                mDrivingLicense.address = da;
            }
        }

        private void BrithdayTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string bt = this.BrithdayTb.Text.Trim();
            if (!String.IsNullOrEmpty(bt))
            {
                mDrivingLicense.birthday = bt;
            }
        }

        private void FirstTimeTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ft = this.FirstTimeTb.Text.Trim();
            if (!String.IsNullOrEmpty(ft))
            {
                mDrivingLicense.firstTime = ft;
            }
        }

        private void DriverClassTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string dc = this.DriverClassTb.Text.Trim();
            if (!String.IsNullOrEmpty(dc))
            {
                mDrivingLicense.driverClass = dc;
            }

        }

        private void EffictYearTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ey = this.DriverClassTb.Text.Trim();
            if (!String.IsNullOrEmpty(ey))
            {
                try
                {
                    mDrivingLicense.effictYear = Convert.ToInt32(ey);
                }
                catch { mDrivingLicense.effictYear = 6; }
            }
        }

        private void DrivingStartTimeTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string st = this.DrivingStartTimeTb.Text.Trim();
            if (!String.IsNullOrEmpty(st))
            {
                mDrivingLicense.startTime = st;
            }
        }

        private void DrivingEndTimeTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string et = this.DrivingEndTimeTb.Text.Trim();
            if (!String.IsNullOrEmpty(et))
            {
                mDrivingLicense.endTime = et;
            }
        }
        private void DrivingPhoneTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string phone = this.DrivingPhoneTb.Text;
            if (RegexHelper.IsPhoneNumber(phone))
            {
                mCarInfo.driverMobile = phone;
            }
        }

        #endregion

        /// <summary>
        /// 检查车辆是否存在
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarNumberTb_LostFocus(object sender, RoutedEventArgs e)
        {
            String carnumber = this.CarNumberTb.Text.Trim();
            if (RegexHelper.IsVehicleNumber(carnumber))
            {
                String condition = CarInfoEnum.car_number.ToString() + "=" + Constract.valueSplit + carnumber + Constract.valueSplit;
                String sql = DbBaseHelper.getSelectSql(DataTabeName.car_info.ToString(), null, condition);
                List<CarInfo> list = JsonHelper.DataTableToEntity<CarInfo>(DatabaseOPtionHelper.GetInstance().select(sql));
                if (list.Count > 0)
                {
                    if (list[0].isDelete == 1)
                    {
                        mCarInfo = list[0];
                        mCarInfo.isDelete = 0;
                        mCarInfo.deleteTime = null;
                        isUpdate = true;
                        getVchileLicense();
                        getDriver();
                        getDrivingLicense();
                        bindingVchileLicenseinfo();
                        bindingDrivingLicenseinfo();
                    }
                    else
                    {
                        MessageBox.Show("该车辆信息已经存在！");
                        this.CarNumberTb.Text = null;
                        this.CarNumberTb.Focus();
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("车牌号输入错误！");
                return;
            }
        }
    }
}
