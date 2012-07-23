using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Edge.Core.Scheduling.Objects;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Edge.Applications.PM.SchedulerControl.Objects
{
	public class UnPlannedBindingData : INotifyPropertyChanged
	{
		public ObservableCollection<UnplannedView> UnplannedViewCollection { get; set; }

		public ConflictBehavior[] ConflictBehaviors { get; set; }
		public UnPlannedBindingData(List<Profile> AccountsServiceInformation)
		{

			UnplannedViewCollection = new ObservableCollection<UnplannedView>();
			foreach (Profile accountServiceInformation in AccountsServiceInformation.OrderBy(p => p.Name))
			{
				UnplannedView unplanned = new UnplannedView(accountServiceInformation, UnplanedType.Account, null, null);
				unplanned.Services = new ObservableCollection<UnplannedView>();
				foreach (var ServiceConfiguration in accountServiceInformation.ServiceConfigurations)
				{
					unplanned.Services.Add(new UnplannedView(accountServiceInformation, UnplanedType.Service, ServiceConfiguration.Name, unplanned));

				}
				UnplannedViewCollection.Add(unplanned);
			}
			ConflictBehaviors = (ConflictBehavior[])Enum.GetValues(typeof(ConflictBehavior));

			RaisePropertyChanged("UnplannedViewCollection");
		}
		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;
		public void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion
	}
	public class UnplannedView : INotifyPropertyChanged
	{
		private bool _isChecked;
		public bool _useTargetPeriod; 
		private Dictionary<string, string> _options;
		public Dictionary<string, string> Options
		{
			get
			{
				return _options;
			}
			set
			{

			}
		}
		public bool UseTargetPeriod
		{
			get
			{
				return _useTargetPeriod;
			}
			set
			{
				_useTargetPeriod = value;
				RaisePropertyChanged("UseTargetPeriod");
			}
		}
		private string _serviceToRun;
		private string _serviceName;
		private ObservableCollection<String> _availableServices = new ObservableCollection<string>();
		private UnplanedType _unplanedType;
		public UnplannedView ParentAccount { get; set; }
		public UnplanedType UnplanedType
		{
			get
			{
				return _unplanedType;
			}
		}
		private Profile _accounServiceInformation = null;
		private ConflictBehavior _conflictBehvior = ConflictBehavior.Ignore;
		public ConflictBehavior ConflictBehvior
		{
			get
			{
				return _conflictBehvior;
			}
			set
			{
				_conflictBehvior = value;
				RaisePropertyChanged("ConflictBehvior");
			}
		}
		public ObservableCollection<UnplannedView> Services { get; set; }
		public ObservableCollection<String> AvailableServices
		{
			get
			{
				return _availableServices;
			}
			set
			{
				_availableServices = value;
			}
		}
		public UnplannedView(Profile accounServiceInformation, UnplanedType type, string serviceName, UnplannedView parent)
		{
			_options = new Dictionary<string, string>();


			_accounServiceInformation = accounServiceInformation;
			_unplanedType = type;
			if (type == Objects.UnplanedType.Service)
			{
				_serviceName = serviceName;
				ParentAccount = parent;

			}
			foreach (var available in _accounServiceInformation.ServiceConfigurations)
				_availableServices.Add(available.Name);




			RaisePropertyChanged("AccountName");
		}
		public int AccountID
		{
			get
			{
				return _accounServiceInformation.ID;
			}
		}
		public string AccountName
		{
			get
			{
				return _accounServiceInformation.Name;
			}
		}
		public string Display
		{
			get
			{
				if (_unplanedType == Objects.UnplanedType.Account)
					return string.Format("{0} ({1})", AccountName, AccountID);
				else
					return _serviceName;
			}
			set
			{
				ServiceToRun = value;
			}


		}
		public string ServiceToRun
		{
			get
			{

				return _serviceToRun;
			}
			set
			{
				_serviceToRun = value;
				RaisePropertyChanged("ServiceToRun");
			}
		}
		public bool IsSelected { get; set; }
		public bool IsChecked
		{
			get
			{
				return _isChecked;
			}
			set
			{
				_isChecked = value;
				IsSelected = true;
				RaisePropertyChanged("IsChecked");
				RaisePropertyChanged("IsSelected");

			}
		}
		public string ServiceName
		{
			get
			{
				return _serviceName;

			}
		}


		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;
		public void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion
	}

	public enum ConflictBehavior
	{
		Ignore,
		Abort
	}
	public enum UnplanedType
	{
		Account,
		Service
	}

}
