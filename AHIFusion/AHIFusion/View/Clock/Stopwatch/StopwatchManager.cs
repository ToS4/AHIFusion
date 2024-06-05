using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHIFusion
{
    public class StopwatchManager
    {
        public System.Diagnostics.Stopwatch StopwatchInstance { get; private set; }
        public ObservableCollection<string> LapList { get; private set; }

        private static StopwatchManager? _instance;
        public static StopwatchManager Instance => _instance ?? (_instance = new StopwatchManager());

        private StopwatchManager()
        {
            StopwatchInstance = new System.Diagnostics.Stopwatch();
            LapList = new ObservableCollection<string>();
        }
    }
}
