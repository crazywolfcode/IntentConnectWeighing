using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace IntentConnectWeighing
{
  public  class SendCarInFactoryCommand
    {
        public static RoutedUICommand Command { get; set; }
        public static CommandBinding CommandBinding;


        static SendCarInFactoryCommand() {
            Command = new RoutedUICommand();
            CommandBinding = new CommandBinding(Command);
            CommandBinding.Executed += CommandBinding_Executed;
        }

        private static void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OutWeighingPage page = sender as OutWeighingPage;
            page.Infactory(e.Parameter, true);
        }
    }
}
