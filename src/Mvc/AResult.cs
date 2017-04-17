using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc
{
    /// <summary>
    /// Interface that imitate an ActionResult from C#
    /// </summary>
    public interface AResult
    {
        int StatusCode();
        MemoryStream Content();
        string ContentType();
    }
}
