using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Edge.Core.Configuration;

namespace Edge.Applications.PM.Suite.DataChecks
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			EdgeServicesConfiguration.Load("Edge.Applications.PM.Suite.DataChecks.exe.config");
			System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			System.Windows.Forms.Application.Run(new Edge.Applications.PM.Suite.DataChecks.DataChecksForm());
		}
	}
}
