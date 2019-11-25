using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyHelper
{
    public class ControlResizerHelper
    {

        FrameworkElement mControl;
        Point prevPoint;
        Cursor defaultCursor;
        private Boolean isPressed;
        public Thickness Thickness { get; private set; }
        public double Radius { get; private set; }
        public bool? LeftDirection { get; private set; }
        public bool? TopDirection { get; private set; }

        public ControlResizerHelper(FrameworkElement control, Thickness thickness, double radius, Cursor defCursor = null)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            Thickness = thickness;
            Radius = radius;
            defaultCursor = defCursor;
            mControl = control;
            mControl.MouseEnter += control_MouseEnter;
            mControl.MouseMove += control_MouseMove;
            mControl.MouseDown += control_MouseDown;
            mControl.MouseUp += control_MouseUp;
        }

        private void control_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var point = e.GetPosition(mControl);
            if (!isPressed)
            {
                SetCursor(point);
            }
        }

        private void control_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var point = e.GetPosition(mControl);
            if (!isPressed)
            {
                SetCursor(point);
            }
            else
            {
                double vertiChange, horiChange;
                var pointScr = mControl.PointToScreen(point);
                if (LeftDirection.HasValue)
                {
                    horiChange = pointScr.X - prevPoint.X;
                    double newWidth;
                    if (LeftDirection.Value)
                    {
                        newWidth = mControl.Width - horiChange;
                        if (newWidth >= mControl.MinWidth)
                        {
                            Canvas.SetLeft(mControl, Canvas.GetLeft(mControl) + horiChange);
                            if (newWidth >= 0)
                                mControl.Width = newWidth;
                        }
                    }
                    else
                    {
                        newWidth = mControl.Width + horiChange;
                        if (newWidth <= mControl.MaxWidth && newWidth >= 0)
                        {
                            mControl.Width = newWidth;
                        }
                    }


                }
                if (TopDirection.HasValue)
                {
                    vertiChange = pointScr.Y - prevPoint.Y;
                    double newHeight;
                    if (TopDirection.Value)
                    {
                        newHeight = mControl.Height - vertiChange;
                        if (newHeight >= mControl.MinHeight && newHeight >= 0)
                        {
                            Canvas.SetTop(mControl, Canvas.GetTop(mControl) + vertiChange);
                            mControl.Height = newHeight;
                        }
                    }
                    else
                    {
                        newHeight = mControl.Height + vertiChange;
                        if (newHeight <= mControl.MaxHeight)
                        {
                            if (newHeight >= 0)
                                mControl.Height = newHeight;
                        }
                    }
                }
                prevPoint = pointScr;
            }

        }

        private void control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(mControl);
            isPressed = true;
            mControl.CaptureMouse();
            prevPoint = mControl.PointToScreen(point);
        }

        private void control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isPressed = false;
            mControl.ReleaseMouseCapture();
        }


        private void SetCursor(Point point)
        {
            var left = point.X;
            var top = point.Y;
            var right = mControl.ActualWidth - left;
            var bottom = mControl.ActualHeight - top;

            LeftDirection = TopDirection = null;
            if (left < Thickness.Left)
                LeftDirection = true;
            else if (right < Thickness.Right)
                LeftDirection = false;

            if (top < Thickness.Top)
                TopDirection = true;
            else if (bottom < Thickness.Bottom)
                TopDirection = false;


            Cursor cur = null;
            if (LeftDirection.HasValue)
            {
                //如果上下没有进入区域，尝试按照Radius属性再次计算
                if (!TopDirection.HasValue)
                {
                    if (top < Radius)
                        TopDirection = true;
                    else if (bottom < Radius)
                        TopDirection = false;
                }

                if (TopDirection.HasValue)
                {
                    if (LeftDirection.Value == TopDirection.Value)
                        cur = Cursors.SizeNWSE;
                    else
                        cur = Cursors.SizeNESW;
                }
                else
                    cur = Cursors.SizeWE;
            }
            else if (TopDirection.HasValue)
            {
                //这里leftDirection.HasValue必定是false，所以直接计算
                if (left < Radius)
                    LeftDirection = true;
                else if (right < Radius)
                    LeftDirection = false;

                if (LeftDirection.HasValue)
                {
                    if (LeftDirection.Value == TopDirection.Value)
                        cur = Cursors.SizeNWSE;
                    else
                        cur = Cursors.SizeNESW;
                }
                else
                    cur = Cursors.SizeNS;
            }
            if (cur != null)
                mControl.Cursor = cur;
            else
                mControl.Cursor = defaultCursor;
        }
    }
}
