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
    public partial class MaterialAddW : Window
    {
        private List<MaterialCategory> mMaterialCategorys;
        private Material mMaterial ;
        public Action<Boolean, Boolean, Boolean> ParentRefreshData { get; set; }
        public MaterialAddW(Material material = null)
        {
            InitializeComponent();
            if (material == null)
            {
                mMaterial = new Material();
            }
            else {
                mMaterial = material;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (mMaterial !=null && mMaterial.id!=null)
            {
                BindingCurrrData();
            }
        }

        private void BindingCurrrData()
        {
            this.nameTb.Text = mMaterial.name;
            this.CateNameCb.IsEditable = true;
            this.CateNameCb.Text = mMaterial.categoryName;
            this.CateNameCb.IsEditable = false;
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
            SqlDao.DbHelper optionHelper =  DatabaseOPtionHelper.GetInstance();
            int res = 0;
            if (!String.IsNullOrEmpty(mMaterial.id))
            {
                //update
                mMaterial.name = this.nameTb.Text;
                mMaterial.nameFirstCase = StringHelper.GetFirstPinyin(mMaterial.name);
                mMaterial.syncTime = (Int32)DateTimeHelper.GetTimeStamp();
                res = optionHelper.update(mMaterial);
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
                if (mMaterial.isDelete == 1)
                {
                    mMaterial.isDelete = 0;
                    mMaterial.deleteTime = null;
                    mMaterial.nameFirstCase = StringHelper.GetFirstPinyin(mMaterial.name);
                    mMaterial.updateTime = DateTimeHelper.getCurrentDateTime();
                    mMaterial.updateUserId = App.currentUser.id;
                    mMaterial.updateUserName = App.currentUser.name;
                    mMaterial.syncTime = DateTimeHelper.GetTimeStamp();
                    res = optionHelper.update(mMaterial);
                }
                else
                {
                    if (String.IsNullOrEmpty(mMaterial.categoryId) || String.IsNullOrEmpty(mMaterial.categoryName))
                    {
                        MessageBox.Show("添加物资名称必须先选择物资的分类！");
                        return;
                    }
                    mMaterial.id = Guid.NewGuid().ToString();
                    mMaterial.name = this.nameTb.Text.Trim();
                    mMaterial.addtime = DateTimeHelper.getCurrentDateTime();
                    mMaterial.addUserId = App.currentUser.id;
                    mMaterial.addUserName = App.currentUser.name;
                    mMaterial.isDelete = 0;
                    mMaterial.deleteTime = null;
                    mMaterial.syncTime = DateTimeHelper.GetTimeStamp();                    
                    mMaterial.nameFirstCase = StringHelper.GetFirstPinyin(mMaterial.name);
                    res = optionHelper.insert(mMaterial);
                }
                if (res > 0)
                {
                    MessageBox.Show("保存成功！");

                    new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                        String condition = MaterialCategoryEnum.id.ToString() + "=" + Constract.valueSplit + mMaterial.categoryId + Constract.valueSplit;
                        CommonModel.FieldTryAsc(DataTabeName.material_category.ToString(), MaterialCategoryEnum.children_count.ToString(), 1, condition);
                    })).Start();

                    RefreshParentData();
                    ChangeCategoryChildredCount();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("保存失败！");
                    return;
                }
            }
        }
        /// <summary>
        /// Change Category ChildredCount
        /// </summary>
        /// <param name="isAdd"></param>
        private void ChangeCategoryChildredCount(bool isAdd = true) {
            String sql = String.Empty;
            String set=String.Empty;
            String condition=MaterialCategoryEnum.id.ToString()+"=" +Constract.valueSplit+mMaterial.categoryId+Constract.valueSplit;
            if (isAdd == true)
            {
                set = MaterialCategoryEnum.children_count.ToString() + " = " + MaterialCategoryEnum.children_count.ToString() +"+"+" 1";
            }
            else {
                set = MaterialCategoryEnum.children_count.ToString() + " = " + MaterialCategoryEnum.children_count.ToString() +"-"+" 1";
            }
            sql = DatabaseOPtionHelper.GetInstance().getUpdateSql(DataTabeName.material_category.ToString(), set, condition);
             DatabaseOPtionHelper.GetInstance().update(sql);
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
            string @condition = MaterialEnum.name.ToString() + "=" + Constract.valueSplit + name + Constract.valueSplit
                + " and "
                + MaterialEnum.category_id.ToString() + "+" + Constract.valueSplit + mMaterial.categoryId + Constract.valueSplit;
            string sql = DatabaseOPtionHelper.GetInstance().getSelectSqlNoSoftDeleteCondition(DataTabeName.material.ToString(), null, condition, null, null, null, 1, 0);
            List<Material> list =  DatabaseOPtionHelper.GetInstance().select<Material>(sql);           
            if (list == null || list.Count <= 0)
            {
                this.AlertInfoTb.Text = "该物资名称可以添加！";
                this.AlertInfoTb.Foreground = Brushes.Green;
            }
            else
            {
                mMaterial = list[0];
            }
            if (mMaterial != null)
            {
                if (mMaterial.isDelete != 1)
                {
                    this.AlertInfoTb.Text = "该物资已经存在，不需要再添加！";
                    this.nameTb.Focus();
                }
            }
        }

        private bool isfirstChanged = true;
        private void nameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            String name = this.nameTb.Text.Trim();
            if (mMaterial != null && mMaterial.id != null)
            {
                if (isfirstChanged == true) {
                    isfirstChanged = false;
                    return;
                }
            }
            
            if (String.IsNullOrEmpty(name))
            {
                this.saveBtn.IsEnabled = false;
                this.AlertInfoTb.Foreground = Brushes.Gray;
                this.AlertInfoTb.Text = "物资名称建议采用中文,不可以重复！";
                return;
            }
            if (!RegexHelper.IsChineseCharacter(name))
            {
                this.AlertInfoTb.Foreground = Brushes.Red;
                this.AlertInfoTb.Text = "物资名称建议采用中文";
            }
            mMaterial.name = name;
            this.saveBtn.IsEnabled = true;
            CheckedExist(name);
        }

        private void CateNameCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MaterialCategory mc = (MaterialCategory)this.CateNameCb.SelectedItem;
            mMaterial.categoryId = mc.id;
            mMaterial.categoryName = mc.name;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            InitCateGoryData();
        }

        private void InitCateGoryData()
        {
            String sql = DatabaseOPtionHelper.GetInstance().getSelectSql(DataTabeName.material_category.ToString());
            mMaterialCategorys =  DatabaseOPtionHelper.GetInstance().select<MaterialCategory>(sql);   
            this.CateNameCb.ItemsSource = mMaterialCategorys;
            for (int i = 0; i < this.CateNameCb.Items.Count; i++)
            {
                MaterialCategory mc = (MaterialCategory)this.CateNameCb.Items[i];
                if (mc.id == mMaterial.categoryId) {
                    this.CateNameCb.SelectedIndex = i;
                    break;
                }
            }
        }
    }
}
