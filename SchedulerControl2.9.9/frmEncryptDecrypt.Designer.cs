namespace Edge_Applications.PM.common
{
	partial class frmEncryptDecrypt
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
            this.encryptBtn = new System.Windows.Forms.Button();
            this.inputTxt = new System.Windows.Forms.TextBox();
            this.pswdLabel = new System.Windows.Forms.Label();
            this.encDecLabel = new System.Windows.Forms.Label();
            this.outputTxt = new System.Windows.Forms.TextBox();
            this.decryptBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // encryptBtn
            // 
            this.encryptBtn.Location = new System.Drawing.Point(205, 129);
            this.encryptBtn.Name = "encryptBtn";
            this.encryptBtn.Size = new System.Drawing.Size(75, 23);
            this.encryptBtn.TabIndex = 0;
            this.encryptBtn.Text = "Encrypt";
            this.encryptBtn.UseVisualStyleBackColor = true;
            this.encryptBtn.Click += new System.EventHandler(this.encryptBtn_Click);
            // 
            // inputTxt
            // 
            this.inputTxt.Location = new System.Drawing.Point(205, 29);
            this.inputTxt.Name = "inputTxt";
            this.inputTxt.Size = new System.Drawing.Size(220, 20);
            this.inputTxt.TabIndex = 1;
            // 
            // pswdLabel
            // 
            this.pswdLabel.AutoSize = true;
            this.pswdLabel.Location = new System.Drawing.Point(22, 36);
            this.pswdLabel.Name = "pswdLabel";
            this.pswdLabel.Size = new System.Drawing.Size(56, 13);
            this.pswdLabel.TabIndex = 2;
            this.pswdLabel.Text = "Password:";
            // 
            // encDecLabel
            // 
            this.encDecLabel.AutoSize = true;
            this.encDecLabel.Location = new System.Drawing.Point(22, 84);
            this.encDecLabel.Name = "encDecLabel";
            this.encDecLabel.Size = new System.Drawing.Size(161, 13);
            this.encDecLabel.TabIndex = 3;
            this.encDecLabel.Text = "Encrypted\\Decrypted Password:";
            // 
            // outputTxt
            // 
            this.outputTxt.Location = new System.Drawing.Point(205, 81);
            this.outputTxt.Name = "outputTxt";
            this.outputTxt.Size = new System.Drawing.Size(220, 20);
            this.outputTxt.TabIndex = 4;
            // 
            // decryptBtn
            // 
            this.decryptBtn.Location = new System.Drawing.Point(350, 129);
            this.decryptBtn.Name = "decryptBtn";
            this.decryptBtn.Size = new System.Drawing.Size(75, 23);
            this.decryptBtn.TabIndex = 5;
            this.decryptBtn.Text = "Decrypt";
            this.decryptBtn.UseVisualStyleBackColor = true;
            this.decryptBtn.Click += new System.EventHandler(this.decryptBtn_Click);
            // 
            // frmEncryptDecrypt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 201);
            this.Controls.Add(this.decryptBtn);
            this.Controls.Add(this.outputTxt);
            this.Controls.Add(this.encDecLabel);
            this.Controls.Add(this.pswdLabel);
            this.Controls.Add(this.inputTxt);
            this.Controls.Add(this.encryptBtn);
            this.Name = "frmEncryptDecrypt";
            this.Text = "frmEncryptDecrypt";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button encryptBtn;
		private System.Windows.Forms.TextBox inputTxt;
        private System.Windows.Forms.Label pswdLabel;
        private System.Windows.Forms.Label encDecLabel;
        private System.Windows.Forms.TextBox outputTxt;
        private System.Windows.Forms.Button decryptBtn;
	}
}