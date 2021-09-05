using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CSV_Analyzer
{
    public class Dataset
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
                }
            }
        }

        public string Name
        {
            get;
            private set;
        }
        public List<string[]> DataList
        {
            get;
            set;
        }

        public List<double> DataListDouble
        {
            get;
            private set;
        }

        public double MinValue
        {
            get;
            private set;
        }
        public double MaxValue
        {
            get;
            private set;
        }

        public double AvgValue
        {
            get;
            private set;
        }

        private DateTime timeStart;

        public DateTime SelectedTimeStart
        {
            get;
            set;
        }


        private DateTime timeEnd;
        public DateTime SelectedTimeEnd
        {
            get;
            set;
        }

        public Dataset(string _name, List<string[]> _dataList)
        {
            Name = _name;
            DataList = _dataList;
            DataListDouble = generateDoubleList(DataList);
            checkTimeframe();
            SelectedTimeStart = timeStart;
            SelectedTimeEnd = timeEnd;
            calculateMin(timeStart, timeEnd);
            calculateMax(timeStart, timeEnd);
            calculateAvg(timeStart, timeEnd);
        }

        private List<double> generateDoubleList(List<string[]> StringList)
        {
            List<double> DoubleList = new();
            foreach (string[] data in StringList)
            {
                DoubleList.Add(String2Double.GetDouble(data[indexValue], 0.0));
            }

            return DoubleList;
        }


        private void calculateMin(DateTime timeStart, DateTime timeEnd)
        {
            int[] indexes = timeframe2Index(timeStart, timeEnd);
            int startIndex = indexes[0];
            int count = indexes[1]-indexes[0]+1;
            MinValue = DataListDouble.GetRange(startIndex, count).Min();
        }

        private void calculateMax(DateTime timeStart, DateTime timeEnd)
        {
            int[] indexes = timeframe2Index(timeStart, timeEnd);
            int startIndex = indexes[0];
            int count = indexes[1] - indexes[0] + 1;
            MaxValue = DataListDouble.GetRange(startIndex, count).Max();
        }

        private void calculateAvg(DateTime timeStart, DateTime timeEnd)
        {
            int[] indexes = timeframe2Index(timeStart, timeEnd);
            int startIndex = indexes[0];
            int count = indexes[1] - indexes[0] + 1;
            AvgValue = Math.Round(DataListDouble.GetRange(startIndex, count).Average(),2);
        }

        public void UpdateStatistics()
        {
            calculateMin(SelectedTimeStart, SelectedTimeEnd);
            calculateMax(SelectedTimeStart, SelectedTimeEnd);
            calculateAvg(SelectedTimeStart, SelectedTimeEnd);
        }

        private void checkTimeframe()
        {
            List<DateTime> timeList = new List<DateTime>();

            foreach (string[] data in DataList)
            {
                timeList.Add(DateTime.Parse(data[indexTime]));
            }

            timeStart = timeList.Min(p => p);
            timeEnd = timeList.Max(p => p);
        }

        private int[] timeframe2Index(DateTime timeStart, DateTime timeEnd)
        {
            int[] TimeframeIndexes = {0, 0};
            DateTime checkedDateTime;
            for(int i=0; i<DataList.Count; i++)
            {
                checkedDateTime = DateTime.Parse(DataList[i][indexTime]);
                if (checkedDateTime >= timeStart && TimeframeIndexes[0] == 0)
                {
                    TimeframeIndexes[0] = i;
                }
                if (checkedDateTime <= timeEnd)
                {
                    TimeframeIndexes[1] = i;
                }
            }
            return TimeframeIndexes;
        }
    }
}


