﻿namespace Edge.Application.ProductionManagmentTools
{
	partial class main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.dataChecksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastDayPerformanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicateTrackersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deliverySearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.googleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facebookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getAPISettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backofficeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getAccountMeasuersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createFileExampleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.passwordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Gainsboro;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataChecksToolStripMenuItem,
            this.googleToolStripMenuItem,
            this.facebookToolStripMenuItem,
            this.backofficeToolStripMenuItem,
            this.passwordToolStripMenuItem});
            this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1072, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // dataChecksToolStripMenuItem
            // 
            this.dataChecksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lastDayPerformanceToolStripMenuItem,
            this.runToolStripMenuItem,
            this.duplicateTrackersToolStripMenuItem,
            this.deliverySearchToolStripMenuItem});
            this.dataChecksToolStripMenuItem.Name = "dataChecksToolStripMenuItem";
            this.dataChecksToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.dataChecksToolStripMenuItem.Text = "Data Checks";
            // 
            // lastDayPerformanceToolStripMenuItem
            // 
            this.lastDayPerformanceToolStripMenuItem.Name = "lastDayPerformanceToolStripMenuItem";
            this.lastDayPerformanceToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.lastDayPerformanceToolStripMenuItem.Text = "Last Day Performance";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Image = global::Edge.Application.ProductionManagmentTools.Properties.Resources.database;
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.runToolStripMenuItem.Text = "DB Checks";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // duplicateTrackersToolStripMenuItem
            // 
            this.duplicateTrackersToolStripMenuItem.Image = global::Edge.Application.ProductionManagmentTools.Properties.Resources.DupTracker;
            this.duplicateTrackersToolStripMenuItem.Name = "duplicateTrackersToolStripMenuItem";
            this.duplicateTrackersToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.duplicateTrackersToolStripMenuItem.Text = "Duplicate Trackers";
            // 
            // deliverySearchToolStripMenuItem
            // 
            this.deliverySearchToolStripMenuItem.Name = "deliverySearchToolStripMenuItem";
            this.deliverySearchToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.deliverySearchToolStripMenuItem.Text = "Delivery Search";
            this.deliverySearchToolStripMenuItem.Click += new System.EventHandler(this.deliverySearchToolStripMenuItem_Click);
            // 
            // googleToolStripMenuItem
            // 
            this.googleToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.googleToolStripMenuItem.Name = "googleToolStripMenuItem";
            this.googleToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.googleToolStripMenuItem.Text = "Google";
            this.googleToolStripMenuItem.Click += new System.EventHandler(this.googleToolStripMenuItem_Click);
            // 
            // facebookToolStripMenuItem
            // 
            this.facebookToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getAPISettingsToolStripMenuItem});
            this.facebookToolStripMenuItem.Name = "facebookToolStripMenuItem";
            this.facebookToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.facebookToolStripMenuItem.Text = "Facebook";
            // 
            // getAPISettingsToolStripMenuItem
            // 
            this.getAPISettingsToolStripMenuItem.Name = "getAPISettingsToolStripMenuItem";
            this.getAPISettingsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.getAPISettingsToolStripMenuItem.Text = "Get API Settings";
            this.getAPISettingsToolStripMenuItem.Click += new System.EventHandler(this.getAPISettingsToolStripMenuItem_Click);
            // 
            // backofficeToolStripMenuItem
            // 
            this.backofficeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getAccountMeasuersToolStripMenuItem,
            this.createFileExampleToolStripMenuItem});
            this.backofficeToolStripMenuItem.Name = "backofficeToolStripMenuItem";
            this.backofficeToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.backofficeToolStripMenuItem.Text = "Backoffice";
            // 
            // getAccountMeasuersToolStripMenuItem
            // 
            this.getAccountMeasuersToolStripMenuItem.Name = "getAccountMeasuersToolStripMenuItem";
            this.getAccountMeasuersToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.getAccountMeasuersToolStripMenuItem.Text = "Get Account Measuers";
            this.getAccountMeasuersToolStripMenuItem.Click += new System.EventHandler(this.getAccountMeasuersToolStripMenuItem_Click);
            // 
            // createFileExampleToolStripMenuItem
            // 
            this.createFileExampleToolStripMenuItem.Name = "createFileExampleToolStripMenuItem";
            this.createFileExampleToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.createFileExampleToolStripMenuItem.Text = "Create File Example";
            // 
            // passwordToolStripMenuItem
            // 
            this.passwordToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decToolStripMenuItem});
            this.passwordToolStripMenuItem.Name = "passwordToolStripMenuItem";
            this.passwordToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.passwordToolStripMenuItem.Text = "Password";
            // 
            // decToolStripMenuItem
            // 
            this.decToolStripMenuItem.Name = "decToolStripMenuItem";
            this.decToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.decToolStripMenuItem.Text = "Enc \\ Dec";
            this.decToolStripMenuItem.Click += new System.EventHandler(this.decToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 549);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1072, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(1072, 571);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "main";
            this.Text = "Production managment tools";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ToolStripMenuItem googleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem facebookToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem getAPISettingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem backofficeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem getAccountMeasuersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFileExampleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dataChecksToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem duplicateTrackersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem passwordToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem decToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastDayPerformanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deliverySearchToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip;
	}
}



