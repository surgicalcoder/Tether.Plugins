using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tether.Plugins;

namespace Tether.HTTPServiceRequestQueues
{
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

                DisposeAll(counters);
            }


            return values;
        }

        private void DisposeAll(PerformanceCounter[] counters)
        {
            foreach (var counter in counters)
            {
                try
                {
                    counter.Dispose();
                }
                catch (System.Exception)
                {
                    // Yeah, I know. Yeah, I really do know.
                }
            }
        }

        public string Key => "ASPNET-Applications-Queues";
    }
}