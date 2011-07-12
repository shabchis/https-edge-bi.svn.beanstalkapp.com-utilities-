﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
		public delegate void SetStepsProgressMethod(KeyValuePair<Edge.Core.Scheduling.Objects.SchedulingData, ServiceInstance> rowTag, DataGridViewRow row = null);
		SetLogMethod setLogMethod;
		UpdateGridMethod updateGridMethod;
		SetStepsProgressMethod setStepsProgressMethod;
		bool _scheduleStarted = false;
		Thread _timerToStartScheduling;
		Dictionary<string, List<StepProperties>> _stepsByConfiguration = new Dictionary<string, List<StepProperties>>();
		object lastTag;


		public frmSchedulingControl()
		{
			try
			{
				InitializeComponent();

				setLogMethod = new SetLogMethod(SetLogTextBox);
				updateGridMethod = new UpdateGridMethod(UpdateGridData);
				this.FormClosed += new FormClosedEventHandler(frmSchedulingControl_FormClosed);
				setStepsProgressMethod = new SetStepsProgressMethod(SetStepsView);
				LoadBaseConfigurations();


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

					this.Text = System.AppDomain.CurrentDomain.FriendlyName;
					_timerToStartScheduling = new Thread(new ThreadStart(delegate()
			{
				Thread.Sleep(new TimeSpan(0, 1, 0));
				if (!_scheduleStarted)
				{
					_scheduler.Start();
				}
			}));
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
				this.Invoke(setLogMethod, new object[] { string.Format("Service: {0} for account: {1} will not run at all!!!!!!!!", notRunService.Configuration.Name, notRunService.profileID) });
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
					
					Edge.Core.Utilities.Log.Write("SchedulingControlForm", string.Format("Service: {0} initalized", serviceInstance.ServiceName), Edge.Core.Utilities.LogMessageType.Information);


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
				legacy.ServiceInstance child = e.RequestedService;
				child.ProgressReported += new EventHandler(child_ProgressReported);
				e.RequestedService.Initialize();
			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}
		}

		void child_ProgressReported(object sender, EventArgs e)
		{
			legacy.ServiceInstance instance = (legacy.ServiceInstance)sender;
			if (_stepsByConfiguration.ContainsKey(instance.ParentInstance.Configuration.Name))
			{

				IEnumerable<StepProperties> steps = from s in _stepsByConfiguration[instance.ParentInstance.Configuration.Name]
													where s.step.ActualName == instance.Configuration.Name
													select s;

				StepProperties step = steps.First();
				step.SetValue(instance.Progress);

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
						Exception notScheduleException = new Exception(string.Format("Some servies could not be schedule:\n{0}", _strNotScheduled.ToString()));
						//Edge.Core.Utilities.Log.Write("Scheduler", "Some services could not be scheduled", null, Edge.Core.Utilities.LogMessageType.Information);

						this.Invoke(setLogMethod, new Object[] { _strNotScheduled.ToString() });
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
				_scheduler.NewSchedule();
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
					if (Object.Equals(((KeyValuePair<SchedulingData, ServiceInstance>)row.Tag).Value.LegacyInstance, serviceInstance))
					{
						row.Cells["dynamicStaus"].Value = serviceInstance.State;
						row.Cells["outCome"].Value = serviceInstance.Outcome;
						row.Cells["actualEndTime"].Value = serviceInstance.TimeEnded.ToString("dd/MM/yyyy HH:mm:ss");

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
					else if (scheduledService.Value.LegacyInstance.Configuration.Options.ContainsKey("TargetPeriod"))
						date = scheduledService.Value.LegacyInstance.Configuration.Options["TargetPeriod"];
					else
						date = string.Empty;



					if (!_scheduledServices.ContainsKey(scheduledService.Key))
						_scheduledServices.Add(scheduledService.Key, scheduledService.Value);
					int row = scheduleInfoGrid.Rows.Add(new object[] { scheduledService.Key.GetHashCode(), scheduledService.Value.ServiceName, scheduledService.Value.ProfileID, 
                        scheduledService.Value.StartTime.ToString("dd/MM/yyy HH:mm:ss"), scheduledService.Value.EndTime.ToString("dd/MM/yyy HH:mm:ss"),
                        scheduledService.Value.LegacyInstance.TimeEnded.ToString("dd/MM/yyy HH:mm:ss"), scheduledService.Value.LegacyInstance.State,
                        scheduledService.Key.Rule.Scope, scheduledService.Value.Deleted, scheduledService.Value.LegacyInstance.Outcome,
                        scheduledService.Value.LegacyInstance.State, scheduledService.Value.Priority ,
                        date
                    });

					scheduleInfoGrid.Rows[row].DefaultCellStyle.BackColor = GetColorByState(scheduledService.Value.LegacyInstance.State, scheduledService.Value.LegacyInstance.Outcome, scheduledService.Value.Deleted);
					scheduleInfoGrid.Rows[row].Tag = scheduledService;

				}


			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}
		}
		private void LoadBaseConfigurations()
		{
			foreach (ServiceElement serviceElement in EdgeServicesConfiguration.Current.Services)
			{

				foreach (WorkflowStepElement step in serviceElement.Workflow)
				{
					if (step.IsEnabled)
					{
						if (!_stepsByConfiguration.ContainsKey(serviceElement.Name))
						{
							_stepsByConfiguration.Add(serviceElement.Name, new List<StepProperties>());
						}

						StepProperties stepProp = new StepProperties() { step = step, ParentName = serviceElement.Name };

						//stepProp.SetValue(0d);
						stepProp.ValueChanged += new EventHandler(stepProp_ValueChanged);
						_stepsByConfiguration[serviceElement.Name].Add(stepProp);
					}
				}
			}
		}

		void stepProp_ValueChanged(object sender, EventArgs e)
		{
			if (scheduleInfoGrid.SelectedRows.Count > 0)
			{
				KeyValuePair<Edge.Core.Scheduling.Objects.SchedulingData, ServiceInstance> tag = (KeyValuePair<Edge.Core.Scheduling.Objects.SchedulingData, ServiceInstance>)scheduleInfoGrid.SelectedRows[0].Tag;
				if (tag.Value.ServiceName == ((StepProperties)sender).ParentName)
					this.Invoke(new Action<KeyValuePair<Edge.Core.Scheduling.Objects.SchedulingData, ServiceInstance>, DataGridViewRow>(SetStepsView), tag, scheduleInfoGrid.SelectedRows[0]);

			}

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

		private void ReSchedule()
		{
			try
			{
				_scheduler.ReSchedule();
			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}
		}

		private void getServicesButton_Click(object sender, EventArgs e)
		{
			try
			{
				GetScheduleServices();
			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}
		}

		private void rescheduleBtn_Click(object sender, EventArgs e)
		{
			try
			{
				_strNotScheduled.Clear();
				_scheduledServices.Clear();
			}
			catch (Exception ex)
			{

				Edge.Core.Utilities.Log.Write("SchedulingControlForm", ex.Message, ex, Edge.Core.Utilities.LogMessageType.Error);
			}

		}

		private void button1_Click(object sender, EventArgs e)
		{
			ConfigurationManager.RefreshSection("edge.services");
			frmUnPlannedService f = new frmUnPlannedService(_listner, _scheduler);
			f.Show();
		}

		private void AddUnplanedServiceConfiguration()
		{

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
				{
					e.Cancel = false;
					_scheduler.Stop();
					Program.DeliveryServer.Stop();
					
					
				}
				else
				{
					e.Cancel = true;


				}

			}
			else
			{
				e.Cancel = true;
				
			}

		}



		private void scheduleInfoGrid_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
		{
			if (e.Row.Tag != null)
			{
				KeyValuePair<Edge.Core.Scheduling.Objects.SchedulingData, ServiceInstance> tag = (KeyValuePair<Edge.Core.Scheduling.Objects.SchedulingData, ServiceInstance>)e.Row.Tag;
				this.Invoke(new Action<KeyValuePair<Edge.Core.Scheduling.Objects.SchedulingData, ServiceInstance>, DataGridViewRow>(SetStepsView), tag, e.Row);
			}

			//this.Invoke(new Action<IEnumerable<KeyValuePair<Edge.Core.Scheduling.Objects.SchedulingData, ServiceInstance>>>(SetGridData), scheduledServices);
			//)e.Row.Tag)
		}

		private void SetStepsView(KeyValuePair<Edge.Core.Scheduling.Objects.SchedulingData, ServiceInstance> rowTag, DataGridViewRow row = null)
		{
			//cleanup
			string configurationName = rowTag.Value.ServiceName;
			if (row != null)
			{
				if (row.Tag != lastTag)
				{


					List<Control> controlsToRemove = new List<Control>();
					foreach (Control control in this.splitContainerSub.Panel1.Controls)
					{
						if (control.Name != "lblSteps")
							controlsToRemove.Add(control);
					}
					foreach (Control control in controlsToRemove)
						this.splitContainerSub.Panel1.Controls.Remove(control);



					if (row.State == (DataGridViewElementStates.Displayed | DataGridViewElementStates.Visible | DataGridViewElementStates.Selected))
					{
						if (_stepsByConfiguration.ContainsKey(configurationName))
						{
							DrawControls(configurationName);
						}
					}
				}
				else
				{
					if (row.State == (DataGridViewElementStates.Displayed | DataGridViewElementStates.Visible | DataGridViewElementStates.Selected))
					{
						if (_stepsByConfiguration.ContainsKey(configurationName))
						{
							foreach (var step in _stepsByConfiguration[configurationName])
							{
								if (splitContainerSub.Panel1.Controls.ContainsKey(string.Format("Progress{0}", step.step.ActualName)))
								{
									ProgressBar p = (ProgressBar)splitContainerSub.Panel1.Controls[string.Format("Progress{0}", step.step.ActualName)];
									if (p.Value != 100)
										p.Value = step.Value;
								}


							}

						}

					}

				}
				lastTag = row.Tag;
			}
		}

		private void DrawControls(string configurationName)
		{
			Control baseControl = this.splitContainerSub.Panel1.Controls["lblSteps"];
			foreach (StepProperties step in _stepsByConfiguration[configurationName])
			{
				Label lbl = new Label();
				lbl.Name = string.Format("lbl{0}", step.step.ActualName);
				lbl.Location = new Point(baseControl.Left, baseControl.Top + baseControl.Height + 5);
				lbl.Text = step.step.ActualName;
				lbl.Visible = true;
				this.splitContainerSub.Panel1.Controls.Add(lbl);

				ProgressBar progress = new ProgressBar();
				progress.Name = string.Format("Progress{0}", step.step.ActualName);
				progress.Minimum = 0;
				progress.Maximum = 100;

				progress.Location = new Point(lbl.Left + lbl.Width + 5, lbl.Top);
				progress.Width = 200;

				progress.Value = step.Value;
				Application.DoEvents();

				this.splitContainerSub.Panel1.Controls.Add(progress);
				progress.Visible = true;


				baseControl = lbl;
			}
		}







	}
	public class StepProperties
	{
		public WorkflowStepElement step;
		public int Value { get; private set; }
		public string ParentName;
		public event EventHandler ValueChanged;
		public void SetValue(double val)
		{
			Value = Convert.ToInt32(val * 100);
			ValueChanged(this, new EventArgs());
		}

	}
}
