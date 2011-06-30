using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using Edge.Core.Utilities;
using Edge.Core.Configuration;
using Edge.Data.Pipeline;
using Edge.Core.Data;

namespace Edge.Applications.TempScheduler
{
	static class Program
	{
		public static DeliveryDBServer DeliveryServer;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			// Get an alternate file name
			try
			{

				string configFileName = EdgeServicesConfiguration.DefaultFileName;
				if (args.Length > 0 && args[0].StartsWith("/") && args[0].Length > 1)
				{
					configFileName = args[0].Substring(1);
				}
				EdgeServicesConfiguration.Load(configFileName);
				DataManager.ConnectionString = AppSettings.GetConnectionString("Edge.Data.Pipeline.Importing.AdDataImportSession", "Oltp");

				DeliveryServer = new DeliveryDBServer();
				DeliveryServer.Start(null);

				AppDomain currentDomain = AppDomain.CurrentDomain;
				currentDomain.UnhandledException += new UnhandledExceptionEventHandler(currentDomain_UnhandledException);
				Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
				Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new frmSchedulingControl());
			}
			catch (Exception ex)
			{
				
				MessageBox.Show(ex.Message + ex.StackTrace + ex.InnerException + ex.Data+ex.TargetSite);
			}
		}

		static void Application_ApplicationExit(object sender, EventArgs e)
		{
			DeliveryServer.Stop();
		}

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            
            MessageBox.Show(e.Exception.Message);
            Smtp.Send("SchedulerTester  exception", true, string.Format("Message:\n{0}\nInner Exception:\n{1}\nExeption.ToString():\n{2}\nIsTerminating:{3}\nStack:\n{4}", e.Exception.Message, e.Exception.InnerException, e.Exception, "true", e.Exception.StackTrace), false, string.Empty);
            Log.Write("SchedulerTester", e.Exception.Message, e.Exception, LogMessageType.Error);
            MessageBox.Show(e.Exception.Message);
        }

        static void currentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            
            Exception ex=(Exception)e.ExceptionObject;           
            Smtp.Send("SchedulerTester  exception", true, string.Format("Message:\n{0}\nInner Exception:\n{1}\nExeption.ToString():\n{2}\nIsTerminating:{3}\nStack:\n{4}", ex.Message, ex.InnerException, ex, e.IsTerminating,ex.StackTrace), false, string.Empty);
            Log.Write("SchedulerTester", ex.Message, ex, LogMessageType.Error);
            MessageBox.Show(ex.Message);
            

        }
	}
}
