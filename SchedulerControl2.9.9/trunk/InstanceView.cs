using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Edge.Core.Scheduling.Objects;
using System.Collections.ObjectModel;

namespace Edge.Applications.PM.SchedulerControl
{
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
				_childsSteps = value;
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
				ParentID = _instanceInfo.ParentInstanceID;
				RaiseAllPropertyChange();
			}

		}
		public Guid ParentID;
		public InstanceView()
		{
			_childsSteps.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_childsSteps_CollectionChanged);
			IsExpanded = true;
		}
		void _childsSteps_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			RaisePropertyChanged("ChildsSteps");
		}
		public Guid ID
		{
			get
			{
				return _instanceInfo.LegacyInstanceGuid;
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
		public string SchdeuleStartTime
		{
			get
			{
				return _instanceInfo.SchdeuleStartTime.ToShortTimeString();
			}
		}
		public string ScheduleEndTime
		{
			get
			{
				return _instanceInfo.ScheduleEndTime.ToShortTimeString();
			}
		}
		public string ActualStartTime
		{
			get
			{
				return _instanceInfo.ActualStartTime.ToShortTimeString();
			}

		}
		public string ActualEndTime
		{
			get
			{
				return _instanceInfo.ActualEndTime.ToShortTimeString();
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
		public double Progress 
		{
			get
			{
				return _instanceInfo.Progress;
			}
			
		}
		public string Background
		{
			get
			{
				string color=string.Empty;
				switch (Outcome)
				{

					case Edge.Core.Services.ServiceOutcome.Unspecified:						
						break;
					case Edge.Core.Services.ServiceOutcome.Success:
						color = "Green";
						break;
					case Edge.Core.Services.ServiceOutcome.Failure:
						color = "Red";
						break;
					case Edge.Core.Services.ServiceOutcome.Aborted:
						break;
					case Edge.Core.Services.ServiceOutcome.Reset:
						break;
					default:
						break;
				}
				return color;
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
		private void RaiseAllPropertyChange()
		{
			foreach (var property in this.GetType().GetProperties())
			{
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs(property.Name));
			}

		}

		#endregion
	}
}
