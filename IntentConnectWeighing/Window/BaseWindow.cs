using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyCustomControlLibrary;

namespace IntentConnectWeighing
{
    public class BaseWindow :Window
    {
        private ControlTemplate baseWindowTemplate;
        private Thickness mainBorderMargin;


        public BaseWindow()
        {
            InitializeStyle();

            //缩放，最大化 等默认事件的修复
            RepairWindowDefaultEvent();

            this.StateChanged += BaseWindow_StateChanged;
    
        }

        private void BaseWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized) {
                HideShadowEffect();
            } else {
                ShowShadowEffect();
            }

        }
              
        private void InitializeStyle()
        {
           this.Style = (Style)App.Current.Resources["BaseWindowStyle"];            
        }

        public void MyInitializeStyle(Window window, MyWindowsStyle style)
        {
            switch (style)
            {
                case MyWindowsStyle.main:
                    initMinBtn(window);
                    initMaxBtn(window);
                    initCloseBtn(window);
                    initMenu(window);
                    initThemeBtn(window);
                    initTitleDragEvent();
                    HideIcon();
                    HideWindowTitle();
                    break;
                case MyWindowsStyle.nomal:
                    initMinBtn(window);
                    initMaxBtn(window);
                    initCloseBtn(window);
                    initMenu(window, false);
                    initThemeBtn(window, false);
                    initTitleDragEvent();
                    SetWindowTitleHeight(window, 30);
                    break;
                case MyWindowsStyle.dialog:
                    initMinBtn(window, false);
                    initMaxBtn(window, false);
                    initCloseBtn(window);
                    initMenu(window, false);
                    initThemeBtn(window, false);
                    initTitleDragEvent();
                    SetWindowTitleHeight(window, 30);
                    break;
            }
        }

        /// <summary>
        /// 缩放，最大化 等默认事件的修复
        /// </summary>
        public void RepairWindowDefaultEvent()
        {
            WindowBehavior.newInstance(this).RepairWindowDefaultBehavior();           
        }
        public ControlTemplate GetBaseWindowTemplate()
        {
            if (baseWindowTemplate == null)
            {
                baseWindowTemplate = TemplateHelper.GetControlTemplate(ResourceName.BaseWindowControlTemplate);
            }
            return baseWindowTemplate;
        }


        #region  公共方法
        /// <summary>
        /// 改变菜单AlertNumber的数据
        /// </summary>
        /// <param name="name">菜单的资源名称</param>
        /// <param name="value">增加或者是减少的值</param>
        /// <param name="type"> 1 增加 0 减少</param>
        public void ChangeAlertNumber(ResourceName name, int value, int type)
        {
            int temp = ResourceHelper.getIntFromDictionaryResource(name);
            if (type == 1)
            {
                ResourceHelper.setIntToDictionaryResource(name, temp + value);
            }
            else
            {
                if (temp > 0)
                {
                    ResourceHelper.setIntToDictionaryResource(name, temp - value);
                }
            }
            MessageBox.Show(name.ToString() + " " + value + " " + type + " " + temp);
        }


        /// <summary>
        ///设置窗口标题栏的高度
        /// </summary>
        /// <param name="height">高度值</param>
        public void SetWindowTitleHeight(System.Windows.Window window, int height)
        {
            Border border = GetBaseWindowTemplate().FindName("windowTitle", window) as Border;
            border.Height = height;
        }

        /// <summary>
        ///隐藏窗口标题
        /// </summary>
        /// <param name="height">高度值</param>
        public void HideWindowTitle()
        {
            TextBlock t = GetBaseWindowTemplate().FindName("WindowTitleText", this) as TextBlock;
            t.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///设置窗口标题
        /// </summary>
        /// <param name="text">窗口的标题</param>
        public void SetWindowTitle(System.Windows.Window window, string text)
        {
            TextBlock t = GetBaseWindowTemplate().FindName("WindowTitleText", window) as TextBlock;
            t.Text = text;
            t.Visibility = Visibility.Visible;
        }


        /// <summary>
        ///隐藏窗口Icon图标 todo
        /// </summary>
        public void HideIcon()
        {
            Image i = GetBaseWindowTemplate().FindName("Icon", this) as Image;
            i.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///设置窗口Icon图标
        ///Absolute path can show
        /// </summary>
        /// <param name="imageSource">icon 的图像地址</param>
        public void setIcon(System.Windows.Window window, string imageSource)
        {
            Image i = GetBaseWindowTemplate().FindName("Icon", window) as Image;
            i.Source = new BitmapImage(new Uri(imageSource, UriKind.RelativeOrAbsolute));
            if (i.Visibility != Visibility.Visible)
                i.Visibility = Visibility.Visible;
        }

        #endregion

        #region 绑定窗口事件
        /// <summary>
        /// 初始化菜单
        /// </summary>
        /// <param name="isShow"> 是否显示 </param>
        /// <param name="parent">父控件</param>
        public void initThemeBtn(Window window, bool isShow = true)
        {
            ImageButton btn = GetBaseWindowTemplate().FindName("ThemeBtn", window) as ImageButton;
            if (isShow == true)
            {
                btn.Visibility = Visibility.Visible;
            }
            else
            {
                btn.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 初始化菜单
        /// </summary>
        /// <param name="isShow"> 是否显示 </param>
        public void initMenu(System.Windows.Window window, bool isShow = true)
        {
            Menu menu = (Menu)GetBaseWindowTemplate().FindName("Menu", window);
            if (isShow == true)
            {
                menu.Visibility = Visibility.Visible;
            }
            else
            {
                menu.Visibility = Visibility.Collapsed;
            }
        }


        private void ShowShadowEffect() {
            FrameworkElement border = (FrameworkElement)GetBaseWindowTemplate().FindName("Windows_Border", this);            
            border.Margin = mainBorderMargin ;
        }
        private void HideShadowEffect() {
            FrameworkElement border = (FrameworkElement)GetBaseWindowTemplate().FindName("Windows_Border", this);
            mainBorderMargin = border.Margin;
            border.Margin = new Thickness(0);
        }

        /// <summary>
        /// 初始化最小化按键
        /// </summary>
        /// <param name="isShow"> 是否显示 </param>
        public void initMinBtn(System.Windows.Window window, bool isShow = true)
        {
            ImageButton minBtn = (ImageButton)GetBaseWindowTemplate().FindName("MinBtn", window);
            if (isShow == true)
            {
                minBtn.Visibility = Visibility.Visible;
                minBtn.Click += MinBtn_Click;
            }
            else
            {
                minBtn.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// 初始化最大化按键
        /// </summary>
        /// <param name="isShow"> 是否显示 </param>
        public void initMaxBtn(System.Windows.Window window, bool isShow = true)
        {
            ImageButton maxBtn = (ImageButton)GetBaseWindowTemplate().FindName("MaxBtn", window);
            if (isShow == true)
            {
                maxBtn.Visibility = Visibility.Visible;
                maxBtn.Click += MaxBtn_Click;
            }
            else
            {
                maxBtn.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// 初始化关闭按键
        /// </summary>
        /// <param name="isShow"> 是否显示 </param>
        /// <param name="window">控件</param>
        public void initCloseBtn(System.Windows.Window window, bool isShow = true)
        {
            ImageButton closeBtn = (ImageButton)GetBaseWindowTemplate().FindName("CloseBtn", window);
            if (isShow == true)
            {
                closeBtn.Visibility = Visibility.Visible;
                closeBtn.Click += CloseBtn_Click;
            }
            else
            {
                closeBtn.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 绑定窗口的拖动和双击事件
        /// </summary>
        /// <param name="isCanDrag"></param>
        public void initTitleDragEvent(bool isCanDrag = true)
        {
            if (isCanDrag == true)
            {
                Border windowTitle = (Border)GetBaseWindowTemplate().FindName("windowTitle", this);
                windowTitle.MouseMove += windowTitle_MouseMove;
                windowTitle.MouseDown +=   windowTitle_MouseDown;
            }
        }
        #endregion

        #region Window Event 窗口事件

        /// <summary>
        /// 窗口移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void windowTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        /// <summary>
        /// 窗口最大化与还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void MaxBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized) {
                this.WindowState = WindowState.Normal;
            }
            else if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized; 
            }
            else
            {
                this.WindowState = WindowState.Normal; 
            }
        }

        /// <summary>
        /// 窗口最小化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MinBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 关闭窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); //关闭窗口
        }

        int i = 0;
        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void windowTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //如果是右键点击，直接返回
            if (e.RightButton == MouseButtonState.Pressed)
            {
                return;
            }

            i += 1;
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer.Tick += (s, e1) => { timer.IsEnabled = false; i = 0; };
            timer.IsEnabled = true;

            //如果不是双击，直接返回
            if (i % 2 != 0)
            {
                return;
            }
            timer.IsEnabled = false;
            i = 0;
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        #endregion
    }
}
