using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    ///     <MyNamespace:FlipView/>
    ///
    /// </summary>
    public class FlipView : Selector
    {
        #region Private Fields
        private ContentControl PART_CurrentItem;
        private ContentControl PART_PreviousItem;
        private ContentControl PART_NextItem;
        private FrameworkElement PART_Root;
        private FrameworkElement PART_Container;
        private FrameworkElement PART_NextButton;
        private FrameworkElement PART_PreviousButton;
        private double fromValue = 0.0;
        #endregion

        #region
        static FlipView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(typeof(FlipView)));
            SelectedIndexProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(-1, OnSelectedIndexChanged));
        }

        public FlipView()
        {
            this.CommandBindings.Add(new CommandBinding(NextCommand, this.OnNextExecuted, this.OnNextCanExecute));
            this.CommandBindings.Add(new CommandBinding(PreviousCommand, this.OnPreviousExecuted, this.OnPreviousCanExecute));
        }
        #endregion

        #region Private methods
        private Storyboard GetAnimation(DependencyObject target, double to, double from)
        {
            Storyboard story = new Storyboard();
            Storyboard.SetTargetProperty(story, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            Storyboard.SetTarget(story, target);

            var doubleAnimation = new DoubleAnimationUsingKeyFrames();

            EasingDoubleKeyFrame fromFrame = new EasingDoubleKeyFrame(from);
            fromFrame.EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut };
            fromFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0));

            EasingDoubleKeyFrame toFrame = new EasingDoubleKeyFrame(to);
            toFrame.EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut };
            toFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200));

            doubleAnimation.KeyFrames.Add(fromFrame);
            doubleAnimation.KeyFrames.Add(toFrame);
            story.Children.Add(doubleAnimation);

            return story;
        }
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.RefreshViewPort(this.SelectedIndex);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if(this.SelectedIndex > -1)
            {
                this.RefreshViewPort(this.SelectedIndex);
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FlipView;

            control.OnSelectedIndexChanged(e);
        }

        private void OnSelectedIndexChanged(DependencyPropertyChangedEventArgs e)
        {
            if(!this.EnsureTemplateParts())
            {
                return;
            }
            if((int)e.NewValue >= 0 && (int)e.NewValue < this.Items.Count)
            {
                double toValue = (int)e.OldValue < (int)e.NewValue ? -this.ActualWidth : this.ActualWidth;
                this.RunSlideAnimation(toValue, fromValue);
            }
        }

        private void RefreshViewPort(int selectedIndex)
        {
            if(!this.EnsureTemplateParts())
            {
                return;
            }
            Canvas.SetLeft(this.PART_PreviousItem, -this.ActualWidth);
            Canvas.SetLeft(this.PART_NextItem, this.ActualWidth);
            this.PART_Root.RenderTransform = new TranslateTransform();

            var currentItem = this.GetItemAt(selectedIndex);
            var nextItem = this.GetItemAt(selectedIndex + 1);
            var previousItem = this.GetItemAt(selectedIndex - 1);

            this.PART_CurrentItem.Content = currentItem;
            this.PART_NextItem.Content = nextItem;
            this.PART_PreviousItem.Content = previousItem;
        }

        private void RunSlideAnimation(double toValue, double fromValue = 0)
        {
            if(!(this.PART_Root.RenderTransform is TranslateTransform))
            {
                this.PART_Root.RenderTransform = new TranslateTransform();
            }

            var story = GetAnimation(this.PART_Root, toValue, fromValue);
            story.Completed += (s, e) =>
            {
                this.RefreshViewPort(this.SelectedIndex);
            };
            story.Begin();
        }

        private object GetItemAt(int index)
        {
            if(index < 0 || index >= this.Items.Count)
            {
                return null;
            }

            return this.Items[index];
        }

        private bool EnsureTemplateParts()
        {
            return this.PART_CurrentItem != null &&
                this.PART_NextItem != null &&
                this.PART_PreviousItem != null &&
                this.PART_Root != null;
        }

        private void OnPreviousCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectedIndex > 0;
        }

        private void OnPreviousExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.SelectedIndex -= 1;
        }

        private void OnNextCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectedIndex < (this.Items.Count - 1);
        }

        private void OnNextExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.SelectedIndex += 1;
        }

        private void PART_Container_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = Mouse.GetPosition(this.PART_Container);

            if(pos.X < 50 && pos.Y >30 && this.SelectedIndex > 0)
            {
                PART_PreviousButton.Visibility = System.Windows.Visibility.Visible;
            }
            else if(pos.X > PART_Container.RenderSize.Width - 50
              && pos.Y >30 && this.SelectedIndex < (this.Items.Count - 1))
            {
                PART_NextButton.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                PART_PreviousButton.Visibility = System.Windows.Visibility.Hidden;
                PART_NextButton.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        #endregion

        #region Commands
        public static RoutedUICommand NextCommand = new RoutedUICommand("Next", "Next", typeof(FlipView));
        public static RoutedUICommand PreviousCommand = new RoutedUICommand("Previous", "Previous", typeof(FlipView));
        #endregion

        #region Override methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_PreviousItem = this.GetTemplateChild("PART_PreviousItem") as ContentControl;
            this.PART_NextItem = this.GetTemplateChild("PART_NextItem") as ContentControl;
            this.PART_CurrentItem = this.GetTemplateChild("PART_CurrentItem") as ContentControl;
            this.PART_Root = this.GetTemplateChild("PART_Root") as FrameworkElement;
            this.PART_Container = this.GetTemplateChild("PART_Container") as FrameworkElement;
            this.PART_PreviousButton = this.GetTemplateChild("PART_PreviousButton") as FrameworkElement;
            this.PART_NextButton = this.GetTemplateChild("PART_NextButton") as FrameworkElement;

            this.Loaded += this.OnLoaded;
            this.SizeChanged += this.OnSizeChanged;

            if(PART_PreviousButton != null && PART_NextButton != null)
            {
                this.PART_Container.AddHandler(Mouse.MouseMoveEvent,
                    new MouseEventHandler(PART_Container_MouseMove), true);
            }
        }
        #endregion
    }
}
