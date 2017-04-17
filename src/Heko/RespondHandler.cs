using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using PHttp;
using PHttp.Application;
using Mvc;


namespace Heko
{
    /// <summary>
    /// This class is responsible for all responses to the client
    /// </summary>
    public class RespondHandler
    {
        /// <summary>
        /// Container of the Server configuration.
        /// </summary>
        Mvc.ServerConfiguration configurationManager;

        /// <summary>
        /// Application which serves the framework.
        /// </summary>
        IPHttpApplication App = null;

        /// <summary>
        /// Array data for the response.
        /// </summary>
        byte[] data;

        /// <summary>
        /// Store for the BinaryReader.
        /// </summary>
        MemoryStream stream;

        /// <summary>
        /// List of all application created.  
        /// </summary>
        Task<List<IPHttpApplication>> AllApps;

        /// <summary>
        /// Container of the Action of the Request.
        /// </summary>
        Dictionary<string, object> ActionRequest;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="conf">PHttp.ConfigurationManager: Object of the configuration of the server</param>
        public RespondHandler(Mvc.ServerConfiguration conf, Task<List<IPHttpApplication>> allApps)
        {
            this.configurationManager = conf;
            this.AllApps = allApps;
        }

        /// <summary>
        /// This is a method that responds to the customer according to the event received.
        /// </summary>
        /// <param name="requestEvent">HttpRequestEventArgs: Request event.</param>
        /// <returns>HttpOutputStream: Event respond of the argument received.</returns>
        public HttpOutputStream GetRespond(HttpRequestEventArgs requestEvent)
        {
            ActionRequest = new Dictionary<string, object>();

            /// Array of the URL path received.
            string[] UrlSplited = requestEvent.Request.Path.Replace("favicon.ico", string.Empty).Split('/');
            string SiteName = UrlSplited[1];

            // show state in the console.
            Console.WriteLine("Client: {0} makes request => {1}", SiteName, requestEvent.Request.Path);

            if (requestEvent.Request.Path.Equals("/"))
            {
                requestEvent.Response.ContentType = HttpMimeType.
                    GetMimeType(Path.GetExtension(configurationManager.defaultDocument["index"]));
                requestEvent.Response.Status = "200";
                data = File.ReadAllBytes(configurationManager.defaultDocument["index"]);
            }
            else
            {
                // Do it if the site not exist.
                if (!Exist(SiteName))
                {
                    requestEvent.Response.ContentType = HttpMimeType
                        .GetMimeType(Path.GetExtension(configurationManager.errorPages["404"]));
                    requestEvent.Response.Status = "404";
                    data = File.ReadAllBytes(configurationManager.errorPages["404"]);
                }
                // else: execute method of one of the apps loaded.
                else
                {

                    ActionRequest.Add("URLPath", requestEvent.Request.Path.Replace("/" + SiteName, "").ToLower());
                    ActionRequest.Add("HttpMethod", requestEvent.Request.HttpMethod);
                    ActionRequest.Add("Params", requestEvent.Request.Form);
                    ActionRequest.Add("Header", requestEvent.Request.Headers);

                    Dictionary<string, HttpFile> files = new Dictionary<string, HttpFile>();

                    for (int i = 0; i < requestEvent.Request.Files.Count; i++)
                    {
                        HttpPostedFile file = requestEvent.Request.Files.Get(i);
                        HttpFile httpFile = new HttpFile(file.ContentLength, file.ContentType, file.FileName, file.InputStream);
                        files.Add(requestEvent.Request.Files.GetKey(i), httpFile);
                    }
                    ActionRequest.Add("Files", files);

                    foreach (IPHttpApplication Application in AllApps.Result)
                    {
                        Site AppSite = (Site)Application.GetSite();

                        if (AppSite.virtualPath.ToLower() == SiteName.ToLower())
                        {
                            var response = Application.ExecuteAction(ActionRequest);
                            requestEvent.Response.Status = ((AResult)response).StatusCode().ToString();
                            requestEvent.Response.ContentType = ((AResult)response).ContentType();

                            try
                            {
                                stream = ((AResult)response).Content();
                                return new HttpOutputStream(stream);
                            }
                            catch (Exception ex)
                            {
                                requestEvent.Response.ContentType = HttpMimeType.GetMimeType(Path.GetExtension(configurationManager.errorPages["404"]));
                                requestEvent.Response.Status = "404";
                                data = File.ReadAllBytes(configurationManager.errorPages["404"]);
                            }

                        }

                    }


                    /// TODO:hacer que se actualize site en el js despues que el user cree algun view. 

                }
            }

            stream = new MemoryStream(data);
            return new HttpOutputStream(stream);
        }

        /// <summary>
        /// Evaluate if a site exists.
        /// </summary>
        /// <param name="siteName">string: Site name</param>
        /// <returns>true: if exist, false: if not exist.</returns>
        private bool Exist(string siteName)
        {
            foreach (Site site in configurationManager.sites)
            {
                if (siteName.ToLower().Equals(site.virtualPath.ToLower()))
                    return true;
            }
            return false;
        }





    }
}