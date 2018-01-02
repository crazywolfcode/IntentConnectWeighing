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
        public BaseDataPage()
        {
            InitializeComponent();          
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            string path = "../../DynamicTemplate/ItemPage.xaml";
           
            for (int i = 0; i < 12; i++)
            {
                FrameworkElement element = TemplateHelper.getFrameworkElementFromXaml(path);
                Button addbtn;
                addbtn = element.FindName("AddBtn") as Button;
                addbtn.Tag = "ButtonTag" + i;
                addbtn.Command = AddCommand.ShowSelfTagCommand;
                addbtn.CommandBindings.Add(AddCommand.ShowSelfTagCommandBinding);
                this.ContentStackPanel.Children.Add(element);
            }               
        }
    }
}
