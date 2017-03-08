using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PHttp
{
    public class HttpServer : IDisposable
    {
        #region properties
        public IPEndPoint EndPoint { get; private set; }
        public int ReadBufferSize;
        public int WriteBufferSize;
        public string ServerBanner;
        public TimeSpan ReadTimeout;
        public TimeSpan WriteTimeout;
        public TimeSpan ShutdownTimeout;
        public int Port { get; private set; }
        public HttpServerState State { get { return _state; } set { if (value != _state) { _state = value; OnStateChanged(EventArgs.Empty); } } }

        private TcpListener _listener;
        internal HttpTimeoutManager TimeoutManager { get; private set; }
        internal HttpServerUtility ServerUtility { get; private set; }
        private AutoResetEvent _clientsChangedEvent = new AutoResetEvent(false);
        private bool _disposed;
        private HttpServerState _state = HttpServerState.Stopped;
        #endregion properties

        #region Events
        private event EventHandler StateChanged;
        #endregion Events

        public HttpServer(int port)
        {
            Port = port;
            State = HttpServerState.Stopped;
            EndPoint = new IPEndPoint(IPAddress.Loopback, Port);
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
            VerifyState(HttpServerState.Stopped);
            State = HttpServerState.Starting;
            Console.WriteLine("-- Server Starting at EndPoint {0}:{1}", EndPoint.Address, EndPoint.Port);
            TimeoutManager = new HttpTimeoutManager(this);
            var listener = new TcpListener(EndPoint);

            try
            {
                listener.Start();
                EndPoint = listener.LocalEndpoint as IPEndPoint;
                _listener = listener;
                ServerUtility = new HttpServerUtility();
                Console.WriteLine("-- Server Running at EndPoint {0}:{1}", EndPoint.Address, EndPoint.Port);
                State = HttpServerState.Started;
                BeginAcceptTcpClient();
            }
            catch (Exception e)
            {
                State = HttpServerState.Stopped;
                Console.WriteLine("** The Server failed to start. | Exception: " + e.Message);
                throw new PHttpException("The Server failed to start.");
            }
        }
        public void Stop()
        {
            throw new NotImplementedException();
        }
        private void VerifyState(HttpServerState state)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
            if (_state != state)
                throw new InvalidOperationException(String.Format("Expected server to be in the '{0}' state", state));
        }
        private void StopClients()
        {
            throw new NotImplementedException();
        }
        private void BeginAcceptTcpClient()
        {
            var listener = _listener;
            if (listener != null)
            {
                listener.BeginAcceptTcpClient(AcceptTcpClientCallback, null);
            }
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
            if(!_disposed)
            {
                if(State == HttpServerState.Started)
                {
                    Stop();
                }
                if(_clientsChangedEvent != null)
                {
                    _clientsChangedEvent.Dispose();
                    _clientsChangedEvent = null;
                }
                if(TimeoutManager != null)
                {
                    TimeoutManager.Dispose();
                    TimeoutManager = null;
                }
                _disposed = true;
            }
            //throw new NotImplementedException();
        }

        protected virtual void OnStateChanged(EventArgs e)
        {
            if(StateChanged != null)
            {
                StateChanged(this, e);
            }
        }
        #endregion methods
    }
}
