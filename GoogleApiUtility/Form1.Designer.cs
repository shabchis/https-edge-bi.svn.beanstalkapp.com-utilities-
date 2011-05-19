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
			this.dataGridView1.Size = new System.Drawing.Size(980, 536);
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
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1068, 591);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.GetFields_btn);
			//this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
	}
}