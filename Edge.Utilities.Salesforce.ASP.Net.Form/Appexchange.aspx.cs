using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Edge.Utilities.Salesforce.ASP.Net.Form
{
    public partial class Appexchange : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GetItNowButton_Click(object sender, ImageClickEventArgs e)
        {
            string loginUrl = string.Empty;
            string redirectedUrl = string.Empty;
            MatchCollection timeMatches = Regex.Matches(this.LoginURL.Text, @"(?<https>(https:[/][/]|www.)([a-z]|[A-Z]|[0-9]|[/.])*)([/.]com)", RegexOptions.IgnoreCase);

            if (timeMatches.Count > 0)
                loginUrl = timeMatches[0].Value;

            redirectedUrl = string.Format("{0}/services/oauth2/authorize?response_type=code&client_id={1}&redirect_uri={2}", loginUrl, Config.ConsumerKey, Config.CallBack_URI);
            Response.Redirect(redirectedUrl);
        }
    }
    public static class Config
    {
        public static string ConsumerKey
        {
            get { return ConfigurationManager.AppSettings["ConsumerKey"]; }
        }

        public static string SecretKey
        {
            get { return ConfigurationManager.AppSettings["SecretKey"]; }
        }
        public static string CallBack_URI
        {
            get { return ConfigurationManager.AppSettings["CallBack_URI"]; }
        }
    }
}