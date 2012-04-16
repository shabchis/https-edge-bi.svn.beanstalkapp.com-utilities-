namespace Edge.Applications.PM.Client
{
	partial class Main
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
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripContainer1
			// 
			this.toolStripContainer1.BottomToolStripPanelVisible = false;
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add(this.menuStrip);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(967, 49);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.RightToolStripPanelVisible = false;
			this.toolStripContainer1.Size = new System.Drawing.Size(967, 49);
			this.toolStripContainer1.TabIndex = 0;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// menuStrip
			// 
			this.menuStrip.Dock = System.Windows.Forms.DockStyle.Fill;
			this.menuStrip.ImeMode = System.Windows.Forms.ImeMode.On;
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(967, 49);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip1";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(43, 45);
			this.toolStripMenuItem1.Text = "9999";
			this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.ClientSize = new System.Drawing.Size(967, 49);
			this.Controls.Add(this.toolStripContainer1);
			this.MainMenuStrip = this.menuStrip;
			this.Name = "Main";
			this.Text = "Form1";
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.ContentPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
	}
}

