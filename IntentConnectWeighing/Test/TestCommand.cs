using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using MyCustomControlLibrary;
namespace IntentConnectWeighing
{
    class TestCommand
    {
        public static RoutedUICommand ShowNameCommand {  set; get; }
        public static CommandBinding ShowNameCommandBinding ;
        public static RoutedUICommand ChangeContentCommand { set; get; }
        public static CommandBinding ChangeContentCommandBinding;

      static  TestCommand() {
            ShowNameCommand = new RoutedUICommand();           
            ShowNameCommandBinding = new CommandBinding(ShowNameCommand);
            ShowNameCommandBinding.Executed += ShowNameCommandBinding_Executed;

           ChangeContentCommand = new RoutedUICommand();
            ChangeContentCommandBinding = new CommandBinding(ChangeContentCommand);
            ChangeContentCommandBinding.Executed += ChangeContentCommandBinding_Executed;
        }

        private static void ChangeContentCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Page page= (Page) sender;
            HomePage homepage = (HomePage)page;
            homepage.ShowCompanyName();
            IconButton c = homepage.FindName("Warining") as IconButton;
            if (c != null) {
                if(c.IsFocused ==true)
                c.Background = Brushes.Red;
                c.Background = Brushes.White;
            } else {
                MessageBox.Show(" iconbutton is null");
            }
            if (e.Parameter != null) { MessageBox.Show(e.Parameter.ToString()); } else {
                MessageBox.Show("e.Parameter null");
            }
        }

        private static void ShowNameCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IconButton c = ((Page)sender).FindName("Question") as IconButton;
            if (c != null) { c.Background = Brushes.Red; MessageBox.Show(e.Parameter.ToString()); }
            else
            {
                MessageBox.Show(" iconbutton is null");
            }
        }
    }
}
