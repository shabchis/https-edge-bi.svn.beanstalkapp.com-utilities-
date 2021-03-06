﻿using System;
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
using Edge.Core.Scheduling;
using System.ServiceModel;
using Edge.Applications.PM.SchedulerControl.Objects;
using Core = Edge.Core;
using System.Collections;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;
using Edge.Core.Configuration;
using System.Threading;
using System.Windows.Threading;
using Edge.Core.Services;



namespace Edge.Applications.PM.SchedulerControl
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region members
		public static SchedulerBindingData BindingData = new SchedulerBindingData();
		private DuplexChannelFactory<ISchedulingHost> _channel;
		private ISchedulingHost _schedulingCommunicationChannel;
		private Callback _callBack;
		Thread _clearEnded;
		#endregion
		#region ctor
		public MainWindow()
		{
			InitializeComponent();
			//this.DataContext = MainWindow.BindingData;
			//MainWindow.BindingData.LoadSchedulers();
		}
		#endregion
		#region events
		void _callBack_NewInstanceEvent(object sender, EventArgs e)
		{
			RequestsEventArgs ie = (RequestsEventArgs)e;
			List<ServiceInstance> schedulingRequestInfo = ie.requests;
			BindingData.UpdateInstances(schedulingRequestInfo);
		}
		
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			frmHistoryView f = new frmHistoryView(((SchedulerView)_combo.SelectedValue).Name);
			f.Show();
		}
		private void _btnConnect(object sender, RoutedEventArgs e)
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
			Guid guid = ((RequestView)((MenuItem)sender).DataContext).ID;
			_schedulingCommunicationChannel.Abort(guid);
		}
		private void _btnShowHistory_Click(object sender, RoutedEventArgs e)
		{
			frmHistoryView frmHistory = new frmHistoryView(((SchedulerView)_combo.SelectedValue).Name);
			frmHistory.Show();
		}
		private void MenuIsAlive_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region methods
		private void Disconnect()
		{
			
			_callBack.NewInstanceEvent -= new EventHandler(_callBack_NewInstanceEvent);
			MainWindow.BindingData.Disconnect();
			_channel.Close();
			BindingData.Connected = false;
		}
		private void Connect(string endPointConfigurationName)
		{


			_callBack = new Callback();
			_channel = new DuplexChannelFactory<ISchedulingHost>(_callBack, endPointConfigurationName);
			_schedulingCommunicationChannel = _channel.CreateChannel();
			_schedulingCommunicationChannel.Subscribe();
			_callBack.NewInstanceEvent += new EventHandler(_callBack_NewInstanceEvent);
			BindingData.Connected = true;
		}
		
		#endregion

		private void _chkClearAutoMaticly_Click(object sender, RoutedEventArgs e)
		{
			ThreadStart threadStart = new ThreadStart(delegate()
			{
				while (true)
				{
					Thread.Sleep(new TimeSpan(0, 2, 0));
					BindingData.ClearEnded();
				}
			});
			if (_clearEnded == null)
				_clearEnded = new Thread(threadStart);
			if (_chkClearAutoMaticly.IsChecked.Value)
				_clearEnded.Start();
			else
				_clearEnded.Abort();

		}

		private void _btnClearEnded_Click(object sender, RoutedEventArgs e)
		{
			BindingData.ClearEnded();
		}

		private void _btnLog_Click(object sender, RoutedEventArgs e)
		{
			Button btnLog = (Button)sender;

			RequestView iv = (RequestView)btnLog.DataContext;
			ServiceHistoryView shv = new ServiceHistoryView()
			{
				AccountID =int.Parse(iv.AccountID),
				ServiceName = iv.ServiceName,
				InstanceID = int.Parse(iv.InstanceID),
				Outcome = iv.Outcome
			};
			Log l = new Log(shv);
			l.Show();
		}

		private void _btnReset_Click(object sender, RoutedEventArgs e)
		{
			_schedulingCommunicationChannel.ResetUnended();
			MessageBox.Show("Done!");
		}

		private void _chkClearAutoMaticly_Click_1(object sender, RoutedEventArgs e)
		{

		}

		private void _btnUnPlanned_Click(object sender, RoutedEventArgs e)
		{

			Thread t = new Thread(new ThreadStart(delegate()
			{

				frmUnPlanned f = new frmUnPlanned(_schedulingCommunicationChannel);
				f.Show();
				System.Windows.Threading.Dispatcher.Run();
			}));
			t.IsBackground = true;
			if (t.TrySetApartmentState(ApartmentState.STA))
				t.Start();
		}




	}
}
