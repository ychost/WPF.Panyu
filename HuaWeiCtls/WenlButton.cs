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

namespace HuaWeiCtls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:HuaWeiCtls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:HuaWeiCtls;assembly=HuaWeiCtls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:WenlButton/>
    ///
    /// </summary>
    public class WenlButton : Button
    {
        private Border border = null;

        private Brush mStaticBackground = new SolidColorBrush(Color.FromRgb(0x42, 0x8B, 0xCA));
        private Brush mStaticBorderBrush = new SolidColorBrush(Color.FromRgb(0x35, 0x7E, 0xBD));
        private Brush mMouseOverBackground = new SolidColorBrush(Color.FromRgb(0x32, 0x76, 0xB1));
        private Brush mMouseOverBorderBrush = new SolidColorBrush(Color.FromRgb(0x28, 0x5E, 0x8F));
        private Brush mPressedBackground = new SolidColorBrush(Color.FromRgb(0x28, 0x5E, 0x8F));
        private Brush mPressedBorderBrush = new SolidColorBrush(Color.FromRgb(0x2C, 0x6A, 0xA1));

        static WenlButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WenlButton),
                new FrameworkPropertyMetadata(typeof(WenlButton)));
        }

        #region Foreground,Background,BorderBrush依赖属性
        private CornerRadius cornerRadius = new CornerRadius(3);
        public CornerRadius CornerRadius
        {
            get { return cornerRadius; }
            set
            {
                cornerRadius = value;
                if(border != null)
                {
                    border.CornerRadius = cornerRadius;
                }
            }
        }

        public Brush StaticForeground
        {
            get { return (Brush)GetValue(StaticForegroundProperty); }
            set { SetValue(StaticForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StaticForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StaticForegroundProperty =
            DependencyProperty.Register("StaticForeground", typeof(Brush), typeof(WenlButton),
            new PropertyMetadata(Brushes.White));

        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverForegroundProperty =
            DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(WenlButton),
            new PropertyMetadata(Brushes.White));

        public Brush PressedForeground
        {
            get { return (Brush)GetValue(PressedForegroundProperty); }
            set { SetValue(PressedForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedForegroundProperty =
            DependencyProperty.Register("PressedForeground", typeof(Brush), typeof(WenlButton),
            new PropertyMetadata(Brushes.White));


        public Brush StaticBackground
        {
            get { return (Brush)GetValue(StaticBackgroundProperty); }
            set { SetValue(StaticBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StaticForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StaticBackgroundProperty =
            DependencyProperty.Register("StaticBackground", typeof(Brush), typeof(WenlButton),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x42, 0x8B, 0xCA))));

        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(WenlButton),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x32, 0x76, 0xB1))));

        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.Register("PressedBackground", typeof(Brush), typeof(WenlButton),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x28, 0x5E, 0x8F))));

        public Brush StaticBorderBrush
        {
            get { return (Brush)GetValue(StaticBorderBrushProperty); }
            set { SetValue(StaticBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StaticForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StaticBorderBrushProperty =
            DependencyProperty.Register("StaticBorderBrush", typeof(Brush), typeof(WenlButton),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x35, 0x7E, 0xBD))));

        public Brush MouseOverBorderBrush
        {
            get { return (Brush)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverBorderBrushProperty =
            DependencyProperty.Register("MouseOverBorderBrush", typeof(Brush), typeof(WenlButton),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x28, 0x5E, 0x8F))));

        public Brush PressedBorderBrush
        {
            get { return (Brush)GetValue(PressedBorderBrushProperty); }
            set { SetValue(PressedBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedBorderBrushProperty =
            DependencyProperty.Register("PressedBorderBrush", typeof(Brush), typeof(WenlButton),
            new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x2C, 0x6A, 0xA1))));
        #endregion

        /// <summary>
        /// 控件预定义的按钮样式
        /// </summary>
        public enum ButtonPattern
        {
            /// <summary>
            /// 原始按钮
            /// </summary>
            Primary,
            /// <summary>
            /// 成功按钮
            /// </summary>
            Success,
            /// <summary>
            /// 信息按钮
            /// </summary>
            Info,
            /// <summary>
            /// 警告按钮
            /// </summary>
            Warning,
            /// <summary>
            /// 危险按钮
            /// </summary>
            Danger
        }

        public ButtonPattern Pattern
        {
            get { return (ButtonPattern)GetValue(PatternProperty); }
            set { SetValue(PatternProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PatternProperty =
            DependencyProperty.Register("Pattern", typeof(ButtonPattern), typeof(WenlButton),
            new FrameworkPropertyMetadata(ButtonPattern.Primary,
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnButtonPatternChangedCallback)));

        /// <summary>
        /// 按钮类型
        /// </summary>
        public enum ButtonType
        {
            /// <summary>
            /// 正常按钮类型
            /// </summary>
            Normal,
            /// <summary>
            /// 链接类型
            /// </summary>
            Link,
            /// <summary>
            /// 标签类型
            /// </summary>
            Label,
            /// <summary>
            /// 自定义类型
            /// </summary>
            Customised
        }

        public ButtonType Type
        {
            get { return (ButtonType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(ButtonType), typeof(WenlButton),
            new FrameworkPropertyMetadata(ButtonType.Normal,
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnButtonTypeChangedCallBack)));

        /// <summary>
        /// 改变按钮的样式
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void OnButtonPatternChangedCallback(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            WenlButton wBtn = d as WenlButton;
            if(e.NewValue != e.OldValue && wBtn != null)
            {
                ButtonPattern pattern = (ButtonPattern)e.NewValue;
                SetWenlButtonColorPattern(wBtn, pattern, wBtn.Type);
            }
        }

        /// <summary>
        /// 改变按钮的类型
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnButtonTypeChangedCallBack(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            WenlButton wBtn = d as WenlButton;
            if(e.NewValue != e.OldValue && wBtn != null)
            {
                ButtonType type = (ButtonType)e.NewValue;
                SetWenlButtonColorPattern(wBtn, wBtn.Pattern, type);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wBtn"></param>
        /// <param name="type"></param>
        /// <param name="pattern"></param>
        private static void SetWenlButtonColorPattern(WenlButton wBtn,
            ButtonPattern pattern, ButtonType type)
        {
            if(type == ButtonType.Customised)
            {
                wBtn.mStaticBackground = wBtn.StaticBackground;
                wBtn.mStaticBorderBrush = wBtn.StaticBorderBrush;
                wBtn.mMouseOverBackground = wBtn.MouseOverBackground;
                wBtn.mMouseOverBorderBrush = wBtn.MouseOverBorderBrush;
                wBtn.mPressedBackground = wBtn.PressedBackground;
                wBtn.mPressedBorderBrush = wBtn.PressedBorderBrush;
            }
            else if(type == ButtonType.Normal || type == ButtonType.Label)
            {
                switch(pattern)
                {
                case ButtonPattern.Primary:
                    wBtn.mStaticBackground = new SolidColorBrush(Color.FromRgb(0x42, 0x8B, 0xCA));
                    wBtn.mStaticBorderBrush = new SolidColorBrush(Color.FromRgb(0x35, 0x7E, 0xBD));
                    wBtn.mMouseOverBackground = new SolidColorBrush(Color.FromRgb(0x32, 0x76, 0xB1));
                    wBtn.mMouseOverBorderBrush = new SolidColorBrush(Color.FromRgb(0x28, 0x5E, 0x8F));
                    wBtn.mPressedBackground = new SolidColorBrush(Color.FromRgb(0x28, 0x5E, 0x8F));
                    wBtn.mPressedBorderBrush = new SolidColorBrush(Color.FromRgb(0x2C, 0x6A, 0xA1));
                    break;
                case ButtonPattern.Success:
                    wBtn.mStaticBackground = new SolidColorBrush(Color.FromRgb(0x5C, 0xB8, 0x5C));
                    wBtn.mStaticBorderBrush = new SolidColorBrush(Color.FromRgb(0x4C, 0xAE, 0x4C));
                    wBtn.mMouseOverBackground = new SolidColorBrush(Color.FromRgb(0x46, 0xA5, 0x46));
                    wBtn.mMouseOverBorderBrush = new SolidColorBrush(Color.FromRgb(0x38, 0x85, 0x38));
                    wBtn.mPressedBackground = new SolidColorBrush(Color.FromRgb(0x40, 0x95, 0x40));
                    wBtn.mPressedBorderBrush = new SolidColorBrush(Color.FromRgb(0x38, 0x83, 0x38));
                    break;
                case ButtonPattern.Info:
                    wBtn.mStaticBackground = new SolidColorBrush(Color.FromRgb(0x5B, 0xC0, 0xDE));
                    wBtn.mStaticBorderBrush = new SolidColorBrush(Color.FromRgb(0x46, 0xB8, 0xDA));
                    wBtn.mMouseOverBackground = new SolidColorBrush(Color.FromRgb(0x38, 0xB3, 0xD7));
                    wBtn.mMouseOverBorderBrush = new SolidColorBrush(Color.FromRgb(0x26, 0x9B, 0xBD));
                    wBtn.mPressedBackground = new SolidColorBrush(Color.FromRgb(0x33, 0xA7, 0xC7));
                    wBtn.mPressedBorderBrush = new SolidColorBrush(Color.FromRgb(0x26, 0x9B, 0xBD));
                    break;
                case ButtonPattern.Warning:
                    wBtn.mStaticBackground = new SolidColorBrush(Color.FromRgb(0xF0, 0xAD, 0x4E));
                    wBtn.mStaticBorderBrush = new SolidColorBrush(Color.FromRgb(0xE5, 0x9D, 0x36));
                    wBtn.mMouseOverBackground = new SolidColorBrush(Color.FromRgb(0xED, 0x9D, 0x28));
                    wBtn.mMouseOverBorderBrush = new SolidColorBrush(Color.FromRgb(0xD5, 0x85, 0x12));
                    wBtn.mPressedBackground = new SolidColorBrush(Color.FromRgb(0xDD, 0x91, 0x23));
                    wBtn.mPressedBorderBrush = new SolidColorBrush(Color.FromRgb(0xD5, 0x85, 0x12));
                    break;
                case ButtonPattern.Danger:
                    wBtn.mStaticBackground = new SolidColorBrush(Color.FromRgb(0xD9, 0x53, 0x4F));
                    wBtn.mStaticBorderBrush = new SolidColorBrush(Color.FromRgb(0xC6, 0x39, 0x36));
                    wBtn.mMouseOverBackground = new SolidColorBrush(Color.FromRgb(0xD3, 0x32, 0x2C));
                    wBtn.mMouseOverBorderBrush = new SolidColorBrush(Color.FromRgb(0xAD, 0x28, 0x24));
                    wBtn.mPressedBackground = new SolidColorBrush(Color.FromRgb(0xC3, 0x2E, 0x28));
                    wBtn.mPressedBorderBrush = new SolidColorBrush(Color.FromRgb(0xAD, 0x28, 0x24));
                    break;
                default:
                    break;
                }
                if(type == ButtonType.Label)
                {
                    wBtn.mStaticBackground = Brushes.Transparent;
                    wBtn.mStaticBorderBrush = Brushes.Transparent;
                }
            }
            else
            {
                wBtn.mStaticBackground = Brushes.Transparent;
                wBtn.mStaticBorderBrush = Brushes.Transparent;
                wBtn.mMouseOverBackground = Brushes.Transparent;
                wBtn.mMouseOverBorderBrush = Brushes.Transparent;
                wBtn.mPressedBackground = Brushes.Transparent;
                wBtn.mPressedBorderBrush = Brushes.Transparent;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.border = GetTemplateChild("border") as Border;

            this.MouseEnter += WenlButton_MouseEnter;
            this.MouseLeave += WenlButton_MouseLeave;

            this.Background = mStaticBackground;
            this.BorderBrush = mStaticBorderBrush;
            if(Type == ButtonType.Link)
            {
                this.Foreground = GetLinkTypeForeground(1);
            }

            this.Loaded += WenlButton_Loaded;
        }

        void WenlButton_Loaded(object sender, RoutedEventArgs e)
        {
            if(border != null)
            {
                border.CornerRadius = this.cornerRadius;
            }
        }

        void WenlButton_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Background = mMouseOverBackground;
            this.BorderBrush = mMouseOverBorderBrush;
            if(Type == ButtonType.Link)
            {
                this.Foreground = GetLinkTypeForeground(2);
            }
        }

        void WenlButton_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Background = mStaticBackground;
            this.BorderBrush = mStaticBorderBrush;
            if(Type == ButtonType.Link)
            {
                this.Foreground = GetLinkTypeForeground(1);
            }
        }

        protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsPressedChanged(e);

            if(this.IsPressed)
            {
                this.Background = mPressedBackground;
                this.BorderBrush = mPressedBorderBrush;
                if(Type == ButtonType.Link)
                {
                    this.Foreground = GetLinkTypeForeground(3);
                }
            }
            else if(this.IsMouseOver)
            {
                this.Background = mMouseOverBackground;
                this.BorderBrush = mMouseOverBorderBrush;
                if(Type == ButtonType.Link)
                {
                    this.Foreground = GetLinkTypeForeground(2);
                }
            }
            else
            {
                this.Background = mStaticBackground;
                this.BorderBrush = mStaticBorderBrush;
                if(Type == ButtonType.Link)
                {
                    this.Foreground = GetLinkTypeForeground(1);
                }
            }
        }

        /// <summary>
        /// 根据按钮设置的不同样式和所处的状态生成Foreground
        /// </summary>
        /// <param name="state">1,static; 2,mouseover; 3,pressed</param>
        /// <returns></returns>
        private Brush GetLinkTypeForeground(int state)
        {
            if(state == 1)
            {
                switch(Pattern)
                {
                case ButtonPattern.Primary:
                    return new SolidColorBrush(Color.FromRgb(0x42, 0x8B, 0xCA));
                case ButtonPattern.Success:
                    return new SolidColorBrush(Color.FromRgb(0x5C, 0xB8, 0x5C));
                case ButtonPattern.Info:
                    return new SolidColorBrush(Color.FromRgb(0x5B, 0xC0, 0xDE));
                case ButtonPattern.Warning:
                    return new SolidColorBrush(Color.FromRgb(0xF0, 0xAD, 0x4E));
                case ButtonPattern.Danger:
                    return new SolidColorBrush(Color.FromRgb(0xD9, 0x53, 0x4F));
                default:
                    return Brushes.Black;
                }
            }
            else if(state == 2)
            {
                switch(Pattern)
                {
                case ButtonPattern.Primary:
                    return new SolidColorBrush(Color.FromRgb(0x1F, 0x62, 0x9D));
                case ButtonPattern.Success:
                    return new SolidColorBrush(Color.FromRgb(0x2E, 0x74, 0x2E));
                case ButtonPattern.Info:
                    return new SolidColorBrush(Color.FromRgb(0x31, 0x86, 0x9B));
                case ButtonPattern.Warning:
                    return new SolidColorBrush(Color.FromRgb(0xD6, 0x86, 0x12));
                case ButtonPattern.Danger:
                    return new SolidColorBrush(Color.FromRgb(0xD6, 0x05, 0x00));
                default:
                    return Brushes.Black;
                }
            }
            else if(state == 3)
            {
                switch(Pattern)
                {
                case ButtonPattern.Primary:
                    return new SolidColorBrush(Color.FromRgb(0x28, 0x5E, 0x8F));
                case ButtonPattern.Success:
                    return new SolidColorBrush(Color.FromRgb(0x40, 0x95, 0x40));
                case ButtonPattern.Info:
                    return new SolidColorBrush(Color.FromRgb(0x33, 0xA7, 0xC7));
                case ButtonPattern.Warning:
                    return new SolidColorBrush(Color.FromRgb(0xDD, 0x91, 0x23));
                case ButtonPattern.Danger:
                    return new SolidColorBrush(Color.FromRgb(0xC3, 0x2E, 0x28));
                default:
                    return Brushes.Black;
                }
            }
            return Brushes.Black;
        }
    }
}
