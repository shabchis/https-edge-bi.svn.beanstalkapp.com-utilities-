using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edge.Applications.PM.Suite.DataChecks.Common;

namespace Edge.Applications.PM.Suite.DataChecks
{
	public class AdMetricsValidation : DataChecksBase
	{

		public override void RunUsingLocalConfig(List<ValidationType> SelectedTypes, System.Windows.Forms.ListBox.SelectedObjectCollection SelectedAccounts, Dictionary<string, Object> EventsHandlers)
		{
			//Prepering accounts list
			StringBuilder accounts = new StringBuilder();
			foreach (string item in SelectedAccounts)
			{
				accounts.Append(item.Split('-')[0]+",");
			}

			foreach (ValidationType type in SelectedTypes)
			{
				//RUN SERVICE
				base.InitServices(TimePeriod, type.ServiceToUse[this.GetType().Name], Channel, accounts: accounts.ToString().Remove(accounts.Length - 1), eventsHandlers: EventsHandlers);
				
			}
		}

		

	}
}
