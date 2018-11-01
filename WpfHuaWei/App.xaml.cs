using HuaWeiUtils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfHuaWei.Utils;

namespace WpfHuaWei
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Trace.Listeners.Add(new ExListener());

            AppDomain.CurrentDomain.UnhandledException
                += CurrentDomain_UnhandledException;

            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void CurrentDomain_UnhandledException(
            object sender, UnhandledExceptionEventArgs e)
        {
            Trace.WriteLine(e.ExceptionObject);
            SaveCurrentStateAndRestartApplication();
        }

        private void App_DispatcherUnhandledException(
            object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Trace.WriteLine(e.Exception.ToString());
            SaveCurrentStateAndRestartApplication();
        }

        /// <summary>
        /// 保存应用程序状态并重启应用程序
        /// </summary>
        private void SaveCurrentStateAndRestartApplication()
        {
            // 保存机台状态
            MainWindow mw = Application.Current.MainWindow as MainWindow;
            ApplicationStateManager.SaveMachineState(mw.BaseMachine);

            // 重启一个应用程序实例
            string moduleName = Process.GetCurrentProcess().MainModule.ModuleName;
            string appPath = System.IO.Directory.GetCurrentDirectory() + "/" + moduleName;
            if (File.Exists(appPath))
                System.Diagnostics.Process.Start(appPath, "CrashRestart");

            System.Environment.Exit(0); // 当前应用程序实例退出
        }
    }

    class ExListener : TraceListener
    {
        public static readonly string ExceptionLogFilePath = "ex.log";

        public override void Write(string message)
        {
            try
            {
                File.AppendAllText(ExceptionLogFilePath,
                    DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]")
                    + Environment.NewLine + message);
            }
            catch { }
        }

        public override void WriteLine(string message)
        {
            try
            {
                File.AppendAllText(ExceptionLogFilePath,
                    DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]")
                    + Environment.NewLine + message + Environment.NewLine);
            }
            catch { }
        }
    }
}
