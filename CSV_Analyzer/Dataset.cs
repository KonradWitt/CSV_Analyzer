using System;
using System.Collections.Generic;
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

    public Dataset(string _name, List<string[]> _dataList)
        {
            Name = _name;
            DataList = _dataList;
            DataListDouble = generateDoubleList(DataList);
            calculateMin();
            calculateMax();
            calculateAvg();
        }

        private List<double> generateDoubleList(List<string[]> StringList)
        {
            List<double> DoubleList = new();
            foreach(string[] data in StringList)
            {
                DoubleList.Add(String2Double.GetDouble(data[indexValue], 0.0));
            }

            return DoubleList;
        }


        private void calculateMin()
        {
            MinValue = DataListDouble.Min();
        }

        private void calculateMax()
        {
            MaxValue = DataListDouble.Max();
        }

        private void calculateAvg()
        {
            AvgValue = DataListDouble.Average();
        }

    }
}
