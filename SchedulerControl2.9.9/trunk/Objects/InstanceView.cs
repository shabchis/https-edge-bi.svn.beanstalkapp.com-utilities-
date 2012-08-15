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
	public class RequestView : INotifyPropertyChanged
	{
		#region members
		private SchedulingRequestInfo _requestInfo;
		private ObservableCollection<RequestView> _childsSteps = new ObservableCollection<RequestView>();
		#endregion
		#region properties
		public ObservableCollection<RequestView> ChildsSteps
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
		public SchedulingRequestInfo schedulingRequestInfo
		{
			get
			{
				return _requestInfo;
			}
			set
			{
				_requestInfo = value;
				ParentID = _requestInfo.ParentRequestID;
				RaiseAllPropertyChange();
			}
		}
		public Guid ParentID { get; set; }
		public Guid ID
		{
			get
			{
				return _requestInfo.RequestID;
			}
		}
		public string InstanceID
		{
			get
			{
				return _requestInfo.LegacyInstanceID.ToString();
			}
		}
		public string ServiceName
		{
			get
			{
				return _requestInfo.ServiceName;
			}
		}
		public int AccountID
		{
			get
			{
				return _requestInfo.ProfileID;
			}
		}
		public string ScheduledTime
		{
			get
			{
				//TimeSpan to = _requestInfo.ScheduleEndTime - _requestInfo.ScheduleStartTime;
				return string.Format("{0}-{1}", _requestInfo.ScheduledStartTime.ToShortTimeString(), _requestInfo.ScheduledEndTime.ToShortTimeString());
			}
		}
		public string ActualStartTime
		{
			get
			{
				return _requestInfo.ActualStartTime.ToShortTimeString();
			}
		}
		public string ActualEndTime
		{
			get
			{
				return _requestInfo.ActualEndTime.ToShortTimeString();
			}
		}
		public Core.Services.ServiceState State
		{
			get
			{
				return _requestInfo.ServiceState;
			}
		}
		public Core.Services.ServiceOutcome Outcome
		{
			get
			{
				return _requestInfo.ServiceOutcome;
			}
		}
		//public string DayCode
		//{
		//    get
		//    {
		//        return _instanceInfo.;
		//    }
		//}
		public string Options
		{
			get
			{
				return _requestInfo.Options.Definition;
			}
		}

		public bool IsExpanded { get; set; }
		public double Progress
		{
			get
			{
				return _requestInfo.Progress;
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
		public RequestView()
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
