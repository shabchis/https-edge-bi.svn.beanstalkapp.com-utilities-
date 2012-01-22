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
		}

	

		private void btnConnect_Click(object sender, EventArgs e)
		{
			Connect();
			GetUsers();
			GetGroups();
			GetAccounts();
			GetPermmissions();
			relatedGroupsView.DataSource = _relatedGroups;
			relatedGroupsView.Refresh();
			usersGrid.DataSource = _users;
			usersGrid.Refresh();


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
		}

		private void GetUsers()
		{
			_users = new List<UserView>();
			foreach (var user in _usersGroupsDataLayer.GetUsers())
			{
				_users.Add(new UserView(user));

			}
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
			if (_selectedUser!=null && _selectedUser.CalculatedPermissions == null && _selectedUser.Changed == false)
			{
				//Get new calculated permission
				_selectedUser.CalculatedPermissions = _usersGroupsDataLayer.GetCalculatedPermissions(_selectedUser.ID);
			}
			//Clear  permissions tree
			foreach (var permission in _permissions)
				permission.Value.SetAssignmentType(PermissionAssignmentType.NotAssigned, false);
			if (_selectedAccount != null && _selectedUser!=null)
			{
				if (_selectedUser.CalculatedPermissions.ContainsKey(_selectedAccount.ID.Value))
				{
					foreach (var calc in _selectedUser.CalculatedPermissions[_selectedAccount.ID.Value])
						_permissions[calc.Path].SetAssignmentType(PermissionAssignmentType.Allow, true);
				}
				if (_selectedUser.AssignedPermissions.ContainsKey(_selectedAccount.ID.Value))
				{
					foreach (var assigned in _selectedUser.AssignedPermissions[_selectedAccount.ID.Value])
					{
						if (assigned.Value == true)
							_permissions[assigned.PermissionType].SetAssignmentType(PermissionAssignmentType.Allow, false);
						else
							_permissions[assigned.PermissionType].SetAssignmentType(PermissionAssignmentType.Allow, true);


						
					}
				}


			}
			permissionTree.Refresh();





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
			
			bool assigned=(bool)relatedGroupsView[e.ColumnIndex, e.RowIndex].Value;
			RealatedGroupView group = (RealatedGroupView)relatedGroupsView.Rows[e.RowIndex].DataBoundItem;
			group.Assigned = assigned;
			if (assigned)
				_selectedUser.Groups.Add(group.GetGroup());
			else
				_selectedUser.Groups.Remove(group.GetGroup());

		}

		private void relatedGroupsView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			relatedGroupsView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void toolStripMenuAllow_Click(object sender, EventArgs e)
		{
			TreeNode t = (TreeNode)sender;
			PermissionView p = (PermissionView)t.Tag;
			p.SetAssignmentType(PermissionAssignmentType.Allow, false);
			if (_selectedAccount != null && _selectedUser != null)
			{
				if (!_selectedUser.AssignedPermissions.ContainsKey(_selectedAccount.ID.Value))
					_selectedUser.AssignedPermissions.Add(_selectedAccount.ID.Value, new List<AssignedPermission>());
				

			}

		}

		

		

		
	}
	public class UserView
	{
		User _inner;
		public UserView(User user)
		{
			this.Email = user.Email;
			this.IsAcountAdmin = user.IsAcountAdmin;
			this.Name = user.Name;
			this.IsActive = user.IsActive;
			this.ID = user.UserID;
			this.AssignedPermissions = user.AssignedPermissions;
			_inner = user;
			
			





		}
		public int ID;
		public string Name { get; set; }
		public string Email { get; set; }
		public bool IsAcountAdmin { get; set; }
		public bool IsActive { get; set; }
		public List<Group> Groups
		{
			get
			{
				return _inner.AssignedToGroups;
			}

		}
		public Dictionary<int, List<CalculatedPermission>> CalculatedPermissions = null;
		public Dictionary<int, List<AssignedPermission>> AssignedPermissions;
		
		public bool Changed;






	}
	public class RealatedGroupView
	{
		Group _inner;

		public RealatedGroupView(Group group, bool assigned)
		{
			_inner = group;
			this.ID = _inner.GroupID;
			this.Name = _inner.Name;
			Assigned = assigned;



		}

		public int ID;
		public bool Assigned { get; set; }
		public string Name { get; set; }


		internal Group GetGroup()
		{
			return _inner;
		}
	}
	public class PermissionView
	{
		Permission _inner;
		public string Name;
		public string Path;
		PermissionAssignmentType _permissionAssignmentType;

		bool IsCalc;
		public PermissionAssignmentType PermissionAssignmentType
		{
			get
			{
				return _permissionAssignmentType;
			}
			private set { }
			


		}
		public TreeNode PermissionNode;
		public List<PermissionView> ChildPermissions = new List<PermissionView>();


		public PermissionView(Permission permission)
		{
			_inner = permission;
			
			Name = permission.PermissionName;
			Path = permission.Path;
			PermissionAssignmentType = ProductionManagmentTools.PermissionAssignmentType.NotAssigned;
			foreach (var childPermission in permission.ChildPermissions)
				ChildPermissions.Add(new PermissionView(childPermission));

		}
		public void SetAssignmentType(PermissionAssignmentType perType, bool IsCalc)
		{
			_permissionAssignmentType = perType;
			if (PermissionNode != null)
			{
				switch (perType)
				{
					case ProductionManagmentTools.PermissionAssignmentType.Allow:
						{
							if (!IsCalc)
							{
								this.PermissionNode.ImageKey = "tick_circle.ico";
								this.PermissionNode.SelectedImageKey = "tick_circle.ico";
							}
							else
							{
								this.PermissionNode.ImageKey = "tick_circle-faded.ico";
								this.PermissionNode.SelectedImageKey = "tick_circle-faded.ico";
							}

							break;
						}

					case ProductionManagmentTools.PermissionAssignmentType.NotAssigned:
						{
							if (!IsCalc)
							{
								this.PermissionNode.ImageKey = "question_blue.ico";
								this.PermissionNode.SelectedImageKey = "question_blue.ico";
							}

							else
							{
								this.PermissionNode.ImageKey = "question_blue-faded.ico";
								this.PermissionNode.SelectedImageKey = "question_blue-faded.ico";
							}
							break;
						}
					case ProductionManagmentTools.PermissionAssignmentType.NotAllow:
						{
							if (!IsCalc)
							this.PermissionNode.ImageKey = "cancel_round.ico";
							else
								this.PermissionNode.ImageKey = "cancel_round-faded.ico";
							break;
						}

				}
			}

		}

	}
	public enum PermissionAssignmentType
	{
		NotAssigned,
		Allow,
		NotAllow

	}



}
