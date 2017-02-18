using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Sync_client
    {
        static void Main(string[] args)
        {
            byte[] bytes = new byte[1024];

            // connect to a Remote device 
            try
            {
                // Establish the remote end point for the socket 
                IPHostEntry ipHost = Dns.Resolve("45.55.77.201");
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 1337);

                Socket sender = new Socket(AddressFamily.InterNetwork,
                                           SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint 

                sender.Connect(ipEndPoint);

                Console.WriteLine("Socket connected to {0}",
                sender.RemoteEndPoint.ToString());

                string theMessage = "This is a test";

                byte[] msg = Encoding.ASCII.GetBytes(theMessage + "<theend>");

                // Send the data through the socket 
                int bytesSent = sender.Send(msg);

                // Receive the response from the remote device 
                int bytesRec = sender.Receive(bytes);

                Console.WriteLine("The Server says : {0}",
                                  Encoding.ASCII.GetString(bytes, 0, bytesRec));

                // Release the socket 
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                Console.In.ReadLine();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                Console.In.ReadLine();
            }
        }
    }
}
