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
		public DataChecksModelView DataChecksModelView { set; get; }
		public Dictionary<string, EventHandler> EventsHandlerDic { set; get; }
		
		public DataChecksForm()
		{
			DataChecksModelView = new DataChecksModelView();
			EventsHandlerDic = new Dictionary<string, EventHandler>();

			InitializeComponent();

			//Load Validation Types from configuration
			DataChecksModelView.SetValidationTypesItems(this.ValidationTypes.Nodes);

			//Load Metrics Validations from configuration
			DataChecksModelView.SetMetricsValidationsItems(this.MerticsValidations.Nodes);

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
						RunUsingLocalConfig(DataChecksModelView.GetSelectedValidationTypes(), this.AccountsCheckedListBox.SelectedItems, new EventHandler(instance_OutcomeReported));
				}
				//Run service using external configuration
				//else
				//	((DataChecksBase)MetricsValidation.Value).
				//	RunUsingExternalConfig(this.DataChecksModelView.SelectedValidationsTypes, this.AccountsCheckedListBox.SelectedItems);
			}
		}

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



	}


}
