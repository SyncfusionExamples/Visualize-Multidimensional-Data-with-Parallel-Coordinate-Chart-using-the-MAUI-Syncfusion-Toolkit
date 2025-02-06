using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParallelCoordinateChart
{
    public class ViewModel
    {
        public ObservableCollection<ChartModel> DataSource { get; set; }
        public ObservableCollection<FlightDataModel> Source { get; set; }

        public ViewModel()
        {
            Source = new ObservableCollection<FlightDataModel>(ReadCSV());
            DataSource = new ObservableCollection<ChartModel>();
            foreach (var data in Source)
            {
                DataSource.Add(new ChartModel(new List<object> { data }));
            }
        }

        private IEnumerable<FlightDataModel> ReadCSV()
        {
            Assembly executingAssembly = typeof(App).GetTypeInfo().Assembly;
            Stream? inputStream = executingAssembly.GetManifestResourceStream("ParallelCoordinateChart.Resources.Raw.data.csv");

            if (inputStream == null)
            {
                return Enumerable.Empty<FlightDataModel>(); 
            }

            List<FlightDataModel> flightDataList = new List<FlightDataModel>();

            using (StreamReader reader = new StreamReader(inputStream))
            {
                string? line;
                List<string> lines = new List<string>();

                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                foreach (var csvLine in lines.Skip(1))
                {
                    string[] data = csvLine.Split(',');

                    if (data.Length < 7) continue; 

                    bool success1 = double.TryParse(data[1], out double operations);
                    bool success2 = double.TryParse(data[2], out double percentOnTimeArrivals);
                    bool success3 = double.TryParse(data[3], out double percentLateArrivals);
                    bool success4 = double.TryParse(data[4], out double percentLateDepartures);
                    bool success5 = double.TryParse(data[5], out double percentCancelled);
                    bool success6 = double.TryParse(data[6], out double percentDiverted);

                    flightDataList.Add(new FlightDataModel
                    {
                        Year = data[0],
                        Operations = success1 ? operations : 0,
                        PercentOnTimeArrivals = success2 ? percentOnTimeArrivals : 0,
                        PercentLateArrivals = success3 ? percentLateArrivals : 0,
                        PercentLateDepartures = success4 ? percentLateDepartures : 0,
                        PercentCancelled = success5 ? percentCancelled : 0,
                        PercentDiverted = success6 ? percentDiverted : 0
                    });
                }
            }

            return flightDataList;
        }
    }
}
