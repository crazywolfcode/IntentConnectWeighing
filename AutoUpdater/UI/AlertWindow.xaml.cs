using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutoUpdater
{
    /// <summary>
    /// AlertWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AlertWindow : Window
    {
        public AlertWindow()
        {
            InitializeComponent();
        }
        #region window Event
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void headerBorder_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnePointLoading_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var point = this.Mborder.PointToScreen(new Point());
            var size = new Size(this.Mborder.ActualWidth, this.Mborder.ActualHeight);

            //MyCustomControlLibrary.MMessageBox.ShowAlert("Success!", Orientation.Horizontal, null, "#3ca9fe", false);

            //MyCustomControlLibrary.MMessageBox.ShowModalAlert("Success!", point, size, Orientation.Vertical, "&#xe654;", "#3ca9fe");

            //MyCustomControlLibrary.MMessageBox.ShowSuccessAlert();
            // MyCustomControlLibrary.MMessageBox.ShowSuccessAlert("Success!");
            //MyCustomControlLibrary.MMessageBox.ShowErrorAlert();
            //MyCustomControlLibrary.MMessageBox.ShowErrorAlert("Error !");

            //MyCustomControlLibrary.MMessageBox.ShowSuccessModelAlert(size,point);
            //MyCustomControlLibrary.MMessageBox.ShowSuccessModelAlert(size, point, "Success!");
            //MyCustomControlLibrary.MMessageBox.ShowErrorModelAlert(size, point);
            //MyCustomControlLibrary.MMessageBox.ShowErrorModelAlert(size, point,"Error !");

            MyCustomControlLibrary.MMessageBox.Reault reault = MyCustomControlLibrary.MMessageBox.ShouBox(
                "是否要删除！",
                "警 告",
                MyCustomControlLibrary.MMessageBox.ButtonType.YesNo,
                MyCustomControlLibrary.MMessageBox.IconType.warring,
                Orientation.Horizontal,
                "是",
                "否"
                );
        }

        private void AlertOne_Click(object sender, RoutedEventArgs e)
        {
            MyCustomControlLibrary.MMessageBox.ShowAlert(
               "Success!",
                Orientation.Horizontal,
                null,
                "#3ca9fe",
                false);

            //  MyCustomControlLibrary.MMessageBox.ShowSuccessAlert();
            // MyCustomControlLibrary.MMessageBox.ShowSuccessAlert("Success!");

        }

        private void AlertTwo_Click(object sender, RoutedEventArgs e)
        {
            var point = this.Mborder.PointToScreen(new Point());
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, this.Mborder.ActualHeight);

            MyCustomControlLibrary.MMessageBox.ShowModalAlert(
                "Success!",
                point,
                size,
                Orientation.Vertical,
               String.Empty,
                "#3ca9fe");
            //MyCustomControlLibrary.MMessageBox.ShowSuccessModelAlert(size,point);
            //MyCustomControlLibrary.MMessageBox.ShowSuccessModelAlert(size, point, "Success!");
        }

        private void AlertThree_Click(object sender, RoutedEventArgs e)
        {
            var point = this.Mborder.PointToScreen(new Point());
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, this.Mborder.ActualHeight);

            MyCustomControlLibrary.MMessageBox.ShowModalAlert(
                "Success!",
                point,
                size,
                Orientation.Vertical,
                 null,
                "#ffffffff");
        }

        private void loadOne_Click(object sender, RoutedEventArgs e)
        {
            MyCustomControlLibrary.MMessageBox.ShowLoading(
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
            var point = this.Mborder.PointToScreen(new Point());
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, this.Mborder.ActualHeight);
            MyCustomControlLibrary.MMessageBox.ShowLoading(
                MyCustomControlLibrary.MMessageBox.LoadType.Circle,
                String.Empty,
                 point,
                 size,
                "&#xe62e;",
                Orientation.Horizontal,
                "#ffffff",
                3);
        }

        private void loadThree_Click(object sender, RoutedEventArgs e)
        {
            var point = this.Mborder.PointToScreen(new Point());
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, this.Mborder.ActualHeight);
            MyCustomControlLibrary.MMessageBox.ShowLoading(
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

            var point = this.Mborder.PointToScreen(new Point());
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, this.Mborder.ActualHeight);
            MyCustomControlLibrary.MMessageBox.ShowLoading(
                MyCustomControlLibrary.MMessageBox.LoadType.Foure,
                "Loading...",
                point,
                size,
                "&#xe752;",
                Orientation.Vertical,
                "#ffffff",
                5);
        }

        private void loadfive_Click(object sender, RoutedEventArgs e)
        {
            var point = this.Mborder.PointToScreen(new Point());
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, this.Mborder.ActualHeight);
            MyCustomControlLibrary.MMessageBox.ShowLoading(
                MyCustomControlLibrary.MMessageBox.LoadType.Two,
                "Loading...",
                point,
                size,
                "&#xe752;",
                Orientation.Vertical,
                "#ffffff",
                3);
        }

        private void loadSix_Click(object sender, RoutedEventArgs e)
        {
            var point = this.Mborder.PointToScreen(new Point());
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, this.Mborder.ActualHeight);
            MyCustomControlLibrary.MMessageBox.ShowLoading(
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
            MyCustomControlLibrary.MMessageBox.ShowLoading(
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
            var point = this.Mborder.PointToScreen(new Point());
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, this.Mborder.ActualHeight);
            MyCustomControlLibrary.MMessageBox.ShowLoading(
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
            var point = this.Mborder.PointToScreen(new Point());
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, this.Mborder.ActualHeight);
            MyCustomControlLibrary.MMessageBox.ShowLoading(
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
            MyCustomControlLibrary.MMessageBox.Reault reault = MyCustomControlLibrary.MMessageBox.ShouBox(
                "操作成功！",
                "信息",
                MyCustomControlLibrary.MMessageBox.ButtonType.No,
                MyCustomControlLibrary.MMessageBox.IconType.success
                );

            if (reault == MyCustomControlLibrary.MMessageBox.Reault.No)
            {
                MyCustomControlLibrary.MMessageBox.ShowSuccessAlert("你点了 取消");
            }
        }

        private void MessageboxTwo_Click(object sender, RoutedEventArgs e)
        {
            MyCustomControlLibrary.MMessageBox.Reault reault = MyCustomControlLibrary.MMessageBox.ShouBox(
             "操作成功！",
             "信息",
             MyCustomControlLibrary.MMessageBox.ButtonType.Yes,
             MyCustomControlLibrary.MMessageBox.IconType.success
             );
            var point = this.Mborder.PointToScreen(new Point());
            //Mborder 窗口内容区域的边框
            var size = new Size(this.Mborder.ActualWidth, this.Mborder.ActualHeight);
            if (reault == MyCustomControlLibrary.MMessageBox.Reault.Yes)
            {
                MyCustomControlLibrary.MMessageBox.ShowSuccessModelAlert(size, point, "你点了 确定");
            }
        }

        private void MessageboxThree_Click(object sender, RoutedEventArgs e)
        {
            MyCustomControlLibrary.MMessageBox.Reault reault = MyCustomControlLibrary.MMessageBox.ShouBox(
           "是否要删除！",
           "警 告",
           MyCustomControlLibrary.MMessageBox.ButtonType.YesNo,
           MyCustomControlLibrary.MMessageBox.IconType.warring,
           Orientation.Horizontal,
           "是",
           "否"
           );

            if (reault == MyCustomControlLibrary.MMessageBox.Reault.Yes)
            {
                MyCustomControlLibrary.MMessageBox.ShowSuccessAlert("你点了 是");
            }
            else
            {
                MyCustomControlLibrary.MMessageBox.ShowSuccessAlert("你点了 否");
            }
        }
    }
}
