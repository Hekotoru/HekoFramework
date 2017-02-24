using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PHttp
{
    class HttpServer : IDisposable
    {
        #region properties
        public IPEndPoint EndPoint;
        public int ReadBufferSize;
        public int WriteBufferSize;
        public string ServerBanner;
        public TimeSpan ReadTimeout;
        public TimeSpan WriteTimeout;
        public TimeSpan ShutdownTimeout;
        private HttpServerUtility ServerUtility;
        private HttpTimeoutManager TimeoutManager;
        #endregion properties

        public HttpServer()
        {
            EndPoint = new IPEndPoint(IPAddress.Loopback, 0);
            ReadBufferSize = 4096;
            WriteBufferSize = 4096;
            ShutdownTimeout = TimeSpan.FromSeconds(30);
            ReadTimeout = TimeSpan.FromSeconds(90);
            WriteTimeout = TimeSpan.FromSeconds(90);
            ServerBanner = String.Format("PHttp/{0}", GetType().Assembly.GetName().Version);
        }

        #region methods
        public void Start()
        {
            throw new NotImplementedException();
        }
        public void Stop()
        {
            throw new NotImplementedException();
        }
        private void VerifyState(HttpServerState state)
        {
            throw new NotImplementedException();
        }
        private void StopClients()
        {
            throw new NotImplementedException();
        }
        private void BeginAcceptTcpClient()
        {
            throw new NotImplementedException();
        }
        private void AcceptTcpClientCallback(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }
        private void RegisterClient(HttpClient client)
        {
            throw new NotImplementedException();
        }
        internal void UnregisterClient(HttpClient client)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion methods
    }
}
