using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MyCustomControlLibrary
{
    [TemplateVisualState(Name = "Large", GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Small", GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Inactive", GroupName = "ActiveStates")]
    [TemplateVisualState(Name = "Active", GroupName = "ActiveStates")]
    public class CirclePointRingLoading : Control
    {
        private string StateActive = "Active";
        private string StateInActive = "InActive";
        private string StateLarge = "Large";
        private string StateSmall = "Small";

        #region BindableWidth
        public static readonly DependencyProperty BindableWidthProperty = DependencyProperty.Register("BindableWidth", typeof(double), typeof(CirclePointRingLoading), new PropertyMetadata(default(double), BindableWidthCallback));
        public double BindableWidth
        {
            get { return (double)GetValue(BindableWidthProperty); }
            private set { SetValue(BindableWidthProperty, value); }
        }
        private static void BindableWidthCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var ring = dependencyObject as CirclePointRingLoading;
            if (ring == null)
                return;
            ring.SetEllipseDiameter((double)e.NewValue);
            ring.SetEllipseOffset((double)e.NewValue);
            ring.SetMaxSideLength((double)e.NewValue);
        }

        /// <summary>
        /// 设置圆点的直径
        /// </summary>
        /// <param name="width"></param>
        private void SetEllipseDiameter(double width) {
            EllipseDiameter = width / 8;
        }

        private void SetEllipseOffset(double width) {
            EllipseOffset = new Thickness(0, width / 2, 0, 0);
        }
        /// <summary>
        /// 设置最大的边距
        /// </summary>
        /// <param name="width"></param>
        private void SetMaxSideLength(double width) {
            MaxSideLength = width <= 20 ? 20 : width;
        }

        #endregion

        #region IsActive

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(CirclePointRingLoading), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsActiveChanged));

        private static void IsActiveChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CirclePointRingLoading loading = sender as CirclePointRingLoading;
            if (loading == null)
            {
                return;
            }
            loading.UpdateActiveState();
        }
        private void UpdateActiveState()
        {
            if (IsActive)
                VisualStateManager.GoToState(this, StateActive, true);
            else
                VisualStateManager.GoToState(this, StateInActive, true);
        }
        #endregion

        #region IsLarge

        public bool IsLarge
        {
            get { return (bool)GetValue(IsLargeProperty); }
            set { SetValue(IsLargeProperty, value); }
        }

        public static readonly DependencyProperty IsLargeProperty = DependencyProperty.Register("IsLarge", typeof(bool), typeof(CirclePointRingLoading), new PropertyMetadata(true, IsLargeChangedCallback));

        private static void IsLargeChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CirclePointRingLoading loading = sender as CirclePointRingLoading;
            if (loading == null)
            {
                return;
            }
            loading.UpdateLargeState();
        }
        private void UpdateLargeState()
        {
            if (IsLarge)
                VisualStateManager.GoToState(this, StateLarge, true);
            else
                VisualStateManager.GoToState(this, StateSmall, true);
        }
        #endregion

        #region MaxSideLength
        public double MaxSideLength
        {
            get { return (double)GetValue(MaxSideLengthProperty); }
            set { SetValue(MaxSideLengthProperty, value); }
        }

        public static readonly DependencyProperty MaxSideLengthProperty = DependencyProperty.Register("MaxSideLength", typeof(double), typeof(CirclePointRingLoading), new PropertyMetadata(default(double)));

        #endregion

        #region EllipseDiameter

        public double EllipseDiameter
        {
            get { return (double)GetValue(EllipseDiameterProperty); }
            set { SetValue(EllipseDiameterProperty, value); }
        }
        public static readonly DependencyProperty EllipseDiameterProperty = DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(CirclePointRingLoading), new PropertyMetadata(default(double)));

        #endregion

        #region EllipseOffset
        public Thickness EllipseOffset
        {
            get { return (Thickness)GetValue(EllipseOffsetProperty); }
            set { SetValue(EllipseOffsetProperty, value); }
        }

        public static readonly DependencyProperty EllipseOffsetProperty =
            DependencyProperty.Register("EllipseOffset", typeof(Thickness), typeof(CirclePointRingLoading), new PropertyMetadata(default(Thickness)));
        #endregion

        #region constructian
        public CirclePointRingLoading()
        {
            SizeChanged += delegate
            {
                BindableWidth = ActualWidth;
            };
        }

        static CirclePointRingLoading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CirclePointRingLoading), new FrameworkPropertyMetadata(typeof(CirclePointRingLoading)));

            VisibilityProperty.OverrideMetadata(typeof(CirclePointRingLoading),
                                              new FrameworkPropertyMetadata(
                                                  new PropertyChangedCallback(
                                                      (ringObject, e) =>
                                                      {
                                                          if (e.NewValue != e.OldValue)
                                                          {
                                                              var ring = (CirclePointRingLoading)ringObject;
                                                              if ((Visibility)e.NewValue != Visibility.Visible)
                                                              {
                                                                  ring.SetCurrentValue(CirclePointRingLoading.IsActiveProperty, false);
                                                              }
                                                              else
                                                              {
                                                                  ring.IsActive = true;
                                                              }
                                                          }
                                                      })));
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateLargeState();
            UpdateActiveState();
        }
    }
}
