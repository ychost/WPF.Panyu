using AppResorces;
using HuaWeiBase;
using HuaWeiDAL;
using HuaWeiModel;
using HuaWeiUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using HuaWeiCtls;
using WpfHuaWei.DataService;
using WpfHuaWei.DeviceView;
using WpfHuaWei.SettingView;
using YCsharp.Util;

namespace WpfHuaWei {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        /// <summary>
        /// 应用程序任务栏托盘通知图标
        /// </summary>
        private System.Windows.Forms.NotifyIcon notifyIcon;

        /// <summary>
        /// 当前显示的机台界面
        /// </summary>
        public BaseMachine BaseMachine { get; set; }

        /// <summary>
        /// 网络异常时，一个周期性检查网络的定时器
        /// </summary>
        private System.Timers.Timer networkCheckTimer;

        public MainWindow() {
            InitializeComponent();

            // 初始化主窗口
            InitializeMainWindow();

            // 设置通知栏图标
            InitailizeNotifyIcon();
            try {
                // 开发电脑上面就不用自启了
                // 设置开机自启
                if (!YUtil.GetWindowsUserName().ToUpper().Contains("SOMIAR")) {
                    YUtil.SetAppAutoStart("PanyuVisualSystem", true);
                } else {
                    UIMessageBox.Show("提示", "开发电脑无需自启", 5000);
                }
            } catch {
                UIMessageBox.Show("异常", "设置启动异常", 5000);
                // ignored
            }
            try {
                // 番禺的防火墙服务默认是禁用
                // 这样就会封掉所有端口，所以只能打开防火墙服务，然后关闭防火墙才行
                // 当然防火墙的状态是设置的为关闭
                YUtil.SetWinServiceStartupType("MpsSvc", ServiceStartMode.Automatic);
                YUtil.StartWinService("MpsSvc");
            } catch {
                // ignored
            }
            // 检查网络连接
            if (OnlineDataCenter.IsNetwork2ServerOK()) {
                InitAndSetupMachineWindow();
                try {
                    // 同步服务器时间
                    var time = YUtil.GetNtpTime(Configuration.ServerIp);
                    if (Math.Abs((DateTime.Now - time).TotalSeconds) > 10) {
                        YUtil.SetLoadTimeByDateTime(time);
                    }
                } catch {
                    // ignored
                }
            } else {
                Task.Run(() => {
                    SustainedNetworkTest(null);
                });
            }
        }
        /// <summary>
        /// 配置并启动机台界面
        /// </summary>
        private void InitAndSetupMachineWindow() {
            // 初始化应用程序配置数据
            if (InitMachineEmployeeAndParameterCode()) {
                // 创建默认的机台页面
                InitializeDefaultMachineView();

                OnlineDataCenter.StartUseMqAsync();

                // 开启数据接收服务
                //                OnlineDataCenter.StartService();

                // 显示界面并开启接收数据
                pageContainer.Visibility = Visibility.Visible;
                checkNetGrid.Visibility = Visibility.Collapsed;

                // 开启机台接收数据
                BaseMachine.StartLoadingData();
            } else {
                System.Windows.MessageBox.Show("从服务器中获取初始化数据"
                    + "失败，\r\n请检查网络连接是否稳定，然后重新启动！",
                    "出错提示", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(0);
            }
        }

        /// <summary>
        /// 持续地测试与服务器的网络连接
        /// </summary>
        /// <param name="state"></param>
        private void SustainedNetworkTest(object state) {
            int tryTime = 0;
            while (!OnlineDataCenter.IsNetwork2ServerOK()) {
                if (++tryTime > 10) {
                    networkCheckTimer = new System.Timers.Timer();
                    networkCheckTimer.Interval = 3000;
                    networkCheckTimer.Elapsed += networkCheckTimer_Elapsed;
                    networkCheckTimer.Start();

                    this.Dispatcher.Invoke(new Action(() => {
                        spCheckNet.Visibility = Visibility.Hidden;
                        spNetTips.Visibility = Visibility.Visible;
                    }));

                    return;
                }
                Thread.Sleep(2000);
            }
            Dispatcher.Invoke(InitAndSetupMachineWindow);
        }

        /// <summary>
        /// 周期性检查网络
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void networkCheckTimer_Elapsed(object sender, EventArgs e) {
            if (OnlineDataCenter.IsNetwork2ServerOK()) {
                networkCheckTimer.Stop();
                Dispatcher.Invoke(new Action(() => {
                    InitAndSetupMachineWindow();
                }));
            }
        }

        /// <summary>
        /// 初始化界面并加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitializeMainWindow() {
            // 设置窗口标题
            this.Title = Configuration.Title;
            tbSubTitle.Text = Configuration.SubTitle;

            // 设置图标
            this.Icon = ResourcesUtils.GetImageSource("PanyuLogo.png");

            // 为设置按钮添加事件处理函数
            this.captionButtons.appSetButton.Click += appSetButton_Click;

            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        }

        /// <summary>
        /// 初始化程序任务栏托盘通知图标
        /// </summary>
        private void InitailizeNotifyIcon() {
            this.notifyIcon = new System.Windows.Forms.NotifyIcon();
            // 设置程序启动时显示的文本
            this.notifyIcon.BalloonTipText = "广州番禺电缆可视化监测系统";
            // 最小化到托盘时，鼠标点击时显示的文本
            this.notifyIcon.Text = "广州番禺电缆可视化监测系统";
            // 静应用程序图标设置为托盘通知的图标
            this.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(
                System.Windows.Forms.Application.ExecutablePath);
            this.notifyIcon.Visible = false;
            //打开菜单项
            MenuItem open = new System.Windows.Forms.MenuItem("打开主界面",
                new EventHandler((o, e) => {
                    this.Show();
                    this.notifyIcon.Visible = false;
                }));
            //退出菜单项
            MenuItem exit = new System.Windows.Forms.MenuItem("退出程序",
                new EventHandler((o, e) => {
                    System.Windows.Application.Current.Shutdown(0);
                }));
            //关联托盘控件
            MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);
            notifyIcon.MouseDoubleClick += new MouseEventHandler((o, e) => {
                if (e.Button == MouseButtons.Left) {
                    this.Show();
                    this.notifyIcon.Visible = false;
                }
            });
            this.notifyIcon.ShowBalloonTip(1000);
        }

        /// <summary>
        /// 初始化机台相关数据
        /// </summary>
        private bool InitMachineEmployeeAndParameterCode() {
            // 获取并存储所有机台信息
            List<Machine> machines = MachineDAL.SelectAll();
            if (machines != null && machines.Count > 0) {
                foreach (Machine machine in machines)
                    Configuration.MachineMap[machine.MachineName] = machine;
            } else { return false; }

            // 获取并存储所有员工信息
            List<Employee> employees = EmployeeDAL.SelectAll();
            if (employees != null && employees.Count > 0) {
                foreach (Employee employee in employees)
                    Configuration.EmployeeMap[employee.EmployeeCode] = employee;
            } else { return false; }

            // 获取并存储所有参数信息
            List<ParameterCode> parameterCodes = ParameterCodeDAL.SelectAll();
            if (parameterCodes != null && parameterCodes.Count > 0) {
                Configuration.ParameterCodeArray
                    = new string[parameterCodes.Last().ParameterTypeId + 1];
                foreach (ParameterCode paramCode in parameterCodes)
                    Configuration.ParameterCodeArray[
                        paramCode.ParameterTypeId] = paramCode.ParameterName;
            } else { return false; }

            return true;
        }

        /// <summary>
        /// 重写OnClosing来实现隐藏到任务栏
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            base.OnClosing(e);

            if ("false".Equals(XmlUtil.XmlGetElementText(XmlUtil.AppConfig,
                @"/Application/CloseMainwindowToExit").ToLower())) {
                this.Hide();
                this.notifyIcon.Visible = true;

                e.Cancel = true;
            }
        }

        /// <summary>
        /// 创建默认的机台页面
        /// </summary>
        private void InitializeDefaultMachineView() {
            // 创建默认的机台页面
            if (!Configuration.MachineMap.ContainsKey(Configuration.DefaultMachine)
                || (BaseMachine = GetMachineViewByTypeId(Configuration.MachineMap[
                Configuration.DefaultMachine].MachineTypeID)) == null) {
                System.Windows.MessageBox.Show("配置文件的默认机台信息有误！",
                    "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(0);
            }
            BaseMachine.MachineName = Configuration.DefaultMachine;
            pageContainer.Children.Insert(0, BaseMachine); //将机台页面添加到主界面
        }

        /// <summary>
        /// 根据机台类型生成对应的机台对象
        /// </summary>
        /// <param name="machineTypeId"></param>
        /// <returns></returns>
        private BaseMachine GetMachineViewByTypeId(int machineTypeId) {
            if (machineTypeId == 2)
                return new CurrentJiJueYuan();

            if (machineTypeId == 5)
                return new CurrentJiHutao();

            return null;
        }

        private void appSetButton_Click(object sender, RoutedEventArgs e) {
            this.popupSettings.IsOpen = true;
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e) {
            this.popupSettings.IsOpen = false;

            FrameworkElement element = sender as FrameworkElement;
            switch (element.Name) {
                case "btnSettings":
                    AppSettings appSettingsDlg = new AppSettings();
                    appSettingsDlg.Owner = this;
                    appSettingsDlg.ShowDialog();
                    break;
                case "btnAbout":
                    About aboutDlg = new About();
                    aboutDlg.Owner = this;
                    aboutDlg.ShowDialog();
                    break;
                case "btnExit":
                    System.Environment.Exit(0);
                    break;
                default: break;
            }
        }
    }
}
