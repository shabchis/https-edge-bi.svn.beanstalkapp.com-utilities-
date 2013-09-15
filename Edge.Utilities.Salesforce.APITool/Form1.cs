
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Edge.Utilities.Salesforce.APITool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.OAuthUrl.Text = string.Format("{0}/services/oauth2/authorize?response_type=code&client_id={1}&redirect_uri={2}", this.loginUrl.Text, this.consumerKey.Text, this.RedirectUrl.Text);

        }

        private void Download_Click(object sender, EventArgs e)
        {
            if (RequierdFieldsValidate())
            {
                Token token = Token.Get(this.consumerKey.Text, this.connectionString.Text);
                //if not exist
                if (string.IsNullOrEmpty(token.access_token) || (string.IsNullOrEmpty(token.refresh_token)))
                    token = GetAccessTokenParamsFromSalesForce();

                ////check if access_token is not expired
                if (token.UpdateTime.Add((TimeSpan.Parse("02:00:00"))) < DateTime.Now)
                    token = RefreshToken(token.refresh_token);


                string sourceUrl = string.Format("{0}/services/data/v20.0/query?q={1}", token.instance_url, this.soql.Text);

                //creating download request
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sourceUrl);
                request.Headers.Add("Authorization: OAuth " + token.access_token);
               // request.Method = "POST";

                //Getting response
                WebResponse response;

                try { response = request.GetResponse(); }
                catch (Exception ex)
                {
                    
                    this.response.AppendText(ex.Message);
                   
                    WebException webex;
                    if (ex is WebException && (webex = (WebException)ex).Status == WebExceptionStatus.ProtocolError)
                    {
                        response = webex.Response;
                    }
                    else
                        return;
                }

                string bufferSizeSt = "20"; // from configuration example.
                Stream stream = response.GetResponseStream();
                using (FileStream outputStream = File.Create(this.path.Text))
                {
                    
                    using (stream)
                    {
                        int bufferSize = 2 << int.Parse(bufferSizeSt);
                        byte[] buffer = new byte[bufferSize];

                        int bytesRead = 0;
                        int totalBytesRead = 0;
                        while ((bytesRead = stream.Read(buffer, 0, bufferSize)) != 0)
                        {
                            totalBytesRead += bytesRead;
                            outputStream.Write(buffer, 0, bytesRead);
                        }
                        outputStream.Close();

                        // Update the file info with physical file info
                        if (outputStream is FileStream)
                        {
                            System.IO.FileInfo fileinfo = new System.IO.FileInfo(this.path.Text);
                            
                            if (fileinfo.Length < 1)
                                throw new Exception(String.Format("Downloaded file ({0}) was 0 bytes.", this.path.Text));
                        }


                    }
                }
                //this.response.LoadFile(this.path.Text);
            }


        }

        private bool RequierdFieldsValidate()
        {
            throw new NotImplementedException();
        }

        public Token RefreshToken(string refreshToken)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(this.AuthenticationUrl.Text);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";

            using (StreamWriter writer = new StreamWriter(myRequest.GetRequestStream()))
            {
                writer.Write(string.Format("refresh_token={0}&client_id={1}&client_secret={2}&grant_type=refresh_token",
                    refreshToken,
                   this.consumerKey.Text,
                   this.secretKey.Text));
            }

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            Stream responseBody = myResponse.GetResponseStream();

            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");


            StreamReader readStream = new StreamReader(responseBody, encode);


            Token tokenResponse;
            tokenResponse = (Token)JsonConvert.DeserializeObject(readStream.ReadToEnd(), typeof(Token));
            tokenResponse.refresh_token = refreshToken;
            tokenResponse.UpdateTime = DateTime.Now;
            tokenResponse.Save(this.consumerKey.Text, this.connectionString.Text);
            return tokenResponse;
        }

        public Token GetAccessTokenParamsFromSalesForce()
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(this.AuthenticationUrl.Text);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";


            using (StreamWriter writer = new StreamWriter(myRequest.GetRequestStream()))
            {
                writer.Write(string.Format("code={0}&grant_type=authorization_code&client_id={1}&client_secret={2}&redirect_uri={3}",
                    this.Code.Text,
                    this.consumerKey.Text,
                    this.secretKey.Text,
                   this.RedirectUrl.Text));
            }
            HttpWebResponse myResponse;
            try
            {
                myResponse = (HttpWebResponse)myRequest.GetResponse();

            }
            catch (WebException webEx)
            {
                using (StreamReader reader = new StreamReader(webEx.Response.GetResponseStream()))
                {
                    throw new Exception(reader.ReadToEnd());

                }


            }

            Stream responseBody = myResponse.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader readStream = new StreamReader(responseBody, encode);
            Token token = JsonConvert.DeserializeObject<Token>(readStream.ReadToEnd());
            token.UpdateTime = DateTime.Now;
            token.Save(this.consumerKey.Text, this.connectionString.Text);
            //return string itself (easier to work with)
            return token;
        }

        private void CreateConnection_Click(object sender, EventArgs e)
        {
            GetAccessTokenParamsFromSalesForce();
        }
    }

    public class Token
    {
        public string id { get; set; }
        public string issued_at { get; set; }
        public DateTime UpdateTime { get; set; }
        public string refresh_token { get; set; }
        public string instance_url { get; set; }
        public string signature { get; set; }
        public string access_token { get; set; }
        public string ClientID { get; set; }

        public void Save(string clientID, string connection)
        {

            Token tokenResponse = new Token();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                using (SqlCommand command = new SqlCommand("SalesForce_SaveToken", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    command.Parameters.AddWithValue("Id", this.id);
                    command.Parameters.AddWithValue("ClientID", clientID);
                    command.Parameters.AddWithValue("Instance_url", this.instance_url);
                    command.Parameters.AddWithValue("AccessToken", this.access_token);
                    command.Parameters.AddWithValue("RefreshToken", this.refresh_token);
                    command.Parameters.AddWithValue("Signature", this.signature);
                    command.Parameters.AddWithValue("Issued_at", this.issued_at);
                    command.Parameters.AddWithValue("UpdateTime", DateTime.Now);

                    command.ExecuteNonQuery();

                }
            }

        }

        internal static Token Get(string clientID, string connection)
        {
            Token tokenResponse = new Token();
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand command = new SqlCommand("SalesForce_GetToken", conn);
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter clientID_Sql = new SqlParameter("ClientID", clientID);
                command.Parameters.Add(clientID_Sql);

                using (command)
                {
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            tokenResponse.UpdateTime = Convert.ToDateTime(reader["UpdateTime"]);
                            tokenResponse.ClientID = clientID;
                            tokenResponse.id = reader["Id"].ToString();
                            tokenResponse.access_token = reader["AccessToken"].ToString();
                            tokenResponse.issued_at = reader["Issued_at"].ToString();
                            tokenResponse.instance_url = reader["Instance_url"].ToString();
                            tokenResponse.signature = reader["Signature"].ToString();
                            tokenResponse.refresh_token = reader["RefreshToken"].ToString();
                        }
                    }
                }
            }
            return tokenResponse;
        }
    }
}
