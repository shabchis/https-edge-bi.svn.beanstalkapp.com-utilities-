using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using Edge.Core.Scheduling.Objects;
using System.Data.SqlClient;
using Edge.Core.Data;
using System.Data;
using Edge.Core.Configuration;

namespace Edge.Applications.PM.SchedulerControl.Objects
{
	public class SchedulerBindingData : INotifyPropertyChanged
	{
		#region Members
		private bool _connected;
		private Dictionary<Guid, InstanceView> _InstancesRef = new Dictionary<Guid, InstanceView>();
		
		#endregion
		#region ctor
		public SchedulerBindingData()
		{
			Instances = new ObservableCollection<InstanceView>();
			var collectionview = CollectionViewSource.GetDefaultView(Instances);
			collectionview.SortDescriptions.Add(new SortDescription("ScheduledTime", ListSortDirection.Ascending));
		}
		#endregion
		#region LoadData
		
		public void LoadSchedulers()
		{
			using (SqlConnection conn = new SqlConnection(AppSettings.GetConnectionString(this, "Shcedulers")))
			{
				conn.Open();
				using (SqlCommand command = new SqlCommand(@"SELECT Name,EndPointConfigurationName FROM Schedulers", conn))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (Schedulers == null)
							Schedulers = new ObservableCollection<SchedulerView>();
						while (reader.Read())
							Schedulers.Add(new SchedulerView() { Name = reader["Name"].ToString(), EndPointAddress = reader["EndPointConfigurationName"].ToString() });
					}
				}
			}
		}
		public void UpdateInstances(List<ServiceInstanceInfo> instancesInfo)
		{
			lock (Instances)
			{
				List<InstanceView> childs = new List<InstanceView>();
				foreach (var instance in instancesInfo)
				{
					if (_InstancesRef.ContainsKey(instance.LegacyInstanceGuid))
						_InstancesRef[instance.LegacyInstanceGuid].ServiceInstanceInfo = instance;
					else
					{
						InstanceView iv = new InstanceView() { ServiceInstanceInfo = instance };
						_InstancesRef[instance.LegacyInstanceGuid] = iv;
						if (iv.ParentID == Guid.Empty)
							Instances.Add(iv);
						else
							childs.Add(iv);
					}
				}
				foreach (var child in childs)
				{
					if (_InstancesRef.ContainsKey(child.ParentID))
						_InstancesRef[child.ParentID].ChildsSteps.Add(child);
				}
				//TODO: ADD REMOVE ENDED 
			}



		}
		#endregion
		#region Properties
		public bool Connected
		{
			set
			{
				_connected = value;
				RaisePropertyChanged("ConnectButtonText");
				RaisePropertyChanged("Connected");
			}
			get
			{
				return _connected;
			}
		}
		public string ConnectButtonText
		{
			get
			{
				if (Connected)
					return "DisConnect";
				else
					return "Connect";
			}
		}
		public ObservableCollection<InstanceView> Instances { get; set; }
		
		public ObservableCollection<SchedulerView> Schedulers { get; set; }
		#endregion
		#region INotifyPropertyChanged Members
		public event PropertyChangedEventHandler PropertyChanged;
		private void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion
		internal void Disconnect()
		{
			_InstancesRef.Clear();
			Instances.Clear();
		}
		internal void ClearEnded()
		{

			lock (Instances)
			{
				List<int> toRemove = new List<int>();
				for (int i = 0; i < Instances.Count; i++)
				{
					InstanceView instance = Instances[i];
					if (instance.State == Core.Services.ServiceState.Ended && DateTime.Parse(instance.ActualEndTime).AddMinutes(5) < DateTime.Now)
					{
						foreach (var child in instance.ChildsSteps)
						{
							_InstancesRef.Remove(child.ID);
						}
						_InstancesRef.Remove(instance.ID);
						toRemove.Add(i);
					}
				}
				foreach (var index in toRemove)
					Instances.RemoveAt(index);		 
			}
		
		}
	}
}
