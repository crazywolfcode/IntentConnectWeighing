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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IntentConnectWeighing
{
    /// <summary>
    /// testExpender.xaml 的交互逻辑
    /// </summary>
    public partial class TestExpender : Window
    {
        public TestExpender()
        {
            InitializeComponent();
        }


        public class Text
        {

            public string Content { get; set; }
            public Text(String content)
            {
                Content = content;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Text> items = new List<Text>();
            for (int i = 0; i < 20; i++)
            {
                items.Add(new Text("AAAAA" + i));
            }
         // this.listView.ItemsSource = items;     
            Comp cp = new Comp
            {
                name = "云南省",
                num = 20
            };
            this.expander.Header = cp;
            Comp cp1 = new Comp
            {
                name = "贵州省",
                num = 10
            };
           
        }

        private void expander_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Expander ep = sender as Expander;
            MessageBox.Show(ep.IsExpanded == true ? "true" : "false");
        }

        public class Comp
        {
            public string name { get; set; }
            public Int32 num { get; set; }
        }

    }

}
