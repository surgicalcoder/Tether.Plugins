using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tether.Plugins;

namespace Tether.HTTPServiceRequestQueues
{
    public class ASPNETGlobalQueues : ICheck
    {
        public object DoCheck()
        {
            IDictionary<string, object> values = new Dictionary<string, object>();

            var v2Category = new PerformanceCounterCategory("ASP.NET v2.0.50727");
            values.Add("v2-Requests-Queued", v2Category.GetCounters().FirstOrDefault(e => e.CounterName == "Requests Queued").NextValue());
            var v4Category = new PerformanceCounterCategory("ASP.NET v4.0.30319");
            values.Add("v4-Requests-Queued", v4Category.GetCounters().FirstOrDefault(e => e.CounterName == "Requests Queued").NextValue());
            values.Add("v4-Requests-NativeQueued", v4Category.GetCounters().FirstOrDefault(e => e.CounterName == "Requests In Native Queue").NextValue());

            return values;
        }

        public string Key => "ASPNET-Global-Queues";
    }
}