using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Edge.Core.Data;
using Edge.Core.Configuration;

namespace Edge.Application.ProductionManagmentTools
{
    public class Profile
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public bool IsAuto { set; get; }
        public List<string> Accounts { set; get; }
        public List<CBox> Channels { set; get; }
        public List<CBox> ValidationServices { set; get; }
        
        
        public Profile()
        {
            Accounts = new List<string>();
            Channels = new List<CBox>();
            ValidationServices = new List<CBox>();

            CheckBox a = new CheckBox();
            
        }

        internal bool TrySave()
        {
            using (SqlConnection sqlCon = new SqlConnection(AppSettings.GetConnectionString("Edge.Application.ProductionManagmentTools", "Seperia")))
            {
                sqlCon.Open();
                SqlCommand sqlCommand = DataManager.CreateCommand(
                    "Insert into [dbo].[DataChecksProfiles] values (@ProfileName,@IsAutomatic,@Schedule,@Accounts,@Channels,@ValidationServices");

                SqlParameter profileNameParam = new SqlParameter("@ProfileName", System.Data.SqlDbType.NVarChar);
                SqlParameter IsAutomaticParam = new SqlParameter("@IsAutomatic", System.Data.SqlDbType.Bit);
                SqlParameter ScheduleParam = new SqlParameter("@Schedule", System.Data.SqlDbType.NVarChar);
                SqlParameter AccountsParam = new SqlParameter("@Accounts", System.Data.SqlDbType.NVarChar);
                SqlParameter ChannelsParam = new SqlParameter("@Channels", System.Data.SqlDbType.NVarChar);
                SqlParameter ValidationServicesParam = new SqlParameter("@ValidationServices", System.Data.SqlDbType.NVarChar);

                profileNameParam.Value = Name;
                sqlCommand.Parameters.Add(profileNameParam);

                IsAutomaticParam.Value = false;
                sqlCommand.Parameters.Add(IsAutomaticParam);

                ScheduleParam.Value = string.Empty;
                sqlCommand.Parameters.Add(ScheduleParam);

                AccountsParam.Value = Newtonsoft.Json.JsonConvert.SerializeObject(Accounts);
                sqlCommand.Parameters.Add(AccountsParam);

                ChannelsParam.Value = Newtonsoft.Json.JsonConvert.SerializeObject(this.Channels);
                sqlCommand.Parameters.Add(ChannelsParam);

                ValidationServicesParam.Value = Newtonsoft.Json.JsonConvert.SerializeObject(ValidationServices);
                sqlCommand.Parameters.Add(ValidationServicesParam);

                sqlCommand.Connection = sqlCon;
                sqlCommand.ExecuteNonQuery();

            }
            return true;
        }

        internal void AddChannel(CheckBox checkBox)
        {
          
        }

        internal void RemoveChannel(CheckBox checkBox)
        {
            throw new NotImplementedException();
        }
    }
    public class CBox 
    {
        public string Name { set; get; }
        public bool Checked { set; get; }
       
    }

}
