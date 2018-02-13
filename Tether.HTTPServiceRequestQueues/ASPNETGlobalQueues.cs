using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tether.Plugins;

namespace Tether.HTTPServiceRequestQueues
{
    public class ASPNETGlobalQueueMetricProvider : IMetricProvider
    {
        private PerformanceCounterCategory v2Category;
        private PerformanceCounterCategory v4Category;
        private PerformanceCounter[] v4Counters;
        private PerformanceCounter[] v2Counters;
        private PerformanceCounter v2RequestQueued;
        private PerformanceCounter v4RequestQueued;
        private PerformanceCounter v4NativeQueued;
        private PerformanceCounterCategory HttpServiceRequestCategory;
        public ASPNETGlobalQueueMetricProvider()
        {
            v2Category = new PerformanceCounterCategory("ASP.NET v2.0.50727");
            v4Category = new PerformanceCounterCategory("ASP.NET v4.0.30319");
            
            HttpServiceRequestCategory = new PerformanceCounterCategory("HTTP Service Request Queues");

            v2Counters = v2Category.GetCounters();    
            v4Counters = v4Category.GetCounters();
            
            v2RequestQueued = v2Counters.FirstOrDefault(e => e.CounterName == "Requests Queued");
            v4RequestQueued = v4Counters.FirstOrDefault(e => e.CounterName == "Requests Queued");
            
            v4NativeQueued = v4Counters.FirstOrDefault(e => e.CounterName == "Requests In Native Queue");
        }

        public List<Metric> GetMetrics()
        {
            var values = new List<Metric>();

            values.Add(new Metric("windows.aspnet.queues", v2RequestQueued.NextValue(), tags:new Dictionary<string, string>{{"queue", "v2"},{"type", "app"}}));
            values.Add(new Metric("windows.aspnet.queues", v4RequestQueued.NextValue(), tags:new Dictionary<string, string>{{"queue", "v4"},{"type", "app"}}));
            values.Add(new Metric("windows.aspnet.queues", v4NativeQueued.NextValue(), tags:new Dictionary<string, string>{{"queue", "v4"},{"type", "native"}}));

            values.AddRange(from instanceName in HttpServiceRequestCategory.GetInstanceNames() let performanceCounters = HttpServiceRequestCategory.GetCounters(instanceName) select new Metric("windows.aspnet.applications.queues", performanceCounters.FirstOrDefault(f => f.CounterName == "CurrentQueueSize")?.NextValue(), tags: new Dictionary<string, string> {{"queue", "v4"}, {"type", "http.sys"}, {"app", instanceName}}));

            return values;
        }
    }
   
}