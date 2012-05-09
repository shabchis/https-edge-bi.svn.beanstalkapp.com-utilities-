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
		ISchedulingCommunication _schedulingCommunication;
		public static UnPlannedBindingData BindingData;
		public frmUnPlanned(ISchedulingCommunication schedulingCommunication)
		{
			InitializeComponent();
			_schedulingCommunication = schedulingCommunication;
			frmUnPlanned.BindingData = new UnPlannedBindingData(_schedulingCommunication.GetServicesConfigurations());
			this.DataContext = frmUnPlanned.BindingData;
			
			
				
				

	


				
			


			
		}

		private void _accountsTreeUser_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{

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
			if (view!=null)
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
			foreach (UnplannedView unPlanedView in frmUnPlanned.BindingData.UnplannedViewCollection.Where(u => u.IsChecked == true && u.UnplanedType == UnplanedType.Service).ToList()) 
			{
				int accountID = unPlanedView.AccountID;
				string serviceName = unPlanedView.ServiceName;
				Dictionary<string, string> options;
				options = unPlanedView.Options;
				options["ConflictBehvior"] = unPlanedView.ConflictBehvior.ToString();
				if (!string.IsNullOrEmpty(unPlanedView.ServiceToRun))
					options["ServiceToRun"] = unPlanedView.ServiceToRun;

				if (_useTargetPeriod.IsChecked.Value)
				{
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
					options.Add(PipelineService.ConfigurationOptionNames.TargetPeriod, daterange.ToAbsolute().ToString());
				}

					


				
				
			}

		}
	}
}
