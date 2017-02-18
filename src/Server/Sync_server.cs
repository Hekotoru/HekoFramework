using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Sync_server
    {
        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.Resolve("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 8080);

            Socket sListener = new Socket(AddressFamily.InterNetwork,
                                          SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting for a connection on port {0}", ipEndPoint);

                    Socket handler = sListener.Accept();

                    string data = "";

                    while (!data.EndsWith("\r\n\r\n"))
                    {
                        byte[] bytes = new byte[1024];

                        int bytesRec = handler.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        if (data.IndexOf("<theend>") > -1)
                        {
                            break;
                        }
                    }

                    // show the data on the console 
                    Console.WriteLine("Text Received: {0}", data);


                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.In.ReadLine();
            }
        }
    }
}
