using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edge.Utilities.CLR.Alerts.CampaignCPA
{
	public abstract class AbstractAlertedUnit
	{
		public int ID;
		public string Name;
		public double Cost;
		public double CPA;
		public double Acq1;
		public double CPR;
		public double Acq2;

		public Dictionary<string, object> ExtraFields = new Dictionary<string, object>();

		public double GetCalculatedCPA()
		{
			if (this.CPA == 0 && this.Cost != 0)
				return this.Cost;
			else
				return this.CPA;
		}

		public double GetCalculatedCPR()
		{
			if (this.CPR == 0 && this.Cost != 0)
				return this.Cost;
			else
				return this.CPR;
		}
	}
}
