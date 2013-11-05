﻿using System;
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
using Edge.Data.Pipeline;
using Edge.Data.Pipeline.Services;

namespace Edge.Applications.TempScheduler
{
	public partial class frmUnPlannedService : Form
	{
		private Listener _listener;
		private Scheduler _scheduler;
		public frmUnPlannedService(Listener listener, Scheduler scheduler)
		{
			InitializeComponent();
			_listener = listener;
			_scheduler = scheduler;
		}


		private void FillComboBoxes()
		{
			servicesTreeView.BeginUpdate();
			foreach (AccountElement accountElement in EdgeServicesConfiguration.Current.Accounts)
			{
				TreeNode currentNode = servicesTreeView.Nodes.Add(string.Format("{0}-{1}", accountElement.ID, accountElement.Name));
				currentNode.Tag = accountElement;

				AddServices(currentNode, accountElement.Services);
			}

			servicesTreeView.EndUpdate();
			List<string> services = new List<string>();
			foreach (ServiceElement service in EdgeServicesConfiguration.Current.Services)
			{
				if (service.Name != "Rerun")
					services.Add(service.Name);
			}
			services.Add("Ignore");
			cmbValue.DataSource = services;
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
			ServicePriority servicePriority = ServicePriority.Low;
			var options = new Edge.Core.SettingsCollection();
			DateTime targetDateTime = new DateTime(dateToRunPicker.Value.Year, dateToRunPicker.Value.Month, dateToRunPicker.Value.Day, timeToRunPicker.Value.Hour, timeToRunPicker.Value.Minute, 0);

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
			int countedSelectedServices = 0;
			foreach (TreeNode accountNode in servicesTreeView.Nodes)
			{
				foreach (TreeNode serviceNode in accountNode.Nodes)
				{
					if (!serviceNode.Checked)
						continue;

					countedSelectedServices++;
					AccountElement account = (AccountElement)accountNode.Tag;
					ActiveServiceElement service = (ActiveServiceElement)serviceNode.Tag;
					if (useOptionsCheckBox.Checked)
					{
						foreach (ListViewItem item in optionsListView.Items)
							options.Add(item.SubItems[0].Text.Trim(), item.SubItems[1].Text.Trim());

						DateTime from = FromPicker.Value;
						DateTime to = toPicker.Value;
						if (to.Date < from.Date || to.Date > DateTime.Now.Date)
						{
							MessageBox.Show(String.Format("Account {0} service {1}: To date must be equal or greater than from date, and both should be less then today's date",
								account.ID,
								service.Name
								));
							continue;
						}

						DateTimeRange daterange = new DateTimeRange()
						{
							Start = new DateTimeSpecification()
							{
								BaseDateTime = from,
								Hour = new DateTimeTransformation() { Type = DateTimeTransformationType.Exact, Value = 0 },

							},
							End = new DateTimeSpecification()
							{
								BaseDateTime = to,
								Hour = new DateTimeTransformation() { Type = DateTimeTransformationType.Max },

							}
						};
						options.Add(PipelineService.ConfigurationOptionNames.TimePeriod, daterange.ToAbsolute().ToString());
					}

					_listener.AddToSchedule(service, account, targetDateTime, options, servicePriority);
					options.Clear();

				}
			}

			MessageBox.Show(String.Format("{0} unplanned services were added.", countedSelectedServices));
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
				if (item.SubItems[0].Text.Trim() == cmbKey.Text)
					exist = true;

			}
			if (!string.IsNullOrEmpty(cmbKey.Text) && !string.IsNullOrEmpty(cmbValue.Text))
			{
				if (!exist)
				{
					optionsListView.Items.Add(new ListViewItem(new string[] { cmbKey.Text, cmbValue.Text }));
					cmbKey.Text = string.Empty;
					cmbValue.Text = string.Empty;
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

