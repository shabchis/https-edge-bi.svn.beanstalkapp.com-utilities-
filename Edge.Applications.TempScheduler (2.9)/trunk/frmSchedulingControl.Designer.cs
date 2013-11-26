namespace Edge.Applications.TempScheduler
{
	partial class frmSchedulingControl
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSchedulingControl));
			this.scheduleInfoGrid = new System.Windows.Forms.DataGridView();
			this.lblVer = new System.Windows.Forms.Label();
			this.splitContainerMain = new System.Windows.Forms.SplitContainer();
			this.logtextBox = new System.Windows.Forms.TextBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.btnSchedulerStart = new System.Windows.Forms.ToolStripButton();
			this.btnSchedulerStop = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.btnServicesUnplanned = new System.Windows.Forms.ToolStripButton();
			this.btnServicesAbort = new System.Windows.Forms.ToolStripButton();
			this.btnServicesRemove = new System.Windows.Forms.ToolStripButton();
			this.btnServicesReset = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
			this.btnToolsEncrypt = new System.Windows.Forms.ToolStripButton();
			this.accountID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.instanceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.serviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.timeEnded = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.scheduledID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.scheduledStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.scheduledEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.timePeriodStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.timePeriodEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.scheduleInfoGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
			this.splitContainerMain.Panel1.SuspendLayout();
			this.splitContainerMain.Panel2.SuspendLayout();
			this.splitContainerMain.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// scheduleInfoGrid
			// 
			this.scheduleInfoGrid.AllowUserToAddRows = false;
			this.scheduleInfoGrid.AllowUserToDeleteRows = false;
			this.scheduleInfoGrid.AllowUserToOrderColumns = true;
			this.scheduleInfoGrid.AllowUserToResizeRows = false;
			this.scheduleInfoGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.scheduleInfoGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.scheduleInfoGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.accountID,
            this.instanceID,
            this.serviceName,
            this.status,
            this.timeEnded,
            this.scheduledID,
            this.scheduledStart,
            this.scheduledEnd,
            this.timePeriodStart,
            this.timePeriodEnd});
			this.scheduleInfoGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scheduleInfoGrid.Location = new System.Drawing.Point(0, 0);
			this.scheduleInfoGrid.Name = "scheduleInfoGrid";
			this.scheduleInfoGrid.ReadOnly = true;
			this.scheduleInfoGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.scheduleInfoGrid.Size = new System.Drawing.Size(1263, 275);
			this.scheduleInfoGrid.TabIndex = 2;
			this.scheduleInfoGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scheduleInfoGrid_MouseDown);
			// 
			// lblVer
			// 
			this.lblVer.AutoSize = true;
			this.lblVer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblVer.ForeColor = System.Drawing.Color.Red;
			this.lblVer.Location = new System.Drawing.Point(0, 563);
			this.lblVer.Name = "lblVer";
			this.lblVer.Size = new System.Drawing.Size(138, 13);
			this.lblVer.TabIndex = 10;
			this.lblVer.Text = "V662-D12/05/2011-T07:48";
			// 
			// splitContainerMain
			// 
			this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerMain.Location = new System.Drawing.Point(0, 25);
			this.splitContainerMain.Name = "splitContainerMain";
			this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerMain.Panel1
			// 
			this.splitContainerMain.Panel1.Controls.Add(this.scheduleInfoGrid);
			// 
			// splitContainerMain.Panel2
			// 
			this.splitContainerMain.Panel2.Controls.Add(this.logtextBox);
			this.splitContainerMain.Size = new System.Drawing.Size(1263, 551);
			this.splitContainerMain.SplitterDistance = 275;
			this.splitContainerMain.TabIndex = 11;
			// 
			// logtextBox
			// 
			this.logtextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.logtextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logtextBox.Location = new System.Drawing.Point(0, 0);
			this.logtextBox.Multiline = true;
			this.logtextBox.Name = "logtextBox";
			this.logtextBox.ReadOnly = true;
			this.logtextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.logtextBox.Size = new System.Drawing.Size(1263, 272);
			this.logtextBox.TabIndex = 9;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.btnSchedulerStart,
            this.btnSchedulerStop,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.btnServicesUnplanned,
            this.btnServicesAbort,
            this.btnServicesRemove,
            this.btnServicesReset,
            this.toolStripSeparator2,
            this.toolStripLabel3,
            this.btnToolsEncrypt});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(1263, 25);
			this.toolStrip1.TabIndex = 11;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.ForeColor = System.Drawing.SystemColors.ControlDark;
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(62, 22);
			this.toolStripLabel1.Text = "Scheduler:";
			// 
			// btnSchedulerStart
			// 
			this.btnSchedulerStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnSchedulerStart.Image = ((System.Drawing.Image)(resources.GetObject("btnSchedulerStart.Image")));
			this.btnSchedulerStart.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSchedulerStart.Name = "btnSchedulerStart";
			this.btnSchedulerStart.Size = new System.Drawing.Size(35, 22);
			this.btnSchedulerStart.Text = "Start";
			this.btnSchedulerStart.Click += new System.EventHandler(this.btnSchedulerStart_Click);
			// 
			// btnSchedulerStop
			// 
			this.btnSchedulerStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnSchedulerStop.Image = ((System.Drawing.Image)(resources.GetObject("btnSchedulerStop.Image")));
			this.btnSchedulerStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSchedulerStop.Name = "btnSchedulerStop";
			this.btnSchedulerStop.Size = new System.Drawing.Size(35, 22);
			this.btnSchedulerStop.Text = "Stop";
			this.btnSchedulerStop.Click += new System.EventHandler(this.btnSchedulerStop_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.ForeColor = System.Drawing.SystemColors.ControlDark;
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(52, 22);
			this.toolStripLabel2.Text = "Services:";
			// 
			// btnServicesUnplanned
			// 
			this.btnServicesUnplanned.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnServicesUnplanned.Image = ((System.Drawing.Image)(resources.GetObject("btnServicesUnplanned.Image")));
			this.btnServicesUnplanned.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnServicesUnplanned.Name = "btnServicesUnplanned";
			this.btnServicesUnplanned.Size = new System.Drawing.Size(42, 22);
			this.btnServicesUnplanned.Text = "Add...";
			this.btnServicesUnplanned.Click += new System.EventHandler(this.btnServicesUnplanned_Click);
			// 
			// btnServicesAbort
			// 
			this.btnServicesAbort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnServicesAbort.Image = ((System.Drawing.Image)(resources.GetObject("btnServicesAbort.Image")));
			this.btnServicesAbort.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnServicesAbort.Name = "btnServicesAbort";
			this.btnServicesAbort.Size = new System.Drawing.Size(41, 22);
			this.btnServicesAbort.Text = "Abort";
			this.btnServicesAbort.Click += new System.EventHandler(this.btnServicesAbort_Click);
			// 
			// btnServicesRemove
			// 
			this.btnServicesRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnServicesRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnServicesRemove.Image")));
			this.btnServicesRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnServicesRemove.Name = "btnServicesRemove";
			this.btnServicesRemove.Size = new System.Drawing.Size(54, 22);
			this.btnServicesRemove.Text = "Remove";
			this.btnServicesRemove.Click += new System.EventHandler(this.btnServicesRemove_Click);
			// 
			// btnServicesReset
			// 
			this.btnServicesReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnServicesReset.Image = ((System.Drawing.Image)(resources.GetObject("btnServicesReset.Image")));
			this.btnServicesReset.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnServicesReset.Name = "btnServicesReset";
			this.btnServicesReset.Size = new System.Drawing.Size(39, 22);
			this.btnServicesReset.Text = "Reset";
			this.btnServicesReset.Click += new System.EventHandler(this.btnServicesReset_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel3
			// 
			this.toolStripLabel3.ForeColor = System.Drawing.SystemColors.ControlDark;
			this.toolStripLabel3.Name = "toolStripLabel3";
			this.toolStripLabel3.Size = new System.Drawing.Size(39, 22);
			this.toolStripLabel3.Text = "Tools:";
			// 
			// btnToolsEncrypt
			// 
			this.btnToolsEncrypt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnToolsEncrypt.Image = ((System.Drawing.Image)(resources.GetObject("btnToolsEncrypt.Image")));
			this.btnToolsEncrypt.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnToolsEncrypt.Name = "btnToolsEncrypt";
			this.btnToolsEncrypt.Size = new System.Drawing.Size(51, 22);
			this.btnToolsEncrypt.Text = "Encrypt";
			this.btnToolsEncrypt.Click += new System.EventHandler(this.btnToolsEncrypt_Click);
			// 
			// accountID
			// 
			this.accountID.HeaderText = "Account ID";
			this.accountID.Name = "accountID";
			this.accountID.ReadOnly = true;
			this.accountID.Width = 86;
			// 
			// instanceID
			// 
			this.instanceID.HeaderText = "Instance ID";
			this.instanceID.Name = "instanceID";
			this.instanceID.ReadOnly = true;
			this.instanceID.Width = 87;
			// 
			// serviceName
			// 
			this.serviceName.HeaderText = "Name";
			this.serviceName.Name = "serviceName";
			this.serviceName.ReadOnly = true;
			this.serviceName.Width = 60;
			// 
			// status
			// 
			this.status.HeaderText = "Status";
			this.status.Name = "status";
			this.status.ReadOnly = true;
			this.status.Width = 62;
			// 
			// timeEnded
			// 
			this.timeEnded.HeaderText = "Time Ended";
			this.timeEnded.Name = "timeEnded";
			this.timeEnded.ReadOnly = true;
			this.timeEnded.Width = 89;
			// 
			// scheduledID
			// 
			this.scheduledID.HeaderText = "Scheduled ID";
			this.scheduledID.Name = "scheduledID";
			this.scheduledID.ReadOnly = true;
			this.scheduledID.Visible = false;
			this.scheduledID.Width = 97;
			// 
			// scheduledStart
			// 
			this.scheduledStart.HeaderText = "Scheduled Start";
			this.scheduledStart.Name = "scheduledStart";
			this.scheduledStart.ReadOnly = true;
			this.scheduledStart.Width = 108;
			// 
			// scheduledEnd
			// 
			this.scheduledEnd.HeaderText = "Scheduled End";
			this.scheduledEnd.Name = "scheduledEnd";
			this.scheduledEnd.ReadOnly = true;
			this.scheduledEnd.Width = 105;
			// 
			// timePeriodStart
			// 
			this.timePeriodStart.HeaderText = "Time Period Start";
			this.timePeriodStart.Name = "timePeriodStart";
			this.timePeriodStart.ReadOnly = true;
			this.timePeriodStart.Width = 113;
			// 
			// timePeriodEnd
			// 
			this.timePeriodEnd.HeaderText = "Time Period End";
			this.timePeriodEnd.Name = "timePeriodEnd";
			this.timePeriodEnd.ReadOnly = true;
			this.timePeriodEnd.Width = 110;
			// 
			// frmSchedulingControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1263, 576);
			this.Controls.Add(this.lblVer);
			this.Controls.Add(this.splitContainerMain);
			this.Controls.Add(this.toolStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmSchedulingControl";
			this.Text = "Edge Temp Scheduler";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSchedulingControl_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.scheduleInfoGrid)).EndInit();
			this.splitContainerMain.Panel1.ResumeLayout(false);
			this.splitContainerMain.Panel2.ResumeLayout(false);
			this.splitContainerMain.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
			this.splitContainerMain.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView scheduleInfoGrid;
        private System.Windows.Forms.Label lblVer;
		private System.Windows.Forms.SplitContainer splitContainerMain;
		private System.Windows.Forms.TextBox logtextBox;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripButton btnSchedulerStart;
		private System.Windows.Forms.ToolStripButton btnSchedulerStop;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
		private System.Windows.Forms.ToolStripButton btnServicesAbort;
		private System.Windows.Forms.ToolStripButton btnServicesRemove;
		private System.Windows.Forms.ToolStripButton btnServicesUnplanned;
		private System.Windows.Forms.ToolStripButton btnServicesReset;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripLabel toolStripLabel3;
		private System.Windows.Forms.ToolStripButton btnToolsEncrypt;
		private System.Windows.Forms.DataGridViewTextBoxColumn accountID;
		private System.Windows.Forms.DataGridViewTextBoxColumn instanceID;
		private System.Windows.Forms.DataGridViewTextBoxColumn serviceName;
		private System.Windows.Forms.DataGridViewTextBoxColumn status;
		private System.Windows.Forms.DataGridViewTextBoxColumn timeEnded;
		private System.Windows.Forms.DataGridViewTextBoxColumn scheduledID;
		private System.Windows.Forms.DataGridViewTextBoxColumn scheduledStart;
		private System.Windows.Forms.DataGridViewTextBoxColumn scheduledEnd;
		private System.Windows.Forms.DataGridViewTextBoxColumn timePeriodStart;
		private System.Windows.Forms.DataGridViewTextBoxColumn timePeriodEnd;
	}
}

