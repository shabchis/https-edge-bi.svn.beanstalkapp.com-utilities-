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
			
			
			this.DataContext = MainWindow.BindingData;
		}
		void _callBack_NewInstanceEvent(object sender, EventArgs e)
		{
			InstanceEventArgs ie = (InstanceEventArgs)e;
			ServiceInstanceInfo serviceInstanceInfo = ie.instanceStateOutcomerInfo;
			BindingData.UpdateInstances(new ServiceInstanceInfo[] { serviceInstanceInfo});
		}
		void _callBack_NewScheduleCreatedEvent(object sender, EventArgs e)
		{
			ScheduleCreatedEventArgs scheduleCreatedEventArgs = (ScheduleCreatedEventArgs)e;		
			BindingData.UpdateInstances(scheduleCreatedEventArgs.ScheduleAndStateInfo);
		}
	}
	public class BindingData : INotifyPropertyChanged
	{
		InstanceView _currentInstance;
		Dictionary<Guid, InstanceView> _InstancesRef = new Dictionary<Guid, InstanceView>();
		public BindingData()
		{
			Instances = new ObservableCollection<InstanceView>();
			var collectionview = CollectionViewSource.GetDefaultView(Instances);
			collectionview.SortDescriptions.Add(new SortDescription("SchdeuleStartTime",ListSortDirection.Ascending));

			
		}
		public ObservableCollection<InstanceView> Instances { get; set; }		
		public void UpdateInstances(ServiceInstanceInfo[] instancesInfo)
		{


			lock (Instances)
			{
				List<InstanceView> childs = new List<InstanceView>();
				foreach (var instance in instancesInfo)
				{


					if (_InstancesRef.ContainsKey(instance.LegacyInstanceGuid))
						_InstancesRef[instance.LegacyInstanceGuid].ServiceInstanceInfo = instance;
					else
					{
						InstanceView iv = new InstanceView() { ServiceInstanceInfo = instance };
						_InstancesRef[instance.LegacyInstanceGuid] = iv;
						if (iv.ParentID == Guid.Empty)
							Instances.Add(iv);
						else
							childs.Add(iv);
					}

				}
				foreach (var child in childs)
				{
					if (_InstancesRef.ContainsKey(child.ParentID))
						_InstancesRef[child.ParentID].ChildsSteps.Add(child);
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



}
