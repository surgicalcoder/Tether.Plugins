using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Wmi;
using Tether.Plugins;

namespace Tether.SQLServer
{

    internal class SQLMetric
    {
        public SQLMetric()
        {
            Counters = new List<(string Name, PerformanceCounter perfCounter)>();
        }

        public string InstanceName { get; set; }
        public List<(string Name, PerformanceCounter perfCounter)> Counters { get; set; }
    }

    public class SQLServerMetricProvider : IMetricProvider
    {
        private ManagedComputer mc;
        
        private ServerInstanceCollection serverInstanceCollection;

        private List<SQLMetric> metrics;

        public SQLServerMetricProvider()
        {
            mc = new ManagedComputer();
            
            serverInstanceCollection = mc.ServerInstances;

            metrics = new List<SQLMetric>();
            
            foreach (ServerInstance serverInstance in serverInstanceCollection)
            {
                var metric = new SQLMetric(){InstanceName = serverInstance.Name};
                
                metric.Counters.AddRange(GetLocks(serverInstance.Name));
                metric.Counters.AddRange(GetDatabases(serverInstance.Name));
                
                metric.Counters.AddRange(GetBufferManagerStats(serverInstance.Name));
                metric.Counters.AddRange(GetWaitStats(serverInstance.Name));

                metric.Counters.Add(("sqlserver.errors.persec", new PerformanceCounter($"{serverInstance.Name}:SQL Errors", "Errors/sec", "_Total")));
                
                metric.Counters.Add(("sqlserver.plan.cache.hit_ratio", new PerformanceCounter($"{serverInstance.Name}:Plan Cache", "Cache Hit Ratio", "_Total")));
                metric.Counters.Add(("sqlserver.plan.cache.pages", new PerformanceCounter($"{serverInstance.Name}:Plan Cache", "Cache Pages", "_Total")));
                metric.Counters.Add(("sqlserver.plan.cache.obj_counts", new PerformanceCounter($"{serverInstance.Name}:Plan Cache", "Cache Object Counts", "_Total")));
                metric.Counters.Add(("sqlserver.plan.cache.obj_in_use", new PerformanceCounter($"{serverInstance.Name}:Plan Cache", "Cache Objects in use", "_Total")));

                metric.Counters.Add(("sqlserver.broker.tasks.running", new PerformanceCounter($"{serverInstance.Name}:Broker Activation", "Tasks Running", "_Total")));

                metrics.Add(metric);
            }
        }

        private IEnumerable<(string Name, PerformanceCounter perfCounter)> GetWaitStats(string serverInstanceName)
        {
            var values = new List<(string Name, PerformanceCounter perfCounter)>();

            var perfCounterName = $"{serverInstanceName}:Wait Statistics";

            var instanceName = "Average Wait Time (ms)";

            values.Add(("sqlserver.wait.locks", new PerformanceCounter(perfCounterName, "Lock waits", instanceName)));
            values.Add(("sqlserver.wait.memory_grant_queue", new PerformanceCounter(perfCounterName, "Memory grant queue waits", instanceName)));
            values.Add(("sqlserver.wait.thread_safe_memory", new PerformanceCounter(perfCounterName, "Thread-safe memory objects waits", instanceName)));
            values.Add(("sqlserver.wait.log_writes", new PerformanceCounter(perfCounterName, "Log write waits", instanceName)));
            values.Add(("sqlserver.wait.log_buffer", new PerformanceCounter(perfCounterName, "Log buffer waits", instanceName)));
            values.Add(("sqlserver.wait.network_io", new PerformanceCounter(perfCounterName, "Network IO waits", instanceName)));
            values.Add(("sqlserver.wait.page_io_latch", new PerformanceCounter(perfCounterName, "Page IO latch waits", instanceName)));
            values.Add(("sqlserver.wait.page_latch", new PerformanceCounter(perfCounterName, "Page latch waits", instanceName)));
            values.Add(("sqlserver.wait.non_page_latch", new PerformanceCounter(perfCounterName, "Non-Page latch waits", instanceName)));
            values.Add(("sqlserver.wait.worker", new PerformanceCounter(perfCounterName, "Wait for the worker", instanceName)));
            values.Add(("sqlserver.wait.workspace_sync", new PerformanceCounter(perfCounterName, "Workspace synchronization waits", instanceName)));
            values.Add(("sqlserver.wait.transaction_ownership", new PerformanceCounter(perfCounterName, "Transaction ownership waits", instanceName)));
            

            return values;
        }

        private IEnumerable<(string Name, PerformanceCounter perfCounter)> GetBufferManagerStats(string serverInstanceName)
        {
            var values = new List<(string Name, PerformanceCounter perfCounter)>();

            var perfCounterName = $"{serverInstanceName}:Buffer Manager";

            values.Add(("sqlserver.buffer.hit_ratio", new PerformanceCounter(perfCounterName, "Buffer cache hit ratio")));
            values.Add(("sqlserver.buffer.checkpoint_pages_persec", new PerformanceCounter(perfCounterName, "Checkpoint pages/sec")));
            values.Add(("sqlserver.buffer.lazy_writes_persec", new PerformanceCounter(perfCounterName, "Lazy writes/sec")));
            values.Add(("sqlserver.buffer.page_life_expectancy", new PerformanceCounter(perfCounterName, "Page life expectancy")));
            values.Add(("sqlserver.buffer.page_lookups_persec", new PerformanceCounter(perfCounterName, "Page life lookups/sec")));
            values.Add(("sqlserver.buffer.page_reads_persec", new PerformanceCounter(perfCounterName, "Page reads/sec")));
            values.Add(("sqlserver.buffer.page_writes_persec", new PerformanceCounter(perfCounterName, "Page writes/sec")));

            return values;
        }

        private IEnumerable<(string Name, PerformanceCounter perfCounter)> GetDatabases(string serverInstanceName)
        {
            var values = new List<(string Name, PerformanceCounter perfCounter)>();

            values.Add(("sqlserver.databases.datafile_size", new PerformanceCounter($"{serverInstanceName}:Databases", "Data File(s) Size (KB)", "_Total")));
            values.Add(("sqlserver.databases.logfile_size", new PerformanceCounter($"{serverInstanceName}:Databases", "Log File(s) Size (KB)", "_Total")));
            values.Add(("sqlserver.databases.logfile_size", new PerformanceCounter($"{serverInstanceName}:Databases", "Log File(s) Size (KB)", "_Total")));
            values.Add(("sqlserver.databases.logfile_used", new PerformanceCounter($"{serverInstanceName}:Databases", "Log File(s) Used Size (KB)", "_Total")));
            values.Add(("sqlserver.databases.logfile_used_pc", new PerformanceCounter($"{serverInstanceName}:Databases", "Percent Log Used", "_Total")));
            values.Add(("sqlserver.databases.transactions", new PerformanceCounter($"{serverInstanceName}:Databases", "Active Transactions", "_Total")));
            values.Add(("sqlserver.databases.transactions_per_sec", new PerformanceCounter($"{serverInstanceName}:Databases", "Transactions/sec", "_Total")));

            return values;
        }

        private List<(string Name, PerformanceCounter perfCounter)> GetLocks(string serverInstanceName)
        {
            var values = new List<(string Name, PerformanceCounter perfCounter)>();

            var perfCounterName = $"{serverInstanceName}:Databases";

            values.Add(("sqlserver.databases.avg_wait_time", new PerformanceCounter(perfCounterName, "Average Wait Time (ms)", "_Total")));
            values.Add(("sqlserver.databases.per_sec", new PerformanceCounter(perfCounterName, "Lock Requests/sec", "_Total")));
            values.Add(("sqlserver.databases.timeouts_gt_1sec", new PerformanceCounter(perfCounterName, "Lock Timeouts (timeout > 0)/sec", "_Total")));
            values.Add(("sqlserver.databases.timeouts", new PerformanceCounter(perfCounterName, "Lock Timeouts/sec", "_Total")));
            values.Add(("sqlserver.databases.wait_time", new PerformanceCounter(perfCounterName, "Lock Wait Time (ms)", "_Total")));
            values.Add(("sqlserver.databases.waits", new PerformanceCounter(perfCounterName, "Lock Waits/sec", "_Total")));
            values.Add(("sqlserver.databases.deadlocks_persec", new PerformanceCounter(perfCounterName, "Number of Deadlocks/sec", "_Total")));
            values.Add(("sqlserver.databases.repl_pending_xacts", new PerformanceCounter(perfCounterName, "Repl. Pending Xacts", "_Total")));
            values.Add(("sqlserver.databases.repl_trans_rate", new PerformanceCounter(perfCounterName, "Repl. Trans. Rate", "_Total")));
            values.Add(("sqlserver.databases.log_cache_reads_persec", new PerformanceCounter(perfCounterName, "Log Cache Reads/sec", "_Total")));
            values.Add(("sqlserver.databases.log_cache_hit_ratio", new PerformanceCounter(perfCounterName, "Log Cache Hit Ratio", "_Total")));
            values.Add(("sqlserver.databases.bulk_copy_rows_persec", new PerformanceCounter(perfCounterName, "Bulk Copy Rows/sec", "_Total")));
            values.Add(("sqlserver.databases.bulk_copy_thruput_persec", new PerformanceCounter(perfCounterName, "Bulk Copy Throughput/sec", "_Total")));

            return values;
        }

        public List<Metric> GetMetrics()
        {
            var values = new List<Metric>();
            
            foreach (var sqlMetric in metrics)
            {
                foreach (var counter in sqlMetric.Counters)
                {
                    values.Add(new Metric(counter.Name, counter.perfCounter.NextValue(), tags: new Dictionary<string, string>{{"instance", sqlMetric.InstanceName}}));
                }
            }

            return values;
        }
    }

    
}
