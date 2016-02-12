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
        public object DoCheck()
        {
            PerformanceCounterCategory category = new PerformanceCounterCategory("HTTP Service Request Queues");

            IDictionary<string, object> values = new Dictionary<string, object>();

            var instances = category.GetInstanceNames();

            foreach (var instance in instances)
            {
                var counters = category.GetCounters(instance);
                values.Add(instance, counters.FirstOrDefault(e => e.CounterName == "CurrentQueueSize").NextValue());
            }


            return values;
        }

        public string Key => "HTTP-Service-Request-Queues";
    }


    public class ASPNETApplicationQueues : ICheck
    {
        public object DoCheck()
        {
            PerformanceCounterCategory category = new PerformanceCounterCategory("ASP.NET Applications");

            IDictionary<string, object> values = new Dictionary<string, object>();

            var instances = category.GetInstanceNames();

            foreach (var instance in instances)
            {
                var counters = category.GetCounters(instance);
                values.Add(instance, counters.FirstOrDefault(e => e.CounterName == "Requests In Application Queue").NextValue());
            }


            return values;

        }

        public string Key => "ASPNET-Applications-Queues";
    }

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



