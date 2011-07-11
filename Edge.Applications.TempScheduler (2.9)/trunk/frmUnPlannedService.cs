using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Edge.Core.Scheduling;
using Edge.Core.Scheduling.Objects;
using Edge.Core.Services;
using Edge.Core.Configuration;

namespace Edge.Applications.TempScheduler
{
	public partial class frmUnPlannedService : Form
	{
		private Listener _listner;
		private Scheduler _scheduler;
		public frmUnPlannedService(Listener listner, Scheduler scheduler)
		{
			InitializeComponent();
			_listner = listner;
			_scheduler = scheduler;
		}



		private void FillComboBoxes()
		{
			//services per account

			//List<ServiceConfiguration> serviceConfigurations = _scheduler.GetAllExistServices(); //.Where(s => s.SchedulingRules.Count > 0 && s.SchedulingRules[0].Scope != SchedulingScope.UnPlanned).ToList();
			//serviceConfigurations = serviceConfigurations.OrderBy((s => s.SchedulingProfile.ID)).ToList();
			//foreach (ServiceConfiguration serviceConfiguration in serviceConfigurations)
			//{
			//    servicesCheckedListBox.Items.Add(string.Format("{0}    :   {1}", serviceConfiguration.SchedulingProfile.Name, serviceConfiguration.Name));

			//}
			servicesTreeView.BeginUpdate();
			foreach (AccountElement accountElement in EdgeServicesConfiguration.Current.Accounts)
			{
				TreeNode currentNode = servicesTreeView.Nodes.Add(string.Format("{0}-{1}", accountElement.ID, accountElement.Name));
				currentNode.Tag = accountElement;

				AddServices(currentNode, accountElement.Services);



			}
			servicesTreeView.EndUpdate();
			priorityCmb.Items.Add(ServicePriority.Low);
			priorityCmb.Items.Add(ServicePriority.Normal);
			priorityCmb.Items.Add(ServicePriority.High);
			priorityCmb.Items.Add(ServicePriority.Immediate);



		}

		private void AddServices(TreeNode currentNode, AccountServiceElementCollection accountServiceElementCollection = null, WorkflowStepElementCollection workflowStepElementCollection = null)
		{
			TreeNode current;
			if (accountServiceElementCollection != null)
			{
				foreach (AccountServiceElement accountServiceElement in accountServiceElementCollection)
				{
					ActiveServiceElement activeServiceElement = new ActiveServiceElement(accountServiceElement);
					current = currentNode.Nodes.Add(activeServiceElement.Name);
					current.Tag = activeServiceElement;
					AddServices(current, workflowStepElementCollection: activeServiceElement.Workflow);
				}
			}
			else if (workflowStepElementCollection != null)
			{
				foreach (WorkflowStepElement stepElement in workflowStepElementCollection)
				{
					current = currentNode.Nodes.Add(stepElement.ActualName);
					current.Checked = stepElement.IsEnabled;
					current.Tag = stepElement;
					AddServices(current, workflowStepElementCollection: stepElement.BaseConfiguration.Element.Workflow);

				}
			}
			return;
		}

		private void frmUnPlanedService_Load(object sender, EventArgs e)
		{
			FillComboBoxes();
			FromPicker.Value = DateTime.Now.Date.AddDays(-1);
			toPicker.Value = DateTime.Now.Date.AddDays(-1);
		}

		private void addBtn_Click(object sender, EventArgs e)
		{
			bool allSucceed = true;
			try
			{

				ServicePriority servicePriority = ServicePriority.Low;
				Edge.Core.SettingsCollection options = new Edge.Core.SettingsCollection();
				DateTime targetDateTime = new DateTime(dateToRunPicker.Value.Year, dateToRunPicker.Value.Month, dateToRunPicker.Value.Day, timeToRunPicker.Value.Hour, timeToRunPicker.Value.Minute, 0);
				bool result = false;

				if (priorityCmb.SelectedItem != null)
					switch (priorityCmb.SelectedItem.ToString())
					{
						case "Low":
							{
								servicePriority = ServicePriority.Low;
								break;
							}
						case "Normal":
							{
								servicePriority = ServicePriority.Normal;
								break;
							}
						case "High":
							{
								servicePriority = ServicePriority.High;
								break;
							}
						case "Immediate":
							{
								servicePriority = ServicePriority.Immediate;
								break;
							}
					}
				foreach (TreeNode accountNode in servicesTreeView.Nodes)
				{
					if (accountNode.Checked)
						foreach (TreeNode serviceNode in accountNode.Nodes)
						{
							AccountElement account = (AccountElement)accountNode.Tag;
							ActiveServiceElement service = (ActiveServiceElement)serviceNode.Tag;
							if (useOptionsCheckBox.Checked)
							{
								foreach (ListViewItem item in optionsListView.Items)
									options.Add(item.SubItems[0].Text.Trim(), item.SubItems[1].Text.Trim());

								DateTime from = FromPicker.Value;
								DateTime to = toPicker.Value;
								if (to.Date < from.Date || to.Date > DateTime.Now.Date)
									throw new Exception("to date must be equal or greater then from date, and both should be less then today's date");


								while (from.Date <= to.Date)
								{
									options["Date"] = from.ToString("yyyyMMdd");
									result = _listner.FormAddToSchedule(service, account, targetDateTime, options, servicePriority);
									if (!result)
									{
										allSucceed = result;
										MessageBox.Show(string.Format("Service {0} for account {1} did not run", service.Name, accountNode.Text));
									}
									from = from.AddDays(1);
								}
							}
							else
							{
								result = _listner.FormAddToSchedule(service, account, targetDateTime, options, servicePriority);
								if (!result)
									allSucceed = result;
							}
						}
				}
				if (!allSucceed)
					throw new Exception("Some services did not run");




			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.Message);
			}
		}

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			this.Close();

		}

		private void addOptionBtn_Click(object sender, EventArgs e)
		{
			bool exist = false;
			foreach (ListViewItem item in optionsListView.Items)
			{
				if (item.SubItems[0].Text.Trim() == keyTxt.Text)
					exist = true;

			}
			if (!string.IsNullOrEmpty(keyTxt.Text) && !string.IsNullOrEmpty(valueTxt.Text))
			{
				if (!exist)
				{
					optionsListView.Items.Add(new ListViewItem(new string[] { keyTxt.Text, valueTxt.Text }));
					keyTxt.Text = string.Empty;
					valueTxt.Text = string.Empty;
				}
				else
					MessageBox.Show("DuplicateKey");
			}
		}

		private void useOptionsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (useOptionsCheckBox.Checked)
				serviceOptionsGroupBox.Enabled = true;
			else
				serviceOptionsGroupBox.Enabled = false;
		}

		private void removeOptionBtn_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in optionsListView.SelectedItems)
			{
				item.Remove();

			}
		}

		private void clearOptionsBtn_Click(object sender, EventArgs e)
		{
			optionsListView.Clear();
		}

		private void selectAllbtn_Click(object sender, EventArgs e)
		{
			//for (int i = 0; i < servicesCheckedListBox.Items.Count; i++)
			//{
			//    if (!servicesCheckedListBox.Items[i].ToString().StartsWith("-1"))
			//        servicesCheckedListBox.SetItemChecked(i, true);
			//    else
			//        servicesCheckedListBox.SetItemChecked(i, false);

			//}
		}

		private void unSelectAllBtn_Click(object sender, EventArgs e)
		{
			//for (int i = 0; i < servicesCheckedListBox.Items.Count; i++)
			//{

			//        servicesCheckedListBox.SetItemChecked(i, false);

			//}

		}

		private void servicesTreeView_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (e.Action != TreeViewAction.Unknown)
			{
				CheckAllChildNodes(e.Node, e.Node.Checked);
			}
		}
		private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
		{
			if (treeNode.Tag is WorkflowStepElement)
				((WorkflowStepElement)treeNode.Tag).IsEnabled = nodeChecked;

			foreach (TreeNode node in treeNode.Nodes)
			{
				if (!(node.Tag is WorkflowStepElement))
				{
					node.Checked = nodeChecked;

					if (!(node.Tag is ActiveServiceElement) && node.Nodes.Count > 0)
					{
						// If the current node has child nodes, call the CheckAllChildsNodes method recursively.
						this.CheckAllChildNodes(node, nodeChecked);
					}
				}

			}

		}


	}

}

