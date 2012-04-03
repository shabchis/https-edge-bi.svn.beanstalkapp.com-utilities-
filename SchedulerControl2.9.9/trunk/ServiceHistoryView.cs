using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Edge.Applications.PM.SchedulerControl
{
	public class ServiceHistoryView : INotifyPropertyChanged
	{

		public int? InstanceID { get; set; }
		public string ServiceName { get; set; }
		public int AccountID { get; set; }		
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public Core.Services.ServiceOutcome Outcome { get; set; }
		public int DayCode { get; set; }
		public int? ParentInstanceID { get; set; }
		public string TimeScheduled { get; set; }
		public string TimeStarted { get; set; }
		public string TimeEnded { get; set; }
		public bool IsExpanded { get; set; }
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
						color = Colors.LawnGreen.ToString();;
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
		
		public ObservableCollection<ServiceHistoryView> ChildsHistoryView
		{
			get
			{
				return _childsHistoryView;
			}

		}
		private ObservableCollection<ServiceHistoryView> _childsHistoryView = new ObservableCollection<ServiceHistoryView>();



		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
