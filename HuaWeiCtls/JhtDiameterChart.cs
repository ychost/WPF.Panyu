using HuaWeiBase;
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
using System.Windows.Media.Animation;
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
    ///     <MyNamespace:JhtDiameterChart/>
    ///
    /// </summary>
    public class JhtDiameterChart : Control
    {
        private FrameworkElement outerElipse = null;
        private Image InnerImage = null;

        private double lastOuterDiameter = 0;
        private double lastInnerDiameter = 0;

        private TextBlock tbDelta = null;

        private TextBlock tbOuterDiameter = null;
        private TextBlock tbInnerDiameter = null;
        private TextBlock tbDeltaDiameter = null;

        private List<AnimationTask> animationTasks = new List<AnimationTask>();

        static JhtDiameterChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JhtDiameterChart),
                new FrameworkPropertyMetadata(typeof(JhtDiameterChart)));
        }

        private double referencedDiameter = 10.0;
        public double ReferencedDiameter
        {
            get { return referencedDiameter; }
            set { referencedDiameter = value; }
        }

        private double duration = 300;
        public double Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public Brush OuterBackground
        {
            get { return (Brush)GetValue(OuterBackgroundProperty); }
            set { SetValue(OuterBackgroundProperty, value); }
        }

        public static readonly DependencyProperty OuterBackgroundProperty =
            DependencyProperty.Register("OuterBackground", typeof(Brush),
            typeof(JhtDiameterChart), new PropertyMetadata(Brushes.Black));

        /// <summary>
        /// 外径值变化时使用动画更新
        /// </summary>
        /// <param name="outerDiameter"></param>
        public void SetOuterDiameter(double outerDiameter)
        {
            lastOuterDiameter = outerDiameter;
            SetEillpseDiameter(outerDiameter, lastInnerDiameter, duration);
        }

        /// <summary>
        /// 暂存内径值，等待外径值变化后更新
        /// </summary>
        /// <param name="innerDiameter"></param>
        public void SetInnerDiameter(double innerDiameter)
        {
            lastInnerDiameter = innerDiameter;
            if (lastOuterDiameter <= 0)
            {
                SetEillpseDiameter(lastOuterDiameter, innerDiameter, duration);
            }
        }

        public void SetOuterInnerDiameter(double outerDiameter, double innerDiameter)
        {
            lastOuterDiameter = outerDiameter;
            lastInnerDiameter = innerDiameter;

            SetEillpseDiameter(outerDiameter, innerDiameter, duration);
        }

        private void SetEillpseDiameter(double outerDiameter, double innerDiameter, double duration)
        {
            if (outerDiameter <= 0.0)
            {
                tbOuterDiameter.Text = "无数据";
                tbDeltaDiameter.Text = GlobalConstants.NullOrEmptyReplacer;
                outerDiameter = 0.0;
            }
            if (innerDiameter <= 0.0)
            {
                tbInnerDiameter.Text = "无数据";
                tbDeltaDiameter.Text = GlobalConstants.NullOrEmptyReplacer;
                innerDiameter = 0.0;
            }
            if (outerDiameter > 0 && innerDiameter > 0
                && innerDiameter > outerDiameter)
            {
                tbInnerDiameter.Text = "值异常";
                tbDeltaDiameter.Text = GlobalConstants.NullOrEmptyReplacer;
                innerDiameter = 0.0;
            }

            if (EnsureTemplateParts() && (outerDiameter > 0 || innerDiameter > 0))
            {
                bool restartAnimation = false;
                lock (this)
                {
                    if (animationTasks.Count == 0)
                    {
                        restartAnimation = true;
                    }
                    animationTasks.Add(new AnimationTask(outerDiameter, innerDiameter, duration));
                }
                if (restartAnimation)
                    SetEillpseDiameter0(outerDiameter, innerDiameter, duration);
            }
        }

        private void SetEillpseDiameter0(AnimationTask animationTask)
        {
            SetEillpseDiameter0(animationTask.OuterValue, animationTask.InnerValue,
                animationTask.Duration);
        }

        private void SetEillpseDiameter0(double outerDiameter, double innerDiameter, double duration)
        {
            double outerWidth = outerDiameter > 0 ? AdjustOuterDiameter(outerDiameter) : 0;
            double innerWidth = AdjustInnerDiameter(innerDiameter, outerDiameter > 0.0);

            SizeAnimation(outerWidth, innerWidth, duration);

            if (outerDiameter > 0.0)
                tbOuterDiameter.Text = outerDiameter.ToString(GlobalConstants.DoubleFormat);

            if (innerDiameter > 0.0)
                tbInnerDiameter.Text = innerDiameter.ToString(GlobalConstants.DoubleFormat);

            if (innerDiameter > 0.0 && outerDiameter > 0 && innerDiameter < outerDiameter)
                tbDeltaDiameter.Text = ((outerDiameter - innerDiameter) / 2.0).ToString(GlobalConstants.DoubleFormat);
        }

        // 外径连续偏大的次数
        private int successiveOverrangeCount11 = 0;
        // 外径连续偏小的次数
        private int successiveOverrangeCount12 = 0;
        // 内径连续偏大的次数
        private int successiveOverrangeCount21 = 0;
        // 内径连续偏小的次数
        private int successiveOverrangeCount22 = 0;

        /// <summary>
        /// 调整外径值和最大直径MaxmiunDiameter
        /// </summary>
        /// <param name="outerDiameter"></param>
        /// <returns></returns>
        private double AdjustOuterDiameter(double outerDiameter)
        {
            double adjustedDiameter;
            double factor = outerDiameter / referencedDiameter;
            if (factor > 1.15)
            {
                if (factor > 1.3)
                    adjustedDiameter = RenderSize.Width * 1.3 * 0.7;
                else
                    adjustedDiameter = RenderSize.Width * factor * 0.7;
                if (++successiveOverrangeCount11 >= 5)
                {
                    successiveOverrangeCount11 = 0;
                    referencedDiameter = outerDiameter;
                }
            }
            else if (factor < 0.85 && factor > 0)
            {
                adjustedDiameter = RenderSize.Width * factor * 0.7;
                if (++successiveOverrangeCount12 >= 5)
                {
                    successiveOverrangeCount12 = 0;
                    referencedDiameter = outerDiameter;
                }
            }
            else
            {
                successiveOverrangeCount11 = 0;
                successiveOverrangeCount12 = 0;
                adjustedDiameter = RenderSize.Width * factor * 0.7;
            }

            return adjustedDiameter;
        }

        /// <summary>
        /// 调整内径值和最大直径MaxmiunDiameter
        /// </summary>
        /// <param name="innerDiameter"></param>
        /// <param name="hasOuterDiameter">是否有外径的数据</param>
        /// <returns></returns>
        private double AdjustInnerDiameter(double innerDiameter, bool hasOuterDiameter)
        {
            double factor = innerDiameter / referencedDiameter;

            if (hasOuterDiameter)
                return RenderSize.Width * factor * 0.7;

            double adjustedDiameter;
            if (factor > 1.15)
            {
                if (factor > 1.3)
                    adjustedDiameter = RenderSize.Width * 1.3 * 0.7;
                else
                    adjustedDiameter = RenderSize.Width * factor * 0.7;
                if (++successiveOverrangeCount21 >= 5)
                {
                    successiveOverrangeCount21 = 0;
                    referencedDiameter = innerDiameter;
                }
            }
            else if (factor < 0.85 && factor > 0)
            {
                adjustedDiameter = RenderSize.Width * factor * 0.7;
                if (++successiveOverrangeCount22 >= 5)
                {
                    successiveOverrangeCount22 = 0;
                    referencedDiameter = innerDiameter;
                }
            }
            else
            {
                successiveOverrangeCount21 = 0;
                successiveOverrangeCount22 = 0;
                adjustedDiameter = RenderSize.Width * factor * 0.7;
            }

            return adjustedDiameter;
        }

		
        /// <summary>
        /// 使用动画改变内外经
        /// </summary>
        /// <param name="outerDiameter"></param>
        /// <param name="innerDiameter"></param>
        /// <param name="milliseconds"></param>
        private void SizeAnimation(double outerDiameter, double innerDiameter, double milliseconds)
        {
            Storyboard story = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromMilliseconds(milliseconds));

            DoubleAnimation outerWidthAnimation =
                new DoubleAnimation(outerDiameter, duration, FillBehavior.Stop);
            DoubleAnimation outerHeightAnimation =
                new DoubleAnimation(outerDiameter, duration, FillBehavior.Stop);
            DoubleAnimation innerWidthAnimation =
                new DoubleAnimation(innerDiameter, duration, FillBehavior.Stop);
            DoubleAnimation innerHeightAnimation =
                new DoubleAnimation(innerDiameter, duration, FillBehavior.Stop);

            double xTranslate = (outerDiameter + innerDiameter) / 4.0;
            if (outerDiameter <= 0 || innerDiameter <= 0)
                xTranslate = xTranslate * 2;
            DoubleAnimation xTransAnimation = 
                new DoubleAnimation(xTranslate, duration, FillBehavior.Stop);

            story.Children.Add(outerWidthAnimation);
            story.Children.Add(outerHeightAnimation);
            story.Children.Add(innerWidthAnimation);
            story.Children.Add(innerHeightAnimation);
            story.Children.Add(xTransAnimation);

            Storyboard.SetTarget(outerWidthAnimation, outerElipse);
            Storyboard.SetTarget(outerHeightAnimation, outerElipse);
            Storyboard.SetTarget(innerWidthAnimation, InnerImage);
            Storyboard.SetTarget(innerHeightAnimation, InnerImage);
            Storyboard.SetTarget(xTransAnimation, tbDelta);

            Storyboard.SetTargetProperty(outerWidthAnimation,
                new PropertyPath(FrameworkElement.WidthProperty));
            Storyboard.SetTargetProperty(outerHeightAnimation,
                new PropertyPath(FrameworkElement.HeightProperty));
            Storyboard.SetTargetProperty(innerWidthAnimation,
                new PropertyPath(FrameworkElement.WidthProperty));
            Storyboard.SetTargetProperty(innerHeightAnimation,
                new PropertyPath(FrameworkElement.HeightProperty));
            Storyboard.SetTargetProperty(xTransAnimation,
                new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            story.Completed += new EventHandler(delegate(object s1, EventArgs e1)
            {
                outerElipse.Width = outerElipse.Height = outerDiameter;
                InnerImage.Width = InnerImage.Height = innerDiameter;
                (tbDelta.RenderTransform as TranslateTransform).X = xTranslate;

                AnimationTask animationTask = null;
                lock (this)
                {
                    if (animationTasks.Count > 0)
                    {
                        animationTasks[0] = null;
                        animationTasks.RemoveAt(0);
                    }
                    if (animationTasks.Count > 0)
                        animationTask = animationTasks[0];
                }

                if (animationTask != null)
                    SetEillpseDiameter0(animationTask);
            });
            story.Begin();
        }

        private bool EnsureTemplateParts()
        {
            return outerElipse != null && InnerImage != null && tbOuterDiameter != null
                && tbInnerDiameter != null && tbDeltaDiameter != null;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            outerElipse = GetTemplateChild("PART_Outer") as FrameworkElement;
            InnerImage = GetTemplateChild("PART_Inner") as Image;
            tbDelta = GetTemplateChild("tbDelta") as TextBlock;
            tbOuterDiameter = GetTemplateChild("tbOuterDiameter") as TextBlock;
            tbInnerDiameter = GetTemplateChild("tbInnerDiameter") as TextBlock;
            tbDeltaDiameter = GetTemplateChild("tbDeltaDiameter") as TextBlock;

            InnerImage.Source =
                CtrlUtils.ChangeBitmapToImageSource(HuaWeiCtls.Properties.Resources.jhtCableCore);
            tbOuterDiameter.Text = tbInnerDiameter.Text = "无数据";
            tbDeltaDiameter.Text = GlobalConstants.NullOrEmptyReplacer;

            this.Loaded += JhtDiameterChart_Loaded;
        }

        private void JhtDiameterChart_Loaded(object sender, RoutedEventArgs e)
        {
            if (RenderSize.Width > RenderSize.Height)
                this.Width = this.RenderSize.Height;
            else
                this.Height = this.RenderSize.Width;
        }

        internal class AnimationTask
        {
            public double OuterValue { get; set; }
            public double InnerValue { get; set; }
            public double Duration { get; set; }

            public AnimationTask(double outerValue, double innerValue, double duration)
            {
                this.OuterValue = outerValue;
                this.InnerValue = innerValue;
                this.Duration = duration;
            }
        }
    }
}
