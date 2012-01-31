using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Edge.Application.ProductionManagmentTools.Objects;
using Edge.Objects;

namespace Edge.Application.ProductionManagmentTools
{
	public partial class Permissions : Form
	{
		UsersGroupsDataLayer _usersGroupsDataLayer;
		List<UserView> _users;
		List<GroupView> _groups;
		UserView _selectedUser;
		GroupView _selectedGroup;
		List<RealatedGroupView> _relatedGroups;
		List<RealatedUserView> _relatedusers;
		Dictionary<string, PermissionView> _userPermissions = new Dictionary<string, PermissionView>();
		Dictionary<string, PermissionView> _groupPermissions = new Dictionary<string, PermissionView>();
		Edge.Objects.Account _selectedAccount;
		List<Edge.Objects.Account> _accounts;

		public Permissions()
		{
			InitializeComponent();
			_usersGroupsDataLayer = new UsersGroupsDataLayer();
			toolStripMenuAllow.Tag = PermissionAssignmentType.AllowFromUser;
			toolStripMenuNotAllow.Tag = PermissionAssignmentType.NotAllowFromUser;
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			Connect();
			GetAccounts();
			GetGroups();
			GetUsers();
			GetPermmissions();
		}

		private void GetPermmissions()
		{
			List<Permission> permissions = _usersGroupsDataLayer.GetPermissionTree();
			BuildUserPermissionsTree(permissions);
			BuildGroupPermissionsTree(permissions);

		}

		private void BuildUserPermissionsTree(TreeNode tn)
		{


			foreach (var permission in ((KeyValuePair<string, PermissionView>)tn.Tag).Value.ChildPermissions)
			{
				KeyValuePair<string, PermissionView> permissionView = new KeyValuePair<string, PermissionView>(permission.Path, permission);
				TreeNode tnn = new TreeNode(permissionView.Value.Name);
				tnn.Tag = permissionView;
				tnn.ContextMenuStrip = contextMenuPermissions;
				permissionView.Value.PermissionNode = tnn;
				tnn.ImageKey = "cancel_round.ico";
				tn.Nodes.Add(tnn);
				_userPermissions.Add(permissionView.Key, permissionView.Value);
				BuildUserPermissionsTree(tnn);

			}

		}

		private void BuildUserPermissionsTree(List<Permission> permissions)
		{

			userPermissionTree.ImageList = new ImageList();
			userPermissionTree.ImageList.Images.Add("question_blue.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\question_blue.ico"));
			userPermissionTree.ImageList.Images.Add("question_blue-faded.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\question_blue-faded.ico"));
			userPermissionTree.ImageList.Images.Add("tick_circle.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\tick_circle.ico"));
			userPermissionTree.ImageList.Images.Add("tick_circle-faded.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\tick_circle-faded.ico"));
			userPermissionTree.ImageList.Images.Add("cancel_round.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\cancel_round.ico"));
			userPermissionTree.ImageList.Images.Add("cancel_round-faded.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\cancel_round-faded.ico"));

			foreach (var permission in permissions)
			{


				KeyValuePair<string, PermissionView> permissionView = new KeyValuePair<string, PermissionView>(permission.Path, new PermissionView(permission));
				TreeNode tn = new TreeNode(permissionView.Value.Name);

				tn.ContextMenuStrip = contextMenuPermissions;
				tn.ImageKey = "question_blue.ico";
				tn.Tag = permissionView;
				permissionView.Value.PermissionNode = tn;
				userPermissionTree.Nodes.Add(tn);
				_userPermissions.Add(permissionView.Key, permissionView.Value);
				BuildUserPermissionsTree(tn);

			}
		}

		private void BuildGroupPermissionsTree(TreeNode tn)
		{


			foreach (var permission in ((KeyValuePair<string, PermissionView>)tn.Tag).Value.ChildPermissions)
			{
				KeyValuePair<string, PermissionView> permissionView = new KeyValuePair<string, PermissionView>(permission.Path, permission);
				TreeNode tnn = new TreeNode(permissionView.Value.Name);
				tnn.Tag = permissionView;
				tnn.ContextMenuStrip = contextMenuPermissions;
				permissionView.Value.PermissionNode = tnn;
				tnn.ImageKey = "cancel_round.ico";
				tn.Nodes.Add(tnn);
				_groupPermissions.Add(permissionView.Key, permissionView.Value);
				BuildGroupPermissionsTree(tnn);

			}

		}

		private void BuildGroupPermissionsTree(List<Permission> permissions)
		{

			groupsPermissionTree.ImageList = new ImageList();
			groupsPermissionTree.ImageList.Images.Add("question_blue.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\question_blue.ico"));
			groupsPermissionTree.ImageList.Images.Add("question_blue-faded.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\question_blue-faded.ico"));
			groupsPermissionTree.ImageList.Images.Add("tick_circle.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\tick_circle.ico"));
			groupsPermissionTree.ImageList.Images.Add("tick_circle-faded.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\tick_circle-faded.ico"));
			groupsPermissionTree.ImageList.Images.Add("cancel_round.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\cancel_round.ico"));
			groupsPermissionTree.ImageList.Images.Add("cancel_round-faded.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\cancel_round-faded.ico"));

			foreach (var permission in permissions)
			{


				KeyValuePair<string, PermissionView> permissionView = new KeyValuePair<string, PermissionView>(permission.Path, new PermissionView(permission));
				TreeNode tn = new TreeNode(permissionView.Value.Name);

				tn.ContextMenuStrip = contextMenuPermissions;
				tn.ImageKey = "question_blue.ico";
				tn.Tag = permissionView;
				permissionView.Value.PermissionNode = tn;
				groupsPermissionTree.Nodes.Add(tn);
				_groupPermissions.Add(permissionView.Key, permissionView.Value);
				BuildGroupPermissionsTree(tn);

			}
		}

		private void GetAccounts()
		{
			_accounts = _usersGroupsDataLayer.GetAccountsTree();
			BuildUserAccountsTree(null);
			BuildGroupAccountTree(null);
		}

		private void GetGroups()
		{
			_relatedGroups = new List<RealatedGroupView>();
			_groups = new List<GroupView>();
			foreach (var group in _usersGroupsDataLayer.GetGroups())
			{
				_relatedGroups.Add(new RealatedGroupView(group, false));
				_groups.Add(new GroupView(group));

			}

			groupsView.DataSource = _groups;
			relatedGroupsView.DataSource = _relatedGroups;
			relatedGroupsView.Columns["AssignedPermissions"].Visible = false;
			relatedGroupsView.Refresh();
			groupsView.DataSource = _groups;
			groupsView.Columns["AssignedPermissions"].Visible = false;
			groupsView.Refresh();

		}

		private void GetUsers()
		{
			_users = new List<UserView>();
			_relatedusers = new List<RealatedUserView>();
			foreach (var user in _usersGroupsDataLayer.GetUsers())
			{
				_relatedusers.Add(new RealatedUserView(user, false));
				_users.Add(new UserView(user));



			}
			membersGrid.DataSource = _relatedusers;
			membersGrid.Refresh();
			usersGrid.DataSource = _users;
			usersGrid.Columns["AssignedPermissions"].Visible = false;
			usersGrid.Refresh();
		}

		private void Connect()
		{
			_usersGroupsDataLayer.Login("doron@edge.bi", "123456");

		}

		private void usersGrid_SelectionChanged(object sender, EventArgs e)
		{

			if (usersGrid.SelectedRows.Count > 0)
			{
				_selectedUser = (UserView)usersGrid.SelectedRows[0].DataBoundItem;
				lblUserName.Text = _selectedUser.Name;
				Dictionary<int, RealatedGroupView> groups = _relatedGroups.ToDictionary(rg => rg.ID);
				groups.Values.Any(g => g.Assigned = false);
				foreach (var g in _selectedUser.Groups)
				{
					groups[g.GroupID].Assigned = true;

				}
				SetUserPermmisions();
				relatedGroupsView.Refresh();



			}
			else
			{
				if (_selectedUser != null)
				{
					_selectedUser = null;
				}
			}


		}

		private void SetUserPermmisions()
		{
			//reset permissions
			foreach (var permission in _userPermissions)
			{
				permission.Value.PermissionAssignmentType = PermissionAssignmentType.NotAssigned;
				
			}
			Dictionary<string, AssignedPermission> assignedPermissionsPerAccount = new Dictionary<string, AssignedPermission>();
			if (_selectedAccount != null && _selectedUser != null)
			{
				//Group Level
				foreach (Group groupMember in _selectedUser.Groups)
				{
					if (groupMember.IsAcountAdmin!=null && groupMember.IsAcountAdmin.Value)
					{
						BubbleDownUserPermission(null,true,PermissionFrom.isAccountAdmin);
					}
					else if(groupMember.AssignedPermissions.ContainsKey(_selectedAccount.ID.Value))
					{
						foreach (AssignedPermission groupPermission in groupMember.AssignedPermissions[_selectedAccount.ID.Value])
						{

							if (!assignedPermissionsPerAccount.ContainsKey(groupPermission.PermissionType))
								assignedPermissionsPerAccount.Add(groupPermission.PermissionType, groupPermission);
							else
							{
								if (groupPermission.Value == false)
									assignedPermissionsPerAccount[groupPermission.PermissionType].Value = false;
							}

						} 
					}
				}
				foreach (var assign in assignedPermissionsPerAccount)				
					BubbleDownUserPermission(_userPermissions[assign.Key],assign.Value.Value,PermissionFrom.Group);

				assignedPermissionsPerAccount = new Dictionary<string, AssignedPermission>();
				//User Level
				if (_selectedUser.IsAcountAdmin)
				{
					BubbleDownUserPermission(null, true, PermissionFrom.isAccountAdmin);
				}
				else if (_selectedUser.AssignedPermissions.ContainsKey(_selectedAccount.ID.Value))
				{
					foreach (var userAssignPermission in _selectedUser.AssignedPermissions[_selectedAccount.ID.Value])
					{
						if (!assignedPermissionsPerAccount.ContainsKey(userAssignPermission.PermissionType))
							assignedPermissionsPerAccount.Add(userAssignPermission.PermissionType, userAssignPermission);
						else
						{
							if (userAssignPermission.Value == false)
								assignedPermissionsPerAccount[userAssignPermission.PermissionType].Value = false;
						}
					}
				}

				foreach (var assign in assignedPermissionsPerAccount)
					BubbleDownUserPermission(_userPermissions[assign.Key], assign.Value.Value, PermissionFrom.User);

			}



		}
		private void BubbleDownUserPermission(PermissionView permissionView, bool value, PermissionFrom from)
		{

			if (from == PermissionFrom.Group || from == PermissionFrom.Bubble)
			{
				if (value)
					permissionView.PermissionAssignmentType = PermissionAssignmentType.AllowFromGroup;
				else
					permissionView.PermissionAssignmentType = PermissionAssignmentType.NotAllowFromGroup;
			}
			else if (from == PermissionFrom.User)
			{

				if (value)
					permissionView.PermissionAssignmentType = PermissionAssignmentType.AllowFromUser;
				else
					permissionView.PermissionAssignmentType = PermissionAssignmentType.NotAllowFromUser;

			}
			if (permissionView != null)
			{
				foreach (var child in permissionView.ChildPermissions)
				{
					BubbleDownUserPermission(child, value, PermissionFrom.Bubble);
				}
			}
			else //is account admin
			{
				foreach (var permission in _userPermissions)
				{
					permission.Value.PermissionAssignmentType = PermissionAssignmentType.AllowFromGroup;
					foreach (var child in permission.Value.ChildPermissions)
						BubbleDownUserPermission(child, true, PermissionFrom.Bubble);
				}

			}
		}

		private void BuildGroupAccountTree(TreeNode tn)
		{
			if (tn == null)
				BuildGroupAccountTree();
			else
			{
				foreach (var account in ((Edge.Objects.Account)tn.Tag).ChildAccounts.OrderBy(a => a.ID))
				{
					TreeNode tnn = new TreeNode(string.Format("{0}-{1}", account.ID, account.Name));
					tnn.Tag = account;
					tn.Nodes.Add(tnn);
					BuildUserAccountsTree(tnn);

				}
			}
		}

		private void BuildGroupAccountTree()
		{
			foreach (var account in _accounts.OrderBy(a => a.ID))
			{

				TreeNode tn = new TreeNode(string.Format("{0}-{1}", account.ID, account.Name));
				tn.Tag = account;
				groupsAccountsTreeView.Nodes.Add(tn);
				BuildGroupAccountTree(tn);

			}
		}
		private void BuildUserAccountsTree(TreeNode tn)
		{
			if (tn == null)
				BuildUserAccountsTree();
			else
			{
				foreach (var account in ((Edge.Objects.Account)tn.Tag).ChildAccounts.OrderBy(a => a.ID))
				{
					TreeNode tnn = new TreeNode(string.Format("{0}-{1}", account.ID, account.Name));
					tnn.Tag = account;
					tn.Nodes.Add(tnn);
					BuildUserAccountsTree(tnn);

				}
			}
		}

		private void BuildUserAccountsTree()
		{
			foreach (var account in _accounts.OrderBy(a => a.ID))
			{

				TreeNode tn = new TreeNode(string.Format("{0}-{1}", account.ID, account.Name));
				tn.Tag = account;
				usersAccountsTreeView.Nodes.Add(tn);
				BuildUserAccountsTree(tn);

			}
		}

		private void accountsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.IsSelected)
			{
				_selectedAccount = (Edge.Objects.Account)e.Node.Tag;

				SetUserPermmisions();
			}
		}

		private void relatedGroupsView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{

			bool assigned = (bool)relatedGroupsView[e.ColumnIndex, e.RowIndex].Value;
			RealatedGroupView group = (RealatedGroupView)relatedGroupsView.Rows[e.RowIndex].DataBoundItem;
			group.Assigned = assigned;
			if (assigned)
				_selectedUser.Groups.Add(group.GetGroup());
			else
				_selectedUser.Groups.RemoveAll(g => g.GroupID == group.ID);
			SetUserPermmisions();

		}

		private void relatedGroupsView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			relatedGroupsView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void toolStripMenu_Click(object sender, EventArgs e)
		{
			if (tabControl1.SelectedIndex == 0)			
				AddUserPermission(sender);
			else
				AddGroupPermission(sender);
			

		}

		private void AddGroupPermission(object sender)
		{
			PermissionAssignmentType permissionType = (PermissionAssignmentType)((ToolStripMenuItem)sender).Tag;
			PermissionView p = (PermissionView)((KeyValuePair<string, PermissionView>)groupsPermissionTree.SelectedNode.Tag).Value;
			groupsPermissionTree.BeginUpdate();
			bool allow;
			if (permissionType == PermissionAssignmentType.AllowFromUser)
				allow = true;
			else
				allow = false;
			BubbleDownGroupPermission(p, allow, PermissionFrom.User);

			groupsPermissionTree.EndUpdate();
			groupsPermissionTree.Refresh();
			if (_selectedAccount != null && _selectedGroup != null)
			{
				if (!_selectedGroup.AssignedPermissions.ContainsKey(_selectedAccount.ID.Value))
					_selectedGroup.AssignedPermissions.Add(_selectedAccount.ID.Value, new List<AssignedPermission>());

				AssignedPermission current = null;

				foreach (var assignedPermission in _selectedGroup.AssignedPermissions[_selectedAccount.ID.Value])
				{

					if (assignedPermission.PermissionType == p.Path)
					{
						current = assignedPermission;
						break;
					}
				}
				if (current == null)
				{
					current = new AssignedPermission() { PermissionName = p.Name, PermissionType = p.Path, Value = allow };
					_selectedGroup.AssignedPermissions[_selectedAccount.ID.Value].Add(current);
				}
				else
					current.Value = allow;
			}
		}

		private void AddUserPermission(object sender)
		{
			PermissionAssignmentType permissionType = (PermissionAssignmentType)((ToolStripMenuItem)sender).Tag;
			PermissionView p = (PermissionView)((KeyValuePair<string, PermissionView>)userPermissionTree.SelectedNode.Tag).Value;
			userPermissionTree.BeginUpdate();
			bool allow;
			if (permissionType == PermissionAssignmentType.AllowFromUser)
				allow = true;
			else
				allow = false;
			BubbleDownUserPermission(p, allow, PermissionFrom.User);

			userPermissionTree.EndUpdate();
			userPermissionTree.Refresh();
			if (_selectedAccount != null && _selectedUser != null)
			{
				if (!_selectedUser.AssignedPermissions.ContainsKey(_selectedAccount.ID.Value))
					_selectedUser.AssignedPermissions.Add(_selectedAccount.ID.Value, new List<AssignedPermission>());

				AssignedPermission current = null;

				foreach (var assignedPermission in _selectedUser.AssignedPermissions[_selectedAccount.ID.Value])
				{

					if (assignedPermission.PermissionType == p.Path)
					{
						current = assignedPermission;
						break;
					}
				}
				if (current == null)
				{
					current = new AssignedPermission() { PermissionName = p.Name, PermissionType = p.Path, Value = allow };
					_selectedUser.AssignedPermissions[_selectedAccount.ID.Value].Add(current);
				}
				else
					current.Value = allow;
			}
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			try
			{
				if (tabControl1.SelectedIndex == 0)
					_usersGroupsDataLayer.SaveUser(_selectedUser);
				else
					_usersGroupsDataLayer.SaveGroup(_selectedGroup);
				MessageBox.Show("Saved");
			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.Message);
			}

		}

		private void btnAddRow_Click(object sender, EventArgs e)
		{
			User u = new User() { Name = "Please enter User Name", Email = "Please enter email", IsAcountAdmin = false, Password = "fsdbabfgsdjabfgj3G" };
			_users.Add(new UserView(u) { IsNew = true });
			usersGrid.DataSource = null;
			usersGrid.DataSource = _users;
			usersGrid.Refresh();
			usersGrid.CurrentCell = usersGrid.Rows[usersGrid.Rows.Count - 1].Cells[1];
		}

		private void usersGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow row = usersGrid.Rows[e.RowIndex];
			row.HeaderCell.Value = "*";
			UserView u = (UserView)row.DataBoundItem;
			u.Changed = true;


		}

		private void btnUserDeleteRow_Click(object sender, EventArgs e)
		{
			if (usersGrid.SelectedRows[0] != null)
			{
				if (_selectedUser != null)
				{

					DialogResult dialogResult = MessageBox.Show(string.Format("Are you sure you want to delete the user {0}", _selectedUser.Name), "Delete approval", MessageBoxButtons.YesNo);
					if (dialogResult == System.Windows.Forms.DialogResult.Yes)
					{
						try
						{
							_usersGroupsDataLayer.DeleteUser(_selectedUser);
							_users.Remove(_selectedUser);
							usersGrid.DataSource = _users;
							usersGrid.Refresh();
							MessageBox.Show("User Deleted!");
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.Message);
						}
					}
				}

			}

		}

		private void btnChangePassword_Click(object sender, EventArgs e)
		{
			ChangePasswordForm f = new ChangePasswordForm(_usersGroupsDataLayer, _selectedUser.ID);
			f.Show();
		}

		private void Permissions_Load(object sender, EventArgs e)
		{
			toolTipChangePassword.SetToolTip(btnChangePassword, "Change Paasword");
		}

		private void txtFindUser_TextChanged(object sender, EventArgs e)
		{
			string value = txtFindUser.Text;
			foreach (DataGridViewRow row in usersGrid.Rows)
			{
				if (row.Cells[1].Value.ToString().ToUpper().Trim().StartsWith(value.ToUpper().Trim()))
				{
					row.Selected = true;
					break;
				}


			}

		}

		private void txtFindGroup_TextChanged(object sender, EventArgs e)
		{
			string value = txtFindGroup.Text;
			foreach (DataGridViewRow row in groupsView.Rows)
			{
				if (row.Cells[1].Value.ToString().ToUpper().Trim().StartsWith(value.ToUpper().Trim()))
				{
					row.Selected = true;
					break;
				}


			}
		}

		private void btnAddGroupRow_Click(object sender, EventArgs e)
		{
			Group g = new Group() { Name = "Please enter User Name", IsAcountAdmin = false };
			_groups.Add(new GroupView(g) { IsNew = true });
			groupsView.DataSource = null;
			groupsView.DataSource = _groups;
			groupsView.Refresh();
			groupsView.CurrentCell = groupsView.Rows[groupsView.Rows.Count - 1].Cells[1];
		}
		private void groupsView_SelectionChanged(object sender, EventArgs e)
		{
			if (groupsView.SelectedRows.Count > 0)
			{
				_selectedGroup = (GroupView)groupsView.SelectedRows[0].DataBoundItem;
				lblGroupName.Text = _selectedGroup.Name;
				Dictionary<int, RealatedUserView> members = _relatedusers.ToDictionary(ru => ru.ID);
				members.Values.Any(g => g.Assigned = false);
				foreach (var m in _selectedGroup.Members)
				{
					members[m.UserID].Assigned = true;

				}
				membersGrid.DataSource = _relatedusers;
				membersGrid.Refresh();
				SetGroupPermmisions();



			}
			else
			{
				if (_selectedUser != null)
				{
					_selectedUser = null;
				}
			}
		}



		private void SetGroupPermmisions()
		{
			//reset permissions
			foreach (var permission in _groupPermissions)
			{
				permission.Value.PermissionAssignmentType = PermissionAssignmentType.NotAssigned;

			}
			Dictionary<string, AssignedPermission> assignedPermissionsPerAccount = new Dictionary<string, AssignedPermission>();
			if (_selectedAccount != null && _selectedGroup != null)
			{


				assignedPermissionsPerAccount = new Dictionary<string, AssignedPermission>();
				//group Level
				if (_selectedGroup.IsAccountAdmin)
				{
					BubbleDownGroupPermission(null, true, PermissionFrom.isAccountAdmin);
				}
				else if (_selectedGroup.AssignedPermissions.ContainsKey(_selectedAccount.ID.Value))
				{
					foreach (var groupAssignPermission in _selectedGroup.AssignedPermissions[_selectedAccount.ID.Value])
					{
						if (!assignedPermissionsPerAccount.ContainsKey(groupAssignPermission.PermissionType))
							assignedPermissionsPerAccount.Add(groupAssignPermission.PermissionType, groupAssignPermission);
						else
						{
							if (groupAssignPermission.Value == false)
								assignedPermissionsPerAccount[groupAssignPermission.PermissionType].Value = false;
						}
					}
				}

				foreach (var assign in assignedPermissionsPerAccount)
					BubbleDownGroupPermission(_groupPermissions[assign.Key], assign.Value.Value, PermissionFrom.User);

			}
		}

		private void BubbleDownGroupPermission(PermissionView permissionView, bool value, PermissionFrom from)
		{
			if (from == PermissionFrom.Group || from == PermissionFrom.Bubble)
			{
				if (value)
					permissionView.PermissionAssignmentType = PermissionAssignmentType.AllowFromGroup;
				else
					permissionView.PermissionAssignmentType = PermissionAssignmentType.NotAllowFromGroup;
			}
			else if (from==PermissionFrom.User)
			{

				if (value)
					permissionView.PermissionAssignmentType = PermissionAssignmentType.AllowFromUser;
				else
					permissionView.PermissionAssignmentType = PermissionAssignmentType.NotAllowFromUser;

			}
			if (permissionView != null)
			{
				foreach (var child in permissionView.ChildPermissions)
				{
					BubbleDownGroupPermission(child, value, PermissionFrom.Bubble);
				}
			}
			else //isacount admin
			{
				foreach (var permission in _groupPermissions)
				{
					permission.Value.PermissionAssignmentType = PermissionAssignmentType.AllowFromGroup;
					foreach (var child in permission.Value.ChildPermissions)
						BubbleDownGroupPermission(child, true, PermissionFrom.Bubble);
				}

			}
		}

		private void groupsAccountsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.IsSelected)
			{
				_selectedAccount = (Edge.Objects.Account)e.Node.Tag;

				SetGroupPermmisions();
			}
		}






	}
	public class UserView
	{
		User _inner;
		public UserView(User user)
		{
			_inner = user;
		}
		public int ID
		{
			get
			{
				return _inner.UserID;
			}
		}
		public string Name
		{
			get
			{
				return _inner.Name;
			}
			set
			{
				_inner.Name = value;

			}
		}

		public bool IsAcountAdmin
		{
			get
			{
				return _inner.IsAcountAdmin;
			}
			set
			{
				_inner.IsAcountAdmin = value;

			}
		}
		public bool IsNew;
		public List<Group> Groups
		{
			get
			{
				return _inner.AssignedToGroups;
			}

		}
		public Dictionary<int, List<AssignedPermission>> AssignedPermissions
		{
			get
			{
				return _inner.AssignedPermissions;
			}

		}
		public bool Changed;







		internal object GetUser()
		{
			return _inner;
		}
	}
	public class RealatedGroupView
	{
		Group _inner;

		public RealatedGroupView(Group group, bool assigned)
		{
			_inner = group;
			Assigned = assigned;



		}

		public int ID
		{
			get
			{
				return _inner.GroupID;
			}


		}

		public bool Assigned { get; set; }
		public string Name
		{
			get
			{
				return _inner.Name;
			}

		}
		internal Group GetGroup()
		{
			return _inner;
		}
		public Dictionary<int, List<AssignedPermission>> AssignedPermissions
		{
			get
			{
				return _inner.AssignedPermissions;
			}

		}

	}
	public class RealatedUserView
	{
		User _inner;

		public RealatedUserView(User user, bool assigned)
		{
			_inner = user;
			Assigned = assigned;



		}

		public int ID
		{
			get
			{
				return _inner.UserID;
			}


		}

		public bool Assigned { get; set; }
		public string Name
		{
			get
			{
				return _inner.Name;
			}

		}
		internal User GetUser()
		{
			return _inner;
		}
		public Dictionary<int, List<AssignedPermission>> AssignedPermissions
		{
			get
			{
				return _inner.AssignedPermissions;
			}

		}

	}
	public class PermissionView
	{
		Permission _inner;
		public string Name;
		public string Path;
		PermissionAssignmentType _permissionAssignmentType;
		public PermissionAssignmentType PermissionAssignmentType
		{
			get
			{
				return _permissionAssignmentType;
			}
			set
			{
				_permissionAssignmentType = value;
				switch (_permissionAssignmentType)
				{
					case PermissionAssignmentType.AllowFromGroup:
						{
							this.PermissionNode.SelectedImageKey = "tick_circle-faded.ico";
							this.PermissionNode.ImageKey = "tick_circle-faded.ico";
							this.PermissionNode.StateImageKey = "tick_circle-faded.ico";
							break;
						}
					case PermissionAssignmentType.AllowFromUser:
						{
							this.PermissionNode.ImageKey = "tick_circle.ico";
							this.PermissionNode.SelectedImageKey = "tick_circle.ico";
							this.PermissionNode.StateImageKey = "tick_circle.ico";
							break;
						}
					case PermissionAssignmentType.NotAllowFromGroup:
						{
							this.PermissionNode.ImageKey = "cancel_round-faded.ico";
							this.PermissionNode.SelectedImageKey = "cancel_round-faded.ico";
							this.PermissionNode.StateImageKey = "cancel_round-faded.ico";
							break;
						}
					case PermissionAssignmentType.NotAllowFromUser:
						{
							this.PermissionNode.ImageKey = "cancel_round.ico";
							this.PermissionNode.SelectedImageKey = "cancel_round.ico";
							this.PermissionNode.StateImageKey = "cancel_round.ico";
							break;
						}
					case PermissionAssignmentType.NotAssigned:
						{
							this.PermissionNode.ImageKey = "question_blue.ico";
							this.PermissionNode.SelectedImageKey = "question_blue.ico";
							this.PermissionNode.StateImageKey = "question_blue.ico";
							break;
						}
					default:
						break;
				}
			}



		}
		public TreeNode PermissionNode;
		public List<PermissionView> ChildPermissions = new List<PermissionView>();


		public PermissionView(Permission permission)
		{
			_inner = permission;

			Name = permission.PermissionName;
			Path = permission.Path;
			foreach (var childPermission in permission.ChildPermissions)
				ChildPermissions.Add(new PermissionView(childPermission));

		}


	}
	public enum PermissionAssignmentType
	{
		NotAssigned = 0,
		AllowFromGroup,
		AllowFromUser,
		NotAllowFromGroup,
		NotAllowFromUser

	}
	public class GroupView
	{
		Edge.Objects.Group _inner;
		public GroupView(Edge.Objects.Group group)
		{
			_inner = group;


		}
		public int ID
		{
			get { return (int)_inner.GroupID; }

		}
		public string Name
		{
			get { return _inner.Name; }
			set { _inner.Name = value; }
		}
		public bool IsAccountAdmin
		{
			get { return _inner.IsAcountAdmin.Value; }
			set { _inner.IsAcountAdmin = value; }
		}
		public bool IsNew;
		public Dictionary<int, List<AssignedPermission>> AssignedPermissions
		{
			get { return _inner.AssignedPermissions; }

		}
		public List<User> Members
		{
			get { return _inner.Members; }

		}







		internal void Remove(GroupView _selectedGroup)
		{
			throw new NotImplementedException();
		}

		internal object GetGroup()
		{
			return _inner;
		}
	}
	public enum PermissionFrom
	{
		Group,
		User,
		Bubble,
		isAccountAdmin,
	}



}
