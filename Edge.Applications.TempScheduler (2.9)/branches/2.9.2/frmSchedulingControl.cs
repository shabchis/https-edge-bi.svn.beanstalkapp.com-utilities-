﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Edge.Core.Scheduling;
using Edge.Core.Scheduling.Objects;
using System.IO;
using legacy = Edge.Core.Services;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using Edge.Core.Configuration;
using Edge.Core.Data;
using Edge.Core.Utilities;


namespace Edge.Applications.TempScheduler
{
	public partial class frmSchedulingControl : Form
	{
		StringBuilder _strNotScheduled = new StringBuilder();
		private Scheduler _scheduler;
		private Listener _listner;
		private Dictionary<SchedulingData, ServiceInstance> _scheduledServices = new Dictionary<SchedulingData, ServiceInstance>();
		public delegate void SetLogMethod(string lineText);
		public delegate void UpdateGridMethod(legacy.ServiceInstance serviceInstance);
		SetLogMethod setLogMethod;
		private delegate void EnabelDisableButtons();
		UpdateGridMethod updateGridMethod;
		bool _scheduleStarted = false;
		Thread _timerToStartScheduling;		
		object lastTag;


		public frmSchedulingControl()
		{
			try
			{
				InitializeComponent();

				setLogMethod = new SetLogMethod(SetLogTextBox);
				updateGridMethod = new UpdateGridMethod(UpdateGridData);
				this.FormClosed += new FormClosedEventHandler(frmSchedulingControl_FormClosed);


			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}
		}

		void frmSchedulingControl_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				_listner.Dispose();
				_scheduler.Stop();
				Log.Write("SchedulingControlForm", "Threads stops by form closedevent",LogMessageType.Information);
				_timerToStartScheduling.Abort();
				Application.ExitThread();
				Application.Exit();

			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				try
				{
					lblVer.Text = Application.ProductVersion;
					EndBtn.Enabled = false;
					this.Text = System.AppDomain.CurrentDomain.FriendlyName;
					_timerToStartScheduling = new Thread(new ThreadStart(delegate()
			{
				Thread.Sleep(new TimeSpan(0, 1, 0));
				if (!_scheduleStarted)
				{
					_scheduler.Start();

					startBtn.Invoke(new EnabelDisableButtons(delegate()
					{
						startBtn.Enabled = false;
						EndBtn.Enabled = true;
					}));

				}
			}));
					_timerToStartScheduling.IsBackground = true;
					_timerToStartScheduling.Start();


				}
				catch (Exception)
				{

					throw;
				}
				_scheduler = new Scheduler(true);
				_listner = new Listener(_scheduler);
				_listner.Start();

				_scheduler.ServiceRunRequiredEvent += new EventHandler(_scheduler_ServiceRunRequiredEvent);
				_scheduler.NewScheduleCreatedEvent += new EventHandler(_scheduler_NewScheduleCreatedEventHandler);
				_scheduler.NotRunServices += new EventHandler(_scheduler_NotRunServices);
				//	_scheduler.Start();
				//test

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}
		}

		void _scheduler_NotRunServices(object sender, EventArgs e)
		{
			WillNotRunEventArgs ev = (WillNotRunEventArgs)e;
			foreach (SchedulingData notRunService in ev.WillNotRun)
			{
				//this.Invoke(setLogMethod, new object[] { string.Format("Service: {0} for account: {1} will not run at all!!!!!!!!", notRunService.Configuration.Name, notRunService.profileID) });
			}

		}

		void _scheduler_ServiceRunRequiredEvent(object sender, EventArgs e)
		{
			//todo: log2
			try
			{

				TimeToRunEventArgs args = (TimeToRunEventArgs)e;
				foreach (Edge.Core.Scheduling.Objects.ServiceInstance serviceInstance in args.ServicesToRun)
				{

					this.Invoke(setLogMethod, new Object[] { string.Format("Service: {0} is required for running time {1}\r\n", serviceInstance.ServiceName, DateTime.Now.ToString("dd/MM/yy HH:mm")) });

					//FURTURE: ask system control if it's ok to run the service
					////if ok then
					serviceInstance.LegacyInstance.StateChanged += new EventHandler<Edge.Core.Services.ServiceStateChangedEventArgs>(LegacyInstance_StateChanged);
					serviceInstance.LegacyInstance.ChildServiceRequested += new EventHandler<Edge.Core.Services.ServiceRequestedEventArgs>(LegacyInstance_ChildServiceRequested);
					serviceInstance.LegacyInstance.Initialize();
					this.Invoke(setLogMethod, new Object[] { string.Format("\nService: {0} initalized {1}\r\n", serviceInstance.ServiceName, DateTime.Now.ToString("dd/MM/yy HH:mm")) });

					


				}
			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}

		}

		void LegacyInstance_ChildServiceRequested(object sender, Edge.Core.Services.ServiceRequestedEventArgs e)
		{
			try
			{

				legacy.ServiceInstance instance = (legacy.ServiceInstance)sender;

				this.Invoke(setLogMethod, new Object[] { string.Format("\nChild Service: {0} requestedd {1}\r\n", e.RequestedService.Configuration.Name, DateTime.Now.ToString("dd/MM/yy HH:mm")) });

				e.RequestedService.ChildServiceRequested += new EventHandler<legacy.ServiceRequestedEventArgs>(LegacyInstance_ChildServiceRequested);
				e.RequestedService.StateChanged += new EventHandler<legacy.ServiceStateChangedEventArgs>(LegacyInstance_StateChanged);
				e.RequestedService.Initialize();
			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}
		}



		void LegacyInstance_StateChanged(object sender, Edge.Core.Services.ServiceStateChangedEventArgs e)
		{
			try
			{
				legacy.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;
				instance.OutcomeReported += new EventHandler(instance_OutcomeReported);

				this.Invoke(updateGridMethod, new Object[] { instance });


				this.Invoke(setLogMethod, new Object[] { string.Format("\n{0}: {1} is {2} {3}\r\n", instance.AccountID, instance.Configuration.Name, e.StateAfter, DateTime.Now.ToString("dd/MM/yy HH:mm")) });
				if (e.StateAfter == legacy.ServiceState.Ready)
					instance.Start();
			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}

		}

		void instance_OutcomeReported(object sender, EventArgs e)
		{
			legacy.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;
			this.Invoke(updateGridMethod, new Object[] { instance });

		}

		void _scheduler_NewScheduleCreatedEventHandler(object sender, EventArgs e)
		{
			try
			{
				this.Invoke(setLogMethod, new Object[] { "Schedule Created:" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "\r\n" });
				ScheduledInformationEventArgs ee = (ScheduledInformationEventArgs)e;
				_scheduledServices.Clear();
				_strNotScheduled.Clear();



				foreach (KeyValuePair<SchedulingData, ServiceInstance> notSchedInfo in ee.NotScheduledInformation)
					_strNotScheduled.AppendLine(string.Format("Service {0} with profile {1} not scheduled", notSchedInfo.Value.ServiceName, notSchedInfo.Key.profileID));

				foreach (KeyValuePair<SchedulingData, ServiceInstance> SchedInfo in ee.ScheduleInformation)
					_scheduledServices.Add(SchedInfo.Key, SchedInfo.Value);

				GetScheduleServices();

				if (!string.IsNullOrEmpty(_strNotScheduled.ToString()))
					try
					{
						//Exception notScheduleException = new Exception(string.Format("Some servies could not be schedule:\n{0}", _strNotScheduled.ToString()));
						//Edge.Core.Utilities.Log.Write("Scheduler", "Some services could not be scheduled", null, Edge.Core.Utilities.LogMessageType.Information);

						//this.Invoke(setLogMethod, new Object[] { _strNotScheduled.ToString() });
					}
					catch (Exception)
					{


					}
			}

			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}

		}

		private void ScheduleBtn_Click(object sender, EventArgs e)
		{
			try
			{
				_strNotScheduled.Clear();
				_scheduledServices.Clear();
				_scheduler.Schedule(false);
			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}


		}

		private void GetScheduleServices()
		{
			try
			{
				var scheduledServices = _scheduler.GetAlllScheduldServices();
				this.Invoke(new Action<IEnumerable<KeyValuePair<Edge.Core.Scheduling.Objects.SchedulingData, ServiceInstance>>>(SetGridData), scheduledServices);
			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}
		}


		private void UpdateGridData(legacy.ServiceInstance serviceInstance)
		{
			try
			{
				foreach (DataGridViewRow row in scheduleInfoGrid.Rows)
				{
					if (Object.Equals(row.Tag, serviceInstance))
					{
						row.Cells["dynamicStaus"].Value = serviceInstance.State;
						row.Cells["outCome"].Value = serviceInstance.Outcome;
						row.Cells["actualEndTime"].Value = serviceInstance.TimeEnded.ToString("dd/MM/yyyy HH:mm:ss");
						row.Cells["instanceID"].Value = serviceInstance.InstanceID;

						Color color = GetColorByState(serviceInstance.State, serviceInstance.Outcome);
						row.DefaultCellStyle.BackColor = color;
					}
				}
			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}

		}

		private static Color GetColorByState(legacy.ServiceState state, legacy.ServiceOutcome outCome, bool deleted = false)
		{
			Color color = Color.White;
			try
			{
				if (deleted != true)
				{
					switch (state)
					{

						case Edge.Core.Services.ServiceState.Uninitialized:
							break;
						case Edge.Core.Services.ServiceState.Initializing:
							color = Color.FromArgb(0xdd, 0xdd, 0xdd); // light gray
							break;
						case Edge.Core.Services.ServiceState.Ready:
							color = Color.FromArgb(0xee, 0xee, 0xff); // light blue
							break;
						case Edge.Core.Services.ServiceState.Starting:
							color = Color.Green; // green
							break;
						case Edge.Core.Services.ServiceState.Running:
							color = Color.FromArgb(0xe0, 0xff, 0xe0); // light green
							break;
						case Edge.Core.Services.ServiceState.Waiting:
							color = Color.FromArgb(0xee, 0xff, 0xee); // light green (a little lighter)
							break;
						case Edge.Core.Services.ServiceState.Ended:
							{
								if (outCome == legacy.ServiceOutcome.Success)
									color = Color.Turquoise;
								else if (outCome == legacy.ServiceOutcome.Failure)
									color = Color.DarkRed;
								else if (outCome == legacy.ServiceOutcome.Unspecified)
									color = Color.Orange;
								else if (outCome == legacy.ServiceOutcome.Aborted)
									color = Color.Purple;
								break;

							}

						case Edge.Core.Services.ServiceState.Aborting:
							color = Color.Red;
							break;
						default:
							break;
					}
				}
				else
					color = Color.DarkGray;


			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("GetColorByState", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}
			return color;
		}

		private void SetGridData(IEnumerable<KeyValuePair<Edge.Core.Scheduling.Objects.SchedulingData, ServiceInstance>> scheduledServices)
		{
			try
			{

				scheduleInfoGrid.Rows.Clear();
				foreach (var scheduledService in scheduledServices.OrderBy(s => s.Value.StartTime))
				{
					string date;
					if (scheduledService.Value.LegacyInstance.Configuration.Options.ContainsKey("Date"))
						date = scheduledService.Value.LegacyInstance.Configuration.Options["Date"];
					else if (scheduledService.Value.LegacyInstance.Configuration.Options.ContainsKey("TimePeriod"))
						date = scheduledService.Value.LegacyInstance.Configuration.Options["TimePeriod"];
					else
						date = string.Empty;



					if (!_scheduledServices.ContainsKey(scheduledService.Key))
						_scheduledServices.Add(scheduledService.Key, scheduledService.Value);
					int row = scheduleInfoGrid.Rows.Add(new object[] 
                    { scheduledService.Key.GetHashCode(),scheduledService.Value.LegacyInstance.InstanceID, scheduledService.Value.ServiceName, scheduledService.Value.ProfileID, 
                        scheduledService.Value.StartTime.ToString("dd/MM/yyy HH:mm:ss"), scheduledService.Value.EndTime.ToString("dd/MM/yyy HH:mm:ss"),
                        scheduledService.Value.LegacyInstance.TimeEnded.ToString("dd/MM/yyy HH:mm:ss"), scheduledService.Value.LegacyInstance.State,
                        scheduledService.Key.Rule.Scope, scheduledService.Value.Deleted, scheduledService.Value.LegacyInstance.Outcome,
                        scheduledService.Value.LegacyInstance.State, scheduledService.Value.Priority ,
                        date
                    });
					scheduleInfoGrid.Rows[row].DefaultCellStyle.BackColor = GetColorByState(scheduledService.Value.LegacyInstance.State, scheduledService.Value.LegacyInstance.Outcome, scheduledService.Value.Deleted);
					scheduleInfoGrid.Rows[row].Tag = scheduledService.Value.LegacyInstance;
					
					

				}


			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}
		}
		private void ViewLog_newForm(object sender, EventArgs e)
		{
			//Get Log from DB per instance id
			frmInstanceLog form = new frmInstanceLog(this._listner);
			form.UpdateForm(
				Convert.ToInt64(scheduleInfoGrid.SelectedRows[0].Cells[1].Value),
				instanceName: scheduleInfoGrid.SelectedRows[0].Cells[2].Value.ToString(),
				accountId: scheduleInfoGrid.SelectedRows[0].Cells[3].Value.ToString()
				);
			form.Show();
			
		}
		private void endServiceBtn_Click(object sender, EventArgs e)
		{
			try
			{
				DialogResult result = MessageBox.Show("Are you sure you want to abort service?", "Abort Service", MessageBoxButtons.YesNo);

				if (result == System.Windows.Forms.DialogResult.Yes)
				{
					foreach (DataGridViewRow row in scheduleInfoGrid.SelectedRows)
					{
						var scheduleData = from s in _scheduledServices
										   where s.Key.GetHashCode() == Convert.ToInt32(row.Cells["shceduledID"].Value)
										   select s.Key;

						try
						{
							_scheduler.AbortRuningService(scheduleData.First());
						}
						catch (Exception ex)
						{

							MessageBox.Show(string.Format("You cannot delete service in this state\n{0}", ex.Message));
						}

					}

					GetScheduleServices();
				}
			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}

		}

		private void deleteServiceFromScheduleBtn_Click(object sender, EventArgs e)
		{
			try
			{
				DialogResult result = MessageBox.Show("Are you sure you want to Delete service from schedule?", "Delete Service", MessageBoxButtons.YesNo);
				if (result == System.Windows.Forms.DialogResult.Yes)
				{
					foreach (DataGridViewRow row in scheduleInfoGrid.SelectedRows)
					{
						var scheduleData = from s in _scheduledServices
										   where s.Key.GetHashCode() == Convert.ToInt32(row.Cells["shceduledID"].Value)
										   select s.Key;
						if (_scheduledServices[scheduleData.First()].LegacyInstance.State == Edge.Core.Services.ServiceState.Uninitialized)
						{
							SchedulingData schedulingDate = scheduleData.First();
							_scheduler.DeleteScpecificServiceInstance(schedulingDate);
							row.Cells["deleted"].Value = true;
							row.DefaultCellStyle.BackColor = GetColorByState(legacy.ServiceState.Aborting, legacy.ServiceOutcome.Aborted, true);


						}
						else
							MessageBox.Show(string.Format("You can't delete service instance with state {0}", _scheduledServices[scheduleData.First()].LegacyInstance.State));

					}
					//GetScheduleServices(); 

				}
			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}

		}

		private void startBtn_Click(object sender, EventArgs e)
		{
			try
			{
				startBtn.Enabled = false;
				EndBtn.Enabled = true;
				_scheduleStarted = true;
				ConfigurationManager.RefreshSection("edge.services");
				_scheduler.Start();

				logtextBox.Text = logtextBox.Text.Insert(0, "Timer Started:" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "\r\n");
				

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}
		}

		private void EndBtn_Click(object sender, EventArgs e)
		{
			try
			{
				_scheduler.Stop();
				this.Invoke(setLogMethod, new Object[] { "Timer Stoped" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "r\n" });
				Log.Write("SchedulingControlForm", "Threads stops by stop button", LogMessageType.Information);
				startBtn.Enabled = true;
				EndBtn.Enabled = false;

			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}
		}

		public void SetLogTextBox(string lineText)
		{
			try
			{
				lock (logtextBox)
				{
					if (logtextBox.Text.Length > 4000)
						logtextBox.Text = logtextBox.Text.Remove(4000);
					logtextBox.Text = logtextBox.Text.Insert(0, lineText);
				}
			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}
		}

		private void frmSchedulingControl_FormClosing(object sender, FormClosingEventArgs e)
		{
			DialogResult result = MessageBox.Show("YOU MUST NOT CLOSE THIS FORM, IF YOU HAVE TO,\nPLEASE TALK WITH SHAY BAR-CHEN OR AMIT BLUMAN,\nARE YOU SURE YOU WANT TO CLOSE THE SCHEDULER?", "Form Closing!", MessageBoxButtons.YesNo);
			if (result == System.Windows.Forms.DialogResult.Yes)
			{
				result = MessageBox.Show("Are You Sure?", "Form Closing!", MessageBoxButtons.YesNo);
				if (result == System.Windows.Forms.DialogResult.Yes)
					e.Cancel = false;
				else
					e.Cancel = true;
			}
			else
			{
				e.Cancel = true;

			}

		}

		private void unPlannedBtn_Click(object sender, EventArgs e)
		{
			ConfigurationManager.RefreshSection("edge.services");
			frmUnPlannedService f = new frmUnPlannedService(_listner, _scheduler);
			f.Show();
		}
        private void encryptDecryptBtn_Click(object sender, EventArgs e)
        {
            frmEncryptDecrypt form = new frmEncryptDecrypt();
            form.Show();
        }

		private void resetServiceInstanceStateBtn_Click(object sender, EventArgs e)
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

		private void scheduleInfoGrid_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				int currentMouseOverRow = scheduleInfoGrid.HitTest(e.X, e.Y).RowIndex;
				if (currentMouseOverRow != -1)
				{
					scheduleInfoGrid.Rows[currentMouseOverRow].ContextMenuStrip = new ContextMenuStrip();
					scheduleInfoGrid.Rows[currentMouseOverRow].ContextMenuStrip.Items.Add("View Log");
					scheduleInfoGrid.Rows[currentMouseOverRow].Selected = true;
					scheduleInfoGrid.Rows[currentMouseOverRow].ContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(ViewLog_newForm);
				}
				
					
			}
		}

	}
}
