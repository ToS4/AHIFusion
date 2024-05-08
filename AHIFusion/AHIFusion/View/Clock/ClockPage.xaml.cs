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
using Windows.Foundation;
using Windows.Foundation.Collections;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AHIFusion;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ClockPage : Page
{
    public ClockPage()
    {
        this.InitializeComponent();
    }

    private void ClockNavigation_Loaded(object sender, RoutedEventArgs e)
    {

        ClockNavigation.SelectedItem = ClockNavigation.MenuItems[1];

        ContentFrame.Navigated += On_Navigated;

    }

    private void On_Navigated(object sender, NavigationEventArgs e)
    {
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
        else if (ContentFrame.SourcePageType == typeof(WorldClockContent))
        {
            ClockNavigation.SelectedItem = ClockNavigation.MenuItems[7];
        }
    }

    private void ClockNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
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

    private void ClockNavigation_Navigate(Type pageType, NavigationTransitionInfo transitionInfo)
    {
        Type preNavigationPageType = ContentFrame.CurrentSourcePageType;

        if (pageType is not null && pageType != preNavigationPageType)
        {
            ContentFrame.Navigate(pageType, null, transitionInfo);
        }
    }
}
