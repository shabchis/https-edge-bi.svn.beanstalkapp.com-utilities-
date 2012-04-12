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
        private List<ChannelItem> _channelsList;
        private List<AccountItem> _accountsList;

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
            EdgeServicesConfiguration.Load("Edge.Applications.PM.Suite.MeasureEditor.exe.config");
            string p = EdgeServicesConfiguration.Current.CurrentConfiguration.FilePath;
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(typeof(Measure), SystemDatabase)))
            {
                sqlCon.Open();
                Dictionary<int, Account> accounts = Account.GetAccounts(sqlCon);
                _accountsList = new List<AccountItem>();
                Account generalAccount = new Account() { ID = -1, Name = "Unkwon" };
                _accountsList.Add(new AccountItem() { id = -1, name = "Select Account", account = generalAccount });
                foreach (KeyValuePair<int, Account> a in accounts)
                {
                    _accountsList.Add(new AccountItem() { id = a.Value.ID, name = string.Format("{1}({0})", a.Key, a.Value.Name), account = a.Value });
                }
            }
            return _accountsList;
        }

        private List<MeasureView> getMeasures(string SystemDatabase, Account account)
        {
            List<MeasureView> measuresList = new List<MeasureView>();
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(typeof(Measure), SystemDatabase)))
            {
                sqlCon.Open();
                Dictionary<string, Measure> measures = Measure.GetMeasures(account, null, sqlCon, MeasureOptions.IsTarget, MeasureOptionsOperator.Not);
                foreach (KeyValuePair<string, Measure> msr in measures)
                {
                    measuresList.Add(new MeasureView() { m = msr.Value });
                }
            }
            return measuresList;
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


        private void showMeasures(List<MeasureView> measures, int channel, int accountID)
        {
            baseMeasuresListView.Items.Clear();
            rendered = false;
            
            foreach (MeasureView msr in measures)
            {
                ListViewItem lvi = msr.ToListViewItem(_channelsList);
                lvi.Tag = msr.m;
                int msrChannelID = msr.m.Channel == null ? -1 : msr.m.Channel.ID;

                if (channel != 0)//Filter on chosen channel (differentiates between bo base measure and bo account measure)
                {
                    if ((msr.m.Account == null) && ((int)(msr.m.Options & MeasureOptions.IsBackOffice) > 0) && (msrChannelID == channel))
                    {
                        lvi.BackColor = Color.White;
                        lvi.ForeColor = System.Drawing.Color.Gray;
                        baseMeasuresListView.Items.Add(lvi);
                    }
                    else if (msrChannelID == channel)
                    {
                        lvi.BackColor = Color.White;
                        lvi.Checked = true;
                        baseMeasuresListView.Items.Add(lvi);
                    }
                }
                else //Show all measures (differentiates between bo base measure and bo account measure)
                {
                    if ((msr.m.Account == null) && ((int)(msr.m.Options & MeasureOptions.IsBackOffice) > 0))
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
            rendered = true;
        }

        private void showMeasuresBtn_Click(object sender, EventArgs e)
        {
            if (application_cb.SelectedIndex > -1)
            {
                string systemDatabase = application_cb.SelectedItem.ToString();
                Account account = _accountsList.Find(AccountItem => AccountItem.id == (int)accounts_cb.SelectedValue).account;
                //int accountID = account == null ? -1 : account.ID;
                int channelID = (int)channel_cb.SelectedValue;
                List<MeasureView> measures = getMeasures(systemDatabase, account);
                measures.Sort((x, y) => string.Compare(x.m.Name, y.m.Name));
                showMeasures(measures, channelID, account.ID);
            }
        }

        private void addMeasureBtn_Click(object sender, EventArgs e)
        {
            if (baseMeasuresListView.SelectedItems.Count == 1)
            {
                Account account = _accountsList.Find(AccountItem => AccountItem.id == (int)accounts_cb.SelectedValue).account;
                EditMeasureForm editFrm = new EditMeasureForm((Measure)baseMeasuresListView.SelectedItems[0].Tag, account, application_cb.SelectedItem.ToString());
                editFrm.AddMeasureEvent += new EventHandler(AddMeasureHandler);
                editFrm.editFrm_closed += new EventHandler(editFrm_closed);
                editFrm.Show();
                this.Enabled = false;
            }
        }


        void AddMeasureHandler(object sender, EventArgs e)
        {
            MeasureView msr = (MeasureView)e;
            if (msr.m.Account == null)
                addMeasureToBD(msr);
            else
                editMeasureInDB(msr);
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

        private void addMeasureToBD(MeasureView msr)
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
                sqlCommand.Parameters["@channelID"].Value = msr.m.Channel == null ? "-1" : msr.m.Channel.ID.ToString();
                sqlCommand.Parameters["@baseID"].Value = msr.m.BaseMeasureID;
                sqlCommand.Parameters["@sourceName"].Value = string.IsNullOrEmpty(msr.m.SourceName) ? DBNull.Value : (object)msr.m.SourceName;
                sqlCommand.Parameters["@displayName"].Value = msr.m.DisplayName;
                sqlCommand.Parameters["@stringFormat"].Value = string.IsNullOrEmpty(msr.m.StringFormat) ? DBNull.Value : (object)msr.m.StringFormat;
                sqlCommand.Parameters["@acquisitionNum"].Value = msr.m.AcquisitionNum == null ? DBNull.Value : (object)msr.m.AcquisitionNum;
                sqlCommand.Parameters["@check"].Value = string.IsNullOrEmpty(msr.IntegrityCheckRequired) ? DBNull.Value : (object)msr.IntegrityCheckRequired;

                sqlCommand.Connection = sqlCon;
                sqlCommand.ExecuteNonQuery();
            }
            setAcquisitionCPA();
            showMeasuresBtn_Click(null, null);
        }

        private void editMeasureInDB(MeasureView msr)
        {
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(typeof(Measure), application_cb.SelectedItem.ToString())))
            {
                bool includeStringFormat = !(msr.m.StringFormat.Equals("no change"));
                bool includeValidation = !(msr.IntegrityCheckRequired.Equals("no change"));

                string command = string.Format(@"UPDATE [dbo].[Measure]
                SET [SourceName] = @sourceName:Nvarchar, [DisplayName] = @displayName:Nvarchar, [AcquisitionNum] = @acquisitionNum:Nvarchar{0}{1}
                WHERE [MeasureID] = @measureID:Int and [AccountID] = @accountID:Int and [ChannelID] = @channelID:Int and [BaseMeasureID] = @baseID:Int",
                includeStringFormat ? ", [StringFormat] = @stringFormat:Nvarchar" : "", includeValidation ? ", [IntegrityCheckRequired] = @check:Nvarchar" : "");

                sqlCon.Open();
                SqlCommand sqlCommand = DataManager.CreateCommand(command);

                sqlCommand.Parameters["@measureID"].Value = msr.m.ID;
                sqlCommand.Parameters["@accountID"].Value = msr.m.Account.ID;
                sqlCommand.Parameters["@channelID"].Value = msr.m.Channel == null ? "-1" : msr.m.Channel.ID.ToString();
                sqlCommand.Parameters["@baseID"].Value = msr.m.BaseMeasureID;
                sqlCommand.Parameters["@sourceName"].Value = string.IsNullOrEmpty(msr.m.SourceName) ? DBNull.Value : (object)msr.m.SourceName;
                sqlCommand.Parameters["@displayName"].Value = string.IsNullOrEmpty(msr.m.DisplayName) ? DBNull.Value : (object)msr.m.DisplayName;
                sqlCommand.Parameters["@acquisitionNum"].Value = msr.m.AcquisitionNum == null ? DBNull.Value : (object)msr.m.AcquisitionNum;
                if (includeValidation)
                    sqlCommand.Parameters["@check"].Value = string.IsNullOrEmpty(msr.IntegrityCheckRequired) ? DBNull.Value : (object)msr.IntegrityCheckRequired;
                if (includeStringFormat)
                    sqlCommand.Parameters["@stringFormat"].Value = string.IsNullOrEmpty(msr.m.StringFormat) ? DBNull.Value : (object)msr.m.StringFormat;

                sqlCommand.Connection = sqlCon;
                sqlCommand.ExecuteNonQuery();
            }
            setAcquisitionCPA();
            showMeasuresBtn_Click(null, null);
        }

        private void deleteMeasureFromDB(Measure m)
        {
            if (m.AcquisitionNum != null)
                deleteAcquisitionCPA(m);
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(typeof(Measure), application_cb.SelectedItem.ToString())))
            {
                sqlCon.Open();
                SqlCommand sqlCommand = DataManager.CreateCommand(
                @"DELETE FROM [dbo].[Measure]
                WHERE [MeasureID] = @measureID:Int and [AccountID] = @accountID:Int");

                sqlCommand.Parameters["@measureID"].Value = m.ID;
                sqlCommand.Parameters["@accountID"].Value = m.Account.ID;
                sqlCommand.Connection = sqlCon;
                sqlCommand.ExecuteNonQuery();
            }

            showMeasuresBtn_Click(null, null);
            
        }

        private void setAcquisitionCPA()
        {
            string systemDatabase = application_cb.SelectedItem.ToString();
            Account account = _accountsList.Find(AccountItem => AccountItem.id == (int)accounts_cb.SelectedValue).account;
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(typeof(Measure), systemDatabase)))
            {
                sqlCon.Open();
                Dictionary<string, Measure> boMeasures = Measure.GetMeasures(account, null, sqlCon, MeasureOptions.IsBackOffice, MeasureOptionsOperator.And);
                Dictionary<string, Measure> isTargetMsrs = Measure.GetMeasures(account, null, sqlCon, MeasureOptions.IsTarget, MeasureOptionsOperator.And);
                foreach (KeyValuePair<string, Measure> msr in boMeasures)
                {
                    Measure m = msr.Value;
                    if (m.AcquisitionNum != null)
                    {
                        string baseMeasureID = null;
                        string measureID = null;
                        bool update = false;
                        bool add = false;
                        foreach (KeyValuePair<string, Measure> targeted in isTargetMsrs)
                        {
                            if (targeted.Key.Equals(string.Format("Acquisition{0}_CPA", m.AcquisitionNum.ToString())))
                            {
                                if (targeted.Value.Account != null)
                                {
                                    update = true;
                                    measureID = targeted.Value.ID.ToString();
                                }
                                else
                                {
                                    baseMeasureID = targeted.Value.BaseMeasureID.ToString();
                                    add = true;
                                }
                            }                                                            
                        }

                        if (update)
                        {
                            SqlCommand sqlCommand = DataManager.CreateCommand(@"UPDATE [dbo].[Measure]
                             SET [DisplayName] = @displayName:Nvarchar WHERE [MeasureID] = @measureID:Int");

                            sqlCommand.Parameters["@measureID"].Value = measureID;
                            sqlCommand.Parameters["@displayName"].Value = string.Format("CPA ({0})", m.DisplayName);
                            sqlCommand.Connection = sqlCon;
                            sqlCommand.ExecuteNonQuery();
                        }
                        else if (add)
                        {
                            SqlCommand sqlCommand = DataManager.CreateCommand(@"INSERT INTO [dbo].[Measure]
                             ([MeasureID],[AccountID],[ChannelID],[BaseMeasureID],[DisplayName])
                             VALUES( @measureID:Int,@accountID:Int,@channelID:Int,@baseID:Int,@displayName:Nvarchar)");

                            sqlCommand.Parameters["@measureID"].Value = getNewMeasureID();
                            sqlCommand.Parameters["@accountID"].Value = account.ID;
                            sqlCommand.Parameters["@channelID"].Value = m.Channel == null ? "-1" : m.Channel.ID.ToString();
                            sqlCommand.Parameters["@baseID"].Value = baseMeasureID;
                            sqlCommand.Parameters["@displayName"].Value = string.Format("CPA ({0})", m.DisplayName);
                            sqlCommand.Connection = sqlCon;
                            sqlCommand.ExecuteNonQuery();
                        }
                    }                 
                }
            }
        }

        private void deleteAcquisitionCPA(Measure m)
        {
            string systemDatabase = application_cb.SelectedItem.ToString();
            Account account = _accountsList.Find(AccountItem => AccountItem.id == (int)accounts_cb.SelectedValue).account;
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(typeof(Measure), systemDatabase)))
            {
                sqlCon.Open();
                Dictionary<string, Measure> isTargetMsrs = Measure.GetMeasures(account, null, sqlCon, MeasureOptions.IsTarget, MeasureOptionsOperator.And);
                foreach (KeyValuePair<string, Measure> targeted in isTargetMsrs)
                {
                    if ((targeted.Key.Equals(string.Format("Acquisition{0}_CPA", m.AcquisitionNum.ToString())))&&(targeted.Value.DisplayName.Equals(string.Format("CPA ({0})", m.DisplayName))))
                    {
                        SqlCommand sqlCommand = DataManager.CreateCommand( @"DELETE FROM [dbo].[Measure]
                         WHERE [MeasureID] = @measureID:Int and [AccountID] = @accountID:Int");
                        sqlCommand.Parameters["@measureID"].Value = targeted.Value.ID.ToString();
                        sqlCommand.Parameters["@accountID"].Value = account.ID.ToString();
                        sqlCommand.Connection = sqlCon;
                        sqlCommand.ExecuteNonQuery();
                    }
                }

            }
        }

        private void baseMeasuresListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (baseMeasuresListView.SelectedItems.Count == 1)
            {
                Measure m = (Measure)baseMeasuresListView.SelectedItems[0].Tag;
                if (m.Account == null && ((int)(m.Options & MeasureOptions.IsBackOffice) > 0))
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
                    if (m.Account == null)
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
        public Account account;
        public int id { get; set; }
        public string name { get; set; }
    }

    public struct ChannelItem
    {
        public Channel channel;
        public int id { get; set; }
        public string name { get; set; }
    }

    public class MeasureView : EventArgs
    {
        public Measure m;
        public string IntegrityCheckRequired;

        internal ListViewItem ToListViewItem(List<ChannelItem> channels)
        {
            string[] viewItem = new string[8];
            int id = m.Channel == null ? -1 : m.Channel.ID;

            Channel channelItem = channels.Find(ChannelItem => ChannelItem.id == id).channel;

            viewItem[1] = channelItem.Name;
            viewItem[2] = m.Name.ToString();
            viewItem[3] = m.DisplayName.ToString();
            viewItem[4] = m.SourceName.ToString();
            viewItem[5] = m.StringFormat.ToString();
            viewItem[6] = (m.Options & MeasureOptions.ValidationRequired) > 0 ? "true" : "false";
            viewItem[7] = m.AcquisitionNum == null ? "" : m.AcquisitionNum.ToString();

            ListViewItem lvi = new ListViewItem(viewItem);
            return lvi;
        }
    }

}
