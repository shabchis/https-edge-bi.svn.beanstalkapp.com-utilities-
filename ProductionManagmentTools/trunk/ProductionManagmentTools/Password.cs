using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Edge.Core.Utilities;

namespace Edge.Application.ProductionManagmentTools
{
	public partial class Password : Form
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
