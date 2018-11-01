using HuaWeiModel;
using HuaWeiUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using DataDisplay;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YCsharp.Model.Rest;
using YCsharp.Service;
using YCsharp.Util;

namespace WpfHuaWei.DataService {
    internal class OnlineDataCenter {
        #region 连接服务端和数据处理用到的变量

        private static Socket clientSocket;

        private static IPAddress serverIP;

        private static int serverPort;

        #endregion

        // 对方法handleRawBytes加锁，主要保证 s2
        private static object s2Lock = new object();

        /// <summary>
        /// 互斥访问访问clientSocket的锁对象
        /// </summary>
        private static object clientSocketLock = new object();

        /// <summary>
        /// 上次接收数据的时间
        /// </summary>
        private static DateTime lastRecieveTime = DateTime.Now.AddSeconds(2);

        /// <summary>
        /// 用于测试与服务器的网络连接
        /// </summary>
        private static Ping ping = new Ping();

        /// <summary>
        /// 序列化工具
        /// </summary>
        private static SerializationUnit seru = new SerializationUnit();

        /// <summary>
        /// 当前系统显示的机台的ID
        /// </summary>
        private static int currentMachineID;

        /// <summary>
        /// 存储系统当前显示的机台的生产信息对象
        /// </summary>
        private static MachineProductionInfo machineProductionInfo;

        private static ActiveMqService activeMq;
        static OnlineDataCenter() {
            serverPort = int.Parse(Configuration.ServerPort);
            serverIP = IPAddress.Parse(Configuration.ServerIp);
            AppEventsManager.OnUpdateDataServer += Aem_OnUpdateDataServer;
            activeMq = new ActiveMqService("failover:(tcp://192.168.200.100:61616)?timeout=300", "admin", "admin",
                TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// 更新服务端地址和端口号
        /// </summary>
        private static void Aem_OnUpdateDataServer() {
            serverPort = int.Parse(Configuration.ServerPort);
            serverIP = IPAddress.Parse(Configuration.ServerIp);

            CloseClientSocket(); // 关闭，由守护线程重启
        }

        /// <summary>
        /// 获取指定机台的数据
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public static MachineProductionInfo GetMachineProductionInfo(string machineName) {
            if (string.IsNullOrEmpty(machineName)) return null;

            if (Configuration.MachineMap.ContainsKey(machineName)) {
                Machine machineN = Configuration.MachineMap[machineName];
                currentMachineID = machineN.MachineID;
                machineProductionInfo = new MachineProductionInfo();
                machineProductionInfo.MachineName = machineName;
            }
            return machineProductionInfo;
        }

        /// <summary>
        /// 设置当前的机台信息对象
        /// </summary>
        /// <param name="mpi"></param>
        public static void SetMachineProductionInfo(MachineProductionInfo mpi) {
            if (mpi == null) return;

            if (Configuration.MachineMap.ContainsKey(mpi.MachineName)) {
                Machine m = Configuration.MachineMap[mpi.MachineName];
                machineProductionInfo = mpi;
                currentMachineID = m.MachineID;
            }
        }


        /// <summary>
        /// 开启数据服务，与服务端建立连接
        /// </summary>
        public static void StartService() {
            CreateClientSocketAndConnect();
            ThreadPool.QueueUserWorkItem(doMonitorSocketConnection, null);
        }

        /// <summary>
        /// 监听socket连接是否中断
        /// </summary>
        /// <param name="state"></param>
        private static void doMonitorSocketConnection(object state) {
            int baseSleepMilliseconds = 2000;
            int successiveReceiveExceptionCount = 0;
            int sleepMilliseconds = baseSleepMilliseconds;
            while (true) {
                sleepMilliseconds = baseSleepMilliseconds *
                                    (int)Math.Pow(2, successiveReceiveExceptionCount);
                Thread.Sleep(sleepMilliseconds);

                // 如果最近3秒钟都没有接受到服务端的数据，就尝试重连
                if ((DateTime.Now - lastRecieveTime).TotalSeconds > 2) {
                    if (IsNetwork2ServerOK()) {
                        if (++successiveReceiveExceptionCount > 5) {
                            successiveReceiveExceptionCount = 5;
                            CloseAndRebuildSocket();
                        } else { CheckAndFixConnection(); }
                    } else { successiveReceiveExceptionCount = 0; }
                } else { successiveReceiveExceptionCount = 0; }
            }
        }

        /// <summary>
        /// 测试与服务端的的网络是否联通
        /// </summary>
        /// <returns></returns>
        public static bool IsNetwork2ServerOK() {
            try {
                return ping.Send(serverIP, 500).Status == IPStatus.Success;
            } catch { }
            return false;
        }

        #region 连接服务端并接收原始数据

        /// <summary>
        /// 创建socket，并连接到服务端
        /// </summary>
        private static void CreateClientSocketAndConnect() {
            ThreadPool.QueueUserWorkItem(new WaitCallback((o) => {
                lock (clientSocketLock) {
                    // recheck
                    if (clientSocket != null
                        && clientSocket.Connected)
                        return;

                    if (doCreateSocket()) doConnectSocket();
                }
            }), null);
        }

        /// <summary>
        /// 清除当前socket资源，并重新建立socket连接
        /// </summary>
        private static void CloseAndRebuildSocket() {
            CloseClientSocket();
            // 重新创建socket并连接服务端
            CreateClientSocketAndConnect();
        }

        /// <summary>
        /// 关闭客户端socket
        /// </summary>
        private static void CloseClientSocket() {
            if (clientSocket == null) return;

            lock (clientSocketLock) {
                try {
                    clientSocket.Shutdown(SocketShutdown.Both);
                } catch { } finally {
                    if (clientSocket != null) {
                        clientSocket.Close();
                        clientSocket = null;
                    }
                }
            }
        }

        /// <summary>
        /// 客户端socket是否已连接
        /// </summary>
        /// <returns></returns>
        private static bool IsClientSocketConnected() {
            bool clientSocketConnected;
            lock (clientSocketLock) {
                clientSocketConnected = clientSocket != null &&
                                        (clientSocket.Connected || IsSocketConnected());
            }
            return clientSocketConnected;
        }

        /// <summary>
        /// 检测连接的状态，并修复
        /// </summary>
        /// <returns></returns>
        private static void CheckAndFixConnection() {
            if (!IsClientSocketConnected())
                CloseAndRebuildSocket();
        }

        /// <summary>
        /// 当socket.connected为false时，进一步确定下当前连接状态
        /// </summary>
        /// <returns></returns>
        private static bool IsSocketConnected() {
            #region remarks
            /*************************************************************
             * 当Socket.Conneted为false时， 如果您需要确定连接的当前状态，
             * 请进行非阻塞、零字节的 Send 调用。如果该调用成功返回或引发
             * WAEWOULDBLOCK 错误代码 (10035)，则该套接字仍然处于连接状态；        
             * 否则，该套接字不再处于连接状态。
             * Depending on http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.connected.aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-2
            *************************************************************/
            #endregion

            if (clientSocket == null) return false;
            bool connectState = false;
            bool blockingState = false;
            try {
                blockingState = clientSocket.Blocking;
                clientSocket.Blocking = false;

                byte[] tmp = new byte[1];
                //若Send错误会跳去执行catch体
                clientSocket.Send(tmp, 0, 0);
                connectState = true;
            } catch (SocketException e) {
                // 10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                    connectState = true; // Still Connected, but the Send would block
                else
                    connectState = false; // Disconnected
            } catch { connectState = false; } finally {
                try {
                    clientSocket.Blocking = blockingState;
                } catch { }
            }
            return connectState;
        }

        /// <summary>
        /// 创建客户端socket
        /// </summary>
        /// <returns></returns>
        private static bool doCreateSocket() {
            try {
                clientSocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                clientSocket.SendTimeout = 1000;

                // 设置心跳参数，首次探测时间5秒, 间隔侦测时间2秒
                byte[] inValue = new byte[]
                {
                    1, 0, 0, 0, 0x88, 0x13, 0, 0, 0xd0, 0x07, 0, 0
                };
                clientSocket.IOControl(IOControlCode.KeepAliveValues, inValue, null);
                return true;
            } catch {  /** 创建客服端socket失败 Nothing to do... */}
            return false;
        }

        /// <summary>
        /// 客户端socket连接服务端
        /// </summary>
        /// <returns></returns>
        private static bool doConnectSocket() {
            if (clientSocket == null) return false;
            try {
                clientSocket.Connect(serverIP, serverPort);

                StateObject stateObj = new StateObject();
                stateObj.socket = clientSocket;

                clientSocket.BeginReceive(stateObj.buffer, 0, StateObject.BufferSize,
                    0, new AsyncCallback(ReceiveCallback), stateObj);

                return true;
            } catch { /** Nothing to do ... */ }
            return false;
        }

        /// <summary>
        /// 向服务端发送命令
        /// </summary>
        /// <param name="structData"></param>
        public static void SendCommand(StructData structData) {
            if (clientSocket != null && clientSocket.Connected) {
                byte[] sendBytes = seru.SerializeObject(structData);
                if (sendBytes.Length < 2) { return; }

                byte[] sendBuf = new byte[sendBytes.Length + 8];
                byte[] sendkey1 = System.BitConverter.GetBytes(543215);
                byte[] sendkey2 = System.BitConverter.GetBytes(567890);

                try {
                    sendBytes.CopyTo(sendBuf, 4);
                    sendkey1.CopyTo(sendBuf, sendBytes.Length + 4);
                    sendkey2.CopyTo(sendBuf, 0);

                    int n = clientSocket.Send(sendBuf, sendBuf.Length, 0);

                    if (n < 1)
                        n = clientSocket.Send(sendBuf, sendBuf.Length, 0);
                    if (n < 1)
                        n = clientSocket.Send(sendBuf, sendBuf.Length, 0);
                } catch (SocketException) { CloseClientSocket(); } catch (ObjectDisposedException) { CloseClientSocket(); } catch { /** 其他非socket异常，不做处理 */}
            }
        }

        /// <summary>
        /// 接收TCP数据包，解析并处理
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar) {
            lastRecieveTime = DateTime.Now;
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.socket;
            try {
                int bytesRead = client.EndReceive(ar);
                //对端gracefully关闭一个连接
                if (bytesRead <= 0) {
                    CloseClientSocket();
                    return;
                }

                byte[] destinationArray = new byte[bytesRead];
                Array.Copy(state.buffer, destinationArray, bytesRead);

                List<byte[]> lstrec;
                lock (s2Lock) {
                    lstrec = handleRawBytes(destinationArray);
                }
                try {
                    foreach (byte[] destinationArr in lstrec) {
                        StructData sd =
                            (StructData)seru.DeserializeObject(destinationArr);

                        if (sd.cmd == 0x66) {
                            ThreadPool.QueueUserWorkItem(SavePdfBytesToFile, sd);
                        }

                        //[{
                        //    "TaskID": 0,
                        //    "SpecificationID": 0,
                        //    "MachineID": 23,
                        //    "MachineTypeID": 2,
                        //    "EmployeeID_Main": "abc123",
                        //    "EmployeeID_Assistant": "abc123",
                        //    "Start_Axis_No": "abc123",
                        //    "CodeNumber": "abc123",
                        //    "Axis_No": "abc123",
                        //    "Printcode": "abc123",
                        //    "CollectedTime": "abc123",
                        //    "MaterialRFID": 0
                        //}]
                        if (sd.datamain != null && sd.datamain.Length > 0) {
                            byte[] byteString = Encoding.UTF8.GetBytes(sd.datamain);
                            using (MemoryStream stream = new MemoryStream(byteString)) {
                                DataContractJsonSerializer dataMainSerializer =
                                    new DataContractJsonSerializer(typeof(List<DataMain>));
                                OnHandleDataMain((List<DataMain>)dataMainSerializer.ReadObject(stream));
                            }
                        }
                        //[{
                        //    "ParameterCodeID": 42,
                        //    "CollectedValue": "2.161",
                        //    "CollectedTime": "2016/12/16 17:24:22",
                        //    "Axis_No": "abc123",
                        //    "MachineID": 13
                        //}]
                        if (sd.datadetail != null && sd.datadetail.Length > 0) {
                            byte[] byteString = Encoding.UTF8.GetBytes(sd.datadetail);
                            using (MemoryStream stream = new MemoryStream(byteString)) {
                                DataContractJsonSerializer dataDetailSerializer =
                                    new DataContractJsonSerializer(typeof(List<DataDetail>));
                                OnHandleDataDetail((List<DataDetail>)dataDetailSerializer.ReadObject(stream));
                            }
                        }
                    }
                } catch { /* 非Socket异常，忽略 */ }

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            } catch {
                lock (s2Lock) {
                    Array.Clear(s2, 0, s2.Length);
                    s2 = new byte[0];
                }
                CloseClientSocket();
            }
        }

        private static byte[] s2 = new byte[0];

        /// <summary>
        /// 处理接收到的数据
        /// </summary>
        private static List<byte[]> handleRawBytes(byte[] s1) {
            List<byte> lsb = new List<byte>();
            List<byte[]> listrst = new List<byte[]>();

            byte[] sendkey1 = System.BitConverter.GetBytes(543215);
            byte[] sendkey2 = System.BitConverter.GetBytes(567890);

            int kx = 0, rsidlen = 0;

            if (s2.Length > 0)
                rsidlen = s2.Length;

            byte[] s3 = new byte[s1.Length + rsidlen];

            if (s2.Length > 0)
                s2.CopyTo(s3, 0);
            s1.CopyTo(s3, rsidlen);
            Array.Clear(s2, 0, s2.Length);

            for (int i = 0; i < s3.Length; i++) {
                if (i < s3.Length - 3) {
                    if (s3[i] == sendkey1[0] && s3[i + 1] == sendkey1[1]
                        && s3[i + 2] == sendkey1[2] && s3[i + 3] == sendkey1[3]) {
                        listrst.Add(lsb.ToArray());
                        lsb.Clear();
                        i = i + 3;
                    } else {
                        if (s3[i] == sendkey2[0] && s3[i + 1] == sendkey2[1]
                            && s3[i + 2] == sendkey2[2] && s3[i + 3] == sendkey2[3]) {
                            lsb.Clear();
                            i = i + 3;
                        } else {
                            lsb.Add(s3[i]);
                            kx = i;
                        }
                    }
                } else {
                    if (s3.Length - kx > 4)
                        lsb.Add(s3[i]);
                }
            }
            if (lsb.Count > 5 && lsb.Count < 10004) {
                s2 = new byte[lsb.Count];
                lsb.ToArray().CopyTo(s2, 0);
            }

            lsb.Clear();
            return listrst;
        }

        #endregion


        #region 处理接收到的服务器端的数据

        /// <summary>
        /// 强纯过来的Pdf的字节数组保存到本地文件中
        /// </summary>
        /// <param name="state"></param>
        private static void SavePdfBytesToFile(object state) {
            FileStream fileStream = null;
            StructData sd = (StructData)state;
            try {
                fileStream = File.Create(
                    ProductionProcessPdfService.PdfDirPath + sd.filename);

                fileStream.Write(sd.filestream, 0, sd.filestream.Length);
                fileStream.Flush();
                fileStream.Close();

                if (machineProductionInfo != null)
                    machineProductionInfo.PdfService.NotifyGetPdfFromServer(sd.filename);
            } catch {
                if (machineProductionInfo != null)
                    machineProductionInfo.PdfService.NotifyCaugthErrorFromServer(sd.filename);
            } finally {
                if (fileStream != null) { fileStream.Close(); }
            }
        }

        /// <summary>
        /// 获取半成品卡号，机台号，放线处轴号等信息
        /// </summary>
        /// <param name="dtMain"></param>
        private static void OnHandleDataMain(List<DataMain> dataMains) {
            if (dataMains == null || dataMains.Count <= 0
                || machineProductionInfo == null) { return; }

            foreach (DataMain dataMain in dataMains) {
                if (currentMachineID == dataMain.MachineID) {
                    machineProductionInfo.AxisNo = dataMain.Axis_No;
                    machineProductionInfo.EmployeeRfid = dataMain.EmployeeID_Main;

                    string rfid = dataMain.MaterialRFID;
                    if (!string.IsNullOrEmpty(rfid) && rfid.Length > 5) {
                        machineProductionInfo.MaterialRfid = rfid;
                    }
                }
            }
        }

        /// <summary>
        /// 获取详细参数的数据
        /// </summary>
        /// <param name="dtDetail"></param>
        private static void OnHandleDataDetail(List<DataDetail> dataDetails) {
            if (dataDetails == null || dataDetails.Count <= 0
                || machineProductionInfo == null) { return; }

            foreach (DataDetail dataDetail in dataDetails) {
                if (currentMachineID == dataDetail.MachineID)
                    machineProductionInfo.AddDataDetail(dataDetail);
            }
        }

        /// <summary>
        /// 异步启用服务
        /// </summary>
        public static async void StartUseMqAsync() {
            await Task.Run(() => StartUseMq());
        }

        /// <summary>
        /// update: 2018-10-31 
        /// 交互方式更新为 mq
        /// </summary>
        public static void StartUseMq() {
            activeMq.Start();
            activeMq.ListenTopic("mes_online", "98k", null, Show);
        }

        /// <summary>
        /// 刷新显示数据
        /// </summary>
        /// <param name="rest"></param>
        public static void Show(string str) {
            var rest = JsonConvert.DeserializeObject<Rest<List<MesMqParam>>>(str);
            var dataDetails = new List<DataDetail>();
            foreach (var param in rest.data) {
                var detail = new DataDetail();
                detail.MachineID = param.MachineID;
                detail.CollectedTime = param.CollectedTime;
                detail.ParameterCodeID = param.ParameterCodeID;
                detail.CollectedValue = param.CollectedValue;
                dataDetails.Add(detail);
            }
            if (dataDetails == null || dataDetails.Count <= 0
                || machineProductionInfo == null) { return; }

            foreach (var dataDetail in dataDetails) {
                if (currentMachineID == dataDetail.MachineID)
                    machineProductionInfo.AddDataDetail(dataDetail);
            }
        }

        #endregion
    }
}
