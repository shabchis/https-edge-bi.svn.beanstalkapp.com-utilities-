using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Edge.Core.Configuration;
using System.Data.SqlClient;
using Edge.Core.Data;
using Edge.Data.Pipeline;
using Newtonsoft.Json;
using Edge.Data.Pipeline.Services;
using Edge.Core.Services;
using System.Configuration;
using System.Collections;
using System.IO;
using Edge.Applications.PM.Common;
using Edge.Applications.PM.Suite.DataChecks.Common;
using System.Reflection;
using Edge.Applications.PM.Suite.DataChecks.Configuration;

namespace Edge.Applications.PM.Suite.DataChecks
{
	public partial class DataChecksForm : ProductionManagmentBaseForm
	{

		#region Deligate members
		/*============================================================================================*/
		public delegate void UpdateProgressBar(ProgressBar progressBar, int val, bool visible);
		private UpdateProgressBar updateProgressBar;

		public delegate void UpdateResults(List<ValidationResult> results);
		private UpdateResults updateResults;
		/*============================================================================================*/
		#endregion


		private ResultForm resultsForm;
		public DataChecksModelView DataChecksModelView { set; get; }
		public Dictionary<string, Object> EventsHandlers { set; get; }

		public DataChecksForm()
		{
			#region Init Deligate functions
			/**************************************************************************/
			
			updateProgressBar = new UpdateProgressBar(updateProgressBarState);
			updateResults = new UpdateResults(updateResultsDataGrid);

			/**************************************************************************/
			#endregion

			//setting results form
			resultsForm = new ResultForm();
			resultsForm.MdiParent = this.ParentForm;

			DataChecksModelView = new DataChecksModelView();
			EventsHandlers = new Dictionary<string, Object>();

			//Adding Events Handler functions
			#region Attaching Events
			EventsHandlers.Add(Const.EventsTypes.ParentOutcomeReportedEvent, new EventHandler(instance_OutcomeReported));
			EventsHandlers.Add(Const.EventsTypes.ChildOutcomeReportedEvent, new EventHandler(instance_OutcomeReported));
			EventsHandlers.Add(Const.EventsTypes.ParentStateChangedEvent, new EventHandler<Edge.Core.Services.ServiceStateChangedEventArgs>(instance_StateChanged));
			EventsHandlers.Add(Const.EventsTypes.ChildStateChangedEvent, new EventHandler<ServiceStateChangedEventArgs>(child_instance_StateChanged));
			EventsHandlers.Add(Const.EventsTypes.ChildServiceRequested, new EventHandler<ServiceRequestedEventArgs>(instance_ChildServiceRequested));
			#endregion

			InitializeComponent();

			//Load Validation Types from configuration
			DataChecksModelView.LoadValidationTypesItems(this.ValidationTypes.Nodes);

			//Load Metrics Validations from configuration
			DataChecksModelView.LoadMetricsValidationsItems(this.MerticsValidations.Nodes);

		}

		//On Load
		private void DataChecks_Load(object sender, EventArgs e)
		{
			//Setting Dates to previous day
			fromDate.Value = DateTime.Today.AddDays(-1);
			toDate.Value = DateTime.Today.AddDays(-1);
		}


		#region UI Events Code Handler
		/*================================================================================*/

		// Selected Application Event //
		private void application_cb_SelectedValueChanged(object sender, EventArgs e)
		{
			string path = string.Empty;

			if (((ComboBox)sender).SelectedItem.Equals(Const.AdMetricsConst.EdgeApp))
			{
				path = Const.AdMetricsConst.EdgeProductionPathKey;
			}
			else
				path = Const.AdMetricsConst.SeperiaProductionPathKey;

			AccountsCheckedListBox.Items.Clear();

			this.profile_cb.Items.Clear();
			this.profile_cb.Text = "Select Profile";

			//Loading production configuration
			DataChecksModelView.LoadProductionConfiguration(path);

			//Loading Accounts from Data Base
			DataChecksModelView.LoadAccountsFromDB(application_cb.SelectedItem.ToString(), AccountsCheckedListBox, DataChecksModelView.AvailableAccountList);

			//Getting Profiles from configuration 
			if (!DataChecksModelView.TryGetProfilesFromConfiguration(path, profile_cb, DataChecksModelView.Profiles))
				DataChecksModelView.TryGetProfilesFromConfiguration(Const.AdMetricsConst.SeperiaProductionPathKey, profile_cb, DataChecksModelView.Profiles);

			rightSidePanel.Enabled = true;
		}

		private void profile_cb_SelectedValueChanged(object sender, EventArgs e)
		{
			//TO DO : LOAD PROFILE FROM PROFUCTION CONFIGURATION
		}

		private void Start_btn_Click(object sender, EventArgs e)
		{
			//FOREACH SELECTED METRIC VALIDATION RUN SERVICE.
			foreach (var MetricsValidation in this.DataChecksModelView.SelectedMetricsValidations)
			{
				//Run service using current configuration
				if (MetricsValidation.Value.RunHasLocal)
				{
					((DataChecksBase)MetricsValidation.Value).
						RunUsingLocalConfig(DataChecksModelView.GetSelectedValidationTypes(), this.AccountsCheckedListBox.SelectedItems, EventsHandlers);
				}
				//Run service using external configuration
				//else
				//	((DataChecksBase)MetricsValidation.Value).
				//	RunUsingExternalConfig(this.DataChecksModelView.SelectedValidationsTypes, this.AccountsCheckedListBox.SelectedItems);
			}
		}

		private void ValidationTypes_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			((ValidationType)e.Node.Tag).Checked = !((ValidationType)e.Node.Tag).Checked;
		}

		private void MerticsValidations_AfterCheck(object sender, TreeViewEventArgs e)
		{

			//ADDING SELECTED VALIDATION TO SELECTED_METRICS_VALIDATION LIST

			if (e.Node.Checked)
			{
				//Creating instance of validation Class.
				string typeName = string.Format("{0}.{1}", this.GetType().Namespace, ((MetricsItem)e.Node.Tag).ClassName);
				Type type = Type.GetType(typeName);
				DataChecksBase validationInstance = (AdMetricsValidation)Assembly.GetExecutingAssembly().CreateInstance(typeName);


				//SETTING INSTANCE PARAMS
				/******************************************************************/
				validationInstance.Channel = ((MetricsItem)e.Node.Tag).ChannelID;
				validationInstance.TimePeriod = GetTimePeriodFromTimePicker();
				validationInstance.RunHasLocal = Convert.ToBoolean(((MetricsItem)e.Node.Tag).RunHasLocal);
				/*****************************************************************/


				this.DataChecksModelView.SelectedMetricsValidations.Add(((MetricsItem)e.Node.Tag).Name, validationInstance);
			}
			else // Remove from selected
			{
				this.DataChecksModelView.SelectedMetricsValidations.Remove(((MetricsItem)e.Node.Tag).Name.ToString());
			}

		}

		/// <summary>
		/// Getting Time Period from Date Time Picker in UI
		/// </summary>
		/// <returns></returns>
		private DateTimeRange GetTimePeriodFromTimePicker()
		{

			return new DateTimeRange()
			{
				Start = new DateTimeSpecification()
				{
					BaseDateTime = fromDate.Value,
					Hour = new DateTimeTransformation() { Type = DateTimeTransformationType.Exact, Value = 0 },
					//Boundary = DateTimeSpecificationBounds.Lower
				},

				End = new DateTimeSpecification()
				{
					BaseDateTime = toDate.Value,
					Hour = new DateTimeTransformation() { Type = DateTimeTransformationType.Max },
					//Boundary = DateTimeSpecificationBounds.Upper
				}
			};
		}

		/*================================================================================*/
		#endregion

		#region Events Handler Section
		/*=========================================================================*/
		void instance_OutcomeReported(object sender, EventArgs e)
		{
			Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;

			/*TO DO :
			 * 
			 * UPDATE LOG
			 * UPDATE PROGRESS BAR
			 * WHEN ALL FINISHED SET RESULT IMAGE AND STATUS
			*/
		}

		void instance_StateChanged(object sender, Edge.Core.Services.ServiceStateChangedEventArgs e)
		{
			Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;

			if (e.StateAfter == Edge.Core.Services.ServiceState.Ready)
			{
				instance.Start();
			}
			if (e.StateAfter == Edge.Core.Services.ServiceState.Ended)
			{
				//TO DO : UPDATE UI PROGRESS BAR
				//TO DO : UPDATE RESULTS
				//TO DO : UPDATE LOG
			}

		}

		void instance_ChildServiceRequested(object sender, ServiceRequestedEventArgs e)
		{
			Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;

			e.RequestedService.OutcomeReported += new EventHandler(instance_OutcomeReported);
			e.RequestedService.StateChanged += new EventHandler<ServiceStateChangedEventArgs>(child_instance_StateChanged);


			e.RequestedService.Initialize();

		}

		void child_instance_StateChanged(object sender, Edge.Core.Services.ServiceStateChangedEventArgs e)
		{
			Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;


			if (e.StateAfter == Edge.Core.Services.ServiceState.Ready)
			{
				instance.Start();
			}

		}
		/*=========================================================================*/
		#endregion

		#region Deligate Functions
		/*====================================================================================*/
	
		public void updateProgressBarState(ProgressBar progressBarItem, int progress, bool visible = true)
		{
			progressBarItem.Value = progress;
			progressBarItem.Visible = visible;
		}

		public void updateResultsDataGrid(List<ValidationResult> results)
		{
			foreach (ValidationResult item in results)
			{
				if (item.ResultType == ValidationResultType.Error)
				{
					int rowId = resultsForm.ErrorDataGridView.Rows.Add(item.AccountID, item.CheckType, item.Message, item.ChannelID, item.TargetPeriodStart.Date.ToString());
					resultsForm.ErrorDataGridView.Rows[rowId].Tag = item;
				}
				else if (item.ResultType == ValidationResultType.Warning)
					resultsForm.WarningDataGridView.Rows.Add(item.AccountID, item.CheckType, item.Message, item.ChannelID, item.TargetPeriodStart.ToString());
				else
				{
					int rowId = resultsForm.SuccessDataGridView.Rows.Add(item.AccountID, item.CheckType, item.Message, item.ChannelID, item.TargetPeriodStart.ToString());
					resultsForm.SuccessDataGridView.Rows[rowId].Tag = item;
				}
			}

			//if (this._numOfProductionInstances == 0)
			//    report_btn.Enabled = true;

		}

		/*====================================================================================*/
		#endregion


	}


}
