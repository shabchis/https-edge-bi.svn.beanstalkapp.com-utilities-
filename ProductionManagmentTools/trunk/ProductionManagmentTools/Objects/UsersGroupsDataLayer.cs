using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edge.Objects;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Configuration;




namespace Edge.Application.ProductionManagmentTools.Objects
{
	public class UsersGroupsDataLayer
	{
		public string _session;
		public List<User> GetUsers()
		{

			List<User> users;
			HttpWebResponse response;
			HttpWebRequest request = GetServiceRequest("Users", "Get");


			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{

				throw ex;
			}

			users = (List<User>)Deserialize(response.GetResponseStream(), typeof(List<User>));

			return users;




		}

		private HttpWebRequest GetServiceRequest(string methodName, string methodType)
		{
			string fullAddress = string.Format(ConfigurationManager.AppSettings.Get("Edge.Rest.Api.Adress"), methodName);
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(fullAddress);
			request.Method = methodType;
			request.ContentType = "application/json";
			request.Timeout = 9999999;
			request.Accept = "application/json";
			if (methodName.ToLower() != "sessions".ToLower())
				request.Headers.Add("x-edgebi-session", _session);

			if (methodType == "POST" || methodType == "PUT")
			{
				request.Headers["ContentType"] = "application/json";

			}



			return request;

		}
		public void Login(string email, string password)
		{
			HttpWebResponse response;
			SessionRequestData session = new SessionRequestData();
			session.Email = email;
			session.Password = password;


			HttpWebRequest request = GetServiceRequest("Sessions", "POST");
			string body = Serialize(session);
			request.ContentLength = body.Length;
			SetBody(ref request, body);

			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{

				throw ex;
			}



			_session = ((SessionResponseData)Deserialize(response.GetResponseStream(), typeof(SessionResponseData))).Session;

		}

		private void SetBody(ref HttpWebRequest request, string body)
		{
			using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
			{
				writer.Write(body);
			}
		}
		public void Serialize(object instance, Stream stream)
		{
			JsonSerializer serializer = new JsonSerializer();

			using (StreamWriter sw = new StreamWriter(stream))
			{
				using (JsonTextWriter writer = new JsonTextWriter(sw))
				{

					serializer.Serialize(writer, instance);
				}
			}
		}
		public string Serialize(object instance)
		{
			return JsonConvert.SerializeObject(instance);


		}
		public object Deserialize(Stream content, Type T)
		{
			var serializer = new JsonSerializer();

			using (StreamReader sr = new StreamReader(content))
			{
				using (JsonTextReader reader = new JsonTextReader(sr))
				{
					serializer.NullValueHandling = NullValueHandling.Ignore;
					serializer.MissingMemberHandling = MissingMemberHandling.Ignore;

					object result = serializer.Deserialize(reader, T);
					return result;
				}
			}

		}



		internal List<Group> GetGroups()
		{
			List<Group> groups;
			HttpWebResponse response;
			HttpWebRequest request = GetServiceRequest("Groups", "Get");


			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{

				throw ex;
			}

			groups = (List<Group>)Deserialize(response.GetResponseStream(), typeof(List<Group>));

			return groups;
		}

		internal List<Permission> GetPermissionTree()
		{


			List<Permission> permissions;
			HttpWebResponse response;
			HttpWebRequest request = GetServiceRequest("permissions/tree", "Get");


			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{

				throw ex;
			}

			permissions = (List<Permission>)Deserialize(response.GetResponseStream(), typeof(List<Permission>));

			return permissions;
		}

		internal List<Edge.Objects.Account> GetAccountsTree()
		{
			List<Edge.Objects.Account> accounts;
			HttpWebResponse response;
			HttpWebRequest request = GetServiceRequest("accounts", "Get");


			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{

				throw ex;
			}

			accounts = (List<Edge.Objects.Account>)Deserialize(response.GetResponseStream(), typeof(List<Edge.Objects.Account>));

			return accounts;
		}

		internal void SaveUser(UserView user)
		{

			HttpWebResponse response;
			HttpWebRequest request;
			if (user.IsNew)
				request = GetServiceRequest("users", "POST");
			else
				request = GetServiceRequest(string.Format("users/{0}", user.ID), "PUT");


			string body = Serialize(user.GetUser());
			request.ContentLength = body.Length;
			SetBody(ref request, body);


			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{

				throw ex;
			}



		}

		internal Dictionary<int, List<CalculatedPermission>> GetCalculatedPermissions(int userID)
		{
			Dictionary<int, List<CalculatedPermission>> permissions;
			HttpWebResponse response;
			HttpWebRequest request = GetServiceRequest(string.Format("users/{0}/calculatedpermissions", userID), "GET");


			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{

				throw ex;
			}

			permissions = (Dictionary<int, List<CalculatedPermission>>)Deserialize(response.GetResponseStream(), typeof(Dictionary<int, List<CalculatedPermission>>));

			return permissions;

		}

		internal void DeleteUser(UserView user)
		{
			HttpWebResponse response;
			HttpWebRequest request;

			request = GetServiceRequest(string.Format("users/{0}", user.ID), "DELETE");


			//string body = Serialize(user.GetUser());
			//request.ContentLength = body.Length;
			//SetBody(ref request, body);


			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{

				throw ex;
			}

		}

		internal void ChangePasswords(int _userID, string password)
		{
			HttpWebResponse response;
			HttpWebRequest request;

			request = GetServiceRequest(string.Format("users/{0}/password", _userID), "PUT");


			string body = Serialize(password);
			request.ContentLength = body.Length;
			SetBody(ref request, body);


			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{

				throw ex;
			}

		}

		
		

		internal void SaveGroup(GroupView group)
		{
			HttpWebResponse response;
			HttpWebRequest request;
			if (group.IsNew)
				request = GetServiceRequest("groups", "POST");
			else
				request = GetServiceRequest(string.Format("users/{0}", group.ID), "PUT");


			string body = Serialize(group.GetGroup());
			request.ContentLength = body.Length;
			SetBody(ref request, body);


			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{

				throw ex;
			}
		}
	}
}
