using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Edge.Core.Configuration;

namespace Edge.Applications.PM.Suite.DeliverySearch
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
            EdgeServicesConfiguration.Load("Edge.Application.ProductionManagmentTools.exe.config");
			System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new DeliverySearch());
		}
	}
}
