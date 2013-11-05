using System;
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
		private Scheduler _scheduler;
		private Listener _listener;
		private Dictionary<SchedulingData, ServiceInstance> _scheduledServices = new Dictionary<SchedulingData, ServiceInstance>();
		Action<string> _delegateLogText;
		Action _delegateGridRefresh;
		Action<legacy.ServiceInstance> _delegateGridUpdate;
		EventHandler _applicationExitHandler;

		public frmSchedulingControl()
		{
			InitializeComponent();

			_delegateLogText = new Action<string>(LogTextInner);
			_delegateGridRefresh = new Action(GridRefreshInner);
			_delegateGridUpdate = new Action<legacy.ServiceInstance>(GridUpdateInner);

			this.FormClosed += new FormClosedEventHandler((sender, e) =>
			{
				if (e.CloseReason == CloseReason.UserClosing)
					Application.Exit();
			});

			_applicationExitHandler = new EventHandler((sender, e) =>
			{
				Application.ApplicationExit -= _applicationExitHandler;

				try { _listener.Close(); }
				catch { /* Don't care about WCF host problems */ }

				_scheduler.Stop();

			});
			Application.ApplicationExit += _applicationExitHandler;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			lblVer.Text = Application.ProductVersion;
			EndBtn.Enabled = false;
			this.Text = System.AppDomain.CurrentDomain.FriendlyName;

			_scheduler = new Scheduler(true);
			_listener = new Listener(_scheduler);
			_listener.Start();

			_scheduler.ServiceRunRequiredEvent += new EventHandler(_scheduler_ServiceRunRequiredEvent);
			_scheduler.NewScheduleCreatedEvent += new EventHandler(_scheduler_NewScheduleCreatedEventHandler);

			_scheduler.StateChanged += new EventHandler((ss, se) =>
			{
				LogText("Scheduler is " + _scheduler.State.ToString());
				try
				{
					Invoke(new Action(() =>
					{
						startBtn.Enabled = _scheduler.State == SchedulerState.Stopped;
						EndBtn.Enabled = _scheduler.State == SchedulerState.Started;
					}));
				}
				catch { }
			});

			Application.DoEvents();

			_scheduler.Start();
		}

		#region GUI UPDATE
		// ======================================================================
		// Handle refreshing info

		void LogText(string lineText)
		{
			try { Invoke(_delegateLogText, lineText); }
			catch { }
		}

		void LogTextInner(string lineText)
		{
			if (logtextBox.Text.Length > 4000)
				logtextBox.Text = logtextBox.Text.Remove(4000);
			logtextBox.Text = logtextBox.Text.Insert(0, String.Format("{0} >> {1}\r\n",
				DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
				lineText));
		}

		void GridRefresh()
		{
			try { Invoke(_delegateGridRefresh); }
			catch { }
		}

		private void GridRefreshInner()
		{
			var scheduledServices = _scheduler.GetAllScheduledServices();
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
                    {
						scheduledService.Key.GetHashCode(),scheduledService.Value.LegacyInstance.InstanceID, scheduledService.Value.ServiceName, scheduledService.Value.ProfileID, 
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


		void GridUpdate(legacy.ServiceInstance instance)
		{
			try { Invoke(_delegateGridUpdate, instance); }
			catch { }
		}

		void GridUpdateInner(legacy.ServiceInstance instance)
		{
			foreach (DataGridViewRow row in scheduleInfoGrid.Rows)
			{
				if (Object.Equals(row.Tag, instance))
				{
					row.Cells["dynamicStaus"].Value = instance.State;
					row.Cells["outCome"].Value = instance.Outcome;
					row.Cells["actualEndTime"].Value = instance.TimeEnded.ToString("dd/MM/yyyy HH:mm:ss");
					row.Cells["instanceID"].Value = instance.InstanceID;

					Color color = GetColorByState(instance.State, instance.Outcome);
					row.DefaultCellStyle.BackColor = color;
				}
			}
		}

		// ======================================================================
		#endregion

		#region OUT OF THREAD METHODS
		// ======================================================================
		// These are triggered async by other AppDomains or threads

		void _scheduler_NewScheduleCreatedEventHandler(object sender, EventArgs e)
		{
			LogText("New schedule created");
			Log.Write(Program.LS, "New schedule created", LogMessageType.Information);

			var args = (ScheduledInformationEventArgs)e;
			_scheduledServices.Clear();

			foreach (KeyValuePair<SchedulingData, ServiceInstance> info in args.ScheduleInformation)
				_scheduledServices.Add(info.Key, info.Value);

			GridRefresh();
		}

		void _scheduler_ServiceRunRequiredEvent(object sender, EventArgs e)
		{
			TimeToRunEventArgs args = (TimeToRunEventArgs)e;
			foreach (Edge.Core.Scheduling.Objects.ServiceInstance serviceInstance in args.ServicesToRun)
			{
				serviceInstance.LegacyInstance.StateChanged += new EventHandler<Edge.Core.Services.ServiceStateChangedEventArgs>(LegacyInstance_StateChanged);
				serviceInstance.LegacyInstance.ChildServiceRequested += new EventHandler<Edge.Core.Services.ServiceRequestedEventArgs>(LegacyInstance_ChildServiceRequested);
				serviceInstance.LegacyInstance.OutcomeReported += new EventHandler(LegacyInstance_OutcomeReported);

				try { serviceInstance.LegacyInstance.Initialize(); }
				catch (Exception ex)
				{
					string msg = String.Format("{1}: Failed to initialize scheduled service {0}", serviceInstance.ServiceName, serviceInstance.LegacyInstance.AccountID);
					LogText(msg);
					Log.Write(Program.LS, msg, ex);
				}
			}
		}

		void LegacyInstance_ChildServiceRequested(object sender, Edge.Core.Services.ServiceRequestedEventArgs e)
		{

			legacy.ServiceInstance instance = (legacy.ServiceInstance)sender;

			e.RequestedService.ChildServiceRequested += new EventHandler<legacy.ServiceRequestedEventArgs>(LegacyInstance_ChildServiceRequested);
			e.RequestedService.StateChanged += new EventHandler<legacy.ServiceStateChangedEventArgs>(LegacyInstance_StateChanged);
			e.RequestedService.OutcomeReported += new EventHandler(LegacyInstance_OutcomeReported);

			try { e.RequestedService.Initialize(); }
			catch (Exception ex)
			{
				string msg = String.Format("{1}: Failed to initialize child service {0}", instance.Configuration.Name, instance.AccountID);
				LogText(msg);
				Log.Write(Program.LS, msg, ex);
			}
		}

		void LegacyInstance_StateChanged(object sender, Edge.Core.Services.ServiceStateChangedEventArgs e)
		{
			legacy.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;
			GridUpdate(instance);
			LogText(string.Format("{0}: {1} is {2}", instance.AccountID, instance.Configuration.Name, e.StateAfter));

			if (e.StateAfter == legacy.ServiceState.Ready)
			{
				try { instance.Start(); }
				catch (Exception ex)
				{
					string msg = String.Format("{1}: Failed to start service {0}", instance.Configuration.Name, instance.AccountID);
					LogText(msg);
					Log.Write(Program.LS, msg, ex);
				}
			}
		}

		void LegacyInstance_OutcomeReported(object sender, EventArgs e)
		{
			legacy.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;
			GridUpdate(instance);
			LogText(string.Format("{0}: {1} reported {2}", instance.AccountID, instance.Configuration.Name, instance.Outcome));
		}
		// ======================================================================
		#endregion

		#region UTILITY
		// ======================================================================
		// extra

		static Color GetColorByState(legacy.ServiceState state, legacy.ServiceOutcome outCome, bool deleted = false)
		{
			Color color = Color.White;

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

			return color;
		}
		// ======================================================================
		#endregion

		#region GUI INTERACTION
		// ======================================================================
		// button clicks etc.

		private void ViewLog_newForm(object sender, EventArgs e)
		{
			//Get Log from DB per instance id
			frmInstanceLog form = new frmInstanceLog(this._listener);
			form.UpdateForm(
				Convert.ToInt64(scheduleInfoGrid.SelectedRows[0].Cells[1].Value),
				instanceName: scheduleInfoGrid.SelectedRows[0].Cells[2].Value.ToString(),
				accountId: scheduleInfoGrid.SelectedRows[0].Cells[3].Value.ToString()
				);
			form.Show();

		}
		private void endServiceBtn_Click(object sender, EventArgs e)
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
						_scheduler.AbortRunningService(scheduleData.First());
					}
					catch (Exception ex)
					{

						MessageBox.Show(string.Format("You cannot delete service in this state\n{0}", ex.Message));
					}

				}

				GridRefresh();
			}
		}

		private void deleteServiceFromScheduleBtn_Click(object sender, EventArgs e)
		{

			DialogResult result = MessageBox.Show("Are you sure you want to delete service from schedule?", "Delete Service", MessageBoxButtons.YesNo);
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
			}
		}

		private void startBtn_Click(object sender, EventArgs e)
		{
			_scheduler.Start();
		}

		private void EndBtn_Click(object sender, EventArgs e)
		{
			_scheduler.Stop();
		}

		private void frmSchedulingControl_FormClosing(object sender, FormClosingEventArgs e)
		{
			DialogResult result = MessageBox.Show("WARNING: This will shutdown all Edge services. Are you sure you want to continue?", "SCHEDULER SHUTDOWN", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
			if (result == System.Windows.Forms.DialogResult.Yes)
			{
				result = MessageBox.Show("Please confirm again", "SCHEDULER SHUTDOWN", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
				if (result == System.Windows.Forms.DialogResult.OK)
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
			frmUnPlannedService f = new frmUnPlannedService(_listener, _scheduler);
			f.Show();
		}
		private void encryptDecryptBtn_Click(object sender, EventArgs e)
		{
			frmEncryptDecrypt form = new frmEncryptDecrypt();
			form.Show();
		}

		private void resetServiceInstanceStateBtn_Click(object sender, EventArgs e)
		{

			var proc = new BackgroundWorker()
			{
				WorkerReportsProgress = false,
				WorkerSupportsCancellation = false
			};
			proc.DoWork += new DoWorkEventHandler((w, args) =>
			{
				try
				{
					using (SqlConnection conn = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
					{
						conn.Open();
						using (SqlCommand command = DataManager.CreateCommand("ResetUnendedServices", CommandType.StoredProcedure))
						{
							command.Connection = conn;
							args.Result = command.ExecuteNonQuery();
						}
					}
				}
				catch (Exception ex)
				{
					args.Result = ex;
				}
			});
			proc.RunWorkerCompleted += new RunWorkerCompletedEventHandler((w, args) =>
			{
				if (args.Result is Exception)
				{
					var ex = (Exception)args.Result;
					MessageBox.Show(String.Format("Reset operation failed: {0} ({1})", ex.Message, ex.GetType().Name));
				}
				else
				{
					MessageBox.Show(String.Format("{0} row(s) affected", (int)args.Result));
				}
			});
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

		#endregion

	}
}
