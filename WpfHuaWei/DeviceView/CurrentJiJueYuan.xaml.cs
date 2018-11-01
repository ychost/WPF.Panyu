using AppResorces;
using HuaWeiBase;
using HuaWeiDAL;
using HuaWeiModel;
using HuaWeiUtils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WpfHuaWei.DataService;
using WpfHuaWei.Utils;

namespace WpfHuaWei.DeviceView
{
    /// <summary>
    /// CurrentJiJueYuan.xaml 的交互逻辑
    /// </summary>
    public partial class CurrentJiJueYuan : BaseMachine
    {
        /// <summary>
        /// 用于更新界面上的当前时间
        /// </summary>
        private DispatcherTimer timer = new DispatcherTimer();

        /// <summary>
        /// 记录上一次条形码输入框文本变化的时间
        /// </summary>
        private DateTime lastQrcodeTextChangedTime = DateTime.MinValue;

        public CurrentJiJueYuan()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromMilliseconds(995);
            timer.Tick += timer_Tick;

            this.Loaded += CurrentJiJueYuan_Loaded;
        }

        private void CurrentJiJueYuan_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Start();

            // Issue:如果设置pdfReader可见，初始化后diameterChart就看不见
            pdfReader.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// 更新界面的当前时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            int second = DateTime.Now.Second;
            if (second == 0) { SwitchToMainContentView(); }
            if (second == 50) { SwitchToSubContentView(); }

            tbQrcode.Focus();

            double interval = (now - lastQrcodeTextChangedTime).TotalSeconds;
            if (interval > 0.3 && interval < 1.3)
                this.tbQrcode.Text = string.Empty;

            tbCurrentTime.Text = now.ToString(GlobalConstants.DateTimeFormat);
        }

        public override void StartLoadingData(string machineName)
        {
            MachineName = machineName;
            StartLoadingData();
        }

        /// <summary>
        /// 实时数据
        /// </summary>
        /// <param name="machineInfo"></param>
        public override void StartLoadingData()
        {
            ApplicationStateManager.RecoveryMachineState(this);
            ApplicationStateManager.ClearApplicationStatePath();

            if (ProductionInfo == null)
            {
                ProductionInfo =
                    OnlineDataCenter.GetMachineProductionInfo(MachineName);
                if (ProductionInfo == null)
                {
                    MessageBox.Show("配置的机台名称与数据库中的不一致！",
                        "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    System.Environment.Exit(0);
                }
            }
            else OnlineDataCenter.SetMachineProductionInfo(ProductionInfo);

            odChart.Upper = Configuration.OuterDiameterUpper;
            odChart.Lower = Configuration.OuterDiameterLower;

            // 绑定数据源对象
            rootGrid.DataContext = ProductionInfo;

            ProductionInfo.PdfService.ChangeProductionProcessPdf
                += PdfService_ChangeProductionProcessPdf;
            ProductionInfo.DoublePropertyChanged += cpi_DoublePropertyChanged;

            AppEventsManager.OnUpdateOuterDiameterUpperAndLower +=
                AppEventsManager_OnUpdateOuterDiameterUpperAndLower;

            // 重新加载工艺规格文件
            if (!string.IsNullOrEmpty(WorksheetNo))
                ThreadPool.QueueUserWorkItem(QueryAndGetProcessPdf, WorksheetNo);
        }

        /// <summary>
        /// 更改显示的生产工艺Pdf文件
        /// </summary>
        /// <param name="filePath"></param>
        private void PdfService_ChangeProductionProcessPdf(string filePath)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.pdfReader.OpenPdfFile(filePath);
            }));
        }

        private void AppEventsManager_OnUpdateOuterDiameterUpperAndLower()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                odChart.Upper = Configuration.OuterDiameterUpper;
                odChart.Lower = Configuration.OuterDiameterLower;
                odChart.RestartCalculateCPK();
            }));
        }

        /// <summary>
        /// 处理double类型参数的变化
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="dateTime"></param>
        /// <param name="value"></param>
        private void cpi_DoublePropertyChanged(int pid, DateTime dateTime, double value)
        {
            switch (pid)
            {
                case 2: // Od值
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        odChart.AddPoint(dateTime, value);
                        diameterChart.SetOuterDiameter(value);
                    }));
                    if (value > Configuration.OuterDiameterUpper
                        || value < Configuration.OuterDiameterLower)
                    {
                        Outlier outlier = new Outlier();
                        outlier.MachineName = MachineName;
                        outlier.ParamName = "外径值";
                        outlier.Value = value;
                        outlier.Upper = Configuration.OuterDiameterUpper;
                        outlier.Lower = Configuration.OuterDiameterLower;
                        outlier.CollectedTime = dateTime;
                        OutlierDAL.Add(outlier);
                    }
                    break;

                #region 放线张力、十段温度、主机速度、线速度、火花值
                case 9: // 放线张力

                    break;
                case 14: // 一段温度实测值

                    break;
                case 16: // 二段温度实测值

                    break;
                case 18: // 三段温度实测值

                    break;
                case 20: // 四段温度实测值

                    break;
                case 22: // 五段温度实测值

                    break;
                case 24: // 六段温度实测值

                    break;
                case 26: // 颈部温度实测值

                    break;
                case 28: // 机头温度实测值

                    break;
                case 30: // 眼模温度实测值

                    break;
                case 32: // 水槽温度实测值

                    break;
                case 35: // 主机电机转速

                    break;
                case 38: // 线速度

                    break;
                case 40: // 火花检出
                    Outlier outlier2 = new Outlier();
                    outlier2.MachineName = MachineName;
                    outlier2.ParamName = "火花值";
                    outlier2.Value = value;
                    outlier2.Upper = 0;
                    outlier2.Lower = 0;
                    outlier2.CollectedTime = dateTime;
                    OutlierDAL.Add(outlier2);
                    break;

                #endregion

                case 42: // 导体直径
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        diameterChart.SetInnerDiameter(value);
                    }));
                    break;
            }
        }

        /// <summary>
        /// 获取扫描的条形码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbQrcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            lastQrcodeTextChangedTime = DateTime.Now;

            // 获取工单号
            string worksheetNo = tbQrcode.Text;
            if (!string.IsNullOrEmpty(worksheetNo) &&
                !string.IsNullOrEmpty(worksheetNo.Trim()))
            {
                ThreadPool.QueueUserWorkItem(QueryAndGetProcessPdf, worksheetNo.Trim());
            }
            e.Handled = true;
        }

        /// <summary>
        /// 查询并获取新的生产工艺规格文件
        /// </summary>
        /// <param name="state"></param>
        private void QueryAndGetProcessPdf(object state)
        {
            string worksheetNo = (string)state;

            ProductPlan productPlan =
                ProductPlanDAL.GetProductPlan("where worksheetno='" + worksheetNo + "'");
            if (productPlan == null) { return; }

            Crafwork crafwork = CrafworkDAL.GetCrafwork("where id=" + productPlan.Mid);
            if (crafwork == null) { return; }

            WorksheetNo = worksheetNo;
            this.Dispatcher.Invoke(new Action(() =>
            {
                tbQrcode.Text = string.Empty;
                tbWroksheetNo.Text = productPlan.WorkSheetNo;
                tbCrafworkCode.Text = crafwork.CrafworkCode;
                tbSpecification.Text = productPlan.Specification;
                tbMaterialNo.Text = productPlan.MaterialNo;
                tbColor.Text = productPlan.Color;
                tbLength.Text = productPlan.Length + "";
                tbBack.Text = productPlan.Bak;
                tbArrLength.Text = productPlan.ArrLength + "";
            }));

            // 获取工艺参数文件
            ProductionInfo.PdfService.PdfFileName = crafwork.FileName;
        }

        /// <summary>
        /// 切换到主页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMainView_Click(object sender, RoutedEventArgs e)
        {
            SwitchToMainContentView();
        }

        private void SwitchToMainContentView()
        {
            this.btnMainView.FontSize = 24;
            this.btnMainView.FontWeight = FontWeights.Bold;
            this.btnSubView.FontSize = 20;
            this.btnSubView.FontWeight = FontWeights.Normal;

            this.pdfReader.Visibility = Visibility.Visible;
            this.subContentGrid.SetValue(Panel.ZIndexProperty, 100);
            this.mainContentGrid.SetValue(Panel.ZIndexProperty, 200);
        }

        /// <summary>
        /// 切换到其他参数页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubView_Click(object sender, RoutedEventArgs e)
        {
            SwitchToSubContentView();
        }

        private void SwitchToSubContentView()
        {
            this.btnSubView.FontSize = 24;
            this.btnSubView.FontWeight = FontWeights.Bold;
            this.btnMainView.FontSize = 20;
            this.btnMainView.FontWeight = FontWeights.Normal;

            this.pdfReader.Visibility = Visibility.Hidden;
            this.mainContentGrid.SetValue(Panel.ZIndexProperty, 100);
            this.subContentGrid.SetValue(Panel.ZIndexProperty, 200);
        }
    }
}
