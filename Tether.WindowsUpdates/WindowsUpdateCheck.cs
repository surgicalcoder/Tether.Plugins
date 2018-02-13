using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tether.Plugins;
using WUApiLib;

namespace Tether.WindowsUpdates
{
    public class WindowsUpdateCheck : ILongRunningMetricProvider
    {
        private static ISearchResult CheckForUpdates(UpdateSession session)
        {
            ISearchResult uResult;
            session = new UpdateSession();
            var uSearcher = session.CreateUpdateSearcher();
            uResult = uSearcher.Search("IsInstalled = 0 and Type='Software'");
            return uResult;
        }

        public List<Metric> GetMetrics()
        {
            var session = new UpdateSession();
            ISearchResult uResult;
            var updateChecker = new AutomaticUpdatesClass();

            var results = new List<Metric>();

            results.Add(new Metric("windows.updates.enabled", updateChecker.ServiceEnabled ? 1 : 0));

            if (!updateChecker.ServiceEnabled)
            {
                return results;
            }

            int totalUpdates = 0;
            int criticalUpdates = 0;
            int updatesNeedingReboot = 0;
            
            uResult = CheckForUpdates(session);

            if (uResult != null)
            {
                foreach (IUpdate5 uResultUpdate in uResult.Updates)
                {
                    if (uResultUpdate == null)
                    {
                        continue;
                    }

                    if (!String.IsNullOrWhiteSpace(uResultUpdate.MsrcSeverity) && uResultUpdate.MsrcSeverity.Contains("Critical"))
                    {
                        criticalUpdates++;
                    }

                    totalUpdates++;

                    if (uResultUpdate.RebootRequired)
                    {
                        updatesNeedingReboot++;
                    }
                }
            }
            
            results.Add(new Metric("windows.updates.count", totalUpdates, tags:new Dictionary<string, string>{{"severity","all"}}));
            results.Add(new Metric("windows.updates.count", criticalUpdates, tags:new Dictionary<string, string>{{"severity","critical"}}));
            results.Add(new Metric("windows.updates.rebootcount", updatesNeedingReboot, tags:new Dictionary<string, string>{{"severity","all"}}));

            return results;
        }

        public TimeSpan CacheDuration => TimeSpan.FromHours(12);
    }
}
