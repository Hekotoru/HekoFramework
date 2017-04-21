using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mvc;
using MySql.Data.MySqlClient;

namespace App.Controllers
{
    class UrlShortenerController : ControllerBase
    {
        [HttpGet]
        public AResult GetUrls()
        {
            
            string userId = Request.QueryString["userId"];
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;User Id=root;Password=DdaeMLtzsSeyuYFT;Database=heko_shortener"))
            {
                string json = @"{""body"":";
                connection.Open();
                string query = @"SELECT url.shortUrl,url.Description,url.clicks_done,url.last_click,referers.Description as referers,agents.Description as agent 
                               FROM url  LEFT JOIN referers ON referers.url_id = url.id 
                               LEFT JOIN agents ON agents.url_id = url.id WHERE user_id = "+userId+"";
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                MySqlDataReader info= cmd.ExecuteReader();
                json += "[";
                while (info.Read())
                {
                    
                    json += "{";
                    json +="\"shortUrl\": \""+info["shortUrl"].ToString()+ "\",";
                    json += "\"Description\":\"" + info["Description"].ToString() + "\",";
                    json += "\"clicks_done\":\"" + info["clicks_done"].ToString() + "\",";
                    json += "\"last_click\":\"" + info["last_click"].ToString() + "\",";
                    json += "\"referers\":\"" + info["referers"].ToString() + "\",";
                    json += "\"agent\":\"" + info["agent"].ToString() + "\"";
                    if (info.Read())
                        json += "},";
                    else
                        json += "}";
                }
                json += "]}";
                return ReturnJson(json, 200);
            }
        }

        [HttpDelete]
        public AResult Delete()
        {
            string urlId = Request.Params["urlId"];
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;User Id=root;Password=DdaeMLtzsSeyuYFT;Database=heko_shortener"))
            {
                connection.Open();
                string query = "DELETE FROM url WHERE id="+urlId+"";
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
        
            }
            return ReturnJson(@"{ ""status"": ""200"" ""message"":""Delete success""", 200);
        }

        [HttpPost]
        public AResult Shortener()
        {
            string urlDescription = Request.Params["Description"];
            string shortener = "http://45.55.77.201:8888/App/"+Guid.NewGuid().ToString().Substring(0, 8);
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;User Id=root;Password=DdaeMLtzsSeyuYFT;Database=heko_shortener"))
            {
                connection.Open();
                string query = "INSERT INTO url (shortUrl, Description,created,clicks_done) VALUES('"+shortener+"','"+urlDescription+"',NOW(),0)";
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();

            }
            return ReturnJson(@"{ ""status"": ""200"" ""message"":""Shortener created""", 200);
        }

        [HttpGet]
        public AResult Click()
        {
            string Referer = Request.Headers["Referer"];
            string Agent = Request.Headers["User-Agent"];
            string urlId = Request.QueryString["urlId"];
            MySqlCommand cmd = new MySqlCommand();
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;User Id=root;Password=DdaeMLtzsSeyuYFT;Database=heko_shortener"))
            {
                connection.Open();
                string query = "UPDATE url SET clicks_done =(clicks_done+1), last_click = NOW() WHERE id=" + urlId + "";
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                query = "INSERT INTO referers (description,url_id) VALUES('" + Referer + "','" + urlId + "')";
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                query = "INSERT INTO agents (description,url_id) VALUES('" + Agent + "','" + urlId + "')";
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
            }
            return ReturnJson(@"{ ""status"": ""200"" ""message"":""Click done""", 200);
        }

        [HttpGet]
        public AResult GetUrl()
        {
            string urlId = Request.QueryString["urlId"];
            string userId = Request.QueryString["userId"];
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;User Id=root;Password=DdaeMLtzsSeyuYFT;Database=heko_shortener"))
            {
                string json = @"{""body"":";
                connection.Open();
                string query = @"SELECT url.shortUrl,url.Description,url.clicks_done,url.last_click,referers.Description as referers,agents.Description as agent 
                               FROM url  LEFT JOIN referers ON referers.url_id = url.id 
                               LEFT JOIN agents ON agents.url_id = url.id WHERE url.user_id = " + userId + " and url.id='"+urlId+"'";
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                MySqlDataReader info = cmd.ExecuteReader();
                json += "[";
                while (info.Read())
                {

                    json += "{";
                    json += "\"shortUrl\": \"" + info["shortUrl"].ToString() + "\",";
                    json += "\"Description\":\"" + info["Description"].ToString() + "\",";
                    json += "\"clicks_done\":\"" + info["clicks_done"].ToString() + "\",";
                    json += "\"last_click\":\"" + info["last_click"].ToString() + "\",";
                    json += "\"referers\":\"" + info["referers"].ToString() + "\",";
                    json += "\"agent\":\"" + info["agent"].ToString() + "\"";
                    if (info.Read())
                        json += "},";
                    else
                        json += "}";
                }
                json += "]}";
                return ReturnJson(json, 200);

            }
        }

    }
}
