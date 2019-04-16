using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyCustomControlLibrary
{
    public class IndictorPanel : Decorator
    {
        #region 依赖属性
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register("Placement", typeof(Placement), typeof(IndictorPanel),
            new FrameworkPropertyMetadata(Placement.RightCenter, FrameworkPropertyMetadataOptions.AffectsRender, OnDirectionPropertyChangedCallback));

        public Placement Placement
        {
            get { return (Placement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }
        public static readonly DependencyProperty TailWidthProperty =
          DependencyProperty.Register("TailWidth", typeof(double), typeof(IndictorPanel), new FrameworkPropertyMetadata(10d, FrameworkPropertyMetadataOptions.AffectsRender, null));
        /// <summary>
        /// 尾巴的宽度，默认值为7
        /// </summary>
        public double TailWidth
        {
            get { return (double)GetValue(TailWidthProperty); }
            set { SetValue(TailWidthProperty, value); }
        }
        public static readonly DependencyProperty TailHeightProperty =
       DependencyProperty.Register("TailHeight", typeof(double), typeof(IndictorPanel), new FrameworkPropertyMetadata(10d, FrameworkPropertyMetadataOptions.AffectsRender, null));
        /// <summary>
        /// 尾巴的高度，默认值为10
        /// </summary>
        public double TailHeight
        {
            get { return (double)GetValue(TailHeightProperty); }
            set { SetValue(TailHeightProperty, value); }
        }

        public static readonly DependencyProperty TailVerticalOffsetProperty =
           DependencyProperty.Register("TailVerticalOffset", typeof(double), typeof(IndictorPanel), new FrameworkPropertyMetadata(13d, FrameworkPropertyMetadataOptions.AffectsRender, null));
        /// <summary>
        /// 尾巴距离顶部的距离，默认值为10
        /// </summary>
        public double TailVerticalOffset
        {
            get { return (double)GetValue(TailVerticalOffsetProperty); }
            set { SetValue(TailVerticalOffsetProperty, value); }
        }

        public static readonly DependencyProperty TailHorizontalOffsetProperty = DependencyProperty.Register("TailHorizontalOffset", typeof(double), typeof(IndictorPanel), new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsRender, null));
        /// <summary>
        /// 尾巴距离顶部的距离，默认值为10
        /// </summary>
        public double TailHorizontalOffset
        {
            get { return (double)GetValue(TailHorizontalOffsetProperty); }
            set { SetValue(TailHorizontalOffsetProperty, value); }
        }
        public static readonly DependencyProperty BackgroundProperty =
          DependencyProperty.Register("Background", typeof(Brush), typeof(IndictorPanel)
              , new FrameworkPropertyMetadata(Brushes.LightGray, FrameworkPropertyMetadataOptions.AffectsRender, null));
        /// <summary>
        /// 背景色，默认值为#FFFFFF，白色
        /// </summary>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register("Padding", typeof(Thickness), typeof(IndictorPanel)
                , new FrameworkPropertyMetadata(new Thickness(10, 5, 10, 5), FrameworkPropertyMetadataOptions.AffectsRender, null));
        /// <summary>
        /// 内边距
        /// </summary>
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
        public static readonly DependencyProperty BorderBrushProperty =
           DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(IndictorPanel)
               , new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsMeasure, null));
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(IndictorPanel), new FrameworkPropertyMetadata(new Thickness(1), FrameworkPropertyMetadataOptions.AffectsRender, null));
        /// <summary>
        /// 边框大小
        /// </summary>
        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius)
                , typeof(IndictorPanel), new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.AffectsMeasure, null));
        /// <summary>
        /// 边框大小
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion  DependencyProperty

        #region 方法重写
        /// <summary>
        /// 该方法用于测量整个控件的大小
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns>控件的大小</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            Thickness padding = this.Padding;

            Size result = new Size();
            if (Child != null)
            {
                //测量子控件的大小
                Child.Measure(constraint);

                //三角形在左边与右边的，整个容器的宽度则为：里面子控件的宽度 + 设置的padding + 三角形的宽度
                //三角形在上面与下面的，整个容器的高度则为：里面子控件的高度 + 设置的padding + 三角形的高度
                switch (Placement)
                {
                    case Placement.LeftTop:
                    case Placement.LeftBottom:
                    case Placement.LeftCenter:
                    case Placement.RightTop:
                    case Placement.RightBottom:
                    case Placement.RightCenter:
                        result.Width = Child.DesiredSize.Width + padding.Left + padding.Right + this.TailWidth;
                        result.Height = Child.DesiredSize.Height + padding.Top + padding.Bottom;
                        break;
                    case Placement.TopLeft:
                    case Placement.TopCenter:
                    case Placement.TopRight:
                    case Placement.BottomLeft:
                    case Placement.BottomCenter:
                    case Placement.BottomRight:
                        result.Width = Child.DesiredSize.Width + padding.Left + padding.Right;
                        result.Height = Child.DesiredSize.Height + padding.Top + padding.Bottom + this.TailHeight;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// 设置子控件的大小与位置
        /// </summary>
        /// <param name="arrangeSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Thickness padding = this.Padding;
            if (Child != null)
            {
                switch (Placement)
                {
                    case Placement.LeftTop:
                    case Placement.LeftBottom:
                    case Placement.LeftCenter:
                        Child.Arrange(new Rect(new Point(padding.Left + this.TailWidth, padding.Top), Child.DesiredSize));
                        //ArrangeChildLeft();
                        break;
                    case Placement.RightTop:
                    case Placement.RightBottom:
                    case Placement.RightCenter:
                        ArrangeChildRight(padding);
                        break;
                    case Placement.TopLeft:
                    case Placement.TopRight:
                    case Placement.TopCenter:
                        Child.Arrange(new Rect(new Point(padding.Left, this.TailHeight + padding.Top), Child.DesiredSize));
                        break;
                    case Placement.BottomLeft:
                    case Placement.BottomRight:
                    case Placement.BottomCenter:
                        Child.Arrange(new Rect(new Point(padding.Left, padding.Top), Child.DesiredSize));
                        break;
                    default:
                        break;
                }
            }
            return arrangeSize;
        }

        private void ArrangeChildRight(Thickness padding)
        {
            double x = padding.Left;
            double y = padding.Top;

            if (!Double.IsNaN(this.Height) && this.Height != 0)
            {
                y = (this.Height - (Child.DesiredSize.Height)) / 2;
            }

            Child.Arrange(new Rect(new Point(x, y), Child.DesiredSize));
        }

        /// <summary>
        /// 绘制控件
        /// </summary>
        /// <param name="drawingContext"></param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (Child != null)
            {
                Geometry cg = null;
                Brush brush = null;
                //DpiScale dpi = base.getd();
                Pen pen = new Pen
                {
                    Brush = this.BorderBrush,
                    Thickness = IndictorPanel.RoundLayoutValue(BorderThickness.Left, 0.5)
                };

                switch (Placement)
                {
                    case Placement.LeftTop:
                    case Placement.LeftBottom:
                    case Placement.LeftCenter:
                        //生成小尾巴在左侧的图形和底色
                        cg = CreateGeometryTailAtLeft();
                        brush = CreateFillBrush();
                        break;
                    case Placement.RightTop:
                    case Placement.RightCenter:
                    case Placement.RightBottom:
                        //生成小尾巴在右侧的图形和底色
                        cg = CreateGeometryTailAtRight();
                        brush = CreateFillBrush();
                        break;
                    case Placement.TopLeft:
                    case Placement.TopCenter:
                    case Placement.TopRight:
                        //生成小尾巴在右侧的图形和底色
                        cg = CreateGeometryTailAtTop();
                        brush = CreateFillBrush();
                        break;
                    case Placement.BottomLeft:
                    case Placement.BottomCenter:
                    case Placement.BottomRight:
                        //生成小尾巴在右侧的图形和底色
                        cg = CreateGeometryTailAtBottom();
                        brush = CreateFillBrush();
                        break;
                    default:
                        break;
                }
                GuidelineSet guideLines = new GuidelineSet();
                drawingContext.PushGuidelineSet(guideLines);
                drawingContext.DrawGeometry(brush, pen, cg);
            }
        }
        #endregion 方法重写


        private static double RoundLayoutValue(double value, double dpiScale)
        {
            double num;
            if (!IndictorPanel.AreClose(dpiScale, 1.0))
            {
                num = Math.Round(value * dpiScale) / dpiScale;
                if (double.IsInfinity(num) || IndictorPanel.AreClose(num, 1.7976931348623157E+308))
                {
                    num = value;
                }
            }
            else
            {
                num = Math.Round(value);
            }
            return num;
        }

        static bool AreClose(double value1, double value2)
        {
            if (value1 == value2)
            {
                return true;
            }
            double num = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * 2.2204460492503131E-16;
            double num2 = value1 - value2;
            return -num < num2 && num > num2;
        }
        #region 私有方法
        private Geometry CreateGeometryTailAtRight()
        {
            CombinedGeometry result = new CombinedGeometry();

            //三角形默认居中
            this.TailVerticalOffset = (this.ActualHeight - this.TailHeight) / 2;

            #region 绘制三角形
            Point arcPoint1 = new Point(this.ActualWidth - TailWidth, TailVerticalOffset);
            Point arcPoint2 = new Point(this.ActualWidth, TailVerticalOffset + TailHeight / 2);
            Point arcPoint3 = new Point(this.ActualWidth - TailWidth, TailVerticalOffset + TailHeight);

            LineSegment as1_2 = new LineSegment(arcPoint2, false);
            LineSegment as2_3 = new LineSegment(arcPoint3, false);

            PathFigure pf1 = new PathFigure();
            pf1.IsClosed = false;
            pf1.StartPoint = arcPoint1;
            pf1.Segments.Add(as1_2);
            pf1.Segments.Add(as2_3);

            PathGeometry pg1 = new PathGeometry();
            pg1.Figures.Add(pf1);
            #endregion

            #region 绘制矩形边框
            RectangleGeometry rg2 = new RectangleGeometry(new Rect(0, 0, this.ActualWidth - TailWidth, this.ActualHeight)
                , CornerRadius.TopLeft, CornerRadius.BottomRight, new TranslateTransform(0.5, 0.5));
            #endregion

            #region 合并两个图形
            result.Geometry1 = pg1;
            result.Geometry2 = rg2;
            result.GeometryCombineMode = GeometryCombineMode.Union;
            #endregion

            return result;
        }

        private Geometry CreateGeometryTailAtLeft()
        {
            CombinedGeometry result = new CombinedGeometry();

            switch (this.Placement)
            {
                case Placement.LeftTop:
                    //不做任何处理
                    break;
                case Placement.LeftBottom:
                    this.TailVerticalOffset = this.ActualHeight - this.TailHeight - this.TailVerticalOffset;
                    break;
                case Placement.LeftCenter:
                    this.TailVerticalOffset = (this.ActualHeight - this.TailHeight) / 2;
                    break;
            }

            #region 绘制三角形
            Point arcPoint1 = new Point(TailWidth, TailVerticalOffset);
            Point arcPoint2 = new Point(0, TailVerticalOffset + TailHeight / 2);
            Point arcPoint3 = new Point(TailWidth, TailVerticalOffset + TailHeight);

            LineSegment as1_2 = new LineSegment(arcPoint2, false);
            LineSegment as2_3 = new LineSegment(arcPoint3, false);

            PathFigure pf = new PathFigure
            {
                IsClosed = false,
                StartPoint = arcPoint1
            };
            pf.Segments.Add(as1_2);
            pf.Segments.Add(as2_3);

            PathGeometry g1 = new PathGeometry();
            g1.Figures.Add(pf);
            #endregion

            #region 绘制矩形边框
            RectangleGeometry g2 = new RectangleGeometry(new Rect(TailWidth, 0, this.ActualWidth - this.TailWidth, this.ActualHeight)
                , CornerRadius.TopLeft, CornerRadius.BottomRight);
            #endregion

            #region 合并两个图形
            result.Geometry1 = g1;
            result.Geometry2 = g2;
            result.GeometryCombineMode = GeometryCombineMode.Union;
            #endregion

            return result;
        }

        private Geometry CreateGeometryTailAtTop()
        {
            CombinedGeometry result = new CombinedGeometry();

            switch (this.Placement)
            {
                case Placement.TopLeft:
                    break;
                case Placement.TopCenter:
                    this.TailHorizontalOffset = (this.ActualWidth - this.TailWidth) / 2;
                    break;
                case Placement.TopRight:
                    this.TailHorizontalOffset = this.ActualWidth - this.TailWidth - this.TailHorizontalOffset;
                    break;
            }

            #region 绘制三角形
            Point anglePoint1 = new Point(this.TailHorizontalOffset, this.TailHeight);
            Point anglePoint2 = new Point(this.TailHorizontalOffset + (this.TailWidth / 2), 0);
            Point anglePoint3 = new Point(this.TailHorizontalOffset + this.TailWidth, this.TailHeight);

            LineSegment as1_2 = new LineSegment(anglePoint2, true);
            LineSegment as2_3 = new LineSegment(anglePoint3, true);

            PathFigure pf = new PathFigure();
            pf.IsClosed = false;
            pf.StartPoint = anglePoint1;
            pf.Segments.Add(as1_2);
            pf.Segments.Add(as2_3);

            PathGeometry g1 = new PathGeometry();
            g1.Figures.Add(pf);
            #endregion

            #region 绘制矩形边框
            RectangleGeometry g2 = new RectangleGeometry(new Rect(0, this.TailHeight, this.ActualWidth, this.ActualHeight - this.TailHeight)
                , CornerRadius.TopLeft, CornerRadius.BottomRight);
            #endregion

            #region 合并
            result.Geometry1 = g1;
            result.Geometry2 = g2;
            result.GeometryCombineMode = GeometryCombineMode.Union;
            #endregion

            return result;
        }

        private Geometry CreateGeometryTailAtBottom()
        {
            CombinedGeometry result = new CombinedGeometry();

            switch (this.Placement)
            {
                case Placement.BottomLeft:
                    break;
                case Placement.BottomCenter:
                    this.TailHorizontalOffset = (this.ActualWidth - this.TailWidth) / 2;
                    break;
                case Placement.BottomRight:
                    this.TailHorizontalOffset = this.ActualWidth - this.TailWidth - this.TailHorizontalOffset;
                    break;
            }


            #region 绘制三角形
            System.Windows.Point anglePoint1 = new Point(this.TailHorizontalOffset, this.ActualHeight - this.TailHeight);
            Point anglePoint2 = new Point(this.TailHorizontalOffset + this.TailWidth / 2, this.ActualHeight);
            Point anglePoint3 = new Point(this.TailHorizontalOffset + this.TailWidth, this.ActualHeight - this.TailHeight);

            LineSegment as1_2 = new LineSegment(anglePoint2, true);
            LineSegment as2_3 = new LineSegment(anglePoint3, true);

            PathFigure pf = new PathFigure();
            pf.IsClosed = false;
            pf.StartPoint = anglePoint1;
            pf.Segments.Add(as1_2);
            pf.Segments.Add(as2_3);

            PathGeometry g1 = new PathGeometry();
            g1.Figures.Add(pf);
            #endregion

            #region 绘制矩形边框
            RectangleGeometry g2 = new RectangleGeometry(new Rect(0, 0, this.ActualWidth, this.ActualHeight - this.TailHeight), CornerRadius.TopLeft, CornerRadius.BottomRight);
            #endregion

            #region 合并
            result.Geometry1 = g1;
            result.Geometry2 = g2;
            result.GeometryCombineMode = GeometryCombineMode.Union;
            #endregion

            return result;
        }

        private Brush CreateFillBrush()
        {
            Brush result = null;
            GradientStopCollection gsc = new GradientStopCollection();
            gsc.Add(new GradientStop(((SolidColorBrush)this.Background).Color, 0));
            LinearGradientBrush backGroundBrush = new LinearGradientBrush(gsc, new Point(0, 0), new Point(0, 1));
            result = backGroundBrush;
            return result;
        }

        /// <summary>
        /// 根据三角形方向设置消息框的水平位置，偏左还是偏右
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void OnDirectionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as IndictorPanel;
            self.HorizontalAlignment = ((Placement)e.NewValue == Placement.RightCenter) ?
                HorizontalAlignment.Right : HorizontalAlignment.Left;
        }
        #endregion
    }

    public enum Placement
    {
        // 左上        
        LeftTop,
        // 左中        
        LeftBottom,
        // 左下        
        LeftCenter,
        // 右上        
        RightTop,
        // 右下        
        RightBottom,
        // 右中        
        RightCenter,
        // 上左        
        TopLeft,
        // 上中        
        TopCenter,
        // 上右        
        TopRight,
        // 下左
        BottomLeft,
        // 下中
        BottomCenter,
        // 下右
        BottomRight,
    }
}
