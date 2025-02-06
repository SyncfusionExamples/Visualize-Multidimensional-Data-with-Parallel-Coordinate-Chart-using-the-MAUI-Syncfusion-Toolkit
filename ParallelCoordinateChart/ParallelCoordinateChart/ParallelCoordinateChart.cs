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
                        yAxis.LabelCreated += (s, e) =>
                        {
                            e.Label = e.Position switch
                            {
                                0 => "1995",
                                1 => "1996",
                                2 => "1997",
                                3 => "1998",
                                4 => "1999",
                                5 => "2000",
                                6 => "2001",
                                7 => "2002",
                                8 => "2003",
                                9 => "2004",
                                10 => "2005",
                                11 => "2006",
                                12 => "2007",
                                13 => "2008",
                                14 => "2009",
                                15 => "2010",
                                16 => "2011",
                                17 => "2012",
                                18 => "2013",
                                19 => "2014",
                                20 => "2015",
                                21 => "2016",
                                22 => "2017",
                                23 => "2018",
                                24 => "2019",
                                25 => "2020",
                                26 => "2021",
                                27 => "2022",
                                28 => "2023",
                                _ => string.Empty 
                            };
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
            return year switch
            {
                "1995" => 0,
                "1996" => 1,
                "1997" => 2,
                "1998" => 3,
                "1999" => 4,
                "2000" => 5,
                "2001" => 6,
                "2002" => 7,
                "2003" => 8,
                "2004" => 9,
                "2005" => 10,
                "2006" => 11,
                "2007" => 12,
                "2008" => 13,
                "2009" => 14,
                "2010" => 15,
                "2011" => 16,
                "2012" => 17,
                "2013" => 18,
                "2014" => 19,
                "2015" => 20,
                "2016" => 21,
                "2017" => 22,
                "2018" => 23,
                "2019" => 24,
                "2020" => 25,
                "2021" => 26,
                "2022" => 27,
                "2023" => 28,
                _ => -1 
            };
        }
    }
}
