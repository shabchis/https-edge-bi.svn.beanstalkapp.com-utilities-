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

namespace Edge.Application.ProductionManagmentTools
{
	public partial class DataChecks : Form
	{
		#region Deligate members
		/*============================*/
		public delegate void UpdateStepStatus(List<Label> lbls, string text, bool visible);
		private UpdateStepStatus updateStep;
		public delegate void UpdateProgressBar(ProgressBar progressBar, int val, bool visible);
		private UpdateProgressBar updateProgressBar;
		public delegate void UpdateResults(List<ValidationResult> results);
		private UpdateResults updateResults;
		public delegate void UpdateButton(List<Button> buttons, bool Enable, bool visible = true);
		private UpdateButton updateBtn;
		public delegate void UpdatePictureBox(PictureBox pictureBox, Image image, bool visible = true);
		private UpdatePictureBox updateResultImage;
		public delegate void UpdatePanel(List<Panel> panels, bool enable);
		private UpdatePanel updateStepsPanel;
		/*============================*/
		#endregion
		private Dictionary<string, Step> Services;
		private List<ValidationResult> results;
		private ResultForm resultsForm;
		private Dictionary<string, string> _availableAccountList;
		private List<CheckBox> _checkedChannels;
		private List<CheckBox> _checkedServices;
		List<AccountServiceElement> profiles = new List<AccountServiceElement>();
		private string _currentProductionPath = string.Empty;


		public DataChecks()
		{
			InitializeComponent();
			updateStep = new UpdateStepStatus(setLabel);
			updateProgressBar = new UpdateProgressBar(updateProgressBarState);
			updateResults = new UpdateResults(updateResultsDataGrid);
			updateBtn = new UpdateButton(updateButton);
			updateResultImage = new UpdatePictureBox(updateImage);
			updateStepsPanel = new UpdatePanel(updatePanels);

			Services = new Dictionary<string, Step>();
			
			//Set all panels to be enabled = false
			updatePanels(new List<Panel>() { step1, step2, step3, step4 ,step5 }, false);

			report_btn.Enabled = false;
			_availableAccountList = new Dictionary<string, string>();
			_checkedChannels = new List<CheckBox>();
			_checkedServices = new List<CheckBox>();

			GoogleAdwords.BindingContext = new BindingContext() { };
		}

		private bool TryGetServiceUseByCahnnel(string channelId, out string serviceUse)
		{
			switch (channelId)
			{
				case "1":
					serviceUse = Const.AdwordsServiceName;
					return true;
				case "6":
					serviceUse = Const.FacebookServiceName;
					return true;
			}

			//for not supported channels
			serviceUse = string.Empty;
			return false;
		}

		private void DataChecks_Load(object sender, EventArgs e)
		{
			fromDate.Value = DateTime.Today.AddDays(-1);
			toDate.Value = DateTime.Today.AddDays(-1);
		}

		private void GetAccountsFromDB(string SystemDatabase, CheckedListBox accountsListBox, Dictionary<string, string> availableAccountList)
		{
			using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(this, SystemDatabase)))
			{
				sqlCon.Open();
				SqlCommand sqlCommand = DataManager.CreateCommand(
					"SELECT [Account_Name] ,[Account_ID] FROM [dbo].[User_GUI_Account] group by [Account_Name] ,[Account_ID]");
				sqlCommand.Connection = sqlCon;

				using (var _reader = sqlCommand.ExecuteReader())
				{
					if (!_reader.IsClosed)
					{
						while (_reader.Read())
						{
							accountsListBox.Items.Add(string.Format("{0}-{1}", _reader[1], _reader[0]));
							availableAccountList.Add(_reader[1].ToString(), _reader[0].ToString());
						}
					}
				}
			}

			if (AccountsCheckedListBox.Items.Count > 0)
			{
				AccountsCheckedListBox.Sorted = true;
			}
		}

		private bool TryGetAccountFromExtrernalConfig(string fullPath, int accountId, out AccountElement accountElement)
		{
			try
			{
				EdgeServicesConfiguration.Load(fullPath);
				AccountElementCollection accounts = EdgeServicesConfiguration.Current.Accounts;
				accountElement = accounts.GetAccount(accountId);
				return true;
			}
			catch
			{
				accountElement = null;
				return false;
			}
		}

		private bool TryGetProfilesFromConfiguration(string key, ComboBox profilesCombo, List<AccountServiceElement> serviceElement)
		{
			//saving current configuration
			string currentConfigurationFullPath = EdgeServicesConfiguration.Current.CurrentConfiguration.FilePath;
			try
			{
				//Getting configuration path from configuration.
				_currentProductionPath = ConfigurationManager.AppSettings.Get(key);

				AccountElement account;
				TryGetAccountFromExtrernalConfig(_currentProductionPath, -1, out account);

				foreach (AccountServiceElement service in account.Services)
				{
					if (service.Options.ContainsKey("ProfileName"))
					{
						serviceElement.Add(service);
						profilesCombo.Items.Add(service.Options["ProfileName"]);
					}
				}

				profilesCombo.Tag = profiles;


			}
			catch
			{
				//Loading original configuration
				EdgeServicesConfiguration.Load(currentConfigurationFullPath);
				//Directory.SetCurrentDirectory((Path.GetDirectoryName(currentConfigurationFullPath)));
				return false;
			}

			//Loading original configuration
			EdgeServicesConfiguration.Load(currentConfigurationFullPath);
			//Directory.SetCurrentDirectory((Path.GetDirectoryName(currentConfigurationFullPath)));
			return true;


		}

		#region Delegate functions
		/*=========================*/
		public void updatePanels(List<Panel> panels, bool enable)
		{
			foreach (Panel panel in panels)
			{
				panel.Enabled = enable;
				panel.Visible = true;
			}
		}

		public void updateImage(PictureBox pictureBox, Image image, bool visible = true)
		{

			pictureBox.Image = image;
			pictureBox.Visible = visible;

		}

		public void updateButton(List<Button> Buttons, bool Enable, bool Visible = true)
		{
			foreach (Button btn in Buttons)
			{
				btn.Enabled = Enable;
				btn.Visible = Visible;
			}
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

				//resultsForm.errCountResult_lbl.Text = string.Format("Totals ( {0} )", resultsForm.ErrorDataGridView.RowCount);
				//resultsForm.warningsCountResult_lbl.Text = string.Format("Totals ( {0} )", resultsForm.WarningDataGridView.RowCount);
				//resultsForm.sucessCountResult_lbl.Text = string.Format("Totals ( {0} )", resultsForm.SuccessDataGridView.RowCount);

			}

			report_btn.Enabled = true;

		}

		public void updateProgressBarState(ProgressBar progressBar, int val, bool visible)
		{
			progressBar.Value = val;
			progressBar.Visible = visible;
		}

		public void setLabel(List<Label> lbls, string text, bool visible)
		{
			foreach (Label lbl in lbls)
			{
				if (!String.IsNullOrEmpty(text)) lbl.Text = text;
				lbl.Visible = visible;
			}
		}

		/*=========================*/
		#endregion

		#region On Step Change ( check box )
		private void Step1StateChange(object sender, EventArgs e)
		{
			if (level1.Checked)
			{
				Invoke(updateStepsPanel, new object[] { new List<Panel>() { step1 }, true });
				Services.Add(Const.DeliveryOltpService, new Step
				{
					Panel = step1,
					ProgressBar = step1_progressBar,
					ResultImage = step1_ErrorImage,
					Status = step1_status,
					StepName = step1_lbl,
					WarningCount = step1_warningCount,
					ErrorsCount = step1_errorsCount
				});
			}
			else
			{
				Invoke(updateStepsPanel, new object[] { new List<Panel>() { step1 }, false });
				Services.Remove(Const.DeliveryOltpService);
			}
		}

		private void Step2StateChange(object sender, EventArgs e)
		{
			if (level2.Checked)
			{
				Invoke(updateStepsPanel, new object[] { new List<Panel>() { step2 }, true });
				Services.Add(Const.OltpDwhService, new Step
				{
					Panel = step2,
					ProgressBar = step2_progressBar,
					ResultImage = step2_Result,
					Status = step2_status,
					StepName = step2_lbl,
					WarningCount = step2_warningCount,
					ErrorsCount = step2_errorsCount
				});
			}
			else
			{
				Invoke(updateStepsPanel, new object[] { new List<Panel>() { step2 }, false });
				Services.Remove(Const.OltpDwhService);
			}
		}

		private void Step3StateChange(object sender, EventArgs e)
		{
			if (level3.Checked)
			{
				Invoke(updateStepsPanel, new object[] { new List<Panel>() { step3 }, true });
				Services.Add(Const.MdxDwhService, new Step
				{
					Panel = step3,
					ProgressBar = step3_progressBar,
					ResultImage = step3_Result,
					Status = step3_status,
					StepName = step3_lbl,
					WarningCount = step3_warningCount,
					ErrorsCount = step3_errorsCount
				});
			}
			else
			{
				Invoke(updateStepsPanel, new object[] { new List<Panel>() { step3 }, false });
				Services.Remove(Const.MdxDwhService);
			}
		}

		private void Step4StateChange(object sender, EventArgs e)
		{
			if (level4.Checked)
			{
				Invoke(updateStepsPanel, new object[] { new List<Panel>() { step4 }, true });
				Services.Add(Const.MdxOltpService, new Step
				{
					Panel = step4,
					ProgressBar = step4_progressBar,
					ResultImage = step4_Result,
					Status = step4_status,
					StepName = step4_lbl,
					WarningCount = step4_warningCount,
					ErrorsCount = step4_errorsCount
				});
			}
			else
			{
				Invoke(updateStepsPanel, new object[] { new List<Panel>() { step4 }, false });
				Services.Remove(Const.MdxOltpService);
			}
		}

		private void level5_CheckStateChanged(object sender, EventArgs e)
		{
			if (level5.Checked)
			{

				this.ClearCheckTypeCheckBox();
				level1.Enabled = false;
				level2.Enabled = false;
				level3.Enabled = false;
				level4.Enabled = false;

				Invoke(updateStepsPanel, new object[] { new List<Panel>() { step5 }, true });
				Services.Add(Const.AdwordsServiceName, new Step
				{
					Panel = step5,
					ProgressBar = step5_progressBar,
					ResultImage = step5_Result,
					Status = step5_status,
					StepName = step5_lbl,
					WarningCount = step5_warningCount,
					ErrorsCount = step5_errorsCount
				});
			}
			else
			{
				level1.Enabled = true;
				level2.Enabled = true;
				level3.Enabled = true;
				level4.Enabled = true;
				Invoke(updateStepsPanel, new object[] { new List<Panel>() { step5 }, false });
				Services.Remove(Const.AdwordsServiceName);
			}
		}
		#endregion

		#region Run button click functions
		/*=========================*/
		private void Start_btn_Click(object sender, EventArgs e)
		{
			DateTimeRange timePeriod;
			string channels, accounts;

			InitUI();

			if (TryGetServicesParams(AccountsCheckedListBox, out timePeriod, out channels, out accounts))
			{
				//Check if no service has been selected from checked boxes
				if (Services.Count == 0)
				{
					DialogResult dlgRes = new DialogResult();
					dlgRes = MessageBox.Show("Please select at least one Check level", "Check level Error",
					MessageBoxButtons.OK,
					 MessageBoxIcon.Error);
					return;
				}

				foreach (var service in Services)
				{
					try
					{
						if (service.Value.StepName.Equals(step5_lbl))
							//Get Service from production configuration and Run
							InitProductionService(timePeriod, channels, accounts);
						else
							// Get Service from Local Configurartion and run 
							InitServices(timePeriod, service.Key, channels, accounts);
					}
					catch (Exception ex)
					{
						Invoke(updateStep, new object[] 
                        { 
                            new List<Label>(){appErrorLbl},ex.Message,false 
                        });
						Invoke(updateStepsPanel, new object[] { new List<Panel> { AppAlertPanel }, true });
					}
				}

			}

		}


		private void quick_btn_Click(object sender, EventArgs e)
		{
			level1.Checked = true;
			level2.Checked = false;
			level3.Checked = false;
			level4.Checked = true;
			Start_btn_Click(sender, e);
		}

		private void full_btn_Click(object sender, EventArgs e)
		{
			level1.Checked = true;
			level2.Checked = true;
			level3.Checked = true;
			level4.Checked = true;
			Start_btn_Click(sender, e);
		}


		/*=========================*/
		#endregion

		#region On Channel Change
		/*========================*/
		private void Channel_CheckedChanged(object sender)
		{
			//TO DO : add to Profile temp
		}
		private void GoogleAdwords_CheckedChanged(object sender, EventArgs e)
		{
			Channel_CheckedChanged(sender);
		}
		private void Facebook_CheckedChanged(object sender, EventArgs e)
		{
			Channel_CheckedChanged(sender);
		}
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			Channel_CheckedChanged(sender);
		}
		/*========================*/
		#endregion

		#region Clear checked box functions
		/*================================*/
		private void ClearCheckTypeCheckBox()
		{
			level1.Checked = false;
			level2.Checked = false;
			level3.Checked = false;
			level4.Checked = false;
		}
		private void ClearChannelsCheckBox()
		{
			GoogleAdwords.Checked = false;
			Facebook.Checked = false;
			Bing.Checked = false;
		}
		/*================================*/
		#endregion

		#region Service
		/*=======================*/
		private bool TryGetServicesParams(CheckedListBox accountsCheckedListBox, out DateTimeRange timePeriod, out string channelsList, out string accountsList)
		{
			channelsList = "";
			accountsList = "";

			#region TimePeriod
			timePeriod = new DateTimeRange()
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
			#endregion

			#region Channel Params
			//Creating Channel Param
			StringBuilder channels = new StringBuilder();
			if (Facebook.Checked == true)
			{
				if (!String.IsNullOrEmpty(channels.ToString()))
					channels.Append(',');
				channels.Append(6); // add facebook channel id code
			}
			if (GoogleAdwords.Checked == true)
			{
				if (!String.IsNullOrEmpty(channels.ToString()))
					channels.Append(',');
				channels.Append(1); // add facebook channel id code
			}

			if (Bing.Checked == true)
			{
				if (!String.IsNullOrEmpty(channels.ToString()))
					channels.Append(',');
				channels.Append(14); // add Bing channel id code
			}

			if (string.IsNullOrEmpty(channels.ToString()))
			{
				DialogResult dlgRes = new DialogResult();
				dlgRes = MessageBox.Show("Please select at least one Data type", "Data type Error",
				MessageBoxButtons.OK,
				 MessageBoxIcon.Error);
				return false;
			}

			channelsList = channels.ToString();
			#endregion

			#region Account Params
			StringBuilder accounts = new StringBuilder();
			if (accountsCheckedListBox.CheckedItems.Count > 0)
			{
				foreach (string item in accountsCheckedListBox.CheckedItems)
				{
					string[] items = item.Split('-');
					accounts.Append(items[0]);
					if (!String.IsNullOrEmpty(accounts.ToString()))
						accounts.Append(',');
				}
			}
			else
			{
				DialogResult dlgRes = new DialogResult();
				dlgRes = MessageBox.Show("Please select at least one account", "Accounts Error",
				MessageBoxButtons.OK,
				 MessageBoxIcon.Error);
				return false;
			}
			accountsList = accounts.ToString().Remove(accounts.Length - 1);
			#endregion

			return true;
		}

		private void InitProductionService(DateTimeRange timePeriod, string channels, string accounts)
		{
			Edge.Core.Services.ServiceInstance instance;
			ActiveServiceElement serviceElements;

			//Loading Production Configuration -Foreach account and channel
			foreach (string accountId in accounts.Split(','))
			{
				foreach (string channelId in channels.Split(','))
				{
					AccountElement account;
					if (TryGetAccountFromExtrernalConfig(_currentProductionPath, Convert.ToInt32(accountId), out account))
					{
						string serviceUses;
						if (TryGetServiceUseByCahnnel(channelId, out serviceUses))
						{
							int workFlowChangesFlag = 0;
							foreach (AccountServiceElement service in account.Services)
							{
								if (service.Uses.Element.Name == serviceUses)
								{

									//EdgeServicesConfiguration.Current.Accounts.GetAccount(accountID).Services[serviceName])

									serviceElements = new ActiveServiceElement(service);


									#region Setting WorkFlow Configuration
									/*============================================================*/
									foreach (WorkflowStepElement wf in serviceElements.Workflow)
									{
										switch (wf.ActualName)
										{
											case Const.WorkflowServices.CommitServiceName:
												{
													wf.IsEnabled = false;
													workFlowChangesFlag++;
													break;
												}
											case Const.WorkflowServices.OltpDeliveryCheckServiceName:
												{
													wf.IsEnabled = true;
													workFlowChangesFlag++;
													break;
												}
											case Const.WorkflowServices.ResultsHandlerServiceName:
												{
													wf.IsEnabled = false;
													workFlowChangesFlag++;
													break;
												}
										}

									}
									/*============================================================*/
									#endregion

									//If workFlowChangesFlag != 3 it means that some of changes havnt been done in workflows, probably due to missing workflow in configuration
									if (workFlowChangesFlag >= 3)
									{
										instance = Edge.Core.Services.Service.CreateInstance(serviceElements, Convert.ToInt32(accountId));
										instance.Configuration.Options.Add("ConflictBehavior", "Ignore");
										instance.OutcomeReported += new EventHandler(instance_OutcomeReported);
										instance.StateChanged += new EventHandler<Edge.Core.Services.ServiceStateChangedEventArgs>(instance_StateChanged);
										instance.ProgressReported += new EventHandler(instance_ProgressReported);
										instance.ChildServiceRequested += new EventHandler<ServiceRequestedEventArgs>(instance_ChildServiceRequested);

										instance.Initialize();
									}

									//Takes the first service that uses current service name
									break;
								}
							}//End
						}
					}

				} // End of Channel foreach

			}//End of acounts foreach
		}

		void instance_ChildServiceRequested(object sender, ServiceRequestedEventArgs e)
		{
			e.RequestedService.OutcomeReported += new EventHandler(instance_OutcomeReported);
			e.RequestedService.StateChanged += new EventHandler<ServiceStateChangedEventArgs>(instance_StateChanged);

			e.RequestedService.Initialize();

		}

		private void InitServices(DateTimeRange timePeriod, string service, string channels, string accounts)
		{
			ActiveServiceElement serviceElements = new ActiveServiceElement(EdgeServicesConfiguration.Current.Accounts.GetAccount(-1).Services[service]);

			// TimePeriod
			serviceElements.Options.Add("fromDate", timePeriod.Start.ToDateTime().ToString());
			serviceElements.Options.Add("toDate", timePeriod.End.ToDateTime().ToString());

			serviceElements.Options.Add("ChannelList", channels);
			serviceElements.Options.Add("AccountsList", accounts);

			string SourceTable;
			if (service.Equals(Const.DeliveryOltpService))
			{
				if (!serviceElements.Options.Keys.Contains("SourceTable"))
					serviceElements.Options.Add("SourceTable", Const.OltpTable);
			}
			else if (service.Equals(Const.OltpDwhService))
			{
				if (!serviceElements.Options.Keys.Contains("SourceTable"))
					serviceElements.Options.Add("SourceTable", Const.OltpTable);
				if (!serviceElements.Options.Keys.Contains("TargetTable"))
					serviceElements.Options.Add("TargetTable", Const.DwhTable);
			}
			else if (service.Equals(Const.MdxOltpService))
			{
				if (!serviceElements.Options.Keys.Contains("SourceTable"))
					serviceElements.Options.Add("SourceTable", Const.OltpTable);
			}
			else if (service.Equals(Const.MdxDwhService))
				serviceElements.Options.Add("SourceTable", Const.DwhTable);
			else
				//TO DO : Get tabels from configuration.
				throw new Exception("ComparisonTable hasnt been implemented for this service");



			Edge.Core.Services.ServiceInstance instance = Edge.Core.Services.Service.CreateInstance(serviceElements);

			instance.OutcomeReported += new EventHandler(instance_OutcomeReported);
			instance.StateChanged += new EventHandler<Edge.Core.Services.ServiceStateChangedEventArgs>(instance_StateChanged);
			instance.ProgressReported += new EventHandler(instance_ProgressReported);

			instance.Initialize();



		}

		private void InitUI()
		{
			//setting report_btn to be disabled
			Invoke(updateBtn, new object[] { new List<Button>() { report_btn }, false, true });

			//creating new results report
			resultsForm = new ResultForm();
			resultsForm.MdiParent = this.ParentForm;

			//Setting Steps 
			if (level1.Checked)
			{
				Invoke(updateProgressBar, new object[] { step1_progressBar, 0, true });
				Invoke(updateResultImage, new object[] { step1_ErrorImage, global::Edge.Application.ProductionManagmentTools.Properties.Resources.success_icon, false });
				Invoke(updateStep, new object[] 
                { 
                    new List<Label>(){step1_errorsCount,step1_warningCount},"",false 
                });
			}

			if (level2.Checked)
			{
				Invoke(updateProgressBar, new object[] { step2_progressBar, 0, true });
				Invoke(updateResultImage, new object[] { step2_Result, global::Edge.Application.ProductionManagmentTools.Properties.Resources.success_icon, false });
				Invoke(updateStep, new object[] 
                { 
                    new List<Label>(){step2_errorsCount,step1_warningCount},"",false 
                });
			}

			if (level3.Checked)
			{
				Invoke(updateProgressBar, new object[] { step3_progressBar, 0, true });
				Invoke(updateResultImage, new object[] { step3_Result, global::Edge.Application.ProductionManagmentTools.Properties.Resources.success_icon, false });
				Invoke(updateStep, new object[] 
                { 
                    new List<Label>(){step3_errorsCount,step1_warningCount},"",false 
                });
			}

			if (level4.Checked)
			{
				Invoke(updateProgressBar, new object[] { step4_progressBar, 0, true });
				Invoke(updateResultImage, new object[] { step4_Result, global::Edge.Application.ProductionManagmentTools.Properties.Resources.success_icon, false });
				Invoke(updateStep, new object[] 
                { 
                    new List<Label>(){step4_errorsCount,step1_warningCount},"",false 
                });
			}
		}

		/*=======================*/
		#endregion

		private bool HasParent(ServiceInstance instance, out int NumOfChildrens)
		{
			if (instance.ParentInstance != null) // if child
			{
				NumOfChildrens = 0;
				foreach (WorkflowStepElement element in instance.ParentInstance.Configuration.Workflow)
				{
					if (element.IsEnabled == true)
						NumOfChildrens++;
				}
				return true;
			}
			else
			{
				NumOfChildrens = 1;
				return false;
			}
		}
		public string GetStepNameByInstance(ServiceInstance instance)
		{
			if (instance.Configuration.Workflow.Count > 0)
			{
				return instance.Configuration.Name;
			}
			else if (instance.ParentInstance != null)
				return instance.ParentInstance.Configuration.Name;

			else //stand alone service
				return instance.Configuration.Name;

		}

		#region Instances Handler functions
		/*=================================*/
		void instance_OutcomeReported(object sender, EventArgs e)
		{
			Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;
			Step step;
			Services.TryGetValue(GetStepNameByInstance(instance), out step);

			int numOfChildrens;

			if (!HasParent(instance, out numOfChildrens))
				Invoke(updateProgressBar, new object[] { step.ProgressBar, 100, true });
			else 
			Invoke(updateProgressBar, new object[] { step.ProgressBar, 100 / numOfChildrens, true });

			if (resultsForm.ErrorDataGridView.RowCount > 0)
			#region
			{
				Invoke(updateResultImage, new object[] { step.ResultImage, global::Edge.Application.ProductionManagmentTools.Properties.Resources.failed_icon, true });
				Invoke(updateStep, new object[] 
                { 
                    new List<Label>(){step.ErrorsCount},String.Format("{0}{1}",CountRowsByLevelType(resultsForm.ErrorDataGridView.Rows, instance.Configuration.Name)," errors"),true 
                });
			}
			#endregion
			
			else if (resultsForm.WarningDataGridView.RowCount > 0)
			#region 
			{
				Invoke(updateResultImage, new object[] { step.ResultImage, global::Edge.Application.ProductionManagmentTools.Properties.Resources.Warning_icon, true });
				Invoke(updateStep, new object[] 
                { 
                    new List<Label>(){step.WarningCount},String.Format("{0}{1}",   
                    CountRowsByLevelType(resultsForm.WarningDataGridView.Rows, instance.Configuration.Name)," warnings"),true 
                });
			}
			#endregion
			
			else
				
				if (!HasParent(instance, out numOfChildrens))
				Invoke(updateResultImage, new object[] { step.ResultImage, global::Edge.Application.ProductionManagmentTools.Properties.Resources.success_icon, true });

			//setting report_btn to be Enable
			//Invoke(updateBtn, new object[] { new List<Button>() { report_btn }, true, true });


		}
		void instance_StateChanged(object sender, Edge.Core.Services.ServiceStateChangedEventArgs e)
		{
			Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;
			Step step;
			int num;

			if (Services.TryGetValue(GetStepNameByInstance(instance), out step))
			{
				Invoke(updateStep, new object[]
                    {
                        new List<Label>(){step.Status},e.StateAfter.ToString().Equals("Ended")?"Running":e.StateAfter.ToString(),true
                    }
				);
			}

			if (e.StateAfter == Edge.Core.Services.ServiceState.Ready)
			{
				instance.Start();
			}

			if (e.StateAfter == Edge.Core.Services.ServiceState.Ended)
			{
				results = new List<ValidationResult>();
				HasParent(instance, out num);

				Invoke(updateProgressBar, new object[] { step.ProgressBar, 70, true });

			
					#region Getting Instance Log for results
					using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
					{
						sqlCon.Open();
						SqlCommand sqlCommand = DataManager.CreateCommand(
							"SELECT [Message] FROM [dbo].[Log] where [ServiceInstanceID] = @instanceID");
						sqlCommand.Parameters.Add(new SqlParameter() { ParameterName = "@instanceID", Value = instance.InstanceID, SqlDbType = System.Data.SqlDbType.BigInt });
						sqlCommand.Connection = sqlCon;

						using (var _reader = sqlCommand.ExecuteReader())
						{
							if (!_reader.IsClosed)
							{
								while (_reader.Read())
								{
									results.Add((ValidationResult)JsonConvert.DeserializeObject(_reader[0].ToString(), typeof(ValidationResult)));
								}
							}
						}
					}
					#endregion

					if (results.Capacity > 0)
					{
						Invoke(updateResults, new object[] { results });
						//if (!HasParent(instance, out num))
							Invoke(updateStep, new object[] {new List<Label>(){step.Status},"Ended",true});
					}

				//else
				//{
				//    //  alert : no services were found
				//    Invoke(updateStep, new object[]
				//    {
				//         new List<Label>(){appErrorLbl},String.Format("Error, cannot find results from service instance id {0}",instance.InstanceID),true
				//    });
				//}


			}

		}
		private void instance_ProgressReported(object sender, EventArgs e)
		{
			Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;
			Step step;
			int num;
			HasParent(instance, out num);
			Services.TryGetValue(GetStepNameByInstance(instance), out step);
			Invoke(updateProgressBar, new object[] { step.ProgressBar, (int)((ServiceInstance)sender).Progress * 70, true });
		}
		/*=================================*/
		#endregion

		private int CountRowsByLevelType(DataGridViewRowCollection Rows, string type)
		{
			int count = 0;
			foreach (DataGridViewRow row in Rows)
			{
				if ((row.Cells[1].Value != null) && (row.Cells[1].Value.ToString().Equals(type))) count++;
			}
			return count;
		}
		private void report_btn_Click(object sender, EventArgs e)
		{
			//ResultForm results = new ResultForm(ref resultsForm.ErrorDataGridView.Rows, ref resultsForm.WarningDataGridView.Rows, ref resultsForm.SuccessDataGridView.Rows);
			SortResultsDataGrid(resultsForm.ErrorDataGridView, ListSortDirection.Ascending);
			SortResultsDataGrid(resultsForm.WarningDataGridView, ListSortDirection.Ascending);
			SortResultsDataGrid(resultsForm.SuccessDataGridView, ListSortDirection.Ascending);

			resultsForm.Show();
		}
		private void SortResultsDataGrid(DataGridView dataGridView, ListSortDirection listSortDirection)
		{
			if (dataGridView.RowCount > 0)
			{
				dataGridView.Sort(dataGridView.Columns[0], listSortDirection);
			}
		}
		private void profile_cb_SelectedValueChanged(object sender, EventArgs e)
		{
			CheckAllAccounts(false);
			ClearChannelsCheckBox();
			ClearCheckTypeCheckBox();
			try
			{
				var profileService = from b in profiles
									 where b.Options["ProfileName"].ToString() == profile_cb.SelectedItem.ToString()
									 select b;

				if (profileService.FirstOrDefault<AccountServiceElement>() != null)
				{
					//set accounts
					if (profileService.First<AccountServiceElement>().Options.ContainsKey("AccountsList"))
					{
						string[] accountsId = profileService.First<AccountServiceElement>().Options["AccountsList"].Split(',');
						foreach (string accountId in accountsId)
						{
							int index = AccountsCheckedListBox.Items.IndexOf(string.Format("{0}-{1}", accountId, _availableAccountList[accountId]));
							AccountsCheckedListBox.SetItemChecked(index, true);
							//AccountsCheckedListBox.SelectedItem
						}
					}
					//set channels
					if (profileService.First<AccountServiceElement>().Options.ContainsKey("ChannelList"))
					{
						string[] channelsID = profileService.First<AccountServiceElement>().Options["ChannelList"].Split(',');
						foreach (string id in channelsID)
						{
							ClearChannelsCheckBox();
							SetChannelCheckedBox(id);
						}
					}
					//set validation services
					if (profileService.First<AccountServiceElement>().Options.ContainsKey("CheckTypes"))
					{
						string[] validationServices = profileService.First<AccountServiceElement>().Options["CheckTypes"].Split(',');
						foreach (string type in validationServices)
						{
							SetCheckType(type);
						}
					}


				}
			}
			catch (Exception ex)
			{
				Invoke(updateStep, new object[] { new List<Label>() { appErrorLbl }, ex.Message, true });
				Invoke(updateStepsPanel, new object[] { new List<Panel> { AppAlertPanel }, true });
			}
		}
		private void checkAll_CheckedChanged(object sender, EventArgs e)
		{
			if (checkAll.Checked) CheckAllAccounts(true);
			else CheckAllAccounts(false);
		}
		private void SetCheckType(string type)
		{
			switch (type)
			{
				case Const.DeliveryOltpService:
					{
						level1.Checked = true;
						break;
					}
				case Const.OltpDwhService:
					{
						level2.Checked = true;
						break;
					}
				case Const.MdxDwhService:
					{
						level3.Checked = true;
						break;
					}
				case Const.MdxOltpService:
					{
						level4.Checked = true;
						break;
					}

			}
		}
		private void SetChannelCheckedBox(string id)
		{
			switch (id)
			{
				case "1":
					{
						GoogleAdwords.Checked = true;
						break;
					}
				case "6":
					{
						Facebook.Checked = true;
						break;
					}
				case "14":
					{
						Bing.Checked = true;
						break;
					}
			}
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

		private void application_cb_SelectedValueChanged(object sender, EventArgs e)
		{
			string key = string.Empty;

			if (((ComboBox)sender).SelectedItem.Equals(Const.EdgeApp))
			{
				key = Const.EdgeProductionPathKey;
			}
			else
				key = Const.SeperiaProductionPathKey;

			AccountsCheckedListBox.Items.Clear();
			_availableAccountList.Clear();
			profile_cb.Items.Clear();
			profile_cb.Text = "Select Profile";

			LoadProductionConfiguration(key);

			GetAccountsFromDB(application_cb.SelectedItem.ToString(), AccountsCheckedListBox, _availableAccountList);

			if (!TryGetProfilesFromConfiguration(key, profile_cb, profiles))
				TryGetProfilesFromConfiguration(Const.SeperiaProductionPathKey, profile_cb, profiles);
		}

		private void LoadProductionConfiguration(string key)
		{
			string productionPath = ConfigurationManager.AppSettings.Get(key);
			EdgeServicesConfiguration.Load(productionPath);
		}

		private void clear_Click(object sender, EventArgs e)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
				{
					conn.Open();
					using (SqlCommand command = DataManager.CreateCommand("ResetUnendedServices", CommandType.StoredProcedure))
					{
						command.Connection = conn;
						int numOfRows = command.ExecuteNonQuery();
						string msg = String.Format("{0} row(s) affected", numOfRows);
						MessageBox.Show(msg);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

	}

	public static class Const
	{
		// Tabels
		public static string OltpTable = "dbo.Paid_API_AllColumns_v29";
		public static string DwhTable = "Dwh_Fact_PPC_Campaigns";

		//Services
		public const string DeliveryOltpService = "DataChecks.OltpDelivery";
		public const string OltpDwhService = "DataChecks.DwhOltp";
		public const string MdxDwhService = "DataChecks.MdxDwh";
		public const string MdxOltpService = "DataChecks.MdxOltp";

		public const string AdwordsServiceName = "Google.AdWords";
		public const string FacebookServiceName = "facebook";

		//WorkflowServices
		public static class WorkflowServices
		{
			public const string CommitServiceName = "AdMetricsCommit";
			public const string OltpDeliveryCheckServiceName = "DataChecks.OltpDelivery";
			public const string ResultsHandlerServiceName = "DataChecks.ResultsHandler";
		}

		//ProductionKeys
		public const string SeperiaProductionPathKey = "SeperiaProductionConfigurationPath";
		public const string EdgeProductionPathKey = "EdgeProductionConfigurationPath";

		//ApplicationTypes
		public const string SeperiaApp = "Seperia";
		public const string EdgeApp = "Edge.BI";

	}

	public class Step
	{
		public Panel Panel { get; set; }
		public Label StepName { get; set; }
		public ProgressBar ProgressBar { get; set; }
		public Label Status { get; set; }
		public PictureBox ResultImage { get; set; }
		public Label WarningCount { get; set; }
		public Label ErrorsCount { get; set; }
	}
	public class Account
	{
		public int Id { set; get; }
		public string AccountName { set; get; }
	}

}
