using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CSV_Analyzer
{
    public class Dataset : INotifyPropertyChanged
    {
        const int indexName = 0;
        const int indexTime = 1;
        const int indexValue = 2;
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            private set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }


        private List<double> values;
        public List<double> Values
        {
            get { return values; }
            private set
            {
                if (values != value)
                {
                    values = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<DateTime> times;
        public List<DateTime> Times
        {
            get { return times; }
            private set 
            { 
            if (times != value)
                {
                    times = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<double> selectedValues;
        public List<double> SelectedValues
        {
            get { return selectedValues; }
            set
            {
                if(selectedValues!=value)
                {
                    selectedValues = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<DateTime> selectedTimes;
        public List<DateTime> SelectedTimes
        {
            get { return selectedTimes; }
            set 
            {
                if (selectedTimes != value)
                {
                    selectedTimes = value;
                    OnPropertyChanged();
                }
            }
        }

        private double minValue;
        public double MinValue
        {
            get { return minValue; }
            private set {
                if (minValue != value)
                {
                    minValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private double maxValue;
        public double MaxValue
        {
            get { return maxValue; }
            private set
            {
                if (maxValue != value)
                {
                    maxValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private double avgValue;
        public double AvgValue
        {
            get { return avgValue; }
            private set
            {
                if (avgValue != value)
                {
                    avgValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime timeStart;

        public DateTime TimeStart
        {
            get { return timeStart; }
            set { 
                if (timeStart != value)
                {
                    timeStart = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime timeEnd;
        public DateTime TimeEnd
        {
            get { return timeEnd; }
            set
            {
                if (timeEnd != value)
                {
                    timeEnd = value;
                    OnPropertyChanged();
                }
            }
        }

        public Dataset(string _name, List<DateTime> _times, List<double> _values)
        {
            name = _name;
            times = _times;
            selectedTimes = times;
            values = _values;
            selectedValues = values;
            checkTimeframe();
            TimeStart = timeStart;
            TimeEnd = timeEnd;
            UpdateStatistics();
        }


        public void UpdateStatistics()
        {
            MinValue = selectedValues.Min();
            MaxValue = selectedValues.Max();
            AvgValue = Math.Round(selectedValues.Average(), 2);
        }

        public void UpdateTimeFrame(DateTime timeStart, DateTime timeEnd)
        {
            int[] indexes = DateTimeFrame2Indexes.GetIndexes(timeStart, timeEnd, times);
            int startIndex = indexes[0];
            int count = indexes[1] - indexes[0] + 1;
            selectedTimes = times.GetRange(startIndex, count);
            selectedValues = values.GetRange(startIndex, count);
        }

        private void checkTimeframe()
        {
            timeStart = times.Min(p => p);
            timeEnd = times.Max(p => p);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


