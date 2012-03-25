using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using Edge.Core.Utilities;
using Edge.Core.Configuration;
using Edge.Data.Pipeline;
using Edge.Core.Data;

namespace Edge_Applications.PM.common
{
	static class Program
	{
		//public static DeliveryDBServer DeliveryServer;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			// Get an alternate file name
			try
			{

				

				
				Application.Run(new frmServicesControl());

			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.Message + ex.StackTrace + ex.InnerException + ex.Data + ex.TargetSite);
			}
		}

		




	

	
	}
}
