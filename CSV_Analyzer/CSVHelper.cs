using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSV_Analyzer
{
    class CSVHelper
    {
        const int indexName = 0;
        const int indexTime = 1;
        const int indexValue = 2;

        private string filePath;
        private int columnIndexName;
        private int columnIndexTime;
        private int columnIndexValue;

        public CSVHelper(string _filePath, int _columnIndexName, int _columnIndexTime, int _columnIndexValue)
        {
            filePath = _filePath;
            columnIndexName = _columnIndexName;
            columnIndexTime = _columnIndexTime;
            columnIndexValue = _columnIndexValue;
        }

        public List<Dataset> ImportCSV()
        {
            List<Dataset> datasets = new();
            int uniqueNamesNumber = 0;
            List<string> uniqueNames = new();

            var lines = File.ReadAllLines(filePath).Select(a => a.Split(';')).ToList(); 
            List<string[]> filteredLines = new();

            foreach (string[] line in lines)
            {
                string[] filteredLine = new string[3];
                filteredLine[0] = line[columnIndexName];
                filteredLine[1] = line[columnIndexTime];
                filteredLine[2] = line[columnIndexValue];
                filteredLines.Add(filteredLine);
            }

            foreach (string[] filteredLine in filteredLines)
            {
                if (!uniqueNames.Contains(filteredLine[indexName]))
                {
                    uniqueNames.Add(filteredLine[indexName]);
                    uniqueNamesNumber += 1;
                }
                else
                {
                    break;
                }
            }

            for (int i=0; i< uniqueNamesNumber; i++)
            {
                Dataset dataset = new(uniqueNames[i], filteredLines.Where(filteredLine => filteredLine[indexName] == uniqueNames[i]).ToList());
                datasets.Add(dataset);
            }

            return datasets;
        }
    }
}
