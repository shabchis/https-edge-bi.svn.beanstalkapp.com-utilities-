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
using Edge.Data.Pipeline;
using Newtonsoft.Json;
using Edge.Data.Pipeline.Services;

namespace Edge.Application.ProductionManagmentTools
{
    public partial class DataChecks : Form
    {
        public DataChecks()
        {
            InitializeComponent();
        }

        void instance_StateChanged(object sender, Edge.Core.Services.ServiceStateChangedEventArgs e)
        {
            Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;
            if (e.StateAfter == Edge.Core.Services.ServiceState.Ready)
            {
                instance.Start();
                if (level1.Checked) { step1_status.Text = "Ready"; step1_status.Visible = true; }
                if (level2.Checked) { step1_status.Text = "Ready"; step1_status.Visible = true; }
                if (level3.Checked) { step1_status.Text = "Ready"; step1_status.Visible = true; }
            }
            if (e.StateAfter == Edge.Core.Services.ServiceState.Initializing)
            {
                if (level1.Checked) step1_status.Text = "Initializing";
                if (level2.Checked) step1_status.Text = "Initializing";
                if (level3.Checked) step1_status.Text = "Initializing";
            }
            if (e.StateAfter == Edge.Core.Services.ServiceState.Running)
            {
                if (level1.Checked) step1_status.Text = "Running";
                if (level2.Checked) step1_status.Text = "Running";
                if (level3.Checked) step1_status.Text = "Running";
            }
            if (e.StateAfter == Edge.Core.Services.ServiceState.Waiting)
            {
                if (level1.Checked) step1_status.Text = "Waiting";
                if (level2.Checked) step1_status.Text = "Waiting";
                if (level3.Checked) step1_status.Text = "Waiting";
            }
            if (e.StateAfter == Edge.Core.Services.ServiceState.Aborting)
            {
                if (level1.Checked) step1_action.Text = "Aborting";
                if (level2.Checked) step1_action.Text = "Aborting";
                if (level3.Checked) step1_action.Text = "Aborting";
            }
            if (e.StateAfter == Edge.Core.Services.ServiceState.Ended)
            {
                ValidationResult results = null;

                if (level1.Checked) step1_action.Text = "Ended";
                if (level2.Checked) step1_action.Text = "Ended";
                if (level3.Checked) step1_action.Text = "Ended";

                using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
                {
                    sqlCon.Open();
                    SqlCommand sqlCommand = DataManager.CreateCommand(
                        "SELECT [Message] FROM [Source].[dbo].[Log] where [ServiceInstanceID] = @instanceID");
                    sqlCommand.Parameters.Add(new SqlParameter(){ParameterName = "@instanceID", Value = instance.InstanceID,SqlDbType = System.Data.SqlDbType.BigInt});
                    sqlCommand.Connection = sqlCon;

                    using (var _reader = sqlCommand.ExecuteReader())
                    {
                        if (!_reader.IsClosed)
                        {
                            while (_reader.Read())
                            {
                               results = (ValidationResult)JsonConvert.DeserializeObject(_reader[0].ToString(), typeof(ValidationResult));
                            }
                        }
                    }
                }
                if (results == null)
                {

                }

              
            }

        }

        //void instance_OutcomeReported(object sender, Edge.Core.Services.ServiceOutcome e)
        //{
            

        //}

        private void DataChecks_Load(object sender, EventArgs e)
        {
            fromDate.Value = fromDate.Value.AddDays(-1);
            toDate.Value = toDate.Value.AddDays(-1);
            
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
            {
                sqlCon.Open();
                SqlCommand sqlCommand = DataManager.CreateCommand(
                    "SELECT [Account_Name] ,[Account_ID] FROM [seperia].[dbo].[User_GUI_Account] where Status =9 group by [Account_Name] ,[Account_ID] order by [Account_ID]");
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

        private void Start_btn_Click(object sender, EventArgs e)
        {
            bool errors = false;
            ActiveServiceElement serviceElements = new ActiveServiceElement(EdgeServicesConfiguration.Current.Accounts.GetAccount(-1).Services["validation"]);
            
            //Get TimePeriod

            var range = new DateTimeRange()
            {
                Start = new DateTimeSpecification()
                {
                    BaseDateTime = fromDate.Value,
                    Hour = new DateTimeTransformation() { Type = DateTimeTransformationType.Exact, Value = 0 },
                    Boundary = DateTimeSpecificationBounds.Lower
                },

                End = new DateTimeSpecification()
                {
                    BaseDateTime = toDate.Value,
                    Hour = new DateTimeTransformation() { Type = DateTimeTransformationType.Max },
                    Boundary = DateTimeSpecificationBounds.Upper
                }
            };

            serviceElements.Options.Add("fromDate", range.Start.ToDateTime().ToString());
            serviceElements.Options.Add("toDate", range.End.ToDateTime().ToString());
            
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
               // instance.OutcomeReported += new EventHandler<Edge.Core.Services.>(instance_OutcomeReported);
                instance.StateChanged += new EventHandler<Edge.Core.Services.ServiceStateChangedEventArgs>(instance_StateChanged);

                instance.Initialize();
            }

        }

        private void Step1StateChange(object sender, EventArgs e)
        {
            if (level1.Checked)
            {
                step1_lbl.Visible = true;
                step1_progressBar1.Visible = true;
            }
            else
            {
                step1_lbl.Visible = false;
                step1_progressBar1.Visible = false;
            }
        }


    }
    public static class Const
    {
        public static string level1Table = "dbo.Paid_API_AllColumns_v29";
    }
    
}
