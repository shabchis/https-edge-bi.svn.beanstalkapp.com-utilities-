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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Edge.Applications.PM.SchedulerControl.Objects;



namespace Edge.Applications.PM.SchedulerControl
{
	/// <summary>
	/// Interaction logic for History.xaml
	/// </summary>
	public partial class frmHistoryView : Window
	{
		public static HistoryBindingData BindingData;
		public frmHistoryView()
		{			
			InitializeComponent();
			BindingData = new HistoryBindingData();
			frmHistoryView.BindingData.History = new ObservableCollection<ServiceHistoryView>();
			
			frmHistoryView.BindingData.LoadHistory();
			this.DataContext = frmHistoryView.BindingData;
			

		    			
		}

		private void _loadHistory_Click(object sender, RoutedEventArgs e)
		{
			frmHistoryView.BindingData.LoadHistory();
		
		}

		private void _stateCheckAll_Click(object sender, RoutedEventArgs e)
		{
			foreach (var item in frmHistoryView.BindingData.States)
				item.Show = _stateCheckAll.IsChecked.Value;			
		}

		private void _AccountsCheckAll_Click(object sender, RoutedEventArgs e)
		{
			foreach (var item in frmHistoryView.BindingData.Accounts)
				item.Show = _AccountsCheckAll.IsChecked.Value;	
		}

		private void _servicesCheckAll_Click(object sender, RoutedEventArgs e)
		{
			foreach (var item in frmHistoryView.BindingData.Services)
				item.Show = _servicesCheckAll.IsChecked.Value;	
		}

		private void _outComeCheckAll_Click(object sender, RoutedEventArgs e)
		{
			foreach (var item in frmHistoryView.BindingData.Outcomes)
				item.Show = _outComeCheckAll.IsChecked.Value;	

		}

		private void _btnLog_Click(object sender, RoutedEventArgs e)
		{
			Button btnLog = (Button)sender;

			ServiceHistoryView shv = (ServiceHistoryView)btnLog.DataContext;
			Log l = new Log(shv);
			l.Show();

		}

		


		

	


		
	}
	

}
