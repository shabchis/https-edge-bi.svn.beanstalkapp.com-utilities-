﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Data;
using Edge.Applications.PM.SchedulerControl.Infra;
using Edge.Applications.PM.SchedulerControl.Models;
using Edge.Applications.PM.SchedulerControl.Zevel;
using Edge.Core.Services;
using System.Diagnostics;
using System.Windows.Input;
using Edge.Core.Utilities;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using System.Windows.Threading;
using System.Configuration;

namespace Edge.Applications.PM.SchedulerControl.ViewModels
{
	public sealed class MainViewModel : BaseNotifyPropertyChanged, IDisposable
	{
		#region Data Members
		private ServiceEnvironment _environment;
		private ServiceEnvironmentEventListener _listener;
		private readonly DispatcherTimer _cleanTimer = new DispatcherTimer();
		private readonly TimeSpan _removeServicesThreashold;
		
		//private ServiceExecutionHost _host;

		private readonly Dictionary<string, ServiceInstanceModel> _serviceInstanceMap = new Dictionary<string, ServiceInstanceModel>();
		private readonly ObservableCollection<ServiceInstanceModel> _serviceInstanceList = new ObservableCollection<ServiceInstanceModel>();
		#endregion

		#region Properties
		private readonly ICollectionView _view;
		public ICollectionView ServiceInstanceListView
		{
			get { return _view; }
		}

		public IList<string> SortableColumnsList { get; private set; }
		private string _sortedColumn;
		public string SortedColumn
		{
			get { return _sortedColumn; }
			set
			{
				if (_sortedColumn != value)
				{
					_sortedColumn = value;
					AddSorting();
				}
			}
		}

		public ICommand CollapseCommand { get; set; }
		public ICommand ExpandCommand { get; set; }
		public ICommand SelectAllCommand { get; set; }
		public ICommand UnselectAllCommand { get; set; }
		public ICommand RemoveSelectedCommand { get; set; }

		#endregion

		#region Ctor
		public MainViewModel()
		{
			log4net.Config.XmlConfigurator.Configure();
			Log.Start();

			CreateEnvironment();

			Log.Write(ToString(), "Scheduler Monitoring Tool started", LogMessageType.Debug);

			CollapseCommand = new DelegateCommand(() => Expand(false), () => true);
			ExpandCommand = new DelegateCommand(() => Expand(true), () => true);
			SelectAllCommand = new DelegateCommand(() => SelectAll(true), () => true);
			UnselectAllCommand = new DelegateCommand(() => SelectAll(false), () => true);
			RemoveSelectedCommand = new DelegateCommand(RemoveSelectedServices, () => true);

			_view = CollectionViewSource.GetDefaultView(_serviceInstanceList);
			SortedColumn = "Requested Time";

			InitSortableColumnsList();

			// init timer for remove old services
			var days = int.Parse(ConfigurationManager.AppSettings["ShowServicesDaysBack"]);
			_removeServicesThreashold = new TimeSpan(days,0,0,0);

			TimeSpan interval;
			TimeSpan.TryParse(ConfigurationManager.AppSettings["CleanServicesInterval"], out interval);
			_cleanTimer.Interval = interval;
			_cleanTimer.Tick += CleanTimer_Tick;
			_cleanTimer.Start();

			//GenerateSampleData();
		}

		#endregion

		#region Private Functions
		private void UpdateServiceList(List<ServiceInstance> updatedList)
		{
			try
			{
				// first of all update the flat Map with the new instances
				foreach (var instance in updatedList)
				{
					if (_serviceInstanceMap.ContainsKey(instance.InstanceID.ToString()))
					{
						// if service instance already exists, replace it's scheduling info and notify about the change
						_serviceInstanceMap[instance.InstanceID.ToString()].Instance.SchedulingInfo = instance.SchedulingInfo;
						_serviceInstanceMap[instance.InstanceID.ToString()].NotifyAllPropertyChanged();
					}
					else
					{
						// insert into the flat Map
						var instanceModel = new ServiceInstanceModel {Instance = instance};
						_serviceInstanceMap.Add(instanceModel.InstanceID, instanceModel);
						instance.Connect();
						instance.StateChanged += ServiceInstance_StateChanged;
					}
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
			catch (Exception ex)
			{
				Log.Write(ToString(), String.Format("Failed to update service instances, ex: {0}", ex.Message), LogMessageType.Error);
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

			_environment = ServiceEnvironment.Open("Scheduler UI" ,envConfig);
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

		private void Expand(bool toExpand)
		{
			// meanwhile no support for recursive expansion
			foreach (var serviceInstance in _serviceInstanceList)
			{
				serviceInstance.IsExpanded = toExpand;
			}
		}

		private void SelectAll(bool toSelect)
		{
			// recursive selection via IsSelect property
			foreach (var serviceInstance in _serviceInstanceList)
			{
				serviceInstance.IsSelected = toSelect;
			}
		}

		private void InitSortableColumnsList()
		{
			SortableColumnsList = new List<string> { "Service Name", "Account Name", "Requested Time", "State" };
		}
		
		private void AddSorting()
		{
			if (_view != null)
			{
				_view.SortDescriptions.Clear();
				_view.SortDescriptions.Add(new SortDescription(SortedColumn.Replace(" ", ""), ListSortDirection.Ascending));
				if (SortedColumn != "Requested Time")
				{
					_view.SortDescriptions.Add(new SortDescription("RequestedTime", ListSortDirection.Ascending));
				}
			}
		}

		private void RemoveServiceRecursively(ServiceInstanceModel service)
		{
			// clean all service childs recursively
			if (service.ChildsSteps != null)
			{
				foreach(var child in service.ChildsSteps)
				{
					RemoveServiceRecursively(child);
				}
				service.ChildsSteps.Clear();
			}
			_serviceInstanceMap.Remove(service.InstanceID);
		}

		private void RemoveSelectedServices()
		{
			var toRemoveList = _serviceInstanceList.Where(x => x.IsSelected).ToList();
			foreach (var service in toRemoveList)
			{
				// recursively remove childs
				RemoveServiceRecursively(service);
				// remove service itself from hierarchy
				_serviceInstanceList.Remove(service);
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
					// notify about the change, instance already contains the changes
					_serviceInstanceMap[instance.InstanceID.ToString()].NotifyAllPropertyChanged();
				}
			}
		}

		void Listener_ScheduleUpdated(object sender, ScheduleUpdatedEventArgs e)
		{
			Log.Write(ToString(), "Received scheduler updates", LogMessageType.Debug);
			UpdateServiceList(e.ServiceInstanceList);
		}

		void CleanTimer_Tick(object sender, EventArgs e)
		{
			if (_serviceInstanceList.Count == 0) return;

			lock (_serviceInstanceList)
			{
				for (int i = _serviceInstanceList.Count-1; i >= 0; i--)
				{
					if (_serviceInstanceList[i].RequestedTime.Add(_removeServicesThreashold) < DateTime.Now)
					{
						// remove childs recursively, remove from map and remove from hierarchy
						RemoveServiceRecursively(_serviceInstanceList[i]);
						
						_serviceInstanceList.RemoveAt(i);
					}
				}
			}
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
			foreach (var model in _serviceInstanceList)
			{
				model.Instance.Disconnect();
				model.Instance.StateChanged -= ServiceInstance_StateChanged;
			}
			if (_cleanTimer != null)
			{
				_cleanTimer.Stop();
				_cleanTimer.Tick -= CleanTimer_Tick;
			}
		} 
		#endregion
	}
}
