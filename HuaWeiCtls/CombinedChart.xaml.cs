using HuaWeiBase;
using LiveCharts;
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
    /// CombinedChart.xaml 的交互逻辑
    /// </summary>
    public partial class CombinedChart : UserControl, INotifyPropertyChanged
    {
        public List<string> Labels { get; set; }

        public Func<double, string> YFormatter { get; set; }

        public ChartValues<double> UpperValues { get; set; }

        public ChartValues<double> DataValues { get; set; }

        public ChartValues<double> LowerValues { get; set; }

        public ChartValues<double> CPKValues { get; set; }

        private string _xtitle = string.Empty;
        public string XTitle
        {
            get { return _xtitle; }
            set { _xtitle = value; }
        }

        private string _ytitle = string.Empty;
        public string YTitle
        {
            get { return _ytitle; }
            set { _ytitle = value; }
        }

        private double _upper = 10.0;
        public double Upper
        {
            get { return _upper; }
            set { _upper = value; }
        }

        private double _lower = 0.0;
        public double Lower
        {
            get { return _lower; }
            set { _lower = value; }
        }

        /// <summary>
        /// 当前数据点的个数
        /// </summary>
        private int Count = 0;

        /// <summary>
        /// 最多显示的点数
        /// </summary>
        private int _maximumCount = 51;
        public int MaximumCount
        {
            get { return _maximumCount; }
            set { _maximumCount = value; }
        }

        /// <summary>
        /// 用于计算CPK的数据点的个数
        /// </summary>
        private int _CPKCount = 200;
        public int CPKCount
        {
            get { return _CPKCount; }
            set
            {
                if (value > 50 && value <= 200)
                {
                    _CPKCount = value;
                }
            }
        }

        /// <summary>
        /// 缓存用于计算CPK的数据点集合
        /// </summary>
        private List<double> Values = new List<double>();

        private double _cpk;
        public double CPK
        {
            get { return _cpk; }
            set
            {
                if (_cpk != value)
                {
                    _cpk = value;
                    OnPropertyChanged("CPK");
                }
            }
        }

        #region 定义DependencyProperty

        public Brush DataBrush
        {
            get { return (Brush)GetValue(DataBrushProperty); }
            set { SetValue(DataBrushProperty, value); }
        }
        public static readonly DependencyProperty DataBrushProperty =
            DependencyProperty.Register("DataBrush", typeof(Brush), typeof(CombinedChart),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x40, 0x80, 0x00))));


        public Brush CPKBrush
        {
            get { return (Brush)GetValue(CPKBrushProperty); }
            set { SetValue(CPKBrushProperty, value); }
        }
        public static readonly DependencyProperty CPKBrushProperty =
            DependencyProperty.Register("CPKBrush", typeof(Brush), typeof(CombinedChart),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0xEF, 0xB8, 0x00))));


        public Brush UpperBrush
        {
            get { return (Brush)GetValue(UpperBrushProperty); }
            set { SetValue(UpperBrushProperty, value); }
        }
        public static readonly DependencyProperty UpperBrushProperty =
            DependencyProperty.Register("UpperBrush", typeof(Brush),
            typeof(CombinedChart), new PropertyMetadata(Brushes.Red));


        public Brush LowerBrush
        {
            get { return (Brush)GetValue(LowerBrushProperty); }
            set { SetValue(LowerBrushProperty, value); }
        }
        public static readonly DependencyProperty LowerBrushProperty =
            DependencyProperty.Register("LowerBrush", typeof(Brush), typeof(CombinedChart),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0xFF))));

        #endregion

        public CombinedChart()
        {
            InitializeComponent();

            Labels = new List<string>();
            YFormatter = value => value + " ";

            UpperValues = new ChartValues<double>();
            DataValues = new ChartValues<double>();
            LowerValues = new ChartValues<double>();
            CPKValues = new ChartValues<double>();

            this.Loaded += CombinedChart_Loaded;
        }

        private void CombinedChart_Loaded(object sender, RoutedEventArgs e)
        {
            cpkChart.Height = rootGrid.RenderSize.Height * 1.0 / 3.0;

            DataContext = this;
        }

        /// <summary>
        /// 添加数据点
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="value"></param>
        public void AddPoint(DateTime dateTime, double value)
        {
            if (++Count > _maximumCount)
            {
                Count = _maximumCount;

                Labels.RemoveAt(0);
                UpperValues.RemoveAt(0);
                LowerValues.RemoveAt(0);
                DataValues.RemoveAt(0);
                CPKValues.RemoveAt(0);
            }

            Labels.Add(dateTime.ToString(GlobalConstants.TimeFormat));
            tbOdValue.Text = value.ToString(GlobalConstants.DoubleFormat);
            UpperValues.Add(_upper);
            LowerValues.Add(_lower);
            DataValues.Add(double.Parse(value.ToString(GlobalConstants.DoubleFormat)));

            if (Values.Count >= CPKCount)
            {
                Values.RemoveAt(0);
            }
            Values.Add(value);
            CPK = CalculateCPK();
            tbCpkValue.Text = _cpk.ToString(GlobalConstants.DoubleFormat);
            CPKValues.Add(Math.Round(_cpk, 3));
        }

        private double cpkSum = 0.0;
        /// <summary>
        /// 计算最近点的CPK值
        /// </summary>
        /// <returns></returns>
        private double CalculateCPK()
        {
            // Cpk = (1 - K) * Cp
            //     = (1 - 2|M-μ|/T)*T/6σ
            //     = (T - 2|M-μ|) / 6σ
            // K = |M-μ| / (T / 2)
            // Cp = (USL - LSL) / 6σ            过程能力指数
            // M = (USL + LSL) / 2               目标值
            // μ = (x1+x2+...+xn) / n :         过程平均值
            // T = USL - LSL
            // USL (Upper specification limit):  规格上限
            // LSL (Low specification limit):    规格下限
            // σ                                标准差

            double difference;

            // 计算总和
            cpkSum += Values.Last();

            // 数据的平均值
            int yCount = Values.Count;
            double dataAverage = cpkSum / yCount;

            if (yCount >= CPKCount)
                cpkSum -= Values.First();

            // 计算标准差
            double stdDeviation = 0.0;
            foreach (double value in Values)
            {
                difference = value - dataAverage;
                stdDeviation += difference * difference;
            }

            if (stdDeviation <= 0)
            {
                if (dataAverage > Upper || dataAverage < Lower) { return 0.0; }
                else { return 1.67; }
            }

            stdDeviation = Math.Sqrt(stdDeviation / yCount);

            // 计算Cpk值
            double offset = Math.Abs((Upper + Lower) / 2 - dataAverage);
            double cpk = (Upper - Lower - 2 * offset) / (6 * stdDeviation);

            if (cpk < 0) { cpk = 0.0; }
            return cpk > 1.67 ? 1.67 : cpk;
        }

        /// <summary>
        /// 添加数据点集合
        /// </summary>
        /// <param name="points"></param>
        public void AddPoints(List<KeyValuePair<DateTime, double>> points)
        {
            if (points != null && points.Count > 0)
            {
                foreach (var point in points)
                {
                    AddPoint(point.Key, point.Value);
                }
            }
        }

        /// <summary>
        /// 重新开始计算CPK
        /// </summary>
        public void RestartCalculateCPK()
        {
            cpkSum = 0.0;
            Values.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
