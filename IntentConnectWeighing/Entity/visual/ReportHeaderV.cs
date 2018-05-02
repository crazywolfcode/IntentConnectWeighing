using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{
    public class ReportHeaderV : INotifyPropertyChanged
    {

        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
            }
        }
        private string subtitle;
        public string SubTitle
        {
            get
            {
                return subtitle;
            }
            set
            {
                subtitle = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SubTitle"));
            }
        }
        private string datearea;
        public string DateArea
        {
            get
            {
                return datearea;
            }
            set
            {
                datearea = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DateArea"));
                }
            }
        }
        private string yardName;
        public string YardName
        {
            get
            {
                return yardName;
            }
            set
            {
                yardName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("YardName"));
            }
        }
        private string printTime;
        public string PrintTime
        {
            get { return printTime; }
            set
            {
                printTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PrintTime"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
