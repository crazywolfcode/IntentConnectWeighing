using System;
using System.Collections.Generic;
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

namespace IntentConnectWeighing
{
    /// <summary>
    /// BaseDataPage.xaml 的交互逻辑
    /// </summary>
    public partial class BaseDataPage : Page
    {
        public Window paretntWindow;

        public BaseDataPage()
        {
            InitializeComponent();          
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.scrollViewer.Height = this.scrollViewer.ActualHeight - 30;


            string path = Constract.templatePath + "TestItem.xaml";
            for (int i = 0; i < 12; i++)
            {
                FrameworkElement element = TemplateHelper.GetFrameworkElementFromXaml(path);
                Button addbtn;
                addbtn = element.FindName("AddBtn") as Button;
                addbtn.Tag = "ButtonTag" + i;
                addbtn.Command = AddCommand.ShowSelfTagCommand;
                addbtn.CommandBindings.Add(AddCommand.ShowSelfTagCommandBinding);
                this.ContentStackPanel.Children.Add(element);
            }
        }

        #region add 添加

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CompanyTabBtn.IsChecked == true) {
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

        private void AddCompany() {
            MessageBox.Show("公司信息不可这样添加，需要对方公司自己注册。");
        }
        private void AddCar() { }
        private void AddMaterial() { }
        #endregion
    }
}
