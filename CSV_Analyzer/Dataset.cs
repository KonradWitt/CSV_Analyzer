using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CSV_Analyzer
{
    public class Dataset : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isSelected;
        private List<double> values;
        private List<DateTime> times;
        private double minValue;
        private double maxValue;
        private double avgValue;
        private DateTime timeStart;
        private DateTime timeEnd;

        public Dataset(string _name, string _time, string _value)
        {
            values = new List<double>();
            times = new List<DateTime>();
            Name = _name;
            AddTime(_time);
            AddValue(_value);
            OnPropertyChanged("TimeStart");
            OnPropertyChanged("TimeEnd");
            UpdateTimeFrame(TimeStart, TimeEnd);
        }

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

        public string Name { get; }

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

        public double MinValue
        {
            get { return minValue; }
            private set
            {
                if (minValue != value)
                {
                    minValue = value;
                    OnPropertyChanged();
                }
            }
        }

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


        public DateTime TimeStart
        {
            get { return timeStart; }
            set
            {
                if (timeStart != value)
                {
                    timeStart = value;
                    OnPropertyChanged();
                }
            }
        }

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

        internal void AddValue(string value)
        {
            values.Add(String2Double.GetDouble(value, 0));
        }

        internal void AddTime(string time)
        {
            if (DateTime.TryParse(time, out DateTime t))
            {
                times.Add(t);
            }
        }

        public void UpdateTimeFrame(DateTime timeStart, DateTime timeEnd)
        {
            int[] indexes = DateTimeFrame2Indexes.GetIndexes(timeStart, timeEnd, times);
            int startIndex = indexes[0];
            int count = indexes[1] - indexes[0] + 1;
            MinValue = values.GetRange(startIndex, count).Min();
            MaxValue = values.GetRange(startIndex, count).Max();
            AvgValue = Math.Round(values.GetRange(startIndex, count).Average(), 2);
        }

        public void CheckTimeFrame()
        {
            if (times.Count > 1)
            {
                timeStart = times.Min();
                timeEnd = times.Max();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


