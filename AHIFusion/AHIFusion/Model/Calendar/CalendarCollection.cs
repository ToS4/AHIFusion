using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHIFusion;
internal class CalendarCollection
{
    public static ObservableCollection<CalendarDay> Days { get; set; } = new ObservableCollection<CalendarDay>();

    public static void Add(CalendarDay day)
    {
        Days.Add(day);
    }
    public static void Remove(CalendarDay day)
    {
        Days.Remove(day);
    }
}
