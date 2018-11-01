using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaWeiBase
{
    public class DataPattern : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string dbFielName;
        public string DBFieldName
        {
            get { return dbFielName; }
            set 
            {
                dbFielName = value;
                OnPropertyChanged("DBFieldName");
            }
        }

        private double maximum;
        public double Maximum
        {
            get { return maximum; }
            set 
            {
                maximum = value;
                OnPropertyChanged("Maximum");
            }
        }

        private double minimum;
        public double Minimum
        {
            get { return minimum; }
            set 
            {
                minimum = value;
                OnPropertyChanged("Minimum");
            }
        }

        private double upper;
        public double Upper
        {
            get { return upper; }
            set 
            {
                upper = value;
                OnPropertyChanged("Upper");
            }
        }

        private double lower;
        public double Lower
        {
            get { return lower; }
            set 
            { 
                lower = value;
                OnPropertyChanged("Lower");
            }
        }

        private string suffix;
        public string Suffix
        {
            get { return suffix; }
            set 
            {
                suffix = value;
                OnPropertyChanged("Suffix");
            }
        }

        private double interval;
        public double Interval
        {
            get { return interval; }
            set 
            {
                interval = value;
                OnPropertyChanged("Interval");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
