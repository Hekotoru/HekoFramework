using System;
using System.Configuration;
using PHttp;
using System.IO;
using System.Diagnostics;
using System.Text;
using PHttp.Application;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mvc;

namespace Heko
{
    class Program
    {
        /// <summary>
        /// Method to handler the input from developer.
        /// </summary>
        /// <param name="ev"></param>
        /// <returns>new OutputStream</returns>
        public static HttpOutputStream OutputHandler(HttpRequestEventArgs ev)
        {
            // The response must be written to e.Response.OutputStream.
            // When writing text, a StreamWriter can be used.
            string dirPath = @"C:\Users\Hector Aristy\Documents\GitHub\HekoFramework\src\PHttp\Resources";
            string filePath = dirPath + ev.Request.Path;
            if (ev.Request.Path.Equals("/"))
            {
                filePath = dirPath + '/' + @"index.html";
            }
            string extension = Path.GetExtension(filePath);
            string mime = HttpMimeType.GetMimeType(extension);
            ev.Response.ContentType = mime;

            byte[] data;
            if (!File.Exists(filePath))
            {
                data = File.ReadAllBytes(dirPath + '/' + "404.html");
                ev.Response.StatusCode = 404;
                ev.Response.ContentType = "text/html";
            }
            else
            {
                data = File.ReadAllBytes(filePath);
            }
            MemoryStream stream = new MemoryStream(data);

            return new HttpOutputStream(stream);
        }

        /// <summary>
        /// Init of the server
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //string ApplicationsDir = ConfigurationManager.AppSettings["ApplicationsDir"];

            //Console.WriteLine("Looking for apps in {0}",ApplicationsDir);
            //Startup.LoadApps(ApplicationsDir);
            /*
            var config = PHttpServerConfig.GetPHttpServerConfig();
            var startMvc = new Startup();
            Console.WriteLine(config.port);
            */
            /// Object for server configuration.
            Mvc.ServerConfiguration configurationManager = new Mvc.ServerConfiguration();

            /// object for startup all apps.
            Startup startup = new Startup();

            /// Task of List of all apps.
            Task<List<IPHttpApplication>> AsyncAllApps = null;

            /// Load the server configuration.
            configurationManager.Load();

            /// Load all active apps from configuration loaded.
            foreach (Site site in configurationManager.sites)
            {
                AsyncAllApps = startup.LoadApps(site.physicalPath + "/bin/Debug");

                foreach (IPHttpApplication app in AsyncAllApps.Result)
                {
                    //  Init app and Generate all routes according pattern defined 
                    // in every app.
                    app.Init(site);
                }
            }

            if (AsyncAllApps == null)
            {
                Console.WriteLine("Not Apps loaded. Verify physical path of any site added into config.json");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            /// This object is the handler of the respond.
            RequestHandler ResHandler = new RequestHandler(configurationManager, AsyncAllApps);
            using (var server = new HttpServer(configurationManager.port))
            {
                // New requests are signaled through the RequestReceived
                // event.
                server.RequestReceived += (s, e) =>
                {
                    e.Response.OutputStream = ResHandler.GetRespond(e);
                };
                server.Start();

                // Start the default web browser.

                Process.Start(String.Format("http://{0}/", server.EndPoint));   

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();

                // When the HttpServer is disposed, all opened connections
                // are automatically closed.
            }
        }
    }
}
