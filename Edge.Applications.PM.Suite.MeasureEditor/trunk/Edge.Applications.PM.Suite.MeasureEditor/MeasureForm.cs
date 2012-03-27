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
using Edge.Applications.PM.Common;
using Edge.Data.Objects;

namespace Edge.Applications.PM.Suite
{
    public partial class MeasureForm : ProductionManagmentBaseForm
	{
        List<ChannelItem> _channelsList;

		public MeasureForm()
		{
			InitializeComponent();
		}

        private List<ChannelItem> getChannels(string SystemDatabase)
        {
            List<ChannelItem> channelsView = null;
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(typeof(Measure), SystemDatabase)))
            {
                sqlCon.Open();
                Dictionary<int, Channel> channels = Channel.GetChannels(sqlCon);
                channelsView = new List<ChannelItem>();
                channelsView.Add(new ChannelItem() { id = 0, name = "Select Channel", channel = null });
                foreach (KeyValuePair<int, Channel> c in channels)
                {
                    channelsView.Add(new ChannelItem() { id = c.Value.ID, name = string.Format("{1}({0})", c.Key, c.Value.Name), channel = c.Value });
                }
             }
            return channelsView;
        }

        private List<AccountItem> getAccounts(string SystemDatabase)
        {
            List<AccountItem> accountsView = null;
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(typeof(Measure), SystemDatabase)))
            {
                sqlCon.Open();
                Dictionary<int, Account> accounts = Account.GetAccounts(sqlCon);
                accountsView = new List<AccountItem>();
                accountsView.Add(new AccountItem() { id = 0, name = "Select Account" });
                foreach (KeyValuePair<int, Account> a in accounts)
                {
                    accountsView.Add(new AccountItem() { id = a.Value.ID, name = string.Format("{1}({0})", a.Key, a.Value.Name) });
                }
            }
            return accountsView;
        }

		private List<Measure> getMeasures(string SystemDatabase, int accountID)
		{
			List<Measure> measures = new List<Measure>();

            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(typeof(Measure), SystemDatabase)))
            {
                sqlCon.Open();
                SqlCommand sqlCommand = DataManager.CreateCommand(AppSettings.Get(typeof(Measure), "GetMeasures.SP"), System.Data.CommandType.StoredProcedure);
                sqlCommand.Parameters["@accountID"].Value = accountID;
                sqlCommand.Parameters["@includeBase"].Value = false;
                sqlCommand.Connection = sqlCon;

                using (var _reader = sqlCommand.ExecuteReader())
                {
                    if (!_reader.IsClosed)
                    {
                        while (_reader.Read())
                        {
                            int i = (int)_reader[0];
                            measures.Add(new Measure(_reader));
                        }
                    }
                }
            }
			return measures;
		}


        private void showMeasures(List<Measure> measures, int channel, int accountID)
        {
            baseMeasuresListView.Items.Clear();
            rendered = false;

            foreach (Measure m in measures)
            {
                ListViewItem lvi = m.ToListViewItem(_channelsList);
                lvi.Tag = m;

                if (!m.IsTarget)
                {
                    if (channel != 0)//Filter on chosen channel (differentiates between bo base measure and bo account measure)
                    {
                        if ((m.AccountID == -1) && (m.IsBo == true) && (m.ChannelID == channel))
                        {
                            lvi.BackColor = Color.White;
                            lvi.ForeColor = System.Drawing.Color.Gray;
                            baseMeasuresListView.Items.Add(lvi);
                        }
                        else if (m.ChannelID == channel)
                        {
                            lvi.BackColor = Color.White;
                            lvi.Checked = true;
                            baseMeasuresListView.Items.Add(lvi);
                        }
                    }
                    else //Show all measures (differentiates between bo base measure and bo account measure)
                    {
                        if ((m.AccountID == -1) && (m.IsBo == true))
                        {
                            lvi.BackColor = Color.White;
                            lvi.ForeColor = System.Drawing.Color.Gray;
                            baseMeasuresListView.Items.Add(lvi);
                        }
                        else
                        {
                            lvi.BackColor = Color.White;
                            lvi.Checked = true;
                            baseMeasuresListView.Items.Add(lvi);
                        }
                    }
                }
            }
            rendered = true;
        }

		private void application_cb_SelectedValueChanged(object sender, EventArgs e)
		{
            List<AccountItem> accountsList = getAccounts(application_cb.SelectedItem.ToString());
            accounts_cb.DataSource = accountsList;
            accounts_cb.DisplayMember = "name";
            accounts_cb.ValueMember = "id";
            accounts_cb.Refresh();

            _channelsList = getChannels(application_cb.SelectedItem.ToString());
            channel_cb.DataSource = _channelsList;
            channel_cb.DisplayMember = "name";
            channel_cb.ValueMember = "id";
            channel_cb.Refresh(); 
		}

        private void showMeasuresBtn_Click(object sender, EventArgs e)
        {
            string systemDatabase = application_cb.SelectedItem.ToString();
            int accountID = (int)accounts_cb.SelectedValue;
            int channelID = (int)channel_cb.SelectedValue;
            List<Measure> measures = getMeasures(systemDatabase, accountID);
            showMeasures(measures, channelID, accountID);
        }

        private void addMeasureBtn_Click(object sender, EventArgs e)
        {
            if (baseMeasuresListView.SelectedItems.Count == 1)
            {
                EditMeasureForm editFrm = new EditMeasureForm((Measure)baseMeasuresListView.SelectedItems[0].Tag);
                editFrm.AddMeasureEvent += new EventHandler(AddMeasureHandler);
                editFrm.editFrm_closed += new EventHandler(editFrm_closed);
                editFrm.Show();
                this.Enabled = false;
            }
        }


        void AddMeasureHandler(object sender, EventArgs e)
        {
            Measure m = (Measure)e;
            if (m.AccountID == -1)
                addMeasureToBD(m);
            else
                editMeasureInDB(m); 
        }

        private int getNewMeasureID()
        {
            int id;
            using (SqlConnection sqlConnection = new SqlConnection((AppSettings.GetConnectionString(typeof(Measure), application_cb.SelectedItem.ToString()))))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("SELECT (MAX(MeasureID)+1) FROM [dbo].[Measure]", sqlConnection))
                {
                    id = (int)sqlCommand.ExecuteScalar();
                }
            }
            return id;
        }

        private void addMeasureToBD(Measure m)
        {
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(typeof(Measure), application_cb.SelectedItem.ToString())))
            {
                sqlCon.Open();
                SqlCommand sqlCommand = DataManager.CreateCommand(
                @"INSERT INTO [dbo].[Measure]([MeasureID],
                [AccountID],[ChannelID],[BaseMeasureID],[SourceName],[DisplayName],[StringFormat],[AcquisitionNum],[IntegrityCheckRequired])
                VALUES( @measureID:Int,@accountID:Int,@channelID:Int,@baseID:Int,@sourceName:Nvarchar,@displayName:Nvarchar,@stringFormat:Nvarchar,@acquisitionNum:Nvarchar,@check:Nvarchar)");

                sqlCommand.Parameters["@measureID"].Value = getNewMeasureID();
                sqlCommand.Parameters["@accountID"].Value = accounts_cb.SelectedValue;
                sqlCommand.Parameters["@channelID"].Value = m.ChannelID;
                sqlCommand.Parameters["@baseID"].Value = m.BaseMeasureID;
                sqlCommand.Parameters["@sourceName"].Value = string.IsNullOrEmpty(m.SourceName) ? DBNull.Value : (object)m.SourceName;
                sqlCommand.Parameters["@displayName"].Value = m.DisplayName;
                sqlCommand.Parameters["@stringFormat"].Value = string.IsNullOrEmpty(m.StringFormat) ? DBNull.Value : (object)m.StringFormat;
                sqlCommand.Parameters["@acquisitionNum"].Value = string.IsNullOrEmpty(m.AcquisitionNum) ? DBNull.Value : (object)m.AcquisitionNum;
                sqlCommand.Parameters["@check"].Value = string.IsNullOrEmpty(m.IntegrityCheckRequired) ? DBNull.Value : (object)m.IntegrityCheckRequired;

                sqlCommand.Connection = sqlCon;
                sqlCommand.ExecuteNonQuery();
            }
            showMeasuresBtn_Click(null, null);
        }

        private void editMeasureInDB(Measure m)
        {
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(typeof(Measure), application_cb.SelectedItem.ToString())))
            {
                bool includeStringFormat = !(m.StringFormat.Equals("no change"));
                bool includeValidation = !(m.IntegrityCheckRequired.Equals("no change"));

                string command = string.Format(@"UPDATE [dbo].[Measure]
                SET [SourceName] = @sourceName:Nvarchar, [DisplayName] = @displayName:Nvarchar, [AcquisitionNum] = @acquisitionNum:Nvarchar{0}{1}
                WHERE [MeasureID] = @measureID:Int and [AccountID] = @accountID:Int and [ChannelID] = @channelID:Int and [BaseMeasureID] = @baseID:Int",
                includeStringFormat ? ", [StringFormat] = @stringFormat:Nvarchar" : "", includeValidation ? ", [IntegrityCheckRequired] = @check:Nvarchar" : ""); 
                
                sqlCon.Open();
                SqlCommand sqlCommand = DataManager.CreateCommand(command);

                sqlCommand.Parameters["@measureID"].Value = m.MeasureID;
                sqlCommand.Parameters["@accountID"].Value = m.AccountID;
                sqlCommand.Parameters["@channelID"].Value = m.ChannelID;
                sqlCommand.Parameters["@baseID"].Value = m.BaseMeasureID;
                sqlCommand.Parameters["@sourceName"].Value = string.IsNullOrEmpty(m.SourceName) ? DBNull.Value : (object)m.SourceName;
                sqlCommand.Parameters["@displayName"].Value = string.IsNullOrEmpty(m.DisplayName) ? DBNull.Value : (object)m.DisplayName;
                sqlCommand.Parameters["@acquisitionNum"].Value = string.IsNullOrEmpty(m.AcquisitionNum) ? DBNull.Value : (object)m.AcquisitionNum;
                if (includeValidation)
                    sqlCommand.Parameters["@check"].Value = string.IsNullOrEmpty(m.IntegrityCheckRequired) ? DBNull.Value : (object)m.IntegrityCheckRequired;
                if(includeStringFormat)
                    sqlCommand.Parameters["@stringFormat"].Value = string.IsNullOrEmpty(m.StringFormat) ? DBNull.Value : (object)m.StringFormat;

                sqlCommand.Connection = sqlCon;
                sqlCommand.ExecuteNonQuery();
            }
            showMeasuresBtn_Click(null, null);
        }

        private void deleteMeasureFromDB(Measure m)
        {
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(typeof(Measure), application_cb.SelectedItem.ToString())))
            {
                sqlCon.Open();
                SqlCommand sqlCommand = DataManager.CreateCommand(
                @"DELETE FROM [dbo].[Measure]
                WHERE [MeasureID] = @measureID:Int and [AccountID] = @accountID:Int");

                sqlCommand.Parameters["@measureID"].Value = m.MeasureID;
                sqlCommand.Parameters["@accountID"].Value = m.AccountID;
                sqlCommand.Connection = sqlCon;
                sqlCommand.ExecuteNonQuery();
            }
            showMeasuresBtn_Click(null, null);
        }

        private void baseMeasuresListView_SelectedIndexChanged(object sender, EventArgs e)
        {  
            if (baseMeasuresListView.SelectedItems.Count == 1)
            {
                Measure m = (Measure)baseMeasuresListView.SelectedItems[0].Tag;
                if (m.AccountID==-1 && m.IsBo)
                    addMeasureBtn.Text = "Add";
                else
                    addMeasureBtn.Text = "Edit";
            }
        }

        private bool Resizing = false;
        private void frm_SizeChanged(object sender, EventArgs e)
        {
            // Don't allow overlapping of SizeChanged calls
            if (!Resizing)
            {
                // Set the resizing flag
                Resizing = true;
                ListView listView = baseMeasuresListView;
                if (listView != null)
                {
                    float totalColumnWidth = 0;
                    // Get the sum of all column tags
                    for (int i = 0; i < listView.Columns.Count; i++)
                        totalColumnWidth += Convert.ToInt32(listView.Columns[i].Tag);

                    // Calculate the percentage of space each column should 
                    // occupy in reference to the other columns and then set the 
                    // width of the column to that percentage of the visible space.
                    for (int i = 0; i < listView.Columns.Count; i++)
                    {
                        float colPercentage = (Convert.ToInt32(listView.Columns[i].Tag) / totalColumnWidth);
                        listView.Columns[i].Width = (int)(colPercentage * listView.ClientRectangle.Width);
                    }
                }
            }
            // Clear the resizing flag
            Resizing = false;
        }

        private bool rendered = false;//So that won't trigger event when loading measures
        private void baseMeasuresListView_ItemChecked(Object sender, ItemCheckedEventArgs e)
        {
            Measure m = (Measure)e.Item.Tag;

            if (rendered)
            {
                if (e.Item.Checked == false)
                {
                    if (m.AccountID == -1)
                    {
                        e.Item.Checked = true;
                    }
                    else
                    {
                        DialogResult result;
                        result = MessageBox.Show("Are you sure you want to remove this measure?", "Delete Measure", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                            deleteMeasureFromDB(m);
                        else
                        {
                            rendered = false;
                            e.Item.Checked = true;
                            rendered = true;
                        }
                    }
                }
                else
                {
                    e.Item.Selected = true;
                    addMeasureBtn_Click(sender, e);
                }
            }
        }

        void editFrm_closed(object sender, EventArgs e)
        {
            showMeasuresBtn_Click(null, null);
            this.Enabled = true;
        }
    }

    public struct AccountItem
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public struct ChannelItem
    {
        public Channel channel;
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Measure : EventArgs
	{
/*
            Measure m;

            //does m.Options contain (IsBackOffice and IsTarget)
            if (((int)m.Options & (MeasureOptions.IsBackOffice | MeasureOptions.IsTarget)) > 0)
            {
                // this is a backoffice measure!!!
            }
*/

		public Measure(SqlDataReader reader)
		{
            try
            {
                MeasureID = Convert.ToInt16(reader["MeasureID"]);
                BaseMeasureID = Convert.ToInt16(reader["BaseMeasureID"]);
                ChannelID = Convert.ToInt16(reader["ChannelID"]);
                AccountID = Convert.ToInt64(reader["AccountID"]);
                Name = Convert.ToString(reader["Name"]);
                DisplayName = Convert.ToString(reader["DisplayName"]);
                OLTP_Table = Convert.ToString(reader["OLTP_Table"]);
                FieldName = Convert.ToString(reader["FieldName"]);
                DWH_Table = Convert.ToString(reader["DWH_Table"]);
                DWH_ProcessedTable = Convert.ToString(reader["DWH_ProcessedTable"]);
                DWH_Name = Convert.ToString(reader["DWH_Name"]);
                DWH_AggregateFunction = Convert.ToString(reader["DWH_AggregateFunction"]);
                StringFormat = Convert.ToString(reader["StringFormat"]);
                IsAbsolute = Convert.ToBoolean(reader["IsAbsolute"]);
                IsTarget = Convert.ToBoolean(reader["IsTarget"]);
                IsAdTest = Convert.ToBoolean(reader["IsAdTest"]);
                IsBo = Convert.ToBoolean(reader["IsBo"]);
                IsDashboard = Convert.ToBoolean(reader["IsDashboard"]);
                AccountLevelMDX = Convert.ToString(reader["AccountLevelMDX"]);
                IsCalculated = Convert.ToBoolean(reader["IsCalculated"]);
                IntegrityCheckRequired = Convert.ToString(reader["IntegrityCheckRequired"]);
                SourceName = reader["SourceName"].Equals(DBNull.Value) ? string.Empty : Convert.ToString(reader["SourceName"]);
                AcquisitionNum = reader["AcquisitionNum"].Equals(DBNull.Value) ? null : Convert.ToString(reader["AcquisitionNum"]);
                TargetMeasureID = reader["TargetMeasureID"].Equals(DBNull.Value) ? string.Empty : Convert.ToString(reader["TargetMeasureID"]);
            }
            catch(InvalidCastException e)
            {
                MessageBox.Show("One of the measure fields is NULL where it shouldn't be.\n"+ e.ToString());
            }
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
        public string AcquisitionNum { set; get; }

        internal ListViewItem ToListViewItem(List<ChannelItem> channels)
        {
            string[] viewItem = new string[8];

            string channelName = channels.Find(ChannelItem => ChannelItem.id == this.ChannelID).name;

            viewItem[1] = channelName.Substring(0, channelName.IndexOf("("));
            viewItem[2] = this.Name.ToString();
            viewItem[3] = this.DisplayName.ToString();
            viewItem[4] = this.SourceName.ToString();
            viewItem[5] = this.StringFormat.ToString();
            viewItem[6] = this.IntegrityCheckRequired.ToString();
            viewItem[7] = AcquisitionNum==null? "" : this.AcquisitionNum.ToString();

            ListViewItem lvi = new ListViewItem(viewItem);
            return lvi;
        }
    }

    public class MeasureView : EventArgs
    {
        Edge.Data.Objects.Measure m;

        internal ListViewItem ToListViewItem(List<ChannelItem> channels)
        {
            string[] viewItem = new string[8];

            Channel channelItem = channels.Find(ChannelItem => ChannelItem.id == m.ChannelID).channel;

            viewItem[1] = channelItem.Name;
            viewItem[2] = m.Name.ToString();
            viewItem[3] = m.DisplayName.ToString();
            viewItem[4] = m.SourceName.ToString();
            viewItem[5] = m.StringFormat.ToString();
            viewItem[6] = m.IntegrityCheckRequired.ToString();
            viewItem[7] = m.AcquisitionNum == null ? "" : m.AcquisitionNum.ToString();

            ListViewItem lvi = new ListViewItem(viewItem);
            return lvi;
        }
    }
}
