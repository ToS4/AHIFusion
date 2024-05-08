using System;
using System.Collections.Generic;
namespace AHIFusion;
public sealed partial class AlarmControl : UserControl
{
    public AlarmControl()
    {
        this.InitializeComponent();
    }

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(AlarmControl), new PropertyMetadata(""));

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }
}
