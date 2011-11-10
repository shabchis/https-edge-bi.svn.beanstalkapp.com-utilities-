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
        private UpdateStepStatus updateStep;
        public delegate void UpdateProgressBar(ProgressBar progressBar, int val, bool visible);
        private UpdateProgressBar updateProgressBar;
        public delegate void UpdateResults(List<ValidationResult> results);
        private UpdateResults updateResults;
        public delegate void UpdateButton(List<Button> buttons, bool Enable, bool visible = true);
        private UpdateButton updateBtn;
        public delegate void UpdatePictureBox(PictureBox pictureBox, Image image, bool visible = true);
        private UpdatePictureBox updateResultImage;
        public delegate void UpdatePanel(List<Panel> panels, bool enable);
        private UpdatePanel updateStepsPanel;
        private Dictionary<string, Step> Services;
        private List<ValidationResult> results;
        private ResultForm resultsForm;
        private List<Account> _checkedAccountList;
        private List<CheckBox> _checkedChannels;
        private List<CheckBox> _checkedServices;
        private Profile _profile = new Profile();


        public DataChecks()
        {
            InitializeComponent();
            updateStep = new UpdateStepStatus(setLabel);
            updateProgressBar = new UpdateProgressBar(updateProgressBarState);
            updateResults = new UpdateResults(updateResultsDataGrid);
            updateBtn = new UpdateButton(updateButton);
            updateResultImage = new UpdatePictureBox(updateImage);
            updateStepsPanel = new UpdatePanel(updatePanels);

            Services = new Dictionary<string, Step>();
            updatePanels(new List<Panel>() { step1, step2, step3, step4 }, false);

            report_btn.Enabled = false;
            _checkedAccountList = new List<Account>();
            _checkedChannels = new List<CheckBox>();
            _checkedServices = new List<CheckBox>();

            GoogleAdwords.BindingContext= new BindingContext() { };
            
        }

        public void updatePanels(List<Panel> panels, bool enable)
        {
            foreach (Panel panel in panels)
            {
                panel.Enabled = enable;
            }
        }

        public void updateImage(PictureBox pictureBox, Image image, bool visible = true)
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
            foreach (ValidationResult item in results)
            {
                if (item.ResultType == ValidationResultType.Error)
                    resultsForm.ErrorDataGridView.Rows.Add(item.AccountID, item.CheckType, item.Message, item.ChannelID, item.TargetPeriodStart.Date.ToString());

                else if (item.ResultType == ValidationResultType.Warning)
                    resultsForm.WarningDataGridView.Rows.Add(item.AccountID, item.CheckType, item.Message, item.ChannelID, item.TargetPeriodStart.ToString());
                else
                    resultsForm.SuccessDataGridView.Rows.Add(item.AccountID, item.CheckType, item.Message, item.ChannelID, item.TargetPeriodStart.ToString());

                resultsForm.errCountResult_lbl.Text = string.Format("Totals ( {0} )", resultsForm.ErrorDataGridView.RowCount);
                resultsForm.warningsCountResult_lbl.Text = string.Format("Totals ( {0} )", resultsForm.WarningDataGridView.RowCount);
                resultsForm.sucessCountResult_lbl.Text = string.Format("Totals ( {0} )", resultsForm.SuccessDataGridView.RowCount);

            }

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
                if (!String.IsNullOrEmpty(text)) lbl.Text = text;
                lbl.Visible = visible;
            }
        }

        private void DataChecks_Load(object sender, EventArgs e)
        {
            fromDate.Value = DateTime.Today.AddDays(-1);
            toDate.Value = DateTime.Today.AddDays(-1);

            #region Getting Accounts List
            //EdgeServicesConfiguration.Load("Seperia.Services.config");
            //AccountElementCollection accounts= EdgeServicesConfiguration.Current.Accounts;
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
            {
                sqlCon.Open();
                SqlCommand sqlCommand = DataManager.CreateCommand(
                    "SELECT [Account_Name] ,[Account_ID] FROM [seperia].[dbo].[User_GUI_Account] where Status =9 group by [Account_Name] ,[Account_ID]");
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

            if (AccountsCheckedListBox.Items.Count > 0)
            {
                AccountsCheckedListBox.Sorted = true;
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
            DateTimeRange timePeriod;
            string channels, accounts;


            InitUI();
            if (TryGetServicesParams(out timePeriod, out channels, out accounts))
            {
                //Check if no service has been selected from checked boxes
                if (Services.Count == 0)
                {
                    DialogResult dlgRes = new DialogResult();
                    dlgRes = MessageBox.Show("Please select at least one Check level", "Check level Error",
                    MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                    return;
                }
                foreach (var service in Services)
                {
                    try
                    {
                        InitServices(timePeriod, service.Key, channels, accounts);
                    }
                    catch (Exception ex)
                    {
                        Invoke(updateStep, new object[] 
                        { 
                            new List<Label>(){appErrorLbl},ex.Message,false 
                        });
                        Invoke(updateStepsPanel, new object[] { new List<Panel> { AppAlertPanel }, true });
                    }
                }

            }

        }

        private bool TryGetServicesParams(out DateTimeRange timePeriod, out string channelsList, out string accountsList)
        {
            channelsList = "";
            accountsList = "";

            #region TimePeriod
            timePeriod = new DateTimeRange()
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
                    //Boundary = DateTimeSpecificationBounds.Upper
                }
            };
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
                return false;
            }

            channelsList = channels.ToString();
            #endregion

            #region Account Params
            StringBuilder accounts = new StringBuilder();
            if (AccountsCheckedListBox.CheckedItems.Count > 0)
            {
                foreach (string item in AccountsCheckedListBox.CheckedItems)
                {
                    string[] items = item.Split('-');
                    accounts.Append(items[0]);
                    if (!String.IsNullOrEmpty(accounts.ToString()))
                        accounts.Append(',');
                }
            }
            else
            {
                DialogResult dlgRes = new DialogResult();
                dlgRes = MessageBox.Show("Please select at least one account", "Accounts Error",
                MessageBoxButtons.OK,
                 MessageBoxIcon.Error);
                return false;
            }
            accountsList = accounts.ToString().Remove(accounts.Length - 1);
            #endregion

            return true;
        }

        private void InitServices(DateTimeRange timePeriod, string service, string channels, string accounts)
        {
            ActiveServiceElement serviceElements = new ActiveServiceElement(EdgeServicesConfiguration.Current.Accounts.GetAccount(-1).Services[service]);

            // TimePeriod
            serviceElements.Options.Add("fromDate", timePeriod.Start.ToDateTime().ToString());
            serviceElements.Options.Add("toDate", timePeriod.End.ToDateTime().ToString());

            serviceElements.Options.Add("ChannelList", channels);
            serviceElements.Options.Add("AccountsList", accounts);

            string SourceTable;
            if (service.Equals(Const.DeliveryOltpService))
                serviceElements.Options.Add("SourceTable", Const.OltpTable);
            else if (service.Equals(Const.OltpDwhService))
            {
                serviceElements.Options.Add("SourceTable", Const.OltpTable);
                serviceElements.Options.Add("TargetTable", Const.DwhTable);
            }
            else if (service.Equals(Const.MdxOltpService))
                serviceElements.Options.Add("SourceTable", Const.OltpTable);
            else if (service.Equals(Const.MdxDwhService))
                serviceElements.Options.Add("SourceTable", Const.DwhTable);
            else
                //TO DO : Get tabels from configuration.
                throw new Exception("ComparisonTable hasnt been implemented for this service");



            Edge.Core.Services.ServiceInstance instance = Edge.Core.Services.Service.CreateInstance(serviceElements);

            instance.OutcomeReported += new EventHandler(instance_OutcomeReported);
            instance.StateChanged += new EventHandler<Edge.Core.Services.ServiceStateChangedEventArgs>(instance_StateChanged);
            instance.ProgressReported += new EventHandler(instance_ProgressReported);

            instance.Initialize();



        }

        private void InitUI()
        {
            //setting report_btn to be disabled
            Invoke(updateBtn, new object[] { new List<Button>() { report_btn }, false, true });

            //creating new results report
            resultsForm = new ResultForm();
            resultsForm.MdiParent = this.ParentForm;

            //Setting Steps 
            if (level1.Checked)
            {
                Invoke(updateProgressBar, new object[] { step1_progressBar, 0, true });
                Invoke(updateResultImage, new object[] { step1_ErrorImage, global::Edge.Application.ProductionManagmentTools.Properties.Resources.success_icon, false });
                Invoke(updateStep, new object[] 
                { 
                    new List<Label>(){step1_errorsCount,step1_warningCount},"",false 
                });
            }

            if (level2.Checked)
            {
                Invoke(updateProgressBar, new object[] { step2_progressBar, 0, true });
                Invoke(updateResultImage, new object[] { step2_Result, global::Edge.Application.ProductionManagmentTools.Properties.Resources.success_icon, false });
                Invoke(updateStep, new object[] 
                { 
                    new List<Label>(){step2_errorsCount,step1_warningCount},"",false 
                });
            }

            if (level3.Checked)
            {
                Invoke(updateProgressBar, new object[] { step3_progressBar, 0, true });
                Invoke(updateResultImage, new object[] { step3_Result, global::Edge.Application.ProductionManagmentTools.Properties.Resources.success_icon, false });
                Invoke(updateStep, new object[] 
                { 
                    new List<Label>(){step3_errorsCount,step1_warningCount},"",false 
                });
            }

            if (level4.Checked)
            {
                Invoke(updateProgressBar, new object[] { step4_progressBar, 0, true });
                Invoke(updateResultImage, new object[] { step4_Result, global::Edge.Application.ProductionManagmentTools.Properties.Resources.success_icon, false });
                Invoke(updateStep, new object[] 
                { 
                    new List<Label>(){step4_errorsCount,step1_warningCount},"",false 
                });
            }
        }

        void instance_OutcomeReported(object sender, EventArgs e)
        {
            Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;
            Step step;
            Services.TryGetValue(instance.Configuration.Name, out step);

            Invoke(updateProgressBar, new object[] { step.ProgressBar, 100, true });

            if (resultsForm.ErrorDataGridView.RowCount > 0)
            {
                Invoke(updateResultImage, new object[] { step.ResultImage, global::Edge.Application.ProductionManagmentTools.Properties.Resources.failed_icon, true });
                Invoke(updateStep, new object[] 
                { 
                    new List<Label>(){step.ErrorsCount},String.Format("{0}{1}",CountRowsByLevelType(resultsForm.ErrorDataGridView.Rows, instance.Configuration.Name)," errors"),true 
                });


            }

            else if (resultsForm.WarningDataGridView.RowCount > 0)
            {
                Invoke(updateResultImage, new object[] { step.ResultImage, global::Edge.Application.ProductionManagmentTools.Properties.Resources.Warning_icon, true });
                Invoke(updateStep, new object[] 
                { 
                    new List<Label>(){step.WarningCount},String.Format("{0}{1}",   
                    CountRowsByLevelType(resultsForm.WarningDataGridView.Rows, instance.Configuration.Name)," warnings"),true 
                });

                //var rows = from x in resultsForm.WarningDataGridView.Rows.Cast<DataGridViewRow>()
                //           where x.Cells[1].Value.ToString().Equals(instance.Configuration.Name)
                //           select x;
            }
            else
                Invoke(updateResultImage, new object[] { step.ResultImage, global::Edge.Application.ProductionManagmentTools.Properties.Resources.success_icon, true });

            //setting report_btn to be Enable
            //Invoke(updateBtn, new object[] { new List<Button>() { report_btn }, true, true });


        }
        private int CountRowsByLevelType(DataGridViewRowCollection Rows, string type)
        {
            int count = 0;
            foreach (DataGridViewRow row in Rows)
            {
                if (row.Cells[1].Value.ToString().Equals(type)) count++;
            }
            return count;
        }
        void instance_StateChanged(object sender, Edge.Core.Services.ServiceStateChangedEventArgs e)
        {
            Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;
            Step step;
            Services.TryGetValue(instance.Configuration.Name, out step);

            Invoke(updateStep, new object[]
                    {
                        new List<Label>(){step.Status},e.StateAfter.ToString().Equals("Ended")?"Running":e.StateAfter.ToString(),true
                    }
                );

            if (e.StateAfter == Edge.Core.Services.ServiceState.Ready)
            {
                instance.Start();
            }

            if (e.StateAfter == Edge.Core.Services.ServiceState.Ended)
            {
                results = new List<ValidationResult>();

                Invoke(updateProgressBar, new object[] { step.ProgressBar, 70, true });

                #region Getting Instance Log for results
                using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Core.Services", "SystemDatabase")))
                {
                    sqlCon.Open();
                    SqlCommand sqlCommand = DataManager.CreateCommand(
                        "SELECT [Message] FROM [dbo].[Log] where [ServiceInstanceID] = @instanceID");
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
                         new List<Label>(){step.Status},"Ended",true
                    }
                 );

                }
                else
                {
                    //  alert : no services were found
                    Invoke(updateStep, new object[]
                    {
                         new List<Label>(){appErrorLbl},String.Format("Error, cannot find results from service instance id {0}",instance.InstanceID),true
                    });
                }


            }

        }
        private void instance_ProgressReported(object sender, EventArgs e)
        {
            Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;
            Step step;
            Services.TryGetValue(instance.Configuration.Name, out step);
            Invoke(updateProgressBar, new object[] { step.ProgressBar, (int)((ServiceInstance)sender).Progress * 70, true });
        }

        #region On Step Change ( check box )
        private void Step1StateChange(object sender, EventArgs e)
        {
            if (level1.Checked)
            {
                Invoke(updateStepsPanel, new object[] { new List<Panel>() { step1 }, true });
                Services.Add(Const.DeliveryOltpService, new Step
                {
                    Panel = step1,
                    ProgressBar = step1_progressBar,
                    ResultImage = step1_ErrorImage,
                    Status = step1_status,
                    StepName = step1_lbl,
                    WarningCount = step1_warningCount,
                    ErrorsCount = step1_errorsCount
                });
            }
            else
            {
                Invoke(updateStepsPanel, new object[] { new List<Panel>() { step1 }, false });
                Services.Remove(Const.DeliveryOltpService);
            }
        }

        private void Step2StateChange(object sender, EventArgs e)
        {
            if (level2.Checked)
            {
                Invoke(updateStepsPanel, new object[] { new List<Panel>() { step2 }, true });
                Services.Add(Const.OltpDwhService, new Step
                {
                    Panel = step2,
                    ProgressBar = step2_progressBar,
                    ResultImage = step2_Result,
                    Status = step2_status,
                    StepName = step2_lbl,
                    WarningCount = step2_warningCount,
                    ErrorsCount = step2_errorsCount
                });
            }
            else
            {
                Invoke(updateStepsPanel, new object[] { new List<Panel>() { step2 }, false });
                Services.Remove(Const.OltpDwhService);
            }
        }

        private void Step3StateChange(object sender, EventArgs e)
        {
            if (level3.Checked)
            {
                Invoke(updateStepsPanel, new object[] { new List<Panel>() { step3 }, true });
                Services.Add(Const.MdxDwhService, new Step
                {
                    Panel = step3,
                    ProgressBar = step3_progressBar,
                    ResultImage = step3_Result,
                    Status = step3_status,
                    StepName = step3_lbl,
                    WarningCount = step3_warningCount,
                    ErrorsCount = step3_errorsCount
                });
            }
            else
            {
                Invoke(updateStepsPanel, new object[] { new List<Panel>() { step3 }, false });
                Services.Remove(Const.MdxDwhService);
            }
        }

        private void Step4StateChange(object sender, EventArgs e)
        {
            if (level4.Checked)
            {
                Invoke(updateStepsPanel, new object[] { new List<Panel>() { step4 }, true });
                Services.Add(Const.MdxOltpService, new Step
                {
                    Panel = step4,
                    ProgressBar = step4_progressBar,
                    ResultImage = step4_Result,
                    Status = step4_status,
                    StepName = step4_lbl,
                    WarningCount = step4_warningCount,
                    ErrorsCount = step4_errorsCount
                });
            }
            else
            {
                Invoke(updateStepsPanel, new object[] { new List<Panel>() { step4 }, false });
                Services.Remove(Const.MdxOltpService);
            }
        }
        #endregion

        private void report_btn_Click(object sender, EventArgs e)
        {
            SortResultsDataGrid(resultsForm.ErrorDataGridView, ListSortDirection.Ascending);
            SortResultsDataGrid(resultsForm.WarningDataGridView, ListSortDirection.Ascending);
            SortResultsDataGrid(resultsForm.SuccessDataGridView, ListSortDirection.Ascending);
            
            resultsForm.Show();
        }

        private void SortResultsDataGrid(DataGridView dataGridView, ListSortDirection listSortDirection)
        {
            if (dataGridView.RowCount > 0)
            {
                dataGridView.Sort(dataGridView.Columns[0], listSortDirection);
            }
        }

        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAll.Checked)
                for (int index = 0; index < AccountsCheckedListBox.Items.Count; index++)
                {
                    AccountsCheckedListBox.SetItemChecked(index, true);
                }
            else
                for (int index = 0; index < AccountsCheckedListBox.Items.Count; index++)
                {
                    AccountsCheckedListBox.SetItemChecked(index, false);
                }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (profile_cb.SelectedValue.ToString() == "custom")
            {
                DialogResult dlgRes = new DialogResult();
                dlgRes = MessageBox.Show("Do you want to save this profile", "New Profile",
                MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question);

                switch (dlgRes)
                {
                    case DialogResult.Yes:
                        {
                            break;
                        }
                    case System.Windows.Forms.DialogResult.Cancel:
                        {
                            break;
                        }
                }

            }
        }

        private void quick_btn_Click(object sender, EventArgs e)
        {
            level1.Checked = true;
            level2.Checked = false;
            level3.Checked = false;
            level4.Checked = true;
            Start_btn_Click(sender, e);
        }

        private void full_btn_Click(object sender, EventArgs e)
        {
            level1.Checked = true;
            level2.Checked = true;
            level3.Checked = true;
            level4.Checked = true;
            Start_btn_Click(sender, e);
        }

        private void save_btn_Click(object sender, EventArgs e)
        {
			//TO DO : Create Profile in XM: format 

            List<string> checkedAccounts = new List<string>();

            foreach (string item in AccountsCheckedListBox.CheckedItems)
            {
                _profile.Accounts.Add(item.Split('-')[0]);
            }
            _profile.Name = profile_tb.Text;

            bool result = _profile.TrySave();
            
        }

        private void Channel_CheckedChanged(object sender)
        {
            //TO DO : add to Profile temp
        }

        private void CheckLevel_CheckedChanged(object sender)
        {
			//TO DO : add to Profile temp
        }

        private void GoogleAdwords_CheckedChanged(object sender, EventArgs e)
        {
            Channel_CheckedChanged(sender);
        }

        private void Facebook_CheckedChanged(object sender, EventArgs e)
        {
            Channel_CheckedChanged(sender);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Channel_CheckedChanged(sender);
        }

        private void level1_CheckedChanged(object sender, EventArgs e)
        {
            CheckLevel_CheckedChanged(sender);
        }

        private void level3_CheckedChanged(object sender, EventArgs e)
        {
            CheckLevel_CheckedChanged(sender);
        }

        private void level2_CheckedChanged(object sender, EventArgs e)
        {
            CheckLevel_CheckedChanged(sender);
        }

        private void level4_CheckedChanged(object sender, EventArgs e)
        {
            CheckLevel_CheckedChanged(sender);
        }

    }
    public static class Const
    {
        // Tabels
        public static string OltpTable = "dbo.Paid_API_AllColumns_v29";
        public static string DwhTable = "Dwh_Fact_PPC_Campaigns";

        //Services
        public static string DeliveryOltpService = "DeliveryOltpValidation";
        public static string OltpDwhService = "DwhOltpValidation";
        public static string MdxDwhService = "MdxDwhValidation";
        public static string MdxOltpService = "MdxOltpValidation";
    }

    public class Step
    {
        public Panel Panel { get; set; }
        public Label StepName { get; set; }
        public ProgressBar ProgressBar { get; set; }
        public Label Status { get; set; }
        public PictureBox ResultImage { get; set; }
        public Label WarningCount { get; set; }
        public Label ErrorsCount { get; set; }
    }
    public class Account
    {
        public int Id { set; get; }
        public string AccountName { set; get; }
    }
    
}
