using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc
{
    /// <summary>
    /// Manage request content.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// This Field is the representative of URL path of the request.
        /// </summary>
        string UrlPath;

        /// <summary>
        /// This Field is the representative of httpMethod of the request.
        /// </summary>
        public string HttpMethod;

        /// <summary>
        /// This Field is the representative of header of the request.
        /// </summary>
        public NameValueCollection Headers;

        /// <summary>
        /// This Field is the representative of parameters of the request.
        /// </summary>
        public NameValueCollection Params;

        /// <summary>
        /// This Field is the representative of files of the requests.
        /// </summary>
        public Dictionary<string, HttpFile> Files;

        /// <summary>
        /// Constructor whit parameters of the request.
        /// </summary>
        /// <param name="urlPath">Url path of request.</param>
        /// <param name="httpMethod">HttpMethod of request.</param>
        /// <param name="headers">Header of request.</param>
        /// <param name="param">Parameters of request.</param>
        /// <param name="files">files of requests.</param>
        public Request(string urlPath, string httpMethod, NameValueCollection headers, NameValueCollection param, Dictionary<string, HttpFile> files)
        {
            this.UrlPath = urlPath;
            this.Files = files;
            this.Headers = headers;
            this.Params = param;
        }

        /// <summary>
        /// This method evaluate if the request contains Files.
        /// </summary>
        /// <param name="key">string: key of file object.</param>
        /// <returns>true: if contains files, false: isn't contains.</returns>
        public bool ContainsFiles(string key)
        {
            return this.Files.ContainsKey(key);
        }

        /// <summary>
        /// This method evaluate if the request contains Parameters.
        /// </summary>
        /// <param name="key">string: key of parameters object.</param>
        /// <returns>true: if contains files, false: isn't contains.</returns>
        public bool ContainsParams(string key)
        {
            if (Params[key] != null && Params[key] != "")
            {
                return true;
            }
            return false;
        }
    }
}
