using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mvc;

namespace App
{
    /// <summary>
    /// Controller for testing the framework.
    /// </summary>
    public class HomeController : ControllerBase
    {

        [HttpPost]
        public AResult LoginUser()
        {
            string username = (string)Request.Params["username"];
            string pass = (string)Request.Params["password"];

            object userObject = new { Username = username, Password = pass };

            return ReturnJson(userObject.ToString(),200);
        }

        /// <summary>
        /// This action sends a string with json format.
        /// </summary>
        /// <returns>string: json content.</returns>
        [HttpGet]
        public AResult JsonViewer()
        {
            string StrJson = @"{ ""name"":""Test"", ""about"": i think test is, ""position"":""developer"" }";

            return ReturnJson(StrJson, 200);
        }

        /// <summary>
        /// This action send text without format.
        /// </summary>
        /// <returns>String: text.</returns>
        [HttpGet]
        public AResult PlainText()
        {
            string message = "Plain text.";

            return ReturnContent(200,message);
        }

        /// <summary>
        /// This action create a view Dynamically with text sanded.
        /// </summary>
        /// <returns>View with Data.</returns>
        [HttpGet]
        public AResult HandlerbarsView()
        {
            object Object = new { tittle = "Home", message = "This is a Message from Home view using handlebars." };

            return ReturnView(200, Object);
        }

    }
}