using Serilog;

namespace AHIFusion;
public sealed partial class ShowEventControl : UserControl
{
    public DayEvent Event;
    public ShowEventControl()
    {
        try 
        {
            this.InitializeComponent();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while initializing ShowEventControl");
            
        }
    }

    public void UpdateEvent()
    {
        try
        {
            EventTitleTextBlock.Text = Event.Title;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while updating event");
            
        }
    }
}
