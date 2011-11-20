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
			instances = GetServicesInstaceIdList(parentInstanceId);
			if (instances.Count > 0)
			{
				InstancesListBox.Items.Clear();
				foreach (var instance in instances)
				{
					InstancesListBox.Items.Add(string.Format("{0} ({1})", instance.Value, instance.Key));
				}
				string delivery;
				if (TryGetDeliveryId(instances, out delivery))
				{
					deliveryID_lbl.Text = delivery;
				}
				else deliveryID_lbl.Text = "No Delivery";
			}

			this.instaceIDValue.Text = parentInstanceId.ToString();
			this.SourceNameValue.Text = instanceName;
			this.AccountIdValue.Text = accountId;

			InstaceEntity parent = GetInstanceLogById(parentInstanceId);
			//this.parentException_tb.Text = String.IsNullOrEmpty(parent.ExceptionDetails) ? "None" : parent.ExceptionDetails;
			//this.parentInformation_lbl.Text = parent.Message;
			if (!string.IsNullOrEmpty(parent.Message))
				this.instacesSummaryDataGrid.Rows.Add(
					parent.Id, parent.Source, string.Format("{0} {1}", parent.Message, parent.ExceptionDetails), parent.MessageType);

			//Setting instances summary grid
			foreach (var id in instances)
			{
				InstaceEntity entity = GetInstanceLogById(id.Key);
				this.instacesSummaryDataGrid.Rows.Add(
					entity.Id, entity.Source, string.Format("{0} {1}", entity.Message, entity.ExceptionDetails), entity.MessageType);
			}

			//Loading Service Configuration

			//XmlReader xmlReader = GetConfigurationFromDB(parentInstanceId);
		
		}

		private XmlReader GetConfigurationFromDB(long instanceId)
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

				using (var _reader = sqlCommand.ExecuteXmlReader())
				{
					if (_reader.Read())
					{
						return _reader;
					}
				}
			}
			return null;
		}

		private bool TryGetDeliveryId(Dictionary<long, string> instances, out string id)
		{
			id = string.Empty;
			foreach (var item in instances)
			{
				id = GetDeliveryId(item.Key);
				if (!string.IsNullOrEmpty(id)) break;
			}

			if (string.IsNullOrEmpty(id)) return false;
			else return true;
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

		private Dictionary<long, string> GetServicesInstaceIdList(long parentInstaceId)
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

		private string GetDeliveryId(long instanceId)
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
