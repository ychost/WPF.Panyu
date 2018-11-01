using AppResorces;
using HuaWeiBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfHuaWei.DeviceView
{
    /// <summary>
    /// CurrentBianZhi.xaml 的交互逻辑
    /// </summary>
    public partial class CurrentBianZhi : BaseMachine
    {
        public CurrentBianZhi()
        {
            InitializeComponent();
            this.bzImage.Source = ResourcesUtils.GetImageSource("BianZhi.png");
        }

        public override void StartLoadingData(string machineName)
        {
            base.StartLoadingData(machineName);
        }

    }
}
