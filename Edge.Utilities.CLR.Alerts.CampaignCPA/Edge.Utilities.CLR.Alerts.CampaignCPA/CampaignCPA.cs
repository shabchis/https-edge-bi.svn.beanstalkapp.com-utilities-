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


public partial class StoredProcedures
{
	public class campaign
	{
		public int ID;
		public string Name;
		public double Cost;
		public double Acq;
		public double CPA;
		public bool zeroConv = false;
		public Dictionary<string, object> ExtraFields = new Dictionary<string, object>();

		public campaign(SqlDataReader mdxReader, string extraFields, string acqField, string cpaField)
		{
			Name = Convert.ToString(mdxReader["[Getways Dim].[Gateways].[Campaign].[MEMBER_CAPTION]"]);
			SqlContext.Pipe.Send(string.Format("Name = {0}", Name));

			Cost = mdxReader["[Measures].[Cost]"] == DBNull.Value ? 0 : Convert.ToDouble(mdxReader["[Measures].[Cost]"]);
			SqlContext.Pipe.Send(string.Format("Cost = {0}", Cost));

			CPA = mdxReader["[Measures].[" + cpaField + "]"] == DBNull.Value ? 0 : Convert.ToDouble(mdxReader["[Measures].[" + cpaField + "]"]);
			SqlContext.Pipe.Send(string.Format("CPA = {0}", CPA));

			Acq = mdxReader["[Measures].[" + acqField + "]"] == DBNull.Value ? 0 : Convert.ToDouble(mdxReader["[Measures].[" + acqField + "]"]);
			SqlContext.Pipe.Send(string.Format("Conv = {0}", Acq));

			if(!string.IsNullOrEmpty(extraFields))
			{
				foreach (string extraField in extraFields.Split(','))
				{
					ExtraFields.Add(extraField, mdxReader["[Measures].["+extraField+"]"]);
				}
			}

		}
		public double GetCalculatedCPA()
		{
			if (this.CPA == 0 && this.Cost != 0)
				return this.Cost;
			else
				return this.CPA;
		}


	}

	[Microsoft.SqlServer.Server.SqlProcedure]
	public static void CLR_Alerts_ConversionAnalysis(Int32 AccountID, Int32 Period, DateTime ToDay, string ChannelID, Int32 threshold, string excludeIds, string cubeName, string acqFieldName, string cpaFieldName, out SqlString returnMsg, string extraFields)
	{
		returnMsg = string.Empty;

		#region Exclude
		StringBuilder excludeBuilder = new StringBuilder();

		SqlContext.Pipe.Send(excludeIds);
		string excludeSyntax = "[Getways Dim].[Gateways].[Campaign].&[{0}]";
		if (!string.IsNullOrEmpty(excludeIds))
			foreach (string id in excludeIds.Split(','))
			{
				excludeBuilder.Append(string.Format(excludeSyntax, id));
				excludeBuilder.Append(",");
			}
		SqlContext.Pipe.Send(excludeBuilder.ToString());

		if (excludeBuilder.Length > 0)
			excludeBuilder.Remove(excludeBuilder.Length - 1, 1);
		#endregion
		SqlContext.Pipe.Send(excludeBuilder.ToString());

		string fromDate = ToDay.AddDays((Double)(-1 * (Period-1))).ToString("yyyyMMdd");
		string toDate = ToDay.ToString("yyyyMMdd");
		double totalCost = 0;
		double totalConv = 0;
		double avgCpa = 0;

		try
		{
			StringBuilder withMdxBuilder = new StringBuilder();
			withMdxBuilder.Append("With Set [Filtered Campaigns] As ");
			withMdxBuilder.Append("{" + excludeBuilder.ToString() + "}");

			StringBuilder measureBuilder = new StringBuilder();
			measureBuilder.Append("[Measures].[Cost], ");
			measureBuilder.Append(string.Format("[Measures].[{0}],", cpaFieldName)); // cost/reg
			measureBuilder.Append(string.Format("[Measures].[{0}] ", acqFieldName)); //ex. regs

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

			StringBuilder selectMdxBuilder = new StringBuilder();
			selectMdxBuilder.Append("SELECT [Selected Measures] On Columns ,");
			selectMdxBuilder.Append("{ NonEmpty ( { Except ( [Getways Dim].[Gateways].[Campaign].Allmembers, [Filtered Campaigns] )},[Selected Measures] )} On Rows ");

			StringBuilder fromMdxBuilder = new StringBuilder();
			fromMdxBuilder.Append(string.Format("From {0} ", cubeName));
			fromMdxBuilder.Append(string.Format("where ( [Accounts Dim].[Accounts].[Account].&[{0}] ", AccountID.ToString()));

			if (!string.IsNullOrEmpty(ChannelID))
				fromMdxBuilder.Append(string.Format(", [Channels Dim].[Channels].[Channel].&[{0}] ", ChannelID.ToString()));

			fromMdxBuilder.Append(string.Format(", [Time Dim].[Time Dim].[Day].&[{0}]:[Time Dim].[Time Dim].[Day].&[{1}])", fromDate, toDate));

			List<campaign> campaigns = new List<campaign>();

			SqlCommand command = new SqlCommand("dbo.SP_ExecuteMDX");
			command.CommandType = CommandType.StoredProcedure;
			SqlParameter withMDX = new SqlParameter("WithMDX", withMdxBuilder.ToString());
			command.Parameters.Add(withMDX);
			SqlContext.Pipe.Send(withMdxBuilder.ToString());

			SqlParameter selectMDX = new SqlParameter("SelectMDX", selectMdxBuilder.ToString());
			command.Parameters.Add(selectMDX);
			SqlContext.Pipe.Send(selectMdxBuilder.ToString());

			SqlParameter fromMDX = new SqlParameter("FromMDX", fromMdxBuilder.ToString());
			command.Parameters.Add(fromMDX);
			SqlContext.Pipe.Send(fromMdxBuilder.ToString());


			using (SqlConnection conn = new SqlConnection("context connection=true"))
			{
				conn.Open();
				command.Connection = conn;
				using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
				{
					while (reader.Read())
					{
						campaigns.Add(new campaign(reader,extraFields,acqFieldName,cpaFieldName));
					}
				}
			}



			if (campaigns.Count > 0)
			{
				foreach (campaign camp in campaigns)
				{
					totalCost += camp.Cost;
					totalConv += camp.Acq;
				}

				if (totalConv > 0)
					avgCpa = totalCost / totalConv;
				else
					avgCpa = 0;

				SqlContext.Pipe.Send(string.Format("The AVG CPA is {0}", avgCpa));
				SqlContext.Pipe.Send(string.Format("The Total Conv is {0}", totalConv));
				SqlContext.Pipe.Send(string.Format("The Total Cost is {0}", totalCost));

				var alertedCampaigns = from c in campaigns
									   where c.GetCalculatedCPA() > threshold * avgCpa
									   orderby c.CPA, c.Name
									   select c;

				StringBuilder commandBuilder = new StringBuilder();

				foreach (var unit in alertedCampaigns)
				{
					commandBuilder.Append(string.Format("select '{0}' as 'Campaign' , {1} as 'Cost', {2} as '{4}' ,{3} as 'CPA' "
						, unit.Name, Math.Round(unit.Cost, 2), unit.Acq, Math.Round(unit.CPA, 0), acqFieldName));

					//SqlContext.Pipe.Send(string.Format("select '{0}' as 'Campaign' , {1} as 'Cost', {2} as '{4}' ,{3} as 'CPA' "
					//    , unit.Name, Math.Round(unit.Cost, 2), unit.Acq, Math.Round(unit.CPA, 0), acqFieldName));



					//Adding ExtraFields
					if (unit.ExtraFields.Count > 0)
					{
						foreach (var extraField in unit.ExtraFields)
						{
							commandBuilder.Append(string.Format(" ,'{0}' as '{1}'", extraField.Value == DBNull.Value ? 0 : Math.Round(Convert.ToDouble(extraField.Value), 2), extraField.Key));
							//SqlContext.Pipe.Send(string.Format(" ,'{0}' as '{1}'", extraField.Value, extraField.Key));
						}
						
					}

					commandBuilder.Append(" Union ");
					SqlContext.Pipe.Send(" Union ");

				}

				

				if (commandBuilder.Length > 0)
				{
					commandBuilder.Remove(commandBuilder.Length - 6, 6);
					commandBuilder.Append(" Order by 4 desc"); // order by CPA
					SqlCommand reasultsCmd = new SqlCommand(commandBuilder.ToString());
					using (SqlConnection conn2 = new SqlConnection("context connection=true"))
					{
						conn2.Open();
						reasultsCmd.Connection = conn2;
						reasultsCmd.CommandTimeout = 9000;
						using (SqlDataReader reader = reasultsCmd.ExecuteReader())
						{
							SqlContext.Pipe.Send(reader);
						}
					}
				}

			}

		}
		catch (Exception e)
		{
			throw new Exception(".Net Exception : " + e.ToString(), e);
		}

		returnMsg = string.Format("<br><br>Execution Time: {5:dd/MM/yy H:mm} GMT <br><br>Time Period: {0} - {1} ({2} Days) <br><strong> AVG CPA: {3} </strong><br> Defined Threshold: {4}00% <br>",

			ToDay.AddDays(-1 * (Period-1)).ToString("dd/MM/yy"),
			ToDay.ToString("dd/MM/yy"),
			Period,
			Math.Round(avgCpa, 2),
			threshold,
			DateTime.Now
			);



	}
}
