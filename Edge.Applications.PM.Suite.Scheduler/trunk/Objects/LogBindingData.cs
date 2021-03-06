﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data.SqlClient;
using Edge.Core.Configuration;


namespace Edge.Applications.PM.SchedulerControl.Objects
{
	public class LogBindingData : INotifyPropertyChanged
	{
		public Dictionary<string,string> LogHeader { get; set; }
		public ServiceHistoryView ServiceHistoryView { get; set; }
		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		internal void GetLogMessage()
		{
			
			using (SqlConnection conn = new SqlConnection(AppSettings.GetConnectionString(this, "Log")))
			{
				conn.Open();
				{
					SqlCommand command = new SqlCommand(@"SELECT Message,ExceptionDetails 
																		FROM Log
																		WHERE ServiceInstanceID=@InstanceID");
					command.Parameters.AddWithValue("@InstanceID", ServiceHistoryView.InstanceID);

					command.Connection = conn;

					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())						
						{
							if (LogHeader == null)
								LogHeader = new Dictionary<string, string>();
							string message=reader["Message"].ToString();
							
							while (LogHeader.ContainsKey(message))
							{
								message += " ";
								
							}
							LogHeader.Add(message, reader["ExceptionDetails"].ToString());
						}
					}
				}

			}
			

		}
	}
}

