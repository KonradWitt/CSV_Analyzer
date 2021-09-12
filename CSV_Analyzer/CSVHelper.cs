using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSV_Analyzer
{
    class CsvHelper
    {
        public Dictionary<string, Dataset> ImportCSV(string filePath, int columnIndexName, int columnIndexTime, int columnIndexValue)
        {
            List<string[]> lines = File.ReadAllLines(filePath).Select(a => a.Split(';')).ToList();

            var datasets = new Dictionary<string, Dataset>();

            foreach (string[] line in lines)
            {
                string name = line[columnIndexName];
                string time = line[columnIndexTime];
                string value = line[columnIndexValue];

                if (datasets.ContainsKey(name))
                {
                    var dataset = datasets[name];

                    dataset.AddTime(time);
                    dataset.AddValue(value);
                }
                else
                {
                    datasets.Add(name, new Dataset(name, time, value));
                }
            }

            return datasets;
        }
    }
}
