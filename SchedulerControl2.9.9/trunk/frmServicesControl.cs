using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Edge.Core.Scheduling.Objects;
using System.ServiceModel;


namespace Edge_Applications.PM.common
{
	public partial class frmServicesControl : Form
	{
		private DuplexChannelFactory<ISchedulingCommunication> _channel;
		private ISchedulingCommunication _schedulingCommunicationChannel;
		private Callback _callBack;
		private Dictionary<string, ServiceInstanceInfo> _instanceRef = new Dictionary<string, ServiceInstanceInfo>();
		private List<ServiceInstanceInfo> _instanceInfo=null;
		public frmServicesControl()
		{
			InitializeComponent();
			_callBack = new Callback();
			_channel=new DuplexChannelFactory<ISchedulingCommunication>(_callBack,
				new NetNamedPipeBinding() { MaxBufferPoolSize = 20000000, MaxConnections = 20000000, MaxBufferSize = 20000000, MaxReceivedMessageSize = 20000000, CloseTimeout = new TimeSpan(0, 3, 0), OpenTimeout = new TimeSpan(0, 3, 0) },
				new EndpointAddress("net.pipe://localhost/Scheduler"));
			_schedulingCommunicationChannel=_channel.CreateChannel();
			_schedulingCommunicationChannel.Subscribe();
			
		
			
			mainGrid.DataSource = _instanceInfo;
			mainGrid.Refresh();
			
			_callBack.NewScheduleCreatedEvent += new EventHandler(_callBack_NewScheduleCreatedEvent);
			_callBack.NewInstanceEvent += new EventHandler(_callBack_NewInstanceEvent);
			


		}
		void _callBack_NewInstanceEvent(object sender, EventArgs e)
		{
			InstanceEventArgs ie = (InstanceEventArgs)e;
			ServiceInstanceInfo serviceInstanceInfo = ie.instanceStateOutcomerInfo;
			if (_instanceRef.ContainsKey(serviceInstanceInfo.GetHashCode().ToString()))
			{
				_instanceRef[serviceInstanceInfo.GetHashCode().ToString()] = serviceInstanceInfo;				
				mainGrid.DataSource = _instanceInfo;
				mainGrid.Refresh();
			}
		}
		void _callBack_NewScheduleCreatedEvent(object sender, EventArgs e)
		{
			ScheduleCreatedEventArgs scheduleCreatedEventArgs = (ScheduleCreatedEventArgs)e;
			_instanceInfo = scheduleCreatedEventArgs.ScheduleAndStateInfo.ToList().OrderBy(i=>i.SchdeuleStartTime).ToList();
			foreach (var instance in _instanceInfo)			
				_instanceRef[instance.GetHashCode().ToString()] = instance;			
			mainGrid.DataSource = _instanceInfo;
			mainGrid.Refresh();
			
		}
	}
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
			NewInstanceEvent(this, new InstanceEventArgs() { instanceStateOutcomerInfo=StateOutcomerInfo});
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
