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
using Microsoft.Phone.Controls;

namespace DemoMarqueeTextBlock
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
        }

        private void btStart_Click(object sender, RoutedEventArgs e)
        {
            marquee1.StartMarquee();
        }

        private void btStop_Click(object sender, RoutedEventArgs e)
        {
            marquee1.StopMarquee();
        }
    }
}