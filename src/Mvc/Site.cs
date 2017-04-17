using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc
{
    /// <summary>
    /// Class that represents a Site in the server.
    /// </summary>
    public class Site
    {
        /// <summary>
        /// Site name.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Root directory where the site is. 
        /// </summary>
        public string physicalPath { get; set; }

        /// <summary>
        /// Default virtual path.
        /// </summary>
        public string virtualPath { get; set; }

        /// <summary>
        /// If the directory is active or not.
        /// </summary>
        public bool directoryBrowsing = false;

        /// <summary>
        /// Dictionary of default documents.
        /// </summary>
        public Dictionary<string, string> defaultDocument;

        /// <summary>
        /// Dictionary with .html file of the default errors.
        /// </summary>
        public Dictionary<string, string> errorPages;
    }
}
