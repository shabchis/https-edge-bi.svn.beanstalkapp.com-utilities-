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
		Edge.Objects.Account _selectedAccount;
		List<Edge.Objects.Account> _accounts;
		public Permissions()
		{
			InitializeComponent();
			_usersGroupsDataLayer = new UsersGroupsDataLayer();
		}

		private void pictureBox5_Click(object sender, EventArgs e)
		{

		}

		private void pictureBox6_Click(object sender, EventArgs e)
		{

		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			Connect();
			GetUsers();
			GetGroups();
			GetAccounts();
			relatedGroupsView.DataSource = _relatedGroups;
			relatedGroupsView.Refresh();
			usersGrid.DataSource = _users;
			usersGrid.Refresh();
			
			
		}

		private void GetAccounts()
		{
			_accounts = _usersGroupsDataLayer.GetAccountsTree();
			SetAccountsTree(null);
		}

		private void GetGroups()
		{
			_relatedGroups = new List<RealatedGroupView>();
			foreach (var group in _usersGroupsDataLayer.GetGroups())
			{
				_relatedGroups.Add(new RealatedGroupView(group,false));
				
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

		private void SetAccountsTree(TreeNode tn)
		{
			if (tn == null)
				SetAccountsTree();
			else
			{
				foreach (var account in ((Edge.Objects.Account)tn.Tag).ChildAccounts)
				{
					TreeNode tnn = new TreeNode(string.Format("{0}-{1}",account.ID, account.Name));
					tnn.Tag = account;
					tn.Nodes.Add(tnn);
					SetAccountsTree(tnn);
					
				}
			}










		}

		private void SetAccountsTree()
		{
			foreach (var account in _accounts)
			{
				
				TreeNode tn=new TreeNode(string.Format("{0}-{1}",account.ID,account.Name));
				tn.Tag=account;
				accountsTreeView.Nodes.Add(tn);
				SetAccountsTree(tn);
				
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
			_inner = user;
			


		}
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
		

	}
	public class RealatedGroupView
	{
		Group _inner;
		
		public RealatedGroupView(Group group,bool assigned)
		{
			_inner = group;
			this.ID = _inner.GroupID;
			this.Name = _inner.Name;
			Assigned = assigned; 

			

		}

		public int ID;
		public bool Assigned { get; set; }
		public string Name { get; set; }
		
	}
	


}
