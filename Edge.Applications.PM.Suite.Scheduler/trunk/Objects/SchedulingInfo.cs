using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Edge.Core.Scheduling;
using Edge.Core.Scheduling;
using Legacy=Edge.Core.Services;
using Edge.Core.Configuration;
using Edge.Core.Services;

namespace Edge.Applications.PM.SchedulerControl.Objects
{
	#region classes
	public class Callback : ISchedulingHostSubscriber
	{
		public event EventHandler NewScheduleCreatedEvent;
		public event EventHandler NewInstanceEvent;
		#region ISchedulingHostSubscriber Members
		public void InstancesEvents(List<ServiceInstance> requestsEvents)
		{
			NewInstanceEvent(this, new RequestsEventArgs() { requests = requestsEvents });
		}

		#endregion
	}	
	#endregion
	#region eventargs
	public class RequestsEventArgs : EventArgs
	{
		public List<ServiceInstance> requests;

	}
	public class ScheduleCreatedEventArgs : EventArgs
	{
		public ServiceInstance[] ScheduleAndStateInfo;
	}
#endregion
}
