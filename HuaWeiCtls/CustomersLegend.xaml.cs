using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HuaWeiCtls
{
    /// <summary>
    /// CustomersLegend.xaml 的交互逻辑
    /// </summary>
    public partial class CustomersLegend : UserControl, IChartLegend, INotifyPropertyChanged
    {
        private List<SeriesViewModel> _series;

        public CustomersLegend()
        {
            InitializeComponent();

            DataContext = this;
        }

        public List<SeriesViewModel> Series
        {
            get { return _series; }
            set
            {
                _series = value;
                if (_series != null)
                {
                    int idx = 0;
                    while (idx < _series.Count)
                    {
                        if (string.IsNullOrEmpty(_series[idx].Title))
                            _series.RemoveAt(idx);
                        else idx++;
                    }
                }
                OnPropertyChanged("Series");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
