using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Configuration;
using Edge.Applications.PM;
using System.IO;
using System.Reflection;
using Edge.Applications.PM.Client.Configuration;
using Edge.Applications.PM.Common;

namespace Edge.Applications.PM.Client
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();
			LoadMenuFromConfiguration();
		}

		private void LoadMenuFromConfiguration()
		{
			MenuSection menu = (MenuSection)ConfigurationManager.GetSection("Menu");
			
			foreach (Edge.Applications.PM.Client.Configuration.MenuItem item in menu.MenuItems)
			{
				ToolStripMenuItem menuItem = new ToolStripMenuItem(item.Name, null);
				
				//If Top Menu
				if (item.MenuItems.Count > 0)
				{
					foreach (Edge.Applications.PM.Client.Configuration.MenuItem subItem in item.MenuItems)
					{
						ToolStripMenuItem subMenuItem = new ToolStripMenuItem(subItem.Name, null, new EventHandler(ToolStripMenuItem_Click));
						subMenuItem.Tag = subItem;
						menuItem.DropDownItems.Add(subMenuItem);
					}
				}
				else
					menuItem.Click += new EventHandler(ToolStripMenuItem_Click);

				menuItem.Tag = item;
				this.menuStrip.Items.Add(menuItem);
			}
		}

		private void ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Edge.Applications.PM.Client.Configuration.MenuItem menuItem =
				(Edge.Applications.PM.Client.Configuration.MenuItem) ((ToolStripMenuItem)sender).Tag;

			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase= Directory.GetCurrentDirectory();

			AppDomain _appDomain = AppDomain.CreateDomain(this.ToString(), null, setup);

			_appDomain.CreateInstance(
				Assembly.GetExecutingAssembly().FullName,
				typeof(LoadFormHelper).FullName, false, BindingFlags.Default, null, new object[] { menuItem.Name, menuItem.Class }, null, null);
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Form a = new Form();
			a.Show();
		}


	}

	public class LoadFormHelper
	{
		public LoadFormHelper(string formName, string className )
		{
			//Type ty = typeof(Edge.Applications.PM.Suite.DeliverySearch.DeliverySearch);
			//Type formType = Type.GetType("Edge.Applications.PM.Suite.DeliverySearch.DeliverySearch,Edge.Applications.PM.Suite.DeliverySearch",throwOnError: true);
			
			Type formType = Type.GetType(className,throwOnError: true);
			Common.ProductionManagmentBaseForm form = (ProductionManagmentBaseForm)Activator.CreateInstance(formType);
			form.Show();

		}
	}

	
}
