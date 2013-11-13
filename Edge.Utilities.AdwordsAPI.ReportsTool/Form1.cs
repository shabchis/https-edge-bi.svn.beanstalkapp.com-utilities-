using Edge.Core.Data;
using Edge.Core.Utilities;
using Google.Api.Ads.AdWords.Lib;
using Google.Api.Ads.AdWords.Util.Reports;
using Google.Api.Ads.AdWords.v201309;
using Google.Api.Ads.Common.Lib;
using Google.Api.Ads.Common.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
        private static AdWordsUser GetAdwordsUser(bool useOauth, string DeveloperToken, string EnableGzipCompression, string ClientCustomerId, string Email, string OAuth2ClientId = null)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
						{
							{"DeveloperToken" ,DeveloperToken},
							{"UserAgent" ,String.Format("Edge File Manager (version {0})", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())},
							{"EnableGzipCompression",EnableGzipCompression},
							{"ClientCustomerId",ClientCustomerId},
							{"Email",Email}
						};

            if (useOauth)
            {
                AdWordsUser user = new AdWordsUser(headers);
                SetOAuthParams(user, OAuth2ClientId);
                return user;

            }
            return new AdWordsUser(headers);

        }

        private static void SetOAuthParams(AdWordsUser user, string OAuth2ClientId)
        {
            //Get Oauth params from DB
            GetOAuthDetailsFromDB(OAuth2ClientId, user);
        }

        private static void GetOAuthDetailsFromDB(string OAuth2ClientId, AdWordsUser user)
        {

            SqlConnection connection = new SqlConnection("Data Source=79.125.11.74; Database=Seperia;User ID=SeperiaServices;PWD=Asada2011!");
            try
            {
                using (connection)
                {
                    SqlCommand cmd = DataManager.CreateCommand(@"Google_GetAuthDetails(@ClientID:Nvarchar)", System.Data.CommandType.StoredProcedure);
                    cmd.Connection = connection;
                    connection.Open();
                    cmd.Parameters["@ClientID"].Value = OAuth2ClientId;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Config.OAuth2ClientId = OAuth2ClientId;
                            user.Config.OAuth2ClientSecret = reader[0].ToString();
                            user.Config.OAuth2Mode = OAuth2Flow.APPLICATION;
                            user.Config.OAuth2RefreshToken = reader[4].ToString();
                            user.Config.OAuth2RedirectUri = reader[7].ToString();
                            user.Config.OAuth2AccessToken = reader[3].ToString();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while trying to get auth key from DB", ex);
            }

        }

        private void GetReportFields_Click(object sender, EventArgs e)
        {
            //Dictionary<string, string> headers = new Dictionary<string, string>()
            //            {
            //                {"DeveloperToken" ,this.DeveloperToken.Text},
            //                {"UserAgent" ,String.Format("Edge File Manager (version {0})", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())},
            //                {"EnableGzipCompression",this.EnableGzipCompression.Text},
            //                {"ClientCustomerId",this.ClientCustomerId.Text},
            //                {"Email",this.Email.Text}
            //            };


            User = GetAdwordsUser(this.useOauth2.Checked, this.DeveloperToken.Text, this.EnableGzipCompression.Text, this.ClientCustomerId.Text, this.Email.Text, this.OAuth2ClientId.Text);
            try
            {
                //Getting AuthToken
                if (!this.useOauth2.Checked)
                    (User.Config as AdWordsAppConfig).AuthToken = AdwordsUtill.GetAuthToken(User);

                ReportDefinitionReportType reportType = (ReportDefinitionReportType)Enum.Parse(typeof(ReportDefinitionReportType), ReportNamesListBox.SelectedItem.ToString());
                ReportDefinitionService reportDefinitionService = (ReportDefinitionService)User.GetService(AdWordsService.v201309.ReportDefinitionService);

                // Get the report fields.
                ReportDefinitionField[] reportDefinitionFields = reportDefinitionService.getReportFields(reportType);
                foreach (ReportDefinitionField reportDefinitionField in reportDefinitionFields)
                {
                    this.AvailableReportFields.AppendText(string.Format(@"""{0}"",", reportDefinitionField.fieldName));
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
            string QUERY_REPORT_URL_FORMAT = "{0}/api/adwords/reportdownload/{1}?" + "__fmt={2}";
            string reportVersion = "v201309";
            string format = DownloadFormat.GZIPPED_CSV.ToString();
            string downloadUrl = string.Format(QUERY_REPORT_URL_FORMAT,((AdWordsAppConfig) User.Config).AdWordsApiServer, reportVersion, format);
            string query = this.AWQL_textBox.Text;
            string postData = string.Format("__rdquery={0}", HttpUtility.UrlEncode(query));

            ReportUtilities utilities = new ReportUtilities(User);
            utilities.ReportVersion = "v201309";
            utilities.DownloadClientReport(query, DownloadFormat.GZIPPED_CSV.ToString(), this.path.Text);


            //ClientReport retval = new ClientReport();

            //FeedAttributeType f = new FeedAttributeType();


            //ReportsException ex = null;
            //this.response.Text += "\n Report download has started...";
            //int maxPollingAttempts = 30 * 60 * 1000 / 30000;
            //string path = this.path.Text;
            //for (int i = 0; i < 3; i++)
            //{
            //    this.response.Text += "\n Attempt #1...";

            //    try
            //    {
            //        using (FileStream fs = File.OpenWrite(path))
            //        {

            //            fs.SetLength(0);
            //            bool isSuccess = DownloadReportToStream(downloadUrl, config, true, fs, postData, User);
            //            if (!isSuccess)
            //            {
            //                string errors = File.ReadAllText(path);

            //            }
            //        }
            //    }
            //    catch (Exception exception)
            //    {
            //        this.response.Text = exception.Message;
            //    }
            //}
            this.response.Text += "\n DONE !";
        }

        /*
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

            //if (!string.IsNullOrEmpty(config.ClientEmail))
            //{
            //    request.Headers.Add("clientEmail: " + config.ClientEmail);
            //}
            if (!string.IsNullOrEmpty(config.ClientCustomerId))
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
        */
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
                this.rchtxt.Text = exc.Message + " #### " + exc.InnerException != null ? exc.InnerException.Message : string.Empty;
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
            selector.fields = new string[] { "Id", "AdGroupId", "Status" };

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

            Language[] lang = constData.getLanguageCriterion();

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

        private void GetAccountAlerts_Click(object sender, EventArgs e)
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

            // Get the AlertService.
            AlertService alertService = (AlertService)User.GetService(
                AdWordsService.v201302.AlertService);

            // Create the selector.
            AlertSelector selector = new AlertSelector();

            // Create the alert query.
            AlertQuery query = new AlertQuery();
            query.filterSpec = FilterSpec.ALL;
            query.clientSpec = ClientSpec.ALL;
            query.triggerTimeSpec = TriggerTimeSpec.ALL_TIME;
            query.severities = new AlertSeverity[] {AlertSeverity.GREEN, AlertSeverity.YELLOW,
          AlertSeverity.RED};

            // Enter all possible values of AlertType to get all alerts. If you are
            // interested only in specific alert types, then you may also do it as
            // follows:
            // query.types = new AlertType[] {AlertType.CAMPAIGN_ENDING,
            //     AlertType.CAMPAIGN_ENDED};
            query.types = (AlertType[])Enum.GetValues(typeof(AlertType));
            selector.query = query;

            // Set paging for selector.
            selector.paging = new Paging();

            int offset = 0;
            int pageSize = 500;

            AlertPage page = new AlertPage();

            try
            {
                do
                {
                    // Get account alerts.
                    selector.paging.startIndex = offset;
                    selector.paging.numberResults = pageSize;

                    page = alertService.get(selector);

                    // Display the results.
                    if (page != null && page.entries != null)
                    {
                        int i = offset;
                        foreach (Alert alert in page.entries)
                        {
                            Console.WriteLine("{0}) Customer Id is {1:###-###-####}, Alert type is '{2}', " +
                                "Severity is {3}", i + 1, alert.clientCustomerId, alert.alertType,
                                alert.alertSeverity);
                            for (int j = 0; j < alert.details.Length; j++)
                            {
                                Console.WriteLine("  - Triggered at {0}", alert.details[j].triggerTime);
                            }
                            i++;
                        }
                    }
                    offset += pageSize;
                } while (offset < page.totalNumEntries);
                Console.WriteLine("Number of alerts found: {0}", page.totalNumEntries);
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException("Failed to retrieve alerts.", ex);
            }
        }

        private void CreateConnection_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
						{
							{"DeveloperToken" ,this.DeveloperToken.Text},
							{"UserAgent" ,String.Format("Edge File Manager (version {0})", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())},
							{"EnableGzipCompression",this.EnableGzipCompression.Text},
							{"ClientCustomerId",this.ClientCustomerId.Text},
							{"Email",this.Email.Text}
						};
            AdWordsUser user = new AdWordsUser(headers);
            user.Config.OAuth2ClientId = this.OAuth2ClientId.Text;
            user.Config.OAuth2ClientSecret = this.OAuth2ClientSecret.Text;
            user.Config.OAuth2Mode = OAuth2Flow.APPLICATION;
            (user.Config as AdWordsAppConfig).AuthorizationMethod = AdWordsAuthorizationMethod.OAuth2;


            AdWordsAppConfig config = (user.Config as AdWordsAppConfig);
            if (config.AuthorizationMethod == AdWordsAuthorizationMethod.OAuth2)
            {
                if (config.OAuth2Mode == OAuth2Flow.APPLICATION &&
                    string.IsNullOrEmpty(config.OAuth2RefreshToken))
                {
                    DoAuth2Authorization(user);
                }
            }
        }

        /// <summary>
        /// Does the OAuth2 authorization for installed applications.
        /// </summary>
        /// <param name="user">The AdWords user.</param>
        private static void DoAuth2Authorization(AdWordsUser user)
        {
            // Since we are using a console application, set the callback url to null.
            user.Config.OAuth2RedirectUri = null;
            AdsOAuthProviderForApplications oAuth2Provider =
                (user.OAuthProvider as AdsOAuthProviderForApplications);
            // Get the authorization url.
            string authorizationUrl = oAuth2Provider.GetAuthorizationUrl();
            Console.WriteLine("Open a fresh web browser and navigate to \n\n{0}\n\n. You will be " +
                "prompted to login and then authorize this application to make calls to the " +
                "AdWords API. Once approved, you will be presented with an authorization code.",
                authorizationUrl);

            // Accept the OAuth2 authorization code from the user.
            Console.Write("Enter the authorization code :");
            string authorizationCode = Console.ReadLine();

            // Fetch the access and refresh tokens.
            oAuth2Provider.FetchAccessAndRefreshTokens(authorizationCode);
        }

        private void GetOAuthInfoFromDB_Click(object sender, EventArgs e)
        {
            this.User = GetAdwordsUser(this.useOauth2.Checked, this.DeveloperToken.Text, this.EnableGzipCompression.Text, this.ClientCustomerId.Text, this.Email.Text, this.OAuth2ClientId.Text);
            this.OAuth2RedirectUri.Text = User.Config.OAuth2RedirectUri;
            this.OAuth2ClientSecret.Text = User.Config.OAuth2ClientSecret;
            this.OAuth2RefreshToken.Text = User.Config.OAuth2RefreshToken;
        }

        private void RegexTester_Click(object sender, EventArgs e)
        {
            this.RegexRes.Items.Clear();

            Regex regex = null;
            Regex _fixRegex = new Regex(@"\(\?\{(\w+)\}");
            string _fixReplace = @"(?<$1>";
            string[] RawGroupNames = null;
            Regex _varName = new Regex("^[A-Za-z_][A-Za-z0-9_]*$");
            string[] _fragments = null;

            if (!String.IsNullOrWhiteSpace(this.ConfigRegex.Text))
            {
                try
                {
                    regex = new Regex(_fixRegex.Replace(this.ConfigRegex.Text, _fixReplace), RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

                    // skip the '0' group which is always first, the asshole
                    string[] groupNames = regex.GetGroupNames();
                    RawGroupNames = groupNames.Length > 0 ? groupNames.Skip(1).ToArray() : groupNames;

                    List<string> frags = new List<string>();
                    foreach (string frag in RawGroupNames)
                    {
                        if (_varName.IsMatch(frag))
                            frags.Add(frag);
                        else
                            throw new Exception();
                        //throw new MappingConfigurationException(String.Format("'{0}' is not a valid read command fragment name. Use C# variable naming rules.", frag));
                    }
                    _fragments = frags.ToArray();

                    Match m = regex.Match(this.destUrl.Text);
                    if (m.Success)
                    {
                        foreach (string fragment in _fragments)
                        {
                            Group g = m.Groups[fragment];
                            if (g.Success)
                            {
                                this.RegexRes.Items.Add(fragment + " = " + g.Value);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error", ex.Message, MessageBoxButtons.OK);
                }
            }
        }

        private void CreateURL_Click(object sender, EventArgs e)
        {
            
            HttpListener newHttpListener = new System.Net.HttpListener();
            newHttpListener.Prefixes.Add(this.OAuth2RedirectUri.Text);
            try
            {
                newHttpListener.Start();
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine("Looks like {0} is blocked. Please open a command line prompt as " +
                    "Administrator and run the following command: \n" +
                    "netsh http add urlacl url={0} user=machine\\username\n" +
                    "where machine\\username is your user account.\nThen re-run this application.",
                    this.OAuth2RedirectUri.Text);
                return;
            }

           // Console.WriteLine(USAGE, LOCALHOST_ADDRESS);
            // Create an app configuration object.

            SimpleAppConfig appConfig = new SimpleAppConfig();
            appConfig.OAuth2RedirectUri = this.OAuth2RedirectUri.Text;

            // Read the client ID, secret and scope from the user.
            appConfig.OAuth2Scope = this.AuthScope.Text;
            appConfig.OAuth2ClientId = this.OAuth2ClientId.Text;
            appConfig.OAuth2ClientSecret = this.OAuth2ClientSecret.Text;

            // Create the OAuth2 protocol handler and set it to the current user.
            OAuth2ProviderForApplications oAuth2 = new OAuth2ProviderForApplications(appConfig);

            // Get the authorization url and open a browser.
            string authorizationUrl = oAuth2.GetAuthorizationUrl();
            Process.Start(authorizationUrl);

           // string authorizationCode = Prompt.ShowDialog("Enter Authorization Code", "Fetch Access And Refresh Tokens");
            
            // Fetch the access and refresh tokens.
            //oAuth2.FetchAccessAndRefreshTokens(authorizationCode);

            //Console.WriteLine(APP_CONFIG_PATCH, appConfig.OAuth2ClientId, appConfig.OAuth2ClientSecret,
            //    oAuth2.RefreshToken);
            //Console.ReadLine();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

    }
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 150;
            prompt.Text = caption;
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70 };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.ShowDialog();
            return textBox.Text;
        }
    }
    class SimpleAppConfig : AppConfigBase { }
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