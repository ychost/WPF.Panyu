using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace HuaWeiCtls
{
    /// <summary>
    /// Interaction logic for CaptionButtons.xaml
    /// </summary>
    public partial class CaptionButtons
    {
        /// <summary>
        /// The parent Window of the control.
        /// </summary>
        private Window _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptionButtons"/> class.
        /// </summary>
        public CaptionButtons()
        {
            InitializeComponent();
            this.Loaded += CaptionButtonsLoaded;
        }

        /// <summary>
        /// Event when the control is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void CaptionButtonsLoaded(object sender, RoutedEventArgs e)
        {
            _parent = GetTopParent();
        }

        /// <summary>
        /// Action on the button to close the window.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            if (_parent != null)
            {
                _parent.Close();
            }
        }

        /// <summary>
        /// Changes the view of the window (maximized or normal).
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RestoreButtonClick(object sender, RoutedEventArgs e)
        {
            _parent.WindowState = _parent.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        /// <summary>
        /// Minimizes the Window.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            _parent.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Gets the top parent (Window).
        /// </summary>
        /// <returns>The parent Window.</returns>
        private Window GetTopParent()
        {
            return Window.GetWindow(this);
        }

        /// <summary>
        /// Gets or sets the margin button.
        /// </summary>
        /// <value>The margin button.</value>
        public Thickness MarginButton
        {
            get { return (Thickness)GetValue(MarginButtonProperty); }
            set { base.SetValue(MarginButtonProperty, value); }
        }

        /// <summary>
        /// The dependency property for the Margin between the buttons.
        /// </summary>
        public static DependencyProperty MarginButtonProperty = DependencyProperty.Register(
            "MarginButton", typeof(Thickness), typeof(CaptionButtons));

        /// <summary>
        /// Enum of the types of caption buttons
        /// 标题栏按钮类型
        /// </summary>
        public enum CaptionType
        {
            /// <summary>
            /// All the buttons
            /// 包含所有按钮（最小化，最大化，关闭按钮）
            /// </summary>
            Full,
            /// <summary>
            /// Only the close button
            /// 仅包含关闭按钮
            /// </summary>
            Close,
            /// <summary>
            /// Reduce and close buttons
            /// 包含最小化和关闭按钮
            /// </summary>
            ReduceClose
        }

        /// <summary>
        /// Gets or sets the visibility of the buttons.
        /// </summary>
        /// <value>The visible buttons.</value>
        public CaptionType Type
        {
            get { return (CaptionType)GetValue(TypeProperty); }
            set { base.SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// The dependency property for the Margin between the buttons.
        /// </summary>
        public static DependencyProperty TypeProperty = DependencyProperty.Register(
            "Type", typeof(CaptionType), typeof(CaptionButtons), new PropertyMetadata(CaptionType.Full));

        public bool ShowSettingButton
        {
            get { return (bool)GetValue(ShowSettingButtonProperty); }
            set { SetValue(ShowSettingButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowSettingButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowSettingButtonProperty =
            DependencyProperty.Register("ShowSettingButton", typeof(bool), typeof(CaptionButtons), new PropertyMetadata(false));

        /// <summary>
        /// 最小化按钮鼠标放上后的背景颜色
        /// </summary>
        public Color ReduceButtonMouseOverColor
        {
            get { return (Color)GetValue(ReduceButtonMouseOverColorProperty); }
            set { SetValue(ReduceButtonMouseOverColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReduceButtonMouseOverColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReduceButtonMouseOverColorProperty =
            DependencyProperty.Register("ReduceButtonMouseOverColor", typeof(Color), typeof(CaptionButtons), new PropertyMetadata(Colors.Blue));

        /// <summary>
        /// 最小化按钮鼠标按下后的背景颜色
        /// </summary>
        public Color ReduceButtonMousePressedColor
        {
            get { return (Color)GetValue(ReduceButtonMousePressedColorProperty); }
            set { SetValue(ReduceButtonMousePressedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReduceButtonMousePressedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReduceButtonMousePressedColorProperty =
            DependencyProperty.Register("ReduceButtonMousePressedColor", typeof(Color), typeof(CaptionButtons), new PropertyMetadata(Colors.DarkBlue));

        /// <summary>
        /// 最大化按钮鼠标放上后的背景颜色
        /// </summary>
        public Color MaximizeButtonMouseOverColor
        {
            get { return (Color)GetValue(MaximizeButtonMouseOverColorProperty); }
            set { SetValue(MaximizeButtonMouseOverColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaximizeButtonMouseOverColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximizeButtonMouseOverColorProperty =
            DependencyProperty.Register("MaximizeButtonMouseOverColor", typeof(Color), typeof(CaptionButtons), new PropertyMetadata(Colors.Blue));

        /// <summary>
        /// 最大化按钮鼠标按下后的背景颜色
        /// </summary>
        public Color MaximizeButtonMousePressedColor
        {
            get { return (Color)GetValue(MaximizeButtonMousePressedColorProperty); }
            set { SetValue(MaximizeButtonMousePressedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaximizeButtonMousePressedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximizeButtonMousePressedColorProperty =
            DependencyProperty.Register("MaximizeButtonMousePressedColor", typeof(Color), typeof(CaptionButtons), new PropertyMetadata(Colors.DarkBlue));

        /// <summary>
        /// 关闭按钮鼠标放上后的背景颜色
        /// </summary>
        public Color CloseButtonMouseOverColor
        {
            get { return (Color)GetValue(CloseButtonMouseOverColorProperty); }
            set { SetValue(CloseButtonMouseOverColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseButtonMouseOverColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseButtonMouseOverColorProperty =
            DependencyProperty.Register("CloseButtonMouseOverColor", typeof(Color), typeof(CaptionButtons), new PropertyMetadata(Colors.Red));

        /// <summary>
        /// 关闭按钮鼠标按下后的背景颜色
        /// </summary>
        public Color CloseButtonMousePressedColor
        {
            get { return (Color)GetValue(CloseButtonMousePressedColorProperty); }
            set { SetValue(CloseButtonMousePressedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseButtonMousePressedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseButtonMousePressedColorProperty =
            DependencyProperty.Register("CloseButtonMousePressedColor", typeof(Color), typeof(CaptionButtons), new PropertyMetadata(Colors.DarkRed));
    }
}
