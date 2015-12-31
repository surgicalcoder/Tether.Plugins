using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tether.Plugins;

namespace Tether.ClassicASP
{
    public class ClassicASPCheck : ICheck
    {
        public string Key => "Classic-ASP";

        public object DoCheck()
        {
            PerformanceCounterCategory category = new PerformanceCounterCategory("Active Server Pages");
            IDictionary<string, object> values = new Dictionary<string, object>();

            foreach (PerformanceCounter counter in category.GetCounters())
            {
                values.Add(counter.CounterName, counter.NextValue());
            }
            return values;
        }
    }
}