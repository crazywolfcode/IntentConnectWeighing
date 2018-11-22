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
        public static Storyboard GetSerachBorderInAnimation(DependencyObject element, double duration,double form,double to) {
            Storyboard storyBoard = new Storyboard();           
            //DoubleAnimation da1 = new DoubleAnimation();
            //da1.BeginTime = TimeSpan.FromSeconds(0.2);
            //da1.Duration = TimeSpan.FromSeconds(0.6);
            //da1.From = 0;
            //da1.To = 1;
            //storyBoard.Children.Add(da1);
            //storyBoard.FillBehavior = FillBehavior.Stop;
            //Storyboard.SetTarget(da1, element);
            //Storyboard.SetTargetProperty(da1, new PropertyPath("Opacity"));
            //Opacity
            ThicknessAnimation da = new ThicknessAnimation();
            da.BeginTime = TimeSpan.FromSeconds(0);
            da.Duration = TimeSpan.FromSeconds(duration);
            da.From = new Thickness(0, 0, form, 0);
            da.To = new Thickness(0, 0, to, 0);
            storyBoard.Children.Add(da);
            Storyboard.SetTarget(da, element);
            Storyboard.SetTargetProperty(da, new PropertyPath("Margin"));
            return storyBoard;
        }
        public static Storyboard GetSerachBorderOutAnimation(DependencyObject element, double duration, double form, double to)
        {
            Storyboard storyBoard = new Storyboard();
            ThicknessAnimation da = new ThicknessAnimation();
            da.BeginTime = TimeSpan.FromSeconds(0);
            da.Duration = TimeSpan.FromSeconds(duration);
            da.From = new Thickness(0,0,form,0);
            da.To = new Thickness(0, 0, to, 0);
            storyBoard.Children.Add(da);
            Storyboard.SetTarget(da, element);
            Storyboard.SetTargetProperty(da, new PropertyPath("Margin"));
            //Opacity
            //DoubleAnimation da1 = new DoubleAnimation();
            //da1.BeginTime = TimeSpan.FromSeconds(0.2);
            //da1.Duration = TimeSpan.FromSeconds(0.6);
            //da1.From =0;
            //da1.To = 1;
            //storyBoard.Children.Add(da1);
         
            //Storyboard.SetTarget(da1, element);
            //storyBoard.FillBehavior = FillBehavior.HoldEnd;
            //Storyboard.SetTargetProperty(da1, new PropertyPath("Opacity"));
            return storyBoard;
        }
    }
}
