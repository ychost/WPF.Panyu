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
    ///     <MyNamespace:MetroProgressBar/>
    ///
    /// </summary>
    /// <summary>
    /// A metrofied ProgressBar.
    /// <see cref="ProgressBar"/>
    /// </summary>
    public class MetroProgressBar : ProgressBar
    {
        public static readonly DependencyProperty EllipseDiameterProperty =
            DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(MetroProgressBar),
                                        new PropertyMetadata(default(double)));

        public static readonly DependencyProperty EllipseOffsetProperty =
            DependencyProperty.Register("EllipseOffset", typeof(double), typeof(MetroProgressBar),
                                        new PropertyMetadata(default(double)));

        private readonly object lockme = new object();
        private Storyboard indeterminateStoryboard;

        static MetroProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroProgressBar), new FrameworkPropertyMetadata(typeof(MetroProgressBar)));
            IsIndeterminateProperty.OverrideMetadata(typeof(MetroProgressBar), new FrameworkPropertyMetadata(OnIsIndeterminateChanged));
        }

        private void VisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            //reset Storyboard if Visibility is set to Visible #1300
            if (IsIndeterminate)
            {
                ToggleIndeterminate(this, (bool)e.OldValue, (bool)e.NewValue);
            }
        }

        private static void OnIsIndeterminateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ToggleIndeterminate(dependencyObject as MetroProgressBar, (bool)e.OldValue, (bool)e.NewValue);
        }

        private static void ToggleIndeterminate(MetroProgressBar bar, bool oldValue, bool newValue)
        {
            if (bar != null && newValue != oldValue)
            {
                var indeterminateState = bar.GetIndeterminate();
                var containingObject = bar.GetTemplateChild("ContainingGrid") as FrameworkElement;
                if (indeterminateState != null && containingObject != null)
                {
                    if (oldValue && indeterminateState.Storyboard != null)
                    {
                        // remove the previous storyboard from the Grid #1855
                        indeterminateState.Storyboard.Stop(containingObject);
                        indeterminateState.Storyboard.Remove(containingObject);
                    }
                    if (newValue)
                    {
                        bar.ResetStoryboard(bar.ActualWidth, false);
                    }
                }
            }
        }

        /// <summary>
        /// Gets/sets the diameter of the ellipses used in the indeterminate animation.
        /// </summary>
        public double EllipseDiameter
        {
            get { return (double)GetValue(EllipseDiameterProperty); }
            set { SetValue(EllipseDiameterProperty, value); }
        }

        /// <summary>
        /// Gets/sets the offset of the ellipses used in the indeterminate animation.
        /// </summary>
        public double EllipseOffset
        {
            get { return (double)GetValue(EllipseOffsetProperty); }
            set { SetValue(EllipseOffsetProperty, value); }
        }

        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            var actualWidth = ActualWidth;
            var bar = this;
            if (this.Visibility == Visibility.Visible && this.IsIndeterminate)
            {
                bar.ResetStoryboard(actualWidth, true);
            }
        }

        private void ResetStoryboard(double width, bool removeOldStoryboard)
        {
            if (!this.IsIndeterminate)
            {
                return;
            }
            lock (this.lockme)
            {
                //perform calculations
                var containerAnimStart = CalcContainerAnimStart(width);
                var containerAnimEnd = CalcContainerAnimEnd(width);
                var ellipseAnimWell = CalcEllipseAnimWell(width);
                var ellipseAnimEnd = CalcEllipseAnimEnd(width);
                //reset the main double animation
                try
                {
                    var indeterminate = GetIndeterminate();

                    if (indeterminate != null && this.indeterminateStoryboard != null)
                    {
                        var newStoryboard = this.indeterminateStoryboard.Clone();
                        var doubleAnim = newStoryboard.Children.First(t => t.Name == "MainDoubleAnim");
                        doubleAnim.SetValue(DoubleAnimation.FromProperty, containerAnimStart);
                        doubleAnim.SetValue(DoubleAnimation.ToProperty, containerAnimEnd);

                        var namesOfElements = new[] { "E1", "E2", "E3", "E4", "E5" };
                        foreach (var elemName in namesOfElements)
                        {
                            var doubleAnimParent = (DoubleAnimationUsingKeyFrames)newStoryboard.Children.First(t => t.Name == elemName + "Anim");
                            DoubleKeyFrame first, second, third;
                            if (elemName == "E1")
                            {
                                first = doubleAnimParent.KeyFrames[1];
                                second = doubleAnimParent.KeyFrames[2];
                                third = doubleAnimParent.KeyFrames[3];
                            }
                            else
                            {
                                first = doubleAnimParent.KeyFrames[2];
                                second = doubleAnimParent.KeyFrames[3];
                                third = doubleAnimParent.KeyFrames[4];
                            }

                            first.Value = ellipseAnimWell;
                            second.Value = ellipseAnimWell;
                            third.Value = ellipseAnimEnd;
                            first.InvalidateProperty(DoubleKeyFrame.ValueProperty);
                            second.InvalidateProperty(DoubleKeyFrame.ValueProperty);
                            third.InvalidateProperty(DoubleKeyFrame.ValueProperty);

                            doubleAnimParent.InvalidateProperty(Storyboard.TargetPropertyProperty);
                            doubleAnimParent.InvalidateProperty(Storyboard.TargetNameProperty);
                        }

                        var containingGrid = (FrameworkElement)GetTemplateChild("ContainingGrid");

                        if (removeOldStoryboard && indeterminate.Storyboard != null)
                        {
                            // remove the previous storyboard from the Grid #1855
                            indeterminate.Storyboard.Stop(containingGrid);
                            indeterminate.Storyboard.Remove(containingGrid);
                        }

                        indeterminate.Storyboard = newStoryboard;

                        if (indeterminate.Storyboard != null)
                        {
                            indeterminate.Storyboard.Begin(containingGrid, true);
                        }
                    }
                }
                catch (Exception)
                {
                    //we just ignore 
                }
            }
        }

        private VisualState GetIndeterminate()
        {
            var templateGrid = GetTemplateChild("ContainingGrid") as FrameworkElement;
            if (templateGrid == null)
            {
                return null;
            }
            var groups = VisualStateManager.GetVisualStateGroups(templateGrid);
            return groups != null
                ? groups.Cast<VisualStateGroup>()
                        .SelectMany(@group => @group.States.Cast<VisualState>())
                        .FirstOrDefault(state => state.Name == "Indeterminate")
                : null;
        }

        private void SetEllipseDiameter(double width)
        {
            if (width <= 180)
            {
                EllipseDiameter = 4;
                return;
            }
            if (width <= 280)
            {
                EllipseDiameter = 5;
                return;
            }

            EllipseDiameter = 6;
        }

        private void SetEllipseOffset(double width)
        {
            if (width <= 180)
            {
                EllipseOffset = 4;
                return;
            }
            if (width <= 280)
            {
                EllipseOffset = 7;
                return;
            }

            EllipseOffset = 9;
        }

        private double CalcContainerAnimStart(double width)
        {
            if (width <= 180)
            {
                return -34;
            }
            if (width <= 280)
            {
                return -50.5;
            }

            return -63;
        }

        private double CalcContainerAnimEnd(double width)
        {
            var firstPart = 0.4352 * width;
            if (width <= 180)
            {
                return firstPart - 25.731;
            }
            if (width <= 280)
            {
                return firstPart + 27.84;
            }

            return firstPart + 58.862;
        }

        private double CalcEllipseAnimWell(double width)
        {
            return width * 1.0 / 3.0;
        }

        private double CalcEllipseAnimEnd(double width)
        {
            return width * 2.0 / 3.0;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            lock (this.lockme)
            {
                this.indeterminateStoryboard = this.TryFindResource("IndeterminateStoryboard") as Storyboard;
            }

            Loaded -= LoadedHandler;
            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= LoadedHandler;
            SizeChangedHandler(null, null);
            SizeChanged += SizeChangedHandler;
            IsVisibleChanged += VisibleChangedHandler;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Update the Ellipse properties to their default values
            // only if they haven't been user-set.
            if (EllipseDiameter.Equals(0))
            {
                SetEllipseDiameter(ActualWidth);
            }
            if (EllipseOffset.Equals(0))
            {
                SetEllipseOffset(ActualWidth);
            }
        }
    }
}
