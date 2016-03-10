using Tether.Plugins;

namespace Tether.MSSQLPerformance
{
    [PerformanceCounterGrouping("Win32_PerfFormattedData_MSSQLSERVER_SQLServerWaitStatistics", SelectorEnum.Each)]
    public class SQLWaitStats
    {
        public int Lockwaits { get; set; }
        public int Logbufferwaits { get; set; }
        public int Logwritewaits { get; set; }
        public int Memorygrantqueuewaits { get; set; }
        public string Name { get; set; }
        public int NetworkIOwaits { get; set; }
        public int NonPagelatchwaits { get; set; }
        public int PageIOlatchwaits { get; set; }
        public int Pagelatchwaits { get; set; }
        public int Threadsafememoryobjectswaits { get; set; }
        public int Transactionownershipwaits { get; set; }
        public int Waitfortheworker { get; set; }
        public int Workspacesynchronizationwaits { get; set; }
    }
}