using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Edge.Core.Configuration;
using System.Xml;
using System.Data.SqlTypes;
using System.IO;
using Edge.Core.Services;
using Edge.Core.Scheduling;
using Edge.Core;
using Edge.Data.Pipeline;
using Newtonsoft.Json;
using Edge.Data.Pipeline.Services;

namespace Edge.Applications.TempScheduler
{
	public partial class frmInstanceLog : Form
	{
		private Dictionary<long, string> _instances = new Dictionary<long, string>(); //<InstanceId , name > 
		private Dictionary<string, string> _attributes = new Dictionary<string, string>();
		private SettingsCollection _options = new SettingsCollection();
		private DateTimeRange _dateTimeRange = new DateTimeRange();
		private Listener _listner;
		private int _accountId;

		public frmInstanceLog(Listener listner)
		{
			InitializeComponent();
			_listner = listner;
		}

		public void UpdateForm(long parentInstanceId, string instanceName, string accountId)
		{
			string deliveryId = string.Empty;
			this.instaceIDValue.Text = parentInstanceId.ToString();
			this.SourceNameValue.Text = instanceName;
			this.AccountIdValue.Text = accountId;
			this._accountId = Convert.ToInt32(accountId);
			FromPicker.Value = FromPicker.Value.AddDays(-1);
			ToPicker.Value = ToPicker.Value.AddDays(-1);

			#region Getting Instaces
			_instances = GetWorkFlowServicesInstaceIdList(parentInstanceId);
			if (_instances.Count > 0)
			{
				InstancesListBox.Items.Clear();
				foreach (var instance in _instances)
				{
					InstancesListBox.Items.Add(string.Format("{0} ({1})", instance.Value, instance.Key));
				}

				if (TryGetDeliveryId(_instances, out deliveryId))
				{
					deliveryID_lbl.Text = deliveryId;
				}
				else deliveryID_lbl.Text = "No Delivery";
			}
			#endregion

			#region Setting instances List in overview
			InstanceEntity parent = GetInstanceLogById(parentInstanceId);
			if (!string.IsNullOrEmpty(parent.Message))
				this.instacesSummaryDataGrid.Rows.Add(
					parent.Id, parent.Source, string.Format("{0} {1}", parent.Message, parent.ExceptionDetails), parent.MessageType);
			#endregion

			#region Setting instances Summary Grid
			foreach (var id in _instances)
			{
				InstanceEntity entity = GetInstanceLogById(id.Key);
				this.instacesSummaryDataGrid.Rows.Add(
					entity.Id, entity.Source, string.Format("{0} {1}", entity.Message, entity.ExceptionDetails), entity.MessageType);
			}
			#endregion

			#region Service Configuration
			//Loading Service Configuration
			XmlDocument xmlDoc = new XmlDocument();

			try
			{
				string xmlContent = GetConfigurationFromDB(parentInstanceId);
				xmlDoc.LoadXml(xmlContent);
				foreach (XmlAttribute attribute in xmlDoc.DocumentElement.Attributes)
				{
					_attributes.Add(attribute.Name, attribute.Value);

					if (!attribute.Name.Equals(PipelineService.ConfigurationOptionNames.TimePeriod))
						optionsListView.Items.Add(new ListViewItem(new string[] { attribute.Name, attribute.Value }));
					else
						AttributesToRunList.Items.Add(new ListViewItem(new string[] { attribute.Name, attribute.Value }));
				}

			}
			catch
			{
				//TO DO : throw exception
			}

			//Getting TimePeriod
			if (TryGetDeliveryTargetPeriod(deliveryId, out _dateTimeRange))
			{
				_options.Add(PipelineService.ConfigurationOptionNames.TimePeriod, _dateTimeRange.ToAbsolute().ToString());
				FromPicker.Value = _dateTimeRange.Start.ToDateTime();
				ToPicker.Value = _dateTimeRange.End.ToDateTime();
			}
			else
			{
				this.deliveryID_lbl.Text = "Unavailable";
				this.run_btn.Enabled = false;
				MessageBox.Show("\"Run Service Again\" button has been disabled due the following reason : Cannot find target period in delivery data base");
			}

			#endregion
		}

		private bool TryGetDeliveryTargetPeriod(string deliveryId, out DateTimeRange TargetPeriodDefinition)
		{
			TargetPeriodDefinition = new DateTimeRange();
			string TargetPeriodStart = String.Empty;
			string TargetPeriodEnd = String.Empty;
			GetDeliveryTargetPeriodFromDB(deliveryId, out TargetPeriodStart, out TargetPeriodEnd);

			if (!string.IsNullOrEmpty(TargetPeriodStart) && !string.IsNullOrEmpty(TargetPeriodEnd))
			{
				TargetPeriodDefinition.Start = DateTimeSpecification.Parse(TargetPeriodStart);
				TargetPeriodDefinition.End = DateTimeSpecification.Parse(TargetPeriodEnd);
				return true;
			}
			return false;
		}

		private void GetDeliveryTargetPeriodFromDB(string deliveryId, out string TargetPeriodStart, out string TargetPeriodEnd)
		{
			TargetPeriodStart = TargetPeriodEnd = String.Empty;
			using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
			{
				sqlCon.Open();
				SqlCommand sqlCommand = new SqlCommand(
					  @"SELECT [TimePeriodStart],[TimePeriodEnd] FROM [dbo].[Delivery] where [DeliveryID] = @deliveryId"
					  );

				sqlCommand.Parameters.Add(new SqlParameter()
				{
					ParameterName = "@deliveryId",
					Value = deliveryId,
					SqlDbType = System.Data.SqlDbType.Char
				});
				sqlCommand.Connection = sqlCon;

				using (var _reader = sqlCommand.ExecuteReader())
				{
					if (!_reader.IsClosed)
					{
						while (_reader.Read())
						{
							TargetPeriodStart = _reader[0].ToString();
							TargetPeriodEnd = _reader[1].ToString();
						}
					}
				}

			}
		}

		private string GetConfigurationFromDB(long instanceId)
		{
			string config = String.Empty;
			using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
			{
				sqlCon.Open();
				SqlCommand sqlCommand = new SqlCommand(
					  @"SELECT Configuration  from [dbo].[ServiceInstance] where [InstanceID] = @instanceId"
					  );

				sqlCommand.Parameters.Add(new SqlParameter()
				{
					ParameterName = "@instanceId",
					Value = instanceId,
					SqlDbType = System.Data.SqlDbType.BigInt
				});
				sqlCommand.Connection = sqlCon;

				using (var _reader = sqlCommand.ExecuteReader())
				{
					while (_reader.Read())
					{
						config = _reader[0].ToString();
					}
				}
			}
			return config;
		}

		private bool TryGetDeliveryId(Dictionary<long, string> instances, out string id)
		{
			id = string.Empty;
			foreach (var item in instances)
			{
				id = GetDeliveryIdFromDB(item.Key);
				if (!string.IsNullOrEmpty(id))
					return true;
			}
			return false;
		}

		private InstanceEntity GetInstanceLogById(long instanceId)
		{

			using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
			{
				InstanceEntity entity = new InstanceEntity();

				sqlCon.Open();
				SqlCommand sqlCommand = new SqlCommand(
					"SELECT [ID],[Source],[Message],[ExceptionDetails],[MessageType] FROM [dbo].[Log] where [ServiceInstanceID] = @instanceId");

				sqlCommand.Parameters.Add(new SqlParameter()
				{
					ParameterName = "@instanceId",
					Value = instanceId,
					SqlDbType = System.Data.SqlDbType.BigInt
				});

				sqlCommand.Connection = sqlCon;

				using (var _reader = sqlCommand.ExecuteReader())
				{
					if (!_reader.IsClosed)
					{
						while (_reader.Read())
						{
							entity.Id = Convert.ToInt64(_reader[0]);
							entity.Source = _reader[1].ToString();
							entity.Message = _reader[2].ToString();
							entity.ExceptionDetails = _reader[3].ToString();
							entity.MessageType = _reader[4].ToString();
						}
					}
				}

				return entity;
			}
		}

		private Dictionary<long, string> GetWorkFlowServicesInstaceIdList(long parentInstaceId)
		{
			Dictionary<long, string> instances = new Dictionary<long, string>();

			using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
			{
				sqlCon.Open();
				SqlCommand sqlCommand = new SqlCommand(
					"SELECT [InstanceID],[ServiceName] FROM [dbo].[ServiceInstance] where [ParentInstanceID] = @ParentInstanceID");

				sqlCommand.Parameters.Add(new SqlParameter() { ParameterName = "@ParentInstanceID", Value = parentInstaceId, SqlDbType = System.Data.SqlDbType.BigInt });
				sqlCommand.Connection = sqlCon;

				using (var _reader = sqlCommand.ExecuteReader())
				{
					if (!_reader.IsClosed)
					{
						while (_reader.Read())
						{
							instances.Add(Convert.ToInt64(_reader[0]), _reader[1].ToString());
						}
					}
				}
			}
			return instances;

		}

		private string GetDeliveryIdFromDB(long instanceId)
		{
			string deliveryId = String.Empty;
			using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
			{
				sqlCon.Open();
				SqlCommand sqlCommand = new SqlCommand(
					  @"SELECT Configuration.value('data(/ActiveService/@DeliveryID)[1]', 'nvarchar(MAX)')
                        from [dbo].[ServiceInstance] where [InstanceID] = @instanceId"
					  );

				sqlCommand.Parameters.Add(new SqlParameter()
				{
					ParameterName = "@instanceId",
					Value = instanceId,
					SqlDbType = System.Data.SqlDbType.BigInt
				});
				sqlCommand.Connection = sqlCon;

				using (var _reader = sqlCommand.ExecuteReader())
				{
					if (!_reader.IsClosed)
					{
						while (_reader.Read())
						{
							deliveryId = _reader[0].ToString();
						}
					}
				}

			}
			return deliveryId;
		}

		private void addOptionBtn_Click(object sender, EventArgs e)
		{
			bool exist = false;

			foreach (ListViewItem item in AttributesToRunList.Items)
			{
				if (item.SubItems[0].Text.Trim() == cmbKey.Text)
					exist = true;
			}
			if (!string.IsNullOrEmpty(cmbKey.Text) && !string.IsNullOrEmpty(cmbValue.Text))
			{
				if (!exist)
				{
					AttributesToRunList.Items.Add(new ListViewItem(new string[] { cmbKey.Text, cmbValue.Text }));
					cmbKey.Text = string.Empty;
					cmbValue.Text = string.Empty;
				}
				else
					MessageBox.Show("DuplicateKey");
			}
		}

		private void removeOptionBtn_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in AttributesToRunList.SelectedItems)
			{
				item.Remove();
				optionsListView.Items.Add(item);
			}
		}

		private void run_btn_Click(object sender, EventArgs e)
		{
			if (_attributes.ContainsKey("Name"))
			{
				string serviceName = _attributes["Name"];
				ServiceClient<IScheduleManager> scheduleManager = new ServiceClient<IScheduleManager>();

				foreach (ListViewItem item in AttributesToRunList.Items)
				{
					_options.Add(item.SubItems[0].Text.Trim(), item.SubItems[1].Text.Trim());
				}

				if (ConflictBehavior.Checked && !optionsListView.Items.ContainsKey("ConflictBehavior"))
					_options.Add("ConflictBehavior", "Ignore");

				//Run Service
				if (!_options.ContainsKey(PipelineService.ConfigurationOptionNames.TimePeriod))
					_options.Add(PipelineService.ConfigurationOptionNames.TimePeriod, _dateTimeRange.ToAbsolute().ToString());
				bool result = _listner.FormAddToSchedule(serviceName, _accountId, DateTime.Now, _options, ServicePriority.Normal);
				MessageBox.Show("Service has been submited");
				if (!result)
				{
					MessageBox.Show(string.Format("Service {0} for account {1} did not run", serviceName, _accountId));
				}
			}

		}

		private void RemoveOption(string option)
		{
			_attributes.Remove(option);
			_options.Remove(option);
		}

		private void AddFromAvailable_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in optionsListView.SelectedItems)
			{
				optionsListView.Items.Remove(item);
				if (!AttributesToRunList.Items.Contains(item))
					AttributesToRunList.Items.Add(item);
			}
		}

		private void ClearAttributes_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in AttributesToRunList.SelectedItems)
			{
				item.Remove();
				if (!optionsListView.Items.Contains(item))
					optionsListView.Items.Add(item);
			}
		}
		
	}

	public class InstanceEntity
	{
		public long Id { set; get; }
		public string Source { set; get; }
		public string Message { set; get; }
		public string ExceptionDetails { set; get; }
		public string MessageType { set; get; }
	}
}
