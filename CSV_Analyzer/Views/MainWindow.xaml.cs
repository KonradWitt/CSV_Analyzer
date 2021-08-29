using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

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

        private List<Dataset> datasets;
        public List <Dataset> Datasets
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

        private List<Dataset> selectedDatasets = new();



        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void Button_ImportFile_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(filePath))
            {
                CSVHelper csvHelper = new(filePath, columnIndexName, columnIndexTime, columnIndexValue);
                Datasets = csvHelper.ImportCSV();
            }
            var datetime = DateTime.Parse("29.08.2021 15:23");
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ListBox_Variables_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            foreach (Dataset dataset in datasets)
            {
                if (dataset.IsSelected && !selectedDatasets.Contains(dataset))
                {
                    selectedDatasets.Add(dataset);
                }
                else if (!dataset.IsSelected && selectedDatasets.Contains(dataset))
                {
                    selectedDatasets.Remove(dataset);
                }            
            }
        }
    }
}
