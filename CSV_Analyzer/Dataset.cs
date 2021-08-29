using System.Collections.Generic;


namespace CSV_Analyzer
{
    public class Dataset
    {
        const int indexName = 0;
        const int indexTime = 1;
        const int indexValue = 2;
        public bool IsSelected
        {
            get;
            set;
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
        public Dataset(string _name, List<string[]> _dataList)
        {
            Name = _name;
            DataList = _dataList;
        }
    }
}
