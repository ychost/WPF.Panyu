using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace HuaWeiUtils
{
    public class AnimationUtil
    {
        /// <summary>
        /// 使用动画Zoom数据展示控件,使用时目标控件一定要有RenderTransform
        /// 对象，且RenderTransform对象中只有TranslateTransform这一个对象
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="pFrom"></param>
        /// <param name="sTo"></param>
        /// <param name="container"></param>
        /// <param name="handler"></param>
        /// <param name="seconds"></param>
        public static void ZoomCtrlFromPoint(FrameworkElement ctrl, Point pFrom, Size sTo, 
            FrameworkElement container, EventHandler handler, double seconds = 0.3)
        {
            Storyboard story = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(seconds));

            DoubleAnimation widthAnimation = new DoubleAnimation(0, sTo.Width, duration, FillBehavior.Stop);
            DoubleAnimation heightAnimation = new DoubleAnimation(0, sTo.Height, duration, FillBehavior.Stop);
            DoubleAnimation xTransAnimation = new DoubleAnimation(pFrom.X, 0, duration, FillBehavior.Stop);
            DoubleAnimation yTransAnimation = new DoubleAnimation(pFrom.Y, 0, duration, FillBehavior.Stop);

            story.Children.Add(widthAnimation);
            story.Children.Add(heightAnimation);
            story.Children.Add(xTransAnimation);
            story.Children.Add(yTransAnimation);

            Storyboard.SetTargetName(widthAnimation, ctrl.Name);
            Storyboard.SetTargetName(heightAnimation, ctrl.Name);
            Storyboard.SetTargetName(xTransAnimation, ctrl.Name);
            Storyboard.SetTargetName(yTransAnimation, ctrl.Name);

            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(FrameworkElement.WidthProperty));
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(FrameworkElement.HeightProperty));
            Storyboard.SetTargetProperty(xTransAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            Storyboard.SetTargetProperty(yTransAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            story.Completed += handler;

            story.Begin(container);
        }

        /// <summary>
        /// 使用动画最小化数据展示控件,使用时目标控件一定要有RenderTransform
        /// 对象，且RenderTransform对象中只有TranslateTransform这一个对象
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="pTo"></param>
        /// <param name="sFrom"></param>
        /// <param name="container"></param>
        /// <param name="handler"></param>
        /// <param name="seconds"></param>
        public static void MinimizeCtrlToPoint(FrameworkElement ctrl, Point pTo, Size sFrom,
            FrameworkElement container, EventHandler handler, double seconds = 0.3)
        {
            Storyboard story = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(seconds));

            DoubleAnimation widthAnimation = new DoubleAnimation(sFrom.Width, 0, duration, FillBehavior.Stop);
            DoubleAnimation heightAnimation = new DoubleAnimation(sFrom.Height, 0, duration, FillBehavior.Stop);

            DoubleAnimation xTransAnimation = new DoubleAnimation(0, pTo.X, duration, FillBehavior.Stop);
            DoubleAnimation yTransAnimation = new DoubleAnimation(0, pTo.Y, duration, FillBehavior.Stop);

            story.Children.Add(widthAnimation);
            story.Children.Add(heightAnimation);
            story.Children.Add(xTransAnimation);
            story.Children.Add(yTransAnimation);

            Storyboard.SetTargetName(widthAnimation, ctrl.Name);
            Storyboard.SetTargetName(heightAnimation, ctrl.Name);
            Storyboard.SetTargetName(xTransAnimation, ctrl.Name);
            Storyboard.SetTargetName(yTransAnimation, ctrl.Name);

            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(FrameworkElement.WidthProperty));
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(FrameworkElement.HeightProperty));
            Storyboard.SetTargetProperty(xTransAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            Storyboard.SetTargetProperty(yTransAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            story.Completed += handler;

            story.Begin(container);
        }
    }
}
