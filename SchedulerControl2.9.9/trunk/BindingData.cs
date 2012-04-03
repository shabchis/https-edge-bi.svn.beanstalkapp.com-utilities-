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

namespace Edge.Applications.PM.SchedulerControl
{
	public class BindingData : INotifyPropertyChanged
	{
		public void LoadHistory(DateTime? from = null, DateTime? to = null)
		{
			using (SqlConnection conn = new SqlConnection(AppSettings.GetConnectionString(this, "History")))
			{
				conn.Open();
				{
					SqlCommand command = DataManager.CreateCommand(@"SELECT *
																    FROM ServiceInstance
																    WHERE TimeStarted BETWEEN @Form:DateTime AND @To:DateTime", CommandType.Text);

					command.Parameters["@Form"].Value = from == null ? DateTime.Now.AddHours(-2) : from;
					command.Parameters["@To"].Value = to == null ? DateTime.Now : to;
					command.Connection = conn;
					List<ServiceHistoryView> childs = new List<ServiceHistoryView>();
					using (SqlDataReader reader = command.ExecuteReader())
					{

						while (reader.Read())
						{
							ServiceHistoryView hv = new ServiceHistoryView();
							hv.InstanceID = Convert.ToInt32(reader["InstanceID"]);
							hv.AccountID = Convert.ToInt32(reader["AccountID"]);
							hv.ParentInstanceID = reader["ParentInstanceID"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["ParentInstanceID"]);
							hv.ServiceName = reader["ServiceName"].ToString();
							hv.TimeScheduled = reader["TimeScheduled"].ToString();
							hv.TimeStarted = reader["TimeStarted"].ToString();
							hv.TimeEnded = reader["TimeEnded"].ToString();
							hv.Outcome = (Core.Services.ServiceOutcome)Enum.Parse(typeof(Core.Services.ServiceOutcome), (reader["Outcome"].ToString()));

							if (hv.ParentInstanceID.HasValue)
								childs.Add(hv);
							else
								History.Add(hv);
							_historyViewRef.Add(hv.InstanceID.Value, hv);

						}
					}
					foreach (var child in childs)
					{
						_historyViewRef[child.ParentInstanceID.Value].ChildsHistoryView.Add(child);

					}




				}

			}

		}
		InstanceView _currentInstance;
		Dictionary<Guid, InstanceView> _InstancesRef = new Dictionary<Guid, InstanceView>();
		Dictionary<int, ServiceHistoryView> _historyViewRef = new Dictionary<int, ServiceHistoryView>();
		public BindingData()
		{
			Instances = new ObservableCollection<InstanceView>();
			var collectionview = CollectionViewSource.GetDefaultView(Instances);
			collectionview.SortDescriptions.Add(new SortDescription("SchdeuleStartTime", ListSortDirection.Ascending));


		}
		public ObservableCollection<InstanceView> Instances { get; set; }
		public ObservableCollection<ServiceHistoryView> History { get; set; }
		public void UpdateInstances(ServiceInstanceInfo[] instancesInfo)
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
				List<int> toremove=new List<int>();

				for (int i = 0; i < Instances.Count; i++)			
				{

					if (Instances[i].ActualEndTime != "00:00")
					{
						DateTime ended = DateTime.Parse(Instances[i].ActualEndTime);

						if (ended.AddMinutes(2) < DateTime.Now)
						{
							toremove.Add(i);							
						}
					}
					
				}
				foreach (var item in toremove)
					Instances.RemoveAt(item);
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
		#endregion
	}
}
