using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AHIFusion;
public sealed partial class CustomTimePicker : UserControl
{
    public CustomTimePicker()
    {
        this.InitializeComponent();
    }

    public TimeSpan Time
    {
        get
        {
            return new TimeSpan(
                int.Parse(hoursTextBox.Text),
                int.Parse(minutesTextBox.Text),
                int.Parse(secondsTextBox.Text));
        }
        set
        {
            hoursTextBox.Text = value.Hours.ToString("D2");
            minutesTextBox.Text = value.Minutes.ToString("D2");
            secondsTextBox.Text = value.Seconds.ToString("D2");
        }
    }

    public event EventHandler<TimeSpan> TimeChanged;

    private void OnTimeChanged()
    {
        TimeChanged?.Invoke(this, Time);
    }

    private void IncreaseHour(object sender, RoutedEventArgs e)
    {
        int hours = (int.Parse(hoursTextBox.Text) + 1) % 100;
        hoursTextBox.Text = hours.ToString("D2");
        OnTimeChanged();
    }

    private void DecreaseHour(object sender, RoutedEventArgs e)
    {
        int hours = (int.Parse(hoursTextBox.Text) - 1 + 100) % 100;
        hoursTextBox.Text = hours.ToString("D2");
        OnTimeChanged();
    }

    private void IncreaseMinute(object sender, RoutedEventArgs e)
    {
        int minutes = (int.Parse(minutesTextBox.Text) + 1) % 60;
        minutesTextBox.Text = minutes.ToString("D2");
        OnTimeChanged();
    }

    private void DecreaseMinute(object sender, RoutedEventArgs e)
    {
        int minutes = (int.Parse(minutesTextBox.Text) - 1 + 60) % 60;
        minutesTextBox.Text = minutes.ToString("D2");
        OnTimeChanged();
    }

    private void IncreaseSecond(object sender, RoutedEventArgs e)
    {
        int seconds = (int.Parse(secondsTextBox.Text) + 1) % 60;
        secondsTextBox.Text = seconds.ToString("D2");
        OnTimeChanged();
    }

    private void DecreaseSecond(object sender, RoutedEventArgs e)
    {
        int seconds = (int.Parse(secondsTextBox.Text) - 1 + 60) % 60;
        secondsTextBox.Text = seconds.ToString("D2");
        OnTimeChanged();
    }

    private void HoursTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            ValidateAndCorrectTextBox(hoursTextBox, 24);
        }
    }

    private void MinutesTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            ValidateAndCorrectTextBox(minutesTextBox, 60);
        }
    }

    private void SecondsTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            ValidateAndCorrectTextBox(secondsTextBox, 60);
        }
    }

    private void HoursTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        ValidateAndCorrectTextBox(hoursTextBox, 24);
    }

    private void MinutesTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        ValidateAndCorrectTextBox(minutesTextBox, 60);
    }

    private void SecondsTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        ValidateAndCorrectTextBox(secondsTextBox, 60);
    }

    private void ValidateAndCorrectTextBox(TextBox textBox, int maxValue)
    {
        if (int.TryParse(textBox.Text, out int value))
        {
            if (value < 0)
            {
                textBox.Text = "00";
            }
            else if (value >= maxValue)
            {
                textBox.Text = (maxValue - 1).ToString("D2");
            }
            else
            {
                textBox.Text = value.ToString("D2");
            }
        }
        else
        {
            textBox.Text = "00";
        }
        OnTimeChanged();
    }

    // ich mach das nie wieder :D
}
