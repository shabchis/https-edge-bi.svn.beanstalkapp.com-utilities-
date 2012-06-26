using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Edge.Core.Scheduling.Objects;
using Legacy=Edge.Core.Services;
using Edge.Core.Configuration;

namespace Edge.Applications.PM.SchedulerControl.Objects
{
	[ServiceContract(SessionMode = SessionMode.Required,
	 CallbackContract = typeof(ICallBack))]
	#region Interfaces
	public interface ISchedulingCommunication
	{
		[OperationContract]
		void Subscribe();
		[OperationContract]		
		Legacy.IsAlive IsAlive(Guid guid);
		[OperationContract]
		void Abort(Guid guid);
		[OperationContract]
		void ResetUnEnded();
		[OperationContract]
		List<AccountServiceInformation> GetServicesConfigurations();
		[OperationContract]
		Guid AddUnplanedService(int accountID, string serviceName, Dictionary<string, string> options,DateTime targetDateTime);
	}
	public interface ICallBack
	{
		[OperationContract(IsOneWay = true)]
		void ScheduleCreated(ServiceInstanceInfo[] scheduleAndStateInfo);
		[OperationContract(IsOneWay = true)]
		void InstanceEvent(ServiceInstanceInfo StateOutcomerInfo);
	}
	#endregion
	#region classes
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
