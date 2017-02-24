using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public event EventHandler ApplicationStart;
        public event EventHandler PreApplicationStart;

        public Delegate ApplicationStartMethod(Type type, string method)
        {
            throw new NotImplementedException();
        }

        public string ExecuteAction()
        {
            throw new NotImplementedException();
        }

        public Delegate PreApplicationStartMethod(Type type, string method)
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
