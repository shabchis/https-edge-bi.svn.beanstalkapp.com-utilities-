using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Edge.Core.Configuration;
using System.Data.SqlClient;
using Edge.Core.Data;

namespace Edge.Application.ProductionManagmentTools
{
    public partial class DataChecks : Form
    {
        public DataChecks()
        {
            InitializeComponent();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            bool errors = false;
            ActiveServiceElement serviceElements = new ActiveServiceElement(EdgeServicesConfiguration.Current.Accounts.GetAccount(-1).Services["validation"]);
            
            //Get CheckLevel
            if (level1.Checked)
                serviceElements.Options.Add("ComparisonTable", Const.level1Table);

            //Creating Accounts Param
            StringBuilder accounts = new StringBuilder();
            if (AccountsCheckedListBox.CheckedItems.Count > 0)
            {
                foreach (string item in AccountsCheckedListBox.CheckedItems)
                {
                    string[] items = item.Split('-');
                    accounts.Append(items[0]);
                    accounts.Append(',');
                }
                string accountsList = accounts.ToString().Remove(accounts.ToString().Length - 1);
                serviceElements.Options.Add("AccountsList", accountsList);
            }
            else
            {
                DialogResult dlgRes = new DialogResult();
                dlgRes = MessageBox.Show("Please select at least one account", "Accounts Error",
                MessageBoxButtons.OK,
                 MessageBoxIcon.Error);
                //  if (dlgRes == DialogResult.OK) { System.Windows.Forms.Application.Exit(); };
                errors = true;
            }

            //Creating Channel Param
            StringBuilder channels = new StringBuilder();
            if (Facebook.Checked == true)
            {
                if (!String.IsNullOrEmpty(channels.ToString()))
                    channels.Append(',');
                channels.Append(6); // add facebook channel id code
            }
            if (GoogleAdwords.Checked == true)
            {
                if (!String.IsNullOrEmpty(channels.ToString()))
                    channels.Append(',');
                channels.Append(1); // add facebook channel id code
            }

            if (string.IsNullOrEmpty(channels.ToString()))
            {
                DialogResult dlgRes = new DialogResult();
                dlgRes = MessageBox.Show("Please select at least one Data type", "Data type Error",
                MessageBoxButtons.OK,
                 MessageBoxIcon.Error);
                errors = true;
            }
            else
                serviceElements.Options.Add("ChannelList",channels.ToString());

            if (!errors)
            {
                Edge.Core.Services.ServiceInstance instance = Edge.Core.Services.Service.CreateInstance(serviceElements);
                instance.OutcomeReported += new EventHandler(instance_OutcomeReported);
                instance.StateChanged += new EventHandler<Edge.Core.Services.ServiceStateChangedEventArgs>(instance_StateChanged);

                instance.Initialize();
            }

        }

        void instance_StateChanged(object sender, Edge.Core.Services.ServiceStateChangedEventArgs e)
        {
            if (e.StateAfter == Edge.Core.Services.ServiceState.Ready)
            {
                Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;
                instance.Start();
            }

        }

        void instance_OutcomeReported(object sender, EventArgs e)
        {

        }

        private void DataChecks_Load(object sender, EventArgs e)
        {

            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
            {
                sqlCon.Open();
                SqlCommand sqlCommand = DataManager.CreateCommand(
                    "SELECT [Account_Name] ,[Account_ID] FROM [Seperia].[dbo].[User_GUI_Account] where Status =9 group by [Account_Name] ,[Account_ID] order by [Account_ID]");
                sqlCommand.Connection = sqlCon;

                using (var _reader = sqlCommand.ExecuteReader())
                {
                    if (!_reader.IsClosed)
                    {
                        while (_reader.Read())
                        {
                            AccountsCheckedListBox.Items.Add(string.Format("{0}-{1}", _reader[1], _reader[0]));
                        }
                    }
                }
            }

        }

        private void allAccounts_CheckedChanged(object sender, EventArgs e)
        {
            //foreach (var item in AccountsCheckedListBox.)
            //{
            //    ((CheckBox)item).Checked = true;
            //}
        }



    }
    public static class Const
    {
        public static string level1Table = "Paid_API_AllColumns_v29";
    }
}
