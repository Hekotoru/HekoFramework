using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHttp
{
    public class PHttpServerConfig
    {
        public int port { get; set; }
        public List<Dictionary<string, string>> errorPages { get; set; }
        public List<string> defaultDocument { get; set; }
        public List<PHttpSitesConfig> sites { get; set;}

        /*
        public static PHttpServerConfig GetPHttpServerConfig()
        {
            PHttpServerConfig config = JsonConvert.DeserializeObject<PHttpServerConfig>(File.ReadAllText(@"C:\Users\Hector Aristy\Documents\GitHub\HekoFramework\src\PHttp\config.json"));
            return config;
        }
        */
    }
}
