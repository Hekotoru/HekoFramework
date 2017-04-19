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

            using(MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=DdaeMLtzsSeyuYFT;database=Wallapp"))
            {
                connection.Open();
                string query = "INSERT INTO users (username) VALUES ('Hola Desde HekoFramework')";
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();

            }
            return ReturnJson("User created", 200);
        }

        [HttpPost]
        public AResult Login()
        {
            return ReturnJson(":D", 200);
        }
    }
}
