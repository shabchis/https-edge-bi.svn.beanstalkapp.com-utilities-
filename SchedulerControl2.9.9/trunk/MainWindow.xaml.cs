using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Edge.Core.Scheduling.Objects;
using System.ServiceModel;
using Edge.Applications.PM.SchedulerControl.Objects;
using Core = Edge.Core;
using System.Collections;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;
using Edge.Core.Configuration;
using Edge.Core.Data;
using Legacy = Edge.Core.Services;
using System.Threading;
using System.Windows.Threading;


namespace Edge.Applications.PM.SchedulerControl
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region members
		public static SchedulerBindingData BindingData = new SchedulerBindingData();
		private DuplexChannelFactory<ISchedulingCommunication> _channel;
		private ISchedulingCommunication _schedulingCommunicationChannel;
		private Callback _callBack;
		#endregion
		#region ctor
		public MainWindow()
		{
			InitializeComponent();			
			this.DataContext = MainWindow.BindingData;
			MainWindow.BindingData.LoadSchedulers();
		}
		#endregion		
		#region events
		void _callBack_NewInstanceEvent(object sender, EventArgs e)
		{
			InstanceEventArgs ie = (InstanceEventArgs)e;
			ServiceInstanceInfo serviceInstanceInfo = ie.instanceStateOutcomerInfo;
			BindingData.UpdateInstances(new ServiceInstanceInfo[] { serviceInstanceInfo });
		}
		void _callBack_NewScheduleCreatedEvent(object sender, EventArgs e)
		{
			ScheduleCreatedEventArgs scheduleCreatedEventArgs = (ScheduleCreatedEventArgs)e;
			BindingData.UpdateInstances(scheduleCreatedEventArgs.ScheduleAndStateInfo);
		}
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			frmHistoryView f = new frmHistoryView();
			f.Show();
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (_combo.SelectedValue != null)
			{
				if (!MainWindow.BindingData.Connected)
					Connect(((SchedulerView)_combo.SelectedValue).EndPointAddress);
				else
					if (MainWindow.BindingData.Connected)
						Disconnect();
			}
		}
		private void Abort_Click(object sender, RoutedEventArgs e)
		{
			Guid guid = ((InstanceView)((MenuItem)sender).DataContext).ID;
			_schedulingCommunicationChannel.Abort(guid);
		}
		private void _btnShowHistory_Click(object sender, RoutedEventArgs e)
		{
			frmHistoryView frmHistory = new frmHistoryView();
			frmHistory.Show();
		}
		private void MenuIsAlive_Click(object sender, RoutedEventArgs e)
		{
			Guid guid = ((InstanceView)((MenuItem)sender).DataContext).ID;
			ThreadStart start = delegate()
			{
				Dispatcher.Invoke(new Action<Guid>(IsAlive), new object[] { guid });
			};
			new Thread(start).Start();
		}
		#endregion
		#region methods
		private void Disconnect()
		{
			_callBack.NewScheduleCreatedEvent -= new EventHandler(_callBack_NewScheduleCreatedEvent);
			_callBack.NewInstanceEvent -= new EventHandler(_callBack_NewInstanceEvent);
			MainWindow.BindingData.Disconnect();			
			_channel.Close();
			BindingData.Connected = false;
		}
		private void Connect(string endPointConfigurationName)
		{
			_callBack = new Callback();
			_channel = new DuplexChannelFactory<ISchedulingCommunication>(_callBack, endPointConfigurationName);
			_schedulingCommunicationChannel = _channel.CreateChannel();
			_schedulingCommunicationChannel.Subscribe();
			_callBack.NewScheduleCreatedEvent += new EventHandler(_callBack_NewScheduleCreatedEvent);
			_callBack.NewInstanceEvent += new EventHandler(_callBack_NewInstanceEvent);
			BindingData.Connected = true;
		}		
		private void IsAlive(Guid guid)
		{
			try
			{
				Legacy.IsAlive isAlive = _schedulingCommunicationChannel.IsAlive(guid);
				MessageBox.Show(string.Format("State: {0}\n OutCome: {1}\n Progress: {2}", isAlive.State, isAlive.OutCome, isAlive.Progress));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		#endregion

		private void _chkClearAutoMaticly_Click(object sender, RoutedEventArgs e)
		{

		}

		private void _btnClearEnded_Click(object sender, RoutedEventArgs e)
		{
			BindingData.ClearEnded();
		}

		private void _btnLog_Click(object sender, RoutedEventArgs e)
		{
			Button btnLog = (Button)sender;

			InstanceView iv = (InstanceView)btnLog.DataContext;
			ServiceHistoryView shv = new ServiceHistoryView()
			{
				AccountID = iv.AccountID,
				ServiceName=iv.ServiceName,
				InstanceID=int.Parse(iv.InstanceID),
				Outcome=iv.Outcome
			};
			Log l = new Log(shv);
			l.Show();
		}

	}
}
