namespace Edge.Applications.PM.Suite.DataChecks
{
    partial class ResultForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResultForm));
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.ErrorDataGridView = new System.Windows.Forms.DataGridView();
			this.AccountID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TestLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.message = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ChannelID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Repair = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.label17 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.errCountResult_lbl = new System.Windows.Forms.Label();
			this.Errors = new System.Windows.Forms.Label();
			this.step1_ErrorImage = new System.Windows.Forms.PictureBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.SuccessDataGridView = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label2 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.sucessCountResult_lbl = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.WarningDataGridView = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label5 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.warningsCountResult_lbl = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ErrorDataGridView)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.step1_ErrorImage)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SuccessDataGridView)).BeginInit();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.WarningDataGridView)).BeginInit();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox3
			// 
			this.groupBox3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.groupBox3.Controls.Add(this.ErrorDataGridView);
			this.groupBox3.Controls.Add(this.label17);
			this.groupBox3.Controls.Add(this.panel1);
			this.groupBox3.Location = new System.Drawing.Point(15, 50);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(1010, 228);
			this.groupBox3.TabIndex = 19;
			this.groupBox3.TabStop = false;
			// 
			// ErrorDataGridView
			// 
			this.ErrorDataGridView.AllowUserToAddRows = false;
			this.ErrorDataGridView.AllowUserToOrderColumns = true;
			this.ErrorDataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
			this.ErrorDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ErrorDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ErrorDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AccountID,
            this.TestLevel,
            this.message,
            this.ChannelID,
            this.Date,
            this.Repair});
			this.ErrorDataGridView.GridColor = System.Drawing.SystemColors.ActiveCaption;
			this.ErrorDataGridView.Location = new System.Drawing.Point(7, 43);
			this.ErrorDataGridView.Name = "ErrorDataGridView";
			this.ErrorDataGridView.Size = new System.Drawing.Size(997, 179);
			this.ErrorDataGridView.TabIndex = 33;
			// 
			// AccountID
			// 
			this.AccountID.FillWeight = 90F;
			this.AccountID.HeaderText = "Account ID";
			this.AccountID.Name = "AccountID";
			this.AccountID.Width = 70;
			// 
			// TestLevel
			// 
			this.TestLevel.HeaderText = "Level";
			this.TestLevel.Name = "TestLevel";
			// 
			// message
			// 
			this.message.FillWeight = 300F;
			this.message.HeaderText = "Message";
			this.message.Name = "message";
			this.message.Width = 250;
			// 
			// ChannelID
			// 
			this.ChannelID.HeaderText = "Channel ID";
			this.ChannelID.Name = "ChannelID";
			// 
			// Date
			// 
			this.Date.HeaderText = "Date";
			this.Date.Name = "Date";
			this.Date.Width = 200;
			// 
			// Repair
			// 
			this.Repair.HeaderText = "Repair";
			this.Repair.Name = "Repair";
			this.Repair.ToolTipText = "Check for repair";
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
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.panel1.Controls.Add(this.errCountResult_lbl);
			this.panel1.Controls.Add(this.Errors);
			this.panel1.Controls.Add(this.step1_ErrorImage);
			this.panel1.Location = new System.Drawing.Point(6, 9);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(998, 28);
			this.panel1.TabIndex = 19;
			// 
			// errCountResult_lbl
			// 
			this.errCountResult_lbl.AutoSize = true;
			this.errCountResult_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.errCountResult_lbl.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.errCountResult_lbl.Location = new System.Drawing.Point(874, 7);
			this.errCountResult_lbl.Name = "errCountResult_lbl";
			this.errCountResult_lbl.Size = new System.Drawing.Size(69, 13);
			this.errCountResult_lbl.TabIndex = 40;
			this.errCountResult_lbl.Text = "Totals ( 0 )";
			// 
			// Errors
			// 
			this.Errors.AutoSize = true;
			this.Errors.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.Errors.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.Errors.Location = new System.Drawing.Point(33, 7);
			this.Errors.Name = "Errors";
			this.Errors.Size = new System.Drawing.Size(46, 15);
			this.Errors.TabIndex = 21;
			this.Errors.Text = "Errors";
			// 
			// step1_ErrorImage
			// 
			this.step1_ErrorImage.ErrorImage = null;
			this.step1_ErrorImage.Location = new System.Drawing.Point(0, 1);
			this.step1_ErrorImage.Name = "step1_ErrorImage";
			this.step1_ErrorImage.Size = new System.Drawing.Size(27, 26);
			this.step1_ErrorImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.step1_ErrorImage.TabIndex = 20;
			this.step1_ErrorImage.TabStop = false;
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.groupBox1.Controls.Add(this.SuccessDataGridView);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.panel2);
			this.groupBox1.Location = new System.Drawing.Point(12, 512);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(1010, 221);
			this.groupBox1.TabIndex = 33;
			this.groupBox1.TabStop = false;
			// 
			// SuccessDataGridView
			// 
			this.SuccessDataGridView.AllowUserToAddRows = false;
			this.SuccessDataGridView.AllowUserToOrderColumns = true;
			this.SuccessDataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
			this.SuccessDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.SuccessDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.SuccessDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10});
			this.SuccessDataGridView.GridColor = System.Drawing.SystemColors.ActiveCaption;
			this.SuccessDataGridView.Location = new System.Drawing.Point(6, 43);
			this.SuccessDataGridView.Name = "SuccessDataGridView";
			this.SuccessDataGridView.Size = new System.Drawing.Size(998, 172);
			this.SuccessDataGridView.TabIndex = 34;
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.FillWeight = 90F;
			this.dataGridViewTextBoxColumn6.HeaderText = "Account ID";
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			this.dataGridViewTextBoxColumn6.Width = 70;
			// 
			// dataGridViewTextBoxColumn7
			// 
			this.dataGridViewTextBoxColumn7.HeaderText = "Level";
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn8
			// 
			this.dataGridViewTextBoxColumn8.HeaderText = "Message";
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ReadOnly = true;
			this.dataGridViewTextBoxColumn8.Width = 250;
			// 
			// dataGridViewTextBoxColumn9
			// 
			this.dataGridViewTextBoxColumn9.HeaderText = "Channel ID";
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn10
			// 
			this.dataGridViewTextBoxColumn10.HeaderText = "Date";
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			this.dataGridViewTextBoxColumn10.ReadOnly = true;
			this.dataGridViewTextBoxColumn10.Width = 200;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(327, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(0, 16);
			this.label2.TabIndex = 32;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.panel2.Controls.Add(this.sucessCountResult_lbl);
			this.panel2.Controls.Add(this.label3);
			this.panel2.Controls.Add(this.pictureBox2);
			this.panel2.Location = new System.Drawing.Point(6, 9);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(998, 28);
			this.panel2.TabIndex = 19;
			// 
			// sucessCountResult_lbl
			// 
			this.sucessCountResult_lbl.AutoSize = true;
			this.sucessCountResult_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.sucessCountResult_lbl.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.sucessCountResult_lbl.Location = new System.Drawing.Point(877, 7);
			this.sucessCountResult_lbl.Name = "sucessCountResult_lbl";
			this.sucessCountResult_lbl.Size = new System.Drawing.Size(69, 13);
			this.sucessCountResult_lbl.TabIndex = 44;
			this.sucessCountResult_lbl.Text = "Totals ( 0 )";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.label3.Location = new System.Drawing.Point(33, 7);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(60, 15);
			this.label3.TabIndex = 21;
			this.label3.Text = "Success";
			// 
			// pictureBox2
			// 
			this.pictureBox2.Location = new System.Drawing.Point(1, 1);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(27, 27);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox2.TabIndex = 21;
			this.pictureBox2.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.groupBox2.Controls.Add(this.WarningDataGridView);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.panel3);
			this.groupBox2.Location = new System.Drawing.Point(15, 284);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(1010, 222);
			this.groupBox2.TabIndex = 33;
			this.groupBox2.TabStop = false;
			// 
			// WarningDataGridView
			// 
			this.WarningDataGridView.AllowUserToAddRows = false;
			this.WarningDataGridView.AllowUserToDeleteRows = false;
			this.WarningDataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
			this.WarningDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.WarningDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.WarningDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
			this.WarningDataGridView.GridColor = System.Drawing.SystemColors.ActiveCaption;
			this.WarningDataGridView.Location = new System.Drawing.Point(6, 43);
			this.WarningDataGridView.Name = "WarningDataGridView";
			this.WarningDataGridView.ReadOnly = true;
			this.WarningDataGridView.Size = new System.Drawing.Size(995, 173);
			this.WarningDataGridView.TabIndex = 34;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.FillWeight = 90F;
			this.dataGridViewTextBoxColumn1.HeaderText = "Account ID";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Width = 70;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.HeaderText = "Level";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.HeaderText = "Message";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Width = 250;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.HeaderText = "Channel ID";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.HeaderText = "Date";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.Width = 200;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(327, 42);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(0, 16);
			this.label5.TabIndex = 32;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.panel3.Controls.Add(this.warningsCountResult_lbl);
			this.panel3.Controls.Add(this.label6);
			this.panel3.Controls.Add(this.pictureBox1);
			this.panel3.Location = new System.Drawing.Point(6, 9);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(995, 28);
			this.panel3.TabIndex = 19;
			// 
			// warningsCountResult_lbl
			// 
			this.warningsCountResult_lbl.AutoSize = true;
			this.warningsCountResult_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.warningsCountResult_lbl.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.warningsCountResult_lbl.Location = new System.Drawing.Point(874, 7);
			this.warningsCountResult_lbl.Name = "warningsCountResult_lbl";
			this.warningsCountResult_lbl.Size = new System.Drawing.Size(69, 13);
			this.warningsCountResult_lbl.TabIndex = 42;
			this.warningsCountResult_lbl.Text = "Totals ( 0 )";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.label6.Location = new System.Drawing.Point(33, 7);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(67, 15);
			this.label6.TabIndex = 21;
			this.label6.Text = "Warnings";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
			this.pictureBox1.Location = new System.Drawing.Point(0, 2);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(27, 26);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 20;
			this.pictureBox1.TabStop = false;
			// 
			// button1
			// 
			this.button1.Enabled = false;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button1.Location = new System.Drawing.Point(15, 11);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(115, 33);
			this.button1.TabIndex = 34;
			this.button1.Text = "Send Report";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Enabled = false;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button2.Location = new System.Drawing.Point(136, 11);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(115, 33);
			this.button2.TabIndex = 35;
			this.button2.Text = "Close";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// ResultForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1037, 737);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox3);
			this.Name = "ResultForm";
			this.Text = "ResultForm";
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ErrorDataGridView)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.step1_ErrorImage)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.SuccessDataGridView)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.WarningDataGridView)).EndInit();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.DataGridView ErrorDataGridView;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Errors;
        private System.Windows.Forms.PictureBox step1_ErrorImage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.DataGridView SuccessDataGridView;
        public System.Windows.Forms.DataGridView WarningDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
        public System.Windows.Forms.Label errCountResult_lbl;
        public System.Windows.Forms.Label sucessCountResult_lbl;
        public System.Windows.Forms.Label warningsCountResult_lbl;
		private System.Windows.Forms.DataGridViewTextBoxColumn AccountID;
		private System.Windows.Forms.DataGridViewTextBoxColumn TestLevel;
		private System.Windows.Forms.DataGridViewTextBoxColumn message;
		private System.Windows.Forms.DataGridViewTextBoxColumn ChannelID;
		private System.Windows.Forms.DataGridViewTextBoxColumn Date;
		private System.Windows.Forms.DataGridViewCheckBoxColumn Repair;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
    }
}