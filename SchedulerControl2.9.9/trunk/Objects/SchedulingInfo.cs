using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Edge.Core.Scheduling.Objects;

namespace Edge.Applications.PM.SchedulerControl.Objects
{
	[ServiceContract(SessionMode = SessionMode.Required,
	 CallbackContract = typeof(ICallBack))]
	public interface ISchedulingCommunication
	{
		[OperationContract]
		void Subscribe();
	}
	public interface ICallBack
	{
		[OperationContract(IsOneWay = true)]
		void ScheduleCreated(ServiceInstanceInfo[] scheduleAndStateInfo);

		[OperationContract(IsOneWay = true)]
		void InstanceEvent(ServiceInstanceInfo StateOutcomerInfo);
	}
	public class Callback : ICallBack
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
	public class ScheduleCreatedEventArgs : EventArgs
	{
		public ServiceInstanceInfo[] ScheduleAndStateInfo;

	}
	public class InstanceEventArgs : EventArgs
	{
		public ServiceInstanceInfo instanceStateOutcomerInfo;
	}
}
