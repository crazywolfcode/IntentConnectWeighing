using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IntentConnectWeighing
{
 public  class InFactoryComand
    {
        public static RoutedUICommand command { get; set; }
        public static CommandBinding CommandBinding;
        static InFactoryComand() {
            command = new RoutedUICommand();
            CommandBinding = new CommandBinding(command);
            CommandBinding.Executed += CommandBinding_Executed;
        }
        private static void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InWeighingPage page = (InWeighingPage)(sender);
            page.CurrentWeighingBillIsSendBill = true;
            page.mWeighingBill = (WeighingBill)e.Parameter;
            page.Infactory();
        }
    }
}
