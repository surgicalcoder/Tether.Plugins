using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tether.Plugins;

namespace Tether.MSMQMetrics
{

    public class MSMQMetricProvider : IMetricProvider
    {
        PerformanceCounterCategory category;
        public MSMQMetricProvider()
        {
            category = new PerformanceCounterCategory("MSMQ Queue");
        }

        public List<Metric> GetMetrics()
        {
            var values = new List<Metric>();

            var instanceNames = category.GetInstanceNames();

            foreach (var instance in instanceNames)
            {
                if (instance.EndsWith("$")) // MS notation for "Private"
                {
                    continue;
                }

                var performanceCounters = category.GetCounters(instance);
                
                values.Add(new Metric("windows.msmq.bytes", performanceCounters.FirstOrDefault(x=>x.CounterName  == "Bytes in Journal Queue")?.NextValue(), tags:new Dictionary<string, string>{{"type", "journal"},{"name", instance.Split('\\').LastOrDefault() }}));
                values.Add(new Metric("windows.msmq.messages", performanceCounters.FirstOrDefault(x=>x.CounterName  == "Messages in Journal Queue")?.NextValue(), tags:new Dictionary<string, string>{{"type", "journal"},{"name", instance.Split('\\').LastOrDefault() }}));

                values.Add(new Metric("windows.msmq.bytes", performanceCounters.FirstOrDefault(x=>x.CounterName  == "Bytes in Queue")?.NextValue(), tags:new Dictionary<string, string>{{"type", "queue"},{"name", instance.Split('\\').LastOrDefault() }}));
                values.Add(new Metric("windows.msmq.messages", performanceCounters.FirstOrDefault(x=>x.CounterName  == "Messages in Queue")?.NextValue(), tags:new Dictionary<string, string>{{"type", "queue"},{"name", instance.Split('\\').LastOrDefault() }}));
            }

            return values;
        }
    }
}
