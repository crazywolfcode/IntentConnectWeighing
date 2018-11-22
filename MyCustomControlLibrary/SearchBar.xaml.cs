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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyCustomControlLibrary
{
    /// <summary>
    /// SearchBar.xaml 的交互逻辑
    /// </summary>
    public partial class SearchBar : UserControl
    {
        public SearchBar()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.MainBorder.BorderBrush = NomalBrush;
            this.QueryTBox.Width = SearchAreaWidth;
            this.QueryTBox.Height = BarHeight - 2;
            this.SearchBtn.Height = BarHeight - 2;
            this.DeleteButton.Height = BarHeight - 2;
            this.Height = BarHeight;
            this.QueryTBox.ToolTip = ToolTip;
        }

        public int SearchAreaWidth
        {
            get { return (int)GetValue(SearchAreaWidthProperty); }
            set { SetValue(SearchAreaWidthProperty, value); }
        }

        #region Dependency Property
        
        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }        
        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register("IsEditing", typeof(bool), typeof(SearchBar), new FrameworkPropertyMetadata(false,new PropertyChangedCallback(edintingStatusChanged)));

        private static void edintingStatusChanged(DependencyObject sender,DependencyPropertyChangedEventArgs args) {
            SearchBar searchBar = sender as SearchBar;
            searchBar.OnEditintStatusChanged((bool)args.OldValue,(bool)args.NewValue);
        }
        
        public string  SearchContent
        {
            get { return (string )GetValue(SearchContentProperty); }
            set { SetValue(SearchContentProperty, value); }
        }        
        public static readonly DependencyProperty SearchContentProperty =
            DependencyProperty.Register("SearchContent", typeof(string ), typeof(SearchBar), new FrameworkPropertyMetadata("",new PropertyChangedCallback(OnSearchContent)));

        private static void OnSearchContent(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SearchBar bar = sender as SearchBar;          
            bar.OnSearchContentChanged(e.OldValue.ToString(), e.NewValue.ToString());
        }

        public static readonly DependencyProperty SearchAreaWidthProperty =
            DependencyProperty.Register("SearchAreaWidth", typeof(int), typeof(SearchBar), new PropertyMetadata(200));

        public int BarHeight
        {
            get { return (int)GetValue(BarHeightProperty); }
            set { SetValue(BarHeightProperty, value); }
        }        
        public static readonly DependencyProperty BarHeightProperty =
            DependencyProperty.Register("BarHeight", typeof(int), typeof(SearchBar), new PropertyMetadata(28));

        public Brush ActivatedBrush
        {
            get { return (Brush)GetValue(ActivatedBrushProperty); }
            set { SetValue(ActivatedBrushProperty, value); }
        }        
        public static readonly DependencyProperty ActivatedBrushProperty =
            DependencyProperty.Register("ActivatedBrush", typeof(Brush), typeof(SearchBar), new PropertyMetadata(Brushes.LightBlue));
        
        public Brush NomalBrush
            {
            get { return (Brush)GetValue(NomalBrushProperty); }
            set { SetValue(NomalBrushProperty, value); }
        }        
        public static readonly DependencyProperty NomalBrushProperty =
            DependencyProperty.Register("NomalBrush", typeof(Brush), typeof(SearchBar), new PropertyMetadata(Brushes.LightGray));
        
        #endregion

        #region Routed Event

        public static readonly RoutedEvent SearchContentChangedEvent = EventManager.RegisterRoutedEvent("SearchContentChanged",RoutingStrategy.Bubble,typeof(RoutedPropertyChangedEventHandler<String>),typeof(SearchBar));

        public event RoutedPropertyChangedEventHandler<String> SearchContentChanged {
            add { AddHandler(SearchContentChangedEvent, value); }
            remove { RemoveHandler(SearchContentChangedEvent, value); }
        }        
        private void OnSearchContentChanged(String oldValue,String newValue) {
            RoutedPropertyChangedEventArgs<string> args = new RoutedPropertyChangedEventArgs<string>(oldValue, newValue)
            {
                RoutedEvent = SearchContentChangedEvent
            };
            RaiseEvent(args);
        }

        public static readonly RoutedEvent EdittingStutasChangedEvent = EventManager.RegisterRoutedEvent("EdittingStutasChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<bool>), typeof(SearchBar));

        public event RoutedPropertyChangedEventHandler<bool> EdittingStutasChanged {
            add { AddHandler(EdittingStutasChangedEvent, value); }
            remove { RemoveHandler(EdittingStutasChangedEvent, value); }
        }

        private void OnEditintStatusChanged(bool oldValue,bool newValue) {
            RoutedPropertyChangedEventArgs<bool> args = new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue) {
                RoutedEvent = EdittingStutasChangedEvent
            };
            RaiseEvent(args);
        }

        public static readonly RoutedEvent SeachButtonClickEvent = EventManager.RegisterRoutedEvent("SeachButtonClick", RoutingStrategy.Bubble, typeof(EventHandler), typeof(SearchBar));

        public event EventHandler SeachButtonClick {
            add { AddHandler(SeachButtonClickEvent, value); }
            remove { RemoveHandler(SeachButtonClickEvent, value); }
        }
        private  void OnSeachButtonClick() {
            RoutedEventArgs args = new RoutedEventArgs() { RoutedEvent = SeachButtonClickEvent };
            RaiseEvent(args);           
        }
        #endregion

        private void QueryTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string str = this.QueryTBox.Text.Trim();
            if (String.IsNullOrWhiteSpace(str))
            {
                this.DeleteButton.Visibility = Visibility.Collapsed;
            }
            else {
                this.DeleteButton.Visibility = Visibility.Visible;              
            }
            SearchContent = str;
        }

        private void QueryTBox_GotFocus(object sender, RoutedEventArgs e)
        {
            IsEditing = true;
            this.MainBorder.BorderBrush = ActivatedBrush;
        }

        private void QueryTBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsEditing = false;
            this.MainBorder.BorderBrush = NomalBrush;
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {            
            this.OnSeachButtonClick();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            this.QueryTBox.Text = String.Empty;
         }
    }
}
