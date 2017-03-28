using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public HttpServerState State { get { return _state; } set { if (value != _state) { HttpServerState previous = _state;  _state = value; OnStateChanged(new HttpEventArgs(previous, _state));} } }

        private TcpListener _listener;
        internal HttpTimeoutManager TimeoutManager { get; private set; }
        internal HttpServerUtility ServerUtility { get; private set; }
        private AutoResetEvent _clientsChangedEvent =  new AutoResetEvent(false);
        private bool _disposed;
        private HttpServerState _state = HttpServerState.Stopped;
        private Object _syncLock = new object();
        private Dictionary<HttpClient, bool> _clients = new Dictionary<HttpClient, bool>();
        #endregion properties

        #region Events
        public event EventHandler StateChanged;
        public event HttpRequestEventHandler RequestReceived;
        public event HttpExceptionEventHandler UnhandledException;
        #endregion Events

        public HttpServer(int port)
        {
            Port = port;
            //State = HttpServerState.Stopped;
            EndPoint = new IPEndPoint(IPAddress.Loopback, Port);
            ReadBufferSize = 4096;
            WriteBufferSize = 4096;
            ShutdownTimeout = TimeSpan.FromSeconds(30);
            ReadTimeout = TimeSpan.FromSeconds(90);
            WriteTimeout = TimeSpan.FromSeconds(90);
            ServerBanner = String.Format("PHttp/{0}", GetType().Assembly.GetName().Version);
        }

        public HttpServer() :
            this(8081)
        {

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
            try
            {
                VerifyState(HttpServerState.Started);
                State = HttpServerState.Stopping;
                _listener.Stop();
                StopClients();
            }
            catch(Exception e)
            {
                State = HttpServerState.Stopped;
                Console.WriteLine("Failed to stop HTTP server. | Exception: " + e.Message);
                throw new PHttpException("Failed to stop HTTP server.");
            }
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
            var shutdownStarted = DateTime.Now;
            bool forceShutdown = false;
            // Clients that are waiting for new requests are closed.

            List<HttpClient> clients;
            lock (_syncLock)
            {
                clients = new List<HttpClient>(_clients.Keys);
            }

            foreach (var client in clients)
            {
                client.RequestClose();
            }

            // First give all clients a chance to complete their running requests.
            while (true)
            {
                lock (_syncLock)
                {
                    if (_clients.Count == 0)
                        break;
                }

                var shutdownRunning = DateTime.Now - shutdownStarted;

                if (shutdownRunning >= ShutdownTimeout)
                {
                    forceShutdown = true;
                    break;
                }
                _clientsChangedEvent.WaitOne(ShutdownTimeout - shutdownRunning);
            }

            if (!forceShutdown)
                return;

            // If there are still clients running after the timeout, their
            // connections will be forcibly closed.
            lock (_syncLock)
            {
                clients = new List<HttpClient>(_clients.Keys);
            }

            foreach (var client in clients)
            {
                client.ForceClose();
            }

            // Wait for the registered clients to be cleared.
            while (true)
            {
                lock (_syncLock)
                {
                    if (_clients.Count == 0)
                        break;
                }
                _clientsChangedEvent.WaitOne();
            }
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
            try
            {
                var listener = _listener;
                if (listener == null)
                {
                    return;
                }
                var tcpClient = listener.EndAcceptTcpClient(asyncResult);
                if (_state != HttpServerState.Started)
                {
                    tcpClient.Close();
                    return;
                }
                HttpClient client = new HttpClient(this,tcpClient);
                RegisterClient(client);
                client.BeginRequest();
                BeginAcceptTcpClient();
            }
            catch (ObjectDisposedException ob)
            {
                //
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private void RegisterClient(HttpClient client)
        {
            if(client == null)
            {
                throw new NotImplementedException();
            }
            lock(_syncLock)
            {
                _clients.Add(client,true);
                _clientsChangedEvent.Set();
            }

        }
        internal void UnregisterClient(HttpClient client)
        {
            if(client == null)
            {
                throw new ArgumentNullException();
            }
            lock(_syncLock)
            {
                Debug.Assert(_clients.ContainsKey(client));
                _clients.Remove(client);
                _clientsChangedEvent.Set();
            }
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
                    _clientsChangedEvent.Close();
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

        internal void RaiseRequest(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            OnRequestReceived(new HttpRequestEventArgs(context));
        }

        internal bool RaiseUnhandledException(HttpContext context, Exception exception)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            var e = new HttpExceptionEventArgs(context, exception);
            OnUnhandledException(e);
            return e.Handled;
        }


        protected virtual void OnUnhandledException(HttpExceptionEventArgs e)
        {
            var ev = UnhandledException;
            if (ev != null)
                ev(this, e);
        }
        protected virtual void OnStateChanged(HttpEventArgs e)
        {
            var ev = StateChanged;
            if(ev != null)
            {
                ev(this, e);
            }
        }

        protected virtual void OnRequestReceived(HttpRequestEventArgs e)
        {
            var ev = RequestReceived;
            if(ev != null)
            {
                ev(this, e);
            }
        }
        #endregion methods
    }
}
