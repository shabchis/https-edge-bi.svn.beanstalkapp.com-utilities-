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
using System.Windows.Data;
using System.Collections;

namespace Edge.Applications.PM.SchedulerControl.Objects
{
	public class HistoryBindingData : INotifyPropertyChanged
	{
		private List<Predicate<object>> _filters;

		public Predicate<object> Filter { get; private set; }
		public Times Times { get; set; }
		ICollectionView _historyView;
		private Dictionary<int, int> _accountsDic = new Dictionary<int, int>();
		private Dictionary<string, string> ServicesDic = new Dictionary<string, string>();
		public HistoryBindingData()
		{
			Times = new Objects.Times();
			_filters = new List<Predicate<object>>();
			Filter = InternalFilter;
			Outcomes = new ObservableCollection<OutComeView>(); ;
			foreach (var outcome in Enum.GetValues(typeof(ServiceOutcome)))
			{
				OutComeView outcomev = new OutComeView() { Outcome = (ServiceOutcome)outcome, Show = true };
				outcomev.PropertyChanged += new PropertyChangedEventHandler(outcomev_PropertyChanged);
				Outcomes.Add(outcomev);
			}
			States = new ObservableCollection<StateView>(); ;
			foreach (var state in Enum.GetValues(typeof(ServiceState)))
			{
				StateView statev = new StateView() { State = (ServiceState)state, Show = true };
				statev.PropertyChanged += new PropertyChangedEventHandler(statev_PropertyChanged);
				States.Add(statev);
			}
		}
		void statev_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_historyView == null)
				_historyView = CollectionViewSource.GetDefaultView(History);
			StateView sv = (StateView)sender;
			Predicate<object> filter = new Predicate<object>(sv.FileterByState);
			if (sv.Show)
				RemoveFilter(filter);
			else
				AddFilter(filter);
			_historyView.Filter = Filter;
		}
		private bool InternalFilter(object o)
		{
			foreach (var filter in _filters)
			{
				if (!filter(o))
				{
					return false;
				}
			}

			return true;
		}
		public void AddFilter(Predicate<object> filter)
		{
			_filters.Add(filter);
		}

		public void RemoveFilter(Predicate<object> filter)
		{

	
			if (_filters.Contains<Predicate<object>>(filter))
			{
				_filters.Remove(filter);
			}
		}    

		void outcomev_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_historyView == null)
				_historyView = CollectionViewSource.GetDefaultView(History);
			OutComeView ov = (OutComeView)sender;
			Predicate<object> filter = new Predicate<object>(ov.FilterByOutCome);
			if (ov.Show)
				RemoveFilter(filter);
			else
				AddFilter(filter);
			_historyView.Filter = Filter;
		}
		private Dictionary<int, ServiceHistoryView> _historyViewRef = new Dictionary<int, ServiceHistoryView>();
		public ObservableCollection<ServiceHistoryView> History { get; set; }
		public void LoadHistory()
		{
			if (_filters != null)
				_filters.Clear();
			if (_historyViewRef != null)
				_historyViewRef.Clear();
			if (History != null)
				History.Clear();
			using (SqlConnection conn = new SqlConnection(AppSettings.GetConnectionString(this, "History")))
			{
				conn.Open();
				{
					SqlCommand command = DataManager.CreateCommand(@"SELECT *
																    FROM ServiceInstance
																    WHERE TimeStarted BETWEEN @Form:DateTime AND @To:DateTime", CommandType.Text);
					command.Parameters["@Form"].Value = Times.From;
					command.Parameters["@To"].Value = Times.To;
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
							hv.State = (Core.Services.ServiceState)Enum.Parse(typeof(Core.Services.ServiceState), (reader["State"].ToString()));
							if (hv.ParentInstanceID.HasValue)
								childs.Add(hv);
							else
								History.Add(hv);
							if (!_historyViewRef.ContainsKey(hv.InstanceID.Value))
								_historyViewRef.Add(hv.InstanceID.Value, hv);
							if (!_accountsDic.ContainsKey(hv.AccountID))
							{
								_accountsDic.Add(hv.AccountID, hv.AccountID);
								if (Accounts == null)
									Accounts = new ObservableCollection<AccountView>();
								AccountView av = new AccountView() { AccountID = hv.AccountID, Show = true };
								av.PropertyChanged += new PropertyChangedEventHandler(accountView_PropertyChanged);
								Accounts.Add(av);
								
							}
							if (!ServicesDic.ContainsKey(hv.ServiceName) && !hv.ParentInstanceID.HasValue)
							{
								ServicesDic.Add(hv.ServiceName, hv.ServiceName);
								if (Services == null)
									Services = new ObservableCollection<ServiceView>();
								ServiceView sv = new ServiceView() { ServiceName = hv.ServiceName, Show = true };
								sv.PropertyChanged += new PropertyChangedEventHandler(sv_PropertyChanged);
								Services.Add(sv);

							}


						}
					}
					foreach (var child in childs)
						if (_historyViewRef.ContainsKey(child.ParentInstanceID.Value))
							_historyViewRef[child.ParentInstanceID.Value].ChildsHistoryView.Add(child);
				}
			}
		}

		void sv_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_historyView == null)
				_historyView = CollectionViewSource.GetDefaultView(History);
			ServiceView sv = (ServiceView)sender;
			Predicate<object> filter = new Predicate<object>(sv.FilterByServiceName);
			if (sv.Show)
				RemoveFilter(filter);
			else
				AddFilter(filter);
			_historyView.Filter = Filter;
		}
		void accountView_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_historyView == null)
				_historyView = CollectionViewSource.GetDefaultView(History);
			AccountView av = (AccountView)sender;
			Predicate<object> filter=new Predicate<object>(av.FilterByAccountID);			
			if (av.Show)
				RemoveFilter(filter);				
			else
				AddFilter(filter);
			_historyView.Filter = Filter;
		}
		public ObservableCollection<StateView> States { get; set; }
		public ObservableCollection<OutComeView> Outcomes { get; set; }
		public ObservableCollection<AccountView> Accounts { get; set; }
		public ObservableCollection<ServiceView> Services { get; set; }

		#region INotifyPropertyChanged Members

		

		#endregion





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

		public bool FilterByOutCome(object outcome)
		{
			if (((ServiceHistoryView)outcome).Outcome == Outcome)
				return false;
			else
				return true;

		}
	}
	public class StateView : INotifyPropertyChanged
	{
		private bool _show;
		public ServiceState State { get; set; }
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

		public bool FileterByState(object state)
		{
			if (((ServiceHistoryView)state).State == State)
				return false;
			else
				return true;
		}
	}
	public class AccountView : INotifyPropertyChanged
	{
		private bool _show;
		public int AccountID { get; set; }
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
		public bool FilterByAccountID(object account)
		{
			if (((ServiceHistoryView)account).AccountID == AccountID)
				return false;
			else
				return true;
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
	public class ServiceView : INotifyPropertyChanged
	{
		private bool _show;
		public string ServiceName { get; set; }
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
		public bool FilterByServiceName(object Service)
		{
			if (((ServiceHistoryView)Service).ServiceName == ServiceName)
				return false;
			else
				return true;
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
	public class Times : INotifyPropertyChanged
	{
		private DateTime _from;
		public DateTime From
		{
			get
			{
				return _from;

			}
			set
			{
				_from = value;
				RaisePropertyChanged("From");

			}
		}
		private DateTime _to;
		public DateTime To
		{
			get
			{
				return _to;

			}
			set
			{
				_to = value;
				RaisePropertyChanged("To");

			}
		}
		public Times()
		{
			From = DateTime.Now.AddHours(-2);
			To = DateTime.Now;
		}
		#region INotifyPropertyChanged Members
		private void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}



}
