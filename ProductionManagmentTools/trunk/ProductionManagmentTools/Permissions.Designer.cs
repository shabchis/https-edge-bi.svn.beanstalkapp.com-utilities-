namespace Edge.Application.ProductionManagmentTools
{
	partial class Permissions
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Permissions));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.Users = new System.Windows.Forms.TabPage();
			this.btnChangePassword = new System.Windows.Forms.Button();
			this.btnDeleteRow = new System.Windows.Forms.Button();
			this.btnAddRow = new System.Windows.Forms.Button();
			this.relatedGroupsView = new System.Windows.Forms.DataGridView();
			this.label6 = new System.Windows.Forms.Label();
			this.accountsTreeView = new System.Windows.Forms.TreeView();
			this.permissionTree = new System.Windows.Forms.TreeView();
			this.label5 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblUserName = new System.Windows.Forms.Label();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.usersGrid = new System.Windows.Forms.DataGridView();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.txtFindUser = new System.Windows.Forms.TextBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.btnConnect = new System.Windows.Forms.Button();
			this.contextMenuPermissions = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuAllow = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuNotAllow = new System.Windows.Forms.ToolStripMenuItem();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.toolTipChangePassword = new System.Windows.Forms.ToolTip(this.components);
			this.tabControl1.SuspendLayout();
			this.Users.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.relatedGroupsView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.usersGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.contextMenuPermissions.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.Users);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(12, 56);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1213, 610);
			this.tabControl1.TabIndex = 0;
			// 
			// Users
			// 
			this.Users.BackColor = System.Drawing.Color.Gainsboro;
			this.Users.Controls.Add(this.btnChangePassword);
			this.Users.Controls.Add(this.btnDeleteRow);
			this.Users.Controls.Add(this.btnAddRow);
			this.Users.Controls.Add(this.relatedGroupsView);
			this.Users.Controls.Add(this.label6);
			this.Users.Controls.Add(this.accountsTreeView);
			this.Users.Controls.Add(this.permissionTree);
			this.Users.Controls.Add(this.label5);
			this.Users.Controls.Add(this.label2);
			this.Users.Controls.Add(this.lblUserName);
			this.Users.Controls.Add(this.pictureBox2);
			this.Users.Controls.Add(this.usersGrid);
			this.Users.Controls.Add(this.pictureBox1);
			this.Users.Controls.Add(this.txtFindUser);
			this.Users.Location = new System.Drawing.Point(4, 22);
			this.Users.Name = "Users";
			this.Users.Padding = new System.Windows.Forms.Padding(3);
			this.Users.Size = new System.Drawing.Size(1205, 584);
			this.Users.TabIndex = 0;
			this.Users.Text = "Users";
			// 
			// btnChangePassword
			// 
			this.btnChangePassword.Image = global::Edge.Application.ProductionManagmentTools.Properties.Resources.Key16;
			this.btnChangePassword.Location = new System.Drawing.Point(566, 6);
			this.btnChangePassword.Name = "btnChangePassword";
			this.btnChangePassword.Size = new System.Drawing.Size(31, 29);
			this.btnChangePassword.TabIndex = 27;
			this.btnChangePassword.UseVisualStyleBackColor = true;
			this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
			// 
			// btnDeleteRow
			// 
			this.btnDeleteRow.Image = global::Edge.Application.ProductionManagmentTools.Properties.Resources.erase;
			this.btnDeleteRow.Location = new System.Drawing.Point(529, 6);
			this.btnDeleteRow.Name = "btnDeleteRow";
			this.btnDeleteRow.Size = new System.Drawing.Size(31, 29);
			this.btnDeleteRow.TabIndex = 26;
			this.btnDeleteRow.UseVisualStyleBackColor = true;
			this.btnDeleteRow.Click += new System.EventHandler(this.btnDeleteRow_Click);
			// 
			// btnAddRow
			// 
			this.btnAddRow.Image = global::Edge.Application.ProductionManagmentTools.Properties.Resources.Add_user24;
			this.btnAddRow.Location = new System.Drawing.Point(492, 5);
			this.btnAddRow.Name = "btnAddRow";
			this.btnAddRow.Size = new System.Drawing.Size(31, 29);
			this.btnAddRow.TabIndex = 25;
			this.btnAddRow.UseVisualStyleBackColor = true;
			this.btnAddRow.Click += new System.EventHandler(this.btnAddRow_Click);
			// 
			// relatedGroupsView
			// 
			this.relatedGroupsView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.relatedGroupsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.relatedGroupsView.Location = new System.Drawing.Point(664, 197);
			this.relatedGroupsView.Name = "relatedGroupsView";
			this.relatedGroupsView.Size = new System.Drawing.Size(234, 342);
			this.relatedGroupsView.TabIndex = 24;
			this.relatedGroupsView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.relatedGroupsView_CellValueChanged);
			this.relatedGroupsView.CurrentCellDirtyStateChanged += new System.EventHandler(this.relatedGroupsView_CurrentCellDirtyStateChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label6.Location = new System.Drawing.Point(925, 171);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(102, 15);
			this.label6.TabIndex = 21;
			this.label6.Text = "Related Accounts";
			// 
			// accountsTreeView
			// 
			this.accountsTreeView.BackColor = System.Drawing.Color.LightGray;
			this.accountsTreeView.FullRowSelect = true;
			this.accountsTreeView.HideSelection = false;
			this.accountsTreeView.Location = new System.Drawing.Point(925, 194);
			this.accountsTreeView.Name = "accountsTreeView";
			this.accountsTreeView.Size = new System.Drawing.Size(232, 159);
			this.accountsTreeView.TabIndex = 20;
			this.accountsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.accountsTreeView_AfterSelect);
			// 
			// permissionTree
			// 
			this.permissionTree.BackColor = System.Drawing.Color.LightGray;
			this.permissionTree.Location = new System.Drawing.Point(925, 380);
			this.permissionTree.Name = "permissionTree";
			this.permissionTree.Size = new System.Drawing.Size(232, 159);
			this.permissionTree.TabIndex = 16;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label5.Location = new System.Drawing.Point(925, 359);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(75, 15);
			this.label5.TabIndex = 15;
			this.label5.Text = "Permissions";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label2.Location = new System.Drawing.Point(661, 176);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(93, 15);
			this.label2.TabIndex = 14;
			this.label2.Text = "Related Groups";
			// 
			// lblUserName
			// 
			this.lblUserName.AutoSize = true;
			this.lblUserName.BackColor = System.Drawing.Color.Transparent;
			this.lblUserName.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblUserName.Location = new System.Drawing.Point(1090, 142);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.Size = new System.Drawing.Size(99, 18);
			this.lblUserName.TabIndex = 12;
			this.lblUserName.Text = "Avi Manaker";
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::Edge.Application.ProductionManagmentTools.Properties.Resources.user_pic;
			this.pictureBox2.Location = new System.Drawing.Point(1090, 41);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(99, 98);
			this.pictureBox2.TabIndex = 4;
			this.pictureBox2.TabStop = false;
			// 
			// usersGrid
			// 
			this.usersGrid.BackgroundColor = System.Drawing.Color.LightGray;
			this.usersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.usersGrid.Location = new System.Drawing.Point(3, 41);
			this.usersGrid.MultiSelect = false;
			this.usersGrid.Name = "usersGrid";
			this.usersGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.usersGrid.Size = new System.Drawing.Size(592, 534);
			this.usersGrid.TabIndex = 3;
			this.usersGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.usersGrid_CellEndEdit);
			this.usersGrid.SelectionChanged += new System.EventHandler(this.usersGrid_SelectionChanged);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackgroundImage = global::Edge.Application.ProductionManagmentTools.Properties.Resources.Zoom_in48;
			this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.pictureBox1.Location = new System.Drawing.Point(4, 6);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(20, 20);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// txtFindUser
			// 
			this.txtFindUser.Location = new System.Drawing.Point(30, 6);
			this.txtFindUser.Name = "txtFindUser";
			this.txtFindUser.Size = new System.Drawing.Size(127, 20);
			this.txtFindUser.TabIndex = 0;
			this.txtFindUser.TextChanged += new System.EventHandler(this.txtFindUser_TextChanged);
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(1205, 584);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Groups";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// btnConnect
			// 
			this.btnConnect.Location = new System.Drawing.Point(12, 12);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(75, 23);
			this.btnConnect.TabIndex = 1;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// contextMenuPermissions
			// 
			this.contextMenuPermissions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuAllow,
            this.toolStripMenuNotAllow});
			this.contextMenuPermissions.Name = "contextMenuPermissions";
			this.contextMenuPermissions.Size = new System.Drawing.Size(141, 48);
			// 
			// toolStripMenuAllow
			// 
			this.toolStripMenuAllow.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuAllow.Image")));
			this.toolStripMenuAllow.Name = "toolStripMenuAllow";
			this.toolStripMenuAllow.Size = new System.Drawing.Size(140, 22);
			this.toolStripMenuAllow.Text = "Allow";
			this.toolStripMenuAllow.Click += new System.EventHandler(this.toolStripMenuAllow_Click);
			// 
			// toolStripMenuNotAllow
			// 
			this.toolStripMenuNotAllow.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuNotAllow.Image")));
			this.toolStripMenuNotAllow.Name = "toolStripMenuNotAllow";
			this.toolStripMenuNotAllow.Size = new System.Drawing.Size(140, 22);
			this.toolStripMenuNotAllow.Text = "Not Allowed";
			this.toolStripMenuNotAllow.Click += new System.EventHandler(this.toolStripMenuAllow_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Location = new System.Drawing.Point(1146, 672);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(75, 23);
			this.btnUpdate.TabIndex = 2;
			this.btnUpdate.Text = "Add/Update";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// toolTipChangePassword
			// 
			this.toolTipChangePassword.ToolTipTitle = "Change Password";
			// 
			// Permissions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1237, 707);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.btnConnect);
			this.Name = "Permissions";
			this.Text = "Permissions";
			this.Load += new System.EventHandler(this.Permissions_Load);
			this.tabControl1.ResumeLayout(false);
			this.Users.ResumeLayout(false);
			this.Users.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.relatedGroupsView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.usersGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.contextMenuPermissions.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage Users;
		private System.Windows.Forms.TextBox txtFindUser;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.DataGridView usersGrid;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TreeView permissionTree;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblUserName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TreeView accountsTreeView;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.DataGridView relatedGroupsView;
		private System.Windows.Forms.ContextMenuStrip contextMenuPermissions;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuAllow;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuNotAllow;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnAddRow;
		private System.Windows.Forms.Button btnDeleteRow;
		private System.Windows.Forms.Button btnChangePassword;
		private System.Windows.Forms.ToolTip toolTipChangePassword;
	}
}