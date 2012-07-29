using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edge.Data.Pipeline;
using Edge.Core.Configuration;
using System.Windows.Forms;
using Edge.Core.Services;

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
		abstract public void RunUsingLocalConfig(List<ValidationType> SelectedTypes, CheckedListBox.CheckedItemCollection SelectedAccounts, Dictionary<string, Object> EventsHandlers);


		virtual public void RunUsingExternalConfig(Dictionary<string, string> SelectedTypes, ListBox.SelectedObjectCollection SelectedAccounts)
		{

		}

		/// <summary>
		/// Initializing service from current configuration
		/// </summary>
		/// <param name="timePeriod">Service time period</param>
		/// <param name="serviceName">Service Name , See Const</param>
		/// <param name="channels">Channels List in string format seperated by comma</param>
		/// <param name="accounts">Accounts List in string format seperated by comma</param>
		internal void InitServices(DateTimeRange timePeriod, string serviceName, string channels, string accounts, Dictionary<string, Object> eventsHandlers)
		{
			try
			{
				ActiveServiceElement serviceElements = new ActiveServiceElement(EdgeServicesConfiguration.Current.Accounts.GetAccount(-1).Services[serviceName]);



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
				serviceElements.Options.Add("PmsServiceName", serviceName);


				//Update WorkFlow
				foreach (WorkflowStepElement step in serviceElements.Workflow)
				{
					//CHECK IF CHILD SERVICE OPTION IS -> PMS = ENABLED
					if (step.Options.ContainsKey("PMS") && !step.Options["PMS"].ToString().ToUpper().Equals("ENABLED"))
					{
						step.IsEnabled = false;
					}
					else
						step.IsEnabled = true;
				}

				ServiceInstance = Edge.Core.Services.Service.CreateInstance(serviceElements);

				ServiceInstance.OutcomeReported += (EventHandler)eventsHandlers[Const.EventsTypes.ParentOutcomeReportedEvent];
				ServiceInstance.StateChanged += (EventHandler<ServiceStateChangedEventArgs>)eventsHandlers[Const.EventsTypes.ParentStateChangedEvent];
				ServiceInstance.ChildServiceRequested += (EventHandler<ServiceRequestedEventArgs>)eventsHandlers[Const.EventsTypes.ChildServiceRequested];

				//instance.ProgressReported += new EventHandler(instance_ProgressReported);
				ServiceInstance.Initialize();

			}
			catch (Exception ex)
			{

				throw new Exception("Could not init validation service, Check configuration", ex);
			}



		}




	}



}
