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

namespace HuaWeiCtls
{
    /// <summary>
    /// PdfReader.xaml 的交互逻辑
    /// </summary>
    public partial class PdfReader : UserControl
    {
        private FoxitPdfReader foxitPdfReader;

        public event System.EventHandler OnError;

        public event System.EventHandler OnMessage;

        public PdfReader()
        {
            InitializeComponent();

//            foxitPdfReader = new FoxitPdfReader();
//
//            foxitPdfReader.axFoxitCtl.OnError += axFoxitCtl_OnError;
//            foxitPdfReader.axFoxitCtl.OnMessage += axFoxitCtl_OnMessage;
//
//            this.host.Child = foxitPdfReader;
        }

        private void axFoxitCtl_OnError(object sender, EventArgs e)
        {
            if (this.OnError != null)
            {
                this.OnError.Invoke(sender, e);
            }
        }

        private void axFoxitCtl_OnMessage(object sender, EventArgs e)
        {
            if (this.OnMessage != null)
            {
                this.OnMessage.Invoke(sender, e);
            }
        }

        public void OpenPdfFile(string srcFilePath)
        {
            if (foxitPdfReader != null)
            {
                foxitPdfReader.axFoxitCtl.OpenFile(srcFilePath);
            }
        }

    }
}
