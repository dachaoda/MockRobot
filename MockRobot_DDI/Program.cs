using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MockRobotDDI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var newBot = new MockBotDDI();
            //newBot.OpenConnection("127.0.0.1");
            newBot.Initialize();

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();


            //Connect("127.0.0.1", "hi I am here1");

        }

        static void Connect(String server, String message)
        {
            try
            {
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 1000;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }

    class MockBotDDI
    {

        public MockBotClient newCon;

        public String OpenConnection(String IPAddress)
        {
            //connect with IPAddress
            newCon = new MockBotClient(IPAddress);
            return String.Empty;
        }

        public String Initialize()
        {
            //send message Home
            String message;
            message = PreDataErr();
            if (message != "")
            {
                return message;
            }

            newCon.SendData("home%");
            

            return String.Empty;

        }

        public String ExecuteOperation
            (String operation, String[] parameterNames, String[] parameterValues)
        {

            try
            {
                if (operation == "pick")
                {
                    newCon.SendData("pick%");
                    return String.Empty;
                }
                else if (operation == "place")
                {
                    newCon.SendData("place%");
                }
                else if (operation == "transfer")
                {

                }

                throw new ArgumentException
                    ("No operation was recongized, please double check operation name!");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("ArgumentException: {0}", e);
                return "An exception in argument has occured";
            }

        }

        public String Abort() 
        {
            return String.Empty;
        }

        public String PreDataErr()
        {
            if (newCon == null)
            {
                return "Connection was not estalish, plese open connection first.";
            }

            return String.Empty;
        }

        public String PostDataErr()
        {

            if (newCon.ReceiveID() == -1)
            {

            }
            return String.Empty;
        }

    }

    class MockBotClient
    {

        public TcpClient client;
        public NetworkStream stream;

        public MockBotClient(String server)
        {
            try
            {
                Int32 port = 1000;
                client = new TcpClient(server, port);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public void SendData(String message)
        {
            try
            {
                stream = client.GetStream();
                Byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", message);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

        }

        public void ReceiveData()
        {
            try
            {
                Byte[] data = new Byte[256];
                String responseData = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public Int32 ReceiveID()
        {
            try
            {
                Byte[] data = new Byte[256];
                String responseData = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);
                return Int32.Parse(responseData);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                return -1;
            }
        }

        public void CloseClient()
        {
            stream.Close();
            client.Close();
        }
    }
}
