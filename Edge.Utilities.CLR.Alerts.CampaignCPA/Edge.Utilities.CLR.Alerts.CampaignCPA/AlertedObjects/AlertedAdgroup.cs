using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edge.Utilities.CLR.Alerts.CampaignCPA.AlertedObjects
{
	public class AlertedAdgroup : AbstractAlertedUnit
	{
		public double Priority = 1;
		public string CampaignName = string.Empty;

		public AlertedAdgroup(System.Data.SqlClient.SqlDataReader reader, string extraFields, string acq1FieldName, string acq2FieldName, string cpaFieldName, string cprFieldName)
		{
			this.Name = Convert.ToString(reader["[Getways Dim].[Gateways].[Ad Group].[MEMBER_CAPTION]"]);
			CampaignName = Convert.ToString(reader["[Getways Dim].[Gateways].[Campaign].[MEMBER_CAPTION]"]);

			Dictionary<string, object> ExtraFields = new Dictionary<string, object>();

			this.Cost = reader["[Measures].[Cost]"] == DBNull.Value ? 0 : Convert.ToDouble(reader["[Measures].[Cost]"]);
			this.Acq1 = reader["[Measures].[" + acq1FieldName + "]"] == DBNull.Value ? 0 : Convert.ToDouble(reader["[Measures].[" + acq1FieldName + "]"]);
			this.Acq2 = reader["[Measures].[" + acq2FieldName + "]"] == DBNull.Value ? 0 : Convert.ToDouble(reader["[Measures].[" + acq2FieldName + "]"]);
			this.CPR = reader["[Measures].[" + cprFieldName + "]"] == DBNull.Value ? 0 : Convert.ToDouble(reader["[Measures].[" + cprFieldName + "]"]);
			this.CPA = reader["[Measures].[" + cpaFieldName + "]"] == DBNull.Value ? 0 : Convert.ToDouble(reader["[Measures].[" + cpaFieldName + "]"]);

			if (this.Cost != 0)
				this.Priority = (this.Acq2 * 1000 + this.Acq1 * 100) / this.Cost;

			if (!string.IsNullOrEmpty(extraFields))
			{
				foreach (string extraField in extraFields.Split(','))
				{
					this.ExtraFields.Add(extraField, Convert.ToDouble(reader["[Measures].[" + extraField + "]"]));
				}

			}
		}
	}
}
