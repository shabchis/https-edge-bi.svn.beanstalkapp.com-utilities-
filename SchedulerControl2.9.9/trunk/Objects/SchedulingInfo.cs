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
		public void InstancesEvents(List<ServiceInstanceInfo> instancesEvents)
		{
			NewInstanceEvent(this, new InstanceEventArgs() { Instances = instancesEvents });
		}

		#endregion
	}	
	#endregion
	#region eventargs
	public class InstanceEventArgs : EventArgs
	{
		public List<ServiceInstanceInfo> Instances;

	}
	public class ScheduleCreatedEventArgs : EventArgs
	{
		public ServiceInstanceInfo[] ScheduleAndStateInfo;
	}
#endregion
}
