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
using Edge.Core.Services;

namespace Edge.Application.ProductionManagmentTools
{
    public partial class DataChecks : Form
    {
        public delegate void UpdateStepStatus(List<Label> lbls, string text, bool visible);
        UpdateStepStatus updateStep;
        public delegate void UpdateProgressBar(ProgressBar progressBar, int val, bool visible);
        UpdateProgressBar updateProgressBar;
        public delegate void UpdateResults(List<ValidationResult> results);
        UpdateResults updateResults;
        public delegate void UpdateButton(List<Button> buttons, bool Enable, bool visible = true);
        UpdateButton updateBtn;
        public delegate void UpdatePictureBox(PictureBox pictureBox, Image image, bool visible = true);
        UpdatePictureBox updateResultImage;


        ResultForm resultsForm;

        public DataChecks()
        {
            InitializeComponent();
            updateStep = new UpdateStepStatus(setLabel);
            updateProgressBar = new UpdateProgressBar(updateProgressBarState);
            updateResults = new UpdateResults(updateResultsDataGrid);
            updateBtn = new UpdateButton(updateButton);
            updateResultImage = new UpdatePictureBox(updateImage);

            report_btn.Enabled = false;
            //resultsForm = new ResultForm();
            //resultsForm.MdiParent = this.ParentForm;
        }
        public void updateImage(PictureBox pictureBox, Image image , bool visible = true)
        {
            
            pictureBox.Image = image;
            pictureBox.Visible = visible;

        }

        public void updateButton(List<Button> Buttons, bool Enable, bool Visible = true)
        {
            foreach (Button btn in Buttons)
            {
                btn.Enabled = Enable;
                btn.Visible = Visible;
            }
        }

        public void updateResultsDataGrid(List<ValidationResult> results)
        {
            resultsForm = new ResultForm();
            resultsForm.MdiParent = this.ParentForm;

            foreach (ValidationResult item in results)
            {
                if (item.ResultType == ValidationResultType.Error)
                    resultsForm.ErrorDataGridView.Rows.Add(item.AccountID, item.CheckType, item.Message, item.ChannelID, item.TargetPeriodStart.Date.ToString());

                else if (item.ResultType == ValidationResultType.Warning)
                    resultsForm.WarningDataGridView.Rows.Add(item.AccountID, item.CheckType, item.Message, item.ChannelID, item.TargetPeriodStart.ToString());
                else
                    resultsForm.SuccessDataGridView.Rows.Add(item.AccountID, item.CheckType, item.Message, item.ChannelID, item.TargetPeriodStart.ToString());

            }
            //TO DO : Change Image By Step
            if (resultsForm.ErrorDataGridView.RowCount > 0)
                Invoke(updateResultImage, new object[] { step1_ErrorImage, global::Edge.Application.ProductionManagmentTools.Properties.Resources.failed_icon, true });
            else if (resultsForm.WarningDataGridView.RowCount > 0 )
                Invoke(updateResultImage, new object[] { step1_ErrorImage, global::Edge.Application.ProductionManagmentTools.Properties.Resources.Warning_icon,true });
            else
                Invoke(updateResultImage, new object[] { step1_ErrorImage, global::Edge.Application.ProductionManagmentTools.Properties.Resources.success_icon, true });

            report_btn.Enabled = true;

        }

        public void updateProgressBarState(ProgressBar progressBar, int val, bool visible)
        {
            progressBar.Value = val;
            progressBar.Visible = visible;
        }

        public void setLabel(List<Label> lbls, string text, bool visible)
        {
            foreach (Label lbl in lbls)
            {
                lbl.Text = text;
                lbl.Visible = visible;
            }
        }

        void instance_StateChanged(object sender, Edge.Core.Services.ServiceStateChangedEventArgs e)
        {
            Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;

            //setting labels on state change

            // TO DO : check type of Sender ( OLTP , DWH ... ) than update status.
            List<Label> checkedSteps = new List<Label>();
            if (level1.Checked) checkedSteps.Add(step1_status);
            if (level2.Checked) checkedSteps.Add(step2_status);
            if (level3.Checked) checkedSteps.Add(step3_status);
            Invoke(updateStep, new object[]
                    {
                        checkedSteps,e.StateAfter.ToString().Equals("Ended")?"Running":e.StateAfter.ToString(),true
                    }
                );

            if (e.StateAfter == Edge.Core.Services.ServiceState.Ready)
            {
                instance.Start();
            }

            if (e.StateAfter == Edge.Core.Services.ServiceState.Ended)
            {
                List<ValidationResult> results = new List<ValidationResult>();
                Invoke(updateProgressBar, new object[] { step1_progressBar, 70, true });

                #region Getting Instance Log
                using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
                {
                    sqlCon.Open();
                    SqlCommand sqlCommand = DataManager.CreateCommand(
                        "SELECT [Message] FROM [Source].[dbo].[Log] where [ServiceInstanceID] = @instanceID");
                    sqlCommand.Parameters.Add(new SqlParameter() { ParameterName = "@instanceID", Value = instance.InstanceID, SqlDbType = System.Data.SqlDbType.BigInt });
                    sqlCommand.Connection = sqlCon;

                    using (var _reader = sqlCommand.ExecuteReader())
                    {
                        if (!_reader.IsClosed)
                        {
                            while (_reader.Read())
                            {
                                results.Add((ValidationResult)JsonConvert.DeserializeObject(_reader[0].ToString(), typeof(ValidationResult)));
                            }
                        }
                    }
                }
                #endregion

                if (results.Capacity > 0)
                {
                    Invoke(updateResults, new object[] { results });
                    Invoke(updateStep, new object[]
                    {
                        checkedSteps,"Ended",true
                    }
                 );

                }
                else
                {
                    //TO DO :  alert : no services were found
                }


            }

        }

        private void DataChecks_Load(object sender, EventArgs e)
        {
            fromDate.Value = fromDate.Value.AddDays(-1);
            toDate.Value = toDate.Value.AddDays(-1);

            #region Getting Accounts List
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
            #endregion

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
            Invoke(updateProgressBar, new object[] { step1_progressBar, 0, true });
            Invoke(updateBtn, new object[] { new List<Button>() { report_btn }, false, true });
            Invoke(updateResultImage, new object[] { step1_ErrorImage, global::Edge.Application.ProductionManagmentTools.Properties.Resources.success_icon, false });
            ActiveServiceElement serviceElements = new ActiveServiceElement(EdgeServicesConfiguration.Current.Accounts.GetAccount(-1).Services["validation"]);

            #region TimePeriod
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
            #endregion

            //Get CheckLevel
            if (level1.Checked)
                serviceElements.Options.Add("ComparisonTable", Const.level1Table);
            //TO DO : add Dwh , Mdx tabels

            //Creating Accounts Param
            #region Account Params
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
            #endregion

            #region Channel Params
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
                serviceElements.Options.Add("ChannelList", channels.ToString());
            #endregion

            if (!errors)
            {
                Edge.Core.Services.ServiceInstance instance = Edge.Core.Services.Service.CreateInstance(serviceElements);
                instance.OutcomeReported += new EventHandler(instance_OutcomeReported);
                instance.StateChanged += new EventHandler<Edge.Core.Services.ServiceStateChangedEventArgs>(instance_StateChanged);
                instance.ProgressReported += new EventHandler(instance_ProgressReported);

                instance.Initialize();
            }

        }

        void instance_OutcomeReported(object sender, EventArgs e)
        {
            Invoke(updateProgressBar, new object[] { step1_progressBar, 100, true });

        }

        private void instance_ProgressReported(object sender, EventArgs e)
        {
            Invoke(updateProgressBar, new object[] { step1_progressBar, (int)((ServiceInstance)sender).Progress * 70, true });
        }

        private void Step1StateChange(object sender, EventArgs e)
        {
            if (level1.Checked)
            {
                step1_lbl.Visible = true;
                step1_progressBar.Visible = true;
            }
            else
            {
                step1_lbl.Visible = false;
                step1_progressBar.Visible = false;
            }
        }

        private void report_btn_Click(object sender, EventArgs e)
        {
            resultsForm.Show();
        }






    }
    public static class Const
    {
        public static string level1Table = "dbo.Paid_API_AllColumns_v29";
    }

}
