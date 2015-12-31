using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tether.Plugins;

namespace Tether.HTTPServiceRequestQueues
{
    public class RequestQueueCheck : ICheck
    {
        public string Key => "HTTP-Service-Request-Queues";

        public object DoCheck()
        {
            PerformanceCounterCategory category = new PerformanceCounterCategory("HTTP Service Request Queues");
            IDictionary<string, object> values = new Dictionary<string, object>();

            foreach (PerformanceCounter counter in category.GetCounters())
            {
                values.Add(counter.CounterName, counter.NextValue());
            }
            return values;
        }
    }
}
