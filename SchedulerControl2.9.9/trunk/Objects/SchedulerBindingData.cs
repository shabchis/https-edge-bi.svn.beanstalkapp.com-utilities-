using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using Edge.Core.Scheduling;
using System.Data.SqlClient;
using System.Data;
using Edge.Core.Configuration;
using Edge.Core.Services;

namespace Edge.Applications.PM.SchedulerControl.Objects
{
	public class SchedulerBindingData : INotifyPropertyChanged
	{
		#region Members
		private bool _connected;
		private Dictionary<Guid, RequestView> _requestsRef = new Dictionary<Guid, RequestView>();
		
		#endregion
		#region ctor
		public SchedulerBindingData()
		{
			Requests = new ObservableCollection<RequestView>();
			var collectionview = CollectionViewSource.GetDefaultView(Requests);
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
		public void UpdateInstances(List<ServiceInstance> requestInfo)
		{
			lock (Requests)
			{
				List<RequestView> childs = new List<RequestView>();
				foreach (var request in requestInfo)
				{
					if (_requestsRef.ContainsKey(request.InstanceID))
						_requestsRef[request.InstanceID].schedulingRequestInfo = request;
					else
					{
						RequestView rv = new RequestView() { schedulingRequestInfo = request };
						_requestsRef[request.InstanceID] = rv;
						if (rv.ParentID == Guid.Empty)
							Requests.Add(rv);
						else
							childs.Add(rv);
					}
				}
				foreach (var child in childs)
				{
					if (_requestsRef.ContainsKey(child.ParentID))
						_requestsRef[child.ParentID].ChildsSteps.Add(child);
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
		public ObservableCollection<RequestView> Requests { get; set; }
		
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
			_requestsRef.Clear();
			Requests.Clear();
		}
		internal void ClearEnded()
		{

			lock (Requests)
			{
				List<int> toRemove = new List<int>();
				for (int i = 0; i < Requests.Count; i++)
				{
					RequestView instance = Requests[i];
					if (instance.State == Core.Services.ServiceState.Ended && DateTime.Parse(instance.ActualEndTime).AddMinutes(5) < DateTime.Now)
					{
						foreach (var child in instance.ChildsSteps)
						{
							_requestsRef.Remove(child.ID);
						}
						_requestsRef.Remove(instance.ID);
						toRemove.Add(i);
					}
				}
				foreach (var index in toRemove)
					Requests.RemoveAt(index);		 
			}
		
		}
	}
}
