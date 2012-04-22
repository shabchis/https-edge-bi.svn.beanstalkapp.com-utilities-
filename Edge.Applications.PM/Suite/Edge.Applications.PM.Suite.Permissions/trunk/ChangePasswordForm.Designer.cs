namespace Edge.Applications.PM.Suite.Permissions
{
	partial class ChangePasswordForm
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
			this.btnChangePassword = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtValidatePassword = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnChangePassword
			// 
			this.btnChangePassword.Location = new System.Drawing.Point(178, 213);
			this.btnChangePassword.Name = "btnChangePassword";
			this.btnChangePassword.Size = new System.Drawing.Size(75, 23);
			this.btnChangePassword.TabIndex = 0;
			this.btnChangePassword.Text = "Change!";
			this.btnChangePassword.UseVisualStyleBackColor = true;
			this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(28, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Password";
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(113, 51);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(100, 20);
			this.txtPassword.TabIndex = 2;
			// 
			// txtValidatePassword
			// 
			this.txtValidatePassword.Location = new System.Drawing.Point(113, 90);
			this.txtValidatePassword.Name = "txtValidatePassword";
			this.txtValidatePassword.PasswordChar = '*';
			this.txtValidatePassword.Size = new System.Drawing.Size(100, 20);
			this.txtValidatePassword.TabIndex = 4;
			// 
			// ChangePasswordForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.txtValidatePassword);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnChangePassword);
			this.Name = "ChangePasswordForm";
			this.Text = "ChangePasswordForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnChangePassword;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtValidatePassword;
	}
}