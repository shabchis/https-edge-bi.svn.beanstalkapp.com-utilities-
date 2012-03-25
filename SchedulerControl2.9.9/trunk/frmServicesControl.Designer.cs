namespace Edge_Applications.PM.common
{
	partial class frmServicesControl
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
			this.mainGridgroupBox = new System.Windows.Forms.GroupBox();
			this.mainGrid = new System.Windows.Forms.DataGridView();
			this.mainGridgroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mainGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// mainGridgroupBox
			// 
			this.mainGridgroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.mainGridgroupBox.Controls.Add(this.mainGrid);
			this.mainGridgroupBox.Location = new System.Drawing.Point(12, 13);
			this.mainGridgroupBox.Name = "mainGridgroupBox";
			this.mainGridgroupBox.Size = new System.Drawing.Size(1185, 297);
			this.mainGridgroupBox.TabIndex = 0;
			this.mainGridgroupBox.TabStop = false;
			this.mainGridgroupBox.Text = "groupBox1";
			// 
			// mainGrid
			// 
			this.mainGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.mainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainGrid.Location = new System.Drawing.Point(3, 16);
			this.mainGrid.Name = "mainGrid";
			this.mainGrid.Size = new System.Drawing.Size(1179, 278);
			this.mainGrid.TabIndex = 0;
			// 
			// frmServicesControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1209, 322);
			this.Controls.Add(this.mainGridgroupBox);
			this.Name = "frmServicesControl";
			this.Text = "frmServicesControl";
			this.mainGridgroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mainGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox mainGridgroupBox;
		private System.Windows.Forms.DataGridView mainGrid;
	}
}