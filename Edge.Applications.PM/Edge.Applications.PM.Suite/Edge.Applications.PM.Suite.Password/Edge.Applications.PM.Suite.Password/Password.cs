using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Edge.Core.Utilities;
using Edge.Applications.PM.Common;


namespace Edge.Applications.PM.Suite
{
	public partial class Password : ProductionManagmentBaseForm
	{
		public Password()
		{
			InitializeComponent();
		}

		private void dec_btn_Click(object sender, EventArgs e)
		{
			dec.Text = Encryptor.Dec(enc.Text);
		}

		
	}
}
