﻿using System;
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
    /// itemPage.xaml 的交互逻辑
    /// </summary>
    public partial class itemPage : Page
    {
        public itemPage()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(((Button)sender).Tag.ToString());
        }
    }
}
