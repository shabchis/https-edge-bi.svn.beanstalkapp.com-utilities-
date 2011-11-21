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
		Dictionary<long, string> instances = new Dictionary<long, string>(); //<InstanceId , name > 

		public frmInstanceLog()
		{
			InitializeComponent();
		}

		public void UpdateForm(long parentInstanceId, string instanceName, string accountId)
		{
			string deliveryId = string.Empty;
			DateTimeRange dateTimeRange = new DateTimeRange();
			string TargetPeriodDefinition;


			this.instaceIDValue.Text = parentInstanceId.ToString();
			this.SourceNameValue.Text = instanceName;
			this.AccountIdValue.Text = accountId;

			#region Getting Instaces
			instances = GetWorkFlowServicesInstaceIdList(parentInstanceId);
			if (instances.Count > 0)
			{
				InstancesListBox.Items.Clear();
				foreach (var instance in instances)
				{
					InstancesListBox.Items.Add(string.Format("{0} ({1})", instance.Value, instance.Key));
				}

				if (TryGetDeliveryId(instances, out deliveryId))
				{
					deliveryID_lbl.Text = deliveryId;
				}
				else deliveryID_lbl.Text = "No Delivery";
			}
			#endregion

			#region Getting Time Period
			//TO DO : somthing if not getting target period !!!!

			if (TryGetDeliveryTargetPeriod(deliveryId, out TargetPeriodDefinition))
			{
				dateTimeRange = (DateTimeRange)JsonConvert.DeserializeObject(TargetPeriodDefinition, typeof(DateTimeRange));
			}
			#endregion
		
			
			#region Setting instances List in overview
			InstaceEntity parent = GetInstanceLogById(parentInstanceId);
			if (!string.IsNullOrEmpty(parent.Message))
				this.instacesSummaryDataGrid.Rows.Add(
					parent.Id, parent.Source, string.Format("{0} {1}", parent.Message, parent.ExceptionDetails), parent.MessageType);
			#endregion

			#region Setting instances Summary Grid
			foreach (var id in instances)
			{
				InstaceEntity entity = GetInstanceLogById(id.Key);
				this.instacesSummaryDataGrid.Rows.Add(
					entity.Id, entity.Source, string.Format("{0} {1}", entity.Message, entity.ExceptionDetails), entity.MessageType);
			}
			#endregion

			#region Service init
			//Loading Service Configuration
			XmlDocument xmlDoc = new XmlDocument();
			Dictionary<string, string> attributes = new Dictionary<string, string>();
			try
			{
				string xmlContent = this.xmlContent_rtb.Text = GetConfigurationFromDB(parentInstanceId);
				xmlDoc.LoadXml(xmlContent);
				foreach (XmlAttribute attribute in xmlDoc.DocumentElement.Attributes)
				{
					attributes.Add(attribute.Name, attribute.Value);
					optionsListView.Items.Add(new ListViewItem(new string[] { attribute.Name, attribute.Value }));
				}

			}
			catch
			{
				//TO DO : throw exception
			}

			//set service configuration and run service.
			if (attributes.ContainsKey("Name"))
			{
				string serviceName = attributes["Name"];
				ServiceClient<IScheduleManager> scheduleManager = new ServiceClient<IScheduleManager>();
				SettingsCollection options = new SettingsCollection();
				//DateTime targetDateTime = new DateTime(dateToRunPicker.Value.Year, dateToRunPicker.Value.Month, dateToRunPicker.Value.Day, timeToRunPicker.Value.Hour, timeToRunPicker.Value.Minute, 0);

				foreach (var item in attributes)
				{
					options.Add(item.Key, item.Value);
				}
				options.Add(PipelineService.ConfigurationOptionNames.TargetPeriod, dateTimeRange.ToAbsolute().ToString());
				//run the service
		//		scheduleManager.Service.AddToSchedule(serviceName, Convert.ToInt32(accountId), DateTime.Now, options);

			}
			#endregion
		}

		private bool TryGetDeliveryTargetPeriod(string deliveryId, out string TargetPeriodDefinition)
		{
			TargetPeriodDefinition = GetDeliveryTargetPeriodFromDB(deliveryId);
			if (!string.IsNullOrEmpty(TargetPeriodDefinition))
				return true;
			return false;
		}

		private string GetDeliveryTargetPeriodFromDB(string deliveryId)
		{
			string TargetPeriod = String.Empty;
			using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
			{
				sqlCon.Open();
				SqlCommand sqlCommand = new SqlCommand(
					  @"SELECT [TargetPeriodDefinition] FROM [dbo].[Delivery] where [DeliveryID] = @deliveryId"
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
							TargetPeriod = _reader[0].ToString();
						}
					}
				}

			}
			return TargetPeriod;
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

		private InstaceEntity GetInstanceLogById(long instanceId)
		{

			using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
			{
				InstaceEntity entity = new InstaceEntity();

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

		private void removeOptionBtn_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in optionsListView.SelectedItems)
			{
				item.Remove();

			}
		}
	}

	public class InstaceEntity
	{
		public long Id { set; get; }
		public string Source { set; get; }
		public string Message { set; get; }
		public string ExceptionDetails { set; get; }
		public string MessageType { set; get; }
	}
}
