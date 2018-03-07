using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Tether.Plugins;

namespace Tether.WindowsServiceMonitor
{
    public class RunningWindowsServiceMetricProvider : IMetricProvider, IRequireConfigurationData
    {
        public List<Metric> GetMetrics()
        {
            if (services == null || !services.Any())
            {
                return null;
            }

            var values = new List<Metric>();
            var allServices = ServiceController.GetServices();

            foreach (var service in services)
            {
                var svc = allServices.FirstOrDefault(f => f.DisplayName == service || f.ServiceName == service);
                
                var dictionary = new Dictionary<string, string>{{"name", service}};

                values.Add(svc == null
                    ? new Metric("windows.services.running", 0, tags: dictionary)
                    : new Metric("windows.services.running", svc.Status == ServiceControllerStatus.Running ? 1 : 2, tags: dictionary));
            }

            return values;
        }

        private List<string> services;

        public RunningWindowsServiceMetricProvider()
        {
            services = new List<string>();
        }

        public void LoadConfigurationData(dynamic data)
        {
            services = ((List<Object>)data.Services).Cast<String>().ToList();
        }
    }
}
