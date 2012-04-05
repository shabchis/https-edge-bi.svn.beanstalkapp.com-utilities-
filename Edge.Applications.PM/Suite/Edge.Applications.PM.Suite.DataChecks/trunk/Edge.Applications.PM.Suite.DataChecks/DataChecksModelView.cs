using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Edge.Core.Configuration;
using System.Windows.Forms;
using System.Data.SqlClient;
using Edge.Core.Data;
using System.Collections;
using Edge.Applications.PM.Suite.DataChecks.Common;
using Edge.Applications.PM.Suite.DataChecks.Configuration;

namespace Edge.Applications.PM.Suite.DataChecks
{
	public class DataChecksModelView
	{
		#region Members
		/*=============================================================*/
		public Dictionary<string, ValidationType> ValidationsTypes { set; get; }
		public MetricsItemCollection MetricsValidations { set; get; }
		private string _currentConfigPath { set; get; }
		public List<AccountServiceElement> Profiles { set; get; }
		public Dictionary<string, string> AvailableAccountList { set; get; }

		public Dictionary<string,DataChecksBase> SelectedMetricsValidations { set; get; }
	
		/*=============================================================*/
		#endregion
	
		public DataChecksModelView()
		{
			Profiles = new List<AccountServiceElement>();
			AvailableAccountList = new Dictionary<string,string>();
			ValidationsTypes = GetValidationTypesFromConfig();
			MetricsValidations = GetValidationsMetricsFromConfig();
			SelectedMetricsValidations = new Dictionary<string,DataChecksBase>();
			
		}
		/// <summary>
		/// Get Selected Validations Types
		/// </summary>
		public List<ValidationType> GetSelectedValidationTypes()
		{
			//Get all selected types from validationTypes
			var validations =
						from p in ValidationsTypes
						where p.Value.Checked == true
						select p.Value;
			
			return validations.ToList<ValidationType>();
		}

		/// <summary>
		/// Loading validations metrics from configuration
		/// </summary>
		private MetricsItemCollection GetValidationsMetricsFromConfig()
		{
			return ((MetricsValidationsSection)ConfigurationManager.GetSection("DataChecks.MetricsValidations")).MetricsItems;
		}

		/// <summary>
		/// Loading validation types from configuration
		/// </summary>
		private Dictionary<string, ValidationType> GetValidationTypesFromConfig()
		{
			Dictionary<string, ValidationType> validations = new Dictionary<string, ValidationType>();

			ValidationsTypesCollection validationCollection = ((ValidationsTypesSection)ConfigurationManager.GetSection("DataChecks.ValidationTypes")).ValidationTypeItems;
			
			foreach (ValidationTypeItem item in validationCollection)
			{

				if (!validations.ContainsKey(item.Type))
				{
					ValidationType validationTypeValue = new ValidationType();
					validationTypeValue.Name = item.Type;
					validationTypeValue.ServiceToUse.Add(item.ClassRef, item.ServiceToUse);

					validations.Add(item.Type, validationTypeValue);
				}
				else // Add Class Ref to exsiting validation type
				{
					validations[item.Type].ServiceToUse.Add(item.ClassRef, item.ServiceToUse);
				}
			}

			 return validations;
		}

		/// <summary>
		/// Loading Configuration using configuration path from current configuration.
		/// </summary>
		/// <param name="applicationKey"></param>
		internal void LoadProductionConfiguration(string applicationKey)
		{
			string productionPath = ConfigurationManager.AppSettings.Get(applicationKey);
			EdgeServicesConfiguration.Load(productionPath);
		}

		/// <summary>
		/// Getting Accounts Names from Edge DataBase
		/// </summary>
		/// <param name="systemDatabase">AppSetting Configuration Key</param>
		/// <param name="accountsListBox" >Accounts</param>
		/// <param name="availableAccountList" >REF param. The function set this param with available account's names</param>
		internal void LoadAccountsFromDB(string systemDatabase, CheckedListBox accountsListBox, Dictionary<string, string> availableAccountList)
		{
			using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString(this, systemDatabase)))
			{
				sqlCon.Open();
				SqlCommand sqlCommand = DataManager.CreateCommand(
					"SELECT [Account_Name] ,[Account_ID] FROM [dbo].[User_GUI_Account] group by [Account_Name] ,[Account_ID]");
				sqlCommand.Connection = sqlCon;

				using (var _reader = sqlCommand.ExecuteReader())
				{
					if (!_reader.IsClosed)
					{
						while (_reader.Read())
						{
							accountsListBox.Items.Add(string.Format("{0}-{1}", _reader[1], _reader[0]));
							availableAccountList.Add(_reader[1].ToString(), _reader[0].ToString());
						}
					}
				}
			}

		}

		/// <summary>
		/// Getting Profile from configuration file.
		/// </summary>
		/// <param name="key">AppSetting Configuration Key of the  full configuration path ( including file name and extension )</param>
		/// <param name="serviceElement" >OUT param. The function will return the service element that was found. (NULL if account doesnt exists in this configuration)</param>
		/// <returns>True if found, otherwise:False</returns>
		internal bool TryGetProfilesFromConfiguration(string key, ComboBox profilesCombo, List<AccountServiceElement> serviceElement)
		{
			//saving current configuration
			string currentConfigurationFullPath = EdgeServicesConfiguration.Current.CurrentConfiguration.FilePath;
			try
			{
				//Getting configuration path from configuration.
				_currentConfigPath = ConfigurationManager.AppSettings.Get(key);

				AccountElement account;
				TryGetAccountFromExtrernalConfig(_currentConfigPath, accountId: -1, accountElement: out account);

				foreach (AccountServiceElement service in account.Services)
				{
					if (service.Options.ContainsKey("ProfileName"))
					{
						serviceElement.Add(service);
						profilesCombo.Items.Add(service.Options["ProfileName"]);
					}
				}

			}
			catch
			{
				//Loading original configuration
				EdgeServicesConfiguration.Load(currentConfigurationFullPath);
				//Directory.SetCurrentDirectory((Path.GetDirectoryName(currentConfigurationFullPath)));
				return false;
			}

			//Loading original configuration
			EdgeServicesConfiguration.Load(currentConfigurationFullPath);
			//Directory.SetCurrentDirectory((Path.GetDirectoryName(currentConfigurationFullPath)));
			return true;


		}

		/// <summary>
		/// Getting Account Configuration Element from specific configuration
		/// </summary>
		/// <param name="fullPath">Configuration Path ( including file name and extension )</param>
		/// <param name="accountId" >Account ID in the configuration file</param>
		/// <param name="accountElement" >OUT param. The function will return the account element that was found. (NULL if account doesnt exists in this configuration)</param>
		/// <returns>True if found, otherwise:False</returns>
		private bool TryGetAccountFromExtrernalConfig(string fullPath, int accountId, out AccountElement accountElement)
		{
			try
			{
				EdgeServicesConfiguration.Load(fullPath);
				AccountElementCollection accounts = EdgeServicesConfiguration.Current.Accounts;
				accountElement = accounts.GetAccount(accountId);
				return true;
			}
			catch
			{
				accountElement = null;
				return false;
			}
		}

		internal void SetValidationTypesItems(TreeNodeCollection itemCollection)
		{
			foreach (var item in ValidationsTypes)
			{
				TreeNode node = new TreeNode(item.Key.ToString());
				node.Tag = item.Value;
				
				itemCollection.Add(node);
				
			}
		}

		internal void SetMetricsValidationsItems(TreeNodeCollection itemCollection)
		{
			foreach (MetricsItem item in MetricsValidations)
			{
				TreeNode node = new TreeNode(item.Name);
				node.Tag = item;
				itemCollection.Add(node);
			}
		}

		internal object GetConfigurationSection(string sectionName)
		{
			try
			{

				var val = ConfigurationManager.GetSection(sectionName);
				return val;
			}
			catch (Exception e)
			{
				throw new Exception("Configuration Error", e);
			}

		}


	}

	public class ValidationType
	{
		public bool Checked { set; get; }
		public string Name { set; get; }
		public Dictionary<string, string> ServiceToUse { set; get; }

		public ValidationType()
		{
			ServiceToUse = new Dictionary<string, string>();
		}

		public string GetServiceNameByClass(string className)
		{
			return ServiceToUse[className];
		}
	}
}
