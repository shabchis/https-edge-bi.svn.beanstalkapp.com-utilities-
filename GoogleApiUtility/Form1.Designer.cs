namespace APITester
{
	partial class Form1
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
			this.GetFields_btn = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Selected = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CanFilter = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.button1 = new System.Windows.Forms.Button();
			this.AuthToken = new System.Windows.Forms.TextBox();
			this.DeveloperToken = new System.Windows.Forms.TextBox();
			this.ApplicationToken = new System.Windows.Forms.TextBox();
			this.lblAuthToken = new System.Windows.Forms.Label();
			this.lblDeveloperToken = new System.Windows.Forms.Label();
			this.lblApplicationToken = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.MccEmail = new System.Windows.Forms.TextBox();
			this.MccPassword = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// GetFields_btn
			// 
			this.GetFields_btn.Location = new System.Drawing.Point(465, 12);
			this.GetFields_btn.Name = "GetFields_btn";
			this.GetFields_btn.Size = new System.Drawing.Size(118, 23);
			this.GetFields_btn.TabIndex = 0;
			this.GetFields_btn.Text = "Get Available fields";
			this.GetFields_btn.UseVisualStyleBackColor = true;
			this.GetFields_btn.Click += new System.EventHandler(this.GetFields_btn_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(12, 12);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(361, 21);
			this.comboBox1.TabIndex = 1;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Name,
            this.Type,
            this.Selected,
            this.CanFilter,
            this.DisplayName});
			this.dataGridView1.Location = new System.Drawing.Point(12, 39);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(989, 431);
			this.dataGridView1.TabIndex = 3;
			// 
			// Name
			// 
			this.Name.HeaderText = "Name";
			this.Name.Name = "Name";
			this.Name.Width = 200;
			// 
			// Type
			// 
			this.Type.HeaderText = "Type";
			this.Type.Name = "Type";
			// 
			// Selected
			// 
			this.Selected.HeaderText = "Selected";
			this.Selected.Name = "Selected";
			// 
			// CanFilter
			// 
			this.CanFilter.HeaderText = "Can Filter";
			this.CanFilter.Name = "CanFilter";
			// 
			// DisplayName
			// 
			this.DisplayName.HeaderText = "Display Name";
			this.DisplayName.Name = "DisplayName";
			this.DisplayName.Width = 220;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(313, 536);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(139, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "Get Google Params";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// AuthToken
			// 
			this.AuthToken.Location = new System.Drawing.Point(603, 515);
			this.AuthToken.Name = "AuthToken";
			this.AuthToken.Size = new System.Drawing.Size(398, 20);
			this.AuthToken.TabIndex = 5;
			// 
			// DeveloperToken
			// 
			this.DeveloperToken.Location = new System.Drawing.Point(603, 543);
			this.DeveloperToken.Name = "DeveloperToken";
			this.DeveloperToken.Size = new System.Drawing.Size(398, 20);
			this.DeveloperToken.TabIndex = 6;
			// 
			// ApplicationToken
			// 
			this.ApplicationToken.Location = new System.Drawing.Point(603, 569);
			this.ApplicationToken.Name = "ApplicationToken";
			this.ApplicationToken.Size = new System.Drawing.Size(398, 20);
			this.ApplicationToken.TabIndex = 7;
			// 
			// lblAuthToken
			// 
			this.lblAuthToken.AutoSize = true;
			this.lblAuthToken.ForeColor = System.Drawing.Color.DarkRed;
			this.lblAuthToken.Location = new System.Drawing.Point(488, 518);
			this.lblAuthToken.Name = "lblAuthToken";
			this.lblAuthToken.Size = new System.Drawing.Size(60, 13);
			this.lblAuthToken.TabIndex = 8;
			this.lblAuthToken.Text = "AuthToken";
			// 
			// lblDeveloperToken
			// 
			this.lblDeveloperToken.AutoSize = true;
			this.lblDeveloperToken.Location = new System.Drawing.Point(488, 546);
			this.lblDeveloperToken.Name = "lblDeveloperToken";
			this.lblDeveloperToken.Size = new System.Drawing.Size(87, 13);
			this.lblDeveloperToken.TabIndex = 9;
			this.lblDeveloperToken.Text = "DeveloperToken";
			// 
			// lblApplicationToken
			// 
			this.lblApplicationToken.AutoSize = true;
			this.lblApplicationToken.Cursor = System.Windows.Forms.Cursors.AppStarting;
			this.lblApplicationToken.Location = new System.Drawing.Point(488, 576);
			this.lblApplicationToken.Name = "lblApplicationToken";
			this.lblApplicationToken.Size = new System.Drawing.Size(90, 13);
			this.lblApplicationToken.TabIndex = 10;
			this.lblApplicationToken.Text = "ApplicationToken";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 553);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 13);
			this.label1.TabIndex = 12;
			this.label1.Text = "MCC Password";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 525);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 11;
			this.label2.Text = "MCC Email";
			// 
			// MccEmail
			// 
			this.MccEmail.Location = new System.Drawing.Point(114, 517);
			this.MccEmail.Name = "MccEmail";
			this.MccEmail.Size = new System.Drawing.Size(170, 20);
			this.MccEmail.TabIndex = 13;
			this.MccEmail.Text = "edge.bi.mcc@gmail.com";
			// 
			// MccPassword
			// 
			this.MccPassword.Location = new System.Drawing.Point(114, 550);
			this.MccPassword.Name = "MccPassword";
			this.MccPassword.PasswordChar = '*';
			this.MccPassword.Size = new System.Drawing.Size(170, 20);
			this.MccPassword.TabIndex = 14;
			this.MccPassword.Text = "edgebinewfish";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1068, 611);
			this.Controls.Add(this.MccPassword);
			this.Controls.Add(this.MccEmail);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblApplicationToken);
			this.Controls.Add(this.lblDeveloperToken);
			this.Controls.Add(this.lblAuthToken);
			this.Controls.Add(this.ApplicationToken);
			this.Controls.Add(this.DeveloperToken);
			this.Controls.Add(this.AuthToken);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.GetFields_btn);
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button GetFields_btn;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn Type;
		private System.Windows.Forms.DataGridViewTextBoxColumn Selected;
		private System.Windows.Forms.DataGridViewTextBoxColumn CanFilter;
		private System.Windows.Forms.DataGridViewTextBoxColumn DisplayName;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox AuthToken;
		private System.Windows.Forms.TextBox DeveloperToken;
		private System.Windows.Forms.TextBox ApplicationToken;
		private System.Windows.Forms.Label lblAuthToken;
		private System.Windows.Forms.Label lblDeveloperToken;
		private System.Windows.Forms.Label lblApplicationToken;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox MccEmail;
		private System.Windows.Forms.TextBox MccPassword;
	}
}