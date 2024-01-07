using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientApp
{
    public class SocketClient
    {
        
        
        // Client app is the one sending messages to a Server/listener.
        // Both listener and client can send messages back and forth once a
        // communication is established.
        public static void StartClient()
        {
            Byte[] bytes = new Byte[1024];

            try
            {

                // Connect to a Remote server
                // Get Host IP Address that is used to establish a connection
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1
                // If a host has multiple addresses, you will get a list of addresses
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, 3000);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {

                    //Connect to a remote Endpoint
                    sender.Connect(remoteEndPoint);

                    Console.WriteLine($"Socket connected to: {sender.RemoteEndPoint.ToString()}");

                    // Encode the data string into a byte array.
                    Byte[] msg = Encoding.ASCII.GetBytes("This is a test <EOF>");


                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.
                    int bytesReceived = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}", Encoding.ASCII.GetString(bytes, 0, bytesReceived));

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine($"Argument Null Exception: {ane.ToString()}");
                }
                catch (SocketException se)
                {
                    Console.WriteLine($"Socket Exception: {se.ToString()}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
