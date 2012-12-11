using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using Edge.Applications.PM.SchedulerControl.Infra;
using Edge.Applications.PM.SchedulerControl.Models;
using Edge.Applications.PM.SchedulerControl.Zevel;
using Edge.Core.Services;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Edge.Applications.PM.SchedulerControl.ViewModels
{
	public class MainViewModel : BaseNotifyPropertyChanged, IDisposable
	{
		#region Data Members
		private ServiceEnvironment _environment;
		private ServiceEnvironmentEventListener _listener;
		//private ServiceExecutionHost _host;

		private readonly Dictionary<string, ServiceInstanceModel> _serviceInstanceMap = new Dictionary<string, ServiceInstanceModel>(); 
		#endregion

		#region Properties

		private ObservableCollection<ServiceInstanceModel> _serviceInstanceList = new ObservableCollection<ServiceInstanceModel>();
		public ObservableCollection<ServiceInstanceModel> ServiceInstanceList
		{
			get { return _serviceInstanceList; }
			set
			{
				if (_serviceInstanceList != value)
				{
					_serviceInstanceList = value;
					NotifyPropertyChanged("ServiceInstanceList");
				}
			}
		}

		public ICommand UpdateCommand { get; set; }
		public ICommand ReceiveUpdatesCommand { get; set; }

		#endregion

		#region Ctor
		public MainViewModel()
		{
			CreateEnvironment();

			// TODO - to remove
			//UpdateCommand = new DelegateCommand(UpdateService, CanUpdateService);
			
			//GenerateSampleData();
		}
		
		#endregion

		#region Private Functions
		private void UpdateServiceList(List<ServiceInstance> updatedList)
		{
			// first of all update the flat Map with the new instances
			foreach (var instance in updatedList)
			{
				if (_serviceInstanceMap.ContainsKey(instance.InstanceID.ToString()))
				{
					// if service instance already exists, replace the instance
					_serviceInstanceMap[instance.InstanceID.ToString()].Instance = instance;
				}
				else
				{
					// insert into the flat Map
					var instanceModel = new ServiceInstanceModel {Instance = instance};
					_serviceInstanceMap.Add(instanceModel.InstanceID, instanceModel);
					
				}
				instance.Connect();
				instance.StateChanged += ServiceInstance_StateChanged;
			}

			// update the hierarchy
			foreach (var instance in updatedList)
			{
				var instanceModel = _serviceInstanceMap[instance.InstanceID.ToString()];
				if (instance.ParentInstance == null)
				{
					if (!_serviceInstanceList.Contains(instanceModel))
					{
						_serviceInstanceList.Add(instanceModel);
					}
				}
				else
				{
					var parentInstance = _serviceInstanceMap[instance.ParentInstance.InstanceID.ToString()];
					if (parentInstance != null)
					{
						if (!parentInstance.ChildsSteps.Contains(instanceModel))
						{
							parentInstance.ChildsSteps.Add(instanceModel);
						}
					}
				}
			}
		}

		private void CreateEnvironment()
		{
			var envConfig = new ServiceEnvironmentConfiguration
			{
				DefaultHostName = "Johnny",
				ConnectionString = "Data Source=bi_rnd;Initial Catalog=EdgeSystem;Integrated Security=true",
				SP_HostListGet = "Service_HostList",
				SP_HostRegister = "Service_HostRegister",
				SP_HostUnregister = "Service_HostUnregister",
				SP_InstanceSave = "Service_InstanceSave",
				SP_InstanceGet = "Service_InstanceGet",
				SP_InstanceReset = "Service_InstanceReset",
				SP_EnvironmentEventListenerListGet = "Service_EnvironmentEventListenerListGet",
				SP_EnvironmentEventListenerRegister = "Service_EnvironmentEventListenerRegister",
				SP_EnvironmentEventListenerUnregister = "Service_EnvironmentEventListenerUnregister",
				SP_ServicesExecutionStatistics = "Service_ExecutionStatistics_GetByPercentile",
				SP_InstanceActiveListGet = "Service_InstanceActiveList_GetByTime"
			};

			_environment = ServiceEnvironment.Open(envConfig);
			_listener = _environment.ListenForEvents(ServiceEnvironmentEventType.ScheduleUpdated);
			_listener.ScheduleUpdated += Listener_ScheduleUpdated;
			
			//_host = new ServiceExecutionHost(_environment.EnvironmentConfiguration.DefaultHostName, _environment);
		}

		private void AddServiceInstanceModel2Map(ServiceInstanceModel model)
		{
			if (!_serviceInstanceMap.ContainsKey(model.InstanceID))
			{
				_serviceInstanceMap.Add(model.InstanceID, model);
			}
			else
			{
				Debug.WriteLine("Service instance {0} already exists and cannot be added", model.InstanceID);
			}
		}
		#endregion

		#region Zevel Functions - to be deleted!!!
		
		private void UpdateService()
		{
			var newServiceInstance = CreateServiceInstance("service New");
			if (newServiceInstance != null)
			{
				newServiceInstance.SchedulingInfo.ExpectedStartTime = DateTime.Now;
				newServiceInstance.SchedulingInfo.ExpectedEndTime = DateTime.Now.AddSeconds(30);
			}

			// replace existing instance
			_serviceInstanceList[0].Instance = newServiceInstance;
		}

		private bool CanUpdateService()
		{
			return true;
		}

		private void GenerateSampleData()
		{
			for (var i = 0; i < 10; i++)
			{
				var instanceModel = new ServiceInstanceModel 
					{ 
						Instance = CreateServiceInstance(String.Format("service{0}", i)) 
					};
				for (var j = 0; j < i; j++)
				{
					var instanseChild = new ServiceInstanceModel
						{
							Instance = CreateServiceInstance(String.Format("service{0} child{1}", i, j))
						};
					// add to childs and to map
					instanceModel.ChildsSteps.Add(instanseChild);
					AddServiceInstanceModel2Map(instanseChild);
				}
				// add to list and to map
				_serviceInstanceList.Add(instanceModel);
				AddServiceInstanceModel2Map(instanceModel);
			}
		} 

		private ServiceInstance CreateServiceInstance(string name)
		{
			var instance = _environment.NewServiceInstance(CreateServiceConfig(name));
			instance.SchedulingInfo = new SchedulingInfo
			{
				SchedulingStatus = SchedulingStatus.New,
				SchedulingScope = SchedulingScope.Unplanned,
				RequestedTime = DateTime.Now
			};
			instance.StateChanged += ServiceInstance_StateChanged;
			return instance;
		}

		private ServiceConfiguration CreateServiceConfig(string serviceName)
		{
			var serviceConfig = new ServiceConfiguration
			{
				IsEnabled = true,
				ServiceName = serviceName,
				ServiceClass = typeof(TestService).AssemblyQualifiedName,
				HostName = "Johnny",
				ConfigurationID = GetGuidFromString(serviceName),
				Limits = { MaxConcurrentPerTemplate = 1, MaxConcurrentPerProfile = 1 }
			};


			return serviceConfig;
		}

		private Guid GetGuidFromString(string key)
		{
			MD5 md5Hasher = MD5.Create();

			// Convert the input string to a byte array and compute the hash.
			byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(key));
			return new Guid(data);
		} 
		#endregion

		#region Events
		void ServiceInstance_StateChanged(object sender, EventArgs e)
		{
			var instance = sender as ServiceInstance;
			if (instance != null)
			{
				if (_serviceInstanceMap.ContainsKey(instance.InstanceID.ToString()))
				{
					// update the whole instance
					_serviceInstanceMap[instance.InstanceID.ToString()].Instance = instance;
				}
				// add if not exists???
			}
		}

		void Listener_ScheduleUpdated(object sender, ScheduleUpdatedEventArgs e)
		{
			UpdateServiceList(e.ServiceInstanceList);
		} 
		#endregion

		#region IDisposable
		public void Dispose()
		{
			if (_listener != null)
			{
				(_listener as IDisposable).Dispose();
				_listener.ScheduleUpdated -= Listener_ScheduleUpdated;
			}
			foreach (var model in ServiceInstanceList)
			{
				model.Instance.Disconnect();
				model.Instance.StateChanged -= ServiceInstance_StateChanged;
			}
		} 
		#endregion
	}
}
