using AppResorces;
using HuaWeiUtils;
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
using System.Windows.Shapes;

namespace WpfHuaWei.SettingView
{
    /// <summary>
    /// AppSettings.xaml 的交互逻辑
    /// </summary>
    public partial class AppSettings : Window
    {
        public AppSettings()
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

        private void tbOK_Click(object sender, RoutedEventArgs e)
        {
            // 更新通用配置信息
            generalSettings.UpdateGenerialConfiguration();

            // 保存更新的通用配置
            string info = Configuration.SaveConfiguration();
            if (string.IsNullOrEmpty(info))
                this.Close();
            else
                MessageBox.Show(info, "错误提示",
                    MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void tbCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Configuration.RecoveryConfiguration();
        }

    }
}
