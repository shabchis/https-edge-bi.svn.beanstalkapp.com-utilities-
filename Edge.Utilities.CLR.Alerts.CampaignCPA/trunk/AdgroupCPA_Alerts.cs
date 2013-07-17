//------------------------------------------------------------------------------
// <copyright file="CSSqlStoredProcedure.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Edge.Utilities.CLR.Alerts.CampaignCPA;
using Edge.Utilities.CLR.Alerts.CampaignCPA.AlertedObjects;
using System.Text.RegularExpressions;


public partial class StoredProcedures
{

	[Microsoft.SqlServer.Server.SqlProcedure]
	public static void CLR_Alerts_ConversionAnalysis_Adgroup(Int32 AccountID, Int32 Period, DateTime ToDay, string ChannelID, float CPR_threshold, float CPA_threshold, string excludeIds, string cubeName, string acq1FieldName, string acq2FieldName, string cpaFieldName, string cprFieldName, out SqlString returnMsg, string extraFields)
	{
		returnMsg = string.Empty;
		double totalCost = 0;
		double totalAcq1 = 0;
		double totalAcq2 = 0;
		double avgCPR = 0;
		double avgCPA = 0;

		#region Exclude
		StringBuilder excludeBuilder = new StringBuilder();

		//SqlContext.Pipe.Send(excludeIds);
		string excludeSyntax = "[Getways Dim].[Gateways].[Campaign].&[{0}].children";
		if (!string.IsNullOrEmpty(excludeIds))
			foreach (string id in excludeIds.Split(','))
			{
				excludeBuilder.Append(string.Format(excludeSyntax, id));
				excludeBuilder.Append(",");
			}

		if (excludeBuilder.Length > 0)
			excludeBuilder.Remove(excludeBuilder.Length - 1, 1);
		#endregion

		string fromDate = ToDay.AddDays((Double)(-1 * (Period - 1))).ToString("yyyyMMdd");
		string toDate = ToDay.ToString("yyyyMMdd");


		try
		{
			StringBuilder withMdxBuilder;
			StringBuilder selectMdxBuilder;
			StringBuilder fromMdxBuilder;
			GetAdgroupMDXQueryParams(AccountID, ChannelID, cubeName, acq1FieldName, acq2FieldName, cpaFieldName, cprFieldName, extraFields, excludeBuilder, fromDate, toDate, out withMdxBuilder, out selectMdxBuilder, out fromMdxBuilder);

            SqlContext.Pipe.Send(withMdxBuilder.ToString());
            SqlContext.Pipe.Send(selectMdxBuilder.ToString());
            SqlContext.Pipe.Send(fromMdxBuilder.ToString());

			#region Creating Command
			SqlCommand command = new SqlCommand("dbo.SP_ExecuteMDX");
			command.CommandType = CommandType.StoredProcedure;
			SqlParameter withMDX = new SqlParameter("WithMDX", withMdxBuilder.ToString());
			command.Parameters.Add(withMDX);

			SqlParameter selectMDX = new SqlParameter("SelectMDX", selectMdxBuilder.ToString());
			command.Parameters.Add(selectMDX);

			SqlParameter fromMDX = new SqlParameter("FromMDX", fromMdxBuilder.ToString());
			command.Parameters.Add(fromMDX);
			#endregion

			Dictionary<string, AlertedCampaignAdgroups> campaigns = new Dictionary<string, AlertedCampaignAdgroups>();
			using (SqlConnection conn = new SqlConnection("context connection=true"))
			{
				conn.Open();
				command.Connection = conn;
				using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
				{
					while (reader.Read())
					{
						string campName = Convert.ToString(reader["[Getways Dim].[Gateways].[Campaign].[MEMBER_CAPTION]"]);

						AlertedCampaignAdgroups camp = new AlertedCampaignAdgroups();
						if (campaigns.TryGetValue(campName, out camp))
						{// if campaign exists than add adgroup to this campaign 
							camp.AddAdgroup(reader, extraFields, acq1FieldName, acq2FieldName, cpaFieldName, cprFieldName);
						}
						else // if campaign doesnt exists than create campaign and add adgroup
							campaigns.Add(campName, new AlertedCampaignAdgroups(reader, extraFields, acq1FieldName, acq2FieldName, cpaFieldName, cprFieldName));
					}
				}



				List<AlertedAdgroup> alertedAdgroups = new List<AlertedAdgroup>();
				StringBuilder commandBuilder = new StringBuilder();



				if (campaigns.Count > 0)
					CalcTotalsAndAvg(campaigns, out totalCost, out totalAcq1, out totalAcq2, out avgCPA, out avgCPR);

				SetAdgroupValuePriority(campaigns,avgCPA,avgCPR);

                foreach (var camp in campaigns)
				{

					var alertedAdgroupsPerCampaign = (from ag in camp.Value.AdGroups
													  where ((ag.GetCalculatedCPA() >= CPA_threshold * avgCPA) || (ag.GetCalculatedCPR() >= CPR_threshold * avgCPR)&& ag.Priority >= 0)
													  select ag).OrderBy(val => val.Priority);

					alertedAdgroups.AddRange(alertedAdgroupsPerCampaign);

				}

				alertedAdgroups = alertedAdgroups.OrderByDescending(val => val.Priority).ToList();

				//commandBuilder.Append(string.Format("select [Campaign],[Ad Group],[Cost],[{0}],[CPA({1})],[{2}],[CPR({3})] from (", acq2FieldName, cpaFieldName, acq1FieldName, cprFieldName));

				SqlMetaData[] cols = new SqlMetaData[]
				{
					new SqlMetaData("Campaign", SqlDbType.NVarChar, 1024),
					new SqlMetaData("AdGroup", SqlDbType.NVarChar, 1024),
					new SqlMetaData("Cost", SqlDbType.NVarChar, 1024),
					new SqlMetaData(acq2FieldName, SqlDbType.NVarChar, 1024),
					new SqlMetaData(string.Format("CPA({0})",cpaFieldName), SqlDbType.NVarChar, 1024),
					new SqlMetaData(acq1FieldName, SqlDbType.NVarChar, 1024),
					new SqlMetaData(string.Format("CPR({0})",cprFieldName), SqlDbType.NVarChar, 1024),
					//new SqlMetaData("P", SqlDbType.NVarChar, 1024)
				};
				SqlDataRecord rec = new SqlDataRecord(cols);
                			
				if (alertedAdgroups.Count == 0)
					SqlContext.Pipe.Send("Error");

				SqlContext.Pipe.SendResultsStart(rec);
				foreach (AlertedAdgroup adgroup in alertedAdgroups)
				{
					//if (adgroup.Priority > 2)
					//	continue;

					rec.SetSqlString(0, adgroup.CampaignName);
					rec.SetSqlString(1, adgroup.Name);
					//cost
					rec.SetSqlString(2,string.IsNullOrEmpty((Math.Round(adgroup.Cost, 0)).ToString("#,#", CultureInfo.InvariantCulture)) == true ? "0" : '$'+((Math.Round(adgroup.Cost, 0)).ToString("#,#", CultureInfo.InvariantCulture)));
					//actives
					rec.SetSqlString(3, Math.Round(adgroup.Acq2, 0).ToString());
					//CPA
					rec.SetSqlString(4, string.IsNullOrEmpty((Math.Round(adgroup.CPA, 0)).ToString("#,#", CultureInfo.InvariantCulture)) == true ? "0" : '$' + ((Math.Round(adgroup.CPA, 0)).ToString("#,#", CultureInfo.InvariantCulture)));
					//Regs
					rec.SetSqlString(5, Math.Round(adgroup.Acq1, 0).ToString());
					//CPR
					rec.SetSqlString(6, string.IsNullOrEmpty((Math.Round(adgroup.CPR, 0)).ToString("#,#", CultureInfo.InvariantCulture)) == true ? "0" : '$' + ((Math.Round(adgroup.CPR, 0)).ToString("#,#", CultureInfo.InvariantCulture)));

					//Priority
					//rec.SetSqlString(7, adgroup.Priority.ToString());
					SqlContext.Pipe.SendResultsRow(rec);
				}
				SqlContext.Pipe.SendResultsEnd();
			}
		}
		catch (Exception e)
		{
			throw new Exception(".Net Exception : " + e.ToString(), e);
		}
	
		returnMsg = string.Format("<br><br>Execution Time: {0:dd/MM/yy H:mm} GMT <br><br>Time Period:"
		+ "{1} - {2} ({3} Days) <br><strong> AVG CPA: ${4} </strong><br><strong> AVG CPR: ${5} </strong><br>"
		+ " Defined CPA Threshold: {6}% <br> Defined CPR Threshold: {7}% <br>",

		DateTime.Now,
		ToDay.AddDays(-1 * (Period - 1)).ToString("dd/MM/yy"),
		ToDay.ToString("dd/MM/yy"),
		Period,
		Math.Round(avgCPA, 0),
		Math.Round(avgCPR, 0),
		CPA_threshold * 100,
		CPR_threshold * 100

			);



	}

	private static void SetAdgroupValuePriority(Dictionary<string, AlertedCampaignAdgroups> campaigns,double totalAvgCPA, double totalAvgCPR)
	{
		foreach (var campaign in campaigns)
		{
			foreach (AlertedAdgroup adgroup in campaign.Value.AdGroups)
			{
				adgroup.setValuePriority(totalAvgCPA, totalAvgCPR);
			}
		}
	}

	private static void CalcTotalsAndAvg(Dictionary<string, AlertedCampaignAdgroups> campaigns, out double totalCost, out double totalAcq1, out double totalAcq2, out double avgCPA, out double avgCPR)
	{

		totalCost = 0;
		totalAcq1 = 0;
		totalAcq2 = 0;

		foreach (var camp in campaigns)
		{
			totalCost += camp.Value.Cost;
			totalAcq1 += camp.Value.Acq1;
			totalAcq2 += camp.Value.Acq2;
		}

		// calc Avg CPR
		if (totalAcq1 > 0)
			avgCPR = totalCost / totalAcq1;
		else
			avgCPR = 0;

		//calc Avg CPA
		if (totalAcq2 > 0)
			avgCPA = totalCost / totalAcq2;
		else
			avgCPA = 0;
	}

	private static void GetAdgroupMDXQueryParams(Int32 AccountID, string ChannelID, string cubeName, string acq1FieldName, string acq2FieldName, string cpaFieldName, string cprFieldName, string extraFields, StringBuilder excludeBuilder, string fromDate, string toDate, out StringBuilder withMdxBuilder, out StringBuilder selectMdxBuilder, out StringBuilder fromMdxBuilder)
	{
		withMdxBuilder = new StringBuilder();
		withMdxBuilder.Append("With Set [Filtered Campaigns] As ");
		withMdxBuilder.Append("{" + excludeBuilder.ToString() + "}");

		StringBuilder measureBuilder = new StringBuilder();
		measureBuilder.Append("[Measures].[Cost], ");
		measureBuilder.Append(string.Format("[Measures].[{0}],", cpaFieldName)); // cost/active
		measureBuilder.Append(string.Format("[Measures].[{0}], ", acq1FieldName)); //ex. actives
		measureBuilder.Append(string.Format("[Measures].[{0}],", cprFieldName)); // cost/active
		measureBuilder.Append(string.Format("[Measures].[{0}] ", acq2FieldName)); //ex. actives

		//Adding ExtraFields
		if (!String.IsNullOrEmpty(extraFields))
		{
			foreach (string extraField in extraFields.Split(','))
			{
				measureBuilder.Append(string.Format(",[Measures].[{0}] ", extraField));
			}
		}

		withMdxBuilder.Append(" Set [Selected Measures] AS ");
		withMdxBuilder.Append("{" + measureBuilder.ToString() + "}");

		selectMdxBuilder = new StringBuilder();
		selectMdxBuilder.Append("SELECT [Selected Measures] On Columns ,");
		selectMdxBuilder.Append("Filter (Exists ({  Except ( [Getways Dim].[Gateways].[Ad Group].Members, [Filtered Campaigns] )} ) ");
		selectMdxBuilder.Append(string.Format(",[Measures].[Cost] >0  OR [Measures].[{0}] > 0 OR [Measures].[{1}] > 0) On Rows ", acq1FieldName, acq2FieldName));

		fromMdxBuilder = new StringBuilder();
		fromMdxBuilder.Append(string.Format("From {0} ", cubeName));
		fromMdxBuilder.Append(string.Format("where ( [Accounts Dim].[Accounts].[Account].&[{0}] ", AccountID.ToString()));

		if (!string.IsNullOrEmpty(ChannelID))
			fromMdxBuilder.Append(string.Format(", [Channels Dim].[Channels].[Channel].&[{0}] ", ChannelID.ToString()));

		fromMdxBuilder.Append(string.Format(", [Time Dim].[Time Dim].[Day].&[{0}]:[Time Dim].[Time Dim].[Day].&[{1}])", fromDate, toDate));
	}


}
