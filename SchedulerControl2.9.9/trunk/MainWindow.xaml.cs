using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Edge.Core.Scheduling.Objects;
using System.ServiceModel;
using Edge.Applications.PM.SchedulerControl.Objects;
using Core = Edge.Core;
using System.Collections;
using System.Runtime.Serialization;


namespace Edge.Applications.PM.SchedulerControl
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static BindingData BindingData = new BindingData();
		private DuplexChannelFactory<ISchedulingCommunication> _channel;
		private ISchedulingCommunication _schedulingCommunicationChannel;
		private Callback _callBack;
		private Dictionary<string, ServiceInstanceInfo> _instanceRef = new Dictionary<string, ServiceInstanceInfo>();
		public MainWindow()
		{
			InitializeComponent();
			_callBack = new Callback();
			_channel = new DuplexChannelFactory<ISchedulingCommunication>(_callBack,
				new NetNamedPipeBinding() { MaxBufferPoolSize = 20000000, MaxConnections = 20000000, MaxBufferSize = 20000000, MaxReceivedMessageSize = 20000000, CloseTimeout = new TimeSpan(0, 3, 0), OpenTimeout = new TimeSpan(0, 3, 0) },
				new EndpointAddress("net.pipe://localhost/Scheduler"));
			_schedulingCommunicationChannel = _channel.CreateChannel();
			_schedulingCommunicationChannel.Subscribe();

			_callBack.NewScheduleCreatedEvent += new EventHandler(_callBack_NewScheduleCreatedEvent);
			_callBack.NewInstanceEvent += new EventHandler(_callBack_NewInstanceEvent);
			MainWindow.BindingData.Instances = new ObservableCollection<InstanceView>();
			this.DataContext = MainWindow.BindingData;
		}
		void _callBack_NewInstanceEvent(object sender, EventArgs e)
		{
			InstanceEventArgs ie = (InstanceEventArgs)e;
			ServiceInstanceInfo serviceInstanceInfo = ie.instanceStateOutcomerInfo;
			if (_instanceRef.ContainsKey(serviceInstanceInfo.GetHashCode().ToString()))
			{
				_instanceRef[serviceInstanceInfo.GetHashCode().ToString()] = serviceInstanceInfo;

			}
		}
		void _callBack_NewScheduleCreatedEvent(object sender, EventArgs e)
		{
			ScheduleCreatedEventArgs scheduleCreatedEventArgs = (ScheduleCreatedEventArgs)e;
			//_instanceInfo = scheduleCreatedEventArgs.ScheduleAndStateInfo.ToList().OrderBy(i => i.SchdeuleStartTime).ToList();
			BindingData.UpdateInstances(scheduleCreatedEventArgs.ScheduleAndStateInfo);
			foreach (var instance in scheduleCreatedEventArgs.ScheduleAndStateInfo.ToList().OrderBy(i => i.SchdeuleStartTime).ToList())
				_instanceRef[instance.GetHashCode().ToString()] = instance;
			this.DataContext = MainWindow.BindingData;




		}
	}
	public class BindingData : INotifyPropertyChanged
	{
		InstanceView _currentInstance;
		Dictionary<Guid, InstanceView> _instancesRef = new Dictionary<Guid, InstanceView>();


		public ObservableCollection<InstanceView> Instances { get; set; }
		public InstanceView CurrentInstance
		{
			get
			{
				return _currentInstance;
			}
			set
			{
				_currentInstance = value;
				RaisePropertyChanged("CurrentInstance");
			}
		}
		private Dictionary<int, InstanceView> instanceByInstanceID = new Dictionary<int, InstanceView>();
		private Dictionary<Guid, InstanceView> instanceByGuid = new Dictionary<Guid, InstanceView>();

		public void UpdateInstances(ServiceInstanceInfo[] instancesInfo)
		{

			lock (Instances)
			{
				List<InstanceView> childs = new List<InstanceView>();
				foreach (var instance in instancesInfo)
				{
					//first time
					if (_instancesRef.ContainsKey(instance.LegacyInstanceGuid))
						_instancesRef[instance.LegacyInstanceGuid].ServiceInstanceInfo = instance;
					else
					{
						InstanceView iv = new InstanceView() { ServiceInstanceInfo = instance };
						_instancesRef[instance.LegacyInstanceGuid] = iv;
						if (iv.ParentInstanceID == Guid.Empty)
							Instances.Add(iv);
						else
							childs.Add(iv);

					}
				}
				foreach (var child in childs)
				{
					_instancesRef[child.ParentInstanceID].ChildsSteps.Add(child);

				}






			}




		}




		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion
	}

	public class InstanceView : INotifyPropertyChanged
	{
		private ServiceInstanceInfo _instanceInfo;
		private ObservableCollection<InstanceView> _childsSteps = new ObservableCollection<InstanceView>();
		public ObservableCollection<InstanceView> ChildsSteps
		{
			get
			{
				return _childsSteps;
			}
			set
			{


			}
		}
		public ServiceInstanceInfo ServiceInstanceInfo
		{
			get
			{
				return _instanceInfo;
			}
			set
			{
				_instanceInfo = value;
				//todo: property changed
				ParentInstanceID = _instanceInfo.ParentInstanceID;

			}

		}
		public Guid ParentInstanceID;
		public InstanceView()
		{



			IsExpanded = true;

		}
		public int ScheduledID
		{
			get
			{
				return _instanceInfo.ScheduledID;
			}

		}
		public string InstanceID
		{
			get
			{
				return _instanceInfo.InstanceID;
			}

		}
		public string ServiceName
		{
			get
			{
				return _instanceInfo.ServiceName;
			}

		}
		public int AccountID
		{
			get
			{
				return _instanceInfo.AccountID;
			}

		}
		public DateTime SchdeuleStartTime
		{
			get
			{
				return _instanceInfo.SchdeuleStartTime;
			}

		}
		public DateTime ScheduleEndTime
		{
			get
			{
				return _instanceInfo.ScheduleEndTime;
			}

		}
		public DateTime ActualStartTime
		{
			get
			{
				return _instanceInfo.ActualStartTime;
			}

		}
		public Core.Services.ServiceState State
		{
			get
			{
				return _instanceInfo.State;
			}

		}
		public Core.Services.ServiceOutcome Outcome
		{
			get
			{
				return _instanceInfo.Outcome;
			}

		}
		public string DayCode
		{
			get
			{
				return _instanceInfo.DayCode;
			}

		}
		public bool IsExpanded { get; set; }

		public double Progress { get; set; }














		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}

}
