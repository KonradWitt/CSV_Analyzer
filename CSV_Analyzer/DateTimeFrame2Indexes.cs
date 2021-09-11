using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Analyzer
{
    static class DateTimeFrame2Indexes
    {
        public static int[] GetIndexes (DateTime timeStart, DateTime timeEnd, List<DateTime> dateTimes)
        {
            int[] TimeframeIndexes = { 0, 0 };
            bool minFound = false;
            DateTime checkedDateTime;
            for (int i = 0; i < dateTimes.Count; i++)
            {
                checkedDateTime = dateTimes[i];
                if (checkedDateTime >= timeStart && !minFound)
                {
                    TimeframeIndexes[0] = i;
                    minFound = true;
                }
                if (checkedDateTime <= timeEnd)
                {
                    TimeframeIndexes[1] = i;
                }
            }
            return TimeframeIndexes;
        }

        public static int[] GetIndexes(DateTime timeStart, DateTime timeEnd, List<string> dateTimes)
        {
            int[] TimeframeIndexes = { 0, 0 };
            bool minFound = false;
            DateTime checkedDateTime;
            for (int i = 0; i < dateTimes.Count; i++)
            {
                checkedDateTime = DateTime.Parse(dateTimes[i]);
                if (checkedDateTime >= timeStart && !minFound)
                {
                    TimeframeIndexes[0] = i;
                    minFound = true;
                }
                if (checkedDateTime <= timeEnd)
                {
                    TimeframeIndexes[1] = i;
                }
            }
            return TimeframeIndexes;
        }

        public static int[] GetIndexes(DateTime timeStart, DateTime timeEnd, List<string[]> data, int indexTime )
        {
            int[] TimeframeIndexes = { 0, 0 };
            bool minFound = false;
            DateTime checkedDateTime;
            for (int i = 0; i < data.Count; i++)
            {
                checkedDateTime = DateTime.Parse(data[i][indexTime]);
                if (checkedDateTime >= timeStart && !minFound)
                {
                    TimeframeIndexes[0] = i;
                    minFound = true;
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
