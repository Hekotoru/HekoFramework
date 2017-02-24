using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHttp.Application;

namespace Mvc
{
    public class HekoTest : PHttp.Application.IPHttpApplication
    {
        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        event ApplicationStartMethod IPHttpApplication.ApplicationStart
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event PreApplicationStartMethod IPHttpApplication.PreApplicationStart
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public string ExecuteAction()
        {
            throw new NotImplementedException();
        }

        public string Start()
        {
            Console.WriteLine("Heko using reflection :D");
            return null;
        }

    }
}
