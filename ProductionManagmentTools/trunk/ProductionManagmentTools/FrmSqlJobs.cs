using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using Edge.Application.ProductionManagmentTools.Objects;

namespace Edge.Application.ProductionManagmentTools
{
	public partial class FrmSqlJobs : Form
	{
		Server _server;
		List<SqlJob> _jobs;
		public FrmSqlJobs()
		{
			InitializeComponent();
			
			
			
		}

		

		

		

		

		private void FrmSqlJobs_Load(object sender, EventArgs e)
		{
			
		}

		private void LoadJobs()
		{
			foreach (Job job in _server.JobServer.Jobs)
			{
				if (job.IsEnabled)
				{
					_jobs.Add(new SqlJob(job));
				}

			}
			jobsGridView.DataSource = _jobs;
			Button b = new Button();
			b.Text = "Run";

			jobsGridView.Columns.Add(new DataGridViewButtonColumn(){UseColumnTextForButtonValue=true,Text="Run/Stop"});
			
			
			

		}

		private void btnConnectServer_Click(object sender, EventArgs e)
		{
			ConnectServer();
			LoadJobs();

		}
		private void ConnectServer()
		{
			_jobs = new List<SqlJob>();
			_server = new Server(@"console.edge-bi.com");
			_server.ConnectionContext.LoginSecure = false;
			_server.ConnectionContext.Login = "edge1";
			_server.ConnectionContext.Password = "Blublu*2!";
			try
			{
				_server.ConnectionContext.Connect();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void jobsGridView_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex ==6)
			{
				SqlJob j = (SqlJob)jobsGridView.Rows[e.RowIndex].DataBoundItem;
				j.Run();
				Refresh();
			}
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			RefreshJobs();
			jobsGridView.Refresh();


		}
		private void RefreshJobs()
		{
			if (_jobs != null)
			{
				foreach (var job in _jobs)
				{
					job.Refresh();
					
				}
			}
		}

	}
}
