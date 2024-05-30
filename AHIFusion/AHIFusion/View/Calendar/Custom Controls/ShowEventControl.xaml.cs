namespace AHIFusion;
public sealed partial class ShowEventControl : UserControl
{
    public DayEvent Event;
    public ShowEventControl()
    {
        this.InitializeComponent();
    }

    public void UpdateEvent()
    {
        EventTitleTextBlock.Text = Event.Title;
    }
}
