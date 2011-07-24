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
using Edge.Services.Google.Adwords;
using Google.Api.Ads.AdWords.Util;

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
				Email = this.MccEmail.Text,
				Password = this.MccPassword.Text,
				DeveloperToken = "5eCsvAOU06Fs4j5qHWKTCA",
				ApplicationToken = "5eCsvAOU06Fs4j5qHWKTCA",
				//ClientEmail = "lotan3@gmail.com",
				UserAgent = "Edge.BI",
				EnableGzipCompression = true
			};

			GoogleUserEntity userEntity = new GoogleUserEntity(this.MccEmail.Text, this.validEmail.Text, true , this.connectionString.Text);
			//AdWordsUser user = new AdWordsUser(new AdWordsServiceFactory().ReadHeadersFromConfig(config));
			var reportService = (ReportDefinitionService)userEntity.adwordsUser.GetService(AdWordsService.v201101.ReportDefinitionService);
			//AdwordsReport _googleReport = new AdwordsReport(95, txt_mcc.Text, validEmail.Text, "", "");
			ReportDefinitionField[] reportFields = reportService.getReportFields(ReportType);
			foreach (ReportDefinitionField field in reportFields)
			{
				this.dataGridView1.Rows.Add(field.fieldName, field.fieldType, field.canSelect, field.canFilter, field.displayFieldName);
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

		private void button1_Click(object sender, EventArgs e)
		{
			AdWordsAppConfig config = new AdWordsAppConfig()
			{
				Email = this.MccEmail.Text,
				Password = this.MccPassword.Text,
				DeveloperToken = "5eCsvAOU06Fs4j5qHWKTCA",
				ApplicationToken = "5eCsvAOU06Fs4j5qHWKTCA",
				//ClientEmail = "lotan3@gmail.com",
				UserAgent = "Edge.BI",
				EnableGzipCompression = true
			};
			AdWordsUser user = new AdWordsUser(new AdWordsServiceFactory().ReadHeadersFromConfig(config));
			var reportService = (ReportDefinitionService)user.GetService(AdWordsService.v201101.ReportDefinitionService);
			this.AuthToken.Text = reportService.RequestHeader.authToken;
			this.DeveloperToken.Text = reportService.RequestHeader.developerToken;
			this.ApplicationToken.Text = reportService.RequestHeader.applicationToken;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			try
			{
				log.AppendText("creating AdwordsReport ....\n ");
				//AdwordsReport _googleReport = new AdwordsReport();
				GoogleUserEntity user = new GoogleUserEntity(MccEmail.Text, email.Text);
				//_googleReport.user = new GoogleUserEntity(email.Text, email.Text);
				log.AppendText("creating reportService ....\n ");
				ReportDefinitionService reportService = (ReportDefinitionService)user.adwordsUser.GetService(AdWordsService.v201101.ReportDefinitionService);
				long report_id;
				long.TryParse(reportId.Text, out report_id);

				log.AppendText("downloading report ....\n ");
				DownloadReport(user,report_id, path.Text);
			}
			catch (Exception ex)
			{
				log.AppendText(ex.Message.ToString());
			}

			log.AppendText("Done ! \n");
		}
		
		public void DownloadReport(GoogleUserEntity user,long reportId, string Path = @"c:\testingAdwords.zip")
		{

			//========================== Retriever =======================================================
			try
			{
				// Download report.
				new ReportUtilities(user.adwordsUser).DownloadReportDefinition(reportId, Path);
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to download report. Exception says" + ex.Message);
			}
			//======================== End of Retriever =================================================
		}
		private void srch_Click(object sender, EventArgs e)
		{

			AdwordsReport _googleReport = new AdwordsReport(-1, this.KwdEmail.Text, this.KwdEmail.Text, "", "", true, ReportDefinitionDateRangeType.ALL_TIME, ReportDefinitionReportType.KEYWORDS_PERFORMANCE_REPORT);

			string[] kwd = new string[]{
				this.KwdID.Text
			};
			_googleReport.AddFilter("Id", PredicateOperator.EQUALS, kwd);
			_googleReport.AddFilter("Id", PredicateOperator.EQUALS, kwd);
			_googleReport.intializingGoogleReport();
			DownloadReport(_googleReport.user,_googleReport.Id, @"D:\KeywordSearchReport");
			
		}

		private void button3_Click(object sender, EventArgs e)
		{
			List<string> fields = new List<string>();

			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				if ((Boolean)row.Cells["select"].Value == true)
					fields.Add(row.Cells["Name"].ToString());

			}
		}


	}
}
