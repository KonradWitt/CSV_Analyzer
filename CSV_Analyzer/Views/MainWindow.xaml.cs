﻿using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

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

        private BindingList<Dataset> selectedDatasets = new();
        public BindingList<Dataset> SelectedDatasets
        {
            get { return selectedDatasets; }
            set
            {
                if (selectedDatasets != value)
                {
                    selectedDatasets = value;
                }
                OnPropertyChanged();
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
            InitializeComponent();
        }

        private void Button_ImportFile_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(filePath))
            {
                CSVHelper csvHelper = new(filePath, columnIndexName, columnIndexTime, columnIndexValue);
                Datasets = csvHelper.ImportCSV();
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
            foreach (Dataset dataset in datasets)
            {
                if (dataset.IsSelected && !selectedDatasets.Contains(dataset))
                {
                    SelectedDatasets.Add(dataset);
                }
                else if (!dataset.IsSelected && selectedDatasets.Contains(dataset))
                {
                    SelectedDatasets.Remove(dataset);
                }            
            }

            if (SelectedDatasets.Any())
            {
                SelectedTimeStart = SelectedDatasets[0].SelectedTimeStart;
                SelectedTimeEnd = SelectedDatasets[0].SelectedTimeEnd;
            }
        }
        private void Timeframe_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            foreach (Dataset dataset in SelectedDatasets)
            {
                dataset.SelectedTimeStart = selectedTimeStart;
                dataset.SelectedTimeEnd = selectedTimeEnd;
                dataset.UpdateStatistics();
            }
        }

        private void TextBox_TimeframeStart_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            foreach (Dataset dataset in SelectedDatasets)
            {
                dataset.SelectedTimeStart = selectedTimeStart;
                dataset.UpdateStatistics();
            }
        }

        private void TextBox_TimeframeEnd_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            foreach (Dataset dataset in SelectedDatasets)
            {
                dataset.SelectedTimeEnd = selectedTimeEnd;
                dataset.UpdateStatistics();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
