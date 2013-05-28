namespace Edge.Utilities.AdwordsAPI.ReportsTool
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
            this.label1 = new System.Windows.Forms.Label();
            this.AvailableReportFields = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ReportNamesListBox = new System.Windows.Forms.ComboBox();
            this.GetReportFields = new System.Windows.Forms.Button();
            this.Clear = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.Download = new System.Windows.Forms.Button();
            this.response = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Developer_Token = new System.Windows.Forms.Label();
            this.DeveloperToken = new System.Windows.Forms.TextBox();
            this.EnableGzipCompression = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ClientCustomerId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Email = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.path = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AWQL_textBox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.webExceptionTextBox = new System.Windows.Forms.RichTextBox();
            this.get_Acc_Lables = new System.Windows.Forms.Button();
            this.GetAccountHistory = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Report Name:";
            // 
            // AvailableReportFields
            // 
            this.AvailableReportFields.Location = new System.Drawing.Point(68, 132);
            this.AvailableReportFields.Name = "AvailableReportFields";
            this.AvailableReportFields.Size = new System.Drawing.Size(690, 57);
            this.AvailableReportFields.TabIndex = 1;
            this.AvailableReportFields.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(65, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Available fields names for Developers";
            // 
            // ReportNamesListBox
            // 
            this.ReportNamesListBox.FormattingEnabled = true;
            this.ReportNamesListBox.Location = new System.Drawing.Point(144, 52);
            this.ReportNamesListBox.Name = "ReportNamesListBox";
            this.ReportNamesListBox.Size = new System.Drawing.Size(327, 21);
            this.ReportNamesListBox.TabIndex = 3;
            // 
            // GetReportFields
            // 
            this.GetReportFields.Location = new System.Drawing.Point(487, 52);
            this.GetReportFields.Name = "GetReportFields";
            this.GetReportFields.Size = new System.Drawing.Size(102, 23);
            this.GetReportFields.TabIndex = 4;
            this.GetReportFields.Text = "GetReportFields";
            this.GetReportFields.UseVisualStyleBackColor = true;
            this.GetReportFields.Click += new System.EventHandler(this.GetReportFields_Click);
            // 
            // Clear
            // 
            this.Clear.Location = new System.Drawing.Point(596, 52);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(75, 23);
            this.Clear.TabIndex = 5;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(68, 224);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(1063, 252);
            this.dataGridView.TabIndex = 7;
            // 
            // Download
            // 
            this.Download.Location = new System.Drawing.Point(1029, 492);
            this.Download.Name = "Download";
            this.Download.Size = new System.Drawing.Size(102, 23);
            this.Download.TabIndex = 8;
            this.Download.Text = "Download Report";
            this.Download.UseVisualStyleBackColor = true;
            this.Download.Click += new System.EventHandler(this.Download_Click);
            // 
            // response
            // 
            this.response.Location = new System.Drawing.Point(715, 549);
            this.response.Name = "response";
            this.response.Size = new System.Drawing.Size(416, 94);
            this.response.TabIndex = 9;
            this.response.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(794, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Adwords User details";
            // 
            // Developer_Token
            // 
            this.Developer_Token.AutoSize = true;
            this.Developer_Token.Location = new System.Drawing.Point(794, 78);
            this.Developer_Token.Name = "Developer_Token";
            this.Developer_Token.Size = new System.Drawing.Size(87, 13);
            this.Developer_Token.TabIndex = 11;
            this.Developer_Token.Text = "DeveloperToken";
            // 
            // DeveloperToken
            // 
            this.DeveloperToken.Location = new System.Drawing.Point(924, 76);
            this.DeveloperToken.Name = "DeveloperToken";
            this.DeveloperToken.Size = new System.Drawing.Size(207, 20);
            this.DeveloperToken.TabIndex = 16;
            this.DeveloperToken.Text = "5eCsvAOU06Fs4j5qHWKTCA";
            // 
            // EnableGzipCompression
            // 
            this.EnableGzipCompression.Location = new System.Drawing.Point(924, 102);
            this.EnableGzipCompression.Name = "EnableGzipCompression";
            this.EnableGzipCompression.Size = new System.Drawing.Size(207, 20);
            this.EnableGzipCompression.TabIndex = 18;
            this.EnableGzipCompression.Text = "true";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(794, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "EnableGzipCompression";
            // 
            // ClientCustomerId
            // 
            this.ClientCustomerId.Location = new System.Drawing.Point(924, 128);
            this.ClientCustomerId.Name = "ClientCustomerId";
            this.ClientCustomerId.Size = new System.Drawing.Size(207, 20);
            this.ClientCustomerId.TabIndex = 20;
            this.ClientCustomerId.Text = "508-397-0423";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(794, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "ClientCustomerId";
            // 
            // Email
            // 
            this.Email.Location = new System.Drawing.Point(924, 154);
            this.Email.Name = "Email";
            this.Email.Size = new System.Drawing.Size(207, 20);
            this.Email.TabIndex = 22;
            this.Email.Text = "edge.bi.mcc@gmail.com";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(794, 156);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Email";
            // 
            // path
            // 
            this.path.Location = new System.Drawing.Point(804, 494);
            this.path.Name = "path";
            this.path.Size = new System.Drawing.Size(207, 20);
            this.path.TabIndex = 24;
            this.path.Text = "c:\\\\GoogleAdwordsTest.Gzip";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(718, 497);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Download Path";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(718, 524);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Server response";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(65, 208);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Select Report fields to download";
            // 
            // AWQL_textBox
            // 
            this.AWQL_textBox.Location = new System.Drawing.Point(68, 549);
            this.AWQL_textBox.Name = "AWQL_textBox";
            this.AWQL_textBox.Size = new System.Drawing.Size(495, 167);
            this.AWQL_textBox.TabIndex = 27;
            this.AWQL_textBox.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(68, 497);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 28;
            this.button1.Text = "Get AWQL";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(65, 727);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(152, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "Edit this query and learn more :";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(223, 724);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(340, 20);
            this.textBox1.TabIndex = 30;
            this.textBox1.Text = "https://developers.google.com/adwords/api/docs/guides/awql";
            // 
            // webExceptionTextBox
            // 
            this.webExceptionTextBox.Location = new System.Drawing.Point(715, 650);
            this.webExceptionTextBox.Name = "webExceptionTextBox";
            this.webExceptionTextBox.Size = new System.Drawing.Size(416, 94);
            this.webExceptionTextBox.TabIndex = 31;
            this.webExceptionTextBox.Text = "";
            // 
            // get_Acc_Lables
            // 
            this.get_Acc_Lables.Location = new System.Drawing.Point(1246, 45);
            this.get_Acc_Lables.Name = "get_Acc_Lables";
            this.get_Acc_Lables.Size = new System.Drawing.Size(111, 23);
            this.get_Acc_Lables.TabIndex = 32;
            this.get_Acc_Lables.Text = "Get account labels";
            this.get_Acc_Lables.UseVisualStyleBackColor = true;
            this.get_Acc_Lables.Click += new System.EventHandler(this.get_Acc_Lables_Click);
            // 
            // GetAccountHistory
            // 
            this.GetAccountHistory.Location = new System.Drawing.Point(1246, 78);
            this.GetAccountHistory.Name = "GetAccountHistory";
            this.GetAccountHistory.Size = new System.Drawing.Size(111, 23);
            this.GetAccountHistory.TabIndex = 33;
            this.GetAccountHistory.Text = "Get account History";
            this.GetAccountHistory.UseVisualStyleBackColor = true;
            this.GetAccountHistory.Click += new System.EventHandler(this.GetAccountHistory_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1393, 839);
            this.Controls.Add(this.GetAccountHistory);
            this.Controls.Add(this.get_Acc_Lables);
            this.Controls.Add(this.webExceptionTextBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.AWQL_textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.path);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Email);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ClientCustomerId);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.EnableGzipCompression);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DeveloperToken);
            this.Controls.Add(this.Developer_Token);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.response);
            this.Controls.Add(this.Download);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.GetReportFields);
            this.Controls.Add(this.ReportNamesListBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AvailableReportFields);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox AvailableReportFields;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ReportNamesListBox;
        private System.Windows.Forms.Button GetReportFields;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button Download;
        private System.Windows.Forms.RichTextBox response;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Developer_Token;
        private System.Windows.Forms.TextBox DeveloperToken;
        private System.Windows.Forms.TextBox EnableGzipCompression;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ClientCustomerId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Email;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox path;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox AWQL_textBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RichTextBox webExceptionTextBox;
        private System.Windows.Forms.Button get_Acc_Lables;
        private System.Windows.Forms.Button GetAccountHistory;
    }
}

