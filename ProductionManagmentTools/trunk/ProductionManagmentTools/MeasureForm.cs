using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Edge.Core.Data;
using Edge.Core.Configuration;

namespace Edge.Application.ProductionManagmentTools
{
	public partial class MeasureForm : Form
	{
		public MeasureForm()
		{
			InitializeComponent();

			this.accountsList.Items.Add(string.Format("{1}({0})", "-1", "Base Measures"));



		}

		private Dictionary<int, string> GetAccountsFromDB(string SystemDatabase)
		{
			Dictionary<int, string> accounts = new Dictionary<int, string>();

			using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(typeof(DataChecks), SystemDatabase)))
			{
				sqlCon.Open();
				SqlCommand sqlCommand = DataManager.CreateCommand(
					"SELECT  [Account_ID],[Account_Name] FROM [dbo].[User_GUI_Account] group by [Account_Name] ,[Account_ID] order by [Account_ID]");
				sqlCommand.Connection = sqlCon;

				using (var _reader = sqlCommand.ExecuteReader())
				{
					if (!_reader.IsClosed)
					{
						while (_reader.Read())
						{
							accounts.Add((int)_reader[0], (string)_reader[1]);
						}
					}
				}
			}
			return accounts;

		}

		private List<Measure> GetMeasures(string SystemDatabase)
		{
			List<Measure> measures = new List<Measure>();

			//using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(this, SystemDatabase)))
			//{
			//    sqlCon.Open();
			//    //SqlCommand sqlCommand = new SqlCommand(
			//    //sqlCommand.Parameters.Add("@accountID",);
			//    sqlCommand.Connection = sqlCon;

			//    using (var _reader = sqlCommand.ExecuteReader())
			//    {
			//        if (!_reader.IsClosed)
			//        {
			//            while (_reader.Read())
			//            {
			//                measures.Add(new Measure(_reader));
			//            }
			//        }
			//    }
			//}

			return measures;
		}

		private void accountsList_SelectedValueChanged(object sender, EventArgs e)
		{
			int startIndx = ((ComboBox)sender).SelectedItem.ToString().IndexOf('(');
			int endIndx = ((ComboBox)sender).SelectedItem.ToString().IndexOf(')');

			long accountId = Convert.ToInt64(((ComboBox)sender).SelectedItem.ToString().Substring(startIndx+1, endIndx - startIndx-1));

			//foreach (Measure measure in GetMeasures(application_cb.SelectedItem.ToString()))
			//{
			//    MeasureView.Rows.Add(measure.MeasureID,measure.ChannelID,measure.Name,measure.DisplayName,measure.SourceName,measure.FieldName,measure.DWH_Name,measure.OLTP_Table,measure.DWH_Table,measure.DWH_ProcessedTable,measure.
			//}

		}

		private void application_cb_SelectedValueChanged(object sender, EventArgs e)
		{
			foreach (var peir in GetAccountsFromDB(application_cb.SelectedItem.ToString()))
			{
				this.accountsList.Items.Add(string.Format("{1}({0})", peir.Key, peir.Value));
			}
		}




	}

	public class Measure
	{

		public Measure(SqlDataReader reader)
		{
			MeasureID = Convert.ToInt16(reader[0]);
			BaseMeasureID = Convert.ToInt16(reader[1]);
			ChannelID = Convert.ToInt16(reader[2]);
			AccountID = Convert.ToInt64(reader[3]);
			Name = reader[4].Equals(DBNull.Value) ? string.Empty : Convert.ToString(reader[4]);
			DisplayName = reader[5].Equals(DBNull.Value) ? string.Empty : Convert.ToString(reader[5]);
			OLTP_Table = reader[6].Equals(DBNull.Value) ? string.Empty : Convert.ToString(reader[6]);
			FieldName = reader[7].Equals(DBNull.Value) ? string.Empty : Convert.ToString(reader[7]);
			DWH_Table = reader[8].Equals(DBNull.Value) ? string.Empty : Convert.ToString(reader[8]);
			DWH_ProcessedTable = reader[9].Equals(DBNull.Value) ? string.Empty : Convert.ToString(reader[9]);
			DWH_Name = reader[10].Equals(DBNull.Value) ? string.Empty : Convert.ToString(reader[10]);
			DWH_AggregateFunction = reader[11].Equals(DBNull.Value) ? string.Empty : Convert.ToString(reader[11]);
			StringFormat = reader[12].Equals(DBNull.Value) ? string.Empty : Convert.ToString(reader[12]);
			IsAbsolute = Convert.ToBoolean(reader[13]);
			IsTarget = Convert.ToBoolean(reader[14]);
			IsAdTest = Convert.ToBoolean(reader[15]);
			IsBo = Convert.ToBoolean(reader[16]);
			IsDashboard = Convert.ToBoolean(reader[17]);
			AccountLevelMDX = Convert.ToString(reader[18]);
			TargetMeasureID = reader[19].Equals(DBNull.Value) ? string.Empty : Convert.ToString(reader[19]);
			IsCalculated = Convert.ToBoolean(reader[20]);
			IntegrityCheckRequired = Convert.ToString(reader[21]);
			SourceName = Convert.ToString(reader[22]);

		}

		public int MeasureID { set; get; }
		public long AccountID { set; get; }
		public int ChannelID { set; get; }
		public int BaseMeasureID { set; get; }
		public string Name { set; get; }
		public string DisplayName { set; get; }
		public string FieldName { set; get; }
		public string DWH_Name { set; get; }
		public string OLTP_Table { set; get; }
		public string DWH_Table { set; get; }
		public string DWH_ProcessedTable { set; get; }
		public string DWH_AggregateFunction { set; get; }
		public string TargetMeasureID { set; get; }
		public string StringFormat { set; get; }
		public bool IsCalculated { set; get; }
		public bool IsAbsolute { set; get; }
		public bool IsTarget { set; get; }
		public bool IsAdTest { set; get; }
		public bool IsBo { set; get; }
		public bool IsDashboard { set; get; }
		public string AccountLevelMDX { set; get; }
		public string IntegrityCheckRequired { set; get; }
		public string SourceName { set; get; }
	}
}
