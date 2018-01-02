using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IntentConnectWeighing
{
    class AddCommand
    {
        public static RoutedUICommand ShowSelfTagCommand { set; get; }
        public static CommandBinding ShowSelfTagCommandBinding;


        static AddCommand(){
            ShowSelfTagCommand = new RoutedUICommand();
            ShowSelfTagCommandBinding = new CommandBinding(ShowSelfTagCommand);
            ShowSelfTagCommandBinding.Executed += ShowSelfTagCommandBinding_Executed;
        }

        private static void ShowSelfTagCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {           
            Button btn =sender as Button;
            MessageBox.Show(btn.Tag +" " +sender.ToString());
        }
    }
}
