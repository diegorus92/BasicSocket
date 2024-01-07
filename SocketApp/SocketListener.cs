using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketApp
{
    public class SocketListener
    {


        public static void StartServer()
        {
            // Get Host IP Address that is used to establish a connection
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
            // If a host has multiple addresses, you will get a list of addresses
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress,3000);

            try
            {

                //Create Socket that will use TCP protocol
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // A Socket must be associated with an endpoint using the Bind method
                listener.Bind(localEndPoint);
                // Specify how many requests a Socket can listen before it gives Server busy response.
                // We will listen 10 requests at a time
                listener.Listen(10);

                Console.WriteLine("Wating for a connection...");
                Socket handler = listener.Accept();

                // Incoming data from the client.
                string data = null;
                Byte[] bytes = null;

                while (true)
                {
                    bytes = new Byte[1024];
                    int bytesReceived = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesReceived);

                    //If data recive a message, then break bucle
                    //Else, keep listening...
                    if(data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }

                Console.WriteLine($"Text recived:--> {data}");

                Byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
