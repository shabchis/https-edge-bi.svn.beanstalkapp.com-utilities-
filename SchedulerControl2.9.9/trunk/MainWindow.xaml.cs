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


namespace Edge.Applications.PM.SchedulerControl
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static BindingData BindingData = new BindingData();
		private DuplexChannelFactory<ISchedulingCommunication> _channel;
		private ISchedulingCommunication _schedulingCommunicationChannel;
		private Callback _callBack;

		public MainWindow()
		{
			InitializeComponent();
			SubscribeToScheduler();


			this.DataContext = MainWindow.BindingData;
		}

		private void SubscribeToScheduler()
		{
			
		}
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
			Connect();
			
		}

		private void Connect()
		{
			_callBack = new Callback();
			_channel = new DuplexChannelFactory<ISchedulingCommunication>(_callBack,"SchedulerCommunication");				
			_schedulingCommunicationChannel = _channel.CreateChannel();
			_schedulingCommunicationChannel.Subscribe();

			_callBack.NewScheduleCreatedEvent += new EventHandler(_callBack_NewScheduleCreatedEvent);
			_callBack.NewInstanceEvent += new EventHandler(_callBack_NewInstanceEvent);
		}
	}
	



}
