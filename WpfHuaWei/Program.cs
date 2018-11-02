using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace WpfHuaWei
{
    public class Program
    {
        private static System.Threading.Mutex mutex;

        private static App application = null;

        [STAThread]
        public static void Main(string[] args)
        {

            if (args.Length == 0 || !"CrashRestart".Equals(args[0]))
            {
                if (IsApplicationExists())
                {
                    StreamWriter streamWriter = null;
                    NamedPipeClientStream pipeClient = null;
                    try
                    {
                        pipeClient = new NamedPipeClientStream(".",
                            "Panyu.Cable.DZKB", PipeDirection.Out,
                            PipeOptions.None, TokenImpersonationLevel.Impersonation);
                        streamWriter = new StreamWriter(pipeClient);
                        pipeClient.Connect();
                        if (pipeClient.IsConnected)
                        {
                            streamWriter.WriteLine("MaxWindow");
                            streamWriter.Flush();
                        }
                    }
                    catch { }
                    finally
                    {
                        if (streamWriter != null)
                        {
                            try
                            {
                                streamWriter.Close();
                                pipeClient.Close();
                            }
                            catch { }
                        }
                    }
                    return; // 如果存在一个实例，且不是崩溃启动的实例，则直接返回
                }
            }

            // 开启一个命名管道，监测发送的信息
            Thread receiveDataThread =
                new Thread(new ThreadStart(ReceiveDataFromNamedPipe));
            receiveDataThread.IsBackground = true;
            receiveDataThread.Start();

            application = new App();
            application.InitializeComponent();
            application.Run();
        }

        /// <summary>  
        /// 使用互斥量判断程序是否已经运行。  
        /// </summary>  
        /// <returns>已经运行返回true，否则返回false。</returns>  
        internal static bool IsApplicationExists()
        {
            bool creatNew;
            mutex = new Mutex(true, "Panyu-Cable-DZKB", out creatNew);
            if (creatNew)
            {
                GC.KeepAlive(mutex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 开启一个接收数据的命名管道
        /// </summary>
        private static void ReceiveDataFromNamedPipe()
        {
            while (true)
            {
                StreamReader streamReader = null;
                NamedPipeServerStream pipeServer = null;
                try
                {
                    pipeServer = new NamedPipeServerStream(
                        "Panyu.Cable.DZKB", PipeDirection.In, 2);
                    streamReader = new StreamReader(pipeServer);
                    while (true)
                    {
                        pipeServer.WaitForConnection();
                        while (pipeServer.IsConnected)
                        {
                            if (!streamReader.EndOfStream)
                            {
                                string receivedData = streamReader.ReadLine();
                                if ("Exit".Equals(receivedData))
                                {
                                    System.Environment.Exit(0);
                                }
                                else if ("MaxWindow".Equals(receivedData))
                                {
                                    application.Dispatcher.Invoke(new Action(() =>
                                    {
                                        ((MainWindow)application.MainWindow).
                                            WindowState = System.Windows.WindowState.Normal;
                                    }));
                                }
                            }
                            Thread.Sleep(200);
                        }
                        Thread.Sleep(200);
                    }
                }
                catch { }
                finally
                {
                    if (streamReader != null)
                    {
                        try
                        {
                            streamReader.Close();
                            pipeServer.Close();
                        }
                        catch { }
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
