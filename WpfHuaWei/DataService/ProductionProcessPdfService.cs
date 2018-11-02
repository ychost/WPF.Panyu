using DataDisplay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using HuaWeiCtls;
using HuaWeiUtils;
using YCsharp.Util;

namespace WpfHuaWei.DataService {
    public class ProductionProcessPdfService {
        /// <summary>
        /// 工艺参数pdf文件保存的文件夹路径
        /// </summary>
        public static readonly string PdfDirPath = "ProductionProcess\\";

        public delegate void ChangeProductionProcessPdfEventHandler(string fileFullPath);

        public event ChangeProductionProcessPdfEventHandler ChangeProductionProcessPdf;

        public ProductionProcessPdfService() {

        }

        /// <summary>
        /// Pdf文件名称
        /// </summary>
        private string pdfFileName;
        public string PdfFileName {
            get { return pdfFileName; }
            set {
                if (!string.IsNullOrEmpty(value)
                    && value != curOpenFileName) {
                    pdfFileName = value;
                    if (File.Exists(PdfDirPath + pdfFileName))
                        OnChangeProductionProcessPdf(value);
                    else {
                        downloadWorkSheetPDFAsync(value);
                    }
                }
            }
        }

        private string curOpenFileName;

        /// <summary>
        /// 异步从服务器下载 PDF 文件
        /// </summary>
        /// <param name="filename"></param>
        private async void downloadWorkSheetPDFAsync(string filename) {
            await Task.Run(() => {
                if (YUtil.DownloadFtpFile($"ftp://{Configuration.ServerIp}/{filename}", PdfDirPath + filename)) {
                    OnChangeProductionProcessPdf(filename);
                } else {
                    UIMessageBox.Show("错误",$"下载工艺文件失败，请检查 [{filename}] 是否存在于服务器",5000);
                }
            });
        }

        /// <summary>
        /// 当前Pdf文件名称改变后回调注册的函数
        /// </summary>
        /// <param name="pdfFileName"></param>
        private void OnChangeProductionProcessPdf(string pdfFileName) {
            curOpenFileName = pdfFileName;
            ChangeProductionProcessPdf?.Invoke(
                Environment.CurrentDirectory + "\\" + PdfDirPath + pdfFileName);
        }
    }
}
