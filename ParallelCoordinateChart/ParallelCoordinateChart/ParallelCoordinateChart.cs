using Syncfusion.Maui.Toolkit.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelCoordinateChart
{
    public class ParallelCoordinateChart : SfCartesianChart
    {
        public ParallelCoordinateChart()
        {
            GenerateChart();
        }

        private void GenerateChart()
        {
            var xAxes = GenerateXAxes();
            var yAxes = GenerateYAxesList();
            this.XAxes.Add(xAxes);
            foreach (var yaxis in yAxes)
            {
                this.YAxes.Add(yaxis);
            }

            var paletteBrushes = new List<Brush>
                   {
                       new SolidColorBrush(Color.FromArgb("#800080")),
                       new SolidColorBrush(Color.FromArgb("#6495ED")),
                       new SolidColorBrush(Color.FromArgb("#32CD32")),
                       new SolidColorBrush(Color.FromArgb("#FFD700")),
                       new SolidColorBrush(Color.FromArgb("#FF6347")),
                       new SolidColorBrush(Color.FromArgb("#8A2BE2")),
                   };
            this.PaletteBrushes = paletteBrushes;

            var seriesList = GenerateSeries(yAxes);
            foreach (var series in seriesList)
            {
                this.Series.Add(series);
            }
        }

        private NumericalAxis GenerateXAxes()
        {
            var xAxes = new NumericalAxis()
            {
                Minimum = 0,
                Maximum = 6, 
                Interval = 1,
                ShowMajorGridLines = false,
                PlotOffsetStart = 50,
                PlotOffsetEnd = 50,
                CrossesAt = double.MaxValue,
                EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Center,
                MajorTickStyle = new ChartAxisTickStyle()
                {
                    
                    StrokeWidth = 0,
                    
                },
                AxisLineStyle = new ChartLineStyle()
                {
                    StrokeWidth = 0,
                   
                }
            };

            xAxes.LabelCreated += (s, e) =>
            {
                e.Label = e.Position switch
                {
                    0 => "Year",
                    1 => "Operations",
                    2 => "On-Time Arrivals",
                    3 => "Late Arrivals",
                    4 => "Late Departures",
                    5 => "Cancelled",
                    6 => "Diverted",
                    _ => string.Empty
                };
            };

            return xAxes;
        }

        private List<NumericalAxis> GenerateYAxesList()
        {
            var viewModelData = new ViewModel().Source;
            var firstItem = viewModelData.First();
            var properties = firstItem.GetType().GetProperties();
            var list = new List<NumericalAxis>();

            foreach (var property in properties)
            {
                var yAxis = new NumericalAxis()
                {
                    ShowMajorGridLines = false,
                    LabelsPosition = AxisElementPosition.Outside,
                };

                switch (property.Name)
                {
                    case "Year":
                        yAxis.Minimum = 0;
                        yAxis.Maximum = 28;
                        yAxis.Interval = 4;
                        yAxis.CrossesAt = 0;
                        yAxis.EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Fit;

                        string[] years = Enumerable.Range(1995, 29).Select(y => y.ToString()).ToArray();

                        yAxis.LabelCreated += (s, e) =>
                        {
                            if (e.Position >= 0 && e.Position < years.Length)
                            {
                                e.Label = years[(int)e.Position];
                            }
                            else
                            {
                                e.Label = string.Empty;
                            }
                        };
                        break;

                    case "Operations":
                        yAxis.Minimum = 4600000;
                        yAxis.Maximum = 7500000;
                        yAxis.Interval = 500000;
                        yAxis.CrossesAt = 1;
                        yAxis.LabelStyle = new ChartAxisLabelStyle()
                        {
                            LabelFormat = "#,##0,K"
                        };
                        yAxis.EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Fit;
                        yAxis.EdgeLabelsVisibilityMode = EdgeLabelsVisibilityMode.AlwaysVisible;
                        break;

                    case "PercentOnTimeArrivals":
                        yAxis.Minimum = 70;
                        yAxis.Maximum = 90;
                        yAxis.Interval = 5;
                        yAxis.CrossesAt = 2;
                        yAxis.EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Fit;
                        yAxis.EdgeLabelsVisibilityMode = EdgeLabelsVisibilityMode.AlwaysVisible;
                        break;

                    case "PercentLateArrivals":
                        yAxis.Minimum = 8;
                        yAxis.Maximum = 26;
                        yAxis.Interval = 2;
                        yAxis.CrossesAt = 3;
                        yAxis.EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Fit;
                        yAxis.EdgeLabelsVisibilityMode = EdgeLabelsVisibilityMode.AlwaysVisible;
                        break;

                    case "PercentLateDepartures":
                        yAxis.Minimum = 8;
                        yAxis.Maximum = 22;
                        yAxis.Interval = 2;
                        yAxis.CrossesAt = 4;
                        yAxis.EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Fit;
                        yAxis.EdgeLabelsVisibilityMode = EdgeLabelsVisibilityMode.AlwaysVisible;
                        break;

                    case "PercentCancelled":
                        yAxis.Minimum = 1;
                        yAxis.Maximum = 7;
                        yAxis.Interval = 1;
                        yAxis.CrossesAt = 5;
                        yAxis.EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Fit;
                        yAxis.EdgeLabelsVisibilityMode = EdgeLabelsVisibilityMode.AlwaysVisible;
                        break;

                    case "PercentDiverted":
                        yAxis.Minimum = 0.1;
                        yAxis.Maximum = 0.3;
                        yAxis.Interval = 0.02;
                        yAxis.CrossesAt = 6;
                        yAxis.LabelsPosition = AxisElementPosition.Inside;
                        yAxis.TickPosition = AxisElementPosition.Inside;
                        yAxis.EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Fit;
                        yAxis.EdgeLabelsVisibilityMode = EdgeLabelsVisibilityMode.AlwaysVisible;
                        break;
                }

                list.Add(yAxis);
            }
            return list;
        }

        private List<LineSeries> GenerateSeries(List<NumericalAxis> yAxes)
        {
            var viewModel = new ViewModel();
            var seriesList = new List<LineSeries>();

            foreach (var chartModel in viewModel.DataSource)
            {
                var itemSource = new ObservableCollection<SeriesModel>();

                for (int axisIndex = 0; axisIndex < yAxes.Count; axisIndex++)
                {
                    double yValue = 0;

                    if (chartModel.Variable.Count == 0) continue; 
                    var model = chartModel.Variable[0] as FlightDataModel;
                    if (model == null) continue; 

                    switch (axisIndex)
                    {
                        case 0:
                            yValue = Normalize(YearIndex(model.Year), 0, 28);
                            break;
                        case 1:
                            yValue = Normalize(model.Operations, 4600000, 7500000);
                            break;
                        case 2:
                            yValue = Normalize(model.PercentOnTimeArrivals, 70, 90);
                            break;
                        case 3:
                            yValue = Normalize(model.PercentLateArrivals, 8, 26);
                            break;
                        case 4:
                            yValue = Normalize(model.PercentLateDepartures, 8, 22);
                            break;
                        case 5:
                            yValue = Normalize(model.PercentCancelled, 1, 7);
                            break;
                        case 6:
                            yValue = Normalize(model.PercentDiverted, 0.1, 0.3);
                            break;
                    }
                    itemSource.Add(new SeriesModel(axisIndex, yValue));
                }

                var series = new LineSeries()
                {
                    ItemsSource = itemSource,
                    XBindingPath = nameof(SeriesModel.XValues),
                    YBindingPath = nameof(SeriesModel.YValues),
                };

                seriesList.Add(series);
            }

            return seriesList;
        }

        private double Normalize(double value, double min, double max)
        {
            if (max == min) return 0; 
            return (((value - min) / (max - min)) * 28);
        }

        private int YearIndex(string year)
        {
            return int.TryParse(year, out int yearInt) ? yearInt - 1995 : -1;
        }
    }
}
