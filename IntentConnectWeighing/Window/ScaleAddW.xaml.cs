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
            if (!string.IsNullOrEmpty(mId))
            {
                bindingCurrrScale();
            }

            InitDefaultScaleType();
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            App.SetCurrentWindow(this);
        }

        private void InitDefaultScaleType()
        {
            this.DefaultTypeCB.SelectedIndex = mScale.defaultType;
        }

        private void bindingCurrrScale()
        {
            String condition = ScaleEnum.id.ToString() + "=" + Constract.valueSplit + mScale.id + Constract.valueSplit;
            String sql = DbBaseHelper.getSelectSql(DataTabeName.scale.ToString(), null, condition);
            DataTable dt = DatabaseOPtionHelper.GetInstance().select(sql);
            if (dt.Rows.Count > 0)
            {
                mScale = (JsonHelper.DataTableToEntity<Scale>(dt))[0];
            }
            if (mScale != null)
            {
                this.NameTb.Text = mScale.name;
                this.ComTb.Text = mScale.com;
                this.BaudRateTB.Text = mScale.baudRate.ToString();
                this.DataByteTB.Text = mScale.dataByte.ToString();
                this.EndByteTB.Text = mScale.endByte.ToString();
                this.BrandTB.Text = mScale.brand;
                this.SeriesTB.Text = mScale.series;
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
                    mScale.com.Replace("c", "C").Replace("o", "O").Replace("m", "M");
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

            mScale.com = this.ComTb.Text.Trim();
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
                mScale.baudRate = Convert.ToInt32(this.BaudRateTB.Text.Trim());
            }
            catch
            {
                MessageBox.Show("波特率必须为整数！");
                return false;
            }
            try
            {
                mScale.dataByte = Convert.ToInt32(this.DataByteTB.Text.Trim());
            }
            catch
            {
                MessageBox.Show("数据位必须是整数！");
                return false;
            }
            try
            {
                mScale.endByte = Convert.ToInt32(this.EndByteTB.Text.Trim());
            }
            catch
            {
                MessageBox.Show("结束位必须是整数！");
                return false;
            }
            mScale.brand = this.BrandTB.Text.Trim();
            mScale.series = this.SeriesTB.Text.Trim();
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
    }
}
