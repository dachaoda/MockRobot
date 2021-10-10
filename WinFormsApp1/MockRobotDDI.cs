using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MockRobotDDI
{

    public class MockBotDDI
    {

        private MockBotClient TcpClient;
        private Boolean isInitalized = false;

        public String OpenConnection(String IPAddress)
        {
            try
            {
                TcpClient = new MockBotClient(IPAddress);
                return String.Empty;

            }
            catch (SocketException e)
            {
                Debug.WriteLine("SocketException: [0]", e);
                return "Connection was not establish, " +
                       "MockBot is refusing connection attempts.";
            }
        }

        public async Task<String> Initialize()
        {
            try
            {
                while (true)
                {
                    Int32 processID;
                    Boolean wait = true;

                    if (TcpClient == null)
                    {
                        return "Connection was not estalish: " +
                               "Plese open connection first";
                    }

                    TcpClient.SendData("home%");

                    processID = TcpClient.ReceiveID();
                    if (processID == -1)
                    {
                        return "Another process is ongoing: " +
                               "Please try again later";
                    }

                    while (wait)
                    {
                        String status;
                        TcpClient.SendData("status%" + processID);
                        status = TcpClient.ReceiveData();
                        if (status == "In Progress")
                        {
                            await Task.Delay(30000);
                        }
                        else if (status == "Finished Successfully")
                        {
                            isInitalized = true;
                            return String.Empty;
                        }
                        else if (status == "Terminated With Error")
                        {
                            return "home process was terminated with error ";
                        }
                    }
                }
            }
            catch (SocketException e)
            {
                Debug.WriteLine("SocketException: [0]", e);
                return "An exception in connection has occured, " +
                       "please re-establish connection to MockBot.";
            }
            catch (IOException e)
            {
                return "Data transmition was unsuccesful. \n Exception:" + e;
            }
            catch (ObjectDisposedException e)
            {
                Debug.WriteLine("ObjectDisposedException: [0]", e);
                return "Unsuccessful initialization " +
                       "cause by closure of old connection, " +
                       "please establish a new connection";
            }

        }

        public String ExecuteOperation(
            String operation, 
            String[] parameterNames, 
            String[] parameterValues) 
        {
            try
            {
                if (TcpClient == null)
                {
                    return "Connection was not estalish: " +
                           "Plese open connection first";
                }

                if (!isInitalized)
                {
                    return "MockBot was not initialized: " +
                           "Please complete intialization before execution.";
                }



                if (operation == "pick")
                {

                    return PickAsync(
                        parameterNames[0], 
                        parameterValues[0]).Result;

                }
                else if (operation == "place")
                {

                    return PlaceAsync(
                        parameterNames[0], 
                        parameterValues[0]).Result;

                }
                else if (operation == "transfer")
                {

                    if (parameterNames[0] == "Source Location")
                    {

                        return Transfer(
                            parameterNames[0], parameterValues[0], 
                            parameterNames[1], parameterValues[1]);

                    }
                    else if (parameterNames[1] == "Source Location")
                    {

                        return Transfer(
                            parameterNames[1], parameterValues[1], 
                            parameterNames[0], parameterValues[0]);

                    }

                    return "Source Location for transder " +
                           "operation was not found, " +
                           "please double check input!";

                }

                return "No operation was recongized, " +
                       "please double check operation name!";

            }
            catch (SocketException e)
            {

                Debug.WriteLine("SocketException: [0]", e);
                return "An exception in connection has occured, " +
                       "please re-establish connection to MockBot.";

            }
            catch (IOException e)
            {

                Debug.WriteLine("IOException: [0]", e);
                return "Data transmition was unsuccesful, " +
                       "please double check connection to MockBot";

            }

        }

        public String Abort()
        {
            if (TcpClient != null)
            {
                TcpClient.CloseClient();
                isInitalized = false;
                return String.Empty;
            }
            return "No communcatiate was abort due to no active connection.";
        }

        private static Boolean DigitOnly(String str)
        {
            foreach (Char c in str)
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private async Task<String> PickAsync(
            String parameterNames, 
            String parameterValues)
        {
            try
            {

                Int32 processID;
                if (parameterNames != "Source Location")
                {

                    return "Source location for place operation " +
                           "has invalid parameter name";

                }

                if (!DigitOnly(parameterValues) || 
                    parameterValues.Equals(String.Empty))
                {

                    return "Parameter values for place operation is invalid.";

                }

                TcpClient.SendData("pick%" + parameterValues);

                processID = TcpClient.ReceiveID();
                if (processID == -1)
                {
                    return "Another process is ongoing: " +
                           "Please try again later";
                }

                while (true)
                {
                    String status;
                    TcpClient.SendData("status%" + processID);
                    status = TcpClient.ReceiveData();
                    if (status == "In Progress")
                    {
                        await Task.Delay(30000);
                    }
                    else if (status == "Finished Successfully")
                    {
                        return String.Empty;
                    }
                    else if (status == "Terminated With Error")
                    {
                        return "pick process was terminated with error ";
                    }
                }
            }
            catch (SocketException e)
            {

                Debug.WriteLine("SocketException: [0]", e);
                return "An exception in connection has occured, " +
                       "please re-establish connection to MockBot.";

            }
            catch (IOException e)
            {

                Debug.WriteLine("IOException: [0]", e);
                return "Data transmition was unsuccesful, " +
                       "please double check connection to MockBot";

            }
        }

        private async Task<String> PlaceAsync(
            String parameterNames, 
            String parameterValues)
        {
            try
            {
                Int32 processID;
                if (parameterNames != "Destination Location")
                {

                    return "Destination location for place operation " +
                           "has invalid parameter name";

                }

                if (!DigitOnly(parameterValues) || 
                    parameterValues.Equals(String.Empty))
                {

                    return "Parameter values for place operation is invalid.";

                }

                TcpClient.SendData("place%" + parameterValues);

                processID = TcpClient.ReceiveID();
                if (processID == -1)
                {

                    return "Another process is ongoing: " +
                           "Please try again later";

                }

                while (true)
                {
                    String status;
                    TcpClient.SendData("status%" + processID);
                    status = TcpClient.ReceiveData();
                    if (status == "In Progress")
                    {
                        await Task.Delay(30000);
                    }
                    else if (status == "Finished Successfully")
                    {
                        return String.Empty;
                    }
                    else if (status == "Terminated With Error")
                    {
                        return "place process was terminated with error";
                    }
                }
            }
            catch (SocketException e)
            {

                Debug.WriteLine("SocketException: [0]", e);
                return "An exception in connection has occured, " +
                       "please re-establish connection to MockBot.";

            }
            catch (IOException e)
            {

                Debug.WriteLine("IOException: [0]", e);
                return "Data transmition was unsuccesful, " +
                       "please double check connection to MockBot";

            }
        }

        private String Transfer(
            String SourceLocation, 
            String SourceValue, 
            String DestinationLocation, 
            String DestinationValue)
        {
            String message;

            message = PickAsync(SourceLocation, SourceValue).Result;
            if (message != String.Empty)
            {
                return message;
            }
            message = PlaceAsync(DestinationLocation, DestinationValue).Result;
            if (message != String.Empty)
            {
                return message;
            }

            return String.Empty;
        }
    }

    internal class MockBotClient
    {

        internal TcpClient client;
        private NetworkStream stream;

        internal MockBotClient(String server)
        {

            Int32 port = 1000;
            client = new TcpClient(server, port);

        }

        internal void SendData(String message)
        {

            stream = client.GetStream();
            Byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Sent: {0}", message);

        }

        internal String ReceiveData()
        {

            Byte[] data = new Byte[256];
            String responseData = String.Empty;
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);
            return responseData;

        }

        internal Int32 ReceiveID()
        {

            Byte[] data = new Byte[256];
            String responseData = String.Empty;
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);
            return Int32.Parse(responseData);


        }

        internal void CloseClient()
        {
            if (stream != null)
            {
                stream.Close();
            }
            if (client != null)
            {
                client.Close();
            }
        }

    }
}
