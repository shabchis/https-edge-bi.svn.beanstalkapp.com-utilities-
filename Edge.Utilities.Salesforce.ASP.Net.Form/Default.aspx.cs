using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Edge.Utilities.Salesforce.ASP.Net.Form
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            this.code.Text = this.Request.Params["Code"];
           
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            string code = this.Request.Params["Code"];
            SmtpClient smtp = new SmtpClient();

            smtp.Host = "mail.authsmtp.com";
            smtp.Port = 25;
            smtp.Credentials = new NetworkCredential("ac56719", "mbrxxyg7nkcugr");
            MailAddress from = new MailAddress("edgeapp@edge.bi");

            MailAddress to = new MailAddress("shay@edge.bi");
            MailMessage msg = new MailMessage(from, to);
            msg.CC.Add("naamafr@seperia.com");
            msg.Subject = "Integration code :" + code;
            msg.Body = "Consumer key: " + this.consumerKey.Text + "### Secret Key: " + this.secretKey.Text;
            smtp.Send(msg);
        }
    }
}