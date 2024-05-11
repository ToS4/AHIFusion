using AHIFusion.Model;

namespace AHIFusion;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public partial class AlarmContent : Page
{
    public Alarm alarm1 = new Alarm("TestAlarm", new TimeOnly(7, 0));

    public AlarmContent()
    {
        this.InitializeComponent();

        DataContext = this;

        AlarmControl alarmControl = new AlarmControl()
        {
            Title = alarm1.Title,
            Time = new TimeOnly(3,0),
            IsOn = true
        };

        

        MainGrid.Children.Add(alarmControl);

        Grid.SetRow(alarmControl, 1);
        Grid.SetColumn(alarmControl, 1);
    }
}
