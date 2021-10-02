using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.ObjectModel;
using OxyPlot;
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
            get { return datasets; }
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
                var csvHelper = new CsvHelper();

                Datasets = new ObservableCollection<Dataset>(csvHelper.ImportCSV(filePath, columnIndexName, columnIndexTime, columnIndexValue).Values);
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
            foreach (Dataset dataset in Datasets.Where(p => p.IsSelected))
            {
                if (Datasets.Where(p => p.IsSelected).Count() == 1)
                {
                    dataset.CheckTimeFrame();
                    SelectedTimeStart = dataset.TimeStart;
                    SelectedTimeEnd = dataset.TimeEnd;
                }                 
                dataset.UpdateTimeFrame(SelectedTimeStart, SelectedTimeEnd);
                updatePlotModel(dataset);
            }
            
            
        }


        private void Button_ApplyTimeframes_Click(object sender, RoutedEventArgs e)
        {
            if (Datasets != null && Datasets.Any(p => p.IsSelected))
            {
                foreach (Dataset dataset in Datasets.Where(p => p.IsSelected))
                {
                    dataset.TimeStart = selectedTimeStart;
                    dataset.TimeEnd = selectedTimeEnd;
                    dataset.UpdateTimeFrame(selectedTimeStart, selectedTimeEnd);
                    updatePlotModel(dataset);
                }
            }

        }

        private void DataGrid_VariablesPreprocessing_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            if (Datasets != null && Datasets.Any(p => p.IsSelected))
            {
                foreach (Dataset dataset in Datasets.Where(p => p.IsSelected))
                {
                    if (e.Row.Item.Equals(dataset))
                    { 
                    dataset.TimeStart = selectedTimeStart;
                    dataset.TimeEnd = selectedTimeEnd;
                    dataset.PreprocessValues();
                    dataset.UpdateTimeFrame(selectedTimeStart, selectedTimeEnd);
                    updatePlotModel(dataset);
                    }
                }
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
            PlotModel.LegendPosition = LegendPosition.RightMiddle;
            PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            PlotModel.LegendBorder = OxyColors.Black;
            var dateAxis = new OxyPlot.Axes.DateTimeAxis() { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 30, StringFormat = "dd.MM.yyyy hh:mm", Angle = 55, Minimum = DateTimeAxis.ToDouble(selectedTimeStart), Maximum = DateTimeAxis.ToDouble(selectedTimeEnd), Font = "Helvetica", FontWeight = 500, TitleFont = "Helvetica", TitleFontWeight = 500 };
            PlotModel.Axes.Add(dateAxis);
            var valueAxis = new OxyPlot.Axes.LinearAxis() { MajorGridlineStyle = LineStyle.Solid, IntervalLength = 25, MinorGridlineStyle = LineStyle.Dot, Font = "Helvetica", FontWeight = 500, TitleFont = "Helvetica", TitleFontWeight = 500 };
            PlotModel.Axes.Add(valueAxis);
            PlotModel.Series.Clear();
            PlotModel.InvalidatePlot(true);
        }

        private void updatePlotModel(Dataset changedDataset)
        {
            for (int i = PlotModel.Series.Count - 1; i >= 0; i--)
            {
                if (!Datasets.Where(p => p.IsSelected).Any(p => p.Name == PlotModel.Series[i].Title))
                {
                    PlotModel.Series.RemoveAt(i);
                }
            }
            if (PlotModel.Series.Any(p => p.Title == changedDataset.Name))
            {
                PlotModel.Series.Remove(PlotModel.Series.First(p => p.Title == changedDataset.Name));
            }
            foreach (var selectedDataset in Datasets.Where(p => p.IsSelected))
            {
                if (!PlotModel.Series.Any(p => p.Title == selectedDataset.Name))
                {
                    var lineserie = new LineSeries();
                    foreach (var timestamp in selectedDataset.Times)
                    {
                        lineserie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(timestamp), selectedDataset.PreprocessedValues[selectedDataset.Times.IndexOf(timestamp)]));
                    }
                    lineserie.Title = selectedDataset.Name;
                    lineserie.CanTrackerInterpolatePoints = false;
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
