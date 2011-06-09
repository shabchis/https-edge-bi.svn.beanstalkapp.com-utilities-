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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
			this.Auth = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.email = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.log = new System.Windows.Forms.RichTextBox();
			this.path = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.reportId = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.KwdID = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.srch = new System.Windows.Forms.Button();
			this.KwdEmail = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.Auth.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.SuspendLayout();
			// 
			// GetFields_btn
			// 
			this.GetFields_btn.Location = new System.Drawing.Point(479, 6);
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
			this.comboBox1.Location = new System.Drawing.Point(26, 6);
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
			this.dataGridView1.Location = new System.Drawing.Point(6, 35);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(1010, 538);
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
			this.button1.Location = new System.Drawing.Point(320, 56);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(139, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "Get Google Params";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// AuthToken
			// 
			this.AuthToken.Location = new System.Drawing.Point(610, 35);
			this.AuthToken.Name = "AuthToken";
			this.AuthToken.Size = new System.Drawing.Size(398, 20);
			this.AuthToken.TabIndex = 5;
			// 
			// DeveloperToken
			// 
			this.DeveloperToken.Location = new System.Drawing.Point(610, 63);
			this.DeveloperToken.Name = "DeveloperToken";
			this.DeveloperToken.Size = new System.Drawing.Size(398, 20);
			this.DeveloperToken.TabIndex = 6;
			// 
			// ApplicationToken
			// 
			this.ApplicationToken.Location = new System.Drawing.Point(610, 89);
			this.ApplicationToken.Name = "ApplicationToken";
			this.ApplicationToken.Size = new System.Drawing.Size(398, 20);
			this.ApplicationToken.TabIndex = 7;
			// 
			// lblAuthToken
			// 
			this.lblAuthToken.AutoSize = true;
			this.lblAuthToken.ForeColor = System.Drawing.Color.DarkRed;
			this.lblAuthToken.Location = new System.Drawing.Point(495, 38);
			this.lblAuthToken.Name = "lblAuthToken";
			this.lblAuthToken.Size = new System.Drawing.Size(60, 13);
			this.lblAuthToken.TabIndex = 8;
			this.lblAuthToken.Text = "AuthToken";
			// 
			// lblDeveloperToken
			// 
			this.lblDeveloperToken.AutoSize = true;
			this.lblDeveloperToken.Location = new System.Drawing.Point(495, 66);
			this.lblDeveloperToken.Name = "lblDeveloperToken";
			this.lblDeveloperToken.Size = new System.Drawing.Size(87, 13);
			this.lblDeveloperToken.TabIndex = 9;
			this.lblDeveloperToken.Text = "DeveloperToken";
			// 
			// lblApplicationToken
			// 
			this.lblApplicationToken.AutoSize = true;
			this.lblApplicationToken.Cursor = System.Windows.Forms.Cursors.AppStarting;
			this.lblApplicationToken.Location = new System.Drawing.Point(495, 96);
			this.lblApplicationToken.Name = "lblApplicationToken";
			this.lblApplicationToken.Size = new System.Drawing.Size(90, 13);
			this.lblApplicationToken.TabIndex = 10;
			this.lblApplicationToken.Text = "ApplicationToken";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(19, 73);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 13);
			this.label1.TabIndex = 12;
			this.label1.Text = "MCC Password";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(19, 45);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 11;
			this.label2.Text = "MCC Email";
			// 
			// MccEmail
			// 
			this.MccEmail.Location = new System.Drawing.Point(121, 37);
			this.MccEmail.Name = "MccEmail";
			this.MccEmail.Size = new System.Drawing.Size(170, 20);
			this.MccEmail.TabIndex = 13;
			this.MccEmail.Text = "edge.bi.mcc@gmail.com";
			// 
			// MccPassword
			// 
			this.MccPassword.Location = new System.Drawing.Point(121, 70);
			this.MccPassword.Name = "MccPassword";
			this.MccPassword.PasswordChar = '*';
			this.MccPassword.Size = new System.Drawing.Size(170, 20);
			this.MccPassword.TabIndex = 14;
			this.MccPassword.Text = "edgebinewfish";
			// 
			// Auth
			// 
			this.Auth.AccessibleName = "AuthTab";
			this.Auth.Controls.Add(this.tabPage1);
			this.Auth.Controls.Add(this.tabPage2);
			this.Auth.Controls.Add(this.tabPage3);
			this.Auth.Controls.Add(this.tabPage4);
			this.Auth.Location = new System.Drawing.Point(12, 12);
			this.Auth.Name = "Auth";
			this.Auth.SelectedIndex = 0;
			this.Auth.Size = new System.Drawing.Size(1030, 605);
			this.Auth.TabIndex = 15;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.Gainsboro;
			this.tabPage1.Controls.Add(this.MccEmail);
			this.tabPage1.Controls.Add(this.MccPassword);
			this.tabPage1.Controls.Add(this.button1);
			this.tabPage1.Controls.Add(this.AuthToken);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.DeveloperToken);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.ApplicationToken);
			this.tabPage1.Controls.Add(this.lblApplicationToken);
			this.tabPage1.Controls.Add(this.lblAuthToken);
			this.tabPage1.Controls.Add(this.lblDeveloperToken);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1022, 579);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Auth";
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.Gainsboro;
			this.tabPage2.Controls.Add(this.comboBox1);
			this.tabPage2.Controls.Add(this.dataGridView1);
			this.tabPage2.Controls.Add(this.GetFields_btn);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(1022, 579);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Reports Fields";
			// 
			// tabPage3
			// 
			this.tabPage3.BackColor = System.Drawing.Color.Gainsboro;
			this.tabPage3.Controls.Add(this.email);
			this.tabPage3.Controls.Add(this.label6);
			this.tabPage3.Controls.Add(this.label5);
			this.tabPage3.Controls.Add(this.log);
			this.tabPage3.Controls.Add(this.path);
			this.tabPage3.Controls.Add(this.label4);
			this.tabPage3.Controls.Add(this.reportId);
			this.tabPage3.Controls.Add(this.button2);
			this.tabPage3.Controls.Add(this.label3);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(1022, 579);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Report Download";
			// 
			// email
			// 
			this.email.Location = new System.Drawing.Point(182, 122);
			this.email.Name = "email";
			this.email.Size = new System.Drawing.Size(212, 20);
			this.email.TabIndex = 8;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(84, 125);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(81, 13);
			this.label6.TabIndex = 7;
			this.label6.Text = "Account Email :";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(87, 222);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(25, 13);
			this.label5.TabIndex = 6;
			this.label5.Text = "Log";
			// 
			// log
			// 
			this.log.Location = new System.Drawing.Point(87, 241);
			this.log.Name = "log";
			this.log.Size = new System.Drawing.Size(604, 288);
			this.log.TabIndex = 5;
			this.log.Text = "";
			// 
			// path
			// 
			this.path.Location = new System.Drawing.Point(182, 96);
			this.path.Name = "path";
			this.path.Size = new System.Drawing.Size(212, 20);
			this.path.TabIndex = 4;
			this.path.Text = "c:\\testingAdwords.zip";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(84, 99);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(92, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "File save location:";
			// 
			// reportId
			// 
			this.reportId.Location = new System.Drawing.Point(182, 70);
			this.reportId.Name = "reportId";
			this.reportId.Size = new System.Drawing.Size(110, 20);
			this.reportId.TabIndex = 2;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(182, 159);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Download";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(84, 73);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Report ID:";
			// 
			// tabPage4
			// 
			this.tabPage4.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tabPage4.Controls.Add(this.KwdEmail);
			this.tabPage4.Controls.Add(this.label8);
			this.tabPage4.Controls.Add(this.KwdID);
			this.tabPage4.Controls.Add(this.label7);
			this.tabPage4.Controls.Add(this.srch);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(1022, 579);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "KeyWord Search";
			// 
			// KwdID
			// 
			this.KwdID.Location = new System.Drawing.Point(114, 34);
			this.KwdID.Name = "KwdID";
			this.KwdID.Size = new System.Drawing.Size(140, 20);
			this.KwdID.TabIndex = 2;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(40, 37);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(68, 13);
			this.label7.TabIndex = 1;
			this.label7.Text = "KeyWord ID:";
			// 
			// srch
			// 
			this.srch.Location = new System.Drawing.Point(360, 48);
			this.srch.Name = "srch";
			this.srch.Size = new System.Drawing.Size(93, 32);
			this.srch.TabIndex = 0;
			this.srch.Text = "Search";
			this.srch.UseVisualStyleBackColor = true;
			this.srch.Click += new System.EventHandler(this.srch_Click);
			// 
			// KwdEmail
			// 
			this.KwdEmail.Location = new System.Drawing.Point(114, 60);
			this.KwdEmail.Name = "KwdEmail";
			this.KwdEmail.Size = new System.Drawing.Size(140, 20);
			this.KwdEmail.TabIndex = 5;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(40, 63);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(35, 13);
			this.label8.TabIndex = 4;
			this.label8.Text = "Email:";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1051, 620);
			this.Controls.Add(this.Auth);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "Google Api Utility";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.Auth.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.tabPage4.ResumeLayout(false);
			this.tabPage4.PerformLayout();
			this.ResumeLayout(false);

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
		private System.Windows.Forms.TabControl Auth;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.RichTextBox log;
		private System.Windows.Forms.TextBox path;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox reportId;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox email;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TextBox KwdID;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button srch;
		private System.Windows.Forms.TextBox KwdEmail;
		private System.Windows.Forms.Label label8;
	}
}