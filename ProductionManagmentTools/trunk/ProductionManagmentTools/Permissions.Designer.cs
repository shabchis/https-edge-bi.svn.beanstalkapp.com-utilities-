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
			this.btnDeleteUserRow = new System.Windows.Forms.Button();
			this.btnAddUserRow = new System.Windows.Forms.Button();
			this.relatedGroupsView = new System.Windows.Forms.DataGridView();
			this.label6 = new System.Windows.Forms.Label();
			this.usersAccountsTreeView = new System.Windows.Forms.TreeView();
			this.userPermissionTree = new System.Windows.Forms.TreeView();
			this.label5 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblUserName = new System.Windows.Forms.Label();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.usersGrid = new System.Windows.Forms.DataGridView();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.txtFindUser = new System.Windows.Forms.TextBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.btnAddGroupRow = new System.Windows.Forms.Button();
			this.membersGrid = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
			this.groupsAccountsTreeView = new System.Windows.Forms.TreeView();
			this.groupsPermissionTree = new System.Windows.Forms.TreeView();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lblGroupName = new System.Windows.Forms.Label();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.groupsView = new System.Windows.Forms.DataGridView();
			this.txtFindGroup = new System.Windows.Forms.TextBox();
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
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.membersGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupsView)).BeginInit();
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
			this.Users.Controls.Add(this.btnDeleteUserRow);
			this.Users.Controls.Add(this.btnAddUserRow);
			this.Users.Controls.Add(this.relatedGroupsView);
			this.Users.Controls.Add(this.label6);
			this.Users.Controls.Add(this.usersAccountsTreeView);
			this.Users.Controls.Add(this.userPermissionTree);
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
			// btnDeleteUserRow
			// 
			this.btnDeleteUserRow.Image = global::Edge.Application.ProductionManagmentTools.Properties.Resources.erase;
			this.btnDeleteUserRow.Location = new System.Drawing.Point(529, 6);
			this.btnDeleteUserRow.Name = "btnDeleteUserRow";
			this.btnDeleteUserRow.Size = new System.Drawing.Size(31, 29);
			this.btnDeleteUserRow.TabIndex = 26;
			this.btnDeleteUserRow.UseVisualStyleBackColor = true;
			this.btnDeleteUserRow.Click += new System.EventHandler(this.btnUserDeleteRow_Click);
			// 
			// btnAddUserRow
			// 
			this.btnAddUserRow.Image = global::Edge.Application.ProductionManagmentTools.Properties.Resources.Add_user24;
			this.btnAddUserRow.Location = new System.Drawing.Point(492, 5);
			this.btnAddUserRow.Name = "btnAddUserRow";
			this.btnAddUserRow.Size = new System.Drawing.Size(31, 29);
			this.btnAddUserRow.TabIndex = 25;
			this.btnAddUserRow.UseVisualStyleBackColor = true;
			this.btnAddUserRow.Click += new System.EventHandler(this.btnAddRow_Click);
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
			// usersAccountsTreeView
			// 
			this.usersAccountsTreeView.BackColor = System.Drawing.Color.LightGray;
			this.usersAccountsTreeView.FullRowSelect = true;
			this.usersAccountsTreeView.HideSelection = false;
			this.usersAccountsTreeView.Location = new System.Drawing.Point(925, 194);
			this.usersAccountsTreeView.Name = "usersAccountsTreeView";
			this.usersAccountsTreeView.Size = new System.Drawing.Size(232, 159);
			this.usersAccountsTreeView.TabIndex = 20;
			this.usersAccountsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.accountsTreeView_AfterSelect);
			// 
			// userPermissionTree
			// 
			this.userPermissionTree.BackColor = System.Drawing.Color.LightGray;
			this.userPermissionTree.Location = new System.Drawing.Point(925, 380);
			this.userPermissionTree.Name = "userPermissionTree";
			this.userPermissionTree.Size = new System.Drawing.Size(232, 159);
			this.userPermissionTree.TabIndex = 16;
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
			this.tabPage2.Controls.Add(this.pictureBox4);
			this.tabPage2.Controls.Add(this.btnAddGroupRow);
			this.tabPage2.Controls.Add(this.membersGrid);
			this.tabPage2.Controls.Add(this.label1);
			this.tabPage2.Controls.Add(this.groupsAccountsTreeView);
			this.tabPage2.Controls.Add(this.groupsPermissionTree);
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Controls.Add(this.label4);
			this.tabPage2.Controls.Add(this.lblGroupName);
			this.tabPage2.Controls.Add(this.pictureBox3);
			this.tabPage2.Controls.Add(this.groupsView);
			this.tabPage2.Controls.Add(this.txtFindGroup);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(1205, 584);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Groups";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// pictureBox4
			// 
			this.pictureBox4.BackgroundImage = global::Edge.Application.ProductionManagmentTools.Properties.Resources.Zoom_in48;
			this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.pictureBox4.Location = new System.Drawing.Point(9, 8);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(20, 20);
			this.pictureBox4.TabIndex = 40;
			this.pictureBox4.TabStop = false;
			// 
			// btnAddGroupRow
			// 
			this.btnAddGroupRow.Image = global::Edge.Application.ProductionManagmentTools.Properties.Resources.Add_user24;
			this.btnAddGroupRow.Location = new System.Drawing.Point(570, 6);
			this.btnAddGroupRow.Name = "btnAddGroupRow";
			this.btnAddGroupRow.Size = new System.Drawing.Size(31, 29);
			this.btnAddGroupRow.TabIndex = 38;
			this.btnAddGroupRow.UseVisualStyleBackColor = true;
			this.btnAddGroupRow.Click += new System.EventHandler(this.btnAddGroupRow_Click);
			// 
			// membersGrid
			// 
			this.membersGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.membersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.membersGrid.Enabled = false;
			this.membersGrid.Location = new System.Drawing.Point(670, 199);
			this.membersGrid.Name = "membersGrid";
			this.membersGrid.Size = new System.Drawing.Size(234, 342);
			this.membersGrid.TabIndex = 37;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label1.Location = new System.Drawing.Point(931, 173);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(102, 15);
			this.label1.TabIndex = 36;
			this.label1.Text = "Related Accounts";
			// 
			// groupsAccountsTreeView
			// 
			this.groupsAccountsTreeView.BackColor = System.Drawing.Color.LightGray;
			this.groupsAccountsTreeView.FullRowSelect = true;
			this.groupsAccountsTreeView.HideSelection = false;
			this.groupsAccountsTreeView.Location = new System.Drawing.Point(931, 196);
			this.groupsAccountsTreeView.Name = "groupsAccountsTreeView";
			this.groupsAccountsTreeView.Size = new System.Drawing.Size(232, 159);
			this.groupsAccountsTreeView.TabIndex = 35;
			this.groupsAccountsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.groupsAccountsTreeView_AfterSelect);
			// 
			// groupsPermissionTree
			// 
			this.groupsPermissionTree.BackColor = System.Drawing.Color.LightGray;
			this.groupsPermissionTree.Location = new System.Drawing.Point(931, 382);
			this.groupsPermissionTree.Name = "groupsPermissionTree";
			this.groupsPermissionTree.Size = new System.Drawing.Size(232, 159);
			this.groupsPermissionTree.TabIndex = 34;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label3.Location = new System.Drawing.Point(931, 361);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(75, 15);
			this.label3.TabIndex = 33;
			this.label3.Text = "Permissions";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.label4.Location = new System.Drawing.Point(667, 178);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(60, 15);
			this.label4.TabIndex = 32;
			this.label4.Text = "Members";
			// 
			// lblGroupName
			// 
			this.lblGroupName.AutoSize = true;
			this.lblGroupName.BackColor = System.Drawing.Color.Transparent;
			this.lblGroupName.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblGroupName.Location = new System.Drawing.Point(1096, 144);
			this.lblGroupName.Name = "lblGroupName";
			this.lblGroupName.Size = new System.Drawing.Size(99, 18);
			this.lblGroupName.TabIndex = 31;
			this.lblGroupName.Text = "Avi Manaker";
			// 
			// pictureBox3
			// 
			this.pictureBox3.Image = global::Edge.Application.ProductionManagmentTools.Properties.Resources.user_pic;
			this.pictureBox3.Location = new System.Drawing.Point(1096, 43);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(99, 98);
			this.pictureBox3.TabIndex = 30;
			this.pictureBox3.TabStop = false;
			// 
			// groupsView
			// 
			this.groupsView.BackgroundColor = System.Drawing.Color.LightGray;
			this.groupsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.groupsView.Location = new System.Drawing.Point(9, 43);
			this.groupsView.MultiSelect = false;
			this.groupsView.Name = "groupsView";
			this.groupsView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.groupsView.Size = new System.Drawing.Size(592, 534);
			this.groupsView.TabIndex = 29;
			this.groupsView.SelectionChanged += new System.EventHandler(this.groupsView_SelectionChanged);
			// 
			// txtFindGroup
			// 
			this.txtFindGroup.Location = new System.Drawing.Point(35, 8);
			this.txtFindGroup.Name = "txtFindGroup";
			this.txtFindGroup.Size = new System.Drawing.Size(127, 20);
			this.txtFindGroup.TabIndex = 28;
			this.txtFindGroup.TextChanged += new System.EventHandler(this.txtFindGroup_TextChanged);
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
			this.toolStripMenuAllow.Click += new System.EventHandler(this.toolStripMenu_Click);
			// 
			// toolStripMenuNotAllow
			// 
			this.toolStripMenuNotAllow.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuNotAllow.Image")));
			this.toolStripMenuNotAllow.Name = "toolStripMenuNotAllow";
			this.toolStripMenuNotAllow.Size = new System.Drawing.Size(140, 22);
			this.toolStripMenuNotAllow.Text = "Not Allowed";
			this.toolStripMenuNotAllow.Click += new System.EventHandler(this.toolStripMenu_Click);
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
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.membersGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupsView)).EndInit();
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
		private System.Windows.Forms.TreeView userPermissionTree;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblUserName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TreeView usersAccountsTreeView;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.DataGridView relatedGroupsView;
		private System.Windows.Forms.ContextMenuStrip contextMenuPermissions;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuAllow;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuNotAllow;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnAddUserRow;
		private System.Windows.Forms.Button btnDeleteUserRow;
		private System.Windows.Forms.Button btnChangePassword;
		private System.Windows.Forms.ToolTip toolTipChangePassword;
		private System.Windows.Forms.PictureBox pictureBox4;
		private System.Windows.Forms.Button btnAddGroupRow;
		private System.Windows.Forms.DataGridView membersGrid;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TreeView groupsAccountsTreeView;
		private System.Windows.Forms.TreeView groupsPermissionTree;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblGroupName;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.DataGridView groupsView;
		private System.Windows.Forms.TextBox txtFindGroup;
	}
}