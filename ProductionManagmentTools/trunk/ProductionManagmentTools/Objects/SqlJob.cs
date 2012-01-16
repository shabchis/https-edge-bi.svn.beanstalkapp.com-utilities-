using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Smo.Agent;
using System.Windows.Forms;

namespace Edge.Application.ProductionManagmentTools.Objects
{
	public class SqlJob
	{
		public SqlJob(Job job)
		{
			_job = job;
			Init();
			
			
			
		}

		private void Init()
		{
			Name = _job.Name;
			Status = _job.CurrentRunStatus.ToString();
			this.Step = string.Format("{0} Within {1}", _job.CurrentRunStep, _job.JobSteps.Count);
			LastRun = _job.LastRunDate;
			NextRun = _job.NextRunDate;
			LastRunOutCome = _job.LastRunOutcome.ToString();
		}
		private Job _job;
		public string Name { get; set; }
		public string Status { get; set; }
		public string Step { get; set; }
		public DateTime LastRun { get; set; }
		public DateTime NextRun { get; set; }
		public string LastRunOutCome { get; set; }





		internal void Run()
		{
			if (_job.CurrentRunStatus == JobExecutionStatus.Executing)
				_job.Stop();
			else
				_job.Start();
			_job.Refresh();
		}

		internal void Refresh()
		{
			_job.Refresh();
			Init();
		}
	}
}
