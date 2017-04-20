using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mvc;
using MySql.Data.MySqlClient;

namespace App.Controllers
{
    class UserController : ControllerBase 
    {
        [HttpPost]
        public AResult RegisterUser()
        {
            string username = (string)Request.Params["username"];
            string pass = (string)Request.Params["password"];
            string firstname = (string)Request.Params["firstname"];
            string lastname = (string)Request.Params["lastname"];

            if(username == null || pass == null || firstname == null || lastname == null)
            {
                return ReturnJson(@"{ ""status"":""403"",""message"":""Missing form information""}", 403);
            }
            using(MySqlConnection connection = new MySqlConnection("Server=localhost;User Id=root;Password=DdaeMLtzsSeyuYFT;Database=heko_shortener"))
            {
                connection.Open();
                string query = "INSERT INTO users (username,password,firstname,lastname) VALUES ('"+username+"','"+pass+"','"+firstname+"','"+lastname+"')";
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();

            }
            return ReturnJson(@"{ ""status"": ""200"",""message"":""User created""}", 200);
        }

        [HttpPost]
        public AResult Login()
        {
            string username = (string)Request.Params["username"];
            string pass = (string)Request.Params["password"];
            if (username == null || pass == null)
            {
                return ReturnJson(@"{ ""status"":""403"",""message"":""Missing form information""}", 403);
            }
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;User Id=root;Password=DdaeMLtzsSeyuYFT;Database=heko_shortener"))
            {
                connection.Open();
                string query = "SELECT firstname,lastname FROM users WHERE username='"+username+"' AND password='"+pass+"'";
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();

            }
            return ReturnJson(@"{ ""status"": ""200"" ""message"":""Login success""", 200);
        }

        [HttpGet]
        public AResult Register()
        {
            object Object = new {};

            return ReturnView(200, Object,"register");
        }

        [HttpGet]
        public AResult Index()
        {
            object Object = new {};

            return ReturnView(200, Object,"index");
        }

        [HttpGet]
        public AResult LoginPage()
        {
            object Object = new {};

            return ReturnView(200, Object,"login");
        }
    }
}
