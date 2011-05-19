using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Google.Api.Ads.AdWords.v201101;
using Google.Api.Ads.AdWords.Lib;

namespace APITester
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		//	List<string> reportDefinitionReportType =(List<string>) Enum.GetValues(typeof(ReportDefinitionReportType)).Cast<ReportDefinitionReportType>();
			var reportDefinitionReportType = (Array)Enum.GetValues(typeof(ReportDefinitionReportType)).Cast<ReportDefinitionReportType>();
			foreach (ReportDefinitionReportType item in reportDefinitionReportType)
			{
			    this.comboBox1.Items.Add(item.ToString());
			}
			this.comboBox1.SelectedIndex = 1;
			
		}

		private void GetFieldsList(ReportDefinitionReportType ReportType)
		{

			AdWordsAppConfig config = new AdWordsAppConfig()
			{
				AuthToken = "DQAAAJUAAAC7ij1fVVAQmRw4uuNEoldvGJSL-ndgOvfFgUwzMyACF4-JYOvqW75DQw_qoZmwX3FcPNkmp5slK-YVtmvb_7oLNYtImccG0yFp0E3TOcczVt0_7bKM82myTrN0kOyZQ6sVeMNjd2tuQQQUNij09yoVgn8qaRJvN_ieefjGJzItwZlK9O__AjdPXejbfuycNRJLcN-oG4NA3vdeUFq940hR",
				DeveloperToken = "5eCsvAOU06Fs4j5qHWKTCA",
				ApplicationToken = "5eCsvAOU06Fs4j5qHWKTCA",

				ClientEmail = "bezeqaccess@gmail.com",
				UserAgent = "Edge.BI",
				EnableGzipCompression = true
			}; AdWordsUser user = new AdWordsUser(new AdWordsServiceFactory().ReadHeadersFromConfig(config));

			var reportService = (ReportDefinitionService)user.GetService(AdWordsService.v201101.ReportDefinitionService);

			ReportDefinitionField[] reportFields = reportService.getReportFields(ReportType);
			foreach (ReportDefinitionField field in reportFields)
			{
				this.dataGridView1.Rows.Add(field.fieldName, field.fieldType,field.canSelect,field.canFilter,field.displayFieldName);
			}
			

		}

		private void GetFields_btn_Click(object sender, EventArgs e)
		{
			ReportDefinitionReportType type = (ReportDefinitionReportType)Enum.Parse(typeof(ReportDefinitionReportType), this.comboBox1.Text, true);
			GetFieldsList(type);
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.dataGridView1.Rows.Clear();
		}
	}
}
