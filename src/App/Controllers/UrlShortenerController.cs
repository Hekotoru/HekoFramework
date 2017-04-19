using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mvc;

namespace App.Controllers
{
    class UrlShortenerController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public AResult GetUrl()
        {
            return ReturnJson("test",200);
        }


    }
}
