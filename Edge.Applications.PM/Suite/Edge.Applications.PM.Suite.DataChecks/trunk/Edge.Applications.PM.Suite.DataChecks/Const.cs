using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edge.Applications.PM.Suite.DataChecks
{
	public static class Const
	{
		public static class AdMetricsConst
		{
			//TO DO : GET PARAMS FROM CONFIGURATION AND USE STATIC 
			//TO DO : SET THE FOLLOWING PARAMS WHILE LODING DATACHECK WINDOW.

			// Tabels
			public static string OltpTable = "dbo.Paid_API_AllColumns_v29";
			public static string DwhTable = "Dwh_Fact_PPC_Campaigns";

			//Services
			public const string DeliveryOltpService = "DataChecks.OltpDelivery";
			public const string OltpDwhService = "DataChecks.DwhOltp";
			public const string MdxDwhService = "DataChecks.MdxDwh";
			public const string MdxOltpService = "DataChecks.MdxOltp";

			public const string AdwordsServiceName = "Google.AdWords";
			public const string FacebookServiceName = "Facebook.GraphApi";
			public const string BingServiceName = "Bing.Api";

			//WorkflowServices
			public static class WorkflowServices
			{
				public const string CommitServiceName = "AdMetricsCommit";
				public const string OltpDeliveryCheckServiceName = "DataChecks.OltpDelivery";
				public const string ResultsHandlerServiceName = "DataChecks.ResultsHandler";
			}

			//ProductionKeys
			public const string SeperiaProductionPathKey = "SeperiaProductionConfigurationPath";
			public const string EdgeProductionPathKey = "EdgeProductionConfigurationPath";

			//ApplicationTypes
			public const string SeperiaApp = "Seperia";
			public const string EdgeApp = "Edge.BI";

		}
	}
}
