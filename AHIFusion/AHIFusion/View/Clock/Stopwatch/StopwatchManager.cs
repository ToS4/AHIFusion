using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using AHIFusion.Model;

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

        public void SaveToFile(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // For pretty printing
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string jsonString = JsonSerializer.Serialize(LapList, options);
            File.WriteAllText(filePath, jsonString);
        }

        public void LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                LapList = JsonSerializer.Deserialize<ObservableCollection<string>>(jsonString);
            }
        }
    }
}
