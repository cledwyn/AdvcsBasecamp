using Basecamp.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Basecamp.Models
{
    public class BCX
    {
        string ClientID = ConfigurationManager.AppSettings["BasecampClientId"];
        string ClientSecret = ConfigurationManager.AppSettings["BasecampClientSecret"];
        string RedirectURL = "https://advancement.macalester.edu/m/auth/basecamp.aspx";

        public string ProjectBaseUrl = "https://basecamp.com/2063670/api/v1";
        public DateTime ExpireDate { get; set; }
        public string AccessToken { get; set; }
        public string ID_NUMBER { get; set; }




        public BCX(string idNumber)
        {
            foreach (DataRow r in OracleRS.GetDataTable("Select * from macadvweb.oauth where id_number = '" +   App.SqlScrubString(idNumber) + "'", "macadvweb").Rows)
            {
                this.ID_NUMBER = r["ID_NUMBER"].ToString();
                this.AccessToken = r["access_token"].ToString();
                this.ExpireDate = DateTime.Parse(r["expire_date"].ToString());

                if (ExpireDate < DateTime.Now)
                    RefreshToken();
            }


        }

        public void RefreshToken()
        {
            string token = OracleRS.ExecuteScalar(String.Format("select max(refresh_token) from MACADVWEB.OAUTH  where id_number = '{0}'", ID_NUMBER), "macadvweb");
            string refreshUrl = "https://launchpad.37signals.com/authorization/token?type=refresh&client_id={0}&client_secret={1}&refresh_token={2}";
            refreshUrl = String.Format(refreshUrl, ClientID, ClientSecret, token);
            string response = PostURL(refreshUrl, "", true);

            var json = Newtonsoft.Json.Linq.JObject.Parse(response);
            //  GET THE NEW VALUES
            this.AccessToken = json["access_token"].ToString();
            ExpireDate = DateTime.Now.AddSeconds(double.Parse(json["expires_in"].ToString()));
            // SET THEM
            OracleRS.Command cmd = new OracleRS.Command("update macadvweb.oauth set access_token=:access_token, expire_date=:expiresin where id_number = :id_number", "macadvweb", System.Data.CommandType.Text);
            cmd.AddParameterWithValue(":access_token", AccessToken);
            cmd.AddParameterWithValue(":expiresin", ExpireDate);
            cmd.AddParameterWithValue(":id_number", ID_NUMBER);
            cmd.ExecuteNonQuery();
        }

        public JArray Projects()
        {
            string endpoint = "/projects.json";
            string url = ProjectBaseUrl + endpoint + "?access_token=" + AccessToken;
            string response = App.GetWebPage(url);
            return JArray.Parse(response);
        }

        public JArray ToDoLists(int project)
        {
            string endpoint = String.Format("/projects/{0}/todolists.json", project);
            string url = ProjectBaseUrl + endpoint + "?access_token=" + AccessToken;
            string response = App.GetWebPage(url);
            return JArray.Parse(response);
        }

        public JObject ToDoList(int project, int list)
        {
            string endpoint = String.Format("/projects/{0}/todolists/{1}.json", project, list);
            string url = ProjectBaseUrl + endpoint + "?access_token=" + AccessToken;
            System.Diagnostics.Debug.Write(url);
            string response = App.GetWebPage(url);
            JObject ans = JObject.Parse(response);
            return ans;
        }

        public JObject ToDo(int project, int todo)
        {
            string endpoint = String.Format("/projects/{0}/todos/{1}.json", project, todo);
            string url = ProjectBaseUrl + endpoint + "?access_token=" + AccessToken;
            System.Diagnostics.Debug.Write(url);
            string response = App.GetWebPage(url);
            JObject ans = JObject.Parse(response);
            return ans;
        }



        string PostURL(string url, string postData, bool IsPost = false)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }


    }

}