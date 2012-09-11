using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Edge.Core.Scheduling;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Edge.Core.Services;

namespace Edge.Applications.PM.SchedulerControl.Objects
{
	public class RequestView : INotifyPropertyChanged
	{
		#region members
		private ServiceInstance _requestInfo;
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
		public ServiceInstance schedulingRequestInfo
		{
			get
			{
				return _requestInfo;
			}
			set
			{
				_requestInfo = value;
				ParentID = _requestInfo.ParentInstance.InstanceID;
				RaiseAllPropertyChange();
			}
		}
		public Guid ParentID { get; set; }
		public Guid ID
		{
			get
			{
				return _requestInfo.InstanceID;
			}
		}
		public string InstanceID
		{
			get
			{
				return _requestInfo.InstanceID.ToString();
			}
		}
		public string ServiceName
		{
			get
			{
				return _requestInfo.Configuration.ServiceName;
			}
		}
		public string AccountID
		{
			get
			{
				return _requestInfo.Configuration.Profile.Parameters["AccountID"].ToString();
			}
		}
		public string ScheduledTime
		{
			get
			{
				//TimeSpan to = _requestInfo.ScheduleEndTime - _requestInfo.ScheduleStartTime;
				return string.Format("{0}-{1}", _requestInfo.SchedulingInfo.ExpectedStartTime.ToShortTimeString(), _requestInfo.SchedulingInfo.ExpectedEndTime.ToShortTimeString());
			}
		}
		public string ActualStartTime
		{
			get
			{
				return _requestInfo.TimeStarted.ToShortTimeString();
			}
		}
		public string ActualEndTime
		{
			get
			{
				return _requestInfo.TimeEnded.ToShortTimeString();
			}
		}
		public Core.Services.ServiceState State
		{
			get
			{
				return _requestInfo.State;
			}
		}
		public Core.Services.ServiceOutcome Outcome
		{
			get
			{
				return _requestInfo.Outcome;
			}
		}
		//public string DayCode
		//{
		//    get
		//    {
		//        return _instanceInfo.;
		//    }
		//}
		public string OutPut
		{
			get
			{
				return "should be the outcometext but the property gone with doron's changes";
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
					case Edge.Core.Services.ServiceOutcome.Killed:
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
				if (State == Core.Services.ServiceState.Ended || State == Core.Services.ServiceState.Ending)
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
