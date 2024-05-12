using AHIFusion.Model;

namespace AHIFusion;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public partial class AlarmContent : Page
{
    public Alarm alarm1 { get; set; }

    public AlarmContent()
    {
        this.InitializeComponent();

        alarm1 = new Alarm("Test Alarm", new TimeOnly(15, 0), false);
        
        DataContext = this;
    }
}
