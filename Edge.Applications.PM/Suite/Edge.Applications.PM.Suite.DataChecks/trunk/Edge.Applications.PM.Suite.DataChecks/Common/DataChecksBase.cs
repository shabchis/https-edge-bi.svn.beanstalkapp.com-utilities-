using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edge.Data.Pipeline;
using Edge.Core.Configuration;
using System.Windows.Forms;

namespace Edge.Applications.PM.Suite.DataChecks.Common
{
	abstract public class DataChecksBase
	{
		public string Channel { set; get; }
		public DateTimeRange TimePeriod { set; get; }
		public bool RunHasLocal { set; get; }
		public Edge.Core.Services.ServiceInstance ServiceInstance { set; get; }

		/// <summary>
		/// Run service as local service
		/// </summary>
		/// <param name="SelectedTypes">List of all validation services such as "DeliveryOltp"</param>
		/// <param name="SelectedAccounts">Collection of selected accounts</param>
		abstract public void RunUsingLocalConfig(List<ValidationType> SelectedTypes, ListBox.SelectedObjectCollection SelectedAccounts , EventHandler eventHandler);
	

		virtual public void RunUsingExternalConfig(Dictionary<string, string> SelectedTypes, ListBox.SelectedObjectCollection SelectedAccounts)
		{

		}

		/// <summary>
		/// Initializing service from current configuration
		/// </summary>
		/// <param name="timePeriod">Service time period</param>
		/// <param name="service">Service Name , See Const</param>
		/// <param name="channels">Channels List in string format seperated by comma</param>
		/// <param name="accounts">Accounts List in string format seperated by comma</param>
		internal void InitServices(DateTimeRange timePeriod, string service, string channels, string accounts, EventHandler eventHandler)
		{
			ActiveServiceElement serviceElements = new ActiveServiceElement(EdgeServicesConfiguration.Current.Accounts.GetAccount(-1).Services[service]);
			
			//Removing overide options 
			serviceElements.Options.Remove("fromDate");
			serviceElements.Options.Remove("toDate");
			serviceElements.Options.Remove("ChannelList");
			serviceElements.Options.Remove("AccountsList");
			
			// TimePeriod
			serviceElements.Options.Add("fromDate", timePeriod.Start.ToDateTime().ToString());
			serviceElements.Options.Add("toDate", timePeriod.End.ToDateTime().ToString());

			serviceElements.Options.Add("ChannelList", channels);
			serviceElements.Options.Add("AccountsList", accounts);

			//Setting Service Options
			//if (service.Equals(Edge.Applications.PM.Suite.DataChecks.Const.AdMetricsConst.DeliveryOltpService))
			//{
			//    if (!serviceElements.Options.Keys.Contains("SourceTable"))
			//        serviceElements.Options.Add("SourceTable", Edge.Applications.PM.Suite.DataChecks.Const.AdMetricsConst.OltpTable);
			//}
			//else if (service.Equals(Edge.Applications.PM.Suite.DataChecks.Const.AdMetricsConst.OltpDwhService))
			//{
			//    if (!serviceElements.Options.Keys.Contains("SourceTable"))
			//        serviceElements.Options.Add("SourceTable", Edge.Applications.PM.Suite.DataChecks.Const.AdMetricsConst.OltpTable);
			//    if (!serviceElements.Options.Keys.Contains("TargetTable"))
			//        serviceElements.Options.Add("TargetTable", Edge.Applications.PM.Suite.DataChecks.Const.AdMetricsConst.DwhTable);
			//}
			//else if (service.Equals(Edge.Applications.PM.Suite.DataChecks.Const.AdMetricsConst.MdxOltpService))
			//{
			//    if (!serviceElements.Options.Keys.Contains("SourceTable"))
			//        serviceElements.Options.Add("SourceTable", Edge.Applications.PM.Suite.DataChecks.Const.AdMetricsConst.OltpTable);
			//}
			//else if (service.Equals(Edge.Applications.PM.Suite.DataChecks.Const.AdMetricsConst.MdxDwhService))
			//    serviceElements.Options.Add("SourceTable", Edge.Applications.PM.Suite.DataChecks.Const.AdMetricsConst.DwhTable);

			//else if (service.Equals(Edge.Applications.PM.Suite.DataChecks.Const.AdMetricsConst.MdxDwhService))
			//    serviceElements.Options.Add("SourceTable", Edge.Applications.PM.Suite.DataChecks.Const.AdMetricsConst.DwhTable);
			//else
			//    //TO DO : Get tabels from configuration.
			//    throw new Exception("ComparisonTable hasnt been implemented for this service");

			ServiceInstance = Edge.Core.Services.Service.CreateInstance(serviceElements);
			//ServiceInstance.OutcomeReported += eventHandler;
			
			//instance.StateChanged += new EventHandler<Edge.Core.Services.ServiceStateChangedEventArgs>(instance_StateChanged);
			//instance.ProgressReported += new EventHandler(instance_ProgressReported);
			ServiceInstance.Initialize();



		}

		#region Events Handler Section

		void instance_OutcomeReported(object sender, EventArgs e)
		{
			Edge.Core.Services.ServiceInstance instance = (Edge.Core.Services.ServiceInstance)sender;
			
			/*TO DO :
			 * 
			 * UPDATE LOG
			 * UPDATE PROGRESS BAR
			 * WHEN ALL FINISHED SET RESULT IMAGE AND STATUS
			*/
		}
		
		#endregion
		
		
	}


}
