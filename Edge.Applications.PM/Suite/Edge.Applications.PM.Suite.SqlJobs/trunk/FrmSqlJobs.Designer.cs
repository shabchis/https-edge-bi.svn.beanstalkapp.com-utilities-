namespace Edge.Applications.PM.Suite.SqlJobs
{
	partial class FrmSqlJobs
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.jobsGridView = new System.Windows.Forms.DataGridView();
			this.btnConnectServer = new System.Windows.Forms.Button();
			this.btnRefresh = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.jobsGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// jobsGridView
			// 
			this.jobsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.jobsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.jobsGridView.Location = new System.Drawing.Point(25, 66);
			this.jobsGridView.Name = "jobsGridView";
			this.jobsGridView.Size = new System.Drawing.Size(801, 323);
			this.jobsGridView.TabIndex = 0;
			this.jobsGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.jobsGridView_CellClick);
			// 
			// btnConnectServer
			// 
			this.btnConnectServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnConnectServer.Location = new System.Drawing.Point(701, 27);
			this.btnConnectServer.Name = "btnConnectServer";
			this.btnConnectServer.Size = new System.Drawing.Size(125, 23);
			this.btnConnectServer.TabIndex = 1;
			this.btnConnectServer.Text = "Connect Server";
			this.btnConnectServer.UseVisualStyleBackColor = true;
			this.btnConnectServer.Click += new System.EventHandler(this.btnConnectServer_Click);
			// 
			// btnRefresh
			// 
			this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh.Location = new System.Drawing.Point(612, 27);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(75, 23);
			this.btnRefresh.TabIndex = 2;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// FrmSqlJobs
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(838, 401);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.btnConnectServer);
			this.Controls.Add(this.jobsGridView);
			this.Name = "FrmSqlJobs";
			this.Text = "Sql Job Agent";			
			((System.ComponentModel.ISupportInitialize)(this.jobsGridView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView jobsGridView;
		private System.Windows.Forms.Button btnConnectServer;
		private System.Windows.Forms.Button btnRefresh;
	}
}