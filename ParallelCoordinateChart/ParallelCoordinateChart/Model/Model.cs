using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelCoordinateChart
{
    public class FlightDataModel
    {
        public string? Year { get; set; }
        public double Operations { get; set; }
        public double PercentOnTimeArrivals { get; set; }
        public double PercentLateArrivals { get; set; }
        public double PercentLateDepartures { get; set; }
        public double PercentCancelled { get; set; }
        public double PercentDiverted { get; set; }
    }
}
