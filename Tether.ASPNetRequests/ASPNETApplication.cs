using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tether.Plugins;

namespace Tether.ASPNetRequests
{

    public class ASPNEETApplciationMetricProvider : IMetricProvider
    {
        PerformanceCounterCategory category;
        public ASPNEETApplciationMetricProvider()
        {
            category = new PerformanceCounterCategory("ASP.NET Applications");
        }

        public List<Metric> GetMetrics()
        {

            var values = new List<Metric>();

            foreach (var instanceName in category.GetInstanceNames())
            {
                var performanceCounters = category.GetCounters(instanceName);
                
                values.Add(new Metric("windows.aspnet.applications.ProcessorTime", performanceCounters.FirstOrDefault(f=>f.CounterName == "% Managed Processor Time (estimated)")?.NextValue(), tags:new Dictionary<string, string>{{"application", instanceName.Replace("_LM_W3SVC_", "")}}));
                values.Add(new Metric("windows.aspnet.applications.ErrorsPerSec", performanceCounters.FirstOrDefault(f=>f.CounterName == "Errors Total/Sec")?.NextValue(), tags:new Dictionary<string, string>{{"application", instanceName.Replace("_LM_W3SVC_", "")}}));
                values.Add(new Metric("windows.aspnet.applications.PipelineInstanceCount", performanceCounters.FirstOrDefault(f=>f.CounterName == "Pipeline Instance Count")?.NextValue(), tags:new Dictionary<string, string>{{"application", instanceName.Replace("_LM_W3SVC_", "")}}));
                values.Add(new Metric("windows.aspnet.applications.RequestExecutionTime", performanceCounters.FirstOrDefault(f=>f.CounterName == "Request Execution Time")?.NextValue(), tags:new Dictionary<string, string>{{"application", instanceName.Replace("_LM_W3SVC_", "")}}));
                values.Add(new Metric("windows.aspnet.applications.RequestWaitTime", performanceCounters.FirstOrDefault(f=>f.CounterName == "Request Wait Time")?.NextValue(), tags:new Dictionary<string, string>{{"application", instanceName.Replace("_LM_W3SVC_", "")}}));
                values.Add(new Metric("windows.aspnet.applications.RequestsInApplicaitonQueue", performanceCounters.FirstOrDefault(f=>f.CounterName == "Requests In Application Queue")?.NextValue(), tags:new Dictionary<string, string>{{"application", instanceName.Replace("_LM_W3SVC_", "")}}));
                values.Add(new Metric("windows.aspnet.applications.RequestsPerSec", performanceCounters.FirstOrDefault(f=>f.CounterName == "Requests/Sec")?.NextValue(), tags:new Dictionary<string, string>{{"application", instanceName.Replace("_LM_W3SVC_", "")}}));

            }

            return values;
        }
    }
}
