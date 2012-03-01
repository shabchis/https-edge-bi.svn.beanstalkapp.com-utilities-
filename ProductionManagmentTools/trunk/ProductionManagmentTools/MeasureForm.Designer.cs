namespace Edge.Application.ProductionManagmentTools
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeasureForm));
			this.pictureBox11 = new System.Windows.Forms.PictureBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.application_cb = new System.Windows.Forms.ComboBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.accountsList = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.AddFromAvailable = new System.Windows.Forms.Button();
			this.optionsListView = new System.Windows.Forms.ListView();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.ChannelID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MeasureID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.BaseMeasureID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.DisplayName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SourceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.StringFormat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox11
			// 
			this.pictureBox11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox11.Image = global::Edge.Application.ProductionManagmentTools.Properties.Resources.users;
			this.pictureBox11.Location = new System.Drawing.Point(18, 74);
			this.pictureBox11.Name = "pictureBox11";
			this.pictureBox11.Size = new System.Drawing.Size(15, 15);
			this.pictureBox11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox11.TabIndex = 53;
			this.pictureBox11.TabStop = false;
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
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.Image = global::Edge.Application.ProductionManagmentTools.Properties.Resources.preferences_system_windows_actions;
			this.pictureBox1.Location = new System.Drawing.Point(18, 47);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(15, 15);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 61;
			this.pictureBox1.TabStop = false;
			// 
			// accountsList
			// 
			this.accountsList.FormattingEnabled = true;
			this.accountsList.Location = new System.Drawing.Point(125, 73);
			this.accountsList.Name = "accountsList";
			this.accountsList.Size = new System.Drawing.Size(172, 21);
			this.accountsList.TabIndex = 68;
			this.accountsList.Text = "Select Account";
			this.accountsList.SelectedValueChanged += new System.EventHandler(this.accountsList_SelectedValueChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.pictureBox2);
			this.groupBox1.Controls.Add(this.optionsListView);
			this.groupBox1.Controls.Add(this.AddFromAvailable);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
			this.groupBox1.Location = new System.Drawing.Point(12, 100);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(715, 180);
			this.groupBox1.TabIndex = 71;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Base Measures";
			// 
			// AddFromAvailable
			// 
			this.AddFromAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
			this.AddFromAvailable.Location = new System.Drawing.Point(663, 47);
			this.AddFromAvailable.Name = "AddFromAvailable";
			this.AddFromAvailable.Size = new System.Drawing.Size(35, 30);
			this.AddFromAvailable.TabIndex = 42;
			this.AddFromAvailable.Text = ">";
			this.AddFromAvailable.UseVisualStyleBackColor = false;
			// 
			// optionsListView
			// 
			this.optionsListView.BackColor = System.Drawing.SystemColors.Menu;
			this.optionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ChannelID,
            this.MeasureID,
            this.BaseMeasureID,
            this.Name,
            this.DisplayName,
            this.SourceName,
            this.StringFormat});
			this.optionsListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
			this.optionsListView.FullRowSelect = true;
			this.optionsListView.Location = new System.Drawing.Point(12, 21);
			this.optionsListView.Name = "optionsListView";
			this.optionsListView.Size = new System.Drawing.Size(632, 142);
			this.optionsListView.TabIndex = 25;
			this.optionsListView.UseCompatibleStateImageBehavior = false;
			this.optionsListView.View = System.Windows.Forms.View.Details;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
			this.pictureBox2.Location = new System.Drawing.Point(12, 0);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(0, 0);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox2.TabIndex = 72;
			this.pictureBox2.TabStop = false;
			// 
			// ChannelID
			// 
			this.ChannelID.Text = "Channel ID";
			// 
			// MeasureID
			// 
			this.MeasureID.Text = "MeasureI D";
			this.MeasureID.Width = 96;
			// 
			// BaseMeasureID
			// 
			this.BaseMeasureID.Text = "Base MeasureI D";
			this.BaseMeasureID.Width = 99;
			// 
			// Name
			// 
			this.Name.Text = "Name";
			this.Name.Width = 88;
			// 
			// DisplayName
			// 
			this.DisplayName.Text = "Display Name";
			this.DisplayName.Width = 94;
			// 
			// SourceName
			// 
			this.SourceName.Text = "Source Name";
			this.SourceName.Width = 102;
			// 
			// StringFormat
			// 
			this.StringFormat.Text = "String Format";
			this.StringFormat.Width = 87;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.button1);
			this.groupBox2.Controls.Add(this.listView1);
			this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
			this.groupBox2.Location = new System.Drawing.Point(12, 286);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(715, 266);
			this.groupBox2.TabIndex = 73;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Measures";
			// 
			// listView1
			// 
			this.listView1.BackColor = System.Drawing.SystemColors.Menu;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
			this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
			this.listView1.FullRowSelect = true;
			this.listView1.Location = new System.Drawing.Point(12, 17);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(633, 243);
			this.listView1.TabIndex = 25;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Channel ID";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "MeasureI D";
			this.columnHeader2.Width = 96;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Base MeasureI D";
			this.columnHeader3.Width = 99;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Name";
			this.columnHeader4.Width = 88;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Display Name";
			this.columnHeader5.Width = 94;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Source Name";
			this.columnHeader6.Width = 102;
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "String Format";
			this.columnHeader7.Width = 87;
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
			this.button1.Location = new System.Drawing.Point(663, 44);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(35, 30);
			this.button1.TabIndex = 74;
			this.button1.Text = "<";
			this.button1.UseVisualStyleBackColor = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Yoav Cursive", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label1.Location = new System.Drawing.Point(14, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(138, 23);
			this.label1.TabIndex = 74;
			this.label1.Text = "Measure Editor";
			// 
			// MeasureForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(744, 591);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.accountsList);
			this.Controls.Add(this.pictureBox11);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.application_cb);
			
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox application_cb;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ComboBox accountsList;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.ListView optionsListView;
		private System.Windows.Forms.Button AddFromAvailable;
		private System.Windows.Forms.ColumnHeader ChannelID;
		private System.Windows.Forms.ColumnHeader MeasureID;
		private System.Windows.Forms.ColumnHeader BaseMeasureID;
		private System.Windows.Forms.ColumnHeader Name;
		private System.Windows.Forms.ColumnHeader DisplayName;
		private System.Windows.Forms.ColumnHeader SourceName;
		private System.Windows.Forms.ColumnHeader StringFormat;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.Label label1;
	}
}