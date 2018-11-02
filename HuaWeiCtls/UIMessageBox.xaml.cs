using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HuaWeiCtls {
    // <summary>

    /// UMessageBox.xaml 的交互逻辑

    /// </summary>

    public partial class UIMessageBox : Window {

        /// <summary>

        /// 禁止在外部实例化

        /// </summary>

        private UIMessageBox() {
            InitializeComponent();
        }

        public new string Title {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }

        }

        public string Message {
            get { return this.lblMsg.Text; }
            set { this.lblMsg.Text = value; }
        }

        /// <summary>
        /// 静态方法 模拟MESSAGEBOX.Show方法
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static bool? Show(string title, string msg) {
            var msgBox = new UIMessageBox();
            msgBox.Title = title;
            msgBox.Message = msg;
            return msgBox.ShowDialog();
        }

        [STAThread]
        public static void Show(string title, string msg, int autoCloseDelayMs) {

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                int times = (autoCloseDelayMs / 1000);
                if (times <= 0) {
                    times = 1;
                }
                var msgBox = new UIMessageBox();
                msgBox.Title = title;
                msgBox.Message = msg;
                msgBox.Topmost = true;
                msgBox.confirm_btn.Text = $"确定{times}";
                msgBox.Show();
                SetInterval(1000, (i) => {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        new Action(() => {
                            if (i < times) {
                                msgBox.confirm_btn.Text = "确定 (" + (times - i) + ")";
                            } else {
                                msgBox.Close();
                            }
                        }));
                }, times);
            }));
        }

        /// <summary>
        /// 可控制 Interval 的次数
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="action"></param>
        /// <param name="cycleTimes"></param>
        /// <returns></returns>
        public static Timer SetInterval(double interval, Action<int> action, int cycleTimes) {
            Timer timer = new Timer(interval);
            int times = 0;
            timer.Elapsed += (s, e) => {
                if (++times <= cycleTimes) {
                    action(times);
                } else {
                    ClearTimeout(timer);
                }
            };
            RecoveryTimeout(timer);
            return timer;
        }

        /// <summary>
        /// 实现类似于js的setTimetout
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="action"></param>
        public static Timer SetTimeout(double interval, Action action) {
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e) {
                ClearTimeout(timer);
                action();
            };
            RecoveryTimeout(timer);
            return timer;
        }

        /// <summary>
        /// 清除记时
        /// </summary>
        /// <param name="timer"></param>
        public static void ClearTimeout(Timer timer) {
            if (timer != null) {
                timer.Enabled = false;
            }
        }

        /// <summary>
        /// 恢复记时
        /// </summary>
        /// <param name="timer"></param>
        public static void RecoveryTimeout(Timer timer) {
            if (timer != null) {
                timer.Enabled = true;
            }
        }
        private void Yes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            this.DialogResult = true;
            this.Close();
        }

        private void No_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            this.DialogResult = false;
            this.Close();
        }

    }

}
