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
using Microsoft.UI;
using Windows.UI;
using System.Diagnostics;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AHIFusion
{
	public sealed partial class TimerControl : UserControl
	{
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private double initialTime;

        public TimerControl()
        {
            this.InitializeComponent();

            initialTime = Time.TotalSeconds;

            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;

            RingBColor = new SolidColorBrush(Colors.DarkSlateGray);
            RingFColor = new SolidColorBrush(Colors.DarkSlateGray);
            RingValue = (Time.TotalSeconds / initialTime) * 100;
        }

        private void DispatcherTimer_Tick(object? sender, object e)
        {
            if (Time.TotalSeconds > 0)
            {
                Time -= TimeSpan.FromSeconds(1);
                RingValue = (Time.TotalSeconds / initialTime) * 100;
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

        public static readonly DependencyProperty RingValueProperty = DependencyProperty.Register("RingValue", typeof(double), typeof(TimerControl), new PropertyMetadata(100));
        public double RingValue
        {
            get { return (double)GetValue(RingValueProperty); }
            set { SetValue(RingValueProperty, value); }
        }

        public static readonly DependencyProperty RingBColorProperty = DependencyProperty.Register("RingBColor", typeof(SolidColorBrush), typeof(TimerControl), new PropertyMetadata(new SolidColorBrush(Colors.DarkSlateGray)));
        public SolidColorBrush RingBColor
        {
            get { return (SolidColorBrush)GetValue(RingBColorProperty); }
            set { SetValue(RingBColorProperty, value); }
        }

        public static readonly DependencyProperty RingFColorProperty = DependencyProperty.Register("RingFColor", typeof(SolidColorBrush), typeof(TimerControl), new PropertyMetadata(new SolidColorBrush(Colors.AliceBlue)));
        public SolidColorBrush RingFColor
        {
            get { return (SolidColorBrush)GetValue(RingFColorProperty); }
            set { SetValue(RingFColorProperty, value); }
        }

        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register("TextColor", typeof(SolidColorBrush), typeof(TimerControl), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        public SolidColorBrush TextColor
        {
            get { return (SolidColorBrush)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
            {
                IsRunning = false;
                dispatcherTimer.Stop();
                RingFColor = new SolidColorBrush(Color.FromArgb(255, 220, 228, 235));
                TextColor = new SolidColorBrush(Color.FromArgb(255, 70, 70, 70));
            }
            else
            {
                IsRunning = true;
                dispatcherTimer.Start();
                RingFColor = new SolidColorBrush(Colors.AliceBlue);
                TextColor = new SolidColorBrush(Colors.Black);
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Time = TimeSpan.FromSeconds(initialTime);
            IsRunning = false;
            dispatcherTimer.Stop();
            RingFColor = new SolidColorBrush(Color.FromArgb(255, 220, 228, 235));
            TextColor = new SolidColorBrush(Color.FromArgb(255, 70, 70, 70));
            RingValue = (Time.TotalSeconds / initialTime) * 100;
        }
    }
}
