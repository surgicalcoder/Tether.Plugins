﻿using System;
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
}



