using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHttp
{
    public class PHttpSitesConfig
    {
      public List<string> defaultDocument { get; set; }
      public bool directoryBrowsing { get; set; }
      public List<Dictionary<string,string>> errorPages { get; set; } 
      public string name { get; set; }
      public string physicalPath { get; set; }
      public string virtualPath { get; set; }

        public PHttpSitesConfig()
        {
            defaultDocument = new List<string>();
            errorPages = new List<Dictionary<string, string>>();
        }
    }
}
