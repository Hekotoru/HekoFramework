using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace Mvc
{
    /// <summary>
    /// Class responsible for loading and contain configuration of the
    /// server using the file: "config.json"
    /// </summary>
    public class ServerConfiguration
    {
        /// <summary>
        /// Default server port.
        /// </summary>
        public int port;

        /// <summary>
        /// Dictionary with "path" of .html files
        /// with the error messages provided by the framework.
        /// </summary>
        public Dictionary<string, string> errorPages;

        /// <summary>
        /// Dictionary of default documents or files.
        /// </summary>
        public Dictionary<string, string> defaultDocument;

        /// <summary>
        /// List of current sites on the server.
        /// </summary>
        public List<Site> sites;

        /// <summary>
        /// Determine if the configuration is loaded.
        /// </summary>
        private bool ConfigLoaded = false;

        /// <summary>
        /// It loads all the values for the configuration of the server that are in: the config.json.
        /// </summary>
        public void Load()
        {
            string filePath = @"../../../PHttp/config.json";

            if (File.Exists(filePath))
            {
                JObject jsonConfigFile = JObject.Parse(File.ReadAllText(filePath));

                this.port = (int)jsonConfigFile["port"];

                this.errorPages = JsonConvert.DeserializeObject
                    <Dictionary<string, string>>(jsonConfigFile["errorPages"].ToString());

                this.defaultDocument = JsonConvert.DeserializeObject
                    <Dictionary<string, string>>(jsonConfigFile["defaultDocument"].ToString());

                this.sites = JsonConvert.DeserializeObject<List<Site>>(jsonConfigFile["sites"].ToString());

                this.ConfigLoaded = true;

            }
            else
            {
                Console.WriteLine("Error: Config file not Found.");
                this.ConfigLoaded = false;
            }

        }

        /// <summary>
        /// Search for a site according to "virtualPath"
        /// </summary>
        /// <param name="physicarPath">string: Virtual Path of site.</param>
        /// <returns>Site: Site Loaded.</returns>
        public Site GetSiteByVirtualPath(string virtualPath)
        {
            if (this.ConfigLoaded)
            {
                foreach (Site site in this.sites)
                {
                    if (site.virtualPath.ToLower().Equals(virtualPath.ToLower()))
                    {
                        return site;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Update the initial configuration file: "config.json" and
        /// returns data loaded.
        /// </summary>
        public void UpdateConfigFile()
        {
            string rootpath = "../../../";

            string[] ingoreFolders = { "PHttp", "Mvc", "Franci_Framework", "packages", ".vs", "artifacts" };

            List<string> directoriesSites = GetDirectories(rootpath, ingoreFolders);

            DirectoryInfo directoryInfo;
            FileInfo[] fileInfo;

            List<string> directoriesConfigs = new List<string>();

            foreach (string dirSite in directoriesSites)
            {
                directoryInfo = new DirectoryInfo(dirSite);
                fileInfo = directoryInfo.GetFiles("*.json");

                foreach (FileInfo fi in fileInfo)
                {
                    string site = File.ReadAllText(fi.FullName);

                    JObject AppSite = JObject.Parse(site);


                }

            }


        }


        /// <summary>
        /// Gets all directories from path received.
        /// </summary>
        /// <param name="path">sting: root path.</param>
        /// <param name="ingoreFolders">string: folders to ignore.</param>
        /// <returns>sting: array of Sub-directories.</returns>
        private static List<string> GetDirectories(string path, string[] ingoreFolders)
        {
            string searchPattern = "*";

            List<string> directories = GetDirectories(path, searchPattern);

            directories = RemoveDirectories(directories, ingoreFolders);

            return directories;
        }

        /// <summary>
        /// search for all the directories of the sites within the specified path.
        /// </summary>
        /// <param name="path">string:"Path" with the directory where you can find the folders.</param>
        /// <param name="searchPattern">String: search pattern</param>
        /// <returns>The list of directories.</returns>
        private static List<string> GetDirectories(string path, string searchPattern)
        {
            try
            {
                return Directory.GetDirectories(path, searchPattern).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Removes all path of received directories
        /// </summary>
        /// <param name="directory">List <string>: list with directories</param>
        /// <param name="ingnoreFolders">string[]: Array with directories to be deleted</param>
        /// <returns>List<string>: list updated.</returns>
        private static List<string> RemoveDirectories(List<string> directory, string[] ingnoreFolders)
        {
            for (int i = 0; i < ingnoreFolders.Length; i++)
            {
                for (int j = 0; j < directory.Count; j++)
                {
                    if (directory[j].Contains(ingnoreFolders[i]))
                    {
                        directory.Remove("../../../" + ingnoreFolders[i]);
                        break;
                    }
                }
            }

            return directory;
        }


    }
}