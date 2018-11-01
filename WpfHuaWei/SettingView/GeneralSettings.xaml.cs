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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfHuaWei.SettingView
{
    /// <summary>
    /// GeneralSettings.xaml 的交互逻辑
    /// </summary>
    public partial class GeneralSettings : UserControl
    {
        public GeneralSettings()
        {
            InitializeComponent();

            rootGrid.DataContext = Configuration.ConfigurationDto;

            pbPassword.Password = Configuration.ConfigurationDto.Password;
            if (!Configuration.ConfigurationDto.CloseMainwindowToExit)
                this.rbHidden.IsChecked = true;
        }

        /// <summary>
        /// 更新修改的通用配置信息
        /// </summary>
        public void UpdateGenerialConfiguration()
        {
            Configuration.ConfigurationDto.Password = pbPassword.Password;
        }
    }
}
