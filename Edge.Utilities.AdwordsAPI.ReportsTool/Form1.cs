﻿using Edge.Core.Data;
using Edge.Core.Utilities;
using Google.Api.Ads.AdWords.Lib;
using Google.Api.Ads.AdWords.Util.Reports;
using Google.Api.Ads.AdWords.v201302;
using Google.Api.Ads.Common.Lib;
using Google.Api.Ads.Common.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Edge.Utilities.AdwordsAPI.ReportsTool
{
    public partial class Form1 : Form
    {
        private const int MAX_ERROR_LENGTH = 4096;
        AdWordsUser User;
        StringBuilder sb;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Setting Adwords User


            foreach (var item in Enum.GetNames(typeof(ReportDefinitionReportType)))
            {
                this.ReportNamesListBox.Items.Add(item);
            }

            this.dataGridView.Columns.Add("fieldName", "fieldName");
            this.dataGridView.Columns.Add("displayFieldName", "displayFieldName");
            this.dataGridView.Columns.Add("fieldType", "fieldType");
            this.dataGridView.Columns.Add("isBeta", "isBeta");
            this.dataGridView.Columns.Add("canFilterSpecified", "canFilterSpecified");
            this.dataGridView.Columns.Add("canSelect", "canSelect");
            this.dataGridView.Columns.Add("canSelectSpecified", "canSelectSpecified");
            this.dataGridView.Columns.Add("enumValuePairs", "enumValuePairs");
            this.dataGridView.Columns.Add("enumValues", "enumValues");
            this.dataGridView.Columns.Add("isBetaSpecified", "isBetaSpecified");
            this.dataGridView.Columns.Add("isEnumType", "isEnumType");
            this.dataGridView.Columns.Add("isEnumTypeSpecified", "isEnumTypeSpecified");
            this.dataGridView.Columns.Add("xmlAttributeName", "xmlAttributeName");
            this.dataGridView.Columns.Add("canFilter", "canFilter");


        }

        private void GetReportFields_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
						{
							{"DeveloperToken" ,this.DeveloperToken.Text},
							{"UserAgent" ,String.Format("Edge File Manager (version {0})", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())},
							{"EnableGzipCompression",this.EnableGzipCompression.Text},
							{"ClientCustomerId",this.ClientCustomerId.Text},
							{"Email",this.Email.Text}
						};


            User = new AdWordsUser(headers);
            try
            {
                //Getting AuthToken
                (User.Config as AdWordsAppConfig).AuthToken = AdwordsUtill.GetAuthToken(User);
                ReportDefinitionReportType reportType = (ReportDefinitionReportType)Enum.Parse(typeof(ReportDefinitionReportType), ReportNamesListBox.SelectedItem.ToString());
                ReportDefinitionService reportDefinitionService = (ReportDefinitionService)User.GetService(AdWordsService.v201302.ReportDefinitionService);

                // Get the report fields.
                ReportDefinitionField[] reportDefinitionFields = reportDefinitionService.getReportFields(reportType);
                foreach (ReportDefinitionField reportDefinitionField in reportDefinitionFields)
                {
                    this.AvailableReportFields.AppendText(string.Format(@"""{0}"",",reportDefinitionField.fieldName ));
                    List<object> rowObjects = new List<object>();

                    this.dataGridView.Rows.Add(
                                        reportDefinitionField.fieldName,
                                        reportDefinitionField.displayFieldName,
                                        reportDefinitionField.fieldType,
                                        reportDefinitionField.isBeta,
                                        reportDefinitionField.canFilterSpecified,
                                        reportDefinitionField.canSelect,
                                        reportDefinitionField.canSelectSpecified,
                                        reportDefinitionField.enumValuePairs,
                                        reportDefinitionField.enumValues,
                                        reportDefinitionField.isBetaSpecified,
                                        reportDefinitionField.isEnumType,
                                        reportDefinitionField.isEnumTypeSpecified,
                                        reportDefinitionField.xmlAttributeName,
                                        reportDefinitionField.canFilter);
                }

                this.AvailableReportFields.Text.Remove(this.AvailableReportFields.Text.Length - 1, 1);
            }
            catch (Exception ex)
            {
                this.response.Text = string.Format("{0} Inner: {1}", ex.Message, ex.InnerException.Message);
            }
        }

        public static class AdwordsUtill
        {
            public static string GetAuthToken(AdWordsUser user, bool generateNew = false)
            {

                string pass;
                string auth = string.Empty;

                auth = GetAuthFromDB((user.Config as AdWordsAppConfig).Email, out pass);

                //Set User Password
                (user.Config as AdWordsAppConfig).Password = pass;

                if (generateNew)
                    auth = GetAuthFromApi(user);

                return string.IsNullOrEmpty(auth) ? GetAuthFromApi(user) : auth;
            }

            private static string GetAuthFromApi(AdWordsUser user)
            {
                string auth;
                try
                {
                    auth = new AuthToken(
                           (user.Config as AdWordsAppConfig),
                           AdWordsSoapClient.SERVICE_NAME,
                           (user.Config as AdWordsAppConfig).Email,
                           (user.Config as AdWordsAppConfig).Password).GetToken();

                    SaveAuthTokenToDB((user.Config as AdWordsAppConfig).Email, auth);
                }
                catch (Exception ex)
                {
                    //Log.Write("Error while trying to create new Auth key", ex);
                    throw new Exception("Error while trying to create new Auth key", ex);
                }
                return auth;

            }

            private static void SaveAuthTokenToDB(string mccEmail, string authToken)
            {
                SqlConnection connection;

                connection = new SqlConnection("Data Source=79.125.11.74; Database=Seperia;User ID=SeperiaServices;PWD=Asada2011!");

                try
                {
                    using (connection)
                    {
                        SqlCommand cmd = DataManager.CreateCommand(@"SetGoogleMccAuth(@MccEmail:Nvarchar,@AuthToken:Nvarchar)", System.Data.CommandType.StoredProcedure);
                        cmd.Connection = connection;
                        connection.Open();
                        cmd.Parameters["@MccEmail"].Value = mccEmail;
                        cmd.Parameters["@AuthToken"].Value = authToken;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while trying to set a new Auth key", ex);
                }

            }

            private static string GetAuthFromDB(string mccEmail, out string mccPassword)
            {
                string auth = "";
                mccPassword = "";
                SqlConnection connection;

                connection = new SqlConnection("Data Source=79.125.11.74; Database=Seperia;User ID=SeperiaServices;PWD=Asada2011!");
                try
                {
                    using (connection)
                    {
                        SqlCommand cmd = DataManager.CreateCommand(@"GetGoogleMccAuth(@MccEmail:Nvarchar)", System.Data.CommandType.StoredProcedure);
                        cmd.Connection = connection;
                        connection.Open();
                        cmd.Parameters["@MccEmail"].Value = mccEmail;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                mccPassword = Encryptor.Dec(reader[0].ToString());
                                auth = reader[1].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while trying to get auth key from DB", ex);
                }

                return auth;
            }

        }

        private void Clear_Click(object sender, EventArgs e)
        {
            this.AvailableReportFields.Clear();
            this.dataGridView.Rows.Clear();
            this.AWQL_textBox.Text = string.Empty;
            this.response.Text = string.Empty;
        }

        private void Download_Click(object sender, EventArgs e)
        {
            AdWordsAppConfig config = (AdWordsAppConfig)User.Config;
            
            string QUERY_REPORT_URL_FORMAT = "{0}/api/adwords/reportdownload/{1}?" + "__fmt={2}";

            string reportVersion = "v201302";
            string format = DownloadFormat.GZIPPED_CSV.ToString();
            string downloadUrl = string.Format(QUERY_REPORT_URL_FORMAT, config.AdWordsApiServer, reportVersion, format);
            string query = this.AWQL_textBox.Text;
            string postData = string.Format("__rdquery={0}", HttpUtility.UrlEncode(query));

            ClientReport retval = new ClientReport();

            FeedAttributeType f = new FeedAttributeType();
            

            ReportsException ex = null;
            this.response.Text += "\n Report download has started...";
            int maxPollingAttempts = 30 * 60 * 1000 / 30000;
            string path = this.path.Text;
            for (int i = 0; i < 3; i++)
            {
                this.response.Text += "\n Attempt #1...";

                try
                {
                    using (FileStream fs = File.OpenWrite(path))
                    {

                        fs.SetLength(0);
                        bool isSuccess = DownloadReportToStream(downloadUrl, config, true, fs, postData, User);
                        if (!isSuccess)
                        {
                            string errors = File.ReadAllText(path);

                        }
                    }
                }
                catch (Exception exception)
                {
                    this.response.Text = exception.Message;
                }
            }
            this.response.Text += "\n DONE !";
        }

     
        private bool DownloadReportToStream(string downloadUrl, AdWordsAppConfig config, bool returnMoneyInMicros, Stream outputStream, string postBody, AdWordsUser user)
        {
            this.response.Text += "\n Creating request...";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(downloadUrl);
            if (!string.IsNullOrEmpty(postBody))
            {
                request.Method = "POST";
            }
            request.Proxy = config.Proxy;
            request.Timeout = config.Timeout;
            request.UserAgent = config.GetUserAgent();

            if (!string.IsNullOrEmpty(config.ClientEmail))
            {
                request.Headers.Add("clientEmail: " + config.ClientEmail);
            }
            else if (!string.IsNullOrEmpty(config.ClientCustomerId))
            {
                request.Headers.Add("clientCustomerId: " + config.ClientCustomerId);
            }
            request.ContentType = "application/x-www-form-urlencoded";
            if (config.EnableGzipCompression)
            {
                (request as HttpWebRequest).AutomaticDecompression = DecompressionMethods.GZip
                    | DecompressionMethods.Deflate;
            }
            else
            {
                (request as HttpWebRequest).AutomaticDecompression = DecompressionMethods.None;
            }
            if (config.AuthorizationMethod == AdWordsAuthorizationMethod.OAuth2)
            {
                if (user.OAuthProvider != null)
                {
                    request.Headers["Authorization"] = user.OAuthProvider.GetAuthHeader(downloadUrl);
                }
                else
                {
                    //throw new AdWordsApiException(null, AdWordsErrorMessages.OAuthProviderCannotBeNull);
                }
            }
            else if (config.AuthorizationMethod == AdWordsAuthorizationMethod.ClientLogin)
            {
                string authToken = (!string.IsNullOrEmpty(config.AuthToken)) ? config.AuthToken :
                    new AuthToken(config, AdWordsSoapClient.SERVICE_NAME, config.Email,
                        config.Password).GetToken();
                request.Headers["Authorization"] = "GoogleLogin auth=" + authToken;
            }

            request.Headers.Add("returnMoneyInMicros: " + returnMoneyInMicros.ToString().ToLower());
            request.Headers.Add("developerToken: " + config.DeveloperToken);
            // The client library will use only apiMode = true.
            request.Headers.Add("apiMode", "true");

            if (!string.IsNullOrEmpty(postBody))
            {
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(postBody);
                }
            }

            // AdWords API now returns a 400 for an API error.
            bool retval = false;
            WebResponse response = null;
            try
            {
                this.response.Text += "\n Getting server response...";
                response = request.GetResponse();
                retval = true;
            }
            catch (WebException ex)
            {
                response = ex.Response;
                byte[] preview = ConvertStreamToByteArray(response.GetResponseStream(), MAX_ERROR_LENGTH);
                string previewString = ConvertPreviewBytesToString(preview);
                retval = false;
                this.webExceptionTextBox.Text = previewString;
            }
            MediaUtilities.CopyStream(response.GetResponseStream(), outputStream);
            response.Close();
            return retval;
        }
        private string ConvertPreviewBytesToString(byte[] previewBytes)
        {
            if (previewBytes == null)
            {
                return "";
            }

            // It is possible that our byte array doesn't end at a valid utf-8 string
            // boundary, so we use a progressive decoder to decode bytes as far as
            // possible.
            Decoder decoder = Encoding.UTF8.GetDecoder();
            char[] charArray = new char[previewBytes.Length];
            int bytesUsed;
            int charsUsed;
            bool completed;

            decoder.Convert(previewBytes, 0, previewBytes.Length, charArray, 0, charArray.Length, true,
                out bytesUsed, out charsUsed, out completed);
            return new string(charArray, 0, charsUsed);
        }
        private static byte[] ConvertStreamToByteArray(Stream sourceStream, int maxPreviewBytes)
        {
            if (sourceStream == null)
            {
                throw new ArgumentNullException("sourceStream");
            }

            int bufferSize = 2 << 20;
            byte[] buffer = new byte[bufferSize];
            List<Byte> byteArray = new List<byte>();

            int bytesRead = 0;
            while ((bytesRead = sourceStream.Read(buffer, 0, bufferSize)) != 0)
            {
                int index = 0;
                while (byteArray.Count < maxPreviewBytes && index < bytesRead)
                {
                    byteArray.Add(buffer[index]);
                    index++;
                }
            }
            return byteArray.ToArray();
        }

        private void get_Acc_Lables_Click(object sender, EventArgs e)
        {
            // Get the CampaignService.
            CampaignService campaignService =
                (CampaignService)User.GetService(AdWordsService.v201302.CampaignService);

            // Create the query.
            string query = "SELECT Id, Name, Status ORDER BY Name";

            int offset = 0;
            int pageSize = 500;

            CampaignPage page = new CampaignPage();
            List<Campaign> campaignsList = new List<Campaign>();
            try
            {
                do
                {
                    string queryWithPaging = string.Format("{0} LIMIT {1}, {2}", query, offset, pageSize);

                    // Get the campaigns.
                    page = campaignService.query(queryWithPaging);

                    // Display the results.
                    if (page != null && page.entries != null)
                    {
                        int i = offset;
                        foreach (Campaign campaign in page.entries)
                        {
                            campaignsList.Add(campaign);
                            Console.WriteLine("{0}) Campaign with id = '{1}', name = '{2}' and status = '{3}'" +
                              " was found.", i + 1, campaign.id, campaign.name, campaign.status);
                            i++;
                        }
                    }
                    offset += pageSize;
                } while (offset < page.totalNumEntries);
                Console.WriteLine("Number of campaigns found: {0}", page.totalNumEntries);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException("Failed to retrieve campaigns", ex);
            }
        }

        private void GetAccountHistory_Click(object sender, EventArgs e)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>()
						{
							{"DeveloperToken" ,this.DeveloperToken.Text},
							{"UserAgent" ,String.Format("Edge File Manager (version {0})", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())},
							{"EnableGzipCompression",this.EnableGzipCompression.Text},
							{"ClientCustomerId",this.ClientCustomerId.Text},
							{"Email",this.Email.Text}
						};


            User = new AdWordsUser(headers);
            try
            {
                //Getting AuthToken
                (User.Config as AdWordsAppConfig).AuthToken = AdwordsUtill.GetAuthToken(User);

                // Get the CustomerSyncService.
                CustomerSyncService customerSyncService =
                    (CustomerSyncService)User.GetService(AdWordsService.v201302.CustomerSyncService);

                // The date time string should be of the form  yyyyMMdd HHmmss zzz
                string minDateTime = DateTime.Now.AddDays(-1).ToUniversalTime().ToString("yyyyMMdd HHmmss")
                    + " UTC";
                string maxDateTime = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd HHmmss") + " UTC";

                // Create date time range.
                DateTimeRange dateTimeRange = new DateTimeRange();
                dateTimeRange.min = minDateTime;
                dateTimeRange.max = maxDateTime;

                // Create the selector.
                CustomerSyncSelector selector = new CustomerSyncSelector();
                selector.dateTimeRange = dateTimeRange;
                selector.campaignIds = GetAllCampaignIds(User);

                // Get all account changes for campaign.
                CustomerChangeData accountChanges = customerSyncService.get(selector);

                // Display the changes.
                if (accountChanges != null && accountChanges.changedCampaigns != null)
                {
                    Console.WriteLine("Displaying changes up to: {0}", accountChanges.lastChangeTimestamp);
                    foreach (CampaignChangeData campaignChanges in accountChanges.changedCampaigns)
                    {
                        Console.WriteLine("Campaign with id \"{0}\" was changed:", campaignChanges.campaignId);
                        Console.WriteLine("  Campaign changed status: {0}",
                            campaignChanges.campaignChangeStatus);
                        if (campaignChanges.campaignChangeStatus != ChangeStatus.NEW)
                        {
                            Console.WriteLine("  Added ad extensions: {0}", GetFormattedList(
                                campaignChanges.addedAdExtensions));
                            Console.WriteLine("  Added campaign criteria: {0}",
                                GetFormattedList(campaignChanges.addedCampaignCriteria));
                            Console.WriteLine("  Added campaign targeting: {0}",
                                campaignChanges.campaignTargetingChanged ? "yes" : "no");
                            Console.WriteLine("  Deleted ad extensions: {0}",
                                GetFormattedList(campaignChanges.deletedAdExtensions));
                            Console.WriteLine("  Deleted campaign criteria: {0}",
                                GetFormattedList(campaignChanges.deletedCampaignCriteria));

                            if (campaignChanges.changedAdGroups != null)
                            {
                                foreach (AdGroupChangeData adGroupChanges in campaignChanges.changedAdGroups)
                                {
                                    Console.WriteLine("  Ad group with id \"{0}\" was changed:",
                                        adGroupChanges.adGroupId);
                                    Console.WriteLine("    Ad group changed status: {0}",
                                        adGroupChanges.adGroupChangeStatus);
                                    if (adGroupChanges.adGroupChangeStatus != ChangeStatus.NEW)
                                    {
                                        Console.WriteLine("    Ads changed: {0}",
                                            GetFormattedList(adGroupChanges.changedAds));
                                        Console.WriteLine("    Criteria changed: {0}",
                                            GetFormattedList(adGroupChanges.changedCriteria));
                                        Console.WriteLine("    Criteria deleted: {0}",
                                            GetFormattedList(adGroupChanges.deletedCriteria));
                                    }
                                }
                            }
                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No account changes were found."); ;
                }
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException("Failed to get account changes.", ex);
            }
        }


        /// <summary>
        /// Gets all campaign ids in the account.
        /// </summary>
        /// <param name="user">The user for which campaigns are retrieved.</param>
        /// <returns>The list of campaign ids.</returns>
        private long[] GetAllCampaignIds(AdWordsUser user)
        {
            // Get the CampaignService.
            CampaignService campaignService =
                (CampaignService)user.GetService(AdWordsService.v201302.CampaignService);

            List<long> allCampaigns = new List<long>();

            // Create the selector.
            Selector selector = new Selector();
            selector.fields = new string[] { "Id" };

            // Get all campaigns.
            CampaignPage page = campaignService.get(selector);

            // Return the results.
            if (page != null && page.entries != null)
            {
                foreach (Campaign campaign in page.entries)
                {
                    allCampaigns.Add(campaign.id);
                }
            }
            return allCampaigns.ToArray();
        }
        private string GetFormattedList(long[] ids)
        {
            StringBuilder builder = new StringBuilder();
            if (ids != null)
            {
                foreach (long id in ids)
                {
                    builder.AppendFormat("{0}, ", id);
                }
            }
            return "[" + builder.ToString().TrimEnd(',', ' ') + "]";
        }

        private void GetAccountHierarchy_Click(object sender, EventArgs e)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>()
						{
							{"DeveloperToken" ,this.DeveloperToken.Text},
							{"UserAgent" ,String.Format("Edge File Manager (version {0})", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())},
							{"EnableGzipCompression",this.EnableGzipCompression.Text},
							{"ClientCustomerId",this.ClientCustomerId.Text},
							{"Email",this.Email.Text}
						};


            User = new AdWordsUser(headers);

            try
            {
                //Getting AuthToken
                (User.Config as AdWordsAppConfig).AuthToken = AdwordsUtill.GetAuthToken(User);
            }
            catch (Exception exc)
            {
                this.rchtxt.Text = exc.Message + " #### " + exc.InnerException != null? exc.InnerException.Message:string.Empty;
            }

            // Get the ManagedCustomerService.
            ManagedCustomerService managedCustomerService = (ManagedCustomerService)User.GetService(
                AdWordsService.v201302.ManagedCustomerService);
            managedCustomerService.RequestHeader.clientCustomerId = null;

            // Create selector.
            Selector selector = new Selector();
            selector.fields = new String[] { "Login", "CustomerId", "Name" };

            try
            {
                // Get results.
                ManagedCustomerPage page = managedCustomerService.get(selector);

                // Display serviced account graph.
                if (page.entries != null)
                {
                    // Create map from customerId to customer node.
                    Dictionary<long, ManagedCustomerTreeNode> customerIdToCustomerNode = new Dictionary<long, ManagedCustomerTreeNode>();

                    // Create account tree nodes for each customer.
                    foreach (ManagedCustomer customer in page.entries)
                    {
                        ManagedCustomerTreeNode node = new ManagedCustomerTreeNode();
                        node.Account = customer;
                        customerIdToCustomerNode.Add(customer.customerId, node);
                    }

                    // For each link, connect nodes in tree.
                    if (page.links != null)
                    {
                        foreach (ManagedCustomerLink link in page.links)
                        {
                            ManagedCustomerTreeNode managerNode =
                                customerIdToCustomerNode[link.managerCustomerId];
                            ManagedCustomerTreeNode childNode = customerIdToCustomerNode[link.clientCustomerId];
                            childNode.ParentNode = managerNode;
                            if (managerNode != null)
                            {
                                managerNode.ChildAccounts.Add(childNode);
                            }
                        }
                    }

                    // Find the root account node in the tree.
                    ManagedCustomerTreeNode rootNode = null;
                    foreach (ManagedCustomer account in page.entries)
                    {
                        if (customerIdToCustomerNode[account.customerId].ParentNode == null)
                        {
                            rootNode = customerIdToCustomerNode[account.customerId];
                            break;
                        }
                    }

                    // Display account tree.
                    rchtxt.AppendText("Login, CustomerId, Name");
                    rchtxt.AppendText(rootNode.ToTreeString(0, new StringBuilder()));
                   // Console.WriteLine("Login, CustomerId, Name");
                   // Console.WriteLine(rootNode.ToTreeString(0, new StringBuilder()));
                }
                else
                {
                    Console.WriteLine("No serviced accounts were found.");
                }
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException("Failed to create ad groups.", ex);
            }
        }

        private void GetAwql_Click(object sender, EventArgs e)
        {
             
            if (this.dataGridView.SelectedRows.Count != 0)
            {
                this.AvailableReportFields.Clear();
                sb = new StringBuilder();
                sb.Append("SELECT ");

                //Get selected fields
                foreach (DataGridViewRow row in this.dataGridView.SelectedRows)
                {
                    sb.Append(row.Cells["fieldName"].Value);
                    sb.Append(",");
                    this.AvailableReportFields.AppendText(string.Format(@"""{0}"",", row.Cells["fieldName"].Value));
                }

                sb.Remove(sb.Length - 1, 1); // removing last ","
                sb.Append(" FROM " + ReportNamesListBox.SelectedItem.ToString());
                sb.Append(" DURING YESTERDAY ");

                this.AWQL_textBox.Text = sb.ToString();
                path.Text = "C:\\" + ClientCustomerId.Text + "_" + ReportNamesListBox.SelectedItem.ToString() + "_" + DateTime.Now.ToString("ddssFFFFFFF") + ".Gzip";
            }
            else this.AWQL_textBox.Text = "No rows have been selected";
        }

        private void GetCampaigns_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
						{
							{"DeveloperToken" ,this.DeveloperToken.Text},
							{"UserAgent" ,String.Format("Edge File Manager (version {0})", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())},
							{"EnableGzipCompression",this.EnableGzipCompression.Text},
							{"ClientCustomerId",this.ClientCustomerId.Text},
							{"Email",this.Email.Text}
						};


            User = new AdWordsUser(headers);

            try
            {
                //Getting AuthToken
                (User.Config as AdWordsAppConfig).AuthToken = AdwordsUtill.GetAuthToken(User);
            }
            catch (Exception exc)
            {
                this.rchtxt.Text = exc.Message + " #### " + exc.InnerException != null ? exc.InnerException.Message : string.Empty;
            }

            
            // Get the CampaignService.
            CampaignService campaignService =
                (CampaignService)User.GetService(AdWordsService.v201302.CampaignService);
            
 

            // Create the query.
            string query = "SELECT Id, Name, Status,Settings ORDER BY Name";

            int offset = 0;
            int pageSize = 500;

            CampaignPage page = new CampaignPage();

            try
            {
                do
                {
                    string queryWithPaging = string.Format("{0} LIMIT {1}, {2}", query, offset, pageSize);

                    // Get the campaigns.
                    page = campaignService.query(queryWithPaging);

                    // Display the results.
                    if (page != null && page.entries != null)
                    {
                        int i = offset;
                        foreach (Campaign campaign in page.entries)
                        {
                            this.rchtxt.AppendText(string.Format("/n Campaign id = '{0}', name = '{1}' ,status = '{2}'" + " was found.", campaign.id, campaign.name, campaign.status));
                            
                           //  i++;
                        }
                    }
                    offset += pageSize;
                } while (offset < page.totalNumEntries);
                Console.WriteLine("Number of campaigns found: {0}", page.totalNumEntries);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException("Failed to retrieve campaigns", ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
						{
							{"DeveloperToken" ,this.DeveloperToken.Text},
							{"UserAgent" ,String.Format("Edge File Manager (version {0})", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())},
							{"EnableGzipCompression",this.EnableGzipCompression.Text},
							{"ClientCustomerId",this.ClientCustomerId.Text},
							{"Email",this.Email.Text}
						};


            User = new AdWordsUser(headers);

            try
            {
                //Getting AuthToken
                (User.Config as AdWordsAppConfig).AuthToken = AdwordsUtill.GetAuthToken(User);
            }
            catch (Exception exc)
            {
                this.rchtxt.Text = exc.Message + " #### " + exc.InnerException != null ? exc.InnerException.Message : string.Empty;
            }

            CampaignCriterionService campaignCriterionService = (CampaignCriterionService)User.GetService(AdWordsService.v201302.CampaignCriterionService);

            // Create the selector.
            Selector selector = new Selector();
            selector.fields = new string[] { "Id", "CriteriaType", "CampaignId" };

            // Set the filters.
          //  Predicate predicate = new Predicate();
          //  predicate.field = "CampaignId";
           // predicate.@operator = PredicateOperator.EQUALS;
           // predicate.values = new string[] { campaignId.ToString() };

          //  selector.predicates = new Predicate[] { predicate };

            // Set the selector paging.
            selector.paging = new Paging();

            int offset = 0;
            int pageSize = 500;

            CampaignCriterionPage page = new CampaignCriterionPage();

            try
            {
                do
                {
                    selector.paging.startIndex = offset;
                    selector.paging.numberResults = pageSize;

                    // Get all campaign targets.
                    page = campaignCriterionService.get(selector);

                    // Display the results.
                    if (page != null && page.entries != null)
                    {
                        int i = offset;
                        foreach (CampaignCriterion campaignCriterion in page.entries)
                        {
                            string negative = (campaignCriterion is NegativeCampaignCriterion) ? "Negative " : "";
                            Console.WriteLine("{0}) {1}Campaign criterion with id = '{2}' and Type = {3} was " +
                                " found for campaign id '{4}'", i, negative, campaignCriterion.criterion.id,
                                campaignCriterion.criterion.type, campaignCriterion.campaignId);
                            i++;
                        }
                    }
                    offset += pageSize;
                } while (offset < page.totalNumEntries);
                Console.WriteLine("Number of campaign targeting criteria found: {0}", page.totalNumEntries);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException("Failed to get campaign targeting criteria.", ex);
            }





        }

        private void GetAgSettings_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
						{
							{"DeveloperToken" ,this.DeveloperToken.Text},
							{"UserAgent" ,String.Format("Edge File Manager (version {0})", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())},
							{"EnableGzipCompression",this.EnableGzipCompression.Text},
							{"ClientCustomerId",this.ClientCustomerId.Text},
							{"Email",this.Email.Text}
						};


            User = new AdWordsUser(headers);

            try
            {
                //Getting AuthToken
                (User.Config as AdWordsAppConfig).AuthToken = AdwordsUtill.GetAuthToken(User);
            }
            catch (Exception exc)
            {
                this.rchtxt.Text = exc.Message + " #### " + exc.InnerException != null ? exc.InnerException.Message : string.Empty;
            }

            AdGroupCriterionService agCriterionService = (AdGroupCriterionService)User.GetService(AdWordsService.v201302.AdGroupCriterionService);

            // Create the selector.
            Selector selector = new Selector();
            selector.fields = new string[] { "Id", "AdGroupId","Status" };

            // Set the filters.
            //  Predicate predicate = new Predicate();
            //  predicate.field = "CampaignId";
            // predicate.@operator = PredicateOperator.EQUALS;
            // predicate.values = new string[] { campaignId.ToString() };

            //  selector.predicates = new Predicate[] { predicate };

            // Set the selector paging.
            selector.paging = new Paging();

            int offset = 0;
            int pageSize = 500;

            AdGroupCriterionPage page = new AdGroupCriterionPage();

            try
            {
                do
                {
                    selector.paging.startIndex = offset;
                    selector.paging.numberResults = pageSize;

                    // Get all campaign targets.
                    page = agCriterionService.get(selector);

                    // Display the results.
                    if (page != null && page.entries != null)
                    {
                        int i = offset;
                        foreach (AdGroupCriterion adGroupCriterion in page.entries)
                        {
                            i++;
                        }
                    }
                    offset += pageSize;
                } while (offset < page.totalNumEntries);
                Console.WriteLine("Number of ad groups criteria found: {0}", page.totalNumEntries);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException("Failed to get adgroup targeting criteria.", ex);
            }
        }

        private void GetAgSettings2_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
						{
							{"DeveloperToken" ,this.DeveloperToken.Text},
							{"UserAgent" ,String.Format("Edge File Manager (version {0})", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())},
							{"EnableGzipCompression",this.EnableGzipCompression.Text},
							{"ClientCustomerId",this.ClientCustomerId.Text},
							{"Email",this.Email.Text}
						};


            User = new AdWordsUser(headers);

            try
            {
                //Getting AuthToken
                (User.Config as AdWordsAppConfig).AuthToken = AdwordsUtill.GetAuthToken(User);
            }
            catch (Exception exc)
            {
                this.rchtxt.Text = exc.Message + " #### " + exc.InnerException != null ? exc.InnerException.Message : string.Empty;
            }

            AdGroupService agService = (AdGroupService)User.GetService(AdWordsService.v201302.AdGroupService);
            ConstantDataService constData = (ConstantDataService)User.GetService(AdWordsService.v201302.ConstantDataService);

            Language[] lang =  constData.getLanguageCriterion();

            // Create the selector.
            Selector selector = new Selector();
            selector.fields = new string[] { "Id", "Status", "Clicks" };

            // Set the filters.
            //  Predicate predicate = new Predicate();
            //  predicate.field = "CampaignId";
            // predicate.@operator = PredicateOperator.EQUALS;
            // predicate.values = new string[] { campaignId.ToString() };

            //  selector.predicates = new Predicate[] { predicate };

            // Set the selector paging.
            selector.paging = new Paging();

            int offset = 0;
            int pageSize = 500;

            AdGroupPage page = new AdGroupPage();

            try
            {
                do
                {
                    selector.paging.startIndex = offset;
                    selector.paging.numberResults = pageSize;

                    // Get all campaign targets.
                    page = agService.get(selector);

                    // Display the results.
                    if (page != null && page.entries != null)
                    {
                        int i = offset;
                        foreach (AdGroup adGroup in page.entries)
                        {
                            i++;
                        }
                    }
                    offset += pageSize;
                } while (offset < page.totalNumEntries);
                Console.WriteLine("Number of ad groups criteria found: {0}", page.totalNumEntries);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException("Failed to get adgroup targeting criteria.", ex);
            }
        }

        private void GetTargetingIdea_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
						{
							{"DeveloperToken" ,this.DeveloperToken.Text},
							{"UserAgent" ,String.Format("Edge File Manager (version {0})", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())},
							{"EnableGzipCompression",this.EnableGzipCompression.Text},
							{"ClientCustomerId",this.ClientCustomerId.Text},
							{"Email",this.Email.Text}
						};


            User = new AdWordsUser(headers);

            try
            {
                //Getting AuthToken
                (User.Config as AdWordsAppConfig).AuthToken = AdwordsUtill.GetAuthToken(User);
            }
            catch (Exception exc)
            {
                this.rchtxt.Text = exc.Message + " #### " + exc.InnerException != null ? exc.InnerException.Message : string.Empty;
            }

            TargetingIdeaService targetingService = (TargetingIdeaService)User.GetService(AdWordsService.v201302.TargetingIdeaService);

            // Create the selector.
            TargetingIdeaSelector selector = new TargetingIdeaSelector();
            selector.currencyCode = "USD";


           // selector.fields = new string[] { "Id", "Status", "Clicks" };

            // Set the filters.
            //  Predicate predicate = new Predicate();
            //  predicate.field = "CampaignId";
            // predicate.@operator = PredicateOperator.EQUALS;
            // predicate.values = new string[] { campaignId.ToString() };

            //  selector.predicates = new Predicate[] { predicate };

            // Set the selector paging.
            selector.paging = new Paging();

            int offset = 0;
            int pageSize = 500;

            TargetingIdeaPage page = new TargetingIdeaPage();

            try
            {
                do
                {
                    selector.paging.startIndex = offset;
                    selector.paging.numberResults = pageSize;

                    // Get all campaign targets.
                    page = targetingService.get(selector);

                    // Display the results.
                    if (page != null && page.entries != null)
                    {
                        int i = offset;
                        foreach (TargetingIdea target in page.entries)
                        {
                            i++;
                        }
                    }
                    offset += pageSize;
                } while (offset < page.totalNumEntries);
                Console.WriteLine("Number of ad groups criteria found: {0}", page.totalNumEntries);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException("Failed to get adgroup targeting criteria.", ex);
            }
        }
    }

    class ManagedCustomerTreeNode
    {
        /// <summary>
        /// The parent node.
        /// </summary>
        private ManagedCustomerTreeNode parentNode;

        /// <summary>
        /// The account associated with this node.
        /// </summary>
        private ManagedCustomer account;

        /// <summary>
        /// The list of child accounts.
        /// </summary>
        private List<ManagedCustomerTreeNode> childAccounts = new List<ManagedCustomerTreeNode>();

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        public ManagedCustomerTreeNode ParentNode
        {
            get { return parentNode; }
            set { parentNode = value; }
        }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        public ManagedCustomer Account
        {
            get { return account; }
            set { account = value; }
        }

        /// <summary>
        /// Gets the child accounts.
        /// </summary>
        public List<ManagedCustomerTreeNode> ChildAccounts
        {
            get { return childAccounts; }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override String ToString()
        {
            String login = String.IsNullOrEmpty(account.login) ? "(no login)" : account.login;
            return String.Format("{0}, {1}, {2}", login, account.customerId, account.name);
        }

        /// <summary>
        /// Returns a string representation of the current level of the tree and
        /// recursively returns the string representation of the levels below it.
        /// </summary>
        /// <param name="depth">The depth of the node.</param>
        /// <param name="sb">The String Builder containing the tree
        /// representation.</param>
        /// <returns>The tree string representation.</returns>
        public string ToTreeString(int depth, StringBuilder sb)
        {
            sb.Append(new String('-', depth * 2));
            sb.Append(this);
            sb.Append("\n");
            foreach (ManagedCustomerTreeNode childAccount in childAccounts)
            {
                childAccount.ToTreeString(depth + 1, sb);
            }
            return sb.ToString();
        }
    }
}