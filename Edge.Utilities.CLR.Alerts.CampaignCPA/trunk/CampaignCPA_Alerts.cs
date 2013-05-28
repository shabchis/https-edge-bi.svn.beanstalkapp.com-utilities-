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


public partial class StoredProcedures
{


    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CLR_Alerts_ConversionAnalysis(Int32 AccountID, Int32 Period, DateTime ToDay, string ChannelID, Int32 threshold, string excludeIds, string cubeName, string acqFieldName, string cpaFieldName, out SqlString returnMsg, string acq2FieldName, string cprFieldName)
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

        string fromDate = ToDay.AddDays((Double)(-1 * (Period - 1))).ToString("yyyyMMdd");
        string toDate = ToDay.ToString("yyyyMMdd");
        double totalCost = 0;
        double totalConv = 0;
        double avgCpa = 0;

        try
        {
            StringBuilder withMdxBuilder = new StringBuilder();
            StringBuilder measureBuilder = new StringBuilder();
            StringBuilder selectMdxBuilder = new StringBuilder();
            StringBuilder fromMdxBuilder = new StringBuilder();

            withMdxBuilder.Append("With Set [Filtered Campaigns] As ");
            withMdxBuilder.Append("{" + excludeBuilder.ToString() + "}");

            measureBuilder.Append("[Measures].[Cost], ");
            measureBuilder.Append(string.Format("[Measures].[{0}],", cpaFieldName)); // cost/reg
            measureBuilder.Append(string.Format("[Measures].[{0}], ", acqFieldName)); //ex. regs
            measureBuilder.Append(string.Format("[Measures].[{0}], ", acq2FieldName));
            measureBuilder.Append(string.Format("[Measures].[{0}] ", cprFieldName));



            withMdxBuilder.Append(" Set [Selected Measures] AS ");
            withMdxBuilder.Append("{" + measureBuilder.ToString() + "}");

            selectMdxBuilder.Append("SELECT [Selected Measures] On Columns ,");
            selectMdxBuilder.Append("{ NonEmpty ( { Except ( [Getways Dim].[Gateways].[Campaign].Allmembers, [Filtered Campaigns] )},[Selected Measures] )} On Rows ");


            fromMdxBuilder.Append(string.Format("From {0} ", cubeName));
            fromMdxBuilder.Append(string.Format("where ( [Accounts Dim].[Accounts].[Account].&[{0}] ", AccountID.ToString()));

            if (!string.IsNullOrEmpty(ChannelID))
                fromMdxBuilder.Append(string.Format(", [Channels Dim].[Channels].[Channel].&[{0}] ", ChannelID.ToString()));

            fromMdxBuilder.Append(string.Format(", [Time Dim].[Time Dim].[Day].&[{0}]:[Time Dim].[Time Dim].[Day].&[{1}])", fromDate, toDate));

            List<AlertedCampaign> campaigns = new List<AlertedCampaign>();

            #region Creating Command
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
            #endregion

            using (SqlConnection conn = new SqlConnection("context connection=true"))
            {
                conn.Open();
                command.Connection = conn;
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        campaigns.Add(new AlertedCampaign(reader, acqFieldName, cpaFieldName, acq2FieldName, cprFieldName));
                    }
                }
            }



            if (campaigns.Count > 0)
            {
                foreach (AlertedCampaign camp in campaigns)
                {
                    totalCost += camp.Cost;
                    totalConv += camp.Acq1;
                }

                if (totalConv > 0)
                    avgCpa = totalCost / totalConv;
                else
                    avgCpa = 0;

                //SqlContext.Pipe.Send(string.Format("The AVG CPA is {0}", avgCpa));
                //SqlContext.Pipe.Send(string.Format("The Total Conv is {0}", totalConv));
                //SqlContext.Pipe.Send(string.Format("The Total Cost is {0}", totalCost));

                var alertedCampaigns = (from c in campaigns
                                        where c.GetCalculatedCPA() > threshold * avgCpa
                                        select c).OrderByDescending(val => val.Priority);

               SqlContext.Pipe.Send("Count:"+alertedCampaigns.Count<AlertedCampaign>().ToString());
                
                #region Creating CLR Table Cols
                SqlMetaData[] cols_list = new SqlMetaData[]
                {
                    new SqlMetaData("Campaign", SqlDbType.NVarChar, 1024),
					new SqlMetaData("Cost", SqlDbType.NVarChar, 1024),
					new SqlMetaData(acqFieldName, SqlDbType.NVarChar, 1024),
					new SqlMetaData(string.Format("CPA({0})",cpaFieldName), SqlDbType.NVarChar, 1024),
                    new SqlMetaData(acq2FieldName, SqlDbType.NVarChar, 1024),
					new SqlMetaData(string.Format("CPR({0})",cprFieldName), SqlDbType.NVarChar, 1024),
					//new SqlMetaData(string.Format("Priority"), SqlDbType.NVarChar, 1024),
                };
                SqlDataRecord rec = new SqlDataRecord(cols_list);
                
                #endregion

                StringBuilder commandBuilder = new StringBuilder();
                //SqlContext.Pipe.Send("foreach (AlertedCampaign alertedCamp in alertedCampaigns)");
                
                SqlContext.Pipe.SendResultsStart(rec);
                #region Send Rows
                foreach (AlertedCampaign alertedCamp in alertedCampaigns)
                {
                    //Campaign Name
                    rec.SetSqlString(0, alertedCamp.Name);
                    //cost
                    rec.SetSqlString(1, string.IsNullOrEmpty((Math.Round(alertedCamp.Cost, 0)).ToString("#,#", CultureInfo.InvariantCulture)) == true ? "0" : '$' + ((Math.Round(alertedCamp.Cost, 0)).ToString("#,#", CultureInfo.InvariantCulture)));
                    //actives
                    rec.SetSqlString(2, Math.Round(alertedCamp.Acq1, 0).ToString());
                    //CPA
                    rec.SetSqlString(3, string.IsNullOrEmpty((Math.Round(alertedCamp.CPA, 0)).ToString("#,#", CultureInfo.InvariantCulture)) == true ? "0" : '$' + ((Math.Round(alertedCamp.CPA, 0)).ToString("#,#", CultureInfo.InvariantCulture)));
                    //Regs
                    rec.SetSqlString(4, Math.Round(alertedCamp.Acq2, 0).ToString());
                    //CPR
                    rec.SetSqlString(5, string.IsNullOrEmpty((Math.Round(alertedCamp.CPR, 0)).ToString("#,#", CultureInfo.InvariantCulture)) == true ? "0" : '$' + ((Math.Round(alertedCamp.CPR, 0)).ToString("#,#", CultureInfo.InvariantCulture)));

                    //Priority
                   // rec.SetSqlString(6, alertedCamp.Priority.ToString());

                    SqlContext.Pipe.SendResultsRow(rec);
                }
                #endregion
                SqlContext.Pipe.SendResultsEnd();
              
            }
        }

        catch (Exception e)
        {
            throw new Exception(".Net Exception : " + e.ToString(), e);
        }

        returnMsg = string.Format("<br><br>Execution Time: {5:dd/MM/yy H:mm} GMT <br><br>Time Period: {0} - {1} ({2} Days) <br><strong> AVG CPA: ${3} </strong><br> Defined Threshold: {4}00% <br>",

            ToDay.AddDays(-1 * (Period - 1)).ToString("dd/MM/yy"),
            ToDay.ToString("dd/MM/yy"),
            Period,
            Math.Round(avgCpa, 0),
            threshold,
            DateTime.Now
            );



    }
}
