using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AHIFusion;
public sealed partial class TodoListControl : UserControl, INotifyPropertyChanged
{
    private ObservableCollection<Todo> Todos { get; set; }
    public bool IsNew { get; set; }

    public TodoListControl(bool isnew)
    {
        IsNew = isnew;
        this.InitializeComponent();
        this.Loaded += TodoListControl_Loaded;
        this.RightTapped += TodoListControl_RightTapped;
    }

    private void TodoListControl_RightTapped(object sender, RightTappedRoutedEventArgs e)
    {
        ShowCommandBarFlyout();
    }

    private void TodoListControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (IsNew)
        {
            this.DispatcherQueue.TryEnqueue(() =>
            {
                FocusOnTextBox();
            });
            IsNew = false;
        }
    }

    public void FocusOnTextBox()
    {
        NameTextBlock.Visibility = Visibility.Collapsed;
        NameTextBox.Visibility = Visibility.Visible;
        NameTextBox.Focus(FocusState.Programmatic);
        NameTextBox.SelectAll();
    }

    private void NameTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            NameTextBlock.Visibility = Visibility.Visible;
            NameTextBox.Visibility = Visibility.Collapsed;
            Name = NameTextBox.Text;
        }
    }

    private void ShowCommandBarFlyout()
    {
        var flyout = new CommandBarFlyout();

        var renamButton = new AppBarButton
        {
            Icon = new SymbolIcon(Symbol.Rename),
            Label = "Rename"
        };
        renamButton.Click += RenameButton_Click;

        var deleteButton = new AppBarButton
        {
            Icon = new SymbolIcon(Symbol.Delete),
            Label = "Delete"
        };
        deleteButton.Click += DeleteButton_Click;

        flyout.PrimaryCommands.Add(renamButton);
        flyout.PrimaryCommands.Add(deleteButton);

        flyout.ShowAt(this);
    }

    private void RenameButton_Click(object sender, RoutedEventArgs e)
    {
        FocusOnTextBox();
    }

    public event EventHandler DeleteClicked;
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        TodoList currentTodoList = this.DataContext as TodoList;
        TodoCollection.TodoLists.Remove(currentTodoList);

        DeleteClicked?.Invoke(this, EventArgs.Empty);
    }

    public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(TodoListControl), new PropertyMetadata(""));
    public string Name
    {
        get { return (string)GetValue(NameProperty); }
        set
        {
            SetValue(NameProperty, value);
            OnPropertyChanged("Name");
        }
    }

    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(TodoListControl), new PropertyMetadata(Color.FromArgb(255,255,255,255)));
    public Color Color
    {
        get { return (Color)GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }

    public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(TodoListControl), new PropertyMetadata(0));
    public int Id
    {
        get { return (int)GetValue(IdProperty); }
        set { SetValue(IdProperty, value); }
    }

    public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(TodoListControl), new PropertyMetadata(false, OnIsSelectedChanged));
    public bool IsSelected
    {
        get { return (bool)GetValue(IsSelectedProperty); }
        set { SetValue(IsSelectedProperty, value); }
    }

    private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TodoListControl control)
        {
            control.UpdateSelectedColor();
        }
    }

    private void UpdateSelectedColor()
    {
        if (IsSelected)
        {
            BackgroundRect.Fill= new SolidColorBrush(Color.FromArgb(255, 94, 109, 124));
        }
        else
        {
            BackgroundRect.Fill = new SolidColorBrush(Color.FromArgb(255, 119, 136, 153));
        }
    }

    private void NameTextBox_FocusDisengaged(Control sender, FocusDisengagedEventArgs args)
    {
        NameTextBlock.Visibility = Visibility.Visible;
        NameTextBox.Visibility = Visibility.Collapsed;
    }

    private void Ellipse_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        Flyout flyout = new Flyout();
        ColorPicker colorPicker = new ColorPicker()
        {
            ColorSpectrumShape = ColorSpectrumShape.Ring,
            IsColorPreviewVisible = false,
            IsColorChannelTextInputVisible = false,
            IsHexInputVisible = false
        };
        flyout.Content = colorPicker;

        flyout.FlyoutPresenterStyle = new Style(typeof(FlyoutPresenter));
        flyout.FlyoutPresenterStyle.Setters.Add(new Setter(BackgroundProperty, new SolidColorBrush(Colors.White)));

        flyout.Closed += (s, e) =>
        {
            Color = colorPicker.Color;
        };

        flyout.ShowAt(sender as FrameworkElement);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
