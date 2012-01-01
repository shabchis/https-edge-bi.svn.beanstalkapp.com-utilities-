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
using Edge.Core.Data;
using Edge.Data.Pipeline;
using System.Configuration;

namespace Edge.Application.ProductionManagmentTools
{
    public partial class DeliverySearch : Form
    {
        public DeliverySearch()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeliveryDataGridView.Rows.Clear();
            DeliveryHistory.Rows.Clear();
            DeliveryHistoryParams.Rows.Clear();
            DeliveryParams.Rows.Clear();
            DeliveryFiles.Rows.Clear();
            DeliveryFileHistory.Rows.Clear();
            DeliveryFileHistoryParams.Rows.Clear();

            #region TimePeriod
            DateTimeRange timePeriod = new DateTimeRange()
            {
                Start = new DateTimeSpecification()
                {
                    BaseDateTime = fromDate.Value,
                    Hour = new DateTimeTransformation() { Type = DateTimeTransformationType.Exact, Value = 0 },
                    //Boundary = DateTimeSpecificationBounds.Lower
                },

                End = new DateTimeSpecification()
                {
                    BaseDateTime = toDate.Value,
                    Hour = new DateTimeTransformation() { Type = DateTimeTransformationType.Max },
                    // Boundary = DateTimeSpecificationBounds.Upper
                }
            };
            #endregion

            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(this, "DeliveryDB")))
            {

                sqlCon.Open();
                StringBuilder whereQuery = new StringBuilder();
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(accountID.Text))
                    parameters.Add("AccountID", accountID.Text);

                if (!string.IsNullOrWhiteSpace(DeliveryID_tb.Text))
                    parameters.Add("DeliveryID", DeliveryID_tb.Text);


				string query = "SELECT DeliveryID,AccountID,OriginalID,ChannelID,DateCreated,TargetPeriodStart,TargetPeriodEnd,[Committed] from dbo.Delivery " +
                   " where TargetPeriodStart between @TargetPeriodStart and @TargetPeriodEnd ";

                foreach (var item in parameters)
                {
                    if (!string.IsNullOrWhiteSpace(item.Value))
                    {
                        whereQuery.Append(string.Format(" and {0} = '{1}'", item.Key, item.Value));
                    }
                }

                query = string.Concat(query, whereQuery.ToString());
                SqlCommand sqlCommand = DataManager.CreateCommand(query);
                SqlParameter targetPeriodStartParam = new SqlParameter("@TargetPeriodStart", System.Data.SqlDbType.DateTime2);
                SqlParameter targetPeriodEndParam = new SqlParameter("@TargetPeriodEnd", System.Data.SqlDbType.DateTime2);


                targetPeriodStartParam.Value = timePeriod.Start.ToDateTime();
                targetPeriodEndParam.Value = timePeriod.End.ToDateTime();

                //accountIdParam.Value = accountID.Text;
                sqlCommand.Parameters.Add(targetPeriodStartParam);
                sqlCommand.Parameters.Add(targetPeriodEndParam);



                sqlCommand.Connection = sqlCon;

                using (var _reader = sqlCommand.ExecuteReader())
                {
                    if (!_reader.IsClosed)
                    {
                        while (_reader.Read())
                        {
                            if (!_reader[0].Equals(DBNull.Value))
                            {
                                DeliveryDataGridView.Rows.Add(_reader[0], _reader[1], _reader[2], _reader[3], _reader[4], _reader[5], _reader[6],_reader[7]);
                            }
                        }
                    }
                }
            }
        }

        private void DeliveryDataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DeliveryDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewSelectedRowCollection rows = DeliveryDataGridView.SelectedRows;
            if (rows.Count > 1)
            {
                DialogResult dlgRes = new DialogResult();
                dlgRes = MessageBox.Show("Only one delivery can be selected", "Delivery ID",
                MessageBoxButtons.OK,
                 MessageBoxIcon.Error);
                return;
            }
            else if (rows.Count != 0)
            {
                #region DeliveryHistory
                using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(this, "DeliveryDB")))
                {

                    sqlCon.Open();
                    SqlCommand sqlCommand = DataManager.CreateCommand(
                       "SELECT [ServiceInstanceID],[Index],[Operation],[DateRecorded] from dbo.DeliveryHistory " +
                       " where DeliveryID = @DeliveryID "
                       );
                    SqlParameter delId = new SqlParameter("@DeliveryID", System.Data.SqlDbType.Char);
                    delId.Value = rows[0].Cells[0].Value.ToString();
                    sqlCommand.Parameters.Add(delId);
                    sqlCommand.Connection = sqlCon;

                    using (var _reader = sqlCommand.ExecuteReader())
                    {
                        if (!_reader.IsClosed)
                        {
                            while (_reader.Read())
                            {
                                if (!_reader[0].Equals(DBNull.Value))
                                {
                                    DeliveryHistory.Rows.Add(_reader[0], _reader[1], _reader[2], _reader[3]);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region DeliveryHistoryParameters
                using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(this, "DeliveryDB")))
                {

                    sqlCon.Open();
                    SqlCommand sqlCommand = DataManager.CreateCommand(
                       "SELECT [Index],[Key],[Value] from dbo.DeliveryHistoryParameters " +
                       " where DeliveryID = @DeliveryID "
                       );
                    SqlParameter delId = new SqlParameter("@DeliveryID", System.Data.SqlDbType.Char);
                    delId.Value = rows[0].Cells[0].Value.ToString();
                    sqlCommand.Parameters.Add(delId);
                    sqlCommand.Connection = sqlCon;

                    using (var _reader = sqlCommand.ExecuteReader())
                    {
                        if (!_reader.IsClosed)
                        {
                            while (_reader.Read())
                            {
                                if (!_reader[0].Equals(DBNull.Value))
                                {
                                    DeliveryHistoryParams.Rows.Add(_reader[0], _reader[1], _reader[2]);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region DeliveryParameters
                using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(this, "DeliveryDB")))
                {

                    sqlCon.Open();
                    SqlCommand sqlCommand = DataManager.CreateCommand(
                       "SELECT [Key],[Value] from dbo.DeliveryParameters " +
                       " where DeliveryID = @DeliveryID "
                       );
                    SqlParameter delId = new SqlParameter("@DeliveryID", System.Data.SqlDbType.Char);
                    delId.Value = rows[0].Cells[0].Value.ToString();
                    sqlCommand.Parameters.Add(delId);
                    sqlCommand.Connection = sqlCon;

                    using (var _reader = sqlCommand.ExecuteReader())
                    {
                        if (!_reader.IsClosed)
                        {
                            while (_reader.Read())
                            {
                                if (!_reader[0].Equals(DBNull.Value))
                                {
                                    DeliveryParams.Rows.Add(_reader[0], _reader[1]);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region DeliveryFiles
                using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(this, "DeliveryDB")))
                {

                    sqlCon.Open();
                    SqlCommand sqlCommand = DataManager.CreateCommand(
                       "SELECT [FileID],[Name] from dbo.DeliveryFile " +
                       " where DeliveryID = @DeliveryID "
                       );
                    SqlParameter delId = new SqlParameter("@DeliveryID", System.Data.SqlDbType.Char);
                    delId.Value = rows[0].Cells[0].Value.ToString();
                    sqlCommand.Parameters.Add(delId);
                    sqlCommand.Connection = sqlCon;

                    using (var _reader = sqlCommand.ExecuteReader())
                    {
                        if (!_reader.IsClosed)
                        {
                            while (_reader.Read())
                            {
                                if (!_reader[0].Equals(DBNull.Value))
                                {
                                    DeliveryFiles.Rows.Add(_reader[0], _reader[1]);
                                }
                            }
                        }
                    }
                }
                #endregion

                //#region DeliveryMeasuers
                //using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(this, "DeliveryDB")))
                //{
                //    sqlCon.Open();
                //    SqlCommand sqlCommand = DataManager.CreateCommand(
                //        "SELECT Account_ID,SUM(cost),SUM(clicks),SUM(imps)" +
                //        "FROM [dbo].[Paid_API_AllColumns_v29]" +
                //        "where [DeliveryID] = @DeliveryID" +
                //        " group by Account_ID"
                //       );
                //    SqlParameter delId = new SqlParameter("@DeliveryID", System.Data.SqlDbType.Char);
                //    delId.Value = rows[0].Cells[0].Value.ToString();
                //    sqlCommand.Parameters.Add(delId);
                //    sqlCommand.Connection = sqlCon;

                //    using (var _reader = sqlCommand.ExecuteReader())
                //    {
                //        if (!_reader.IsClosed)
                //        {
                //            while (_reader.Read())
                //            {
                //                if (!_reader[0].Equals(DBNull.Value))
                //                {
                //                    MeasuresTable.Rows.Add(_reader[0], _reader[1], _reader[2], _reader[3]);
                //                }
                //            }
                //        }
                //    }
                //}
                //#endregion
            }
        }

        private void DhistoryClear_Click(object sender, EventArgs e)
        {
            DeliveryHistory.Rows.Clear();
            DeliveryHistoryParams.Rows.Clear();
            DeliveryParams.Rows.Clear();
        }

        private void DeliveryFiles_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewSelectedRowCollection rows = DeliveryFiles.SelectedRows;
            if (rows.Count > 1)
            {
                DialogResult dlgRes = new DialogResult();
                dlgRes = MessageBox.Show("Only one file can be selected", "Error",
                MessageBoxButtons.OK,
                 MessageBoxIcon.Error);
                return;
            }
            else if (rows.Count != 0)
            {

                #region DeliveryFileHistoryParameters
                using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(this, "DeliveryDB")))
                {
                    sqlCon.Open();
                    SqlCommand sqlCommand = DataManager.CreateCommand
                        ("SELECT [Name],[Index],[Key],[Value] from dbo.DeliveryFileHistoryParameters where Name = @FileName ");

                    SqlParameter fileName = new SqlParameter("@FileName", System.Data.SqlDbType.NVarChar);
                    fileName.Value = rows[0].Cells[1].Value.ToString();
                    sqlCommand.Parameters.Add(fileName);
                    sqlCommand.Connection = sqlCon;

                    using (var _reader = sqlCommand.ExecuteReader())
                    {
                        if (!_reader.IsClosed)
                        {
                            while (_reader.Read())
                            {
                                if (!_reader[0].Equals(DBNull.Value))
                                {
                                    DeliveryFileHistoryParams.Rows.Add(_reader[0], _reader[1], _reader[2], _reader[3]);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region DeliveryFileHistory
                using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(this, "DeliveryDB")))
                {
                    sqlCon.Open();
                    SqlCommand sqlCommand = DataManager.CreateCommand
                        ("SELECT [ServiceInstanceID],[Index],[Operation] from dbo.DeliveryFileHistory where Name = @FileName ");

                    SqlParameter fileName = new SqlParameter("@FileName", System.Data.SqlDbType.NVarChar);
                    fileName.Value = rows[0].Cells[1].Value.ToString();
                    sqlCommand.Parameters.Add(fileName);
                    sqlCommand.Connection = sqlCon;

                    using (var _reader = sqlCommand.ExecuteReader())
                    {
                        if (!_reader.IsClosed)
                        {
                            while (_reader.Read())
                            {
                                if (!_reader[0].Equals(DBNull.Value))
                                {
                                    DeliveryFileHistory.Rows.Add(_reader[0], _reader[1], _reader[2]);
                                }
                            }
                        }
                    }
                }
                #endregion

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DeliveryFiles.Rows.Clear();
            DeliveryFileHistory.Rows.Clear();
            DeliveryFileHistoryParams.Rows.Clear();
        }
	
		private void application_cb_SelectedValueChanged(object sender, EventArgs e)
		{
			string key = string.Empty;

			if (((ComboBox)sender).SelectedItem.Equals(Const.EdgeApp))
			{
				key = Const.EdgeProductionPathKey;
			}
			else
				key = Const.SeperiaProductionPathKey;

			string productionPath = ConfigurationManager.AppSettings.Get(key);
			EdgeServicesConfiguration.Load(productionPath);
			
		}

    }
}
