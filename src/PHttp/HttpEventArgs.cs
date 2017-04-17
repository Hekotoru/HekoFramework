using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHttp
{
    public class HttpEventArgs : EventArgs
    {
        public HttpServerState _previousState { get; private set; }

       public HttpServerState _newState { get; private set; }

        public HttpEventArgs(HttpServerState previous, HttpServerState next) : base()
        {
            this._newState = next;
            this._previousState = previous;
        }
    }
}
