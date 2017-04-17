using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc
{
    /// <summary>
    /// This class has all basic functions of controllers. 
    /// </summary>
    public class ControllerBase
    {
        public Request Request;
        public HttpContext httpContext; 
        public Routes route;
        public ControllerBase()
        {
            httpContext = new HttpContext();
        }

        /// <summary>
        /// Send a json object.
        /// </summary>
        /// <param name="_StatusCode">string: status code.</param>
        /// <param name="_json">string: json string.</param>
        /// <returns>MemoryStream for the response with json content.</returns>
        public AResult ReturnJson(string _json, int _StatusCode)
        {
            return new Json(_json, _StatusCode);
        }

        /// <summary>
        /// Show a Dynamic view with content received.
        /// </summary>
        /// <param name="statusCode">string: Status code of request.</param>
        /// <param name="Object">string: Data send to the view.</param>
        /// <param name="customView">string: Custom view name. If no send vie name, show default view.</param>
        /// <returns>A view with data received.</returns>
        public AResult ReturnView(int statusCode, object Object, string customView = "")
        {
            string controllerName = route._controller.ToLower();

            if (controllerName.Contains("controller"))
            {
                controllerName = controllerName.Replace("controller", "");
            }

            return new View(statusCode, Object, controllerName, httpContext.Site.physicalPath, customView);
        }

        /// <summary>
        /// Send plain text to the browser
        /// </summary>
        /// <param name="statusCode">int: Status code of request</param>
        /// <param name="content">string: Plain text to render.</param>
        /// <returns></returns>
        public AResult ReturnContent(int statusCode, string content)
        {
            return new Content(statusCode, content);
        }
    }
}
