﻿using System;
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
		public UnPlannedBindingData(List<AccounServiceInformation> AccountsServiceInformation)
		{

			UnplannedViewCollection = new ObservableCollection<UnplannedView>();
			foreach (var accountServiceInformation in AccountsServiceInformation.OrderBy(p => p.AccountName))
			{
				UnplannedView unplanned = new UnplannedView(accountServiceInformation, UnplanedType.Account, null, null);
				unplanned.Services = new ObservableCollection<UnplannedView>();
				foreach (var serviceName in accountServiceInformation.Services)
				{
					unplanned.Services.Add(new UnplannedView(accountServiceInformation, UnplanedType.Service, serviceName, unplanned));

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
		private string _serviceToRun;
		private string _serviceName;
		private UnplanedType _unplanedType;
		public UnplannedView ParentAccount { get; set; }
		public UnplanedType UnplanedType
		{
			get
			{
				return _unplanedType;
			}
		}
		private AccounServiceInformation _accounServiceInformation = null;
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
		public UnplannedView(AccounServiceInformation accounServiceInformation, UnplanedType type, string serviceName, UnplannedView parent)
		{
			_accounServiceInformation = accounServiceInformation;
			_unplanedType = type;
			if (type == Objects.UnplanedType.Service)
			{
				_serviceName = serviceName;
				ParentAccount = parent;
			}

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
				return _accounServiceInformation.AccountName;
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


		}



		public string ServiceToRun
		{
			get
			{
				if (string.IsNullOrEmpty(_serviceToRun))
					return Display;
				else
					return _serviceToRun;
			}
			set
			{
				_serviceToRun = value;
				RaisePropertyChanged("ServiceToRun");
			}
		}
		public bool IsSelected { get; set; }
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