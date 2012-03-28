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

namespace Edge.Applications.PM.Suite
{
    public partial class EditMeasureForm : ProductionManagmentBaseForm
    {
        public event EventHandler AddMeasureEvent;
        private Measure _measure;

        public event EventHandler editFrm_closed;
        public string _IntegrityCheckRequired;

        public EditMeasureForm()
        {
            InitializeComponent();
        }

        public EditMeasureForm(Measure m)
        { 
            InitializeComponent();
            this.TopMost = true; 

            msrLbl.Text += m.BaseMeasureID.ToString();
            msrLbl1.Text += m.ID.ToString();
            chnnlLbl.Text += m.ChannelID.ToString();
            accountLbl.Text += m.Account == null ? "-1" : m.Account.ID.ToString();
            msrNameLbl.Text += m.Name.ToString();
            displayNameTxt.Text = m.DisplayName.ToString();

            this._measure = m;

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
                        Convert.ToInt32(acqNumTxt.Text);
                   
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

                AddMeasureEvent(this, new MeasureView() { m = this._measure, IntegrityCheckRequired = _IntegrityCheckRequired });
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
            
            return valid;
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
