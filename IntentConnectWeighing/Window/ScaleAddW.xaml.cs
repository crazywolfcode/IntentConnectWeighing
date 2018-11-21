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
    public partial class ScaleAddW : Window
    {
        private string mId;
        private Scale mScale;
        private ScaleBrand mScaleBrand;
        private List<ScaleBrand> mScaleBrands;
        private List<ScaleSeries> mScaleSeries;
        #region Camera   
        public Int32 currCameraId = -1;
        public bool isInitSDK;
        public bool isLogin = false;
        public bool isPreviewSuccess = false;
        #endregion
        public ScaleAddW(String scaleId = null)
        {
            InitializeComponent();
            mId = scaleId;
            mScale = new Scale() { defaultType = (int)ScaleDefaultType.No };
            
            if (String.IsNullOrEmpty(mId))
            {
                mScale.id = Guid.NewGuid().ToString();
            }
            else
            {
                mScale.id = mId;
            }
        }
        public delegate void MyDebugInfo(string str);
        public void DebugInfo(string str)
        {
            ConsoleHelper.writeLine(str);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
         
            FindCom();
            InitDefaultScaleType();
            InitBrandItemSource();
            if (!string.IsNullOrEmpty(mId))
            {
                bindingCurrrScale();
            }
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            App.SetCurrentWindow(this);
        }

        private void FindCom() {
            int count = System.IO.Ports.SerialPort.GetPortNames().Count();
            this.ComCb.ItemsSource = System.IO.Ports.SerialPort.GetPortNames();
            if (count > 0)
            {
                ComAlertLabel.Content = count + " 个串口可用";
                if (String.IsNullOrEmpty(mId))
                {
                    this.ComCb.SelectedIndex = 0;
                }
            }
            else
            {
                ComAlertLabel.Content = "没有可以的串口";
            }
            
        }
        private void InitDefaultScaleType()
        {
            this.DefaultTypeCB.SelectedIndex = mScale.defaultType;
        }

        private void InitBrandItemSource() {
            mScaleBrands = new ScaleBrandModel().getAll();
            this.BrandCB.ItemsSource =mScaleBrands;
            this.BrandCB.DisplayMemberPath = "brandName";
        }

        private void bindingCurrrScale()
        {
            String condition = ScaleEnum.id.ToString() + "=" + Constract.valueSplit + mScale.id + Constract.valueSplit;
            String sql = DbBaseHelper.getSelectSql(DataTabeName.scale.ToString(), null, condition);
            DataTable dt = DatabaseOPtionHelper.GetInstance().select(sql);
            if (dt.Rows.Count > 0)
            {
                mScale = (DbBaseHelper.DataTableToEntitys<Scale>(dt))[0];
            }
            if (mScale != null)
            {
                this.NameTb.Text = mScale.name;
                this.ComCb.Text = mScale.com;
                this.BaudRateCB.Text = mScale.baudRate.ToString();
                this.DataByteCB.Text = mScale.dataByte.ToString();
                this.EndByteCB.Text = mScale.endByte.ToString();
                this.DefaultTypeCB.SelectedIndex = mScale.defaultType;
                for(int i=0; i < mScaleBrands.Count;i++) {
                    if (mScaleBrands[i].brandName == mScale.brand) {
                        this.BrandCB.SelectedIndex = i;
                        continue;
                    }
                }
                for (int i = 0; i < mScaleSeries.Count; i++)
                {
                    if (mScaleSeries[i].name == mScale.series)
                    {
                        this.SeriesCB.SelectedIndex = i;
                        continue;
                    }
                }
            }
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
            if (checkInput())
            {
                int res = 0;
                ScaleModel scaleModel = new ScaleModel();
                if (String.IsNullOrEmpty(mId))
                {
                    //insert
                    mScale.status = 1;
                    mScale.addTime = DateTimeHelper.getCurrentDateTime();
                    mScale.clientId = App.CurrClientId;
                    mScale.companyId = App.currentCompany.id;
                    mScale.com.Replace("c", "C").Replace("o", "O").Replace("m", "M");
                    if (App.currentUser != null)
                    {
                        mScale.addUserId = App.currentUser.id;
                        mScale.addUserName = App.currentUser.name;
                    }
                    mScale.defaultType = (int)ScaleDefaultType.No;
                    mScale.syncTime = 0;                 
                    if (scaleModel.isExist(mScale)) {
                        MessageBox.Show("已经存在，不能重复添加！");
                        return;
                    }
                    res = DatabaseOPtionHelper.GetInstance().insert(mScale);
                    if (res > 0)
                    {
                        MessageBox.Show("添加成功！");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("添加失败！");
                    }
                }
                else
                {
                    //update             
                    if (scaleModel.isExist(mScale))
                    {
                        MessageBox.Show("已经存在，不能修改！");
                        return;
                    }
                    res = DatabaseOPtionHelper.GetInstance().update(mScale);
                    if (res > 0)
                    {
                        MessageBox.Show("修改成功！");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("修改失败！");
                    }
                }
            }
        }

        

        private bool checkInput()
        {
            mScale.name = this.NameTb.Text.Trim();

            mScale.com = this.ComCb.Text.Trim();
            if (String.IsNullOrEmpty(mScale.com))
            {
                MessageBox.Show("Com 串口不能为空！");
                return false;
            }
            if (!RegexHelper.IsMatch(mScale.com, RegexHelper.ComPattern))
            {
                MessageBox.Show("串口输入的格式不正确");
                return false;
            }
            try
            {
                mScale.baudRate = Convert.ToInt32(this.BaudRateCB.Text);
            }
            catch
            {
                MessageBox.Show("波特率必须为整数！");
                return false;
            }
            try
            {
                mScale.dataByte = Convert.ToInt32(this.DataByteCB.Text);
            }
            catch
            {
                MessageBox.Show("数据位必须是整数！");
                return false;
            }
            try
            {
                mScale.endByte = Convert.ToInt32(this.EndByteCB.Text);
            }
            catch
            {
                MessageBox.Show("结束位必须是整数！");
                return false;
            }
            mScale.brand = mScaleBrand.brandName;
            if (String.IsNullOrEmpty(mScale.brand)) {
                MessageBox.Show("品牌名称为必选 ！");
                return false;
            }
            String series = ((ScaleSeries)this.SeriesCB.SelectedItem).name;
            mScale.series = series;
            mScale.brandType =mScaleBrand.type;
            return true;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (App.currWindow == this)
            {
                App.SetCurrentWindow();
                App.ShowCurrentWindow();
            }
        }

        private void DefaultTypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                mScale.defaultType = this.DefaultTypeCB.SelectedIndex;
            }
        }

        private void BrandCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mScaleBrand = (ScaleBrand)BrandCB.SelectedItem;
            InitSeries();
        }

        private void InitSeries() {
            mScaleSeries = new ScaleBrandSeriesModel().GetByBrandID(mScaleBrand.id);
            this.SeriesCB.ItemsSource =mScaleSeries ;
            this.SeriesCB.DisplayMemberPath = "name";
        }
    }
}
