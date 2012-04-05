namespace Edge.Applications.PM.Suite.DataChecks
{
	partial class DataChecksForm
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
			this.fromDate = new System.Windows.Forms.DateTimePicker();
			this.toDate = new System.Windows.Forms.DateTimePicker();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.step1 = new System.Windows.Forms.Panel();
			this.step1_ErrorImage = new System.Windows.Forms.PictureBox();
			this.step1_status = new System.Windows.Forms.Label();
			this.step1_progressBar = new System.Windows.Forms.ProgressBar();
			this.step1_warningCount = new System.Windows.Forms.Label();
			this.step1_errorsCount = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.AccountsCheckedListBox = new System.Windows.Forms.CheckedListBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.clear = new System.Windows.Forms.Button();
			this.panel6 = new System.Windows.Forms.Panel();
			this.pictureBox12 = new System.Windows.Forms.PictureBox();
			this.label12 = new System.Windows.Forms.Label();
			this.pictureBox9 = new System.Windows.Forms.PictureBox();
			this.label11 = new System.Windows.Forms.Label();
			this.pictureBox8 = new System.Windows.Forms.PictureBox();
			this.application_cb = new System.Windows.Forms.ComboBox();
			this.Profile_lbl = new System.Windows.Forms.Label();
			this.profile_cb = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.report_btn = new System.Windows.Forms.Button();
			this.checkAll = new System.Windows.Forms.CheckBox();
			this.label10 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.pictureBox11 = new System.Windows.Forms.PictureBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.pictureBox13 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.Start_btn = new System.Windows.Forms.Button();
			this.panel5 = new System.Windows.Forms.Panel();
			this.pictureBox10 = new System.Windows.Forms.PictureBox();
			this.label6 = new System.Windows.Forms.Label();
			this.rightSidePanel = new System.Windows.Forms.Panel();
			this.MerticsValidations = new System.Windows.Forms.TreeView();
			this.ValidationTypes = new System.Windows.Forms.TreeView();
			this.label13 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.LogBox = new System.Windows.Forms.RichTextBox();
			this.groupBox3.SuspendLayout();
			this.step1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.step1_ErrorImage)).BeginInit();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
			this.panel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).BeginInit();
			this.panel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
			this.rightSidePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// fromDate
			// 
			this.fromDate.Font = new System.Drawing.Font("Microsoft JhengHei", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.fromDate.Location = new System.Drawing.Point(65, 46);
			this.fromDate.Name = "fromDate";
			this.fromDate.Size = new System.Drawing.Size(141, 20);
			this.fromDate.TabIndex = 15;
			this.fromDate.Value = new System.DateTime(2011, 9, 27, 0, 0, 0, 0);
			// 
			// toDate
			// 
			this.toDate.Font = new System.Drawing.Font("Microsoft JhengHei", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.toDate.Location = new System.Drawing.Point(65, 67);
			this.toDate.Name = "toDate";
			this.toDate.Size = new System.Drawing.Size(141, 20);
			this.toDate.TabIndex = 13;
			this.toDate.Value = new System.DateTime(2011, 9, 27, 0, 0, 0, 0);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft JhengHei", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label5.ForeColor = System.Drawing.Color.Black;
			this.label5.Location = new System.Drawing.Point(26, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(19, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "To";
			// 
			// groupBox3
			// 
			this.groupBox3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.groupBox3.Controls.Add(this.step1);
			this.groupBox3.Controls.Add(this.label17);
			this.groupBox3.Controls.Add(this.panel1);
			this.groupBox3.Location = new System.Drawing.Point(11, 535);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(806, 94);
			this.groupBox3.TabIndex = 18;
			this.groupBox3.TabStop = false;
			// 
			// step1
			// 
			this.step1.Controls.Add(this.step1_ErrorImage);
			this.step1.Controls.Add(this.step1_status);
			this.step1.Controls.Add(this.step1_progressBar);
			this.step1.Controls.Add(this.step1_warningCount);
			this.step1.Controls.Add(this.step1_errorsCount);
			this.step1.Enabled = false;
			this.step1.Location = new System.Drawing.Point(13, 42);
			this.step1.Name = "step1";
			this.step1.Size = new System.Drawing.Size(787, 39);
			this.step1.TabIndex = 49;
			// 
			// step1_ErrorImage
			// 
			this.step1_ErrorImage.ErrorImage = global::Edge.Applications.PM.Suite.DataChecks.Properties.Resources.failed_icon;
			this.step1_ErrorImage.Location = new System.Drawing.Point(661, 7);
			this.step1_ErrorImage.Name = "step1_ErrorImage";
			this.step1_ErrorImage.Size = new System.Drawing.Size(27, 26);
			this.step1_ErrorImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.step1_ErrorImage.TabIndex = 20;
			this.step1_ErrorImage.TabStop = false;
			this.step1_ErrorImage.Visible = false;
			// 
			// step1_status
			// 
			this.step1_status.AutoSize = true;
			this.step1_status.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.step1_status.Location = new System.Drawing.Point(516, 13);
			this.step1_status.Name = "step1_status";
			this.step1_status.Size = new System.Drawing.Size(40, 16);
			this.step1_status.TabIndex = 28;
			this.step1_status.Text = "--------";
			this.step1_status.Visible = false;
			// 
			// step1_progressBar
			// 
			this.step1_progressBar.Location = new System.Drawing.Point(19, 13);
			this.step1_progressBar.MarqueeAnimationSpeed = 10;
			this.step1_progressBar.Name = "step1_progressBar";
			this.step1_progressBar.Size = new System.Drawing.Size(332, 19);
			this.step1_progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.step1_progressBar.TabIndex = 19;
			// 
			// step1_warningCount
			// 
			this.step1_warningCount.AutoSize = true;
			this.step1_warningCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.step1_warningCount.ForeColor = System.Drawing.Color.Chocolate;
			this.step1_warningCount.Location = new System.Drawing.Point(712, 5);
			this.step1_warningCount.Name = "step1_warningCount";
			this.step1_warningCount.Size = new System.Drawing.Size(43, 12);
			this.step1_warningCount.TabIndex = 38;
			this.step1_warningCount.Text = "warnings";
			this.step1_warningCount.Visible = false;
			// 
			// step1_errorsCount
			// 
			this.step1_errorsCount.AutoSize = true;
			this.step1_errorsCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.step1_errorsCount.ForeColor = System.Drawing.Color.DarkRed;
			this.step1_errorsCount.Location = new System.Drawing.Point(712, 17);
			this.step1_errorsCount.Name = "step1_errorsCount";
			this.step1_errorsCount.Size = new System.Drawing.Size(31, 12);
			this.step1_errorsCount.TabIndex = 39;
			this.step1_errorsCount.Text = "errors ";
			this.step1_errorsCount.Visible = false;
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label17.Location = new System.Drawing.Point(327, 42);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(0, 16);
			this.label17.TabIndex = 32;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Gainsboro;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Location = new System.Drawing.Point(6, 9);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(794, 28);
			this.panel1.TabIndex = 19;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label2.ForeColor = System.Drawing.Color.Black;
			this.label2.Location = new System.Drawing.Point(22, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 15);
			this.label2.TabIndex = 21;
			this.label2.Text = "Progress";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label8.ForeColor = System.Drawing.Color.Black;
			this.label8.Location = new System.Drawing.Point(660, 6);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(48, 15);
			this.label8.TabIndex = 19;
			this.label8.Text = "Result";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label7.ForeColor = System.Drawing.Color.Black;
			this.label7.Location = new System.Drawing.Point(515, 6);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(47, 15);
			this.label7.TabIndex = 18;
			this.label7.Text = "Status";
			// 
			// AccountsCheckedListBox
			// 
			this.AccountsCheckedListBox.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.AccountsCheckedListBox.CheckOnClick = true;
			this.AccountsCheckedListBox.ColumnWidth = 180;
			this.AccountsCheckedListBox.Font = new System.Drawing.Font("Microsoft JhengHei", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.AccountsCheckedListBox.FormattingEnabled = true;
			this.AccountsCheckedListBox.HorizontalScrollbar = true;
			this.AccountsCheckedListBox.Location = new System.Drawing.Point(11, 191);
			this.AccountsCheckedListBox.MultiColumn = true;
			this.AccountsCheckedListBox.Name = "AccountsCheckedListBox";
			this.AccountsCheckedListBox.Size = new System.Drawing.Size(279, 289);
			this.AccountsCheckedListBox.Sorted = true;
			this.AccountsCheckedListBox.TabIndex = 20;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.panel2.Controls.Add(this.clear);
			this.panel2.Controls.Add(this.panel6);
			this.panel2.Controls.Add(this.pictureBox9);
			this.panel2.Controls.Add(this.label11);
			this.panel2.Controls.Add(this.pictureBox8);
			this.panel2.Controls.Add(this.application_cb);
			this.panel2.Controls.Add(this.Profile_lbl);
			this.panel2.Controls.Add(this.profile_cb);
			this.panel2.Controls.Add(this.label4);
			this.panel2.Location = new System.Drawing.Point(6, 12);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(811, 98);
			this.panel2.TabIndex = 22;
			// 
			// clear
			// 
			this.clear.Location = new System.Drawing.Point(767, 59);
			this.clear.Name = "clear";
			this.clear.Size = new System.Drawing.Size(44, 23);
			this.clear.TabIndex = 61;
			this.clear.Text = "Clear";
			this.clear.UseVisualStyleBackColor = true;
			// 
			// panel6
			// 
			this.panel6.BackColor = System.Drawing.Color.Tan;
			this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel6.Controls.Add(this.pictureBox12);
			this.panel6.Controls.Add(this.label12);
			this.panel6.Location = new System.Drawing.Point(3, 3);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(802, 26);
			this.panel6.TabIndex = 61;
			// 
			// pictureBox12
			// 
			this.pictureBox12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox12.Image = global::Edge.Applications.PM.Suite.DataChecks.Properties.Resources.preferences_system_windows_actions;
			this.pictureBox12.Location = new System.Drawing.Point(-1, -1);
			this.pictureBox12.Name = "pictureBox12";
			this.pictureBox12.Size = new System.Drawing.Size(33, 26);
			this.pictureBox12.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox12.TabIndex = 60;
			this.pictureBox12.TabStop = false;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Bold);
			this.label12.Location = new System.Drawing.Point(35, 6);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(95, 17);
			this.label12.TabIndex = 58;
			this.label12.Text = "Configuration";
			// 
			// pictureBox9
			// 
			this.pictureBox9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox9.Image = global::Edge.Applications.PM.Suite.DataChecks.Properties.Resources.configuration;
			this.pictureBox9.Location = new System.Drawing.Point(22, 69);
			this.pictureBox9.Name = "pictureBox9";
			this.pictureBox9.Size = new System.Drawing.Size(14, 16);
			this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox9.TabIndex = 65;
			this.pictureBox9.TabStop = false;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Microsoft JhengHei", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.ForeColor = System.Drawing.Color.Gainsboro;
			this.label11.Location = new System.Drawing.Point(685, 85);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(120, 13);
			this.label11.TabIndex = 64;
			this.label11.Text = "Data Validation Tool [v 4.0]";
			// 
			// pictureBox8
			// 
			this.pictureBox8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox8.Image = global::Edge.Applications.PM.Suite.DataChecks.Properties.Resources._24;
			this.pictureBox8.Location = new System.Drawing.Point(22, 41);
			this.pictureBox8.Name = "pictureBox8";
			this.pictureBox8.Size = new System.Drawing.Size(14, 16);
			this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox8.TabIndex = 53;
			this.pictureBox8.TabStop = false;
			// 
			// application_cb
			// 
			this.application_cb.FormattingEnabled = true;
			this.application_cb.Items.AddRange(new object[] {
            "Seperia",
            "Edge.BI"});
			this.application_cb.Location = new System.Drawing.Point(113, 40);
			this.application_cb.Name = "application_cb";
			this.application_cb.Size = new System.Drawing.Size(152, 21);
			this.application_cb.TabIndex = 63;
			this.application_cb.Text = "Select Application";
			this.application_cb.SelectedValueChanged += new System.EventHandler(this.application_cb_SelectedValueChanged);
			// 
			// Profile_lbl
			// 
			this.Profile_lbl.AutoSize = true;
			this.Profile_lbl.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Profile_lbl.ForeColor = System.Drawing.Color.White;
			this.Profile_lbl.Location = new System.Drawing.Point(42, 70);
			this.Profile_lbl.Name = "Profile_lbl";
			this.Profile_lbl.Size = new System.Drawing.Size(41, 15);
			this.Profile_lbl.TabIndex = 39;
			this.Profile_lbl.Text = "Profile";
			// 
			// profile_cb
			// 
			this.profile_cb.FormattingEnabled = true;
			this.profile_cb.Items.AddRange(new object[] {
            "Custom"});
			this.profile_cb.Location = new System.Drawing.Point(113, 67);
			this.profile_cb.Name = "profile_cb";
			this.profile_cb.Size = new System.Drawing.Size(152, 21);
			this.profile_cb.TabIndex = 57;
			this.profile_cb.Text = "Custom";
			this.profile_cb.SelectedValueChanged += new System.EventHandler(this.profile_cb_SelectedValueChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(42, 42);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(68, 15);
			this.label4.TabIndex = 61;
			this.label4.Text = "Application";
			// 
			// report_btn
			// 
			this.report_btn.Enabled = false;
			this.report_btn.Location = new System.Drawing.Point(730, 491);
			this.report_btn.Name = "report_btn";
			this.report_btn.Size = new System.Drawing.Size(87, 31);
			this.report_btn.TabIndex = 49;
			this.report_btn.Text = "View Results";
			this.report_btn.UseVisualStyleBackColor = true;
			// 
			// checkAll
			// 
			this.checkAll.AutoSize = true;
			this.checkAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.checkAll.Location = new System.Drawing.Point(15, 169);
			this.checkAll.Name = "checkAll";
			this.checkAll.Size = new System.Drawing.Size(66, 16);
			this.checkAll.TabIndex = 55;
			this.checkAll.Text = " Check  all";
			this.checkAll.UseVisualStyleBackColor = true;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Bold);
			this.label10.Location = new System.Drawing.Point(35, 5);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(66, 17);
			this.label10.TabIndex = 58;
			this.label10.Text = "Accounts";
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.Silver;
			this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel3.Controls.Add(this.pictureBox11);
			this.panel3.Controls.Add(this.label10);
			this.panel3.Location = new System.Drawing.Point(11, 135);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(279, 28);
			this.panel3.TabIndex = 23;
			// 
			// pictureBox11
			// 
			this.pictureBox11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox11.Image = global::Edge.Applications.PM.Suite.DataChecks.Properties.Resources.users;
			this.pictureBox11.Location = new System.Drawing.Point(-2, -2);
			this.pictureBox11.Name = "pictureBox11";
			this.pictureBox11.Size = new System.Drawing.Size(0, 29);
			this.pictureBox11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox11.TabIndex = 53;
			this.pictureBox11.TabStop = false;
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.Color.Silver;
			this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel4.Controls.Add(this.pictureBox13);
			this.panel4.Controls.Add(this.label1);
			this.panel4.Location = new System.Drawing.Point(12, 105);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(458, 28);
			this.panel4.TabIndex = 59;
			// 
			// pictureBox13
			// 
			this.pictureBox13.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox13.Image = global::Edge.Applications.PM.Suite.DataChecks.Properties.Resources.settings;
			this.pictureBox13.Location = new System.Drawing.Point(-1, -1);
			this.pictureBox13.Name = "pictureBox13";
			this.pictureBox13.Size = new System.Drawing.Size(30, 28);
			this.pictureBox13.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox13.TabIndex = 65;
			this.pictureBox13.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Bold);
			this.label1.Location = new System.Drawing.Point(40, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(59, 17);
			this.label1.TabIndex = 58;
			this.label1.Text = "Settings";
			// 
			// Start_btn
			// 
			this.Start_btn.BackColor = System.Drawing.Color.Green;
			this.Start_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.Start_btn.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Bold);
			this.Start_btn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.Start_btn.Location = new System.Drawing.Point(361, 488);
			this.Start_btn.Name = "Start_btn";
			this.Start_btn.Size = new System.Drawing.Size(99, 41);
			this.Start_btn.TabIndex = 47;
			this.Start_btn.Text = "START";
			this.Start_btn.UseVisualStyleBackColor = false;
			this.Start_btn.Click += new System.EventHandler(this.Start_btn_Click);
			// 
			// panel5
			// 
			this.panel5.BackColor = System.Drawing.Color.Silver;
			this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel5.Controls.Add(this.pictureBox10);
			this.panel5.Controls.Add(this.label6);
			this.panel5.Location = new System.Drawing.Point(12, 3);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(205, 26);
			this.panel5.TabIndex = 60;
			// 
			// pictureBox10
			// 
			this.pictureBox10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox10.Image = global::Edge.Applications.PM.Suite.DataChecks.Properties.Resources.kontact1;
			this.pictureBox10.Location = new System.Drawing.Point(-1, -1);
			this.pictureBox10.Name = "pictureBox10";
			this.pictureBox10.Size = new System.Drawing.Size(30, 26);
			this.pictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox10.TabIndex = 65;
			this.pictureBox10.TabStop = false;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Bold);
			this.label6.Location = new System.Drawing.Point(35, 6);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(82, 17);
			this.label6.TabIndex = 58;
			this.label6.Text = "Time Period";
			// 
			// rightSidePanel
			// 
			this.rightSidePanel.Controls.Add(this.MerticsValidations);
			this.rightSidePanel.Controls.Add(this.ValidationTypes);
			this.rightSidePanel.Controls.Add(this.label13);
			this.rightSidePanel.Controls.Add(this.label9);
			this.rightSidePanel.Controls.Add(this.panel5);
			this.rightSidePanel.Controls.Add(this.panel4);
			this.rightSidePanel.Controls.Add(this.label5);
			this.rightSidePanel.Controls.Add(this.toDate);
			this.rightSidePanel.Controls.Add(this.fromDate);
			this.rightSidePanel.Enabled = false;
			this.rightSidePanel.Location = new System.Drawing.Point(296, 135);
			this.rightSidePanel.Name = "rightSidePanel";
			this.rightSidePanel.Size = new System.Drawing.Size(521, 345);
			this.rightSidePanel.TabIndex = 61;
			// 
			// MerticsValidations
			// 
			this.MerticsValidations.CheckBoxes = true;
			this.MerticsValidations.Location = new System.Drawing.Point(265, 161);
			this.MerticsValidations.Name = "MerticsValidations";
			this.MerticsValidations.Size = new System.Drawing.Size(179, 169);
			this.MerticsValidations.TabIndex = 67;
			this.MerticsValidations.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.MerticsValidations_AfterCheck);
			// 
			// ValidationTypes
			// 
			this.ValidationTypes.CheckBoxes = true;
			this.ValidationTypes.Location = new System.Drawing.Point(12, 161);
			this.ValidationTypes.Name = "ValidationTypes";
			this.ValidationTypes.Size = new System.Drawing.Size(179, 169);
			this.ValidationTypes.TabIndex = 66;
			this.ValidationTypes.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ValidationTypes_NodeMouseClick);
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(262, 145);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(95, 13);
			this.label13.TabIndex = 64;
			this.label13.Text = "Metrics Validations";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(9, 145);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(85, 13);
			this.label9.TabIndex = 63;
			this.label9.Text = "Validation Types";
			// 
			// LogBox
			// 
			this.LogBox.Location = new System.Drawing.Point(11, 646);
			this.LogBox.Name = "LogBox";
			this.LogBox.Size = new System.Drawing.Size(806, 118);
			this.LogBox.TabIndex = 63;
			this.LogBox.Text = "";
			// 
			// DataChecksForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.ClientSize = new System.Drawing.Size(837, 776);
			this.Controls.Add(this.Start_btn);
			this.Controls.Add(this.LogBox);
			this.Controls.Add(this.rightSidePanel);
			this.Controls.Add(this.checkAll);
			this.Controls.Add(this.AccountsCheckedListBox);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.report_btn);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.groupBox3);
			this.Name = "DataChecksForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DataChecks";
			this.Load += new System.EventHandler(this.DataChecks_Load);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.step1.ResumeLayout(false);
			this.step1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.step1_ErrorImage)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel6.ResumeLayout(false);
			this.panel6.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).EndInit();
			this.panel5.ResumeLayout(false);
			this.panel5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
			this.rightSidePanel.ResumeLayout(false);
			this.rightSidePanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DateTimePicker toDate;
        private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.PictureBox step1_ErrorImage;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label step1_status;
        private System.Windows.Forms.ProgressBar step1_progressBar;
        private System.Windows.Forms.CheckedListBox AccountsCheckedListBox;
		private System.Windows.Forms.DateTimePicker fromDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Start_btn;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button report_btn;
        private System.Windows.Forms.Label step1_errorsCount;
		private System.Windows.Forms.Label step1_warningCount;
		private System.Windows.Forms.Panel step1;
		private System.Windows.Forms.CheckBox checkAll;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.PictureBox pictureBox11;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.ComboBox application_cb;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label Profile_lbl;
		private System.Windows.Forms.ComboBox profile_cb;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.PictureBox pictureBox8;
		private System.Windows.Forms.PictureBox pictureBox9;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.PictureBox pictureBox12;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Button clear;
		private System.Windows.Forms.Panel rightSidePanel;
		private System.Windows.Forms.PictureBox pictureBox13;
		private System.Windows.Forms.PictureBox pictureBox10;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TreeView ValidationTypes;
		private System.Windows.Forms.TreeView MerticsValidations;
		private System.Windows.Forms.RichTextBox LogBox;
	}
}