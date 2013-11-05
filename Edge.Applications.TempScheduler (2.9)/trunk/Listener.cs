using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Edge.Core.Configuration;
using Edge.Core.Scheduling;
using Edge.Core.Scheduling.Objects;
using Edge.Core.Services;
using Edge.Core.Utilities;

namespace Edge.Applications.TempScheduler
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
	public class Listener : IScheduleManager, IDisposable
	{
		ServiceHost _wcfHost;
		Scheduler _scheduler;

		public Listener(Scheduler scheduler)
		{
			_scheduler = scheduler;
		}

		public void Start()
		{
			_wcfHost = new ServiceHost(this);
			_wcfHost.Open();
		}

		public void Close()
		{
			if (_wcfHost != null)
				_wcfHost.Close();
		}

		void IDisposable.Dispose()
		{
			this.Close();
		}

		void IScheduleManager.BuildSchedule()
		{
			throw new NotImplementedException();
		}

		public bool AddToSchedule(string serviceName, int accountID, DateTime targetTime, Edge.Core.SettingsCollection options)
		{
			AccountElement account = EdgeServicesConfiguration.Current.Accounts.GetAccount(accountID);
			if (account == null)
			{
				Log.Write(Program.LS, String.Format("Ignoring AddToSchedule request for account {0} which does not exist.", accountID), LogMessageType.Warning);
				return false;
			}

			AccountServiceElement service = account.Services[serviceName];
			if (service == null)
			{
				Log.Write(Program.LS, String.Format("Ignoring AddToSchedule request for service {0} which does not exist in account {1}.", serviceName, accountID), LogMessageType.Warning);
				return false;
			}

			var activeServiceElement = new ActiveServiceElement(service);
			ServicePriority priority = activeServiceElement.Options.ContainsKey("ServicePriority") ?
				(ServicePriority) Enum.Parse(typeof(ServicePriority), activeServiceElement.Options["ServicePriority"]) :
				ServicePriority.Normal;

			AddToSchedule(activeServiceElement, account, targetTime, options, priority);
			return true;
		}

		public void AddToSchedule(ActiveServiceElement activeServiceElement, AccountElement account, DateTime targetTime, Edge.Core.SettingsCollection options, ServicePriority servicePriority)
		{
			if (options != null)
			{
				foreach (string option in options.Keys)
					activeServiceElement.Options[option] = options[option];
			}

			//base configuration
			var baseConfiguration = new ServiceConfiguration()
			{
				Name = activeServiceElement.Name,
				MaxConcurrent = activeServiceElement.MaxInstances,
				MaxConcurrentPerProfile = activeServiceElement.MaxInstancesPerAccount
			};

			//configuration per profile
			var instanceConfiguration = new ServiceConfiguration()
			{
				BaseConfiguration = baseConfiguration,
				Name = activeServiceElement.Name,
				MaxConcurrent = (activeServiceElement.MaxInstances == 0) ? 9999 : activeServiceElement.MaxInstances,
				MaxConcurrentPerProfile = (activeServiceElement.MaxInstancesPerAccount == 0) ? 9999 : activeServiceElement.MaxInstancesPerAccount,
				LegacyConfiguration = activeServiceElement
			};

			//scheduling rules 
			instanceConfiguration.SchedulingRules.Add(new SchedulingRule()
			{
				Scope = SchedulingScope.UnPlanned,
				SpecificDateTime = targetTime,
				MaxDeviationAfter = TimeSpan.FromMinutes(30),
				Hours = new List<TimeSpan>(),
				GuidForUnplaned = Guid.NewGuid(),
			});

			instanceConfiguration.SchedulingRules[0].Hours.Add(new TimeSpan(0, 0, 0, 0));

			Profile profile = new Profile()
			{
				ID = account.ID,
				Name = account.ID.ToString(),
				Settings = new Dictionary<string, object>()
			};
			profile.Settings.Add("AccountID", account.ID);
			instanceConfiguration.SchedulingProfile = profile;

			_scheduler.AddNewServiceToSchedule(instanceConfiguration);
		}
		
	}
}
