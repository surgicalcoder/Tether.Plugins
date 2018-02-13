using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using Tether.Plugins;

namespace Tether.IISOldRequests
{
    public class IISLongRunningRequestMetricProvider : IMetricProvider
    {
        public List<Metric> GetMetrics()
        {
            var values = new List<Metric>();

            using (var manager = new ServerManager())
            {
                foreach (var w3wp in manager.WorkerProcesses)
                {
                    var requestCollection = w3wp.GetRequests(10000);
                    
                    int count = 0;
                    int longest = 0;

                    if (requestCollection.Any())
                    {
                        count = requestCollection.Count;
                        longest = requestCollection.Max(f => f.TimeElapsed) / 1000;
                    }
                    values.Add(new Metric("windows.aspnet.longrunning.count", count, tags:new Dictionary<string, string>{{"app", w3wp.AppPoolName}}));
                    values.Add(new Metric("windows.aspnet.longrunning.longest", longest, tags:new Dictionary<string, string>{{"app", w3wp.AppPoolName}}));

                }
                
            }

            return values;
        }
    }
}
