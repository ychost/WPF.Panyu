using System;
using System.Collections.Generic;
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

namespace HuaWeiCtls {
    /// <summary>
    /// PdfReader.xaml 的交互逻辑
    /// </summary>
    public partial class PdfReader : UserControl {
        private readonly FoxitPdfReader foxitPdfReader;

        public event System.EventHandler OnError;

        public event System.EventHandler OnMessage;

        public PdfReader() {
            InitializeComponent();

            foxitPdfReader = new FoxitPdfReader();
            foxitPdfReader.axFoxitCtl1.OnError += axFoxitCtl_OnError;
            foxitPdfReader.axFoxitCtl1.OnMessage += axFoxitCtl_OnMessage;
            this.host.Child = foxitPdfReader;
        }

        private void axFoxitCtl_OnError(object sender, EventArgs e) {
            OnError?.Invoke(sender, e);
        }

        private void axFoxitCtl_OnMessage(object sender, EventArgs e) {
            OnMessage?.Invoke(sender, e);
        }

        public void OpenPdfFile(string srcFilePath) {
            foxitPdfReader?.axFoxitCtl1.OpenFile(srcFilePath);
        }

    }
}
