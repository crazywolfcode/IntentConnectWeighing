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
    public partial class MaterialCategoryAddW : Window
    {
        private String mId;
        private MaterialCategory mMaterialCategory;

        /// <summary>
        ///  company,material,car
        /// </summary>
        public Action<Boolean, Boolean, Boolean> ParentRefreshData { get; set; }

        public MaterialCategoryAddW(String categoryId = null)
        {
            InitializeComponent();
            this.mId = categoryId;
        }
        public delegate void MyDebugInfo(string str);
        public void DebugInfo(string str)
        {
            ConsoleHelper.writeLine(str);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(mId))
            {
               // BindingCurrrData();
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
            if(String.IsNullOrEmpty(this.nameTb.Text) || this.nameTb.Text.Length < 1)
            {
                MessageBox.Show("请输入分类的名称！");
                return;
            }

            SqlDao.DbHelper optionHelper =  DatabaseOPtionHelper.GetInstance();
            String condition = String.Empty;
            String sql = string.Empty;
            int res = 0;
            if (!String.IsNullOrEmpty(mId))
            {
                //update
                mMaterialCategory.name = this.nameTb.Text;
                if (RegexHelper.IsChineseCharacter(mMaterialCategory.name))
                {
                    mMaterialCategory.firstCase = StringHelper.GetFirstPinyin(mMaterialCategory.name);
                }
                else
                {
                    mMaterialCategory.firstCase = mMaterialCategory.name;
                }
                mMaterialCategory.syncTime = (Int32)DateTimeHelper.GetTimeStamp();
                res =  DatabaseOPtionHelper.GetInstance().update(mMaterialCategory);
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
                if (mMaterialCategory != null)
                {
                    mMaterialCategory.isDelete = 0;
                    mMaterialCategory.deleteTime = null;
                    if (RegexHelper.IsChineseCharacter(mMaterialCategory.name))
                    {
                        mMaterialCategory.firstCase = StringHelper.GetFirstPinyin(mMaterialCategory.name);
                    }
                    else
                    {
                        mMaterialCategory.firstCase = mMaterialCategory.name;
                    }
                    mMaterialCategory.addUserId = App.currentUser.id;
                    mMaterialCategory.addUserName = App.currentUser.name;
                    mMaterialCategory.addUserCompany = App.currentCompany.name;
                    mMaterialCategory.syncTime = DateTimeHelper.GetTimeStamp();
                    res = optionHelper.update(mMaterialCategory);
                }
                else
                {
                    mMaterialCategory = new MaterialCategory()
                    {
                        id = Guid.NewGuid().ToString(),
                        name = this.nameTb.Text.Trim(),
                        addtime = DateTimeHelper.getCurrentDateTime(),
                        addUserId = App.currentUser.id,
                        addUserName = App.currentUser.name,
                        addUserCompany = App.currentCompany.name,
                        isDelete = 0,
                        deleteTime = null,
                        syncTime = (Int32)DateTimeHelper.GetTimeStamp()
                    };
                    if (RegexHelper.IsChineseCharacter(mMaterialCategory.name))
                    {
                        mMaterialCategory.firstCase = StringHelper.GetFirstPinyin(mMaterialCategory.name);
                    }
                    else
                    {
                        mMaterialCategory.firstCase = mMaterialCategory.name;
                    }
                    res = optionHelper.insert(mMaterialCategory);
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
                ParentRefreshData(false, true, false);
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void CheckedExist(String name)
        {
            string condition = MaterialCategoryEnum.name.ToString() + "=" + Constract.valueSplit + name + Constract.valueSplit;
            string sql = DatabaseOPtionHelper.GetInstance().getSelectSqlNoSoftDeleteCondition(DataTabeName.material_category.ToString(), null, condition, null, null, null, 1, 0);
            List<MaterialCategory> list =  DatabaseOPtionHelper.GetInstance().select<MaterialCategory>(sql);
            if (list == null || list.Count <= 0)
            {
                this.AlertInfoTb.Text = "该分类名称可以添加！";
                this.AlertInfoTb.Foreground = Brushes.Green;
                return;
            }
            else
            {
                mMaterialCategory = list[0];
            }
            if (mMaterialCategory != null)
            {
                if (mMaterialCategory != null)
                {
                    if (mMaterialCategory.isDelete != 1)
                    {
                        this.AlertInfoTb.Text = "该分类已经存在，不需要再添加！";
                        this.nameTb.Focus();
                    }
                }
            }
        }
        private void nameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            String name = this.nameTb.Text.Trim();
            if (String.IsNullOrEmpty(name))
            {
                this.saveBtn.IsEnabled = false;
                this.AlertInfoTb.Foreground = Brushes.Gray;
                this.AlertInfoTb.Text = "分类名称建议采用中文,不可以重复！";
                return;
            }
            if (!RegexHelper.IsChineseCharacter(name))
            {
                this.AlertInfoTb.Foreground = Brushes.Red;
                this.AlertInfoTb.Text = "分类名称建议采用中文";
            }
            this.saveBtn.IsEnabled = true;
            CheckedExist(name);
        }
    }
}
