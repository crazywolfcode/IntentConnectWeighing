using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyCustomControlLibrary
{
    public class WaterfallPaenel : System.Windows.Controls.Panel
    {

        public static double[] ColumnHeight;

        public int ColumnCount
        {
            get { return (int)GetValue(ColumnCountProperty); }
            set { SetValue(ColumnCountProperty, value); }
        }

        public static readonly DependencyProperty ColumnCountProperty =
            DependencyProperty.Register("ColumnCount", typeof(int), typeof(WaterfallPaenel), new PropertyMetadata(PropertyChanged));

        private static void PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ColumnHeight = new double[(int)e.NewValue];
            if (sender == null || e.NewValue == e.OldValue)
                return;
            sender.SetValue(ColumnCountProperty, e.NewValue);           
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Console.WriteLine(ColumnCount);
            //清空所有列的高度
            for (int i = 0; i < ColumnHeight.Count(); i++)
            {
                ColumnHeight[i] = 0;
            }
            //计算行数
            int indexY = this.Children.Count / ColumnCount;

            //计算行数
            if (this.Children.Count % ColumnCount > 0) indexY++;
            //第几行
            int flagY = 0;
            //声明一个尺寸，用来存放测量后面板的尺寸
            Size resultSize = new Size(0, 0);
            for (int i = 0; i < indexY; i++)//行
            {
                //计算面板要呈现的宽度
                resultSize.Width = Children[i].DesiredSize.Width * ColumnCount;
                //处理最后一行
                if (i == indexY - 1)
                {

                    //剩余内容项个数
                    int residual = Children.Count - i * ColumnCount;
                    //如果集合总数小于列数，那么剩余内容项就是集合总数
                    if (Children.Count <= ColumnCount)
                    {
                        residual = Children.Count;
                    }
                    //循环剩余元素，设置元素呈现大小，计算当前列需要的高度
                    for (int h = 0; h < residual; h++)
                    {
                        //更新当前循环元素的布局
                        Children[ColumnCount * flagY + h].Measure(availableSize);
                        //累加每一列元素的高度
                        ColumnHeight[h] += Children[ColumnCount * flagY + h].DesiredSize.Height;
                        //Console.WriteLine(string.Format("测量高度{1}：{0}", Children[ColumnCount * flagY + h].DesiredSize.Height, ColumnCount * flagY + h));
                    }
                }
                else
                {

                    for (int y = 0; y < ColumnCount; y++)
                    {
                        Children[ColumnCount * flagY + y].Measure(availableSize);
                        ColumnHeight[y] += Children[ColumnCount * flagY + y].DesiredSize.Height;
                        //Console.WriteLine(string.Format("测量高度{1}：{0}", Children[ColumnCount * flagY + y].DesiredSize.Height, ColumnCount * flagY + y));
                    }
                    flagY++;
                }
            }
            //面板的高度等于所有列中最高的值
            resultSize.Height = ColumnHeight.Max();
            Console.WriteLine(resultSize.Height);

            //设置面板呈现的高度
            //如果父元素给子元素提供的是一个无穷的宽，则使用计算的宽度，否则使用父元素的宽
            resultSize.Width =double.IsPositiveInfinity(availableSize.Width) ?
            resultSize.Width : availableSize.Width;

            //设置面板呈现的高度
            //如果父元素给子元素提供的是一个无穷的高，则使用计算的宽度，否则使用父元素的高
            resultSize.Height =
            double.IsPositiveInfinity(availableSize.Height) ?
            resultSize.Height : availableSize.Height;
            //输出
            //Console.WriteLine(string.Format("Width:{0},Height:{1}", resultSize.Width, resultSize.Height));
            //返回测量尺寸
            return resultSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //清空所有列的高度
            for (int i = 0; i < ColumnHeight.Count(); i++)
            {
                ColumnHeight[i] = 0;
            }
            //计算行数
            int indexY = this.Children.Count / ColumnCount;
            if (this.Children.Count % ColumnCount > 0) indexY++;

            //当前行
            int flagY = 0;

            //当前行高
            double flagX = 0;
            #region 实际值

            //循环所有行
            for (int i = 0; i < indexY; i++)
            {
                //元素最终的宽度
                finalSize.Width = Children[i].DesiredSize.Width * ColumnCount;

                //处理最后一行
                if (i == indexY - 1)
                {
                    //列宽
                    flagX = 0;
                    //剩余项个数
                    int residual = Children.Count - i * ColumnCount;
                    if (Children.Count <= ColumnCount)
                    {
                        residual = Children.Count;
                    }
                    for (int h = 0; h < residual; h++)
                    {

                        //Console.WriteLine(string.Format("实际坐标{4}：{0},{1},{2},{3}", flagX, ColumnHeight[h], Children[ColumnCount * i + h].DesiredSize.Width, Children[ColumnCount * i + h].DesiredSize.Height, ColumnCount * flagY + h));
                        Children[ColumnCount * i + h].Arrange(new Rect(new Point(flagX, ColumnHeight[h]), Children[ColumnCount * i + h].DesiredSize));
                        ColumnHeight[h] += Children[ColumnCount * i + h].DesiredSize.Height;
                        flagX += Children[ColumnCount * i + h].DesiredSize.Width;
                    }
                }
                else
                {
                    flagX = 0;
                    for (int y = 0; y < ColumnCount; y++)
                    {
                        //Console.WriteLine(string.Format("实际坐标{4}：{0},{1},{2},{3}", flagX, i * ColumnHeight[y], Children[ColumnCount * i + y].DesiredSize.Width, Children[ColumnCount * i + y].DesiredSize.Height, ColumnCount * flagY + y));
                        Children[ColumnCount * flagY + y].Arrange(new Rect(new Point(flagX, ColumnHeight[y]), Children[ColumnCount * i + y].DesiredSize));
                        ColumnHeight[y] += Children[ColumnCount * flagY + y].DesiredSize.Height;
                        flagX += Children[ColumnCount * flagY + y].DesiredSize.Width;
                    }
                    flagY++;
                }
            }
            #endregion 测量值
            return finalSize;
        }



    }
}
