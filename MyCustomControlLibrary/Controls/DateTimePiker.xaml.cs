using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyCustomControlLibrary
{
    /// <summary>
    /// DateTimePiker.xaml 的交互逻辑
    /// </summary>
    public partial class DateTimePicker : UserControl
    {
        public DateTimePicker()
        {
            InitializeComponent();
        }


        public MDateTime Value
        {
            get { return (MDateTime)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static MDateTime GetDefaultValue()
        {
            return new MDateTime()
            {
                Year = DateTime.Now.Year,
                Month = DateTime.Now.Month,
                Day = DateTime.Now.Day,
                Hour = DateTime.Now.Hour,
                Minute = DateTime.Now.Minute,
                Second = DateTime.Now.Second
            };

        }

        public static DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(MDateTime), typeof(DateTimePicker), new FrameworkPropertyMetadata(GetDefaultValue(), new PropertyChangedCallback(OnValueChanged)));



        public String StringValue
        {
            get { return (String)GetValue(StringValueProperty); }
            set { SetValue(StringValueProperty, value); }
        }

        public static readonly DependencyProperty StringValueProperty =
            DependencyProperty.Register("StringValue", typeof(String), typeof(DateTimePicker), new PropertyMetadata("",new PropertyChangedCallback(OnStringValueChanged)));

        public static void OnStringValueChanged(DependencyObject sedner, DependencyPropertyChangedEventArgs args) {
        
            DateTimePicker picker = sedner as DateTimePicker;
            if (args.NewValue == null)
            {
                picker.MainContentTb.Text = null;
                return;
            }
            DateTime dt;
            DateTime.TryParse(args.NewValue.ToString(), out dt);
            if (dt != null)
            {
                MDateTime mDateTime = new MDateTime()
                {
                    Year = dt.Year,
                    Month = dt.Month,
                    Day = dt.Day,
                    Hour = dt.Hour,
                    Minute = dt.Minute,
                    Second = dt.Second
                };
                picker.Value = mDateTime;
                picker.setDate();
            }
        }

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(DateTimePicker), new PropertyMetadata(""));



        public Brush IconColor
        {
            get { return (Brush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register("IconColor", typeof(Brush), typeof(DateTimePicker), new PropertyMetadata(Brushes.DarkGray));




        public Visibility IconVisibility
        {
            get { return (Visibility)GetValue(IconVisibilityProperty); }
            set { SetValue(IconVisibilityProperty, value); }
        }

        public static readonly DependencyProperty IconVisibilityProperty =
            DependencyProperty.Register("IconVisibility", typeof(Visibility), typeof(DateTimePicker), new FrameworkPropertyMetadata(Visibility.Visible,new PropertyChangedCallback(OnIconVisbilityChanged)));

        public static void OnIconVisbilityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) {
            DateTimePicker picker = (DateTimePicker)sender;
            picker.IconVisibility = (Visibility)args.NewValue;
        }

        public int Hour
        {
            get { return (int)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }

        public static readonly DependencyProperty HourProperty =
            DependencyProperty.Register("Hour", typeof(int), typeof(DateTimePicker), new FrameworkPropertyMetadata(DateTime.Now.Hour, new PropertyChangedCallback(OnHourChanged)));



        public int Minute
        {
            get { return (int)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); }
        }

        public static readonly DependencyProperty MinuteProperty =
            DependencyProperty.Register("Minute", typeof(int), typeof(DateTimePicker), new FrameworkPropertyMetadata(DateTime.Now.Minute, new PropertyChangedCallback(OnMinuteChanged)));



        public int Second
        {
            get { return (int)GetValue(SecondProperty); }
            set { SetValue(SecondProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Second.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondProperty =
            DependencyProperty.Register("Second", typeof(int), typeof(DateTimePicker), new FrameworkPropertyMetadata(DateTime.Now.Second, new PropertyChangedCallback(OnSecondChanged)));



        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<String>), typeof(DateTimePicker));


        public event RoutedPropertyChangedEventHandler<String> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        private void OnValueChanged(String old, String newStr)
        {
            RoutedPropertyChangedEventArgs<String> args = new RoutedPropertyChangedEventArgs<string>(old, newStr);
            args.RoutedEvent = DateTimePicker.ValueChangedEvent;
            RaiseEvent(args);
        }

        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DateTimePicker picker = (DateTimePicker)sender;
            MDateTime old = (MDateTime)args.OldValue;
            MDateTime newValue = (MDateTime)args.NewValue;
            MDateTime mDate = picker.Value;

            picker.Hour = newValue.Hour;
            picker.Minute = newValue.Minute;
            picker.Second = newValue.Second;

            picker.StringValue = MDateTimeTostring(newValue);
            picker.OnValueChanged(old.ToString(), newValue.ToString());
        }
        private static void OnHourChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DateTimePicker picker = (DateTimePicker)sender;
            MDateTime mDate = picker.Value;
            mDate.Hour = (int)args.NewValue;
            picker.Value = mDate;
        }
        private static void OnMinuteChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DateTimePicker picker = (DateTimePicker)sender;
            MDateTime mDate = picker.Value;
            mDate.Minute = (int)args.NewValue;
            picker.Value = mDate;
        }
        private static void OnSecondChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DateTimePicker picker = (DateTimePicker)sender;
            MDateTime mDate = picker.Value;
            mDate.Second = (int)args.NewValue;
            picker.Value = mDate;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.MainContentTb.Text = MDateTimeTostring(this.Value);
            this.HourTb.Text = Hour.ToString();
            this.MinuteTb.Text = Minute.ToString();
            this.SecondTb.Text = Second.ToString();
        }

        public class MDateTime
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int Day { get; set; }
            public int Hour { get; set; }
            public int Minute { get; set; }
            public int Second { get; set; }

        }

        public void setDate() {
            this.MainContentTb.Text = MDateTimeTostring(this.Value);
        }

        public static string MDateTimeTostring(MDateTime mDateTime)
        {
            return mDateTime.Year + "-" + (mDateTime.Month < 10 ? "0" : "") + mDateTime.Month + "-" + (mDateTime.Day < 10 ? "0" : "") + mDateTime.Day + " " + (mDateTime.Hour < 10 ? "0" : "") + mDateTime.Hour + ":" + (mDateTime.Minute < 10 ? "0" : "") + mDateTime.Minute + ":" + (mDateTime.Second < 10 ? "0" : "") + mDateTime.Second;
        }

        private void DateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dt = (DateTime)DateCalendar.SelectedDate;
            if (dt != null)
            {
                this.Value.Year = dt.Year;
                this.Value.Month = dt.Month;
                this.Value.Day = dt.Day;
            }
            this.MainContentTb.Text = MDateTimeTostring(this.Value);
        }

        private void HourTb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void HourTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                string tst = HourTb.Text;
                if (tst.Length > 0)
                {
                    int a = 0;
                    a = Convert.ToInt32(this.HourTb.Text);
                    Hour = a > 23 ? 23 : a;
                }
                this.MainContentTb.Text = MDateTimeTostring(this.Value);
            }
        }

        private void MinuteTb_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.MinuteP.IsOpen = true;
        }

        private void MinuteTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                string tst = MinuteTb.Text;
                if (tst.Length > 0)
                {
                    int a = 0;
                    a = Convert.ToInt32(this.MinuteTb.Text);
                    Minute = a > 59 ? 59 : a;
                }
                this.MainContentTb.Text = MDateTimeTostring(this.Value);
            }
        }

        private void SecondTb_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.SecondP.IsOpen = true;
        }

        private void SecondTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                string tst = SecondTb.Text;
                if (tst.Length > 0)
                {
                    int a = 0;
                    a = Convert.ToInt32(this.SecondTb.Text);
                    Second = a > 59 ? 59 : a;
                }
                this.MainContentTb.Text = MDateTimeTostring(this.Value);
            }
        }


        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            this.SelectP.IsOpen = false;
        }

        private void MainContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime dt;
            DateTime.TryParse(this.MainContentTb.Text, out dt);
            if (dt != null)
            {
                MDateTime mDateTime = new MDateTime()
                {
                    Year = dt.Year,
                    Month = dt.Month,
                    Day = dt.Day,
                    Hour = dt.Hour,
                    Minute = dt.Minute,
                    Second = dt.Second
                };
                this.Value = mDateTime;
            }
        }

        private void HourTb_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.HourP.IsOpen = true;
        }
        private void MainContent_GotFocus(object sender, RoutedEventArgs e)
        {
            this.SelectP.IsOpen = true;
        }

        private void IconTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.SelectP.IsOpen = true;
        }

        private void Hour_TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb= (TextBlock)sender;
            this.HourTb.Text = tb.Text;
            this.HourP.IsOpen = false;
        }

        private void clearHourBtn_Click(object sender, RoutedEventArgs e)
        {
            this.HourP.IsOpen = false;
        }

        private void clearMinuteBtn_Click(object sender, RoutedEventArgs e)
        {
            this.MinuteP.IsOpen = false;
        }

        private void minute_TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            this.MinuteTb.Text = tb.Text;
            this.MinuteP.IsOpen = false;
        }

        private void clearSecondBtn_Click(object sender, RoutedEventArgs e)
        {
            this.SecondP.IsOpen = false;
        }

        private void second_TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            this.SecondTb.Text = tb.Text;
            this.SecondP.IsOpen = false;
        }
    }
}
