using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;

namespace IntentConnectWeighing
{
  public  class ShowSettingWindowsCommand
    {
        public static RoutedUICommand command { get; set; }
        public static CommandBinding commandBinding;

        static ShowSettingWindowsCommand(){
            command = new RoutedUICommand();
            commandBinding = new CommandBinding(command);
            commandBinding.Executed += ShowSettingWindowExecuted;
        }

        private static void ShowSettingWindowExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SettingW w = new SettingW();      
            w.ShowDialog();
        }
    }
}
