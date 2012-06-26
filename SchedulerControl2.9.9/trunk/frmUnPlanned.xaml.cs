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
using Edge.Core.Scheduling;
using Edge.Core.Scheduling.Objects;
using Edge.Data.Pipeline;
using Edge.Data.Pipeline.Services;

namespace Edge.Applications.PM.SchedulerControl
{
	/// <summary>
	/// Interaction logic for frmUnPlanned.xaml
	/// </summary>
	public partial class frmUnPlanned : Window
	{
		ISchedulingHost _schedulingHost;
		public static UnPlannedBindingData BindingData;
		public frmUnPlanned(ISchedulingHost schedulingHost)
		{
			InitializeComponent();
			_schedulingHost = schedulingHost;
			frmUnPlanned.BindingData = new UnPlannedBindingData(_schedulingHost.GetServicesConfigurations());
			this.DataContext = frmUnPlanned.BindingData;
		}



		private void _ClearOptions_Click(object sender, RoutedEventArgs e)
		{
			UnplannedView view = (UnplannedView)_accountsTree.SelectedItem;
			view.Options.Clear();
			view.RaisePropertyChanged("Options");
			_options.Items.Refresh();

		}

		private void _addOption_Click(object sender, RoutedEventArgs e)
		{
			UnplannedView view = (UnplannedView)_accountsTree.SelectedItem;
			if (view != null)
			{
				if (view.Options.ContainsKey(_optionName.Text))
					MessageBox.Show("Key allready exists!");
				else
				{
					view.Options.Add(_optionName.Text.Trim(), _optionValue.Text.Trim());
					view.RaisePropertyChanged("Options");
					_options.Items.Refresh();
				}
			}
		}

		private void _RemoveOption_Click(object sender, RoutedEventArgs e)
		{
			UnplannedView view = (UnplannedView)_accountsTree.SelectedItem;
			if (view != null)
			{
				if (_options.SelectedValue != null)
				{

					if (view.Options.ContainsKey(((KeyValuePair<string, string>)_options.SelectedValue).Key))
					{
						view.Options.Remove(((KeyValuePair<string, string>)_options.SelectedValue).Key);
						view.RaisePropertyChanged("Options");
						_options.Items.Refresh();
					}


				}
			}

		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//UnplannedView view = (UnplannedView)_accountsTree.SelectedItem;
			//if (view != null)
			//{
			//    if (_cmbsSrviceToRun.SelectedValue != null)
			//        view.ServiceToRun = ((UnplannedView)_cmbsSrviceToRun.SelectedValue).ServiceToRun;
			//}
		}



		private void _btnAddServicesClick(object sender, RoutedEventArgs e)
		{
			StringBuilder errors = new StringBuilder();

			foreach (UnplannedView accountView in frmUnPlanned.BindingData.UnplannedViewCollection)
			{
				foreach (var unPlanedView in accountView.Services)
				{


					if (unPlanedView.IsChecked)
					{
						try
						{
							int accountID = unPlanedView.AccountID;
							string serviceName = unPlanedView.ServiceName;
							Dictionary<string, string> options;
							options = unPlanedView.Options;
							options["ConflictBehvior"] = unPlanedView.ConflictBehvior.ToString();
							if (!string.IsNullOrEmpty(unPlanedView.ServiceToRun))
								options["ServiceToRun"] = unPlanedView.ServiceToRun;

							if (unPlanedView._useTargetPeriod)
							{
								if (_from.SelectedDate.Value > _to.SelectedDate.Value)
									throw new Exception("From Date can not be greater then to date");
								DateTimeRange daterange = new DateTimeRange()
								{
									Start = new DateTimeSpecification()
									{
										BaseDateTime = _from.SelectedDate.Value,
										Hour = new DateTimeTransformation() { Type = DateTimeTransformationType.Exact, Value = 0 },
									},
									End = new DateTimeSpecification()
									{
										BaseDateTime = _to.SelectedDate.Value,
										Hour = new DateTimeTransformation() { Type = DateTimeTransformationType.Max },

									}
								};
								options.Add(PipelineService.ConfigurationOptionNames.TimePeriod, daterange.ToAbsolute().ToString());
							}


							_schedulingHost.AddUnplanedService(accountID, serviceName, options, DateTime.Now);

						}
						catch (Exception ex)
						{

							MessageBox.Show(ex.Message);
						} 
					}





				}
			

			}
			if (errors.Length == 0)
				MessageBox.Show("All Services Added Successfuly");
			else
				MessageBox.Show("Not all services added successfully:\n" + errors.ToString());

		}

	

		private void _useTargetPeriod_Click(object sender, RoutedEventArgs e)
		{
			UnplannedView view = (UnplannedView)_accountsTree.SelectedItem;
			if (view != null)
			{
				if (!_useTargetPeriod.IsChecked.Value)
				view.UseTargetPeriod = false;
				else
					view.UseTargetPeriod = true;
			}

		}
	}
}
