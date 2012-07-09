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
		private UpdateProgressBar _updateProgressBar;

		public delegate void UpdateResults(List<ValidationResult> results);
		private UpdateResults _updateResults;

		public delegate void UpdateLogBox(string text);
		private UpdateLogBox _updateLogBox;

		public delegate void ClearBeforeRun();
		private ClearBeforeRun _clearBeforeRun;

		public delegate void SetButtonVisibility(Button button, bool visibility, bool enable);
		private SetButtonVisibility _setButton;

		public delegate void IncCounter(int counter, int value);
		private IncCounter _incCounter;

		public delegate void SetLabelText(Label lbl, string txt);
		private SetLabelText _setLabelText;

		/*============================================================================================*/
		#endregion

		private List<string> logRows = new List<string>();
		private int _runnigServices = 0;
		private int _numOfValidationsToRun = 0;
		private ResultForm _resultsForm;
		public DataChecksModelView DataChecksModelView { set; get; }
		public Dictionary<string, Object> EventsHandlers { set; get; }
		private List<AccountServiceElement> _profilesServiceElement { set; get; }

		public DataChecksForm()
		{
			#region Init Deligate functions
			/**************************************************************************/

			_updateProgressBar = new UpdateProgressBar(updateProgressBarState);
			_updateResults = new UpdateResults(updateResults);
			_updateLogBox = new UpdateLogBox(writeLog);
			_clearBeforeRun = new ClearBeforeRun(clearOnStart);
			_setButton = new SetButtonVisibility(setButtonVisibility);
			_incCounter = new IncCounter(IncreaseCounter);
			_setLabelText = new SetLabelText(SetLabel);

			/**************************************************************************/
			#endregion
			
		

			DataChecksModelView = new DataChecksModelView();
			EventsHandlers = new Dictionary<string, Object>();

			//Adding Service Events Handler functions
			#region Attaching Services Events
			EventsHandlers.Add(Const.EventsTypes.ParentOutcomeReportedEvent, new EventHandler(instance_OutcomeReported));
			//EventsHandlers.Add(Const.EventsTypes.ChildOutcomeReportedEvent, new EventHandler(instance_OutcomeReported));
			EventsHandlers.Add(Const.EventsTypes.ParentStateChangedEvent, new EventHandler<Edge.Core.Services.ServiceStateChangedEventArgs>(instance_StateChanged));
			EventsHandlers.Add(Const.EventsTypes.ChildStateChangedEvent, new EventHandler<ServiceStateChangedEventArgs>(child_instance_StateChanged));
			EventsHandlers.Add(Const.EventsTypes.ChildServiceRequested, new EventHandler<ServiceRequestedEventArgs>(instance_ChildServiceRequested));
			#endregion

			InitializeComponent();

			//Load Validation Types from configuration
			DataChecksModelView.LoadValidationTypesItems(this.ValidationTypes.Nodes);

			//Load Metrics Validations from configuration
			DataChecksModelView.LoadMetricsValidationsItems(this.MerticsValidations.Nodes);

			this.LogBox.Multiline = true;

		}

		//On Load
		private void DataChecks_Load(object sender, EventArgs e)
		{
			//Setting Dates to previous day
			fromDate.Value = DateTime.Today.AddDays(-1);
			toDate.Value = DateTime.Today.AddDays(-1);
		}

		private List<AccountServiceElement> GetProfilesFromConfiguration(string pathKey, ComboBox profilesCombo)
		{
			List<AccountServiceElement> serviceElement = new List<AccountServiceElement>();

			try
			{
				AccountElement account;
				if (DataChecksModelView.TryGetAccountFromExtrernalConfig(ConfigurationManager.AppSettings.Get(pathKey), -1, out account))
				{
					foreach (AccountServiceElement service in account.Services)
					{
						if (service.Options.ContainsKey("ProfileName"))
						{
							serviceElement.Add(service);
							int n = profilesCombo.Items.Add(service.Options["ProfileName"]);
						}
					}
				}
			}
			catch
			{
				//Show error msg
			}

			return serviceElement;
		}
		private void CheckAllAccounts(bool isChecked)
		{
			if (isChecked)
				for (int index = 0; index < AccountsCheckedListBox.Items.Count; index++)
				{
					AccountsCheckedListBox.SetItemChecked(index, true);
				}
			else
				for (int index = 0; index < AccountsCheckedListBox.Items.Count; index++)
				{
					AccountsCheckedListBox.SetItemChecked(index, false);
				}
		}

		#region UI Events Code Handler
		/*================================================================================*/

		// Selected Application Event //
		private void application_cb_SelectedValueChanged(object sender, EventArgs e)
		{

			string pathKey = string.Empty;

			if (((ComboBox)sender).SelectedItem.Equals(Const.AdMetricsConst.EdgeApp))
			{
				pathKey = Const.AdMetricsConst.EdgeProductionPathKey;
			}
			else
				pathKey = Const.AdMetricsConst.SeperiaProductionPathKey;

			AccountsCheckedListBox.Items.Clear();

			this.profile_cb.Items.Clear();
			this.profile_cb.Text = "Select Profile";

			//Loading production configuration
			DataChecksModelView.LoadProductionConfiguration(pathKey);

			//Loading Accounts from Data Base
			DataChecksModelView.LoadAccountsFromDB(application_cb.SelectedItem.ToString(), AccountsCheckedListBox, DataChecksModelView.AvailableAccountList);

			//Getting Profiles from configuration 
			if (!DataChecksModelView.TryGetProfilesFromConfiguration(pathKey, profile_cb, DataChecksModelView.Profiles))
				DataChecksModelView.TryGetProfilesFromConfiguration(Const.AdMetricsConst.SeperiaProductionPathKey, profile_cb, DataChecksModelView.Profiles);

			rightSidePanel.Enabled = true;

			//Getting and setting profiles from selected configuration 
			_profilesServiceElement = GetProfilesFromConfiguration(pathKey, this.profile_cb);

			clearOnStart();
		}

		private void profile_cb_SelectedValueChanged(object sender, EventArgs e)
		{
			CheckAllAccounts(false);
			ClearMetricsValidations();
			ClearCheckTypeCheckBox();
			SetAccountsListByProfile(this.profile_cb.SelectedText);
		}

		private void ClearMetricsValidations()
		{
			foreach (TreeNode item in this.MerticsValidations.Nodes)
			{
				//UNCHECK NODES
				item.Checked = false;
			}
		}

		private void ClearCheckTypeCheckBox()
		{
			foreach (TreeNode item in this.ValidationTypes.Nodes)
			{
				//UNCHECK NODES
				item.Checked = false;
			}
		}

		private void SetAccountsListByProfile(string selectedProfile)
		{
			//TO DO : GET ACCOUNTS FROM CONFIGURATION
		}

		private void Start_btn_Click(object sender, EventArgs e)
		{
			Invoke(_clearBeforeRun);

			//FOREACH SELECTED METRIC VALIDATION RUN SERVICE.
			foreach (var MetricsValidation in this.DataChecksModelView.SelectedMetricsValidations)
			{
				//Inc num of services to run 
				this._numOfValidationsToRun++;

				//Run service using current configuration
				if (MetricsValidation.Value.RunHasLocal)
				{
					((DataChecksBase)MetricsValidation.Value).
						RunUsingLocalConfig(DataChecksModelView.GetSelectedValidationTypes(), this.AccountsCheckedListBox.CheckedItems, EventsHandlers);
					this._runnigServices++;
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
				DataChecksBase validationInstance = (DataChecksBase)Assembly.GetExecutingAssembly().CreateInstance(typeName);


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

		private void report_btn_Click(object sender, EventArgs e)
		{
			//TO DO : FILL REPORT GRIDS BEFORE SHOWING FORM
			this._resultsForm.Show();
			this.report_btn.Enabled = false;
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

		#region Services Events Handler Section
		/*=========================================================================*/
		void instance_OutcomeReported(object sender, EventArgs e)
		{
			Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;

			
		}

		void instance_StateChanged(object sender, Edge.Core.Services.ServiceStateChangedEventArgs e)
		{

			Invoke(_updateProgressBar, new object[] { this.progressBar, 0, true });
			Invoke(_updateProgressBar, new object[] { this.progressBar, 60, true });

			Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;

			string log = string.Format("State Changed : {0} - Account ID: {1} - Service: {2} ", e.StateAfter, instance.AccountID, instance.Configuration.Name);
			Invoke(_updateLogBox, new object[] { log });
			Invoke(_setLabelText, new object[] {ProgressBarTxt,string.Format("{0}-{1}", instance.Configuration.Name, e.StateAfter) });

			if (e.StateAfter == Edge.Core.Services.ServiceState.Ready)
			{
				instance.Start();
			}
			if (e.StateAfter == Edge.Core.Services.ServiceState.Ended)
			{
				Invoke(_updateProgressBar, new object[] { this.progressBar, 100, true });
			}

		}

		void instance_ChildServiceRequested(object sender, ServiceRequestedEventArgs e)
		{
			Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;

			//e.RequestedService.OutcomeReported += new EventHandler(instance_OutcomeReported);
			e.RequestedService.StateChanged += new EventHandler<ServiceStateChangedEventArgs>(child_instance_StateChanged);

			e.RequestedService.Initialize();

		}

		void child_instance_StateChanged(object sender, Edge.Core.Services.ServiceStateChangedEventArgs e)
		{
			Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;

			Invoke(_updateProgressBar, new object[] { this.progressBar, 0, true });
			Invoke(_updateProgressBar, new object[] {this.progressBar,60, true});
			Invoke(_setLabelText, new object[] { ProgressBarTxt, string.Format("{0}-{1}", instance.Configuration.Name, e.StateAfter) });
			
			string log = string.Format("State Changed : {0} - Account ID: {1} - Service: {2} ", e.StateAfter, instance.AccountID, instance.Configuration.Name);
			Invoke(_updateLogBox, new object[] { log });

			if (e.StateAfter == ServiceState.Ready)
			{
				instance.Start();
			}

			if (e.StateAfter == ServiceState.Ended)
			{
				//Update current running services counter
				//Invoke(_incCounter, new object[] { this._runnigServices, -1 });

				
				if (instance.Configuration.Options.ContainsKey("OnEnd") && (instance.Configuration.Options["OnEnd"].ToString().Equals("GetValidationResults")))
				{
					//Get Validations Results
					List<ValidationResult> newResults = DataChecksModelView.GetValidationResultsByInstance(instance);

					//TO DO : Add results to validation results view.
					if (newResults.Capacity > 0)
					{
						Invoke(_updateResults, new object[] { newResults });
						
						//Invoke(_setButton, new object[]  { this.report_btn, true, true });
					}
					else
					{
						//Writing To PMS validation  log 
						Invoke(_updateLogBox, new object[] { "Validation results werent found in DB !!" });
					}

				}
			}


		}

		

		/*=========================================================================*/
		#endregion

		#region Deligate uses Functions
		/*====================================================================================*/
		public void updateProgressBarState(ProgressBar progressBarItem, int progress, bool visible = true)
		{
			progressBarItem.Value = progress;
			progressBarItem.Visible = visible;

			Application.DoEvents();
		}

		public void updateResults(List<ValidationResult> results)
		{
			//Application.DoEvents();

			this._runnigServices--;
			#region Setting results dataGrid
			/*****************************************************************************************/
			foreach (ValidationResult item in results)
			{
				if (item.ResultType == ValidationResultType.Error)
				{
					int rowId = _resultsForm.ErrorDataGridView.Rows.Add(item.AccountID, item.CheckType, item.Message, item.ChannelID, item.TargetPeriodStart.Date.ToString());
					_resultsForm.ErrorDataGridView.Rows[rowId].Tag = item;
				}
				else if (item.ResultType == ValidationResultType.Warning)
					_resultsForm.WarningDataGridView.Rows.Add(item.AccountID, item.CheckType, item.Message, item.ChannelID, item.TargetPeriodStart.ToString());
				else
				{
					int rowId = _resultsForm.SuccessDataGridView.Rows.Add(item.AccountID, item.CheckType, item.Message, item.ChannelID, item.TargetPeriodStart.ToString());
					_resultsForm.SuccessDataGridView.Rows[rowId].Tag = item;
				}
			}
			/*****************************************************************************************/
			#endregion

			//Updating status Bar
			//this.progressBar.Value += 100 / this._numOfValidationsToRun;

			//Finshed checking all requested validations
			if (this._runnigServices == 0)
			{
				this.progressBar.Value = 100;
				report_btn.Enabled = true;
				
				#region Updating result image
				/***************************************************************************/
				if (_resultsForm.ErrorDataGridView.Rows.Count > 0)
				{
					ResultImage.Image = Edge.Applications.PM.Suite.DataChecks.Properties.Resources.failed_icon;

				}
				else if (_resultsForm.WarningDataGridView.Rows.Count > 0)
				{
					ResultImage.Image = Edge.Applications.PM.Suite.DataChecks.Properties.Resources.Warning_icon;
					return;
				}
				else ResultImage.Image = Edge.Applications.PM.Suite.DataChecks.Properties.Resources.success_icon;

				//set results labels
				this.errorsCount.Text = _resultsForm.ErrorDataGridView.Rows.Count.ToString() + " Errors";
				this.errorsCount.Visible = true;
				_resultsForm.errCountResult_lbl.Text = string.Format("Totals({0})", _resultsForm.ErrorDataGridView.Rows.Count);

				this.successCount.Text = _resultsForm.SuccessDataGridView.Rows.Count.ToString() + " Success";
				this.successCount.Visible = true;
				_resultsForm.sucessCountResult_lbl.Text = string.Format("Totals({0})", _resultsForm.SuccessDataGridView.Rows.Count);

				this.warningCount.Text = _resultsForm.WarningDataGridView.Rows.Count.ToString() + " Warnings";
				this.warningCount.Visible = true;
				_resultsForm.warningsCountResult_lbl.Text = string.Format("Totals({0})", _resultsForm.WarningDataGridView.Rows.Count);

				/***************************************************************************/
				#endregion
				
				Application.DoEvents();

			}

		}

		/// <summary>
		/// Deligate function to update log
		/// </summary>
		/// <param name="text">set text to be NULL in order to clear log</param>
		private void writeLog(string text)
		{
			//clear log
			if (string.IsNullOrEmpty(text))
			{
				this.logRows.Clear();
				this.LogBox.Clear();
				return;
			}

			logRows.Add(text);
			this.LogBox.Lines = logRows.ToArray();

			//this.LogBox.lAppendText(text);
			
			Application.DoEvents();
		}

		private void clearOnStart()
		{
			//setting results form
			this._resultsForm = new ResultForm();
			_resultsForm.MdiParent = this.ParentForm;

			this.ResultImage.Image = null;
			this.report_btn.Enabled = false;
			this.logRows.Clear();
			this.LogBox.Clear();
			this.progressBar.Value = 0;
			this._numOfValidationsToRun = 0;
			this._runnigServices = 0;

			//Resets results lables
			this.warningCount.Visible = false;
			this.successCount.Visible = false;
			this.errorsCount.Visible = false;

			Application.DoEvents();
		}

		private void setButtonVisibility(Button button, bool visibility, bool enable)
		{
			button.Visible = visibility;
			button.Enabled = enable;
			Application.DoEvents();
		}

		private void IncreaseCounter(int counter, int value)
		{
			counter += value;
			Application.DoEvents();
		}

		private void SetLabel(Label lbl, string txt)
		{
			lbl.Text = txt;
		}

		/*====================================================================================*/
		#endregion
	}

}
