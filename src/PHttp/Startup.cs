using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using System.IO;
using PHttp.Application;

namespace PHttp
{
    public class Startup
    {
        public Dictionary<string, IPHttpApplication> _appInstances = new Dictionary<string, IPHttpApplication>();

        /// <summary>
        /// First method that we use as an test for making reflection and load apps.
        /// </summary>
        /// <param name="Dlls"></param>
        public void LoadApps(PHttpServerConfig Dlls)
        {
            foreach (var Dll in Dlls.sites)
            {
                if (string.IsNullOrEmpty(Dll.physicalPath)) { return; } //sanity check

                DirectoryInfo info = new DirectoryInfo(Dll.physicalPath);
                if (!info.Exists) { return; } //make sure directory exists

              
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
                            if (_appInstances.ContainsKey(Dll.virtualPath)) { }
                            else
                            {
                                _appInstances.Add(Dll.virtualPath, (IPHttpApplication)Activator.CreateInstance(type));
                            }
                        }
                    }
                }
            }
            foreach (var el in _appInstances)
            {
                //Console.WriteLine("Msg: {0}", el.Start());

                Console.WriteLine("   + Instance Type: {0}", el.ToString());
            }
        }

        /// <summary>
        /// It loads all the dll of an application from a specific path. 
        /// </summary>
        /// <param name="PhysicalPath">Sting:Application physical path.</param>
        /// <returns>Task(list(IPHttpApplication)): Application instance list.</returns>
        public async Task<List<IPHttpApplication>> LoadApps(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            var impl = new List<Application.IPHttpApplication>();

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
