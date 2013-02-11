using System;
using System.Collections.ObjectModel;
using Edge.Applications.PM.SchedulerControl.Infra;
using Edge.Core.Services;

namespace Edge.Applications.PM.SchedulerControl.Models
{
    public class ServiceInstanceModel : BaseNotifyPropertyChanged
    {
        #region Data Members

        private ServiceInstance _serviceInstance;
        private readonly ObservableCollection<ServiceInstanceModel> _childsSteps = new ObservableCollection<ServiceInstanceModel>(); 

        #endregion

        #region Properties

		private bool _isSelected;
		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				if (_isSelected != value)
				{
					_isSelected = value;
					NotifyPropertyChanged("IsSelected");
					
					// select all its childs
					if (ChildsSteps != null)
					{
						foreach (var child in ChildsSteps)
							child.IsSelected = value;
					}
				}
			}
		}

	    private bool _isExpanded = true;
	    public bool IsExpanded
	    {
			get { return _isExpanded; }
			set
			{
				if (_isExpanded != value)
				{
					_isExpanded = value;
					NotifyPropertyChanged("IsExpanded");
				}
			}
	    }

        public ServiceInstance Instance
        {
            get { return _serviceInstance; }
            set
            {
                _serviceInstance = value;
                NotifyAllPropertyChanged();
            }
        }

        public ObservableCollection<ServiceInstanceModel> ChildsSteps
        {
            get { return _childsSteps; }
        }

        public string ServiceName
        {
            get { return _serviceInstance.Configuration.ServiceName; }
        }

	    public string AccountName
	    {
			get { return _serviceInstance.Configuration.Profile != null ?_serviceInstance.Configuration.Profile.Name : string.Empty; }
	    }

        public string AccountID
        {
            get { return _serviceInstance.Configuration.Profile != null ? _serviceInstance.Configuration.Profile.Parameters["AccountID"].ToString() : string.Empty; }
        }

        public string InstanceID
        {
            get { return _serviceInstance.InstanceID.ToString(); }
        }

	    public string RequestedTimeStr
	    {
			get { return _serviceInstance.SchedulingInfo.RequestedTime.ToString("dd/MM/yyyy HH:mm:ss"); }
	    }

		public DateTime RequestedTime
		{
			get { return _serviceInstance.SchedulingInfo.RequestedTime; }
		}

	    public string MaxDeviation
	    {
			get { return _serviceInstance.SchedulingInfo.MaxDeviationAfter.ToString(@"hh\:mm\:ss"); }
	    }

	    public string ScheduledDate
	    {
			get { return _serviceInstance.SchedulingInfo.ExpectedStartTime.ToString("dd/MM/yyyy"); }
	    }

        public string ScheduledTime
        {
            // TODO: may be two seperate columns for expected start and end ?
			get { return string.Format("{0}-{1}", _serviceInstance.SchedulingInfo.ExpectedStartTime.ToString("HH:mm:ss"), _serviceInstance.SchedulingInfo.ExpectedEndTime.ToString("HH:mm:ss")); }
        }

        public string ActualStartTime
        {
			get { return _serviceInstance.TimeStarted.ToString("HH:mm:ss"); }
        }

        public string ActualEndTime
        {
			get { return _serviceInstance.TimeEnded.ToString("HH:mm:ss"); }
        }

        public double Progress
        {
            get { return _serviceInstance.Progress * 100; }
        }

        public string State
        {
            get { return _serviceInstance.State.ToString(); }
        }

		public SchedulingStatus SchedulingStatus
		{
			get { return _serviceInstance.SchedulingInfo.SchedulingStatus; }
		}

		public ServiceOutcome Outcome
		{
			get { return _serviceInstance.Outcome; }
		}

        #endregion

		#region Public Methods
		public void NotifyAllPropertyChanged()
		{
			foreach (var property in GetType().GetProperties())
			{
				NotifyPropertyChanged(property.Name);
			}
		} 
		#endregion
    }
}
