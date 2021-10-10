using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace MockRobotAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            MockRobot_Server();
        }


        public static void MockRobot_Server()
        {
            TcpListener server = null;

            try
            {
                // create tcp network connection setting   
                Int32 port = 1000;
                IPAddress localAddress = IPAddress.Parse("127.0.0.1");

                // create new tcp listener with local address and port at 1000
                server = new TcpListener(localAddress, port);

                // open the tcp server connection and begin listening for potential client
                server.Start();

                // buffer
                Byte[] bytes = new Byte[256];
                String data = null;

                // continue to listen for client
                while (true)
                {
                    Boolean reset = true;
                    Console.WriteLine("Awaiting connection...");

                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Client connected.");

                    data = null;

                    NetworkStream stream = client.GetStream();

                    int i;

                    while (reset)
                    {
                        // Connection testing
                        /*
                        String presult = Console.ReadLine();
                        if (presult == "5")
                        {
                            reset = false;
                        }
                        else if (presult == "6")
                        {

                            client.Close();
                            server.Stop();
                        }
                        Console.WriteLine("pending" + server.Pending());

                        */

                        if(server.Pending())
                        {
                            reset = false;
                        }

                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            // Translate data bytes to a ASCII string.
                            data = Encoding.ASCII.GetString(bytes, 0, i);
                            Console.WriteLine("Received: {0}", data);

                            // Process the data sent by the client.
                            Console.WriteLine("Awating Result");
                            String result = Console.ReadLine();
                            if (result == "1")
                            {
                                data = "In Progress";

                            }
                            else if(result == "2")
                            {
                                data = "Finished Successfully";
                            }
                            else if (result == "3")
                            {
                                data = "Terminated With Error";
                            }
                            else if (result == "4")
                            {
                                data = "-1";
                            }
                            else
                            {
                                data = result;
                            }
                            

                            Byte[] msg = Encoding.ASCII.GetBytes(data);

                            // Send back a response.
                            stream.Write(msg, 0, msg.Length);
                            Console.WriteLine("Sent: {0}", data);

                        }
                    }

                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
