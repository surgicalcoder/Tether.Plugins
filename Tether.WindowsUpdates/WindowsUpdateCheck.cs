using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tether.Plugins;
using WUApiLib;

namespace Tether.WindowsUpdates
{
    public class WindowsUpdateCheck : ILongRunningCheck
    {

        public class WindowsUpdateCheckDetails
        {
            public WindowsUpdateCheckDetails()
            {
                TotalUpdates = 0;
                CriticalUpdates = 0;
            }

            public bool IsWindowsUpdateEnabled { get; set; }
            public int TotalUpdates { get; set; }
            public int CriticalUpdates { get; set; }
            public bool RebootRequired { get; set; }
        }

        public object DoCheck()
        {

            var session = new UpdateSession();
            ISearchResult uResult;
            var updateChecker = new AutomaticUpdatesClass();

            var results = new WindowsUpdateCheckDetails();

            results.IsWindowsUpdateEnabled = updateChecker.ServiceEnabled;

            if (!results.IsWindowsUpdateEnabled)
            {
                return results;
            }

            uResult = CheckForUpdates(session);

            if (uResult != null)
            {
                foreach (IUpdate5 uResultUpdate in uResult.Updates)
                {
                    if (uResultUpdate.MsrcSeverity.Contains("Critical"))
                    {
                        results.CriticalUpdates++;
                    }

                    results.TotalUpdates++;

                    if (uResultUpdate.RebootRequired)
                    {
                        results.RebootRequired = true;
                    }
                }
            }
            

            return results;
        }

        private static ISearchResult CheckForUpdates(UpdateSession session)
        {
            ISearchResult uResult;
            session = new UpdateSession();
            var uSearcher = session.CreateUpdateSearcher();
            uResult = uSearcher.Search("IsInstalled = 0 and Type='Software'");
            return uResult;
        }

        public string Key => "Windows Update";

        public TimeSpan CacheDuration => TimeSpan.FromDays(1);
    }
}
