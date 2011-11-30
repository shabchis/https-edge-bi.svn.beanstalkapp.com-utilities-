using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edge.Core.Configuration;
using Edge.Core.Scheduling;
using Edge.Core.Services;
using Edge.Core;

namespace Edge.Anaylysis.Validation
{
	class MdxOltpValidation
	{
		static void Main(string[] args)
		{
			string serviceName = "MdxOltpValidation";
			ServiceClient<IScheduleManager> scheduleManager = new ServiceClient<IScheduleManager>();
			//run the service
			scheduleManager.Service.AddToSchedule(serviceName, -1, DateTime.Now, new SettingsCollection());
		}

	}
}
