using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tether.Plugins;

namespace Tether.ASPNetRequests
{
    [PerformanceCounterGrouping("ASP.NET Applications", SelectorEnum.Each, UsePerformanceCounter = true)]
    public class ASPNETApplication
    {
        [PerformanceCounterValue("% Managed Processor Time (estimated)")]
        public long PercentProcessorTime { get; set; }

        [PerformanceCounterValue("Errors Total/Sec")]
        public long ErrorsPerSec { get; set; }

        [PerformanceCounterValue("Pipeline Instance Count")]
        public long PipelineInstanceCount { get; set; }

        [PerformanceCounterValue("Request Execution Time")]
        public long RequestExecutionTime { get; set; }

        [PerformanceCounterValue("Request Wait Time")]
        public long RequestWaitTime { get; set; }

        [PerformanceCounterValue("Requests In Application Queue")]
        public long RequestsInApplicaitonQueue { get; set; }

        [PerformanceCounterValue("Requests/Sec")]
        public long RequestsPerSec { get; set; }

        private string name;

        [PerformanceCounterInstanceName]
        public string Name
        {
            get { return name; }
            set { name = value.Replace("_LM_W3SVC_", ""); }
        }
    }
}
