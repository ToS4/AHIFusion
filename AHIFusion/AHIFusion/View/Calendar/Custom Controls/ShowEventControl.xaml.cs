namespace AHIFusion;
public sealed partial class ShowEventControl : UserControl
{
    public ShowEventControl()
    {
        this.InitializeComponent();
    }

    public static readonly DependencyProperty EventTextProperty = DependencyProperty.Register("EventText", typeof(string), typeof(ShowEventControl), new PropertyMetadata(""));
    public string EventText
    {
        get { return (string)GetValue(EventTextProperty); }
        set { SetValue(EventTextProperty, value); }
    }
}
