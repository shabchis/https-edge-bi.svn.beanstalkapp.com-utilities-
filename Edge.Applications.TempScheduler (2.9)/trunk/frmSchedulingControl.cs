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
using Edge.Data.Pipeline;


namespace Edge.Applications.TempScheduler
{
	public partial class frmSchedulingControl : Form
	{
		Scheduler _scheduler;
		Listener _listener;
		Action<string> _delegateLogText;
		Action<Scheduler.Entry[]> _delegateGridRefresh;
		Action<legacy.ServiceInstance> _delegateGridUpdate;
		EventHandler _applicationExitHandler;

		public frmSchedulingControl()
		{
			InitializeComponent();

			_delegateLogText = new Action<string>(LogTextInner);
			_delegateGridRefresh = new Action<Scheduler.Entry[]>(GridRefreshInner);
			_delegateGridUpdate = new Action<legacy.ServiceInstance>(GridUpdateInstanceInner);

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
			btnSchedulerStop.Enabled = false;
			this.Text += String.Format(" ({0})" , System.AppDomain.CurrentDomain.FriendlyName);

			_scheduler = new Scheduler(true);
			_listener = new Listener(_scheduler);
			_listener.Start();

			_scheduler.ServiceRunRequired += new EventHandler<ServiceRunRequiredEventArgs>(_scheduler_ServiceRunRequired);
			_scheduler.ScheduleCreated += new EventHandler<ScheduleCreatedEventsArgs>(_scheduler_ScheduleCreated);

			_scheduler.StateChanged += new EventHandler((ss, se) =>
			{
				LogText("Scheduler is " + _scheduler.State.ToString());
				try
				{
					Invoke(new Action(() =>
					{
						btnSchedulerStart.Enabled = _scheduler.State == SchedulerState.Stopped;
						btnSchedulerStop.Enabled = _scheduler.State == SchedulerState.Started;
					}));
				}
				catch { }
			});

			Application.DoEvents();

			//_scheduler.Start();
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

		void GridRefresh(Scheduler.Entry[] entries)
		{
			try { Invoke(_delegateGridRefresh, entries); }
			catch { }
		}

		private void GridRefreshInner(Scheduler.Entry[] entries)
		{
			scheduleInfoGrid.SuspendLayout();

			int r = 0;
			var scheduled = entries.ToDictionary(entry => entry.Data.Guid);
			var displayed = new Dictionary<Guid, Scheduler.Entry>();
			while (r < scheduleInfoGrid.Rows.Count)
			{
				DataGridViewRow row = scheduleInfoGrid.Rows[r];
				var rowData = (Scheduler.Entry)row.Tag;
				if (!scheduled.ContainsKey(rowData.Data.Guid))
				{
					// row's gone
					scheduleInfoGrid.Rows.RemoveAt(r);
				}
				else
				{
					// row's still here, update it
					displayed.Add(rowData.Data.Guid, rowData);
					GridUpdateRow(row);
					r++;
				}
			}

			// Add new ones
			foreach (var entry in scheduled.OrderBy(s => s.Value.Instance.StartTime))
			{
				if (displayed.ContainsKey(entry.Key))
					continue;

				int newr = scheduleInfoGrid.Rows.Add();
				DataGridViewRow row = scheduleInfoGrid.Rows[newr];

				row.Tag = entry.Value;
				GridUpdateRow(row);
			}

			scheduleInfoGrid.ResumeLayout();
		}


		void GridUpdateInstance(legacy.ServiceInstance instance)
		{
			try { Invoke(_delegateGridUpdate, instance); }
			catch { }
		}

		void GridUpdateInstanceInner(legacy.ServiceInstance legacyInstance)
		{
			foreach (DataGridViewRow row in scheduleInfoGrid.Rows)
			{
				var rowData = (Scheduler.Entry)row.Tag;
				if (Object.Equals(rowData.Instance.LegacyInstance, legacyInstance))
				{
					GridUpdateRow(row);
					break;
				}
			}
		}

		void GridUpdateRow(DataGridViewRow row)
		{
			var rowData = (Scheduler.Entry)row.Tag;
			ServiceInstance instance = rowData.Instance;

			// .....
			// calculate time period to display
			string timePeriodStart = null;
			string timePeriodEnd = null;
			string timePeriodRaw;
			DateTimeRange timePeriod = instance.LegacyInstance.Configuration.Options.TryGetValue(Edge.Data.Pipeline.Services.PipelineService.ConfigurationOptionNames.TimePeriod, out timePeriodRaw) ?
				DateTimeRange.Parse(timePeriodRaw) :
				DateTimeRange.AllOfYesterday;

			if (instance.LegacyInstance.State != legacy.ServiceState.Uninitialized)
			{
				timePeriod = timePeriod.ToAbsolute();
				timePeriodStart = timePeriod.Start.ToDateTime().ToString("dd/MM/yyy");
				timePeriodEnd = timePeriod.End.ToDateTime().ToString("dd/MM/yyy");
			}
			else
			{
				timePeriodStart = timePeriod.Start.ToString();
				timePeriodEnd = timePeriod.End.ToString();
			}

			// .....
			// update the row values
			row.Cells["accountID"].Value = instance.ProfileID;
			row.Cells["serviceName"].Value = instance.ServiceName;
			row.Cells["instanceID"].Value = instance.LegacyInstance.InstanceID >= 0 ? instance.LegacyInstance.InstanceID.ToString() : string.Empty;
			row.Cells["status"].Value = instance.LegacyInstance.State == legacy.ServiceState.Ended ? instance.LegacyInstance.Outcome.ToString() : instance.LegacyInstance.State.ToString();
			row.Cells["timeEnded"].Value = instance.LegacyInstance.State == legacy.ServiceState.Ended ? instance.LegacyInstance.TimeEnded.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
			row.Cells["scheduledStart"].Value = instance.StartTime.ToString("dd/MM/yyy HH:mm:ss");
			row.Cells["scheduledEnd"].Value = instance.EndTime.ToString("dd/MM/yyy HH:mm:ss");
			row.Cells["timePeriodStart"].Value = timePeriodStart;
			row.Cells["timePeriodEnd"].Value = timePeriodEnd;

			// .....
			// update the row color
			Color color = Color.White;
			switch (instance.LegacyInstance.State)
			{
				case Edge.Core.Services.ServiceState.Initializing:
				case Edge.Core.Services.ServiceState.Ready:
				case Edge.Core.Services.ServiceState.Starting:
					color = 0xFFFFFFDD.ToColor();
					break;
				case Edge.Core.Services.ServiceState.Waiting:
				case Edge.Core.Services.ServiceState.Running:
					color = 0xFFDDFFDD.ToColor();
					break;
				case Edge.Core.Services.ServiceState.Aborting:
					color = 0xFFffeeaa.ToColor();
					break;
				case Edge.Core.Services.ServiceState.Ended:
					switch (instance.LegacyInstance.Outcome)
					{
						case legacy.ServiceOutcome.Success:
							color = 0xFFddeeff.ToColor();
							break;
						case legacy.ServiceOutcome.Failure:
							color = 0xFFee8888.ToColor();
							break;
						case legacy.ServiceOutcome.Aborted:
						case legacy.ServiceOutcome.Reset:
						default:
							color = 0xFFaaaaaa.ToColor();
							break;
					}
					break;
				default:
					break;

			}

			row.DefaultCellStyle.BackColor = color;

			if (instance.Removed && !row.DefaultCellStyle.Font.Strikeout)
				row.DefaultCellStyle.Font = new Font(row.DefaultCellStyle.Font, FontStyle.Strikeout);
		}

		


		// ======================================================================
		#endregion

		#region OUT OF THREAD METHODS
		// ======================================================================
		// These are triggered async by other AppDomains or threads

		void _scheduler_ScheduleCreated(object sender, ScheduleCreatedEventsArgs e)
		{
			LogText("New schedule created");
			Log.Write(Program.LS, "New schedule created", LogMessageType.Information);

			GridRefresh(_scheduler.GetAllScheduledServices());
		}

		void _scheduler_ServiceRunRequired(object sender, ServiceRunRequiredEventArgs e)
		{
			foreach (Edge.Core.Scheduling.Objects.ServiceInstance serviceInstance in e.ServicesToRun)
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
			GridUpdateInstance(instance);
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
			GridUpdateInstance(instance);
			LogText(string.Format("{0}: {1} reported {2}", instance.AccountID, instance.Configuration.Name, instance.Outcome));
		}
		// ======================================================================
		#endregion

		
		#region GUI INTERACTION
		// ======================================================================
		// button clicks etc.

		private void btnSchedulerStart_Click(object sender, EventArgs e)
		{
			_scheduler.Start();
		}

		private void btnSchedulerStop_Click(object sender, EventArgs e)
		{
			_scheduler.Stop();
		}

		private void btnServicesUnplanned_Click(object sender, EventArgs e)
		{
			frmUnPlannedService f = new frmUnPlannedService(_listener, _scheduler);
			f.Show();
		}

		private void btnServicesAbort_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Are you sure you want to abort the selected instances?", "Abort Service", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

			if (result == System.Windows.Forms.DialogResult.Yes)
			{
				foreach (DataGridViewRow row in scheduleInfoGrid.SelectedRows)
				{
					var rowData = (Scheduler.Entry)row.Tag;

					try
					{
						rowData.Instance.LegacyInstance.Abort();
						GridUpdateRow(row);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
				}
			}
		}

		private void btnServicesRemove_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Are you sure you want to remove this instance from the schedule?", "Remove Service", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			if (result == System.Windows.Forms.DialogResult.Yes)
			{
				foreach (DataGridViewRow row in scheduleInfoGrid.SelectedRows)
				{
					var rowData = (Scheduler.Entry)row.Tag;

					if (rowData.Instance.LegacyInstance.State == Edge.Core.Services.ServiceState.Uninitialized)
					{
						_scheduler.RemoveFromSchedule(rowData.Data);
						GridUpdateRow(row);
					}
					else
						MessageBox.Show("Only uninitialized services can be removed from the schedule.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);

				}
			}
		}

		private void btnServicesReset_Click(object sender, EventArgs e)
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
			proc.RunWorkerAsync();
		}

		private void btnToolsEncrypt_Click(object sender, EventArgs e)
		{
			frmEncryptDecrypt form = new frmEncryptDecrypt();
			form.Show();
		}

		private void ViewLog_newForm(object sender, EventArgs e)
		{
			//Get Log from DB per instance id
			frmInstanceLog form = new frmInstanceLog(this._listener);
			form.UpdateForm(
				Convert.ToInt64(scheduleInfoGrid.SelectedRows[0].Cells["instanceID"].Value),
				instanceName: scheduleInfoGrid.SelectedRows[0].Cells["serviceName"].Value.ToString(),
				accountId: scheduleInfoGrid.SelectedRows[0].Cells["accountID"].Value.ToString()
				);
			form.Show();

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

	static class ExtensionMethods
	{
		public static Color ToColor(this uint argb)
		{
			return Color.FromArgb((byte)((argb & -16777216) >> 0x18),
								  (byte)((argb & 0xff0000) >> 0x10),
								  (byte)((argb & 0xff00) >> 8),
								  (byte)(argb & 0xff));
		}
	}
}
