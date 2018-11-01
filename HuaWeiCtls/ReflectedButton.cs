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
    ///     <MyNamespace:ReflectedButton/>
    ///
    /// </summary>
    public class ReflectedButton : Control
    {
        private FrameworkElement PART_Container;
        private TextBlock PART_TextBlock;
        private Border PART_BottomBorder;
        private Border MaskBorder;
        private Rectangle ReflectRectangle;

        private bool IsMouseInButton = false;

        private DispatcherTimer DTimer = null;
        private int BlinkTime = 0;
        private Brush BlinkSavedBrush = null;

        private static LinearGradientBrush NormalBrush = new LinearGradientBrush();
        private static LinearGradientBrush StoppedBrush = new LinearGradientBrush();
        private static LinearGradientBrush WarningBrush = new LinearGradientBrush();
        private static LinearGradientBrush ErrorBrush = new LinearGradientBrush();

        static ReflectedButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ReflectedButton), new FrameworkPropertyMetadata(typeof(ReflectedButton)));

            NormalBrush.StartPoint = new Point(0.5, 0);
            NormalBrush.EndPoint = new Point(0.5, 1);
            NormalBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0x34, 0xD3, 0x2E), 0));
            NormalBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0x24, 0xB4, 0x21), 0.64));
            NormalBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0x31, 0xC4, 0x2C), 1));

            StoppedBrush.StartPoint = new Point(0.5, 0);
            StoppedBrush.EndPoint = new Point(0.5, 1);
            StoppedBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0xAF, 0xAF, 0xAF), 0));
            StoppedBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0xA0, 0xA0, 0xA0), 0.64));
            StoppedBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0xA4, 0xA4, 0xA4), 1));

            WarningBrush.StartPoint = new Point(0.5, 0);
            WarningBrush.EndPoint = new Point(0.5, 1);
            WarningBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0xE8, 0xE8, 0x00), 0));
            WarningBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0xD0, 0xD0, 0x08), 0.64));
            WarningBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0xE0, 0xE0, 0x00), 1));

            ErrorBrush.StartPoint = new Point(0.5, 0);
            ErrorBrush.EndPoint = new Point(0.5, 1);
            ErrorBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0xF0, 0x58, 0x60), 0));
            ErrorBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0xD0, 0x58, 0x60), 0.64));
            ErrorBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0xE0, 0x58, 0x60), 1));
        }

        #region 依赖属性，设置按钮的文本
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ReflectedButton), new PropertyMetadata(string.Empty));
        #endregion

        #region 依赖属性，设置底边的背景
        public Brush BottomBrush
        {
            get { return (Brush)GetValue(BottomBrushProperty); }
            set { SetValue(BottomBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BottomBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomBrushProperty =
            DependencyProperty.Register("BottomBrush", typeof(Brush), typeof(ReflectedButton), new PropertyMetadata(Brushes.Transparent));
        #endregion

        #region 路由事件，向外通知按钮点击
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ReflectedButton));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }
        #endregion

        private DeviceState _state = DeviceState.Normal;
        public DeviceState State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnStateChanged();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_Container = this.GetTemplateChild("PART_Container") as FrameworkElement;
            this.PART_TextBlock = this.GetTemplateChild("PART_TextBlock") as TextBlock;
            this.PART_BottomBorder = this.GetTemplateChild("PART_BottomBorder") as Border;
            this.MaskBorder = this.GetTemplateChild("MaskBorder") as Border;
            this.ReflectRectangle = this.GetTemplateChild("ReflectRectangle") as Rectangle;

            ImageBrush imageBrush = this.GetTemplateChild("BtnImageBrush") as ImageBrush;
            imageBrush.ImageSource = CtrlUtils.ChangeBitmapToImageSource(HuaWeiCtls.Properties.Resources.btn_bg);

            this.PART_TextBlock.SetBinding(TextBlock.TextProperty, new Binding("Text") { Source = this });
            this.PART_BottomBorder.SetBinding(Border.BackgroundProperty, new Binding("BottomBrush") { Source = this });

            this.SizeChanged += ReflectedButton_SizeChanged;
            this.PART_Container.MouseEnter += PART_Container_MouseEnter;
            this.PART_Container.MouseLeave += PART_Container_MouseLeave;
            this.PART_Container.MouseLeftButtonDown += PART_Container_MouseLeftButtonDown;
            this.PART_Container.MouseLeftButtonUp += PART_Container_MouseLeftButtonUp;
        }

        public void Blink(int count = 2)
        {
            BlinkTime = count * 2;
            BlinkSavedBrush = BottomBrush;

            DTimer = new DispatcherTimer();
            DTimer.Interval = TimeSpan.FromMilliseconds(100);
            DTimer.Tick += DTimer_Tick;

            DTimer.Start();
        }

        private int blinkCount = 0;
        void DTimer_Tick(object sender, EventArgs e)
        {
            if(blinkCount < BlinkTime)
            {
                if(blinkCount % 2 == 0)
                {
                    this.BottomBrush = new SolidColorBrush(Colors.Transparent);
                }
                else
                {
                    this.BottomBrush = BlinkSavedBrush;
                }
                blinkCount += 1;
            }
            else
            {
                blinkCount = 0;
                this.DTimer.Stop();
                this.DTimer = null;
            }
        }

        private void OnStateChanged()
        {
            switch(State)
            {
            case DeviceState.Normal:
                BottomBrush = NormalBrush;
                break;
            case DeviceState.Stopped:
                BottomBrush = StoppedBrush;
                break;
            case DeviceState.Warning:
                BottomBrush = WarningBrush;
                break;
            case DeviceState.Error:
                BottomBrush = ErrorBrush;
                break;
            default:
                break;
            }
        }

        void ReflectedButton_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(ReflectRectangle != null)
            {
                this.ReflectRectangle.Height = this.PART_Container.ActualHeight;
            }
        }

        void PART_Container_MouseEnter(object sender, MouseEventArgs e)
        {
            if(MaskBorder != null)
            {
                this.MaskBorder.Background = new SolidColorBrush(Color.FromArgb(0x18, 0x00, 0x00, 0x00));
            }
        }

        void PART_Container_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsMouseInButton = true;
            if(MaskBorder != null)
            {
                this.MaskBorder.Background = new SolidColorBrush(Color.FromArgb(0x30, 0x00, 0x00, 0x00));
            }
        }

        void PART_Container_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(IsMouseInButton)
            {
                RaiseEvent(new RoutedEventArgs(ClickEvent, this));
            }
            if(MaskBorder != null)
            {
                this.MaskBorder.Background = new SolidColorBrush(Color.FromArgb(0x18, 0x00, 0x00, 0x00));
            }
        }

        void PART_Container_MouseLeave(object sender, MouseEventArgs e)
        {
            IsMouseInButton = false;
            if(MaskBorder != null)
            {
                this.MaskBorder.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));
            }
        }
    }
}
