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
using Edge.Applications.PM.SchedulerControl.Objects;

namespace Edge.Applications.PM.SchedulerControl
{
	/// <summary>
	/// Interaction logic for Log.xaml
	/// </summary>
	public partial class Log : Window
	{
		public static LogBindingData BindingData;
		ServiceHistoryView _serviceHistoryView;
		public Log()
		{
			InitializeComponent();
		}
		public Log(ServiceHistoryView selectedService)
		{
			InitializeComponent();
			BindingData = new LogBindingData();
			BindingData.ServiceHistoryView = selectedService;
			BindingData.GetLogMessage();
			this.DataContext = Log.BindingData;

			
		}
	}
}
