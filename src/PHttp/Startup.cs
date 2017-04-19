using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Reflection;
using PHttp.Application;

namespace PHttp
{
    public class Startup
    {

        /// <summary>
        /// It loads the dll of an application from a specific path. 
        /// </summary>
        /// <param name="PhysicalPath">Sting:Application physical path.</param>
        /// <returns>IPHttpApplication: Application instance.</returns>
        public static IPHttpApplication LoadApp(string PhysicalPath)
        {
            DirectoryInfo info = new DirectoryInfo(PhysicalPath);

            //make sure path aren't empty or null.
            if (string.IsNullOrEmpty(PhysicalPath)) { return null; }

            //make sure directory exists.
            if (!info.Exists) { return null; }

            //loop through all dll files in directory.
            foreach (FileInfo file in info.GetFiles("*.dll"))
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

                foreach (var type in currentAssembly.GetTypes())
                {
                    if (type != typeof(IPHttpApplication) && typeof(IPHttpApplication).IsAssignableFrom(type))
                    {
                        return ((IPHttpApplication)Activator.CreateInstance(type));
                    }
                }

            }
            return null;
        }


        /// <summary>
        /// It loads all the dll of an application from a specific path. 
        /// </summary>
        /// <param name="PhysicalPath">Sting:Application physical path.</param>
        /// <returns>Task(list(IPHttpApplication)): Application instance list.</returns>
        public async Task<List<IPHttpApplication>> LoadApps(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            var impl = new List<IPHttpApplication>();

            //make sure path aren't empty or null.
            if (string.IsNullOrEmpty(path)) { return impl; }

            //make sure directory exists
            if (!info.Exists) { return impl; }

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

                var types = currentAssembly.GetTypes();

                foreach (var type in types)
                {
                    if (type != typeof(IPHttpApplication) && typeof(IPHttpApplication).IsAssignableFrom(type))
                    {
                        impl.Add((IPHttpApplication)Activator.CreateInstance(type));
                    }
                }
            }
            return impl;
        }

    }
}
