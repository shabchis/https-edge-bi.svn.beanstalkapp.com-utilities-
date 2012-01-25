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
		UserView _selectedUser;
		List<RealatedGroupView> _relatedGroups;
		Dictionary<string, PermissionView> _permissions = new Dictionary<string, PermissionView>();
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
			BuildPermissionsTree(permissions);

		}

		private void BuildPermissionsTree(TreeNode tn)
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
				_permissions.Add(permissionView.Key, permissionView.Value);
				BuildPermissionsTree(tnn);

			}

		}

		private void BuildPermissionsTree(List<Permission> permissions)
		{

			permissionTree.ImageList = new ImageList();
			permissionTree.ImageList.Images.Add("question_blue.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\question_blue.ico"));
			permissionTree.ImageList.Images.Add("question_blue-faded.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\question_blue-faded.ico"));
			permissionTree.ImageList.Images.Add("tick_circle.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\tick_circle.ico"));
			permissionTree.ImageList.Images.Add("tick_circle-faded.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\tick_circle-faded.ico"));
			permissionTree.ImageList.Images.Add("cancel_round.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\cancel_round.ico"));
			permissionTree.ImageList.Images.Add("cancel_round-faded.ico", Image.FromFile(@"D:\Edge\Codebase\2.9\Utilities\ProductionManagmentTools\ProductionManagmentTools\Resources\cancel_round-faded.ico"));

			foreach (var permission in permissions)
			{


				KeyValuePair<string, PermissionView> permissionView = new KeyValuePair<string, PermissionView>(permission.Path, new PermissionView(permission));
				TreeNode tn = new TreeNode(permissionView.Value.Name);
				
				tn.ContextMenuStrip = contextMenuPermissions;
				tn.ImageKey = "question_blue.ico";
				tn.Tag = permissionView;
				permissionView.Value.PermissionNode = tn;
				permissionTree.Nodes.Add(tn);
				_permissions.Add(permissionView.Key, permissionView.Value);
				BuildPermissionsTree(tn);

			}
		}

		private void GetAccounts()
		{
			_accounts = _usersGroupsDataLayer.GetAccountsTree();
			BuildAccountsTree(null);
		}

		private void GetGroups()
		{
			_relatedGroups = new List<RealatedGroupView>();
			foreach (var group in _usersGroupsDataLayer.GetGroups())
			{
				_relatedGroups.Add(new RealatedGroupView(group, false));

			}
			relatedGroupsView.DataSource = _relatedGroups;
			relatedGroupsView.Columns["AssignedPermissions"].Visible = false;
			relatedGroupsView.Refresh();
		
		}

		private void GetUsers()
		{
			_users = new List<UserView>();
			foreach (var user in _usersGroupsDataLayer.GetUsers())
			{
				_users.Add(new UserView(user));

			}
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
				SetPermmisions();
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

		private void SetPermmisions()
		{
			//reset permissions
			foreach (var permission in _permissions)
			{
				permission.Value.PermissionAssignmentType = PermissionAssignmentType.NotAssigned;
				
			}
			Dictionary<string, AssignedPermission> assignedPermissionsPerAccount = new Dictionary<string, AssignedPermission>();
			if (_selectedAccount != null && _selectedUser != null)
			{
				//Group Level
				foreach (Group groupMember in _selectedUser.Groups)
				{
					if (groupMember.AssignedPermissions.ContainsKey(_selectedAccount.ID.Value))
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
					BubbleDownPermission(_permissions[assign.Key],assign.Value.Value,PermissionFrom.Group);

				assignedPermissionsPerAccount = new Dictionary<string, AssignedPermission>();
				//User Level
				if (_selectedUser.AssignedPermissions.ContainsKey(_selectedAccount.ID.Value))
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
					BubbleDownPermission(_permissions[assign.Key], assign.Value.Value, PermissionFrom.User);

			}



		}

		private void BubbleDownPermission(PermissionView permissionView, bool value, PermissionFrom from)
		{
			if (from == PermissionFrom.Group || from == PermissionFrom.Bubble)
			{
				if (value)
					permissionView.PermissionAssignmentType = PermissionAssignmentType.AllowFromGroup;
				else
					permissionView.PermissionAssignmentType = PermissionAssignmentType.NotAllowFromGroup;
			}
			else
			{

				if (value)
					permissionView.PermissionAssignmentType = PermissionAssignmentType.AllowFromUser;
				else
					permissionView.PermissionAssignmentType = PermissionAssignmentType.NotAllowFromUser;

			}
			foreach (var child in permissionView.ChildPermissions)
			{
				BubbleDownPermission(child, value, PermissionFrom.Bubble);
			}
		}

		private void BuildAccountsTree(TreeNode tn)
		{
			if (tn == null)
				BuildAccountsTree();
			else
			{
				foreach (var account in ((Edge.Objects.Account)tn.Tag).ChildAccounts.OrderBy(a => a.ID))
				{
					TreeNode tnn = new TreeNode(string.Format("{0}-{1}", account.ID, account.Name));
					tnn.Tag = account;
					tn.Nodes.Add(tnn);
					BuildAccountsTree(tnn);

				}
			}










		}

		private void BuildAccountsTree()
		{
			foreach (var account in _accounts.OrderBy(a => a.ID))
			{

				TreeNode tn = new TreeNode(string.Format("{0}-{1}", account.ID, account.Name));
				tn.Tag = account;
				accountsTreeView.Nodes.Add(tn);
				BuildAccountsTree(tn);

			}
		}

		private void accountsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.IsSelected)
			{
				_selectedAccount = (Edge.Objects.Account)e.Node.Tag;
				
				SetPermmisions();
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
			SetPermmisions();

		}

		private void relatedGroupsView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			relatedGroupsView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void toolStripMenuAllow_Click(object sender, EventArgs e)
		{
			PermissionAssignmentType permissionType = (PermissionAssignmentType)((ToolStripMenuItem)sender).Tag;
			PermissionView p = (PermissionView)((KeyValuePair<string, PermissionView>)permissionTree.SelectedNode.Tag).Value;
			permissionTree.BeginUpdate();
			bool allow;
			if (permissionType == PermissionAssignmentType.AllowFromUser)
					allow = true;
				else
					allow = false;
				BubbleDownPermission(p, allow, PermissionFrom.User);

				permissionTree.EndUpdate();
			permissionTree.Refresh();
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
				_usersGroupsDataLayer.SaveUser(_selectedUser);
				MessageBox.Show("Saved");
			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.Message);
			}

		}

		private void btnAddRow_Click(object sender, EventArgs e)
		{
			User u = new User() { Name = "Please enter User Name", Email = "Please enter email", IsAcountAdmin = false,Password="fsdbabfgsdjabfgj3G"};
			_users.Add(new UserView(u) { IsNew = true });
			usersGrid.DataSource = null;
			usersGrid.DataSource = _users;
			usersGrid.Refresh();
			usersGrid.CurrentCell = usersGrid.Rows[usersGrid.Rows.Count - 1].Cells[1];
		}

		private void usersGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow row= usersGrid.Rows[e.RowIndex];
			row.HeaderCell.Value="*";
			UserView u = (UserView)row.DataBoundItem;
			u.Changed = true;
		

		}

		private void btnDeleteRow_Click(object sender, EventArgs e)
		{
			if (usersGrid.SelectedRows[0] != null)
			{
				if (_selectedUser != null)
				{
					
					DialogResult dialogResult= MessageBox.Show(string.Format("Are you sure you want to delete the user {0}",_selectedUser.Name),"Delete approval",MessageBoxButtons.YesNo);
					if (dialogResult==System.Windows.Forms.DialogResult.Yes)
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
			ChangePasswordForm f = new ChangePasswordForm(_usersGroupsDataLayer,_selectedUser.ID);
			f.Show();
		}

		private void Permissions_Load(object sender, EventArgs e)
		{
			toolTipChangePassword.SetToolTip(btnChangePassword, "Change Paasword");
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
		public string Email
		{
			get
			{
				return _inner.Email;
			}
			set
			{

				_inner.Email = value;
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
		public bool IsActive
		{
			get
			{
				return _inner.IsActive;
			}
			set
			{
				_inner.IsActive = value;

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
		NotAssigned=0,
		AllowFromGroup,
		AllowFromUser,
		NotAllowFromGroup,
		NotAllowFromUser
		
	}
	public enum PermissionFrom
	{
		Group,
		User,
		Bubble
	}



}
