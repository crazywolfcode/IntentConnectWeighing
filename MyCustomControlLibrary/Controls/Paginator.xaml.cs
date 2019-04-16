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
    /// Paginator.xaml 的交互逻辑
    /// </summary>
    public partial class Paginator : UserControl
    {
        #region variables
        private const String PageTag = "page";
        private const String FirstTag = "First";
        private const String UpTag = "Up";
        private const String NextTag = "Next";
        private const String LastTag = "Last";
        #endregion

        #region DependencyProperty Register

        public static readonly DependencyProperty SelectedForegroundProperty = DependencyProperty.Register("SelectedForground", typeof(Brush), typeof(TabButton), new PropertyMetadata(Brushes.Black, new PropertyChangedCallback(OnPropertyChanged)));
        public static readonly DependencyProperty SelectedeBackgroundProperty = DependencyProperty.Register("SelectedeBackground", typeof(Brush), typeof(TabButton), new PropertyMetadata(Brushes.LightSlateGray, new PropertyChangedCallback(OnPropertyChanged)));
        public static readonly DependencyProperty SelectedIndicatorColorProperty = DependencyProperty.Register("SelectedIndicatorColor", typeof(Brush), typeof(TabButton), new PropertyMetadata(Brushes.RoyalBlue, new PropertyChangedCallback(OnPropertyChanged)));
        public static readonly DependencyProperty SelectedIndicatorHeightProperty = DependencyProperty.Register("SelectedIndicatorHeight", typeof(int), typeof(TabButton), new PropertyMetadata(1, new PropertyChangedCallback(OnPropertyChanged)));
        public static readonly DependencyProperty ButtonTypeProperty = DependencyProperty.Register("ButtonType", typeof(PageButtonType), typeof(Paginator), new PropertyMetadata(PageButtonType.Radius, new PropertyChangedCallback(OnPropertyChanged)));
        public static readonly DependencyProperty DataCountProperty = DependencyProperty.Register("DataCount", typeof(int), typeof(Paginator), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnDataCountChanged)));
        public static readonly DependencyProperty ItemCountCountProperty = DependencyProperty.Register("ItemCount", typeof(int), typeof(Paginator), new PropertyMetadata(10, new PropertyChangedCallback(OnPaginatorCountChanged)));
        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register("CurrentPage", typeof(int), typeof(Paginator), new PropertyMetadata(1, new PropertyChangedCallback(OnCurrentPageChanged)));
        #endregion

        /// <summary>
        ///  选中时的前景颜色
        /// </summary>
        public Brush SelectedForground
        {
            get { return (Brush)GetValue(SelectedForegroundProperty); }
            set { SetValue(SelectedForegroundProperty, value); }
        }


        public Brush SelectedBackground
        {
            get { return (Brush)GetValue(SelectedeBackgroundProperty); }
            set { SetValue(SelectedeBackgroundProperty, value); }
        }


        public Brush SelectedIndicatorColor
        {
            get { return (Brush)GetValue(SelectedIndicatorColorProperty); }
            set { SetValue(SelectedIndicatorColorProperty, value); }
        }

        public int SelectedIndicatorHeight
        {
            get { return (int)GetValue(SelectedIndicatorHeightProperty); }
            set { SetValue(SelectedIndicatorHeightProperty, value); }
        }


        public int DataCount
        {
            get { return (int)GetValue(DataCountProperty); }
            set { SetValue(DataCountProperty, value); }
        }



        public PageButtonType ButtonType
        {
            get { return (PageButtonType)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }

        private static void OnDataCountChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Paginator Paginator = sender as Paginator;
            Paginator.Measure();
        }

        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Paginator p = sender as Paginator;
            p.Measure();
        }

        public int ItemCount
        {
            get { return (int)GetValue(ItemCountCountProperty); }
            set
            {
                if ((int)value <= 0)
                {
                    value = 10;
                }
                SetValue(ItemCountCountProperty, value);
            }
        }

        private static void OnPaginatorCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Paginator Paginator = d as Paginator;
            Paginator.Measure();
        }

        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }
        public int LastPage { get; set; }


        #region  export event       

        public static readonly RoutedEvent PaginatorSelectedEvent = EventManager.RegisterRoutedEvent("PaginatorSelected", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(Paginator));

        public event RoutedPropertyChangedEventHandler<int> PaginatorSelected
        {
            add { AddHandler(PaginatorSelectedEvent, value); }
            remove { RemoveHandler(PaginatorSelectedEvent, value); }
        }

        private static void OnCurrentPageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Paginator Paginator = sender as Paginator;
            int oldValue = (int)args.OldValue;
            int newValue = (int)args.NewValue;
            Paginator.Measure();
            //引发事件
            Paginator.RaiseEventPaginatorSelected(newValue, oldValue);
        }
        private void RaiseEventPaginatorSelected(int newValue, int oldValue)
        {
            RoutedPropertyChangedEventArgs<int> args = new RoutedPropertyChangedEventArgs<int>(newValue, oldValue)
            {
                RoutedEvent = PaginatorSelectedEvent
            };
            RaiseEvent(args);
        }
        #endregion


        public Paginator()
        {
            InitializeComponent();
        }

        #region Measure

        private void Measure()
        {
            if (DataCount <= 0 || ItemCount <= 0)
            {
                this.Visibility = Visibility.Collapsed;
                return;
            }
            else
            {
                this.Visibility = Visibility.Visible;
            }
            int pages = DataCount / ItemCount;
            if (DataCount % ItemCount > 0)
            {
                pages += 1;
            }
            if (pages <= 1)
            {
                this.Visibility = Visibility.Collapsed;
            }
            else {
                this.Visibility = Visibility.Visible;
            }
            LastPage = pages;
            //处理不需要动态生成的页面按键           
            HandleStaticPageBtn();

            int side = 2;
            int windows = 6;

            if (pages > 1)
            {
                if (pages < (side * windows))
                {
                    HideStaticBtn();
                }
                this.Visibility = Visibility.Visible;
                this.PagePanel.Visibility = Visibility.Visible;
                this.PagePanel.Children.Clear();
                if (pages < (side * windows))
                {
                    leftDot.Visibility = Visibility.Collapsed;
                    RightDot.Visibility = Visibility.Collapsed;
                    InsertPageBtn(1, pages);
                }
                else
                {
                    if (CurrentPage == 1)
                    {
                        RadiusUpPage.Visibility = Visibility.Collapsed;
                        CircleUpPage.Visibility = Visibility.Collapsed;
                        UdLineUpPage.Visibility = Visibility.Collapsed;
                    }
                    if (CurrentPage - windows <= 1)
                    {
                        leftDot.Visibility = Visibility.Collapsed;
                        RadiusFirstPage.Visibility = Visibility.Collapsed;
                        CircleFirstPage.Visibility = Visibility.Collapsed;
                        UdLineFirstPage.Visibility = Visibility.Collapsed;
                    }

                    if (CurrentPage == pages)
                    {
                        RadiusNextPage.Visibility = Visibility.Collapsed;
                        CircleNextPage.Visibility = Visibility.Collapsed;
                        UdLineNextPage.Visibility = Visibility.Collapsed;
                    }
                    if ((CurrentPage + windows) < pages)
                    {
                        RadiusLastPage.Visibility = Visibility.Collapsed;
                        CircleLastPage.Visibility = Visibility.Collapsed;
                        UdLineLastPage.Visibility = Visibility.Collapsed;
                        RightDot.Visibility = Visibility.Collapsed;
                    }

                    if ((CurrentPage - windows) > 1)
                    {
                        leftDot.Visibility = Visibility.Visible;
                        switch (ButtonType)
                        {
                            case PageButtonType.Radius:
                                RadiusFirstPage.Visibility = Visibility.Visible;
                                break;
                            case PageButtonType.Circle:
                                CircleFirstPage.Visibility = Visibility.Visible;
                                break;
                            case PageButtonType.UdLine:
                                UdLineFirstPage.Visibility = Visibility.Visible;
                                break;
                        }
                    }
                    else
                    {
                        leftDot.Visibility = Visibility.Collapsed;
                        switch (ButtonType)
                        {
                            case PageButtonType.Radius:
                                RadiusFirstPage.Visibility = Visibility.Collapsed;
                                break;
                            case PageButtonType.Circle:
                                CircleFirstPage.Visibility = Visibility.Collapsed;
                                break;
                            case PageButtonType.UdLine:
                                UdLineFirstPage.Visibility = Visibility.Collapsed;
                                break;
                        }
                    }

                    if ((CurrentPage + windows) >= pages)
                    {
                        RightDot.Visibility = Visibility.Collapsed;
                        switch (ButtonType)
                        {
                            case PageButtonType.Radius:
                                RadiusLastPage.Visibility = Visibility.Collapsed;
                                break;
                            case PageButtonType.Circle:
                                CircleLastPage.Visibility = Visibility.Collapsed;
                                break;
                            case PageButtonType.UdLine:
                                UdLineLastPage.Visibility = Visibility.Collapsed;
                                break;
                        }
                    }
                    else
                    {
                        RightDot.Visibility = Visibility.Visible;
                        switch (ButtonType)
                        {
                            case PageButtonType.Radius:
                                RadiusLastPage.Visibility = Visibility.Visible;
                                break;
                            case PageButtonType.Circle:
                                CircleLastPage.Visibility = Visibility.Visible;
                                break;
                            case PageButtonType.UdLine:
                                UdLineLastPage.Visibility = Visibility.Visible;
                                break;
                        }
                    }
                    if (CurrentPage <= windows)
                    {
                        int end = CurrentPage + windows + (windows - CurrentPage);
                        InsertPageBtn(1, end);
                        if (end < pages)
                        {
                            RightDot.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            RightDot.Visibility = Visibility.Collapsed;
                        }

                    }
                    else if (CurrentPage > windows)
                    {
                        int start = CurrentPage - windows;
                        if (start > 1)
                        {
                            leftDot.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            leftDot.Visibility = Visibility.Collapsed;
                        }
                        int end = CurrentPage + windows;
                        if (end < pages)
                        {
                            RightDot.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            end = end - (end - pages);
                            RightDot.Visibility = Visibility.Collapsed;
                        }
                        InsertPageBtn(start, end);
                    }
                }
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
                this.PagePanel.Visibility = Visibility.Collapsed;
                leftDot.Visibility = Visibility.Collapsed;
                RightDot.Visibility = Visibility.Collapsed;
            }

        }


        private void InsertPageBtn(int startPage, int endPage)
        {
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == CurrentPage)
                {
                    this.PagePanel.Children.Add(CreateBtn(i, true));
                }
                else
                {
                    this.PagePanel.Children.Add(CreateBtn(i));
                }
            }
        }
        #endregion

        private void HideStaticBtn()
        {
            RadiusFirstPage.Visibility = Visibility.Collapsed;
            RadiusUpPage.Visibility = Visibility.Collapsed;
            RadiusNextPage.Visibility = Visibility.Collapsed;
            RadiusLastPage.Visibility = Visibility.Collapsed;
            CircleFirstPage.Visibility = Visibility.Collapsed;
            CircleNextPage.Visibility = Visibility.Collapsed;
            CircleUpPage.Visibility = Visibility.Collapsed;
            CircleLastPage.Visibility = Visibility.Collapsed;
            UdLineFirstPage.Visibility = Visibility.Collapsed;
            UdLineUpPage.Visibility = Visibility.Collapsed;
            UdLineNextPage.Visibility = Visibility.Collapsed;
            UdLineLastPage.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 处理不需要动态生成的页面按键
        /// </summary>
        private void HandleStaticPageBtn()
        {
            switch (ButtonType)
            {
                case PageButtonType.UdLine:
                    RadiusFirstPage.Visibility = Visibility.Collapsed;
                    RadiusUpPage.Visibility = Visibility.Collapsed;
                    RadiusNextPage.Visibility = Visibility.Collapsed;
                    RadiusLastPage.Visibility = Visibility.Collapsed;

                    CircleFirstPage.Visibility = Visibility.Collapsed;
                    CircleNextPage.Visibility = Visibility.Collapsed;
                    CircleUpPage.Visibility = Visibility.Collapsed;
                    CircleLastPage.Visibility = Visibility.Collapsed;

                    UdLineFirstPage.Visibility = Visibility.Visible;
                    UdLineUpPage.Visibility = Visibility.Visible;
                    UdLineNextPage.Visibility = Visibility.Visible;
                    UdLineLastPage.Visibility = Visibility.Visible;
                    break;
                case PageButtonType.Circle:

                    RadiusFirstPage.Visibility = Visibility.Collapsed;
                    RadiusUpPage.Visibility = Visibility.Collapsed;
                    RadiusNextPage.Visibility = Visibility.Collapsed;
                    RadiusLastPage.Visibility = Visibility.Collapsed;

                    CircleFirstPage.Visibility = Visibility.Visible;
                    CircleNextPage.Visibility = Visibility.Visible;
                    CircleUpPage.Visibility = Visibility.Visible;
                    CircleLastPage.Visibility = Visibility.Visible;

                    UdLineFirstPage.Visibility = Visibility.Collapsed;
                    UdLineUpPage.Visibility = Visibility.Collapsed;
                    UdLineNextPage.Visibility = Visibility.Collapsed;
                    UdLineLastPage.Visibility = Visibility.Collapsed;
                    break;
                case PageButtonType.Radius:
                    RadiusFirstPage.Visibility = Visibility.Visible;
                    RadiusUpPage.Visibility = Visibility.Visible;
                    RadiusNextPage.Visibility = Visibility.Visible;
                    RadiusLastPage.Visibility = Visibility.Visible;

                    CircleFirstPage.Visibility = Visibility.Collapsed;
                    CircleNextPage.Visibility = Visibility.Collapsed;
                    CircleUpPage.Visibility = Visibility.Collapsed;
                    CircleLastPage.Visibility = Visibility.Collapsed;

                    UdLineFirstPage.Visibility = Visibility.Collapsed;
                    UdLineUpPage.Visibility = Visibility.Collapsed;
                    UdLineNextPage.Visibility = Visibility.Collapsed;
                    UdLineLastPage.Visibility = Visibility.Collapsed;
                    break;
                default:
                    RadiusFirstPage.Visibility = Visibility.Visible;
                    RadiusUpPage.Visibility = Visibility.Visible;
                    RadiusNextPage.Visibility = Visibility.Visible;
                    RadiusLastPage.Visibility = Visibility.Visible;

                    CircleFirstPage.Visibility = Visibility.Collapsed;
                    CircleNextPage.Visibility = Visibility.Collapsed;
                    CircleUpPage.Visibility = Visibility.Collapsed;
                    CircleLastPage.Visibility = Visibility.Collapsed;

                    UdLineFirstPage.Visibility = Visibility.Collapsed;
                    UdLineUpPage.Visibility = Visibility.Collapsed;
                    UdLineNextPage.Visibility = Visibility.Collapsed;
                    UdLineLastPage.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private PageButton CreateBtn(int PaginatorIndex, Boolean isCurrent = false)
        {
            PageButton button = new PageButton();
            button.Content = PaginatorIndex.ToString();
            button.Type = (int)ButtonType;
            button.FontSize = this.FontSize;
            button.IsChecked = isCurrent;
            button.IndicatorHeight = SelectedIndicatorHeight;
            button.Tag = PageTag;
            button.ActiveForground = SelectedForground;
            button.ActiveIndicatorColor = this.SelectedIndicatorColor;
            button.ActiveBackground = this.SelectedBackground;
            button.Background = this.Background;
            button.Click += PageButton_Click;
            return button;
        }

        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            PageButton button = sender as PageButton;
            String tag = button.Tag.ToString();
            int index = 0;
            try
            {
                index = Convert.ToInt32(button.Content);
                CurrentPage = index;
            }
            catch
            {
                // static orther btn
                button.IsChecked = false;
                switch (tag)
                {
                    case FirstTag:
                        CurrentPage = 1;
                        break;
                    case UpTag:
                        CurrentPage -= 1;
                        break;
                    case NextTag:
                        CurrentPage += 1;
                        break;
                    case LastTag:
                        CurrentPage = LastPage;
                        break;
                    default:
                        break;
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Measure();
        }
    }
}
