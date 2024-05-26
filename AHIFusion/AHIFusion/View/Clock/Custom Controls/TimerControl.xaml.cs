using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Uno.Extensions.ValueType;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AHIFusion
{
	public sealed partial class TimerControl : UserControl
	{
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();


        public TimerControl()
        {
            this.InitializeComponent();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
        }

        private void DispatcherTimer_Tick(object? sender, object e)
        {
            if (Time.TotalSeconds > 0)
            {
                Time -= TimeSpan.FromSeconds(1);
            }
            else
            {
                IsRunning = false;
                dispatcherTimer.Stop();
            }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(TimerControl), new PropertyMetadata(""));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(TimeSpan), typeof(TimerControl), new PropertyMetadata(new TimeSpan(0, 1, 0)));
        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register("IsRunning", typeof(bool), typeof(TimerControl), new PropertyMetadata(false));
        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }

        public static readonly DependencyProperty SoundProperty = DependencyProperty.Register("Sound", typeof(string), typeof(TimerControl), new PropertyMetadata(""));
        public string Sound
        {
            get { return (string)GetValue(SoundProperty); }
            set { SetValue(SoundProperty, value); }
        }

        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
            {
                IsRunning = false;
                dispatcherTimer.Stop();
            }
            else
            {
                IsRunning = true;
                dispatcherTimer.Start();
            }
        }
    }
}
