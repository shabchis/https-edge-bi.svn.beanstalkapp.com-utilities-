using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.Runtime.InteropServices;
//using TaskSchedulerInterop;


using System.Net;
//using TaskScheduler;
using System.Threading;
using System.Diagnostics;

namespace ServicesAlert
{
	class Program
	{

		private static void RerunFileWatcherServiceHost()
		{
			Process[] processes = Process.GetProcessesByName("SeperiaFileWatcher");
			if (processes.Length > 1)
			{
				processes[0].Refresh();
			}
		}

		private static void SendAlert(List<String> ImporterDirsErr)
		{
			string _toAddress, _fromAddress;
			System.Net.Mail.MailMessage msg = new MailMessage();
			msg.IsBodyHtml = false;
			msg.Subject = "Services Alerts";
			msg.Priority = MailPriority.High;
			try
			{
				if (ImporterDirsErr.Count != 0)
				{
					msg.Body = "<H4 style=\"font-family:verdana;color:red\">Cloud importer Alert !!!</H4>";
					msg.Body += "<H6 style=\"font-family:verdana\">Importer directories have been tested and the following error directories were found:</H6>";
					foreach (string path in ImporterDirsErr)
					{
						msg.Body += "<span style=\"font-family:verdana;font-size:0.35cm\">" + path + "</span>";
					}

				}
				else msg.Body = "<H2>Importer directories have been tested and no errors were found.</H2>";
				SmtpClient smtp = Smtp.GetSmtpConnection(out _toAddress, out _fromAddress);
				msg.To.Add(_toAddress);
				msg.From = new MailAddress(_fromAddress);
				Console.WriteLine(_fromAddress);
				smtp.Send(msg);
				System.Console.WriteLine("Message Sent");
			}
			catch (Exception e)
			{
				Console.WriteLine("Error : Cannot send mail");
				Console.WriteLine(e);


			}

		}
		private static IDictionary GetServices()
		{
			IDictionary srvcs = null;
			try
			{
				srvcs = (IDictionary)ConfigurationSettings.GetConfig("ServicesPath");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return srvcs;

		}
		static void Main(string[] args)
		{
			List<String> errorServices = new List<string>();
			IDictionary services = GetServices();
			bool errors = IsServiceDown(services, errorServices);
			//if (errors)
			//	SendAlert(errorServices);
		}

		private static bool IsServiceDown(IDictionary services, List<string> errorServices)
		{
			bool down = false;
			foreach (DictionaryEntry service in services)
			{
				Process[] processes = Process.GetProcessesByName(service.Key.ToString());
				// service is running
				if (processes.Length >= 1)
				{
					processes[0].Refresh();
					//processes[0].Close();
				}

				down = true;
				errorServices.Add(service.Key.ToString());
				Process proc = new Process();
				proc.StartInfo.FileName = service.Value.ToString();
				proc.StartInfo.Arguments = "/"+service.Key.ToString();
				proc.Start();


			}

			return down;
		}
	}
}
