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
		public static string LS;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			LS = AppDomain.CurrentDomain.FriendlyName;

			// Get an alternate file name
			string configFileName = EdgeServicesConfiguration.DefaultFileName;
			if (args.Length > 0 && args[0].StartsWith("/") && args[0].Length > 1)
			{
				configFileName = args[0].Substring(1);
			}
			try
			{
				EdgeServicesConfiguration.Load(configFileName);
			}
			catch (Exception ex)
			{
				MessageBox.Show(String.Format("{0}\n({1})", ex.Message, ex.GetType().Name), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmSchedulingControl());
		}

		static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			MessageBox.Show(String.Format("{0}\n({1})", e.Exception.Message, e.Exception.GetType().Name), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			const string msg = "Fatal error: an unhandled exception is forcing the application to end.";
			Exception ex = e.ExceptionObject as Exception;
			Log.Write(Program.LS, msg, ex);
			MessageBox.Show(msg + (ex == null ? null : String.Format("{0}\n({1})", ex.Message, ex.GetType().Name)), "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
