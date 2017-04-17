using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHttp.Application
{
    // public delegate string PreApplicationStartMethod(Type type, string method);
    //  public delegate string ApplicationStartMethod(Type type, string method);

    /// <summary>
    /// Contract for the class responsible for handling the loaded DLLs.
    /// </summary>
    public interface IPHttpApplication
    {

        //   event PreApplicationStartMethod PreApplicationStart;
        //   event ApplicationStartMethod ApplicationStart;

        void RegisterURLPatern(string pattern);

        /// <summary>
        /// Return the actual Site Instanced.
        /// </summary>
        /// <returns></returns>
        object GetSite();

        /// <summary>
        /// Start a specific App with site loaded.
        /// </summary>
        /// <param name="site">site: object that represent a site.</param>
        void Init(object site);

        /// <summary>
        /// Loads and combines all available routes according to the defined route pattern.
        /// </summary>
        void GenerateAllRoutes();

        /// <summary>
        /// Execute action of Request. 
        /// </summary>
        /// <param name="ActionRequest">Dictionary: Request action to execute.</param>
        /// <returns>Object: Action with to precessed.</returns>
        object ExecuteAction(Dictionary<string, object> ActionRequest);

    }
}
