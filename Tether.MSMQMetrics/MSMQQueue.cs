using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tether.Plugins;

namespace Tether.MSMQMetrics
{
    [PerformanceCounterGrouping("Win32_PerfFormattedData_msmq_MSMQQueue", SelectorEnum.Each)]
    public class MSMQQueue
    {
        public long MessagesInJournalQueue { get; set; }
        public long MessagesInQueue { get; set; }

        public long BytesInJournalQueue { get; set; }
        public long BytesInQueue { get; set; }

        public string Name { get; set; }
    }
}
