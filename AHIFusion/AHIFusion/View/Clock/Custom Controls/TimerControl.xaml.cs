using Microsoft.UI.Xaml.Input;
using Microsoft.UI;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI;
using System.Timers;
using Microsoft.UI.Xaml;
using Serilog;

namespace AHIFusion
{
    public sealed partial class TimerControl : UserControl
    {
        private System.Timers.Timer timer;
        private TimeOnly ringTime;
        private string ringTimeString;
        private MediaPlayer _mediaPlayer;

        public TimerControl()
        {
            try
            {
                Log.Information("Initializing TimerControl");

                this.InitializeComponent();

                timer = new System.Timers.Timer(1000); // Set interval to 1000 milliseconds (1 second)
                timer.Elapsed += Timer_Elapsed;
                timer.AutoReset = true;

                _mediaPlayer = new MediaPlayer();
                TimerSoundPlayer.SetMediaPlayer(_mediaPlayer);

                RingBColor = new SolidColorBrush(Colors.DarkSlateGray);
                RingFColor = new SolidColorBrush(Colors.DarkSlateGray);

                this.Loaded += TimerControl_Loaded;
                this.Unloaded += TimerControl_Unloaded;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
            }

        }

        private void TimerControl_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Elapsed -= Timer_Elapsed;
            timer.Stop();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Log.Information("Timer_Elapsed has been called");

                Log.Debug("Use DispatcherQueue to update the UI");
                DispatcherQueue.TryEnqueue(() =>
                {
                    if (Time.TotalSeconds > 0)
                    {
                        Time -= TimeSpan.FromSeconds(1);
                        RingValue = (Time.TotalSeconds / InitialTime) * 100;
                    }
                    else
                    {
                        RingTimer();
                        IsRunning = false;
                        timer.Stop();
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
            }
        }

        private void TimerControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("TimerControl_Loaded has been called");

                timer.Elapsed -= Timer_Elapsed;
                timer.Elapsed += Timer_Elapsed;
                if (IsRunning && !timer.Enabled)
                {
                    timer.Start();
                }

                RingValue = (Time.TotalSeconds / InitialTime) * 100;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
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

        public static readonly DependencyProperty InitialTimeProperty = DependencyProperty.Register("InitialTime", typeof(double), typeof(TimerControl), new PropertyMetadata(60.0));
        public double InitialTime
        {
            get { return (double)GetValue(InitialTimeProperty); }
            set { SetValue(InitialTimeProperty, value); }
        }

        public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register("IsRunning", typeof(bool), typeof(TimerControl), new PropertyMetadata(false));
        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }

        public static readonly DependencyProperty SoundProperty = DependencyProperty.Register("Sound", typeof(string), typeof(TimerControl), new PropertyMetadata("Timer.mp3"));
        public string Sound
        {
            get { return (string)GetValue(SoundProperty); }
            set { SetValue(SoundProperty, value); }
        }

        public static readonly DependencyProperty RingValueProperty = DependencyProperty.Register("RingValue", typeof(double), typeof(TimerControl), new PropertyMetadata(100.0));
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
            if (Time.TotalSeconds == 0)
            {
                return;
            }
            if (IsRunning)
            {
                StopTimer();
            }
            else
            {
                StartTimer();
            }
        }

        private void StartTimer()
        {
            try
            {
                Log.Information("StartTimer has been called");

                CalculateRingTime();
                RingTimeTextBlock.Text = ringTimeString;
                RingTimeBorder.Visibility = Visibility.Visible;
                RingTimeTextBlock.Visibility = Visibility.Visible;
                IsRunning = true;

                if (!timer.Enabled)
                {
                    timer.Start();
                }

                RingFColor = new SolidColorBrush(Colors.AliceBlue);
                TextColor = new SolidColorBrush(Colors.Black);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
            }
        }

        private void StopTimer()
        {
            try
            {
                Log.Information("StopTimer has been called");

                RingTimeBorder.Visibility = Visibility.Collapsed;
                RingTimeTextBlock.Visibility = Visibility.Collapsed;
                IsRunning = false;
                timer.Stop();
                RingFColor = new SolidColorBrush(Color.FromArgb(255, 220, 228, 235));
                TextColor = new SolidColorBrush(Color.FromArgb(255, 70, 70, 70));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
            Time = TimeSpan.FromSeconds(InitialTime);
            RingValue = (Time.TotalSeconds / InitialTime) * 100;
        }

        private void CalculateRingTime()
        {
            TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
            ringTime = now.AddMinutes(Time.TotalSeconds / 60);
            ringTimeString = ringTime.ToString("hh:mm");
        }

        private void RingTimer()
        {
            try
            {
                Log.Information("RingTimer has been called");

                _mediaPlayer.Source = MediaSource.CreateFromUri(new Uri($"ms-appx:///Assets/Sounds/Timers/{Sound}"));
                _mediaPlayer.Play();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
            }
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            rect.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 149, 149, 149));
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            rect.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 169, 169, 169));
        }

        private async void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (this.DataContext is AHIFusion.Model.Timer timer)
            {
                StopTimer();
                EditTimer editTimerDialog = new EditTimer(timer)
                {
                    XamlRoot = this.XamlRoot
                };
                await editTimerDialog.ShowAsync();
            }
        }
    }
}
