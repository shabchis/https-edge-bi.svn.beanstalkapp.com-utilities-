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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Edge.Applications.PM.SchedulerControl
{
	/// <summary>
	/// Interaction logic for History.xaml
	/// </summary>
	public partial class frmHistoryView : Window
	{
		public static BindingData BindingData = new BindingData();
		public frmHistoryView()
		{			
			InitializeComponent();
			frmHistoryView.BindingData.History = new ObservableCollection<ServiceHistoryView>();
			frmHistoryView.BindingData.LoadHistory();
			this.DataContext = frmHistoryView.BindingData;		
			
		}



		
	}
	

}