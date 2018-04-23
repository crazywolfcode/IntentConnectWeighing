using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace IntentConnectWeighing
{
    public partial class NavMenuButton : RadioButton
    {
        /// <summary>
        /// 是否显示按钮的文本
        /// </summary>
        public bool IsShowContent
        {
            get { return (bool)GetValue(IsShowContentProperty); }
            set { SetValue(IsShowContentProperty, value); }
        }

        public static readonly DependencyProperty IsShowContentProperty =
            DependencyProperty.Register("IsShowContent", typeof(bool), typeof(NavMenuButton), new PropertyMetadata(true));

        /// <summary>
        /// 正常情况下显示的图片       
        /// </summary>
        public Image NomalImage
        {
            get { return (Image)GetValue(NomalImageProperty); }
            set { SetValue(NomalImageProperty, value); }
        }

        public static readonly DependencyProperty NomalImageProperty =
            DependencyProperty.Register("NomalImage", typeof(Image), typeof(NavMenuButton), new PropertyMetadata(null));


        /// <summary>
        /// 正常情况下显示的图片       
        /// </summary>
        public string NomalImageUrl
        {
            get { return (string)GetValue(NomalImageUrlProperty); }
            set { SetValue(NomalImageUrlProperty, value); }
        }

        public static readonly DependencyProperty NomalImageUrlProperty =
            DependencyProperty.Register("NomalImageUrl", typeof(string), typeof(NavMenuButton), new PropertyMetadata(null));



        /// <summary>
        /// 激活状态下显示的图片       
        /// </summary>
        public Image ActivityedImage
        {
            get { return (Image)GetValue(ActivityedImageProperty); }
            set { SetValue(ActivityedImageProperty, value); }
        }
        public static readonly DependencyProperty ActivityedImageProperty =
            DependencyProperty.Register("ActivityedImage", typeof(Image), typeof(NavMenuButton), new PropertyMetadata(null));


        /// <summary>
        /// 激活状态下显示的图片Url
        /// </summary>
        public string ActivityedImageUrl
        {
            get { return (string)GetValue(ActivityedImageUrlProperty); }
            set { SetValue(ActivityedImageUrlProperty, value); }
        }

        public static readonly DependencyProperty ActivityedImageUrlProperty =
            DependencyProperty.Register("ActivityedImageUrl", typeof(string), typeof(NavMenuButton), new PropertyMetadata(null));



        /// <summary>
        /// 菜单的提示数据
        /// </summary>
        public int AlertNumber
        {
            get { return (int)GetValue(AlertNumberProperty); }
            set { SetValue(AlertNumberProperty, value); }
        }

        public static readonly DependencyProperty AlertNumberProperty =
            DependencyProperty.Register("AlertNumber", typeof(int), typeof(NavMenuButton), new PropertyMetadata(0));
        /// <summary>
        /// 是否显示提示数据
        /// </summary>
        public bool IsShowAlertNumber
        {
            get { return (bool)GetValue(IsShowAlertNumberProperty); }
            set { SetValue(IsShowAlertNumberProperty, value); }
        }

        public static readonly DependencyProperty IsShowAlertNumberProperty =
            DependencyProperty.Register("IsShowAlertNumber", typeof(bool), typeof(NavMenuButton), new PropertyMetadata(false));

        static NavMenuButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavMenuButton), new FrameworkPropertyMetadata(typeof(NavMenuButton)));
        }
    }
}
