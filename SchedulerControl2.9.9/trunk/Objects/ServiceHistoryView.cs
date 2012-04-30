﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Edge.Applications.PM.SchedulerControl.Objects
{
	public class ServiceHistoryView : INotifyPropertyChanged
	{
		#region members
		private ObservableCollection<ServiceHistoryView> _childsHistoryView = new ObservableCollection<ServiceHistoryView>();
		#endregion
		#region properties
		public int? InstanceID { get; set; }
		public string ServiceName { get; set; }
		public int AccountID { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public Core.Services.ServiceOutcome Outcome { get; set; }
		public string TargetPeriod { get; set; }
		public int? ParentInstanceID { get; set; }
		public string TimeScheduled { get; set; }
		public string TimeStarted { get; set; }
		public string TimeEnded { get; set; }
		public bool IsExpanded { get; set; }

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
		public ObservableCollection<ServiceHistoryView> ChildsHistoryView
		{
			get
			{
				return _childsHistoryView;
			}
		}
		#endregion
		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public Core.Services.ServiceState State { get; set; }

	}
}
