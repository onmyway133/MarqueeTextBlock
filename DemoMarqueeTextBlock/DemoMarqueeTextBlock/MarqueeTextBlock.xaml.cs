using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DemoMarqueeTextBlock
{
    public partial class MarqueeTextBlock : UserControl
    {
        // storyboard
        Storyboard storyboard;

        // offset when textBlock1 is long, because user want to see smooth scrolling
        private double offset = 0;

        // default offset
        private double defaultOffset = 100;

        // default velocity
        int velocity = 0;

        // whether control has beed loaded
        // because it's when control is loaded do we start marquee
        bool isLoaded = false;

        public MarqueeTextBlock()
        {
            InitializeComponent();

            Loaded += MarqueeTextBlock_Loaded;
            Unloaded += MarqueeTextBlock_Unloaded;
        }

        // unload event
        void MarqueeTextBlock_Unloaded(object sender, RoutedEventArgs e)
        {
            // stop marquee when control is unloaded
            StopMarquee();
        }

        // set data context in Loaded
        void MarqueeTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                // control is loaded
                isLoaded = true;

                LayoutRoot.DataContext = this;

                // resize the clipping
                Rect rect = new Rect(0, 0, canvas.ActualWidth, canvas.ActualHeight);
                RectangleGeometry reo = new RectangleGeometry();
                reo.Rect = rect;
                this.canvas.Clip = reo;

                // always vertical alignment
                ChangeOffsetTop();

                // this is OK for marquee to start
                StartMarquee();
            }
            else
            {
                // just run it, after user come to this page again
                StartMarquee();
            }
        }


        public void StartMarquee()
        {
            // first stop all marquee
            StopMarquee();

            if (ShouldStartMarquee())
            {
                // change offset
                ChangeOffsetLeft(true);

                DoAnimationFirst();
            }
            else
            {
                // change offset
                ChangeOffsetLeft(false);
            }
        }

        public void StopMarquee()
        {
            if (storyboard != null)
            {
                storyboard.Stop();
            }
        }

        bool ShouldStartMarquee()
        {
            return textBlock1.ActualWidth > LayoutRoot.ActualWidth;
        }


        // after 
        void storyboard_Completed(object sender, EventArgs e)
        {
            DoAnimationAfter();

            // remeber to unsubscribe
            storyboard.Completed -= storyboard_Completed;
        }


        ////////////////////////////////////////////////////////////////////////////
        // calculate offset
        void ChangeOffsetLeft(bool isLong)
        {
            if (isLong)
            {
                offset = defaultOffset;
                Canvas.SetLeft(textBlock1, offset);
            }
            else
            {
                offset = (canvas.ActualWidth - textBlock1.ActualWidth) / 2;
                Canvas.SetLeft(textBlock1, offset);
            }
        }

        // center vertical
        void ChangeOffsetTop()
        {
            double topOffset = (canvas.ActualHeight - textBlock1.ActualHeight) / 2;
            Canvas.SetTop(textBlock1, topOffset);
        }

        int CalculateDurationFirst()
        {
            int duration = 4000;



            return duration;
        }

        int CalculateDurationAfter()
        {
            int duration = 6000;



            return duration;
        }


        void DoAnimationFirst()
        {
            storyboard = new Storyboard();
            TranslateTransform trans = new TranslateTransform() { X = 5.0, Y = 1.0 };
            textBlock1.RenderTransformOrigin = new Point(0.5, 0.5);
            textBlock1.RenderTransform = trans;

            // we must calculate the Duration
            DoubleAnimation moveAnim = new DoubleAnimation();
            int durationFirst = CalculateDurationFirst();
            moveAnim.Duration = TimeSpan.FromMilliseconds(durationFirst);

            moveAnim.From = 0;
            moveAnim.To = -textBlock1.ActualWidth - offset;

            Storyboard.SetTarget(moveAnim, textBlock1);
            Storyboard.SetTargetProperty(moveAnim, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            // subscribe to Completed event
            storyboard.Completed += new EventHandler(storyboard_Completed);

            storyboard.Children.Add(moveAnim);
            storyboard.FillBehavior = FillBehavior.HoldEnd;

            storyboard.Begin();
        }



        void DoAnimationAfter()
        {
            storyboard = new Storyboard();
            TranslateTransform trans = new TranslateTransform() { X = 5.0, Y = 1.0 };
            textBlock1.RenderTransformOrigin = new Point(0.5, 0.5);
            textBlock1.RenderTransform = trans;

            // we must calculate the Duration
            DoubleAnimation moveAnim = new DoubleAnimation();
            int durationAfter = CalculateDurationAfter();
            moveAnim.Duration = TimeSpan.FromMilliseconds(durationAfter);

            moveAnim.From = canvas.ActualWidth;
            moveAnim.To = -textBlock1.ActualWidth - offset;

            Storyboard.SetTarget(moveAnim, textBlock1);
            Storyboard.SetTargetProperty(moveAnim, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            storyboard.Children.Add(moveAnim);
            storyboard.RepeatBehavior = RepeatBehavior.Forever;
            storyboard.FillBehavior = FillBehavior.HoldEnd;

            storyboard.Begin();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // dependency property
        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(MarqueeTextBlock), null);

        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set
            {
                this.SetValue(TextProperty, value);

                // this doesnot marquee for the first time
                // because textBlock1.ActualWidth is 0 !!
                if (isLoaded)
                {
                    StartMarquee();
                }

            }
        }

        public static readonly DependencyProperty MFontSizeProperty = DependencyProperty.Register("MFontSize",
        typeof(int), typeof(MarqueeTextBlock), null);

        public int MFontSize
        {
            get { return (int)this.GetValue(MFontSizeProperty); }
            set
            {
                this.SetValue(MFontSizeProperty, value);
            }
        }
    }
}
