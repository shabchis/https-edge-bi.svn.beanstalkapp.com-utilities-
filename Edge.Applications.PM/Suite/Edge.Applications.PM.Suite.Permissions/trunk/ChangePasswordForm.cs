using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Edge.Applications.PM.Suite.Permissions.Objects;
using Edge.Applications.PM.Common;

namespace Edge.Applications.PM.Suite.Permissions
{
	public partial class ChangePasswordForm : ProductionManagmentBaseForm
	{
		int _userID;
		UsersGroupsDataLayer _dataLayer;
		public ChangePasswordForm(UsersGroupsDataLayer dataLayer,int userID)
		{
			_userID = userID;
			_dataLayer = dataLayer;
			InitializeComponent();
		}

		private void btnChangePassword_Click(object sender, EventArgs e)
		{
			if (txtPassword.Text != txtValidatePassword.Text)
				MessageBox.Show("Passwords are not the same!");
			else
			{
				string password = Edge.Core.Utilities.Encryptor.Enc(txtPassword.Text);
				
				_dataLayer.ChangePasswords(_userID,password);

			}
		}
	}
}
