using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Edge.Utilities.CLR.Alerts.CampaignCPA.AlertedObjects;

namespace Edge.Utilities.CLR.Alerts.CampaignCPA
{
	public class AlertedCampaignAdgroups : AbstractAlertedUnit
	{
		public List<Edge.Utilities.CLR.Alerts.CampaignCPA.AlertedObjects.AlertedAdgroup> AdGroups;
        public double Priority = 0;

		public AlertedCampaignAdgroups()
		{
			AdGroups = new List<AlertedObjects.AlertedAdgroup>();
		}

		public AlertedCampaignAdgroups(SqlDataReader reader, string extraFields, string acq1FieldName, string acq2FieldName, string cpaFieldName, string cprFieldName)
		{
			AdGroups = new List<AlertedObjects.AlertedAdgroup>();
			this.Name = Convert.ToString(reader["[Getways Dim].[Gateways].[Campaign].[MEMBER_CAPTION]"]);
			this.AddAdgroup(reader, extraFields, acq1FieldName, acq2FieldName, cpaFieldName, cprFieldName);
		}

		public void AddAdgroup(SqlDataReader reader, string extraFields, string acq1FieldName, string acq2FieldName, string cpaFieldName, string cprFieldName)
		{
			AlertedAdgroup agroup = new AlertedAdgroup(reader, extraFields, acq1FieldName, acq2FieldName, cpaFieldName, cprFieldName);
			this.Cost += agroup.Cost;
			this.Acq1 += agroup.Acq1;
			this.Acq2 += agroup.Acq2;
			this.CPA += agroup.CPA;
			this.CPR += agroup.CPR;

			//ExtraFields
			if (!string.IsNullOrEmpty(extraFields))
			{
				foreach (string extraField in extraFields.Split(','))
				{
					agroup.ExtraFields.Add(extraField, Convert.ToDouble(reader["[Measures].[" + extraField + "]"]));
					this.ExtraFields.Add(extraField, Convert.ToDouble(reader["[Measures].[" + extraField + "]"]));
				}

			}
			this.AdGroups.Add(agroup);

		}
        internal void setValuePriority(double totalAvgCPA, double totalAvgCPR)
        {
            this.Priority = Cost - this.Acq2 * totalAvgCPA - this.Acq1 * totalAvgCPR;
        }
	}
}
