using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Edge.Applications.PM.Common;
using Edge.Data.Objects;
using System.Data.SqlClient;
using Edge.Core.Configuration;

namespace Edge.Applications.PM.Suite
{
    public partial class EditMeasureForm : ProductionManagmentBaseForm
    {
        public event EventHandler AddMeasureEvent;
        private Measure _measure;
        private Account _account;
        private string _systemDatabase;

        public event EventHandler editFrm_closed;
        public string _IntegrityCheckRequired;
        public Measure _oldMeasure;

        public EditMeasureForm()
        {
            InitializeComponent();
        }

        public EditMeasureForm(Measure m, Account a, string systemDB)
        { 
            InitializeComponent();
            this.TopMost = true; 

            msrLbl.Text += m.BaseMeasureID.ToString();
            msrLbl1.Text += m.ID.ToString();
            chnnlLbl.Text += m.Channel == null ? "-1" : m.Channel.ID.ToString();
            accountLbl.Text += m.Account == null ? "-1" : m.Account.ID.ToString();
            msrNameLbl.Text += m.Name.ToString();
            displayNameTxt.Text = m.DisplayName.ToString();

            this._measure = m;
            this._account = a;
            this._systemDatabase = systemDB;
            this._oldMeasure = new Measure() { AcquisitionNum = m.AcquisitionNum, ID = m.ID, DisplayName = m.DisplayName }; 
            if (!((int)(_measure.Options & MeasureOptions.IsBackOffice) > 0))
            {
                srcNameTxt.Enabled = false;
                acqNumTxt.Enabled = false;                         
            }
            if (_measure.Account == null &&((int)(_measure.Options & MeasureOptions.IsBackOffice) > 0))
                addMeasuresBtn.Text = "Add Measure";
            else
                addMeasuresBtn.Text = "Edit Measure";
        }

        private void addMeasuresBtn_Click(object sender, EventArgs e)
        {
            bool valid = false;
            valid = validateMeasure();
            if (valid)
            {
                if (_measure.Account == null)//Need to add measure
                {
                    _measure.SourceName = srcNameTxt.Text;
                    _measure.DisplayName = displayNameTxt.Text;
                    _measure.StringFormat = stringFromatTxt.Text.Equals("Inherit from base") ? string.Empty : stringFromatTxt.Text;
                    
                    if (acqNumTxt.Text.Equals("Inherit from base")||string.IsNullOrEmpty(acqNumTxt.Text)) 
                        _measure.AcquisitionNum = null; 
                    else
                        _measure.AcquisitionNum = Convert.ToInt32(acqNumTxt.Text);
                   
                    if (yesRadioBtn.Checked)
                        _IntegrityCheckRequired = "True";
                    else if (noRadioBtn.Checked)
                        _IntegrityCheckRequired = "False";
                    else
                        _IntegrityCheckRequired = null;
                }
                else //Need to update measure
                {
                    _measure.SourceName = string.IsNullOrEmpty(srcNameTxt.Text) ? _measure.SourceName : srcNameTxt.Text;
                    _measure.DisplayName = string.IsNullOrEmpty(displayNameTxt.Text) ? _measure.DisplayName : displayNameTxt.Text;
                   
                    /** Edit String Format **/
                    if (stringFromatTxt.Text.Equals("Inherit from base"))
                        _measure.StringFormat = string.Empty;
                    else if (string.IsNullOrEmpty(stringFromatTxt.Text))
                        _measure.StringFormat = "no change";
                    else
                        _measure.StringFormat = stringFromatTxt.Text;

                    /** Edit Aquisition Number **/
                    if (acqNumTxt.Text.Equals("Inherit from base"))
                        _measure.AcquisitionNum = null;
                    else
                        _measure.AcquisitionNum = string.IsNullOrEmpty(acqNumTxt.Text) ? _measure.AcquisitionNum : Convert.ToInt32(acqNumTxt.Text);

                    /** Edit Integrity Check **/
                    if (yesRadioBtn.Checked)
                        _IntegrityCheckRequired = "True";
                    else if (noRadioBtn.Checked)
                        _IntegrityCheckRequired = "False";
                    else if (inheritRadioBtn.Checked)
                        _IntegrityCheckRequired = string.Empty;
                    else
                        _IntegrityCheckRequired = "no change";
                }

                AddMeasureEvent(this, new MeasureView() { m = this._measure, IntegrityCheckRequired = _IntegrityCheckRequired, oldMeasure = _oldMeasure });
                this.Close();
            }
        }

        private bool validateMeasure()
        {
            bool valid = true;
            if (_measure.Account == null)
            {
                if (string.IsNullOrEmpty(displayNameTxt.Text))
                {
                    valid = false;
                    MessageBox.Show("Display name cannot be empty");
                }
                else if ((string.IsNullOrEmpty(srcNameTxt.Text) && (srcNameTxt.Enabled == true)))
                {
                    valid = false;
                    MessageBox.Show("Source name cannot be empty");
                }
            }
            int dummy;
            if(!(acqNumTxt.Text.Equals("Inherit from base"))&&(!string.IsNullOrEmpty(acqNumTxt.Text))&&(Int32.TryParse(acqNumTxt.Text, out dummy)==false))
            {
                valid = false;
                MessageBox.Show("Aquisition Number must be a number");
            }
            if (Int32.TryParse(acqNumTxt.Text, out dummy) == true)  
            {
                if ((isDuplicateAcquisition(dummy)))
                {
                    valid = false;
                    MessageBox.Show("Aquisition Number already exists");
                }
                else if(dummy<1)
                {
                    valid = false;
                    MessageBox.Show("Aquisition Number must be greater than 1");
                }
            }

            return valid;
        }

        private bool isDuplicateAcquisition(int acquisitionNumber)
        {          
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(typeof(Measure), _systemDatabase)))
            {
                sqlCon.Open();
                Dictionary<string, Measure> boMeasures = Measure.GetMeasures(_account, null, sqlCon, MeasureOptions.IsBackOffice, MeasureOptionsOperator.And);
                foreach (KeyValuePair<string, Measure> msr in boMeasures)
                {
                    Measure m = msr.Value;
                    if ((m.AcquisitionNum != null) && (m.AcquisitionNum == acquisitionNumber))
                        return true;
                }
            }
            return false;
        }

        private bool isShown = false;
        private void EditMeasureForm_MouseMove(object sender, MouseEventArgs e)
        {
            if ((srcNameTxt == this.GetChildAtPoint(e.Location))||(acqNumTxt == this.GetChildAtPoint(e.Location)))
            {
                if (!isShown)
                {
                    infoToolTip.Show("Available when editing or adding backoffice measures", this, e.Location);
                    isShown = true;
                }
            }
            else
            {
                infoToolTip.Hide(srcNameTxt);
                isShown = false;
            }
        }

        private void editFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            editFrm_closed(this, e);
        }
    }
}
