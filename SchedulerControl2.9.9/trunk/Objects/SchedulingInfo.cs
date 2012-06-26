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
		#region ICallBack Members

		public void ScheduleCreated(ServiceInstanceInfo[] scheduleAndStateInfo)
		{
			NewScheduleCreatedEvent(this, new ScheduleCreatedEventArgs() { ScheduleAndStateInfo = scheduleAndStateInfo });
		}

		#endregion
		#region ICallBack Members
		public void InstanceEvent(ServiceInstanceInfo StateOutcomerInfo)
		{
			NewInstanceEvent(this, new InstanceEventArgs() { instanceStateOutcomerInfo = StateOutcomerInfo });
		}
		#endregion
	}	
	#endregion
	#region eventargs
	public class InstanceEventArgs : EventArgs
	{
		public ServiceInstanceInfo instanceStateOutcomerInfo;

	}
	public class ScheduleCreatedEventArgs : EventArgs
	{
		public ServiceInstanceInfo[] ScheduleAndStateInfo;
	}
#endregion
}
