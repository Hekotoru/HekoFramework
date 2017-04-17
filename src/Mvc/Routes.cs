using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc
{
    /// <summary>
    /// This class is responsible of all the routes defined by the user.
    /// </summary>
    public class Routes
    {
        public string _action;
        public string _controller;
        public string _method;
        public string _pattern;
        public List<string> _params;
        public string UrlPath { get; set; }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="httpMethod">string: Name of  Method Action.</param>
        /// <param name="controllerName">string: Name of controller</param>
        /// <param name="action">string: Name of cation of the controller.</param>
        /// <param name="path">string: Physical path of the controller</param>
        public Routes(string method, string controller, string action, string path)
        {
            UrlPath = path;
            _method = method;
            _controller = controller;
            _action = action;
        }

        /// <summary>
        /// Register of the pattern of routers
        /// </summary>
        /// <param name="pattern">string:Route pattern</param>
        public void registerPattern(string pattern)
        {
            _pattern = pattern;
        }

    }
}
