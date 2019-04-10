using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyCustomControlLibrary
{

    public class PageButton : TabButton
    {

        static PageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageButton), new FrameworkPropertyMetadata(typeof(PageButton)));
        }

        public Thickness CornerRadius
        {
            get { return (Thickness)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(Thickness), typeof(PageButton), new PropertyMetadata(new Thickness(0)));




        public Boolean IsShowIndicator
        {
            get { return (Boolean)GetValue(IsShowIndicatorProperty); }
            set { SetValue(IsShowIndicatorProperty, value); }
        }

        public static readonly DependencyProperty IsShowIndicatorProperty =
            DependencyProperty.Register("IsShowIndicator", typeof(Boolean), typeof(PageButton), new PropertyMetadata(false));

        public static readonly DependencyProperty MouseOverBackgroundProperty =
           DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(PageButton), new PropertyMetadata(Brushes.RoyalBlue));
        /// <summary>
        /// 鼠标进入背景样式
        /// </summary>
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverForegroundProperty =
            DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(PageButton), new PropertyMetadata(Brushes.White));

        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }



        public static readonly DependencyProperty AllowsAnimationProperty = DependencyProperty.Register(
            "AllowsAnimation", typeof(bool), typeof(PageButton), new PropertyMetadata(true));

        public bool AllowsAnimation
        {
            get { return (bool)GetValue(AllowsAnimationProperty); }
            set { SetValue(AllowsAnimationProperty, value); }
        }



        public int Type
        {
            get { return (int)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(int), typeof(PageButton), new PropertyMetadata(0));



    }

  
}
