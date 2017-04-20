using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandlebarsDotNet;
using System.IO;

namespace Mvc
{
    /// <summary>
    /// Class responsible for displaying an html view.
    /// </summary>
    public class View : AResult
    {
        public string path;
        public object data;
        public int statusCode;

        /// <summary>
        /// Container of specific view name.
        /// </summary>
        string CustomView;

        /// <summary>
        /// Name of the controller;
        /// </summary>
        string controllerName;


        public string physicalPath;
        public View(int _statusCode, object _data, string _controllerName, string _physicalPath, string _customView)
        {
            controllerName = _controllerName;
            CustomView = _customView;
            data = _data;
            path = _physicalPath;
            statusCode = _statusCode;
            physicalPath = _physicalPath;
        }

        string AResult.ContentType()
        {
            return "text/html";
        }

        int AResult.StatusCode()
        {
            return statusCode;
        }

        MemoryStream AResult.Content()
        {
            string pathNew = path.Replace("/bin/Debug", "");
            string defaultViewPath = pathNew + "/Views/" + this.controllerName;
            string customViewPath = pathNew + "/Views/" + this.CustomView;
            byte[] dataDynamicView = null;

            customViewPath += ".html";
            defaultViewPath += ".html";

            /// If the custom view parameter was entered. 
            if (!string.IsNullOrEmpty(customViewPath))
            {
                if (File.Exists(customViewPath))
                {
                    dataDynamicView = File.ReadAllBytes(customViewPath);
                }
                else
                {
                    return null;
                }
            }
            else  /// If the custom view parameter was not entered. 
            {
                if (File.Exists(defaultViewPath))
                {
                    dataDynamicView = File.ReadAllBytes(defaultViewPath);
                }
                else
                {
                    return null;
                }
            }

            var template = Handlebars.Compile(Encoding.Default.GetString(dataDynamicView));
            var result = template(this.data);
            return new MemoryStream(Encoding.ASCII.GetBytes(result));
        }
    }
}
