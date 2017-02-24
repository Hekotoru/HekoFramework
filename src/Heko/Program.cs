using System;
using System.Configuration;
using PHttp;

namespace Heko
{
    class Program
    {
        static void Main(string[] args)
        {
            string ApplicationsDir = ConfigurationManager.AppSettings["ApplicationsDir"];

            Console.WriteLine("Looking for apps in {0}",ApplicationsDir);
            Startup.LoadApps(ApplicationsDir);
        }
    }
}
