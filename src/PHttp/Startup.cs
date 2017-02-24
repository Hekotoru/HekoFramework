using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using System.IO;

namespace PHttp
{
    public class Startup
    {
        public static void LoadApps(string Dll)
        {
            if (string.IsNullOrEmpty(Dll)) { return; } //sanity check

            DirectoryInfo info = new DirectoryInfo(Dll);
            if (!info.Exists) { return; } //make sure directory exists

            var impl = new List<Application.IPHttpApplication>();

            foreach (FileInfo file in info.GetFiles("*.dll")) //loop through all dll files in directory
            {
                Assembly currentAssembly = null;
                try
                {
                    var name = AssemblyName.GetAssemblyName(file.FullName);
                    currentAssembly = Assembly.Load(name);
                }
                catch (Exception ex)
                {
                    continue;
                }

                currentAssembly.GetTypes()
                    .Where(t => t != typeof(Application.IPHttpApplication) && typeof(Application.IPHttpApplication).IsAssignableFrom(t))
                    .ToList()
                    .ForEach(x => impl.Add((Application.IPHttpApplication)Activator.CreateInstance(x)));
            }

            foreach (var el in impl)
            {
                Console.WriteLine($"Msg: {el.Start()}");
            }

            Console.ReadKey();
        }
    }
}
