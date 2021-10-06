using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MockRobot_API
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }


        public static void MockRock_Server()
        {
            TcpListener server = null;

            try
            {
                // create tcp network connection setting   
                Int32 port = 1000;
                IPAddress localAddress = IPAddress.Parse("127.0.0.1");

                // create new tcp listener with local address and port at 1000
                server = new TcpListener(localAddress, port);

                //

                
            } 
            catch
            {

            }
        }   
    }
}
