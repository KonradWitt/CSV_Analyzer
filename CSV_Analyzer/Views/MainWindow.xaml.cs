using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.ObjectModel;
using OxyPlot;
//using OxyPlot.Wpf;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace CSV_Analyzer
{
    public partial class MainWindow : INotifyPropertyChanged
    { 
        private string filePath;
        private string fileName = "Please select file";
        public string FileName
        {
            get { return fileName; }
            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    OnPropertyChanged();
                }
            }
        }

        private int columnIndexName = 0;
        public int ColumnIndexName
        {
            get { return columnIndexName; }
            set
            {
                if (columnIndexName != value)
                {
                    columnIndexName = value;
                    OnPropertyChanged();
                }
            }
        }

        private int columnIndexTime = 1;
        public int ColumnIndexTime
        {
            get { return columnIndexTime; }
            set
            {
                if (columnIndexTime != value)
                {
                    columnIndexTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private int columnIndexValue = 2;
        public int ColumnIndexValue
        {
            get { return columnIndexValue; }
            set
            {
                if (columnIndexValue != value)
                {
                    columnIndexValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Dataset> datasets;
        public ObservableCollection<Dataset> Datasets
        {
            get { return datasets;}
            set
            {
                if (datasets != value)
                {
                    datasets = value;
                    OnPropertyChanged();
                }
            }
        }


        private DateTime selectedTimeStart = DateTime.Now.AddHours(-1);
        public DateTime SelectedTimeStart
        {
            get { return selectedTimeStart; }
            set
            {
                if (selectedTimeStart != value)
                {
                    selectedTimeStart = value;
                    OnPropertyChanged();
                }               
            }
        }

        private DateTime selectedTimeEnd = DateTime.Now;
        public DateTime SelectedTimeEnd
        {
            get { return selectedTimeEnd; }
            set
            {
                if (selectedTimeEnd != value)
                {
                    selectedTimeEnd = value;
                    OnPropertyChanged();
                }
                
            }
        }

        public MainWindow()
        {
            DataContext = this;
            PlotModel = new PlotModel();
            setupPlotModel();
            InitializeComponent();
        }

        private void Button_ImportFile_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(filePath))
            {
                CSVHelper csvHelper = new(filePath, columnIndexName, columnIndexTime, columnIndexValue);
                Datasets = new ObservableCollection<Dataset>(csvHelper.ImportCSV());
            }
        }

        private void Button_SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Comma Separated Values Files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory(); 
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                FileName = filePath.Substring(filePath.LastIndexOf("\\"));
            }
        }


        private void ListBox_Variables_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            if (Datasets.Any(p => p.IsSelected == true))
            {
                SelectedTimeStart = Datasets.Where(p => p.IsSelected).First().TimeStart;
                SelectedTimeEnd = Datasets.Where(p => p.IsSelected).First().TimeEnd;
                
            }
            updatePlotModel();             
        }


        private void Button_ApplyTimeframes_Click(object sender, RoutedEventArgs e)
        {
            foreach (Dataset dataset in Datasets.Where(p => p.IsSelected == true))
            {
                dataset.TimeStart = selectedTimeStart;
                dataset.TimeEnd = selectedTimeEnd;
                dataset.UpdateTimeFrame(selectedTimeStart, selectedTimeEnd);
                dataset.UpdateStatistics();
            }

            if (Datasets.Any(p => p.IsSelected == true))
            {
                updatePlotModel();
            }
            
        }

        private PlotModel plotModel;
        public PlotModel PlotModel
        {
            get { return plotModel; }
            set
            {
                if (plotModel != value)
                {
                    plotModel = value;
                    OnPropertyChanged();
                }
            }
        }

        private void setupPlotModel()
        {
            PlotModel.LegendTitle = "Legend";
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.TopRight;
            PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            PlotModel.LegendBorder = OxyColors.Black;
            var dateAxis = new OxyPlot.Axes.DateTimeAxis() { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 30, StringFormat = "dd.MM.yyyy hh:mm", Angle = 45, Minimum = DateTimeAxis.ToDouble(selectedTimeStart), Maximum = DateTimeAxis.ToDouble(selectedTimeEnd)};
            PlotModel.Axes.Add(dateAxis);
            var valueAxis = new OxyPlot.Axes.LinearAxis() { MajorGridlineStyle = LineStyle.Solid, IntervalLength = 25, MinorGridlineStyle = LineStyle.Dot, Title = "Value" };
            PlotModel.Axes.Add(valueAxis);
            PlotModel.Series.Clear();
            PlotModel.InvalidatePlot(true);
        }

        private void updatePlotModel()
        {
            for (int i = PlotModel.Series.Count - 1; i >= 0; i--)
            {
                if (!Datasets.Where(p=>p.IsSelected==true).Any(p => p.Name == PlotModel.Series[i].Title))
                {
                    PlotModel.Series.RemoveAt(i);
                }
            }

            
                foreach (var selectedDataset in Datasets.Where(p => p.IsSelected == true))
                {
                    if (!PlotModel.Series.Any(p => p.Title == selectedDataset.Name))
                    {
                        var lineserie = new LineSeries();
                        foreach (var timestamp in selectedDataset.Times)
                        {
                            lineserie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(timestamp), selectedDataset.Values[selectedDataset.Times.IndexOf(timestamp)]));
                        }
                        lineserie.Title = selectedDataset.Name;
                        PlotModel.Series.Add(lineserie);
                    }
                }


            PlotModel.Axes[0].Minimum = DateTimeAxis.ToDouble(selectedTimeStart);
            PlotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(selectedTimeEnd);
            PlotModel.InvalidatePlot(true);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
