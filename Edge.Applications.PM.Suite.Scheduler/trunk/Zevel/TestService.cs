using System;
using System.Diagnostics;
using System.Threading;
using Edge.Core.Services;

namespace Edge.Applications.PM.SchedulerControl.Zevel
{
    /// <summary>
    /// Dummy service for test
    /// </summary>
    public class TestService : Service
    {
        protected override ServiceOutcome DoWork()
        {
            Debug.WriteLine(DateTime.Now + String.Format(": Starting '{0}'", Configuration.ServiceName));
            Thread.Sleep(TimeSpan.FromSeconds(10));
            Debug.WriteLine(DateTime.Now + String.Format(": Finishing '{0}'", Configuration.ServiceName));

            return ServiceOutcome.Success;
        }
    }
}
