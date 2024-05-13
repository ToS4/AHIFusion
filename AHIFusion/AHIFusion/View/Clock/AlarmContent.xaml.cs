using AHIFusion.Model;
using Windows.Graphics.Capture;

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

        alarm1 = new Alarm("Joudi", new TimeOnly(8, 0), false);
        alarm1.Time = new TimeOnly(8, 0);

        alarm1.Title = "Fick dich";
        
        DataContext = this;
    }
}
