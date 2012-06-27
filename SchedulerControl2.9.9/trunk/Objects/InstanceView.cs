using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Edge.Core.Scheduling.Objects;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Edge.Applications.PM.SchedulerControl.Objects
{
	public class InstanceView : INotifyPropertyChanged
	{
		#region members
		private ServiceInstanceInfo _instanceInfo;
		private ObservableCollection<InstanceView> _childsSteps = new ObservableCollection<InstanceView>();
		#endregion
		#region properties
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
		public Guid ParentID { get; set; }
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
		public string ScheduledTime
		{
			get
			{
				TimeSpan to = _instanceInfo.ScheduleEndTime - _instanceInfo.SchdeuleStartTime;
				return string.Format("{0}-{1}", _instanceInfo.SchdeuleStartTime.ToShortTimeString(), _instanceInfo.ScheduleEndTime.ToShortTimeString());
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
				return _instanceInfo.TargetPeriod;
			}
		}
		public string Options
		{
			get
			{
				return _instanceInfo.Options;
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
				string color = string.Empty;
				switch (Outcome)
				{
					case Edge.Core.Services.ServiceOutcome.Unspecified:
						break;
					case Edge.Core.Services.ServiceOutcome.Success:
						color = Colors.LawnGreen.ToString();
						break;
					case Edge.Core.Services.ServiceOutcome.Failure:
						color = Colors.Red.ToString();
						break;
					case Edge.Core.Services.ServiceOutcome.Aborted:
						color = Colors.Purple.ToString();
						break;
					case Edge.Core.Services.ServiceOutcome.Reset:
						break;
					default:
						break;
				}
				return color;
			}
		}
		public string StatusImage
		{
			get
			{
				switch (Outcome)
				{					
					case Core.Services.ServiceOutcome.Failure:
						return "/Icons/failed.ico";					
					case Core.Services.ServiceOutcome.Success:
						return "/Icons/success.ico";					
					default:
						return string.Empty;
				}
			}
		}
		public bool LogEnabled
		{
			get
			{
				if (State == Core.Services.ServiceState.Ended || State == Core.Services.ServiceState.Aborting)
					return true;
				else
					return false;
			}
		}
		#endregion
		#region ctor
		public InstanceView()
		{
			_childsSteps.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_childsSteps_CollectionChanged);
			IsExpanded = true;
		}
		#endregion
		#region events
		void _childsSteps_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			RaisePropertyChanged("ChildsSteps");
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
		#endregion
	}
}
