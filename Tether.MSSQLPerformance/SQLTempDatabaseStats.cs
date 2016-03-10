using Tether.Plugins;

namespace Tether.MSSQLPerformance
{
    [PerformanceCounterGrouping("Win32_PerfFormattedData_MSSQLSERVER_SQLServerDatabases", SelectorEnum.Name, "tempdb")]
    public class SQLTempDatabaseStats
    {
        public int ActiveTransactions { get; set; }

        public int Committableentries { get; set; }

        public int LogBytesFlushedPersec { get; set; }
        public int LogCacheHitRatio { get; set; }
        public int LogCacheReadsPersec { get; set; }
        public int LogFilesSizeKB { get; set; }
        public int LogFilesUsedSizeKB { get; set; }
        public int LogFlushesPersec { get; set; }
        public int LogFlushWaitsPersec { get; set; }
        public int LogFlushWaitTime { get; set; }
        public int LogFlushWriteTimems { get; set; }
        public int LogGrowths { get; set; }
        public int LogPoolCacheMissesPersec { get; set; }
        public int LogPoolDiskReadsPersec { get; set; }
        public int LogPoolRequestsPersec { get; set; }
        public int LogShrinks { get; set; }
        public int LogTruncations { get; set; }

        public int ReplPendingXacts { get; set; }

        public int ReplTransRate { get; set; }

    }
}