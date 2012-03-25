using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Edge_Applications.PM.common
{
	public partial class frmEncryptDecrypt : Form
	{
		public frmEncryptDecrypt()
		{
			InitializeComponent();
		}

        private void decryptBtn_Click(object sender, EventArgs e)
        {
            //Get variables from form
            string inputPswd = inputTxt.Text;
            try
            {
                if (string.IsNullOrEmpty(inputPswd))
                    throw new Exception("Input password is empty");
               
                outputTxt.Text = Edge.Core.Utilities.Encryptor.Dec(inputPswd);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private void encryptBtn_Click(object sender, EventArgs e)
        {
            //Get variables from form
            string inputPswd = inputTxt.Text;
            outputTxt.Text = Edge.Core.Utilities.Encryptor.Enc(inputPswd);
        }

       

       
	}
}
