using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Api.Ads.AdWords.v201101;
using System.Net;
using Google.Api.Ads.Common.Lib;
using Google.Api.Ads.AdWords.Lib;
using Google.Api.Ads.AdWords.Util;
using System.Windows.Forms;

namespace APITester
{
    class Program
    {

		static string[] AD_PERFORMANCE_REPORT_FIELDS = { "Id", "AdGroupId", "AdGroupName", "AdGroupStatus", "CampaignId", "CampaignName", "Impressions", "Clicks", "Cost", "CreativeDestinationUrl", "KeywordId", "Url" };
		static string[] KEYWORDS_PERFORMANCE_REPORT_FIELDS = { "Id", "AdGroupId", "KeywordText", "KeywordMatchType", "Impressions", "Clicks", "Cost" };


		public ReportDefinition CreateReportDefinition(ReportDefinition reportDefinition, string reportName, ReportDefinitionDateRangeType dateRageType,
			Selector selector, ClientSelector[] clients, DownloadFormat downloadFormat = DownloadFormat.GZIPPED_CSV)
		{
			reportDefinition.reportName = reportName;
			reportDefinition.dateRangeType = dateRageType;
			reportDefinition.reportType = ReportDefinitionReportType.AD_PERFORMANCE_REPORT;
			reportDefinition.selector = selector;
			reportDefinition.downloadFormat = downloadFormat;
			reportDefinition.downloadFormatSpecified = true;
			reportDefinition.clientSelectors = clients.ToArray();
			return reportDefinition;
		}
		
		
		static void Main(string[] args)
        {

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
			
			/*
		   AdWordsAppConfig config = new AdWordsAppConfig()
		   {
			   Email =  "edge.bi.mcc@gmail.com",
			   Password = "edgebinewfish",
			   DeveloperToken = "5eCsvAOU06Fs4j5qHWKTCA",
			   ApplicationToken = "5eCsvAOU06Fs4j5qHWKTCA",

			   ClientEmail ="lotan3@gmail.com",
			   UserAgent = "Edge.BI",
			   EnableGzipCompression = true
		   };
		   */
			//AdWordsAppConfig config = new AdWordsAppConfig()
			//{
			//    AuthToken = "DQAAAJUAAAC7ij1fVVAQmRw4uuNEoldvGJSL-ndgOvfFgUwzMyACF4-JYOvqW75DQw_qoZmwX3FcPNkmp5slK-YVtmvb_7oLNYtImccG0yFp0E3TOcczVt0_7bKM82myTrN0kOyZQ6sVeMNjd2tuQQQUNij09yoVgn8qaRJvN_ieefjGJzItwZlK9O__AjdPXejbfuycNRJLcN-oG4NA3vdeUFq940hR",
			//    DeveloperToken = "5eCsvAOU06Fs4j5qHWKTCA",
			//    ApplicationToken = "5eCsvAOU06Fs4j5qHWKTCA",

			//    ClientEmail = "bezeqaccess@gmail.com",
			//    UserAgent = "Edge.BI",
			//    EnableGzipCompression = true
			//}; AdWordsUser user = new AdWordsUser(new AdWordsServiceFactory().ReadHeadersFromConfig(config));
			
			//var reportService = (ReportDefinitionService)user.GetService(AdWordsService.v201101.ReportDefinitionService);

			////Getting Report fields
			//ReportDefinitionField[] reportFields = reportService.getReportFields(ReportDefinitionReportType.KEYWORDS_PERFORMANCE_REPORT);
			//List<string> fields = new List<string>();
			//foreach (ReportDefinitionField field in reportFields)
			//{
			//    fields.Add(field.fieldName);
			//}
			//fields.ToString();


			//// Create selector.
			//Selector selector = new Selector();
			//selector.fields = AD_PERFORMANCE_REPORT_FIELDS;
			//// selector.setPredicates(new Predicate[] { adGroupPredicate });

			//// Create ClientSelector.
			//List<ClientSelector> clients = new List<ClientSelector>();
			//ClientSelector client = new ClientSelector();
			//client.login = "client@gmail.com";
			//clients.Add(client);

			//// Create report definition.
			//ReportDefinition reportDefinition = new ReportDefinition();
			//reportDefinition.reportName = "Adword AD Report - TestDaily";
			//reportDefinition.dateRangeType = ReportDefinitionDateRangeType.YESTERDAY;
			//reportDefinition.reportType = ReportDefinitionReportType.AD_PERFORMANCE_REPORT;
			//reportDefinition.selector = selector;
			//reportDefinition.downloadFormat = DownloadFormat.GZIPPED_CSV;
			//reportDefinition.downloadFormatSpecified = true;
			//reportDefinition.clientSelectors = clients.ToArray();

			//// Create operations.
			//ReportDefinitionOperation operation = new ReportDefinitionOperation();
			//operation.operand = reportDefinition;
			//operation.@operator = Operator.ADD;
			//ReportDefinitionOperation[] operations = new ReportDefinitionOperation[] { operation };
			
			//ReportDefinition[] reportDefintions = reportService.mutate(operations);
			
			//try
			//{
			//    // Download report.
			//    new ReportUtilities(user).DownloadReportDefinition(reportDefintions[0].id, "c:\\testingAdwords.zip");

			//    Console.WriteLine("Report with definition id '{0}' was downloaded to '{1}'.",
			//        reportDefintions[0].id, "c:\\testingAdwords2.zip");
			//}
			//catch (Exception ex)
			//{
			//    Console.WriteLine("Failed to download report. Exception says \"{0}\"", ex.Message);
			//}
            
        }

    }
}
