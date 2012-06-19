using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Edge.Data.Pipeline.Services;
using Edge.Core.Services;
using Edge.Core.Scheduling;
using Edge.Core;
using Edge.Data.Pipeline;

namespace Edge.Applications.PM.Suite.DataChecks
{
	public partial class ResultForm : Form
	{
		private List<ValidationResult> _repairList;
		public ResultForm()
		{
			_repairList = new List<ValidationResult>();
			InitializeComponent();
		}

		public ResultForm(ref DataGridViewRowCollection errorDataGridViewRows, ref DataGridViewRowCollection warningDataGridViewRows,ref DataGridViewRowCollection successDataGridViewRows)
		{
			_repairList = new List<ValidationResult>();
			InitializeComponent();

			this.errCountResult_lbl.Text = string.Format("Totals ( {0} )", errorDataGridViewRows.Count);
			this.warningsCountResult_lbl.Text = string.Format("Totals ( {0} )", warningDataGridViewRows.Count);
			this.sucessCountResult_lbl.Text = string.Format("Totals ( {0} )", successDataGridViewRows.Count);

			foreach (DataGridViewRow row in errorDataGridViewRows)
			{
				this.ErrorDataGridView.Rows.Add(row);
			}
			foreach (DataGridViewRow row in warningDataGridViewRows)
			{
				this.WarningDataGridView.Rows.Add(row);
			}
			foreach (DataGridViewRow row in successDataGridViewRows)
			{
				this.SuccessDataGridView.Rows.Add(row);
			}
		
		}
			

		
		private void button3_Click(object sender, EventArgs e)
		{
			
			foreach (DataGridViewRow row in this.ErrorDataGridView.Rows)
			{
				if ((bool)row.Cells["Repair"].Value)
				{
					_repairList.Add((ValidationResult)row.Tag);
				}
			}

			RunCommitServices(_repairList);

		}

		private void RunCommitServices(List<ValidationResult> repairList)
		{
			SettingsCollection _options = new SettingsCollection();

			foreach (ValidationResult itemToFix in repairList)
			{
				DateTimeRange _dateTimeRange = new DateTimeRange();
				string TargetPeriodStart = String.Empty;
				string TargetPeriodEnd = String.Empty;

				_dateTimeRange.Start = DateTimeSpecification.Parse(itemToFix.TargetPeriodStart.ToString());
				_dateTimeRange.End = DateTimeSpecification.Parse(itemToFix.TargetPeriodEnd.ToString());

				string serviceName = Const.AdMetricsConst.WorkflowServices.CommitServiceName;
				ServiceClient<IScheduleManager> scheduleManager = new ServiceClient<IScheduleManager>();

				_options.Add("ConflictBehavior", "Ignore");
				_options.Add("DeliveryID", itemToFix.DeliveryID.ToString());

				//Run Service
				_options.Add(PipelineService.ConfigurationOptionNames.TimePeriod, _dateTimeRange.ToAbsolute().ToString());
				
				//bool result = _listner.FormAddToSchedule(serviceName, -1, DateTime.Now, _options, ServicePriority.Normal);
				//if (!result)
				//{
				//    MessageBox.Show(string.Format("Service {0} for account {1} did not run", serviceName, -1));
				//}
			}
		}

	}


}
