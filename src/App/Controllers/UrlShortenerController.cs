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
        [Authorize]
        [HttpGet]
        public AResult GetUrls()
        {
            return ReturnJson("test",200);
        }

        [Authorize]
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
                string query = "INSERT INTO url (shortUrl, Description) VALUES('"+shortener+"','"+urlDescription+"')";
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();

            }
            return ReturnJson(@"{ ""status"": ""200"" ""message"":""Shortener created""", 200);
        }

        [HttpGet]
        [Authorize]
        public AResult Click()
        {
            return ReturnJson(@"{ ""status"": ""200"" ""message"":""Click done""", 200);
        }

        [HttpGet]
        [Authorize]
        public AResult GetUrl()
        {
            return ReturnJson(@"{ ""status"": ""200"" ""message"":""Click done""", 200);
        }

    }
}
