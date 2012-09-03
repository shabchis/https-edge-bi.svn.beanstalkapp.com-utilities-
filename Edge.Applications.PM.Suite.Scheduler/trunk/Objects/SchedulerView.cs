using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Edge.Applications.PM.SchedulerControl.Objects
{
	public class SchedulerView : INotifyPropertyChanged
	{
		#region Properties
		public string Name { get; set; }
		public string EndPointAddress { get; set; }
		#endregion
		#region INotifyPropertyChanged Members
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
