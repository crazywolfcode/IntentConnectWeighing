using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
namespace IntentConnectWeighing
{
    class AnimationHelper
    {
        private static double vibrationWidth, vibrationHeignt, leftCorrdinate, topCorrdinate;
        private static string path;
        public static Storyboard getVibrationAnimation(Window win, Orientation orientation, EventHandler handle = null, int vibrationTimes = 8)
        {
            Storyboard storyBoard = new Storyboard();
            vibrationWidth = vibrationHeignt = 4;
            leftCorrdinate = win.Left;
            topCorrdinate = win.Top;
            if (vibrationTimes < 2)
            {
                vibrationTimes = 2;
            }
            if (vibrationTimes % 2 != 0)
            {
                vibrationTimes = vibrationTimes + 1;
            }
            if (orientation == Orientation.Horizontal)
            {
                path = "Left";
            }
            else if (orientation == Orientation.Vertical)
            {
                path = "Top";
            }
            else
            {
                path = "Left";
            }
            for (int index = 1; index <= vibrationTimes; index++)
            {
                DoubleAnimation da = new DoubleAnimation();
                if (orientation == Orientation.Vertical)
                {
                    if (index % 2 == 0)
                    {
                        da.From = win.Top;
                        da.To = topCorrdinate + vibrationHeignt;
                    }
                    else
                    {
                        da.From = win.Top;
                        da.To = topCorrdinate - vibrationHeignt;
                    }
                }
                else
                {
                    if (index % 2 == 0)
                    {
                        da.From = win.Left;
                        da.To = leftCorrdinate - vibrationWidth;
                    }
                    else
                    {
                        da.From = win.Left;
                        da.To = leftCorrdinate + vibrationWidth;
                    }
                }
                da.BeginTime = TimeSpan.Parse("0:0:" + index * 0.05);
                da.Duration = new Duration(TimeSpan.Parse("0:0:0.05"));
                da.FillBehavior = FillBehavior.Stop;
                Storyboard.SetTarget(da, win);
                Storyboard.SetTargetProperty(da, new PropertyPath(path));
                storyBoard.Children.Add(da);
            }
            if (handle != null)
                storyBoard.Completed += handle; 

            return storyBoard;
        }

    }
}
