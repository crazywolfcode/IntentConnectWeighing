﻿using System;
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

namespace MyCustomControlLibrary
{
    /// <summary>
    /// MMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class MMessageBox : Window
    {
        #region Variable
        private static Style currStyle;
        private static ControlTemplate currControlTemplate;
        private static System.Threading.Timer mTimer;
        private LoadType mLoadType;
        private IconType mIconType;
        private ButtonType mButtonType;
        private String mCapution = "提示";
        private String MYesBtnText;
        private String MNoBtnText;
        private int MOutTime = 5; //Loading超时限定时间5s
        private String mAlterText;
        private ShowType mShowType = ShowType.nomal;
        private Point mPoint;
        private Size mSize;
        private Orientation mOrientation;
        private String mIcon;
        private object mBrush;
        private bool mIsShowClosedBtn;

        public string mblue = "#3ca9fe";
        public string mgreen = "#1ab394";
        #endregion

        public MMessageBox()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (mShowType)
            {
                case ShowType.Alert:
                    BindingAlertTemple();
                    break;
                case ShowType.AlertModel:
                    BindingAlertModelTemple();
                    break;
                case ShowType.Loading:
                    BindingLoadingTemple();
                    break;
                case ShowType.nomal:

                    break;
                case ShowType.messageBox:
                    BindingMessageBoxTemple();
                    break;
                default:
                    this.Close();
                    break;

            }
        }
        private static MMessageBox Instance;
        public static MMessageBox GetInstance()
        {
            if (Instance != null)
            {
                Instance.Close();
            }
            return Instance = new MMessageBox();
        }

        #region alert
        /// <summary>
        /// show a window for alert 显示一个提示窗口
        /// </summary>
        /// <param name="alertText">提示的文本</param>
        /// <param name="orientation">方向</param>
        /// <param name="icon">图标字体</param>
        /// <param name="brush">颜色</param>
        /// <param name="isShowBtn">是否显示关闭按钮</param>
        /// <param name="AutoClose">是否设置自动关闭</param>
        /// <param name="AutoTime">自动关闭时间 3s</param>
        public static void ShowAlert(String alertText, Orientation orientation = Orientation.Horizontal, string icon = null, object brush = null, bool isShowBtn = true, bool AutoClose = true, int AutoTime = 3)
        {
            if (mTimer != null)
            {
                mTimer.Dispose();
            }
            var instance = GetInstance();
            instance.mAlterText = alertText;
            instance.mShowType = ShowType.Alert;
            instance.mOrientation = orientation;
            instance.mIcon = icon;
            instance.mBrush = brush;
            instance.mIsShowClosedBtn = isShowBtn;
            instance.Style = instance.FindResource(MMRK.AlertStyle.ToString()) as Style;

            if (AutoClose == true)
            {
                mTimer = new System.Threading.Timer(delegate
                {
                    instance.Dispatcher.Invoke(new Action(delegate
                        {
                            instance.Close();
                        }));
                }, null, AutoTime * 1000, 0);
            }
            instance.Show();
        }

        /// <summary>
        /// show a modal window for alert 显示一个提示模态窗口
        /// </summary>
        /// <param name="alertText">提示的文本</param>
        /// <param name="point">模态窗口起始点</param>
        /// <param name="size">模态窗口大小</param>
        /// <param name="orientation">方向</param>
        /// <param name="icon">图片</param>
        /// <param name="brush">颜色</param>
        /// <param name="isShowBtn">是否显示关闭按钮</param>
        /// <param name="AutoClose">是否设置自动关闭</param>
        /// <param name="AutoTime">自动关闭时间 3s</param>
        public static void ShowModalAlert(String alertText, Point point, Size size, Orientation orientation = Orientation.Horizontal, string icon = null, object brush = null, bool isShowBtn = true, bool AutoClose = true, int AutoTime = 3)
        {
            if (mTimer != null)
            {
                mTimer.Dispose();
            }
            var instance = GetInstance();
            instance.mAlterText = alertText;
            instance.mShowType = ShowType.AlertModel;
            instance.mPoint = point;
            instance.mSize = size;
            instance.mOrientation = orientation;
            instance.mIcon = icon;
            instance.mBrush = brush;
            instance.mIsShowClosedBtn = isShowBtn;
            instance.Style = instance.FindResource(MMRK.AlertStyle.ToString()) as Style;
            if (AutoClose == true)
            {
                mTimer = new System.Threading.Timer(delegate
                {
                    instance.Dispatcher.Invoke(new Action(delegate
                    {
                        instance.Close();
                    }));
                }, null, AutoTime * 1000, 0);
            }
            instance.Show();
        }

        public static void ShowSuccessAlert(String alertText = "操作成功！")
        {
            //ShowAlert(alertText, Orientation.Horizontal, "&#xe6b2;", Brushes.Green, false);
            ShowAlert(alertText, Orientation.Horizontal, null, Brushes.Green, false);
        }
        public static void ShowErrorAlert(String alertText = "操作失败！")
        {
            ShowAlert(alertText, Orientation.Horizontal, "&#xe508;", Brushes.Red, false);
        }
        public static void ShowSuccessModelAlert(Size size, Point point, String alertText = "操作成功！")
        {
           // ShowModalAlert(alertText, point, size, Orientation.Horizontal, "&#xe6b2;", Brushes.Green, false);
            ShowModalAlert(alertText, point, size, Orientation.Horizontal, null, Brushes.Green, false);
        }
        public static void ShowErrorModelAlert(Size size, Point point, String alertText = "操作失败！")
        {
            ShowModalAlert(alertText, point, size, Orientation.Horizontal, "&#xe508;", Brushes.Red, false);
        }
        private void BindingAlertTemple()
        {
            currControlTemplate = Instance.Template;
            if (mBrush == null)
            {
                mBrush = Brushes.Black;
            }
            else
            {
                if (mBrush is String str)
                {
                    mBrush = (Brush)new BrushConverter().ConvertFromString(str);
                }
            }

            if (String.IsNullOrEmpty(mAlterText))
            {
                return;
            }
            else
            {
                if (currControlTemplate.FindName("MTextBox", Instance) is TextBlock text)
                {
                    text.Text = mAlterText;
                    text.Foreground = (Brush)mBrush;
                }
                else
                {
                    this.Close();
                    return;
                }
            }

            if (mOrientation == Orientation.Vertical)
            {
                if (currControlTemplate.FindName("MPanel", Instance) is StackPanel panel)
                {
                    panel.Orientation = Orientation.Vertical;
                }
            }
            if (currControlTemplate.FindName("IconTextBox", Instance) is IconTextBlock icontb)
            {
                if (!string.IsNullOrEmpty(mIcon))
                {
                    var ff = icontb.FontFamily;
                    icontb.ClearValue(IconTextBlock.FontFamilyProperty);
                    icontb.Visibility = Visibility.Visible;
                    icontb.Text = mIcon;
                    icontb.FontFamily = ff;
                    icontb.Foreground = (Brush)mBrush;
                    icontb.InvalidateVisual();

                }
                else
                {
                    icontb.Visibility = Visibility.Collapsed;
                    icontb.Text = "";
                }
            }
            if (mIsShowClosedBtn == false)
            {
                if (currControlTemplate.FindName("CloseBtn", Instance) is IconButton ib)
                {
                    ib.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void BindingAlertModelTemple()
        {
            currControlTemplate = Instance.Template;
            Instance.Left = mPoint.X;
            Instance.Top = mPoint.Y;
            Instance.Width = mSize.Width;
            Instance.Height = mSize.Height;
            if (mBrush == null)
            {
                mBrush = Brushes.Black;
            }
            else
            {
                if (mBrush is String str)
                {
                    mBrush = (Brush)new BrushConverter().ConvertFromString(str);
                }
            }
            if (currControlTemplate.FindName("border", Instance) is Border border)
            {
                border.BorderThickness = new Thickness(0);
                border.CornerRadius = new CornerRadius(0);
                border.Width = mSize.Width;
                border.Height = mSize.Height;
            }
            if (String.IsNullOrEmpty(mAlterText))
            {
                return;
            }
            else
            {
                if (currControlTemplate.FindName("MTextBox", Instance) is TextBlock text)
                {
                    text.Text = mAlterText;
                    text.Foreground = (Brush)mBrush;
                }
                else
                {
                    this.Close();
                    return;
                }
            }
            if (mOrientation == Orientation.Vertical)
            {
                if (currControlTemplate.FindName("MPanel", Instance) is StackPanel panel)
                {
                    panel.Orientation = Orientation.Vertical;
                }
            }
            if (currControlTemplate.FindName("IconTextBox", Instance) is IconTextBlock icontb)
            {
                if (!string.IsNullOrEmpty(mIcon))
                {
                    icontb.BeginInit();
                    icontb.Visibility = Visibility.Visible;
                    icontb.Text = mIcon;
                    icontb.Style = icontb.Style;
                    icontb.Foreground = (Brush)mBrush;
                    icontb.EndInit();
                }
                else
                {
                    icontb.Visibility = Visibility.Collapsed;
                    icontb.Text = "";
                }
            }
            if (mIsShowClosedBtn == false)
            {
                if (currControlTemplate.FindName("CloseBtn", Instance) is IconButton ib)
                {
                    ib.Visibility = Visibility.Collapsed;
                }
            }
        }
        #endregion

        #region Loading
        /// <summary>
        /// show loading window 显示加载动画
        /// </summary>
        /// <param name="type">动画类型</param>
        /// <param name="alertText">提示文本</param>
        /// <param name="point">显示起始点</param>
        /// <param name="size">窗口的大小</param>
        /// <param name="icon">图标</param>
        /// <param name="orientation">方向</param>
        /// <param name="brush">颜色</param>
        /// <param name="outTime">超时关闭时间 5s</param>
        public static void ShowLoading(LoadType type, String alertText, Point point, Size size, String icon = null, Orientation orientation = Orientation.Horizontal, object brush = null, int outTime = 5)
        {
            if (mTimer != null)
            {
                mTimer.Dispose();
            }
            var instance = GetInstance();
            instance.mAlterText = alertText;
            instance.mLoadType = type;
            instance.mShowType = ShowType.Loading;
            instance.mPoint = point;
            instance.mSize = size;
            instance.mIcon = icon;
            instance.MOutTime = outTime;
            instance.mBrush = brush;
            instance.mOrientation = orientation;
            instance.Style = instance.FindResource(MMRK.LoadingStyle.ToString()) as Style;

            mTimer = new System.Threading.Timer(delegate
            {
                instance.Dispatcher.Invoke(new Action(delegate
                {
                    Instance.Close();
                }));
            }, null, outTime * 1000, 0);
            instance.Show();
        }

        public static void ShowIconLoading() { }

        private void BindingLoadingTemple()
        {
            currControlTemplate = Instance.Template;
            if (mBrush == null)
            {
                mBrush = Brushes.Black;
            }
            else
            {
                if (mBrush is String str)
                {
                    mBrush = (Brush)new BrushConverter().ConvertFromString(str);
                }
            }
            if (mPoint.X > 0 && mPoint.Y > 0 && mSize.Width > 0 && mSize.Height > 0)
            {
                Instance.Left = mPoint.X;
                Instance.Top = mPoint.Y;
                Instance.Width = mSize.Width;
                Instance.Height = mSize.Height;
            }

            if (currControlTemplate.FindName("border", Instance) is Border border)
            {
                if (mPoint.X > 0 && mPoint.Y > 0 && mSize.Width > 0 && mSize.Height > 0)
                {
                    border.BorderThickness = new Thickness(0);
                    border.CornerRadius = new CornerRadius(0);
                    border.Width = mSize.Width;
                    border.Height = mSize.Height;
                }
                else
                {
                    border.BorderThickness = new Thickness(1);
                    border.CornerRadius = new CornerRadius(6);
                }
            }
            var MPanel = currControlTemplate.FindName("MPanel", Instance) as StackPanel;
            if (mOrientation == Orientation.Vertical)
            {
                if (MPanel != null)
                {
                    MPanel.Orientation = Orientation.Vertical;
                }
            }
            var IPanel = currControlTemplate.FindName("IPanel", Instance) as StackPanel;
            var TPanel = currControlTemplate.FindName("TPanel", Instance) as StackPanel;
            IPanel.Children.Clear();
            TPanel.Children.Clear();
            if (IPanel == null || TPanel == null)
            {
                this.Close();
                return;
            }
            if (String.IsNullOrEmpty(mAlterText))
            {
                MPanel.Orientation = Orientation.Vertical;
                TPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                TextBlock tb = new TextBlock()
                {
                    Text = mAlterText,
                    FontSize = 14,
                    Margin = new Thickness(4, 2, 4, 2),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = (Brush)mBrush
                };
                TPanel.Visibility = Visibility.Visible;
                TPanel.Children.Add(tb);
            }
            switch (mLoadType)
            {
                case LoadType.Icon:
                    if (String.IsNullOrEmpty(mIcon))
                    {
                        IPanel.Children.Add(getDefaultLoading());
                    }
                    else
                    {
                        IconLoading iconLoading = new IconLoading
                        {
                            Icon = mIcon,
                            Foreground = (Brush)mBrush,
                            IconSize = 30
                        };
                        IPanel.Children.Add(iconLoading);
                    }
                    break;
                case LoadType.One:
                    OnePointLoading pointLoading = new OnePointLoading()
                    {
                        Margin = new Thickness(5),
                        Foreground = (Brush)mBrush,
                    };
                    IPanel.Children.Add(pointLoading);
                    break;
                case LoadType.Two:
                    TwoPointLoading twoPointLoading = new TwoPointLoading()
                    {
                        EllipseWidth = 16,
                        Margin = new Thickness(4),
                        Foreground = (Brush)mBrush,
                    };
                    IPanel.Children.Add(twoPointLoading);
                    break;
                case LoadType.rander:
                    RadnerLoading radnerLoading = new RadnerLoading()
                    {
                        Foreground = (Brush)mBrush,
                    };
                    IPanel.Children.Add(radnerLoading);
                    break;
                case LoadType.Three:
                    ThreePoingLoading threePoing = new ThreePoingLoading()
                    {
                        Foreground = (Brush)mBrush,
                    };
                    IPanel.Children.Add(threePoing);
                    break;
                case LoadType.Foure:
                    HorizontalPoingLoading horizontalPoing = new HorizontalPoingLoading()
                    {
                        EllipseDiameter = 16,
                        FillBrush = (Brush)mBrush,
                    };
                    IPanel.Children.Add(horizontalPoing);
                    break;
                case LoadType.Firve:
                    FiveColumnLoading fiveColumnLoading = new FiveColumnLoading()
                    {
                        Foreground = (Brush)mBrush,
                    };
                    IPanel.Children.Add(fiveColumnLoading);
                    break;
                case LoadType.Grid:
                    GridLoading gridLoading = new GridLoading()
                    {
                        Foreground = (Brush)mBrush,
                    };
                    IPanel.Children.Add(gridLoading);
                    break;
                case LoadType.Circle:
                    IPanel.Children.Add(getDefaultLoading());
                    break;
                default:
                    IPanel.Children.Add(getDefaultLoading());
                    break;
            }
        }

        private CirclePointRingLoading getDefaultLoading()
        {
            CirclePointRingLoading loading = new CirclePointRingLoading
            {
                Foreground = Foreground = (Brush)mBrush,
                Width = 60,
                Height = 60,
                IsActive = true,
                IsLarge = true
            };
            return loading;
        }
        #endregion

        #region Box
        public static Reault ShouBox(String alertText, string caption, ButtonType buttonType, IconType iconType, Orientation orientation = Orientation.Horizontal,string yesBtnText=null,String nobtnText=null)
        {
            if (mTimer != null)
            {
                mTimer.Dispose();
            }
            var instance = GetInstance();
            instance.ShowInTaskbar = true;
            instance.Style = instance.FindResource(MMRK.BoxStyle.ToString()) as Style;
            instance.mShowType = ShowType.messageBox;
            instance.mAlterText = alertText;
            instance.mCapution = caption;
            instance.MYesBtnText = yesBtnText;
            instance.MNoBtnText = nobtnText;
            instance.mButtonType = buttonType;
            instance.mOrientation = orientation;
            instance.mIconType = iconType;
            bool result = (bool)Instance.ShowDialog();
            return result == true ? Reault.Yes : Reault.No;
        }

        private void BindingMessageBoxTemple()
        {
            currControlTemplate = Instance.Template;
            //caption
            if (currControlTemplate.FindName("Caption", Instance) is TextBlock CaptionText)
            {
                if (String.IsNullOrEmpty(mCapution))
                {
                    mCapution = "提示";             
                }
                CaptionText.Text = mCapution;
            }
            
            //alert text
            var MPanel = currControlTemplate.FindName("MPanel", Instance) as StackPanel;
            if (mOrientation == Orientation.Vertical)
            {
                if (MPanel != null)
                {
                    MPanel.Orientation = Orientation.Vertical;
                }
            }
            var IPanel = currControlTemplate.FindName("IPanel", Instance) as StackPanel;
            var TPanel = currControlTemplate.FindName("TPanel", Instance) as StackPanel;

            if (IPanel == null || TPanel == null)
            {                
                this.Close();
                return;
            }

            switch (mIconType) {
                case IconType.error:
                    if (currControlTemplate.FindName("ErrorIcon", Instance) is IconTextBlock itb) {
                        itb.Visibility = Visibility.Visible;
                    }
                    break;
                case IconType.Info:
                    if (currControlTemplate.FindName("InfoIcon", Instance) is IconTextBlock eitb)
                    {
                        eitb.Visibility = Visibility.Visible;
                    }
                    break;
                case IconType.success:
                    if (currControlTemplate.FindName("SuccessIcon", Instance) is IconTextBlock sitb)
                    {
                        sitb.Visibility = Visibility.Visible;
                    }
                    break;
                case IconType.warring:
                    if (currControlTemplate.FindName("WarringIcon", Instance) is IconTextBlock witb)
                    {
                        witb.Visibility = Visibility.Visible;
                    }
                    break;
                default:
                    MPanel.Orientation = Orientation.Vertical;
                    IPanel.Visibility = Visibility.Collapsed;
                    break;
            }

            if (String.IsNullOrEmpty(mAlterText)) {
                mAlterText = " ";
            }
            if (currControlTemplate.FindName("MTextBox", Instance) is TextBlock tb) {
                tb.Text = mAlterText;
            }
            //button
            var YesBtn = currControlTemplate.FindName("YesBtn", Instance) as IconButton; 
            var NoBtn = currControlTemplate.FindName("NoBtn", Instance) as IconButton;
            if (String.IsNullOrEmpty(MYesBtnText))
            {
                MYesBtnText = "确定";
            }
            if (String.IsNullOrEmpty(MNoBtnText))
            {
                MNoBtnText = "取消";
            }
            switch (mButtonType) {
                case ButtonType.Yes:
                    YesBtn.Visibility = Visibility.Visible;               
                    YesBtn.Content = MYesBtnText;
                    break;
                case ButtonType.No:
                    NoBtn.Visibility = Visibility.Visible;
                    NoBtn.Content = MNoBtnText;
                    break;
                case ButtonType.YesNo:
                    NoBtn.Visibility = Visibility.Visible;
                    YesBtn.Content = MYesBtnText;
                    YesBtn.Visibility = Visibility.Visible;
                    NoBtn.Content = MNoBtnText;
                    break;
            }      
        }
        #endregion



        #region close

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = false;
            }
            catch { }            
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {          
            if (mTimer != null)
            {
                mTimer.Dispose();
            }
        }
        #endregion

        #region enum
        public enum LoadType
        {
            Icon,
            One,
            rander,
            Two,
            Three,
            Foure,
            Firve,
            Circle,
            Grid,
            None
        }

        enum MMRK
        {
            AlertStyle,
            AlertTemplate,
            BoxStyle,
            LoadingStyle,
        }

        public enum Reault
        {
            Yes,
            No
        }
        public enum IconType
        {
            success,
            error,
            Info,
            warring,
            none,
        }
        public enum ButtonType
        {
            Yes,
            No,
            YesNo
        }
        public enum ShowType
        {
            Alert,
            AlertModel,
            Loading,
            LoadingModal,
            messageBox,
            nomal,
        }
        #endregion

        public static void MClosed()
        {
            if (Instance != null)
            {
                Instance.Close();
            }
        }

        private void HeaderBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void YesBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }

}
