using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace Edge.Application.ProductionManagmentTools
{
	public partial class main : Form
	{
		private int childFormNumber = 0;

		public main()
		{
			InitializeComponent();
		}

		private void ShowNewForm(object sender, EventArgs e)
		{
			Form childForm = new Form();
			childForm.MdiParent = this;
			childForm.Text = "Window " + childFormNumber++;
			childForm.Show();
		}

		private void OpenFile(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				string FileName = openFileDialog.FileName;
			}
		}

		private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				string FileName = saveFileDialog.FileName;
			}
		}

		private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void CutToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			
		}

		private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			
		}

		private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.Cascade);
		}

		private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileVertical);
		}

		private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.ArrangeIcons);
		}

		private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (Form childForm in MdiChildren)
			{
				childForm.Close();
			}
		}

		private void getAccountMeasuersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//Form2 f = new Form2();
			//f.MdiParent = this;
			//f.Show();
		}

		private void googleToolStripMenuItem_Click(object sender, EventArgs e)
		{
            //GoogleApiUtility.Form1 f = new GoogleApiUtility.Form1();
            //f.MdiParent = this;
            //f.WindowState = FormWindowState.Maximized;
            //f.Show();
		}

		private void runToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DataChecks f = new DataChecks();
			f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;
			f.Show();
            
		}

		private void decToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Password f = new Password();
			f.MdiParent = this;
			f.Show();
		}

        private void deliverySearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeliverySearch f = new DeliverySearch();
            f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }

        private void getAPISettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FaceBookSettingsRetriever.Form1 f = new FaceBookSettingsRetriever.Form1();
            //f.MdiParent = this;
            //f.WindowState = FormWindowState.Maximized;
            //f.Show();
        }

		private void jobsAgentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FrmSqlJobs frmjobs = new FrmSqlJobs();
			frmjobs.Show();
		}

		

    
	

	
		
	}
}
