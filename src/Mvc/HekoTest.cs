using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHttp.Application;
using System.IO;
using System.Reflection;
using PHttp;
using System.Collections.Specialized;

namespace Mvc
{

    /// <summary>
    /// Delegate for the Pre-Application Start event.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    public delegate string PreApplicationStartMethod(string physicalPath);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    public delegate string ApplicationStartMethod(string physicalPath);

    /// <summary>
    /// Class responsible for loaded applications.
    /// </summary>
    public class HekoTest : IPHttpApplication
    {

        public event PreApplicationStartMethod PreApplicationStart;

        public event ApplicationStartMethod ApplicationStart;


        /// <summary>
        /// This field is the representative of the request header
        /// </summary>
        Dictionary<string, object> Headers;

        /// <summary>
        /// List of Actual routes in the application.
        /// </summary>
        public List<Routes> Routes;

        /// <summary>
        /// Pattern of URL request.
        /// </summary>
        public string URLPattern { get; set; }

        /// <summary>
        /// Actual site to evaluate.
        /// </summary>
        public Site AppSite;

        /// <summary>
        /// Container of all the routes available from the apps.
        /// </summary>
        public Dictionary<string, string> RoutesAvailable;


        /// <summary>
        /// Default Constructor.
        /// </summary>
        public HekoTest()
        {
            Routes = new List<Routes>();
        }


        /// <summary>
        /// Return the actual Site Instanced.
        /// </summary>
        /// <returns>Site: The site that was loaded.</returns>
        public object GetSite()
        {
            return this.AppSite;
        }

        /// <summary>
        /// Creates all possible routes according to the defined route pattern.
        /// </summary>
        public void GenerateAllRoutes()
        {
            // var app = Startup.LoadApp(AppSite.physicalPath);

            //string defaultPattern = "{controller}/{action}/{id}";
            string URLPath = "";
            ControllerBase baseController = new ControllerBase();
            DirectoryInfo directoryInfo = new DirectoryInfo(AppSite.physicalPath + "/bin/Debug");
            FileInfo[] fileInfo = directoryInfo.GetFiles("*.dll");

            foreach (FileInfo fi in fileInfo)
            {
                Assembly assembly = Assembly.LoadFrom(fi.FullName);

                foreach (var type in assembly.GetTypes())
                {
                    if (type != typeof(ControllerBase) && typeof(ControllerBase).IsAssignableFrom(type))
                    {
                        string controller = "";
                        string controllerName = "";
                        baseController = (ControllerBase)Activator.CreateInstance(type);
                        controllerName = baseController.GetType().Name;

                        if (controllerName.Contains("Controller"))
                        {
                            controller += "/" + controllerName.Replace("Controller", "").ToLower();
                        }
                        else
                        {
                            controller += "/" + controllerName.ToLower();
                        }

                        foreach (MethodInfo method in baseController.GetType().GetMethods())
                        {

                            URLPath += controller + "/" + method.Name.ToLower();
                            object[] attributes = method.GetCustomAttributes(true);

                            foreach (var attribute in attributes)
                            {
                                if (attribute.GetType().IsSubclassOf(typeof(HttpMethod)))
                                {
                                    string httpMethod = attribute.GetType().Name;
                                    httpMethod = httpMethod.ToUpper().Replace("HTTP", "");

                                    Routes route = new Routes(httpMethod, controllerName, method.Name, URLPath);
                                    Routes.Add(route);
                                    URLPath = "";
                                }

                            }
                        }

                    }

                }
            }
        }

        /// <summary>
        /// Execute action of Request. 
        /// </summary>
        /// <param name="ActionRequest">Dictionary: Request action to execute.</param>
        /// <returns>Object: Action with to precessed.</returns>
        public object ExecuteAction(Dictionary<string, object> RequestAction)
        {
            if (Routes == null)
            {
                ///TODO: resolver esto.
                return "500";
            }
            else
            {

                foreach (Routes route in Routes)
                {
                    if (route.UrlPath == RequestAction["URLPath"].ToString())
                    {
                        ControllerBase baseController = new ControllerBase();
                        DirectoryInfo directoryInfo = new DirectoryInfo(AppSite.physicalPath + "/bin/Debug");
                        FileInfo[] fileInfo = directoryInfo.GetFiles("*.dll");

                        foreach (FileInfo fi in fileInfo)
                        {
                            Assembly assembly = Assembly.LoadFrom(fi.FullName);

                            foreach (var type in assembly.GetTypes())
                            {
                                if (type != typeof(ControllerBase) && typeof(ControllerBase).IsAssignableFrom(type))
                                {
                                    baseController = (ControllerBase)Activator.CreateInstance(type);

                                    //for test.
                                    string urlPath = (string)RequestAction["URLPath"];
                                    string mehod = (string)RequestAction["HttpMethod"];
                                    NameValueCollection header = (NameValueCollection)RequestAction["Header"];
                                    NameValueCollection parameters = (NameValueCollection)RequestAction["Params"];
                                    Dictionary<string, HttpFile> files = (Dictionary<string, HttpFile>)RequestAction["Files"];

                                    baseController.Request = new Request(urlPath, mehod, header, parameters, files);

                                    baseController.route = route;
                                    baseController.httpContext.Site.physicalPath = AppSite.physicalPath;

                                    if (baseController.GetType().Name == route._controller)
                                    {
                                        MethodInfo method = baseController.GetType().GetMethod(route._action);
                                        AResult result = (AResult)method.Invoke(baseController, new object[] { });
                                        Headers = baseController.httpContext.Headers;
                                        return result;
                                    }

                                }
                            }
                        }

                        //  }
                    }

                }
            }


            return null;
        }

        /// <summary>
        /// Generate all Possibles URL defined on every class 
        /// that Inherited from the class: PHttpApplication.  
        /// </summary>
        /// <param name="site">Site: object site that contains all information. 
        /// about an active site.
        /// </param>
        public void Init(object site)
        {
            this.AppSite = (Site)site;

            GenerateAllRoutes();
        }

        /// <summary>
        /// this method register a Url path pattern. 
        /// </summary>
        /// <param name="pattern"></param>
        public void RegisterURLPatern(string pattern)
        {
            this.URLPattern = pattern;
        }



    }

}
