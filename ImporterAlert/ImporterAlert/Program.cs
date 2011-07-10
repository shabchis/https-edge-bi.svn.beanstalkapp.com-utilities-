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

namespace ImporterAlert
{
    class Program
    {

        static bool IsDirectoryEmpty(List<String> ImporterDirs,List<String> ImporterDirsErr)
        {
           // if ImporterDirs.
			string TargetPath = ConfigurationSettings.AppSettings["CopyPastTempDir"];
			string fileName, TempFilePath;
			bool ReTask = false;
            foreach (string path in ImporterDirs)
            {
                try
                {
                    string[] dirs = System.IO.Directory.GetDirectories(path);
                    string[] files = System.IO.Directory.GetFiles(path);
                    if (dirs.Length == 0 && files.Length == 0)
                    {
                       Console.WriteLine("Empty");
					   if (!ReTask)
					   {
						   RerunFileWatcherServiceHost();
						   ReTask = true;
					   }
                    }
                    else
                    {
                        Console.WriteLine("Not Empty");
                        ImporterDirsErr.Add(path);
						//ReImport the files
						foreach (string ImportFilePath in files)
						{
							// check file date and time if exists more that 20 min than ReImport it
							fileName = System.IO.Path.GetFileName(ImportFilePath);
							DateTime LastAccess = System.IO.File.GetLastAccessTime(ImportFilePath);
							DateTime CurTime = DateTime.Now;
							if (!CheckFileTime(LastAccess))
							{
								//check if service host is down 
								// if down - run service host and wait 1 min 
//								CheckHost();
								TempFilePath = System.IO.Path.Combine(TargetPath, fileName);
								//ImportFilePath = System.IO.Path.Combine(path,fileName);
								System.IO.File.Move(ImportFilePath, TempFilePath); // move to temp directory
								System.IO.File.SetLastWriteTime(TempFilePath, DateTime.Now); // update creation time
								System.IO.File.Move(TempFilePath, ImportFilePath); // ReImport
							}

						}

                    }
                }
                catch(Exception e)
                {
                   // Console.WriteLine(e);
                    Console.WriteLine(path+" doesn't exists");
                }
            }
            if (0 != ImporterDirsErr.Count) return true;
            return false;
            //sw.Stop();
           // Console.WriteLine(sw.ElapsedMilliseconds);

        }

		private static void RerunFileWatcherServiceHost()
		{
			Process[] processes = Process.GetProcessesByName("SeperiaFileWatcher");
			if (processes.Length > 1)
			{
				processes[0].Refresh();
			}
		}

		//private static void CheckHost()
		//{
		//    ScheduledTasks st = new ScheduledTasks(@"\\ShayBa-PC");
			
		//    // Get an array of all the task names
		//    string[] taskNames = st.GetTaskNames();
		//    taskNames = st.GetTaskNames();

		//    // Open each task, write a descriptive string to the console
		//    var taskName = taskNames.First(a => a == "shay");
		//    //foreach (string name in taskNames)
		//    {
		//        Task t = st.OpenTask(taskName);
		//        if (t.Status == TaskStatus.Ready)
		//        {
		//            t.Run();
		//            Thread.Sleep(30000);
		//            return;
		//        }
				
		//    }

		//    // Dispose the ScheduledTasks object to release COM resources.
		//    st.Dispose();
		//}

		private static bool CheckFileTime(DateTime CreationTime)
		{
			DateTime _now = DateTime.Now;
			TimeSpan _timeSpan = _now.Subtract(CreationTime);
			if (0 == _timeSpan.Days)
				if (0 == _timeSpan.Hours)
					if (_timeSpan.Minutes >= 20)
						return false;
					else return true;
			return false;

		}
        private static void SendAlert(List<String> ImporterDirsErr )
        {
            string _toAddress, _fromAddress;
            System.Net.Mail.MailMessage msg = new MailMessage();
            msg.IsBodyHtml = true;
            msg.Subject = "Importer Alerts";
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
                SmtpClient smtp = Smtp.GetSmtpConnection(out _toAddress,out _fromAddress);
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
        private static List<String> GetDir()
        {   
            List<String> ImporterDirsErr = new List<string>();
			//string dir = ConfigurationSettings.AppSettings["CopyPastTempDir"];
            try
            {
                IDictionary Dirs = (IDictionary)ConfigurationSettings.GetConfig("Directories");
                if (0 != Dirs.Count)
                {
                    string DirsKey = null;
                    var Index = Dirs.Count;
                    while (0 != Index)
                    {
                        DirsKey = "Dir" + Index.ToString();
                        ImporterDirsErr.Add(Dirs[DirsKey].ToString());
                        Index--;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
                   
            return ImporterDirsErr;

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
		private static bool IsServiceDown(IDictionary services, List<string> errorServices)
		{
			bool down = false;
			foreach (DictionaryEntry service in services)
			{
				Process[] processes = Process.GetProcessesByName(service.Key.ToString());
				// service is running
				//if (processes.Length >= 1)
				//{
				//    processes[0].Refresh();
				//    //processes[0].Close();
				//}

				down = true;
				errorServices.Add(service.Key.ToString());
				Process proc = new Process();
				proc.StartInfo.FileName = service.Value.ToString();
				proc.StartInfo.Arguments = "/" + service.Key.ToString();
				proc.Start();


			}

			return down;
		}
        static void Main(string[] args)
        {
           List<String> ImporterDirsErr= new List<string>();
		   List<String> UnrunningServices = new List<string>();
		   IDictionary services = GetServices();
		   bool IsDown = IsServiceDown(services, UnrunningServices);
		   if (IsDown) Thread.Sleep(60000);
		   var ImporterDirs = GetDir();
           var errors = IsDirectoryEmpty(ImporterDirs, ImporterDirsErr);
		   //if (errors) SendAlert(ImporterDirsErr);
        }
    }
}
