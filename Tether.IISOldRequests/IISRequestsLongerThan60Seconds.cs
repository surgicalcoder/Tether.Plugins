using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.Administration;
using Tether.Plugins;

namespace Tether.IISOldRequests
{
    public class IISRequestsLongerThan60Seconds : ICheck
    {
        public object DoCheck()
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            using (ServerManager manager = new ServerManager())
            {
                foreach (WorkerProcess w3wp in manager.WorkerProcesses)
                {
                    var requestCollection = w3wp.GetRequests(60000);

                    if (requestCollection.Any())
                    {
                        values.Add(w3wp.AppPoolName + "-Count", requestCollection.Count);
                        values.Add(w3wp.AppPoolName + "-Longest", requestCollection.Max(e => e.TimeElapsed));
                    }
                    else
                    {
                        values.Add(w3wp.AppPoolName + "-Count", 0);
                        values.Add(w3wp.AppPoolName + "-Longest", 0);
                    }
                }
            }

            return values;

        }

        public string Key => "IIS Requests > 60 Sec";
    }
}