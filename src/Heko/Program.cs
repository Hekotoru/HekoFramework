using System;
using System.Configuration;
using PHttp;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace Heko
{
    class Program
    {
        static void Main(string[] args)
        {
            //string ApplicationsDir = ConfigurationManager.AppSettings["ApplicationsDir"];

            //Console.WriteLine("Looking for apps in {0}",ApplicationsDir);
            //Startup.LoadApps(ApplicationsDir);
            using (var server = new HttpServer())
            {
                // New requests are signaled through the RequestReceived
                // event.
                var test = PHttpServerConfig.GetPHttpServerConfig();
                Console.WriteLine(test.port);
                server.StateChanged += (s, e) =>
                {
                    var ar = (HttpEventArgs)e;
                    Console.WriteLine("State changed from {0} to {1}", ar._previousState, ar._newState);
                };
                server.RequestReceived += (s, e) =>
                {
                    // The response must be written to e.Response.OutputStream.
                    // When writing text, a StreamWriter can be used.
                    string dirPath = @"C:\Users\Hector Aristy\Documents\GitHub\HekoFramework\src\PHttp\Resources";
                    string filePath = dirPath + e.Request.Path;
                    if (e.Request.Path.Equals("/"))
                    {
                        filePath = dirPath + '/' + @"index.html";
                    }
                    string extension = Path.GetExtension(filePath);
                    string mime = HttpMimeType.GetMimeType(extension);
                    e.Response.ContentType = mime;

                    byte[] data;
                    if (!File.Exists(filePath))
                    {
                        data = File.ReadAllBytes(dirPath + '/' + "404.html");
                        e.Response.StatusCode = 404;
                        e.Response.ContentType = "text/html";
                    }
                    else
                    {
                        data = File.ReadAllBytes(filePath);
                    }

                    MemoryStream stream = new MemoryStream(data);
                    e.Response.OutputStream = new HttpOutputStream(stream);
                };

                // Start the server on a random port. Use server.EndPoint
                // to specify a specific port, e.g.:
                //
                //     server.EndPoint = new IPEndPoint(IPAddress.Loopback, 80);
                //

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
