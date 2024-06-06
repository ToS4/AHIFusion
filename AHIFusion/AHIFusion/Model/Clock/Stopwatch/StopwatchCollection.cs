using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AHIFusion.Model;

namespace AHIFusion;
public static class StopwatchCollection
{
    public static ObservableCollection<Stopwatch> Stopwatches { get; set; } = new ObservableCollection<Stopwatch>();

    public static void Add(Stopwatch stopwatch)
    {
        Stopwatches.Add(stopwatch);
    }

    public static void Remove(Stopwatch stopwatch)
    {
        Stopwatches.Remove(stopwatch);
    }

    //weil ich nur eine Stopwatch will
    public static Stopwatch sw = new Stopwatch();
}
