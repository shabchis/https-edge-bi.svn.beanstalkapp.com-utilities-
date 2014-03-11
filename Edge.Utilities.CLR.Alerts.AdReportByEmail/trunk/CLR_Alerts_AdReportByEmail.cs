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
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;


public partial class StoredProcedures
{

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CLR_Alerts_AdReportByEmail(int Account_ID, string DaysNum, string CPAThreshold, string CPRThreshold, string AdgroupSpent, out SqlString returnMsg)
    {

        try
        {
           
           
            #region Creating SP_GetAcqAndCpaFieldsNamesByAccountID Command
            SqlCommand fields_command = new SqlCommand("dbo.SP_GetAcqAndCpaFieldsNamesByAccountID");
            fields_command.CommandType = CommandType.StoredProcedure;
            SqlParameter account_ID_SQL = new SqlParameter("Account_ID", Account_ID);
            fields_command.Parameters.Add(account_ID_SQL);
            #endregion

            #region Creating AD_Alert_Report Command
            SqlCommand command = new SqlCommand("dbo.AD_Alert_Report");
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter accid = new SqlParameter("Account_ID", Account_ID);
            command.Parameters.Add(accid);

            SqlParameter daysNum_SQL = new SqlParameter("DaysNum", DaysNum);
            command.Parameters.Add(daysNum_SQL);

            SqlParameter CPAThreshold_SQL = new SqlParameter("CPAThreshold", CPAThreshold);
            command.Parameters.Add(CPAThreshold_SQL);

            SqlParameter CPRThreshold_SQL = new SqlParameter("CPRThreshold", CPRThreshold);
            command.Parameters.Add(CPRThreshold_SQL);

            SqlParameter adgroupSpent_SQL = new SqlParameter("AdgroupSpent", AdgroupSpent);
            command.Parameters.Add(adgroupSpent_SQL);
            #endregion

            string acq1Name = "Regs";
            string acq2Name = "Actives";

           

            using (SqlConnection conn = new SqlConnection("context connection=true"))
            {
                conn.Open();
                command.Connection = conn;
                fields_command.Connection = conn;
               
                using (SqlDataReader reader = fields_command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        acq1Name = reader[0].ToString();
                        acq2Name = reader[1].ToString();
                    }
                }

                SqlMetaData[] cols = new SqlMetaData[]
				{
					new SqlMetaData("Campaign", SqlDbType.NVarChar, 1024),
					new SqlMetaData("AdGroup", SqlDbType.NVarChar, 1024),
					new SqlMetaData("Ad Creative", SqlDbType.NVarChar, 1024),
					new SqlMetaData("Cost", SqlDbType.NVarChar, 1024),
					new SqlMetaData(acq1Name, SqlDbType.NVarChar, 1024),
					new SqlMetaData("CPR", SqlDbType.NVarChar, 1024),
					new SqlMetaData(acq2Name, SqlDbType.NVarChar, 1024),
					new SqlMetaData("CPA", SqlDbType.NVarChar, 1024),
					//new SqlMetaData("P", SqlDbType.NVarChar, 1024)
				};
                SqlDataRecord rec = new SqlDataRecord(cols);
                SqlContext.Pipe.SendResultsStart(rec);
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    string value;
                    while (reader.Read())
                    {
                        rec.SetSqlString(0, reader["[Paid Creatives Dim].[Creatives].[Campaign].[MEMBER_CAPTION]"].ToString());
                        rec.SetSqlString(1, reader["[Paid Creatives Dim].[Creatives].[Ad Group].[MEMBER_CAPTION]"].ToString());
                        rec.SetSqlString(2, reader["[Paid Creatives Dim].[Creatives].[Creative].[MEMBER_CAPTION]"].ToString());

                        try
                        {
                            value = reader["[Measures].[Cost]"] == DBNull.Value ? "0" : (Math.Round(Convert.ToDouble(reader["[Measures].[Cost]"]), 1).ToString("#,#", CultureInfo.InvariantCulture));
                            rec.SetSqlString(3, string.Format("{0}{1}", value.Equals("0") ? string.Empty : "$", value));
                        }
                        catch(Exception e)
                        {
                            object[] values = new object[] { };
                            reader.GetValues(values);
                            StringBuilder sb = new StringBuilder();
                            foreach (var ob in values)
                            {
                                sb.Append(ob.ToString());
                            }
                            throw new Exception("[Measures].[Cost] Error: " + reader["[Measures].[Cost]"].ToString()+"Check Row Values: "+sb.ToString(), e);
                        }

                        try
                        {
                            if (Convert.ToString(reader["[Measures].[Regs_Calc]"]).Contains("E-"))
                                rec.SetSqlString(4, Convert.ToString(reader["[Measures].[Regs_Calc]"]));
                            else
                            rec.SetSqlString(4, reader["[Measures].[Regs_Calc]"] == DBNull.Value ? "0" : Math.Round(Convert.ToDouble(reader["[Measures].[Regs_Calc]"]), 0).ToString());
                        }
                        catch (Exception e)
                        {
                            object[] values = new object[] { };
                            reader.GetValues(values);
                            StringBuilder sb = new StringBuilder();
                            foreach (var ob in values)
                            {
                                sb.Append(ob.ToString());
                            }
                            throw new Exception("[Measures].[Regs_Calc] Error: " + "Check Row Values: " + sb.ToString(), e);
                        }


                        try
                        {
                            value = reader["[Measures].[CPR_Calc]"] == DBNull.Value ? "0" : (Math.Round(Convert.ToDouble(reader["[Measures].[CPR_Calc]"]), 0)).ToString("#,#", CultureInfo.InvariantCulture);
                            rec.SetSqlString(5, string.Format("{0}{1}", value.Equals("0") ? string.Empty : "$", value));
                        }
                        catch (Exception e)
                        {
                            object[] values = new object[] { };
                            reader.GetValues(values);
                            StringBuilder sb = new StringBuilder();
                            foreach (var ob in values)
                            {
                                sb.Append(ob.ToString());
                            }
                            throw new Exception("[Measures].[CPR_Calc] Error: " + "Check Row Values: " + sb.ToString(), e);
                        }

                        try
                        {
                            rec.SetSqlString(6, reader["[Measures].[Actives_Calc]"] == DBNull.Value ? "0" : Math.Round(Convert.ToDouble(reader["[Measures].[Actives_Calc]"]), 0).ToString());
                        }
                        catch (Exception e)
                        {
                            object[] values = new object[] { };
                            reader.GetValues(values);
                            StringBuilder sb = new StringBuilder();
                            foreach (var ob in values)
                            {
                                sb.Append(ob.ToString());
                            }
                            throw new Exception("[Measures].[Actives_Calc] Error: " + "Check Row Values: " + sb.ToString(), e);
                        }

                        try
                        {
                            value = reader["[Measures].[CPA_Calc]"] == DBNull.Value ? "0" : (Math.Round(Convert.ToDecimal(reader["[Measures].[CPA_Calc]"]), 1).ToString("#,#", CultureInfo.InvariantCulture));
                            rec.SetSqlString(7, string.Format("{0}{1}", value.Equals("0") ? string.Empty : "$", value));
                        }
                        catch (Exception e)
                        {
                            object[] values = new object[] { };
                            reader.GetValues(values);
                            StringBuilder sb = new StringBuilder();
                            foreach (var ob in values)
                            {
                                sb.Append(ob.ToString());
                            }
                            throw new Exception("[Measures].[CPA_Calc] Error: " + "Check Row Values: " + sb.ToString(), e);
                        }

                        SqlContext.Pipe.SendResultsRow(rec);

                    }
                    SqlContext.Pipe.SendResultsEnd();
                }

            }

        }
        catch (Exception e)
        {
            throw new Exception(".Net Exception : " + e.ToString(), e);
        }

        returnMsg = string.Format("<br><br>Execution Time: {0:dd/MM/yy H:mm} GMT <br><br>Time Period:"
        + "{1} - {2} ({3} Days) <br>",

        DateTime.Now,
         DateTime.Now.AddDays(-1 * Convert.ToInt64(DaysNum)).ToString("dd/MM/yy"),
         DateTime.Now.AddDays(-1).ToString("dd/MM/yy"),
        DaysNum
                   );

    }


}
