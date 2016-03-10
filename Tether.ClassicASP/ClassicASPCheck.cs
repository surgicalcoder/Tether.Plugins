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

            var counters = category.GetCounters();

            foreach (PerformanceCounter counter in counters)
            {
                values.Add(counter.CounterName, counter.NextValue());
                counter.Dispose();
            }

            DisposeAll(counters);

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
    }
}