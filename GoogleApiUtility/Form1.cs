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
				AuthToken = "DQAAAKYAAAC4aDhadB2PnMH7FeJ-a6ro8tCZzkDvDRfx-FcHrbl-OK5IFUOD0sSwX6eBSTDUOBc0CRmfp7JEbYwedSQVT4PVVhMoXGW1znOeUSdheSKc8cX2wMRhS9Ev-CTG_i3EnlJ_UFZfw3a_7QsrA0-XeUdVYbRRUeoXqK4HPVS_vClEJM0XhpUXTtGRzYss3O-MUvSBb672pFc6cO84pKx39Md3wdYiXcqjPa3k0sdKPrfXyQ",
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
				AdwordsReport _googleReport = new AdwordsReport();
				_googleReport.User = new GoogleUserEntity(email.Text);
				log.AppendText("creating reportService ....\n ");
				_googleReport.reportService = (ReportDefinitionService)_googleReport.User.adwordsUser.GetService(AdWordsService.v201101.ReportDefinitionService);
				long report_id;
				long.TryParse(reportId.Text, out report_id);

				log.AppendText("downloadin report ....\n ");
				_googleReport.DownloadReport(report_id, path.Text);
			}
			catch (Exception ex)
			{
				log.AppendText(ex.Message.ToString());
			}

			log.AppendText("Done ! \n");
		}

		private void srch_Click(object sender, EventArgs e)
		{

			AdwordsReport _googleReport = new AdwordsReport(-1, this.KwdEmail.Text, "", "", true, ReportDefinitionDateRangeType.ALL_TIME, ReportDefinitionReportType.KEYWORDS_PERFORMANCE_REPORT);

			string[] kwd = new string[]{
				this.KwdID.Text
			};

			_googleReport.AddFilter("Id", PredicateOperator.EQUALS, kwd);
			_googleReport.intializingGoogleReport();
			_googleReport.DownloadReport(_googleReport.Id, @"D:\KeywordSearchReport");
			
		}


	}
}
