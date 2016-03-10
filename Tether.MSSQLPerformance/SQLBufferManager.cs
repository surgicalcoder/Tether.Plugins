using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tether.Plugins;

namespace Tether.MSSQLPerformance
{
    [PerformanceCounterGrouping("Win32_PerfFormattedData_MSSQLSERVER_SQLServerBufferManager", SelectorEnum.Single)]
    public class SQLBufferManager
    {
        public int Buffercachehitratio { get; set; }

        public int CheckpointpagesPerSec { get; set; }

        public int LazywritesPersec { get; set; }

        public int Pagelifeexpectancy { get; set; }

        public int PagelookupsPersec { get; set; }

        public int PagereadsPersec { get; set; }

        public int PagewritesPersec { get; set; }
    }
}
