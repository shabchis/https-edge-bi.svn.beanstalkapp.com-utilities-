using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edge.Utilities.CLR.Alerts.CampaignCPA.AlertedObjects
{
	public class AlertedAdgroup : AbstractAlertedUnit
	{
		public AlertedAdgroup(System.Data.SqlClient.SqlDataReader reader, string extraFields, string acq1FieldName, string acq2FieldName, string cpaFieldName, string cprFieldName)
		{
			this.Name =  Convert.ToString(reader["[Getways Dim].[Gateways].[Ad Group].[MEMBER_CAPTION]"]);

			Dictionary<string, object> ExtraFields = new Dictionary<string, object>();

			this.Cost = reader["[Measures].[Cost]"] == DBNull.Value ? 0 : Convert.ToDouble(reader["[Measures].[Cost]"]);
			this.Acq1 = reader["[Measures].[" + acq1FieldName + "]"] == DBNull.Value ? 0 : Convert.ToDouble(reader["[Measures].[" + acq1FieldName + "]"]);
			this.Acq2 = reader["[Measures].[" + acq1FieldName + "]"] == DBNull.Value ? 0 : Convert.ToDouble(reader["[Measures].[" + acq1FieldName + "]"]);
			this.CPR = reader["[Measures].[" + cprFieldName + "]"] == DBNull.Value ? 0 : Convert.ToDouble(reader["[Measures].[" + cprFieldName + "]"]);
			this.CPA = reader["[Measures].[" + cpaFieldName + "]"] == DBNull.Value ? 0 : Convert.ToDouble(reader["[Measures].[" + cpaFieldName + "]"]);

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
