namespace Edge.Applications.PM.Suite
{
	partial class MeasureForm
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
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.application_cb = new System.Windows.Forms.ComboBox();
            this.accounts_cb = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.baseMeasuresListView = new System.Windows.Forms.ListView();
            this.column1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChannelID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MeasureName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DisplayName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SourceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StringFormat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IntegrityCheckRequired = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AcquisitionNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.addMeasureBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.channel_cb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.showMeasuresBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(39, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 17);
            this.label10.TabIndex = 58;
            this.label10.Text = "Account";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(39, 47);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 17);
            this.label12.TabIndex = 58;
            this.label12.Text = "Application";
            // 
            // application_cb
            // 
            this.application_cb.FormattingEnabled = true;
            this.application_cb.Items.AddRange(new object[] {
            "Seperia",
            "Edge.BI"});
            this.application_cb.Location = new System.Drawing.Point(125, 46);
            this.application_cb.Name = "application_cb";
            this.application_cb.Size = new System.Drawing.Size(172, 21);
            this.application_cb.TabIndex = 63;
            this.application_cb.Text = "Select Application";
            this.application_cb.SelectedValueChanged += new System.EventHandler(this.application_cb_SelectedValueChanged);
            // 
            // accounts_cb
            // 
            this.accounts_cb.FormattingEnabled = true;
            this.accounts_cb.Location = new System.Drawing.Point(125, 73);
            this.accounts_cb.Name = "accounts_cb";
            this.accounts_cb.Size = new System.Drawing.Size(172, 21);
            this.accounts_cb.TabIndex = 68;
            this.accounts_cb.Text = "Select Account";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.baseMeasuresListView);
            this.groupBox1.Controls.Add(this.addMeasureBtn);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.groupBox1.Location = new System.Drawing.Point(12, 100);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(747, 459);
            this.groupBox1.TabIndex = 71;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Measures";
            // 
            // baseMeasuresListView
            // 
            this.baseMeasuresListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.baseMeasuresListView.BackColor = System.Drawing.SystemColors.Menu;
            this.baseMeasuresListView.CheckBoxes = true;
            this.baseMeasuresListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column1,
            this.ChannelID,
            this.MeasureName,
            this.DisplayName,
            this.SourceName,
            this.StringFormat,
            this.IntegrityCheckRequired,
            this.AcquisitionNum});
            this.baseMeasuresListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.baseMeasuresListView.FullRowSelect = true;
            this.baseMeasuresListView.HideSelection = false;
            this.baseMeasuresListView.Location = new System.Drawing.Point(13, 17);
            this.baseMeasuresListView.MultiSelect = false;
            this.baseMeasuresListView.Name = "baseMeasuresListView";
            this.baseMeasuresListView.Size = new System.Drawing.Size(647, 423);
            this.baseMeasuresListView.TabIndex = 25;
            this.baseMeasuresListView.UseCompatibleStateImageBehavior = false;
            this.baseMeasuresListView.View = System.Windows.Forms.View.Details;
            this.baseMeasuresListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.baseMeasuresListView_ItemChecked);
            this.baseMeasuresListView.SelectedIndexChanged += new System.EventHandler(this.baseMeasuresListView_SelectedIndexChanged);
            // 
            // column1
            // 
            this.column1.Tag = "1";
            this.column1.Text = "";
            this.column1.Width = 28;
            // 
            // ChannelID
            // 
            this.ChannelID.Tag = "2";
            this.ChannelID.Text = "Channel ID";
            this.ChannelID.Width = 61;
            // 
            // MeasureName
            // 
            this.MeasureName.Tag = "4";
            this.MeasureName.Text = "Name";
            this.MeasureName.Width = 94;
            // 
            // DisplayName
            // 
            this.DisplayName.Tag = "4";
            this.DisplayName.Text = "Display Name";
            this.DisplayName.Width = 96;
            // 
            // SourceName
            // 
            this.SourceName.Tag = "4";
            this.SourceName.Text = "Source Name";
            this.SourceName.Width = 92;
            // 
            // StringFormat
            // 
            this.StringFormat.Tag = "3";
            this.StringFormat.Text = "String Format";
            this.StringFormat.Width = 69;
            // 
            // IntegrityCheckRequired
            // 
            this.IntegrityCheckRequired.Tag = "2";
            this.IntegrityCheckRequired.Text = "Validation Required";
            this.IntegrityCheckRequired.Width = 97;
            // 
            // AcquisitionNum
            // 
            this.AcquisitionNum.Tag = "2";
            this.AcquisitionNum.Text = "Acquisition Num";
            this.AcquisitionNum.Width = 80;
            // 
            // addMeasureBtn
            // 
            this.addMeasureBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addMeasureBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addMeasureBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.addMeasureBtn.Location = new System.Drawing.Point(666, 17);
            this.addMeasureBtn.Name = "addMeasureBtn";
            this.addMeasureBtn.Size = new System.Drawing.Size(58, 30);
            this.addMeasureBtn.TabIndex = 42;
            this.addMeasureBtn.Text = "Add\\Edit";
            this.addMeasureBtn.UseVisualStyleBackColor = false;
            this.addMeasureBtn.Click += new System.EventHandler(this.addMeasureBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(20, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 29);
            this.label1.TabIndex = 74;
            this.label1.Text = "Measure Editor";
            // 
            // channel_cb
            // 
            this.channel_cb.FormattingEnabled = true;
            this.channel_cb.Location = new System.Drawing.Point(408, 46);
            this.channel_cb.Name = "channel_cb";
            this.channel_cb.Size = new System.Drawing.Size(101, 21);
            this.channel_cb.TabIndex = 75;
            this.channel_cb.Text = "Select Channel";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(343, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 17);
            this.label2.TabIndex = 76;
            this.label2.Text = "Channel";
            // 
            // showMeasuresBtn
            // 
            this.showMeasuresBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.showMeasuresBtn.Location = new System.Drawing.Point(346, 73);
            this.showMeasuresBtn.Name = "showMeasuresBtn";
            this.showMeasuresBtn.Size = new System.Drawing.Size(163, 25);
            this.showMeasuresBtn.TabIndex = 77;
            this.showMeasuresBtn.Text = "Show Measures";
            this.showMeasuresBtn.UseVisualStyleBackColor = false;
            this.showMeasuresBtn.Click += new System.EventHandler(this.showMeasuresBtn_Click);
            // 
            // MeasureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(771, 574);
            this.Controls.Add(this.showMeasuresBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.channel_cb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.accounts_cb);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.application_cb);
            this.Name = "MeasureForm";
            this.Text = "MeasureForm";
            this.SizeChanged += new System.EventHandler(this.frm_SizeChanged);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox application_cb;
		private System.Windows.Forms.ComboBox accounts_cb;
        private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListView baseMeasuresListView;
		private System.Windows.Forms.Button addMeasureBtn;
        private System.Windows.Forms.ColumnHeader ChannelID;
		private System.Windows.Forms.ColumnHeader MeasureName;
		private System.Windows.Forms.ColumnHeader DisplayName;
		private System.Windows.Forms.ColumnHeader SourceName;
        private System.Windows.Forms.ColumnHeader StringFormat;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox channel_cb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button showMeasuresBtn;
        private System.Windows.Forms.ColumnHeader IntegrityCheckRequired;
        private System.Windows.Forms.ColumnHeader AcquisitionNum;
        private System.Windows.Forms.ColumnHeader column1;
	}
}