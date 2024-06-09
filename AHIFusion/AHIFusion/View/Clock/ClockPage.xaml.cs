using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Serilog;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AHIFusion;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ClockPage : Page
{
    public static object? SelectedItem { get; set; }

    public ClockPage()
    {
        try
        {
            Log.Information("Initializing ClockPage");

            this.InitializeComponent();

            SelectedItem = ClockNavigation.MenuItems[1];
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        try
        {
            Log.Information("OnNavigatedTo has been called");

            base.OnNavigatedTo(e);

            if (SelectedItem != null)
            {
                ClockNavigation.SelectedItem = SelectedItem;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }

        
    }

    private void ClockNavigation_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            Log.Information("ClockNavigation_Loaded has been called");

            ClockNavigation.SelectedItem = SelectedItem;

            ContentFrame.Navigated += On_Navigated;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }

    private void On_Navigated(object sender, NavigationEventArgs e)
    {
        try
        {
            Log.Information("On_Navigated has been called");

            if (ContentFrame.SourcePageType == typeof(AlarmContent))
            {
                ClockNavigation.SelectedItem = ClockNavigation.MenuItems[1];
            }
            else if (ContentFrame.SourcePageType == typeof(StopwatchContent))
            {
                ClockNavigation.SelectedItem = ClockNavigation.MenuItems[3];
            }
            else if (ContentFrame.SourcePageType == typeof(TimerContent))
            {
                ClockNavigation.SelectedItem = ClockNavigation.MenuItems[5];
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }

    private void ClockNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        try
        {
            Log.Information("ClockNavigation_SelectionChanged has been called");

            SelectedItem = args.SelectedItem;

            if (args.IsSettingsSelected)
            {
                return;
            }
            else if (args.SelectedItemContainer != null)
            {
                Type pageType = Type.GetType(args.SelectedItemContainer.Tag.ToString());
                ClockNavigation_Navigate(pageType, args.RecommendedNavigationTransitionInfo);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }

    private void ClockNavigation_Navigate(Type pageType, NavigationTransitionInfo transitionInfo)
    {
        try
        {
            Log.Information("ClockNavigation_Navigate has been called");

            Type preNavigationPageType = ContentFrame.CurrentSourcePageType;

            if (pageType is not null && pageType != preNavigationPageType)
            {
                ContentFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }
}
