using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page
    {

        public BaseWindow ParentWindow;
        public HomePage()
        {
            InitializeComponent();
            this.CommandBindings.Add(TestCommand.ChangeContentCommandBinding);
            this.CommandBindings.Add(TestCommand.ShowNameCommandBinding);
        }

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ParentWindow != null)
            {
                ParentWindow.ChangeAlertNumber(ResourceName.MenuIndexNumber, 1, 1);
            }

        }


        private void ErrorButton_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(FileHelper.GetRunTimeRootPath());
            App.notifyIcon.ShowBalloonTip(30, "tipTitle", "tipText", System.Windows.Forms.ToolTipIcon.Info);
            //if (this.ParentWindow != null)
            //{
            //    ParentWindow.changeAlertNumber(ResourceName.MenuIndexNumber, 1, 0);
            //}
        }

        public void ShowCompanyName()
        {
            MessageBox.Show(ResourceHelper.getStringFromDictionaryResource(ResourceName.CompanyName));
        }

        private void getIp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(CommomHelpre.getLoclIp());
        }

        private void AlertOne_Click(object sender, RoutedEventArgs e)
        {
            MyCustomControlLibrary.MMessageBox.GetInstance().ShowAlert(
               "Success!",
                Orientation.Horizontal,
                null,
                "#3ca9fe",
                false);

            // MyCustomControlLibrary.MMessageBox.GetInstance().ShowSuccessAlert();
            // MyCustomControlLibrary.MMessageBox.GetInstance().ShowSuccessAlert("Success!");

        }

        private void AlertTwo_Click(object sender, RoutedEventArgs e)
        {
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, 300);

            MyCustomControlLibrary.MMessageBox.GetInstance().ShowModalAlert(
                "Success!",
                new Point(0, 0),
                size,
                Orientation.Vertical,
               String.Empty,
                "#3ca9fe");
            //MyCustomControlLibrary.MMessageBox.ShowSuccessModelAlert(size,point);
            //MyCustomControlLibrary.MMessageBox.ShowSuccessModelAlert(size, point, "Success!");
        }

        private void AlertThree_Click(object sender, RoutedEventArgs e)
        {
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, 300);

            MyCustomControlLibrary.MMessageBox.GetInstance().ShowModalAlert(
                "Success!",
                new Point(0, 0),
                size,
                Orientation.Vertical,
                 null,
                "#ffffffff");
        }

        private void loadOne_Click(object sender, RoutedEventArgs e)
        {
            MyCustomControlLibrary.MMessageBox.GetInstance().ShowLoading(
                MyCustomControlLibrary.MMessageBox.LoadType.Circle,
                "加载中。。。",
                new Point(0, 0),
                new Size(0, 0),
                "&#xe62e;",
                Orientation.Horizontal,
                "#ffffff",
                3);

        }

        private void loadTwo_Click(object sender, RoutedEventArgs e)
        {
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, 300);
            MyCustomControlLibrary.MMessageBox.GetInstance().ShowLoading(
                MyCustomControlLibrary.MMessageBox.LoadType.Circle,
                String.Empty,
                new Point(0, 0),
                 size,
                "&#xe62e;",
                Orientation.Horizontal,
                "#ffffff",
                3);
        }

        private void loadThree_Click(object sender, RoutedEventArgs e)
        {
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, 300);
            MyCustomControlLibrary.MMessageBox.GetInstance().ShowLoading(
            MyCustomControlLibrary.MMessageBox.LoadType.Three,
            "Loading...",
             new Point(0, 0),
            new Size(0, 0),
            null,
            Orientation.Vertical,
            "#ffffff",
            3);
        }

        private void loadFour_Click(object sender, RoutedEventArgs e)
        {
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, 300);
            MyCustomControlLibrary.MMessageBox.GetInstance().ShowLoading(
                MyCustomControlLibrary.MMessageBox.LoadType.Foure,
                "Loading...",
               new Point(0, 0),
                size,
                "&#xe752;",
                Orientation.Vertical,
                "#ffffff",
                5);
        }

        private void loadfive_Click(object sender, RoutedEventArgs e)
        {
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, 300);
            MyCustomControlLibrary.MMessageBox.GetInstance().ShowLoading(
                MyCustomControlLibrary.MMessageBox.LoadType.Two,
                "Loading...",
                new Point(0, 0),
                 size,
                "&#xe752;",
                Orientation.Vertical,
                "#ffffff",
                3);
        }

        private void loadSix_Click(object sender, RoutedEventArgs e)
        {
            var point = new Point(0, 0);
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, 300);
            MyCustomControlLibrary.MMessageBox.GetInstance().ShowLoading(
                MyCustomControlLibrary.MMessageBox.LoadType.Firve,
                "Loading...",
                point,
                size,
                "&#xe752;",
                Orientation.Vertical,
                "#ffffff",
                3);
        }

        private void loadSever_Click(object sender, RoutedEventArgs e)
        {
            MyCustomControlLibrary.MMessageBox.GetInstance().ShowLoading(
              MyCustomControlLibrary.MMessageBox.LoadType.One,
              "Loading...",
           new Point(0, 0),
            new Size(0, 0),
             null,
              Orientation.Vertical,
              "#ffffff",
              3);
        }

        private void loadeight_Click(object sender, RoutedEventArgs e)
        {
            var point = new Point();
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, 300);
            MyCustomControlLibrary.MMessageBox.GetInstance().ShowLoading(
                MyCustomControlLibrary.MMessageBox.LoadType.Grid,
                "Loading...",
                point,
                size,
                "&#xe752;",
                Orientation.Vertical,
                "#ffffff",
                3);
        }

        private void loadnine_Click(object sender, RoutedEventArgs e)
        {
            var point = new Point();
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, 300);
            MyCustomControlLibrary.MMessageBox.GetInstance().ShowLoading(
                MyCustomControlLibrary.MMessageBox.LoadType.One,
                "Loading...",
                point,
                size,
                "&#xe752;",
                Orientation.Vertical,
                "#ffffff",
                3);
        }

        private void MessageboxOne_Click(object sender, RoutedEventArgs e)
        {
            MyCustomControlLibrary.MMessageBox.Result reault = MyCustomControlLibrary.MMessageBox.GetInstance().ShowBox(
                "操作成功！",
                "信息",
                MyCustomControlLibrary.MMessageBox.ButtonType.No,
                MyCustomControlLibrary.MMessageBox.IconType.success
                );

            if (reault == MyCustomControlLibrary.MMessageBox.Result.No)
            {
                MyCustomControlLibrary.MMessageBox.GetInstance().ShowSuccessAlert("你点了 取消");
            }
        }

        private void MessageboxTwo_Click(object sender, RoutedEventArgs e)
        {
            MyCustomControlLibrary.MMessageBox.Result reault = MyCustomControlLibrary.MMessageBox.GetInstance().ShowBox(
             "操作成功！",
             "信息",
             MyCustomControlLibrary.MMessageBox.ButtonType.Yes,
             MyCustomControlLibrary.MMessageBox.IconType.success
             );
            var point = new Point();
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, 300);
            if (reault == MyCustomControlLibrary.MMessageBox.Result.Yes)
            {
                MyCustomControlLibrary.MMessageBox.GetInstance().ShowSuccessModelAlert(size, point, "你点了 确定");
            }
        }

        private void MessageboxThree_Click(object sender, RoutedEventArgs e)
        {
            MyCustomControlLibrary.MMessageBox.Result reault = MyCustomControlLibrary.MMessageBox.GetInstance().ShowBox(
           "是否要删除！",
           "警 告",
           MyCustomControlLibrary.MMessageBox.ButtonType.YesNo,
           MyCustomControlLibrary.MMessageBox.IconType.Info,
           Orientation.Horizontal,
           "是",
           "否"
           );

            if (reault == MyCustomControlLibrary.MMessageBox.Result.Yes)
            {
                MyCustomControlLibrary.MMessageBox.GetInstance().ShowSuccessAlert("你点了 是");
            }
            else
            {
                MyCustomControlLibrary.MMessageBox.GetInstance().ShowSuccessAlert("你点了 否");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.TestComBox.ItemsSource = getSource();       
        }
        private List<VV> getSource()
        {
            List<VV> vl = new List<VV>();
            vl.Add(new VV() { age = 15, name = "name11" });
            vl.Add(new VV() { age = 15, name = "name12" });
            vl.Add(new VV() { age = 15, name = "name13" });
            vl.Add(new VV() { age = 15, name = "name14" });
            vl.Add(new VV() { age = 15, name = "name16" });
            vl.Add(new VV() { age = 15, name = "name18" });
            vl.Add(new VV() { age = 15, name = "name110"});
            return vl;
        }

    }



    class VV
    {
        public int age { get; set; }
        public String name { get; set; }
    }


}
