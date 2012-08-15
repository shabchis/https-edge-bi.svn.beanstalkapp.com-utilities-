using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Edge.Core.Scheduling;
using Edge.Core.Scheduling.Objects;
using Legacy=Edge.Core.Services;
using Edge.Core.Configuration;

namespace Edge.Applications.PM.SchedulerControl.Objects
{
	#region classes
	public class Callback : ISchedulingHostSubscriber
	{
		public event EventHandler NewScheduleCreatedEvent;
		public event EventHandler NewInstanceEvent;
		#region ISchedulingHostSubscriber Members
		public void InstancesEvents(List<SchedulingRequestInfo> requestsEvents)
		{
			NewInstanceEvent(this, new RequestsEventArgs() { requests = requestsEvents });
		}

		#endregion
	}	
	#endregion
	#region eventargs
	public class RequestsEventArgs : EventArgs
	{
		public List<SchedulingRequestInfo> requests;

	}
	public class ScheduleCreatedEventArgs : EventArgs
	{
		public SchedulingRequestInfo[] ScheduleAndStateInfo;
	}
#endregion
}
