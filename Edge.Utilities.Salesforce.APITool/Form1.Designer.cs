namespace Edge.Utilities.Salesforce.APITool
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.loginUrl = new System.Windows.Forms.TextBox();
            this.consumerKey = new System.Windows.Forms.TextBox();
            this.secretKey = new System.Windows.Forms.TextBox();
            this.Code = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.OAuthUrl = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.CreateConnection = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.soql = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.response = new System.Windows.Forms.RichTextBox();
            this.path = new System.Windows.Forms.TextBox();
            this.Download = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.AuthenticationUrl = new System.Windows.Forms.TextBox();
            this.RedirectUrl = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.connectionString = new System.Windows.Forms.TextBox();
            this.result = new System.Windows.Forms.RichTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 287);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Consumer Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 313);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Secret Key";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 441);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Client Code ( From URL )";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 260);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Client Salesforce Login URL:";
            // 
            // loginUrl
            // 
            this.loginUrl.Location = new System.Drawing.Point(167, 257);
            this.loginUrl.Name = "loginUrl";
            this.loginUrl.Size = new System.Drawing.Size(491, 20);
            this.loginUrl.TabIndex = 4;
            this.loginUrl.Text = "https://na9.salesforce.com";
            // 
            // consumerKey
            // 
            this.consumerKey.Location = new System.Drawing.Point(167, 284);
            this.consumerKey.Name = "consumerKey";
            this.consumerKey.Size = new System.Drawing.Size(491, 20);
            this.consumerKey.TabIndex = 5;
            this.consumerKey.Text = "3MVG9y6x0357Hlef8h_O5_I0PNF6kzSnP0IY1dIZbQRnj9Lx79hAWzGXLRcNoQqLd.3ldX2tJyId2OJla" +
    "qJf5";
            // 
            // secretKey
            // 
            this.secretKey.Location = new System.Drawing.Point(167, 310);
            this.secretKey.Name = "secretKey";
            this.secretKey.Size = new System.Drawing.Size(491, 20);
            this.secretKey.TabIndex = 6;
            this.secretKey.Text = "2188694827437286630";
            // 
            // Code
            // 
            this.Code.Location = new System.Drawing.Point(168, 438);
            this.Code.Name = "Code";
            this.Code.Size = new System.Drawing.Size(491, 20);
            this.Code.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 386);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 21);
            this.button1.TabIndex = 8;
            this.button1.Text = "Generate OAuth URL:";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OAuthUrl
            // 
            this.OAuthUrl.Location = new System.Drawing.Point(168, 387);
            this.OAuthUrl.Name = "OAuthUrl";
            this.OAuthUrl.Size = new System.Drawing.Size(491, 20);
            this.OAuthUrl.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label5.Location = new System.Drawing.Point(178, 410);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(305, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "*** Provide this URL to the client and follow the next step in the user guide";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Edge.Utilities.Salesforce.APITool.Properties.Resources.logo;
            this.pictureBox2.Location = new System.Drawing.Point(23, 24);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(175, 72);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 12;
            this.pictureBox2.TabStop = false;
            // 
            // CreateConnection
            // 
            this.CreateConnection.Location = new System.Drawing.Point(220, 473);
            this.CreateConnection.Name = "CreateConnection";
            this.CreateConnection.Size = new System.Drawing.Size(131, 39);
            this.CreateConnection.TabIndex = 13;
            this.CreateConnection.Text = "Create Connection";
            this.CreateConnection.UseVisualStyleBackColor = true;
            this.CreateConnection.Click += new System.EventHandler(this.CreateConnection_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::Edge.Utilities.Salesforce.APITool.Properties.Resources.access_point_add;
            this.pictureBox3.Location = new System.Drawing.Point(167, 465);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(51, 50);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 14;
            this.pictureBox3.TabStop = false;
            // 
            // soql
            // 
            this.soql.Location = new System.Drawing.Point(723, 231);
            this.soql.Name = "soql";
            this.soql.Size = new System.Drawing.Size(449, 236);
            this.soql.TabIndex = 15;
            this.soql.Text = "SELECT CreatedById,CreatedDate,IsConverted ,Status  FROM Lead ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label6.Location = new System.Drawing.Point(723, 209);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(171, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Enter Salesforce Query here:";
            // 
            // response
            // 
            this.response.Location = new System.Drawing.Point(21, 611);
            this.response.Name = "response";
            this.response.Size = new System.Drawing.Size(286, 195);
            this.response.TabIndex = 33;
            this.response.Text = "";
            // 
            // path
            // 
            this.path.Location = new System.Drawing.Point(805, 483);
            this.path.Name = "path";
            this.path.Size = new System.Drawing.Size(260, 20);
            this.path.TabIndex = 35;
            this.path.Text = "c:\\\\SalesforceTest.txt";
            // 
            // Download
            // 
            this.Download.Location = new System.Drawing.Point(1071, 473);
            this.Download.Name = "Download";
            this.Download.Size = new System.Drawing.Size(102, 39);
            this.Download.TabIndex = 32;
            this.Download.Text = "Download Report";
            this.Download.UseVisualStyleBackColor = true;
            this.Download.Click += new System.EventHandler(this.Download_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(719, 486);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Download Path";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 595);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "Server response";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 346);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "Authentication Url:";
            // 
            // AuthenticationUrl
            // 
            this.AuthenticationUrl.Location = new System.Drawing.Point(167, 343);
            this.AuthenticationUrl.Name = "AuthenticationUrl";
            this.AuthenticationUrl.Size = new System.Drawing.Size(491, 20);
            this.AuthenticationUrl.TabIndex = 39;
            this.AuthenticationUrl.Text = "https://login.salesforce.com/services/oauth2/token";
            // 
            // RedirectUrl
            // 
            this.RedirectUrl.Location = new System.Drawing.Point(167, 231);
            this.RedirectUrl.Name = "RedirectUrl";
            this.RedirectUrl.Size = new System.Drawing.Size(491, 20);
            this.RedirectUrl.TabIndex = 41;
            this.RedirectUrl.Text = "http://localhost:8080/RestTest/_callback";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 234);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 13);
            this.label10.TabIndex = 40;
            this.label10.Text = "Redirect URL:";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::Edge.Utilities.Salesforce.APITool.Properties.Resources.database;
            this.pictureBox4.Location = new System.Drawing.Point(21, 184);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(36, 35);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 42;
            this.pictureBox4.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label11.Location = new System.Drawing.Point(60, 185);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(179, 13);
            this.label11.TabIndex = 43;
            this.label11.Text = "Data Base - Connection String";
            // 
            // connectionString
            // 
            this.connectionString.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.connectionString.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.connectionString.Location = new System.Drawing.Point(64, 199);
            this.connectionString.Name = "connectionString";
            this.connectionString.Size = new System.Drawing.Size(531, 11);
            this.connectionString.TabIndex = 44;
            this.connectionString.Text = "Data Source=79.125.11.74;Initial Catalog=Seperia;Integrated Security=false;User I" +
    "D=edge1;PWD=Blublu*2!";
            // 
            // result
            // 
            this.result.Location = new System.Drawing.Point(313, 611);
            this.result.Name = "result";
            this.result.Size = new System.Drawing.Size(859, 195);
            this.result.TabIndex = 45;
            this.result.Text = "";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(310, 595);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 13);
            this.label12.TabIndex = 46;
            this.label12.Text = "Query Result";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label13.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.label13.Location = new System.Drawing.Point(20, 109);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(161, 13);
            this.label13.TabIndex = 47;
            this.label13.Text = "Salesforce Integration Tool";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1268, 862);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.result);
            this.Controls.Add(this.connectionString);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.RedirectUrl);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.AuthenticationUrl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.response);
            this.Controls.Add(this.path);
            this.Controls.Add(this.Download);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.soql);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.CreateConnection);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.OAuthUrl);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Code);
            this.Controls.Add(this.secretKey);
            this.Controls.Add(this.consumerKey);
            this.Controls.Add(this.loginUrl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox loginUrl;
        private System.Windows.Forms.TextBox consumerKey;
        private System.Windows.Forms.TextBox secretKey;
        private System.Windows.Forms.TextBox Code;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox OAuthUrl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button CreateConnection;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.RichTextBox soql;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox response;
        private System.Windows.Forms.TextBox path;
        private System.Windows.Forms.Button Download;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox AuthenticationUrl;
        private System.Windows.Forms.TextBox RedirectUrl;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox connectionString;
        private System.Windows.Forms.RichTextBox result;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
    }
}

