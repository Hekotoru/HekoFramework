using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc
{
    public class AResult
    {
        public static  View ResultView()
        {
            return new View();
        }

        public static Content ResultContent()
        {
            return new Content();
        }

        public static Json ResultJson()
        {
            return new Json();
        }
    }
}
