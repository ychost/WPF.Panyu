using AppResorces;
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
using System.Windows.Shapes;

namespace WpfHuaWei
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();

            this.rootGrid.Background = new ImageBrush()
            {
                Stretch = Stretch.Fill,
                ImageSource = ResourcesUtils.GetImageSource("appsetting_bg.jpg")
            };
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            this.Hide();
            e.Cancel = true;
        }
    }
}
