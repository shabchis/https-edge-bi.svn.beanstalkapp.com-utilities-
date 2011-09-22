namespace Edge.Application.ProductionManagmentTools
{
	partial class Password
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
			this.enc = new System.Windows.Forms.TextBox();
			this.dec = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.dec_btn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(30, 96);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Enc Pass";
			// 
			// enc
			// 
			this.enc.Location = new System.Drawing.Point(88, 93);
			this.enc.Name = "enc";
			this.enc.Size = new System.Drawing.Size(212, 20);
			this.enc.TabIndex = 1;
			// 
			// dec
			// 
			this.dec.Location = new System.Drawing.Point(88, 119);
			this.dec.Name = "dec";
			this.dec.Size = new System.Drawing.Size(212, 20);
			this.dec.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(30, 122);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Dec Pass";
			// 
			// dec_btn
			// 
			this.dec_btn.Location = new System.Drawing.Point(306, 93);
			this.dec_btn.Name = "dec_btn";
			this.dec_btn.Size = new System.Drawing.Size(74, 23);
			this.dec_btn.TabIndex = 4;
			this.dec_btn.Text = "Dec";
			this.dec_btn.UseVisualStyleBackColor = true;
			this.dec_btn.Click += new System.EventHandler(this.dec_btn_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(412, 262);
			this.Controls.Add(this.dec_btn);
			this.Controls.Add(this.dec);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.enc);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox enc;
		private System.Windows.Forms.TextBox dec;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button dec_btn;
	}
}