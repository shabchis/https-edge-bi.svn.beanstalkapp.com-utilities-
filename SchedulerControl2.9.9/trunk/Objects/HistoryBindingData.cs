using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data.SqlClient;
using Edge.Core.Configuration;
using System.Data;
using Edge.Core.Data;
using System.Collections.ObjectModel;
using Edge.Core.Services;

namespace Edge.Applications.PM.SchedulerControl.Objects
{
	public class HistoryBindingData : INotifyPropertyChanged
	{
		public HistoryBindingData()
		{
			Outcomes= new ObservableCollection<OutComeView>(); ;
			foreach (var outcome in Enum.GetValues(typeof(ServiceOutcome)))
			{
				Outcomes.Add(new OutComeView() { Outcome = (ServiceOutcome)outcome });
			}
		}
		private Dictionary<int, ServiceHistoryView> _historyViewRef = new Dictionary<int, ServiceHistoryView>();
		public ObservableCollection<ServiceHistoryView> History { get; set; }
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
						if (_historyViewRef.ContainsKey(child.ParentInstanceID.Value))
							_historyViewRef[child.ParentInstanceID.Value].ChildsHistoryView.Add(child);
				}
			}
		}
		public ObservableCollection<OutComeView> Outcomes { get; set; }

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
	public class OutComeView : INotifyPropertyChanged
	{
		private bool _show;
		public ServiceOutcome Outcome { get; set; }
		public bool Show
		{
			get
			{
				return _show;
			}
			set
			{
				_show = value;
				RaisePropertyChanged("Show");

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
