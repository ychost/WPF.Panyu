using DataDisplay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace WpfHuaWei.DataService
{
    public class ProductionProcessPdfService
    {
        /// <summary>
        /// 工艺参数pdf文件保存的文件夹路径
        /// </summary>
        public static readonly string PdfDirPath = "ProductionProcess\\";

        /// <summary>
        /// 从服务器获取Pdf文件时的超时计时器
        /// </summary>
        private System.Timers.Timer timeoutTimer;

        public delegate void ChangeProductionProcessPdfEventHandler(string fileFullPath);

        public event ChangeProductionProcessPdfEventHandler ChangeProductionProcessPdf;

        public ProductionProcessPdfService()
        {
            timeoutTimer = new System.Timers.Timer();
            timeoutTimer.Interval = 5000; // 5秒
            timeoutTimer.Elapsed += timeoutTimer_Elapsed;
            try
            {
                if (!Directory.Exists(PdfDirPath))
                    Directory.CreateDirectory(PdfDirPath);
                else
                {
                    string[] pdfFiles = Directory.GetFiles(PdfDirPath);
                    foreach (string file in pdfFiles)
                    {
                        File.Delete(file);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 获取Pdf文件超时，重新发送命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeoutTimer_Elapsed(object sender, EventArgs e)
        {
            GetPdfFromServer(pdfFileName);
        }

        /// <summary>
        /// Pdf文件名称
        /// </summary>
        private string pdfFileName;
        public string PdfFileName
        {
            get { return pdfFileName; }
            set
            {
                if (!string.IsNullOrEmpty(value)
                    && !value.Equals(pdfFileName))
                {
                    pdfFileName = value;

                    if (File.Exists(PdfDirPath + pdfFileName))
                        OnChangeProductionProcessPdf(value);
                    else
                    {
                        timeoutTimer.Stop();
                        timeoutTimer.Start();
                        GetPdfFromServer(value);
                    }
                }
            }
        }

        /// <summary>
        /// 从服务器获取指定名称的PDF文件
        /// </summary>
        /// <param name="pdfFileName"></param>
        public void GetPdfFromServer(string pdfFileName)
        {
            StructData structData = new StructData();

            structData.cmd = 0x55;
            structData.filename = pdfFileName;

            OnlineDataCenter.SendCommand(structData);
        }

        /// <summary>
        /// 通知获取从服务器端获取到了Pdf文件
        /// </summary>
        /// <param name="pdfFileName"></param>
        public void NotifyGetPdfFromServer(string pdfFileName)
        {
            if (!string.IsNullOrEmpty(pdfFileName)
                && pdfFileName.Equals(this.pdfFileName))
            {
                timeoutTimer.Stop();
                OnChangeProductionProcessPdf(pdfFileName);
            }
        }

        /// <summary>
        /// 通知从服务器端获取Pdf文件时遇到了错误
        /// </summary>
        /// <param name="pdfFileName"></param>
        public void NotifyCaugthErrorFromServer(string pdfFileName)
        {
            if (!string.IsNullOrEmpty(pdfFileName)
                && (pdfFileName.Equals(this.pdfFileName)))
            {
                timeoutTimer.Stop();
                timeoutTimer.Start();
                GetPdfFromServer(pdfFileName);
            }
        }

        /// <summary>
        /// 当前Pdf文件名称改变后回调注册的函数
        /// </summary>
        /// <param name="pdfFileName"></param>
        private void OnChangeProductionProcessPdf(string pdfFileName)
        {
            if (ChangeProductionProcessPdf != null)
            {
                ChangeProductionProcessPdf.Invoke(
                    Environment.CurrentDirectory + "\\" + PdfDirPath + pdfFileName);
            }
        }
    }
}
