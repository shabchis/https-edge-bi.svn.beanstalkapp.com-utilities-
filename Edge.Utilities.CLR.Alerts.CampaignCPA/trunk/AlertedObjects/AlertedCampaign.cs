using Microsoft.SqlServer.Server;
//------------------------------------------------------------------------------
// <copyright file="CSSqlClassFile.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Edge.Utilities.CLR.Alerts.CampaignCPA
{
    public class AlertedCampaign
    {
        public int ID;
        public string Name;
        public double Cost;
        public double Acq1;
        public double Acq2;
        public double CPA;
        public double CPR;
        public bool zeroConv = false;
        public Dictionary<string, object> ExtraFields = new Dictionary<string, object>();
        public Dictionary<string, Dictionary<string, double>> adGroups = new Dictionary<string, Dictionary<string, double>>();
        public double Priority = 0;

        internal void setValuePriority()
        {
            if (Acq1 == 0)
                this.Priority = Cost;
            else
                this.Priority = CPA;
        }

        public AlertedCampaign(SqlDataReader mdxReader, string acq1Field, string cpaField,string acq2Field, string cprField)
        {
            Name = Convert.ToString(mdxReader["[Getways Dim].[Gateways].[Campaign].[MEMBER_CAPTION]"]);
           // SqlContext.Pipe.Send(string.Format("Name = {0}", Name));

            Cost = mdxReader["[Measures].[Cost]"] == DBNull.Value ? 0 : Convert.ToDouble(mdxReader["[Measures].[Cost]"]);
            //SqlContext.Pipe.Send(string.Format("Cost = {0}", Cost));

            CPA = mdxReader["[Measures].[" + cpaField + "]"] == DBNull.Value ? 0 : Convert.ToDouble(mdxReader["[Measures].[" + cpaField + "]"]);
            //SqlContext.Pipe.Send(string.Format("CPA = {0}", CPA));

            Acq1 = mdxReader["[Measures].[" + acq1Field + "]"] == DBNull.Value ? 0 : Convert.ToDouble(mdxReader["[Measures].[" + acq1Field + "]"]);
            //SqlContext.Pipe.Send(string.Format("Conv = {0}", Acq));

            Acq2 = mdxReader["[Measures].[" + acq2Field + "]"] == DBNull.Value ? 0 : Convert.ToDouble(mdxReader["[Measures].[" + acq2Field + "]"]);
            CPR = mdxReader["[Measures].[" + cprField + "]"] == DBNull.Value ? 0 : Convert.ToDouble(mdxReader["[Measures].[" + cprField + "]"]);

            setValuePriority();
        }

        public AlertedCampaign()
        {
            // TODO: Complete member initialization
        }
        public double GetCalculatedCPA()
        {
            if (this.CPA == 0 && this.Cost != 0)
                return this.Cost;
            else
                return this.CPA;
        }


    }
}
